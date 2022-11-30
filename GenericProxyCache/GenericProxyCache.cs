using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace GenericProxyCache
{
    public class GenericProxyCache : IGenericProxyCache
    {

        private ObjectCache cache = MemoryCache.Default;
        private CacheItemPolicy policy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(3),
        };
        public GenericProxyCache()
        {
            
        }

        public List<Contract> GetAllContracts()
        {
            if(cache.Get("AllContracts") == null)
            {
                cache.Add("AllContracts", JCDecauxItem.GetAllContracts(), ObjectCache.InfiniteAbsoluteExpiration);
            }
            return (List<Contract>) cache.Get("AllContracts");
        }

        public List<Station> GetStationsFromContractWithBikes(string contract)
        {
            if (cache.Get("AllStationsWithBikes") == null)
            {
                cache.Add("AllStationsWithBikes", JCDecauxItem.GetStationsFromContractWithBikes(contract), policy);
            }
            return (List<Station>)cache.Get("AllStationsWithBikes");
        }

        public List<Station> GetStationsFromContractWithStands(string contract)
        {
            if (cache.Get("AllStationsWithStands") == null)
            {
                cache.Add("AllStationsWithStands", JCDecauxItem.GetStationsFromContractWithStands(contract), policy);
            }
            return (List<Station>)cache.Get("AllStationsWithStands");
        }


    }
}
