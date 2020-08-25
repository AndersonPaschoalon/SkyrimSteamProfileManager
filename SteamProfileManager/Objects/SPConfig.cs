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

        // TODO suporte para mais jogos no load

        /// <summary>
        /// Load configuration from default XML settings configuration file
        /// </summary>
        /// <returns></returns>
        public static SPConfig loadConfig()
        {
            // remove
            string gameFolder = "Skyrim";
            string gameName = "Skyrim";
            // vars
            SPConfig configuration;
            ILogger logger = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            string configFile = PathsHelper.getConfigFileName();
            // create if does not exit
            logger.Debug("CurrentDirectory:" + Directory.GetCurrentDirectory());
            logger.Debug("Configuration file: " + configFile);
            logger.Debug("Cheking if configuration file exists");
            if (!File.Exists(configFile))
            {
                logger.Warn("config file does not exit!");
                string xmlTemplate = ProfileManager.Properties.Resources.SPConfigTemplate;
                logger.Debug("Creating config file from template: " + xmlTemplate);
                string formattedConfigFile = String.Format(xmlTemplate, gameName, gameFolder);
                logger.Debug("Formatted XML:" + formattedConfigFile);
                File.WriteAllText(configFile, formattedConfigFile);
                logger.Debug("Config XML written successefuly!");
            }
            // load content
            logger.Debug("reading condiguration file content");
            // usar string
            XmlSerializer serializer = new XmlSerializer(typeof(SPConfig));
            string xmlText = "";
            if (File.Exists(configFile))
            {
                xmlText = File.ReadAllText(configFile);
            }
            else
            {
                logger.Error("COULD NOD LOAD CONFIGURATION FILE " + configFile);
                return new SPConfig();
            }
            logger.Debug("xml configuration file: {" + xmlText  + "}");

            // desserialize configuration file
            logger.Debug("-- desserialize configuration file");
            try
            {
                using (TextReader reader = new StringReader(xmlText))
                {
                    configuration = (SPConfig)serializer.Deserialize(reader);
                    reader.Close();
                }
                return configuration;
            }
            catch (Exception ex)
            {
                logger.Error("*** COULD NOD PARSE CONFIGURATION FILE " + configFile);
                logger.Error("*** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return new SPConfig();
            }
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
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                string newXml = "";
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, this);
                    newXml = textWriter.ToString();
                    File.WriteAllText(filename, newXml);
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
