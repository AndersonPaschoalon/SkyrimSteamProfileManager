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

        [XmlAttribute("docsFolder")]
        public string docsFolder { get; set; }

        [XmlAttribute("appDataFolder")]
        public string appDataFolder { get; set; }

        [XmlAttribute("nmmGameFolder")]
        public string nmmGameFolder { get; set; }

        [XmlAttribute("vortexGameFolder")]
        public string vortexGameFolder { get; set; }

        [XmlAttribute("backupFolder")]
        public string backupFolder { get; set; }

        [XmlAttribute("gameLogPath")]
        public string gameLogPath { get; set; }

        [XmlAttribute("gameLogExt")]
        public string gameLogExt { get; set; }

        [XmlAttribute("documentsPathIsOptional")]
        public string documentsPathIsOptional { get; set; }

        public bool boolDocumentsPathIsOptional
        {
            get 
            {
                return (documentsPathIsOptional.Trim().ToUpper() == "TRUE") ? true : false;
            }
            set 
            {
                documentsPathIsOptional = (value) ? "TRUE" : "FALSE";
            }
        }

        [XmlAttribute("appDataPathIsOptional")]
        public string appDataPathIsOptional { get; set; }

        public bool boolAppDataPathIsOptional
        {
            get
            {
                return (appDataPathIsOptional.Trim().ToUpper() == "TRUE") ? true : false;
            }
            set
            {
                appDataPathIsOptional = (value) ? "TRUE" : "FALSE";
            }
        }

        [XmlAttribute("useGameLogs")]
        public string useGameLogs { get; set; }

        public bool boolUseGameLogs
        {
            get
            {
                return (useGameLogs.Trim().ToUpper() == "TRUE") ? true : false;
            }
            set
            {
                useGameLogs = (value) ? "TRUE" : "FALSE";
            }
        }

    }
}
