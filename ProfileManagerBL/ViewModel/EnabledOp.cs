using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManagerBL.ViewModel
{
    public class EnabledOp
    {
        public  EnabledOp()
        {
            // profiles
            this.editProfile = false;
            this.desactivateProfile = false;
            this.activateProfile = false;
            this.switchProfile = false;
            this.reloadProfile = false;

            // git tools 
            this.createGitignore = false;
            this.openGititnore = false;
            this.deleteGitignore = false;

            // tools helper
            this.killSteamApp = false;
            this.openGameFolder = false;

            // tools launch
            this.launchGame = false;
            this.launchNMM = false;
            this.launchVortex = false;

            // skyrim tools
            this.skyrimOpenLogs = false;
            this.skyrimCleanLogs = false;
            this.skyrimLaunchCreationKit = false;
            this.skyrimLaunchTESVEdit = false;
        }

        // profiles
        public  bool editProfile { get; set; }
        public  bool activateProfile { get; set; }
        public  bool desactivateProfile { get; set; }
        public  bool switchProfile { get; set; }
        public bool reloadProfile { get; set; }

        // git tools
        public bool createGitignore { get; set; }
        public bool openGititnore { get; set; }
        public bool deleteGitignore { get; set; }

        // tools helpers
        public bool killSteamApp { get; set; }
        public bool openGameFolder { get; set; }

        // tools launch 
        public bool launchGame { get; set; }
        public bool launchNMM { get; set; }
        public bool launchVortex { get; set; }

        // skyrim tools
        public bool skyrimOpenLogs { get; set; }
        public bool skyrimCleanLogs { get; set; }
        public bool skyrimLaunchCreationKit { get; set; }
        public bool skyrimLaunchTESVEdit { get; set; }

    }
}
