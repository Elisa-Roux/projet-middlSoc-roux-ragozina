using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JCDecauxProxy
{
    [ServiceContract]
    public interface IJCDecauxService
    {
        [OperationContract]
        List<Contract> GetAllContracts();

        [OperationContract]
        List<Station> GetStationsFromContractWithBikes(string contract);

        [OperationContract]
        List<Station> GetStationsFromContractWithStands(string contract);

    }
}
