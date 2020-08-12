using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProfileManager.Objects
{
    public class SPSettings
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

        [XmlAttribute("gameFolder")]
        public string gameFolder { get; set; }

        [XmlAttribute("backupFolder")]
        public string backupFolder { get; set; }


    }
}
