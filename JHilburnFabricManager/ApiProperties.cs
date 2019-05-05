using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager
{
    public class ApiProperties
    {
        public string BaseUrl { get; set; }
        public Dictionary<string, string> Tokens { get; set; }

        public ApiProperties()
        {
            Tokens = new Dictionary<string, string>();
        }
    }
}
