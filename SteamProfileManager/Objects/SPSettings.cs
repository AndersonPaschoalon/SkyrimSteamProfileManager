using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProfileManager.Objects
{
    /**
      <SETTINGS game="Skyrim SE"
                steamPath="TestEnviroment\Test02\Steam\Commons\"
                documentsPath="TestEnviroment\Test02\Docs"
                appDataPath="TestEnviroment\Test02\AppData"
                nmmModPath="TestEnviroment\Test02\NMM\nmmInfo"
                nmmInfoPath="TestEnviroment\Test02\NMM\nmmInfo"
                gameFolder="Skyrim SE"
                backupFolder="SkyrimSEBackups" />  
     **/
    public class SPSettings
    {
        [XmlAttribute("game")]
        public string game { get; set; }

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
