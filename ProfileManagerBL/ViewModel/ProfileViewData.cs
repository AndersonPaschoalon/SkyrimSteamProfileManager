using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManagerBL.ViewModel
{
    public class ProfileViewData
    {
        public bool isChecked { get; set; }
        public ProfileType state{ get; set; }

        public string name { get; set; }

        public Color color{get; set;}

        // TODO INTEGRAR ESSES ELEMENTOS
        public string colorHex { get; set; }
        public string creatingDate { get; set; }

        public static ProfileViewData getInactive()
        {
            Color darkGray = Color.FromName("DarkGray");
            ProfileViewData inactiveProfileView = new ProfileViewData();
            inactiveProfileView.name = "~INACTIVE";
            inactiveProfileView.color = darkGray;
            inactiveProfileView.state = ProfileType.INACTIVE;
            return inactiveProfileView;
        }

    }
}
