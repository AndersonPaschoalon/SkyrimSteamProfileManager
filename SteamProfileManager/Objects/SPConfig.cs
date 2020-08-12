using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Logger;
using ProfileManager.Enum;

namespace ProfileManager.Objects
{
    [XmlRoot("CONFIG", IsNullable = false)]
    public class SPConfig
    {
        private static Logger.ILogger log = LoggerFactory.getLogger(LoggerFactory.LogType.CONSOLE);
        private string configFileName;
        private static SPConfig instance = null;

        public static SPConfig loadConfig()
        {
            if (SPConfig.instance == null)
            {
                // TODO -> melhorar isso
                string configFileName = "Settings\\SPConfigSyrim.xml";
                log.Debug("-- CurrentDirectory:" + Directory.GetCurrentDirectory());
                try
                {
                    //if (File.Exists(configFileName))
                    //{
                        XmlSerializer serializer = new XmlSerializer(typeof(SPConfig));
                        FileStream fs = new FileStream(configFileName, FileMode.Open);
                        SPConfig.instance = (SPConfig)serializer.Deserialize(fs);
                    //}
                }
                catch (Exception ex)
                {
                    log.Error("Erro loading config file: " + configFileName +
                              ". Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
            }
            return SPConfig.instance;
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
                using (FileStream spConfigFile = File.OpenWrite(filename))
                {
                    XmlSerializer x = new System.Xml.Serialization.XmlSerializer(this.GetType());
                    x.Serialize(spConfigFile, this);
                }
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

        private SPConfig()
        {
            this.settings = new SPSettings();
            this.listProfiles = new SPProfileList();
            this.game = "";
           // this.configFileName = PathsHelper.getConfigFileName(game);
            this.configFileName = PathsHelper.getConfigFileName();
        }

        #endregion private
    }
}
