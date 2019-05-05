using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Models
{
    public class FabricListResponseModel
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }

        public IEnumerable<Fabric> Fabrics { get; set; }
    }
}
