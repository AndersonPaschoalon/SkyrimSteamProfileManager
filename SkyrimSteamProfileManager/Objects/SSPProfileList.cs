using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkyrimSteamProfileManager.Objects
{
    public class SSPProfileList
    {
        public SSPProfileList()
        {
            this.profiles = new List<SSPProfile>();
        }

        [XmlElement("PROFILE")]
        public List<SSPProfile> profiles { get; set; }



    }
}
