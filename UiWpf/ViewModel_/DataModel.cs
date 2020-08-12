using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfileManager;
using ProfileManager.Objects;

namespace UiWpf.ViewModel
{
    public class DataModel
    {

        public DataModel()
        {
            this.nmmInfoPath = "";
            this.nmmModPath = "";
            this.steamPath = "";
            this.appDirPath = "";
            this.docsPath = "";
            this.state = ProfileManager.Enum.SPMState.NO_PROFILE;
            this.activeProf = null;
            this.desactivatedProf = new List<SPProfile>();
        }
        public string selectedGame { get; set; }
        public string steamPath { get; set;}
        public string appDirPath { get; set; }
        public string docsPath { get; set; }
        public string nmmInfoPath { get; set; }
        public string nmmModPath { get; set; }
        public ProfileManager.Enum.SPMState state { get; set; }
        public SPProfile activeProf { get; set; }
        public List<SPProfile> desactivatedProf { get; set; }
    }
}
