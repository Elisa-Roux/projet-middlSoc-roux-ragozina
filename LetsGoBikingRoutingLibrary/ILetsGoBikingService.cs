using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static LetsGoBikingRoutingLibrary.LetsGoBikingService;

namespace LetsGoBikingRoutingLibrary
{
    [ServiceContract]
    public interface ILetsGoBikingService
    {
        [OperationContract]
        String GetItinerary(string origin, string destination);
    }
}
