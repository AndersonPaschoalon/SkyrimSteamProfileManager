using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logger;
using SteamProfileManager.Enum;

namespace SteamProfileManager.Objects
{
    [XmlRoot("SPCONFIG", IsNullable = false)]
    public class SPConfig
    {
        private static Logger.ILogger log = LoggerFactory.getLogger(LoggerFactory.LogType.CONSOLE);
        private string configFileName;

        public static SPConfig getConfig(Game game)
        {
            SPConfig temp = new SPConfig(game);
            SPConfig instance = temp.pGetConfig();
            return instance;
        }

        /// <summary>
        /// Save the content object back to a XML file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool saveConfig(string filename)
        {
            try
            {
                FileStream spConfigFile = File.OpenWrite(filename);
                XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                x.Serialize(spConfigFile, this);
                return true;
            }
            catch (Exception ex)
            {
                log.Warn("Error serializing SPSettings XML back to file " + filename + 
                         ". Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return false;
        }

        public bool saveConfig()
        {
            return this.saveConfig(this.configFileName);
        }


        [XmlAttribute("game")]
        public string game { get; set; }

        [XmlElement("SETTINGS")]
        public SPSettings settings { get; set; }


        [XmlElement("PROFILES")]
        public SPProfileList listProfiles { get; set; }

        #region private 

        // private constructor
        private SPConfig(Game game)
        {
            this.settings = new SPSettings();
            this.listProfiles = new SPProfileList();
            this.game = "";
            this.configFileName = PathsHelper.getConfigFileName(game);
        }

        /// <summary>
        /// Returns a serialized version of the SPConfig file into a class
        /// </summary>
        /// <returns></returns>
        private SPConfig pGetConfig()
        {
            SPConfig config = null;
            try
            {
                if (File.Exists(this.configFileName))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SPConfig));
                    FileStream fs = new FileStream(this.configFileName, FileMode.Open);
                    config = (SPConfig)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                log.Error("Erro loading config file: " + this.configFileName +
                          ". Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return config;
        }

        #endregion private
    }
}
