using Elasticsearch.Net;
using LetsGoBikingRoutingLibrary.ServiceReference1;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml;
using static System.Collections.Specialized.BitVector32;
using Contract = LetsGoBikingRoutingLibrary.ServiceReference1.Contract;
using GeoCoordinate = System.Device.Location.GeoCoordinate;


namespace LetsGoBikingRoutingLibrary
{

    public class LetsGoBikingService : ILetsGoBikingService
    {
        string url, query;
        string apiKeyORS = "5b3ce3597851110001cf6248ab3635969dad46d9bb5c00e367213fe0"; // "5b3ce3597851110001cf6248d4da20c285e74bb8ac6ec42c0ba2ea00";
        //track current execution step
        enum ExecutionStep {
                Searching = 0,
                ReadyToPrint = 1,
                Idle = 2
            }
        string rep = String.Empty;
        string itineraryStep = String.Empty;
        ExecutionStep currentStep = ExecutionStep.Idle;

       GenericProxyCacheClient jcd = new GenericProxyCacheClient();
        public string GetItinerary(string originAddress, string destinationAddress)
        {
            //initialize the producer queue connection
            ActiveMQProducer.activeMQInitialize();

            currentStep = ExecutionStep.Searching;

            Position origin = GetCoordinatesFromAddress(originAddress);
            Position destination = GetCoordinatesFromAddress(destinationAddress);

            String originCity = GetCityFromAddress(originAddress);
            String destinationCity = GetCityFromAddress(destinationAddress);

            Position originCityCoordinates = GetCoordinatesFromAddress(originCity);
            Position destinationCityCoordinates = GetCoordinatesFromAddress(destinationCity);

            ActiveMQProducer.activeMQSendMessage("Searching for the nearest bike stations...");

            String closestOriginContract = GetClosestContract(originCityCoordinates);
            String closestDestinationContract = GetClosestContract(destinationCityCoordinates);

            ActiveMQProducer.activeMQSendMessage("Checking bike availability...");

            Station originStation = GetClosestStationWithBikes(closestOriginContract, origin);
            Station destinationStation = GetClosestStationWithStands(closestDestinationContract, origin);

            ActiveMQProducer.activeMQSendMessage("Getting directions...");

            List<String> originToStation1 = GetInstructions(origin, originStation.position, "foot-walking");
            List<String> Station1ToStation2 = GetInstructions(originStation.position, destinationStation.position, "cycling-regular");
            List<String> Station2ToDestination = GetInstructions(destination, destinationStation.position, "foot-walking");

            currentStep = ExecutionStep.ReadyToPrint;
            Thread.Sleep(1000);

            if (currentStep == ExecutionStep.ReadyToPrint)
            {
                rep = "\nYou want to go from " + originAddress + " to " + destinationAddress + ". Here is what you should do :\n\n";
                String transportType = "";
                if (GetTotalDuration(origin, originStation.position, destinationStation.position, destination) < GetDuration(origin, destination, "foot-walking"))
                {
                    rep += "\n\n---------------  Walk to " + originStation.name + " in " + closestOriginContract.ToString() + "---------------------- \n\n";
                    itineraryStep += rep; ActiveMQProducer.activeMQSendMessage(itineraryStep);

                    foreach (String s in originToStation1)
                    {
                        updateMessage(s);
                    };

                    transportType = "\n\n--------------- Then take a bike to " + destinationStation.name + " in " + closestDestinationContract.ToString() + "------------------\n\n";
                    rep += transportType; itineraryStep += transportType; ActiveMQProducer.activeMQSendMessage(itineraryStep);

                    foreach (String s in Station1ToStation2)
                    {
                        updateMessage(s);
                    };

                    transportType = "\n\n--------------- Finally walk to " + destination + "----------------------\n\n";
                    rep += transportType; itineraryStep += transportType; ActiveMQProducer.activeMQSendMessage(itineraryStep);

                    foreach (String s in Station2ToDestination)
                    {
                        updateMessage(s);
                    };
                }
                else
                {
                    rep += "\n\n-------------------- Walk from " + originAddress + " to " + destinationAddress + "----------------------- \n\n";
                    itineraryStep += rep; ActiveMQProducer.activeMQSendMessage(itineraryStep);
                    List<String> originToDest = GetInstructions(origin, destination, "foot-walking");
                    foreach (String s in originToDest)
                    {
                        updateMessage(s);
                    };
                }
            }
            ActiveMQProducer.activeMQSendMessage("STOP");
            currentStep = ExecutionStep.Idle;
            ActiveMQProducer.activeMQClose();
            return rep;

        }

        public dynamic GetAddressInfos(string address)
        {
            url = "https://api.openrouteservice.org/geocode/search";
            query = "api_key=" + apiKeyORS + "&text=" + address;

            return MakeRequest(url, "?", query);
        }

        public String GetCityFromAddress(string address)
        {
            dynamic addressInfos = GetAddressInfos(address);
            String city = addressInfos["features"][0]["properties"]["locality"];

            return city;
        }

        public Position GetCoordinatesFromAddress(string address)
        {
            dynamic addressInfos = GetAddressInfos(address);
            float addressLat = (float) addressInfos["features"][0]["geometry"]["coordinates"][0];
            float addressLon = (float) addressInfos["features"][0]["geometry"]["coordinates"][1];
            
            return new Position() { latitude = addressLat, longitude = addressLon };  
        }

