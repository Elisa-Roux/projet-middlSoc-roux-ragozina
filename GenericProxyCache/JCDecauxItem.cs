
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;

using System.Text.Json;
using System.Threading.Tasks;

namespace GenericProxyCache
{
    public class JCDecauxItem
    {
        static HttpClient httpClient = new HttpClient();
        static string apiKey = "64346e2dbae9c6364bd75b2ee766c8e33623b537";
        static string apiKeyEntry = "&apiKey=" + apiKey;
        static Task<string> response;

        public static List<Contract> GetAllContracts()
        {
            response = httpClient.GetStringAsync("https://api.jcdecaux.com/vls/v1/contracts?apiKey=" + apiKey);
            response.Wait();
            List<Contract> contracts = JsonConvert.DeserializeObject<List<Contract>>(response.Result);
            List<Contract> finalContracts = new List<Contract>();
            foreach (Contract item in contracts)
            {
                if (item.name != "lund" && item.name != "stockholm" && item.name != "vilnius" && item.name != "toyama" && item.name != "brisbane" && item.name != "jcdecauxbike")
                    finalContracts.Add(item);
            }

            return finalContracts;
        }

        public static List<Station> GetStationsFromContractWithBikes(string contract)
        {
            string url, query, response;
            url = "https://api.jcdecaux.com/vls/v3/stations";
            query = "contract=" + contract + "&apiKey=" + apiKey;
            response = JCDecauxAPICall(url, query).Result;
            List<Station> stations = System.Text.Json.JsonSerializer.Deserialize<List<Station>>(response);

            List<Station> stationsWithBikes = new List<Station>();
            foreach (Station station in stations)
            {
                if (station.totalStands != null && station.totalStands.availabilities.bikes > 0)
                {
                    stationsWithBikes.Add(station);
                    Position tmp = station.position;
                    station.position = new Position() { latitude = tmp.longitude, longitude = tmp.latitude };
                }
            }

            return stationsWithBikes;
        }

        public static List<Station> GetStationsFromContractWithStands(string contract)
        {
            string url, query, response;
            url = "https://api.jcdecaux.com/vls/v3/stations";
            query = "contract=" + contract + "&apiKey=" + apiKey;
            response = JCDecauxAPICall(url, query).Result;
            List<Station> stations = System.Text.Json.JsonSerializer.Deserialize<List<Station>>(response);

            List<Station> stationsWithBikes = new List<Station>();
            foreach (Station station in stations)
            {
                if (station.totalStands != null && station.totalStands.availabilities.stands > 0)
                {
                    stationsWithBikes.Add(station);
                    Position tmp = station.position;
                    station.position = new Position() { latitude = tmp.longitude, longitude = tmp.latitude };
                }
            }

            return stationsWithBikes;
        }

        static async Task<string> JCDecauxAPICall(string url, string query)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url + "?" + query);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }

    public class Station
    {
        public Station() { }
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string status { get; set; }
        public TotalStands totalStands { get; set; }
        public Position position { get; set; }

        public override string ToString()
        {
            return "Name: " + name + "\n" + "Number: " + number + "\n";
        }
    }

    public class Contract
    {
        public Contract() { }
        public string name { get; set; }
        public override string ToString()
        {
            return "Name: " + name + "\n";
        }
    }

    public class Position
    {
        public Position() { }
        public float latitude { get; set; }
        public float longitude { get; set; }

        public override string ToString()
        {
            return "lat: " + latitude + " lon: " + longitude + "\n";
        }
    }

    public class TotalStands
    {
        public TotalStands() { }
        public Availabilities availabilities { get; set; }

    }

    public class Availabilities
    {
        public Availabilities() { }
        public int stands { get; set; }

        public int bikes { get; set; }
    }
}
