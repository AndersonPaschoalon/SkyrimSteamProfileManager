using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProfileManager.Objects
{
    public class SPProfileList
    {
        public SPProfileList()
        {
            this.profiles = new List<SPProfile>();
        }

        [XmlElement("PROFILE")]
        public List<SPProfile> profiles { get; set; }

    }
}
