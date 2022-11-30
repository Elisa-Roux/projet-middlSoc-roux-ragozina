using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.ServiceReference1;

namespace TestClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LetsGoBikingServiceClient client = new LetsGoBikingServiceClient();
           
            Console.WriteLine("hello");
            // Utilisez la variable 'client' pour appeler des opérations sur le service.
            client.GetItinerary("2400 route des dolines","toulouse");
            Console.WriteLine("here are the instructions");
           
            // Fermez toujours le client.
            //client.Close();
        }
    }

   

}
