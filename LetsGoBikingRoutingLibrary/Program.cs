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

            ServiceHost host = new ServiceHost(typeof(LetsGoBikingService));

            host.Open();

            Console.WriteLine("The service is ready");
            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();

            // Close the ServiceHost.
            //host.Close();
        }
    }
}
