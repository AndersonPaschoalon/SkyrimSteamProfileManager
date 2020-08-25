using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utils;

namespace ProfileManager.Objects
{
    public class SPProfile
    {
        public SPProfile()
        {
            this.name = "";
            this.isActive = "TRUE";
        }

        //  name = {alphanumeric string}
        [XmlAttribute("name")]
        public string name;

        // isActive = {true/false}
        [XmlAttribute("isActive")]
        public string isActive
        {
            get 
            {
                return CSharp.boolToStr(this._isActive);
            }
            set
            {
                this._isActive = CSharp.strToBool(value);
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
