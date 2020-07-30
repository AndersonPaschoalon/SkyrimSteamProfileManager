using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteamProfileManager.Objects
{
    public class SPProfile
    {
        public SPProfile()
        {
            this.id = 0;
            this.name = "";
            this.isActive = "TRUE";
        }

        // id={unique int number}
        [XmlAttribute("id")]
        public int id;

        //  name={alphanumeric string}
        [XmlAttribute("name")]
        public string name;

        // isActive={true/false}
        [XmlAttribute("isActive")]
        public string isActive
        {
            get 
            {
                return Helper.boolToStr(this._isActive);
            }
            set
            {
                this._isActive = Helper.strToBool(value);
            }
        }

        [XmlAttribute("color")]
        public string color;

        public bool isProfileActive()
        {
            return this._isActive;
        }
     
        private bool _isActive = false;

    }
}
