using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager.Services.Models
{
    public class ApiPagedResponse<T>
    {
        public int Count { get; set; }
        public int Page { get; set; }
        public int PerPage { get; set; }

        public IEnumerable<T> Content { get; set; }
    }
}
