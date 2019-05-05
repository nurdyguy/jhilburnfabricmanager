using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Models
{
    public class Fabric
    {
        public int id { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public bool active { get; set; }
        public string category { get; set; }
        public string imgUrl { get; set; }
        public int inventory { get; set; }
    }
}
