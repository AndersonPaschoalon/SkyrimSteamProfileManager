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
            this.nmmPath = "";
            this.vortexPath = "";
            this.nmmGameFolder = "";
            this.vortexGameFolder = "";
            this.nmmExe = "";
            this.vortexExe = "";
            this.tesveditExe = "";
        }

        // paths
        public string nmmPath { get; set; }
        public string vortexPath { get; set; }
        public string nmmGameFolder { get; set; }
        public string vortexGameFolder { get; set; }
        // tools
        public string nmmExe { get; set; }
        public string vortexExe { get; set; }
        public string tesveditExe { get; set; }
    }
}
