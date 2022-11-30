using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JCDecauxProxy
{
    public class Program
    {
        static void Main(string[] args)
        {
            Uri httpUrl = new Uri("http://localhost:8090/JCDecauxProxy");
            ServiceHost host = new ServiceHost(typeof(JCDecauxService), httpUrl);
            host.AddServiceEndpoint(typeof(IJCDecauxService), new WSHttpBinding(), "");

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
