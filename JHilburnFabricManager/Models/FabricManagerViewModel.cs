using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Models
{
    public class FabricManagerViewModel
    {
        public IEnumerable<Fabric> Fabrics { get; set; }
        
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int TotalCount { get; set; }

        public string SortBy { get; set; }
        public string SortDir { get; set; }
    }
}
