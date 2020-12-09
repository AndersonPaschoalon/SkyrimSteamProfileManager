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
            this.steam = "";
            this.docs = "";
            this.appData = "";
            this.nmmInfo = "";
            this.nmmMod = "";
        }

        public string steam { get; set; }
        public string docs { get; set; }
        public string appData { get; set; }
        public string nmmInfo { get; set; }
        public string nmmMod { get; set; }
    }
}
