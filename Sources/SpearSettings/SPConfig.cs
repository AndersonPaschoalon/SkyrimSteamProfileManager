using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Utils;
using Utils.Loggers;

namespace SpearSettings
{
    [XmlRoot("CONFIG", IsNullable = false)]
    public class SPConfig
    {
        private readonly ILogger log = Log4NetLogger.getInstance(LogAppender.APP_SETTINGS);
        private readonly string STEAMPATH = @"C:\Program Files (x86)\Steam\steamapps\common";
        private readonly string DOCSPATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly string APPDATAPATH = Environment.GetEnvironmentVariable("localappdata");

        private SPConfig()
        {
        }

        #region load/save

        /// <summary>
        /// Return a list of configured games
        /// </summary>
        /// <returns></returns>
        public static List<string> listGameNames()
        {
            SPConfig config = SPConfig.loadConfig();
            List<string> listGamesStr = new List<string>();
            List<SPGame> listGames = config.listGames;
            if (listGames != null)
            {
                foreach (var item in listGames)
                {
                    string gamaStr = item.game.Trim();
                    listGamesStr.Add(gamaStr);
                }
            }
            return listGamesStr;
        }

        public static string[] arrayGames()
        {
            List<string> lg;
            try
            {
                lg = SPConfig.listGameNames();
                if (lg.Count < 1)
                {
                    MessageBox.Show("Error! Invalid number of Games on Settings File! The file might be corrupted!",
                                    "**Error**",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    string[] arrayError = { "SETTINGS ERROR!" };
                    return arrayError;
                }
                string[] arrayGames = lg.ToArray();
                return arrayGames;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                string[] arrayError = { "SETTINGS ERROR!" };
                return arrayError;
            }
        }

        /// <summary>
        /// Load configuration from default XML settings configuration file
        /// </summary>
        /// <returns></returns>
        public static SPConfig loadConfig()
        {
            // vars
            SPConfig configuration;
            ILogger logger = Log4NetLogger.getInstance(LogAppender.APP_SETTINGS);
            string configFile = PathsHelper.getConfigFileName();
            // create if does not exit
            logger.Debug("CurrentDirectory:" + Directory.GetCurrentDirectory());
            logger.Debug("Configuration file: " + configFile);
            logger.Debug("Cheking if configuration file exists");
            if (!File.Exists(configFile))
            {
                logger.Warn("config file does not exit!");
                string xmlTemplate = Properties.Resources.SPConfig;
                logger.Debug("Creating config file from template: " + xmlTemplate);
                logger.Debug("Formatted XML:" + xmlTemplate);
                File.WriteAllText(configFile, xmlTemplate);
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
                configuration.systemDefaults();
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
        public bool saveConfig(string filename, out string errMsg)
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
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                log.Warn("Error serializing SPSettings XML back to file " + filename + 
                         ". Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// Save current loaded configuration on the default configuration file.
        /// </summary>
        /// <returns></returns>
        public bool saveConfig(out string errMsg)
        {
            string configFile = PathsHelper.getConfigFileName();
            return this.saveConfig(configFile, out errMsg);
        }

        public SPGame selectGame(string game)
        {
            foreach (var item in this.listGames)
            {
                if (item.game.Trim() == game.Trim())
                {
                    return item;
                }
            }
            return null;
        }

        public int updateSettings(SPSettings appSettings, string game, SPGame gameSettings, out string errMsg)
        {
            bool upGame = false;
            bool upApp = false;
            for (int i = 0; i < this.listGames.Count; i++)
            {
                if (this.listGames[i].game.Trim() == game.Trim())
                {
                    this.listGames[i] = gameSettings;
                    upGame = true;
                    break;
                }
            }
            if (appSettings != null)
            {
                this.settings = appSettings;
                upApp = true;
            }

            if (upApp == true && upGame == true)
            {
                errMsg = "";
                return Errors.SUCCESS;
            }
            if (!upGame)
            {
                errMsg = "Invalid game name <" + game + ">";
                return Errors.ERR_INVALID_GAME_NAME_3;
            }
            errMsg = "Invalid settings";
            return Errors.ERR_ARGUMENT_NULL;
        }

        public bool updateSettings(SPSettings appSettings)
        {
            if (appSettings != null)
            {
                this.settings = appSettings;
                return true;
            }
            return false;
        }

        #endregion load/save

       [XmlElement("SETTINGS")]
        public SPSettings settings { get; set; }

        [XmlElement("GAME")]
        public List<SPGame> listGames { get; set; }

        #region helpers

        private void systemDefaults()
        {
            string errMsg = "";
            bool saveSettings = false;
            if (this.settings.steamPath == null || this.settings.steamPath.Trim().Equals(""))
            {
                log.Debug(" -- setting default steamPath:<" + STEAMPATH + ">");
                this.settings.steamPath = STEAMPATH;
                saveSettings = true;
            }
            if (this.settings.documentsPath == null || this.settings.documentsPath.Trim().Equals(""))
            {
                log.Debug(" -- setting default documentsPath:<" + DOCSPATH + ">");
                this.settings.documentsPath = DOCSPATH;
                saveSettings = true;
            }
            if (this.settings.appDataPath == null || this.settings.appDataPath.Trim().Equals(""))
            {
                log.Debug(" -- setting default appDataPath:<" + APPDATAPATH + ">");
                this.settings.appDataPath = APPDATAPATH;
                saveSettings = true;
            }
            // update settings 
            if (saveSettings)
            {
                log.Debug("SAVE defaults settings");
                this.saveConfig(out errMsg);
            }
        }

        #endregion helpers

    }
}
