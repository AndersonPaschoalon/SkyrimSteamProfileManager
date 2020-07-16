using Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SkyrimSteamProfileManager.Objects
{
    [XmlRoot("SSPCONFIG", IsNullable = false)]
    public class SSPConfig
    {
        private static readonly Logger.ILogger log = LoggerFactory.getLogger(LoggerFactory.LogType.CONSOLE);
        const string CONFIG_FILE_NAME = "SSPConfig.xml";

        public SSPConfig()
        {
            this.settings = new SSPSettings();
            this.listProfiles = new SSPProfileList();
        }

        public static SSPConfig getConfig()
        {
            string configFile = CONFIG_FILE_NAME;
            SSPConfig config = null;
            try
            {
                if (File.Exists(CONFIG_FILE_NAME))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SSPConfig));
                    FileStream fs = new FileStream(configFile, FileMode.Open);
                    config = (SSPConfig)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                log.Error("Erro loading config file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            config = new SSPConfig();
            return config;
        }

        public bool saveConfig(string filename)
        {
            try
            {
                FileStream sspConfigFile = File.OpenWrite(filename);
                XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                x.Serialize(sspConfigFile, this);
                return true;
            }
            catch (Exception ex)
            {
                log.Warn("Error serializing SSPSettings XML back to file " + filename + ". Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return false;
        }

        public bool saveConfig()
        {
            return this.saveConfig(CONFIG_FILE_NAME);
        }

        [XmlElement("SETTINGS")]
        public SSPSettings settings { get; set; }


        [XmlElement("PROFILES")]
        public SSPProfileList listProfiles { get; set; }

        //[XmlArray("PROFILES")]
        //public SSPProfile[] listProfiles { get; set; }

    }
}
