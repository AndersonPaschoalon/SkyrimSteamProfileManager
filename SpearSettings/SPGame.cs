using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpearSettings
{
    public class SPGame
    {
        [XmlAttribute("game")]
        public string game { get; set; }

        [XmlAttribute("gameExe")]
        public string gameExe { get; set; }

        [XmlAttribute("gameFolder")]
        public string gameFolder { get; set; }

        [XmlAttribute("backupFolder")]
        public string backupFolder { get; set; }

    }
}
