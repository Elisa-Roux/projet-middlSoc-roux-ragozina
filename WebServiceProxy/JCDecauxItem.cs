using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceProxy
{
    public class JCDecauxItem
    {
    }

    public class Contract
    {
        public string name { get; set; }

        public string commercial_name { get; set; }

        public string country_code { get; set; }

        public List<string> cities { get; set; }

    }

    public class Station
    {
        public int number { get; set; }
        public string name { get; set; }
        public Position position { get; set; }
    }

    public class Position
    {
        public Double latitude { get; set; }
        public Double longitude { get; set; }
    }

}
