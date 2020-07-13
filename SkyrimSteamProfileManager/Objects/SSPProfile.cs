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
        public SSPProfile()
        { 
        }

        // id={unique int number}
        [XmlAttribute("id")]
        public string id;

        //  name={alphanumeric string}
        [XmlAttribute("name")]
        public string name;

        // isActive={true/false}
        [XmlAttribute("isActive")]
        public string isActive;

        [XmlAttribute("color")]
        public string color;

    }
}
