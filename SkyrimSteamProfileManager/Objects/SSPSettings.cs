using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkyrimSteamProfileManager.Objects
{
    public class SSPSettings
    {

        [XmlAttribute("steamPath")]
        public string steamPath { get; set; }


        [XmlAttribute("documentsPath")]
        public string documentsPath { get; set; }


        [XmlAttribute("appDataPath")]
        public string appDataPath { get; set; }


        [XmlAttribute("nmmModPath")]
        public string nmmModPath { get; set; }


        [XmlAttribute("nmmInfoPath")]
        public string nmmInfoPath { get; set; }

        [XmlAttribute("loglevel")]
        public string loglevel { get; set; }

        [XmlAttribute("gameFolder")]
        public string gameFolder { get; set; }

    }
}