        public dynamic GetDirections(Position origin, Position destination, string transport)
        {
            url = "https://api.openrouteservice.org/v2/directions/"+transport ;
            query = "api_key="+apiKeyORS+"&start="+origin.latitude.ToString("G", CultureInfo.InvariantCulture)+ ","+origin.longitude.ToString("G", CultureInfo.InvariantCulture) + "&end="+ destination.latitude.ToString("G", CultureInfo.InvariantCulture) + ","+destination.longitude.ToString("G", CultureInfo.InvariantCulture);
            return MakeRequest(url, "?", query);
        }

        public decimal GetDistance(Position origin, Position destination, string transport)
        {
            dynamic directions = GetDirections(origin, destination, transport);
            return directions["features"][0]["properties"]["segments"][0]["distance"];
        }

        public decimal GetDuration(Position origin, Position destination, string transport)
        {
            dynamic directions = GetDirections(origin, destination, transport);
            return directions["features"][0]["properties"]["segments"][0]["duration"];
        }

        public decimal GetTotalDuration(Position origin, Position firstStation, Position secondStation, Position destination)
        {
            return GetDuration(origin, firstStation, "foot-walking") + GetDuration(firstStation, secondStation, "cycling-regular") + GetDuration(secondStation, destination, "foot-walking");
        }

        public String GetClosestContract(Position current)
        {

            Contract[] allContracts = jcd.GetAllContracts();
            GeoCoordinate currentPosition = new GeoCoordinate(current.latitude,current.longitude);

            Position contractPos1 = GetCoordinatesFromAddress(allContracts[0].name);
            GeoCoordinate contractPosition1 = new GeoCoordinate(contractPos1.latitude, contractPos1.longitude);

            double distance = currentPosition.GetDistanceTo(contractPosition1);
            string closest = "";

            for(int i = 1; i < allContracts.Length; i++)
            {
                Position contractPos = GetCoordinatesFromAddress(allContracts[i].name);
                GeoCoordinate contractPosition = new GeoCoordinate(contractPos.latitude, contractPos.longitude);

                if (currentPosition.GetDistanceTo(contractPosition) < distance)
                {
                    distance = currentPosition.GetDistanceTo(contractPosition);
                    closest = allContracts[i].name;
                }
            }
            return closest;

        }

        public Station GetClosestStationWithBikes(string contract, Position current)
        {
            Station[] allStations = jcd.GetStationsFromContractWithBikes(contract);
            GeoCoordinate currentPosition = new GeoCoordinate(current.latitude, current.longitude);

            Position stationPos1 = allStations[0].position;
            GeoCoordinate stationPosition1 = new GeoCoordinate(stationPos1.latitude, stationPos1.longitude);

            double distance = currentPosition.GetDistanceTo(stationPosition1);
            Station closest = new Station();

            for (int i = 1; i < allStations.Length; i++)
            {
                Position stationPos = allStations[i].position;
                GeoCoordinate contractPosition = new GeoCoordinate(stationPos.latitude, stationPos.longitude);

                if (currentPosition.GetDistanceTo(contractPosition) < distance)
                {
                    distance = currentPosition.GetDistanceTo(contractPosition);
                    closest = allStations[i];
                }
            }
            return closest;
        }

        public Station GetClosestStationWithStands(string contract, Position current)
        {
            Station[] allStations = jcd.GetStationsFromContractWithStands(contract);
            GeoCoordinate currentPosition = new GeoCoordinate(current.latitude, current.longitude);

            Position stationPos1 = allStations[0].position;
            GeoCoordinate stationPosition1 = new GeoCoordinate(stationPos1.latitude, stationPos1.longitude);

            double distance = currentPosition.GetDistanceTo(stationPosition1);
            Station closest = new Station();

            for (int i = 1; i < allStations.Length; i++)
            {
                Position stationPos = allStations[i].position;
                GeoCoordinate contractPosition = new GeoCoordinate(stationPos.latitude, stationPos.longitude);

                if (currentPosition.GetDistanceTo(contractPosition) < distance)
                {
                    distance = currentPosition.GetDistanceTo(contractPosition);
                    closest = allStations[i];
                }
            }
            return closest;
        }

        public List<String> GetInstructions(Position origin, Position destination, string transport)
        {
            dynamic directions = GetDirections(origin, destination, transport);  
            List<object> steps = new List<object>(directions["features"][0]["properties"]["segments"][0]["steps"]);
            //latestPosition = directions["features"][0]["properties"]["segments"][0]["steps"]["name"];
            List<string> ret = new List<string>();

            foreach (Dictionary<string, object> obj in steps)
            {
                ret.Add(obj["instruction"].ToString() + " in " + obj["distance"] + " meters");
            }

            return ret;
        }

        static async Task<string> ProcessAsync(string url, string separator, string query)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url + separator + query);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public dynamic MakeRequest(string url, string separator, string query)
        {
            string response = ProcessAsync(url, separator, query).Result;

            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            dynamic obj = jsonSerializer.Deserialize<dynamic>(response);

            return obj;
        }

        public void updateMessage(String s)
        {
            rep += s + "\n";
            itineraryStep = s + "\n";
            ActiveMQProducer.activeMQSendMessage(itineraryStep);
            Thread.Sleep(1000);
            itineraryStep = String.Empty;
        }
    }

}

