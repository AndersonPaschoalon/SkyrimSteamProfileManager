using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProfileManager.Objects;

namespace ProfileManagerBL.ViewModel
{
    public class ProfileViewData
    {
        public ProfileViewData()
        {
        }

        public ProfileViewData(SPProfile prof, ProfileType profType)
        {
            this.name = prof.name;
            this.color = Color.FromName("Blue");
            this.creatingDate = prof.creationDate;
            this.state = profType;
        }

        public bool isChecked { get; set; }
        public ProfileType state{ get; set; }

        public string name { get; set; }

        public Color color{get; set;}

        public string colorHex { 
            get
            {
                return Utils.CSharp.drawingColorToHex(this.color);
            }
            set 
            {
                try
                {
                    this.color = ColorTranslator.FromHtml(value);
                }
                catch (Exception ex)
                {
                    this.color = Color.FromName("Blue");
                }
                
            } 
        }


        public string creatingDate { get; set; }

    }
}
