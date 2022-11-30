using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace GenericProxyCache
{

    [ServiceContract]
    public interface IGenericProxyCache
    {
        [OperationContract]
        List<Contract> GetAllContracts();

        [OperationContract]
        List<Station> GetStationsFromContractWithBikes(string contract);

        [OperationContract]
        List<Station> GetStationsFromContractWithStands(string contract);
    }
}
