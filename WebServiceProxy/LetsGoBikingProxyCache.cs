using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceProxy
{
    public class LetsGoBikingProxyCache
    {
        ObjectCache cache;

        public LetsGoBikingProxyCache()
        {
            this.cache = MemoryCache.Default;
        }

        public Contract GetContract(Position location)
        {
            return null;

        }

        public List<Station> GetStationsFromContract() { return null; }

        public List<Station> GetAllStations() {  return null; }

        public List<Station> GetStationsWithAvailableStands() { return null; }

    }
}
