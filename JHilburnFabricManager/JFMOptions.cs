using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JHilburnFabricManager
{
    public class JFMOptions
    {
        public ApiProperties FabricApi { get; set; }

        public JFMOptions()
        {
            FabricApi = new ApiProperties();
        }
    }

    
}
