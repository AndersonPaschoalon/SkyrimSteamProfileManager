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
            this.editProfile = false;
            this.desactivateProfile = false;
            this.activateProfile = false;
            this.switchProfile = false;
            this.reloadProfile = false;
            this.createGitignore = false;
            this.deleteGitignore = false;

        }

        public  bool editProfile { get; set; }
        public  bool activateProfile { get; set; }
        public  bool desactivateProfile { get; set; }
        public  bool switchProfile { get; set; }
        public bool reloadProfile { get; set; }
        public bool createGitignore { get; set; }
        public bool deleteGitignore { get; set; }
    }
}
