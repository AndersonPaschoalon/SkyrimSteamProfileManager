using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManagerBL.ViewModel
{
    public class SettingsViewData
    {
        public SettingsViewData()
        {
            this.nmm = "";
            this.vortex = "";
            this.tesvedit = "";            
        }

        public string nmm { get; set; }
        public string vortex { get; set; }
        public string tesvedit { get; set; }
    }
}
