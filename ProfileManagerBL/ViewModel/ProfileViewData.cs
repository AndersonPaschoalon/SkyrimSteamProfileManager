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

    }
}
