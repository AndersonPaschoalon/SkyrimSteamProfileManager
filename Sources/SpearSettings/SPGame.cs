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

        [XmlAttribute("docsSubPath")]
        public string docsSubPath { get; set; }

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

        [XmlAttribute("appDataPathIsOptional")]
        public string appDataPathIsOptional { get; set; }

        [XmlAttribute("useGameLogs")]
        public string useGameLogs { get; set; }


        #region methods

        public bool isDocumentsPathOptional()
        {
            return (this.documentsPathIsOptional.Trim().ToUpper() == "TRUE") ? true : false;
        }

        public void setDocumentsPathIsOptional(bool setVal)
        {
            this.documentsPathIsOptional = (setVal) ? "TRUE" : "FALSE";
        }

        public bool isAppDataPathOptional()
        {
            return (this.appDataPathIsOptional.Trim().ToUpper() == "TRUE") ? true : false;
        }

        public void setAppDataPathIsOptional(bool setVal)
        {
            this.appDataPathIsOptional = (setVal) ? "TRUE" : "FALSE";
        }

        public bool gameLogsAreSet()
        {
            return (this.useGameLogs.Trim().ToUpper() == "TRUE") ? true : false;
        }

        public void setUseGameLogs(bool setVal)
        {
            this.useGameLogs = (setVal) ? "TRUE" : "FALSE";
        }

        #endregion methods



    }
}
