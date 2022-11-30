using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace LetsGoBikingRoutingLibrary
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Uri httpUrl = new Uri("http://localhost:8090/LetsGoBikingService");
            ServiceHost host = new ServiceHost(typeof(LetsGoBikingService), httpUrl);
            host.AddServiceEndpoint(typeof(ILetsGoBikingService), new WSHttpBinding(), "");

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            host.Description.Behaviors.Add(smb);

            host.Open();

            Console.WriteLine("The service is ready");
            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();

            // Close the ServiceHost.
            //host.Close();
        }
    }
}
