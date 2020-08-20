using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
//using Logger;
//using Logger.Loggers;
using ProfileManager.Enum;
using Utils;
using Utils.Loggers;

namespace ProfileManager.Objects
{
    [XmlRoot("CONFIG", IsNullable = false)]
    public class SPConfig
    {
        #region load/save

        /// <summary>
        /// Load configuration from default XML settings configuration file
        /// </summary>
        /// <returns></returns>
        public static SPConfig loadConfig()
        {
            ILogger logger = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            if (SPConfig.instance == null)
            {
                string configFile = PathsHelper.getConfigFileName();
                logger.Debug("-- CurrentDirectory:" + Directory.GetCurrentDirectory());
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SPConfig));

                    using (FileStream fs = File.OpenWrite(configFile))
                    {
                        SPConfig.instance = (SPConfig)serializer.Deserialize(fs);
                    }
                        //FileStream fs = new FileStream(configFile, FileMode.Open);
                    
                }
                catch (Exception ex)
                {
                    logger.Error("Erro loading config file: " + configFile +
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
                    XmlSerializer x = new XmlSerializer(this.GetType());
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

        /// <summary>
        /// Save current loaded configuration on the default configuration file.
        /// </summary>
        /// <returns></returns>
        public bool saveConfig()
        {
            return this.saveConfig(this.configFileName);
        }

        #endregion load/save

        #region xml

        [XmlAttribute("game")]
        public string game { get; set; }

        [XmlElement("SETTINGS")]
        public SPSettings settings { get; set; }

        [XmlElement("PROFILES")]
        public SPProfileList listProfiles { get; set; }

        #endregion xml

        #region private 

        private readonly ILogger log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
        private readonly string configFileName;
        private static SPConfig instance = null;

        private SPConfig()
        {
            this.settings = new SPSettings();
            this.listProfiles = new SPProfileList();
            this.game = "";
            this.configFileName = PathsHelper.getConfigFileName();
        }

        #endregion private
    }
}
