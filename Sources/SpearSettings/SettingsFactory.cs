using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SpearSettings
{
    public class SettingsFactory
    {
        /// <summary>
        /// Tells if a game name exists on the list of configured games.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static bool gameExist(string gameName)
        {
            List<string> listGames = SPConfig.listGameNames();
            if (!listGames.Contains(gameName))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// returns SPSettings object
        /// </summary>
        /// <returns></returns>
        public static SPSettings getSettings()
        {
            SPConfig config = SPConfig.loadConfig();
            return config.settings; ;
        }

        /// <summary>
        /// returns SPGame object.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static SPGame getGameSettings(string gameName)
        {
            SPConfig config = SPConfig.loadConfig();
            return  config.selectGame(gameName);
        }

        /// <summary>
        /// Get PathsHelper object 
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static PathsHelper getPathsHelper(string gameName)
        {
            SPConfig config = SPConfig.loadConfig();
            SPGame gameSettings = SettingsFactory.getGameSettings(gameName);
            SPSettings settings = SettingsFactory.getSettings();
            PathsHelper paths = new PathsHelper(settings, gameSettings);
            return paths;
        }

        /// <summary>
        /// returns date format from settings
        /// </summary>
        /// <returns></returns>
        public static string dateFormat()
        {
            SPSettings settings = SettingsFactory.getSettings();
            return settings.dateFormat;
        }

        /// <summary>
        /// Check settings. If it fails, the application settings are not valid of corrupted, and the application MUST be
        /// re-configurad to work properly and safaty.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static int checkConfig(string gameName)
        {
            // game name
            if (!SettingsFactory.gameExist(gameName))
            {
                return Errors.ERR_INVALID_GAME_NAME_1;
            }
  
            // load settings
            SPSettings settings = SettingsFactory.getSettings();
            SPGame gameSettings = SettingsFactory.getGameSettings(gameName);

            // game name
            if (gameSettings.game == null || gameSettings.game.Trim().Equals(""))
            {
                return Errors.ERR_INVALID_GAME_NAME_2;
            }
            // steam path
            else if (settings.steamPath == null ||
                settings.steamPath.Trim().Equals("") ||
                !Directory.Exists(settings.steamPath))
            {
                return Errors.ERR_STEAM_DIRRECTORY_MISSING_1;
            }
            // documents
            else if (!gameSettings.isDocumentsPathOptional())
            {
                if (settings.documentsPath == null ||
                    settings.documentsPath.Trim().Equals("") ||
                    !Directory.Exists(settings.documentsPath))
                {
                    return Errors.ERR_DOCUMENTS_DIRRECTORY_MISSING_1;
                }
            }
            // app data
            else if (!gameSettings.isAppDataPathOptional())
            {
                if (settings.appDataPath == null ||
                    settings.appDataPath.Trim().Equals("") ||
                    !Directory.Exists(settings.appDataPath))
                {
                    return Errors.ERR_APPDATA_DIRRECTORY_MISSING_2;
                }
            }
            // game folder
            else if (gameSettings.gameFolder == null || gameSettings.gameFolder.Trim().Equals(""))
            {
                return Errors.ERR_INVALID_GAME_FOLDER;
            }
            // game exe
            else if (gameSettings.gameExe == null || gameSettings.gameExe.Trim().Equals(""))
            {
                return Errors.ERR_INVALID_GAME_EXE;
            }
            // backup folder name
            else if (gameSettings.backupFolder == null || gameSettings.backupFolder.Trim().Equals(""))
            {
                return Errors.ERR_INVALID_BACKUP_FOLDER;
            }
            // arquivo de configuração está ok no formato. 
            // agora entradas redundantes ou invalidas devem ser eliminadas se existirem. 
            return Errors.SUCCESS;
        }

        /// <summary>
        /// This method helps the user to update the setting, checking the data consistence
        /// before making any change permanent.
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="userSettings"></param>
        /// <param name="paths"></param>
        /// <param name="spSettings"></param>
        /// <param name="spGame"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static int userUpdaterHelper(in string gameName,
                                            in UserSettings userSettings,
                                            ref PathsHelper paths,
                                            ref SPSettings spSettings,
                                            ref SPGame spGame,
                                            out string errMsg)
        {
            List<string> pathsToCheck = new List<string>();
            if (!userSettings.nmmPath.Trim().Equals(""))
            {
                pathsToCheck.Add(userSettings.nmmPath);
                pathsToCheck.Add(userSettings.nmmPathGameFolder);
            }
            if (!userSettings.vortexPath.Trim().Equals(""))
            {
                pathsToCheck.Add(userSettings.vortexPath);
                pathsToCheck.Add(userSettings.vortexPathGameFolder);
            }

            // Check if directories exist
            string errString = "";
            if (!CSharp.checkDirs(pathsToCheck, out errString))
            {
                errMsg = errString;
                return Errors.ERR_PATH_NOT_EXIST;
            }

            // Settings are OK
            // update paths
            spSettings.nmmPath2 = userSettings.nmmPath;
            spSettings.vortexPath2 = userSettings.vortexPath;
            spGame.nmmGameFolder = userSettings.nmmGameFolder;
            spGame.vortexGameFolder = userSettings.vortexGameFolder;
            // update tools
            spSettings.nmmExe = userSettings.nmmExe;
            spSettings.vortexExe = userSettings.vortexExe;
            spSettings.tesvEditExe = userSettings.tesveditExe;

            // update path helper
            paths.update(spSettings, spGame);

            // -- UPDATE settings file --
            // update Settings container
            string saveErr = "";
            SPConfig config = SPConfig.loadConfig();
            int retUp = config.updateSettings(spSettings, gameName, spGame, out saveErr);
            if (retUp != Errors.SUCCESS)
            {
                errMsg = saveErr;
                return retUp;
            }
            // save file
            bool retSave = config.saveConfig(out saveErr);
            if (!retSave)
            {
                errMsg = saveErr;
                return Errors.ERR_SAVING_CONFIGURATION_FILE;
            }
            errMsg = "";
            return Errors.SUCCESS;
        }


        /*
        public string desactivatedIntegrityFileContent(string prof)
        {
            if (this.desactivatedIntegrityFileExists(prof))
            {
                string intFile = this.desactivatedIntegrityFilePath(prof);
                string integrityFileContent = File.ReadAllText(intFile);
                return integrityFileContent;
            }
            return "";
        }
        */

        /*
        public List<string> desactivatedIntegrityFileItems(string prof)
        {
            string content = this.desactivatedIntegrityFileContent(prof);
            return CSharp.csvToList(content);
        }
        */

        /*
        public bool updateDesactivatedIntegrityFile(SPProfile prof, out string errMsg, out string errPath)
        {
            string content = prof.name + "," + prof.color + "," + prof.creationDate;
            string filePath = this.desactivatedIntegrityFilePath(prof.name);
            try
            {
                File.WriteAllText(filePath, content);
                errMsg = "SUCCESS";
                errPath = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errPath = filePath;
                return false;
            }
        }
        */


    }
}
