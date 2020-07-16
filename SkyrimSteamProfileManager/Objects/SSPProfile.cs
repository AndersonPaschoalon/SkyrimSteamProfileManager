using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkyrimSteamProfileManager.Objects
{
    public class SSPProfile
    {
        public enum State
        {
            EXISTS_INACTIVE,
            EXISTS_ACTIVE,
            NOT_EXISTS,
            CORRUPTED
        }

        public SSPProfile()
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
                return (this._isActive)? "TRUE" : "FALSE";
            }
            set
            {
                if (value.ToUpper() == "FALSE" || value == "0" || value.Trim().Equals(""))
                {
                    this._isActive = false;
                }
                else
                {
                    this._isActive = true;
                }
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
