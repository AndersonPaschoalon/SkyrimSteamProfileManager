using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManagerBL.ViewModel
{
    public class EnabledOp
    {
        private static EnabledOp instance;
        private EnabledOp()
        { 
        }

        public static EnabledOp getInstance()
        {
            if (EnabledOp.instance == null)
            {
                EnabledOp.instance = new EnabledOp();
            }
            return EnabledOp.instance;
        }

        public  bool editProfile { get; set; }
        public  bool activateProfile { get; set; }
        public  bool desactivateProfile { get; set; }
        public  bool switchProfile { get; set; }
    }
}
