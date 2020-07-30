using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamProfileManager.Objects;
using SteamProfileManager.Enum;
using SPErrors;
using Logger;
using Logger.Objects;

namespace SteamProfileManager
{
    /** 
     * Usage:
     * SteamProfileManager manager = new SteamProfileManager(Game.SKYRIM);
     * 
     */
    public class SteamProfileManager
    {
        // Consts
        private const string ACTIVE_INTEGRITY_FILE_NAME = "active_profile.int";

        // readonly
        private readonly string backupRoot = "";
        private readonly ILogger log = ConsoleLogger.getInstance();

        // state
        private SPConfig config;
        private SPMState applicationState = SPMState.NOT_CONFIGURED;
        private SPProfile activeProfile;
        private List<SPProfile> listDesactivated;

        public SteamProfileManager(Game game)
        {
            log.Debug("-- Constructor for game " + game.ToString());
            log.Debug("-- load settings");
            this.config = SPConfig.getConfig(game);
            this.backupRoot = Helper.getBackupRoot(game);
            log.Debug("-- updateManagerState()");
            this.updateManagerState();
        }

        /// <summary>
        /// This method is used to configure the application for the first time, or to update the configuration.
        /// In case of success updates the app settings, otherwise returns an error code, and informs wich parameter
        /// caused the problem.
        /// </summary>
        /// <param name="newSteamPath"></param>
        /// <param name="newDocumentsPath"></param>
        /// <param name="newAppDataPath"></param>
        /// <param name="nmmInfoPath"></param>
        /// <param name="nmmModPath"></param>
        /// <returns></returns>
        public int updateSettings(string newSteamPath, string newDocumentsPath, string newAppDataPath, 
                                  string nmmInfoPath, string nmmModPath, out bool isSteamOk, 
                                  out bool isDocOk, out bool isAppdataOk, out bool isNmmInfoOk,
                                  out bool isNmmModOk)
        {
            int outVal = Errors.SUCCESS;
            isSteamOk = true;
            isDocOk = true;
            isAppdataOk = true;
            isNmmInfoOk = true;
            isNmmModOk = true;

            // check if settings are valid
            if (newSteamPath == null || newSteamPath.Trim().Equals(""))
            {
                outVal = Errors.ERR_INVALID_SETTINGS;
                isSteamOk = false;
            }
            if (newDocumentsPath == null || newDocumentsPath.Trim().Equals(""))
            {
                outVal = Errors.ERR_INVALID_SETTINGS;
                isDocOk = false;
            }
            if (newAppDataPath == null || newAppDataPath.Trim().Equals(""))
            {
                outVal = Errors.ERR_INVALID_SETTINGS;
                isAppdataOk = false;
            }
            if (nmmInfoPath == null)
            {
                outVal = Errors.ERR_INVALID_SETTINGS;
                isNmmInfoOk = false;
            }
            if (nmmModPath == null)
            {
                outVal = Errors.ERR_INVALID_SETTINGS;
                isNmmModOk = false;
            }
            if (outVal != Errors.SUCCESS)
            {
                return outVal;
            }

            // check if paths do exist
            if (!this.checkDir(newSteamPath))
            {
                isSteamOk = false;
                outVal = Errors.ERR_PATH_NOT_EXIST;
            }
            if (!this.checkDir(newDocumentsPath))
            {
                isDocOk = false;
                outVal = Errors.ERR_PATH_NOT_EXIST;
            }
            if (!this.checkDir(newAppDataPath))
            {
                isAppdataOk = false;
                outVal = Errors.ERR_PATH_NOT_EXIST;
            }
            // optional settings
            if (!nmmInfoPath.Trim().Equals(""))
            {
                if (!this.checkDir(nmmInfoPath))
                {
                    isNmmInfoOk = false;
                    outVal = Errors.ERR_PATH_NOT_EXIST;
                }
            }
            if (!nmmModPath.Trim().Equals(""))
            {
                if (!this.checkDir(nmmModPath))
                {
                    isNmmModOk = false;
                    outVal = Errors.ERR_PATH_NOT_EXIST;
                }
            }
            if (outVal != Errors.SUCCESS)
            {
                return outVal;
            }

            // Settings are OK. update settings 
            this.config.settings.appDataPath = newAppDataPath;
            this.config.settings.steamPath = newSteamPath;
            this.config.settings.documentsPath = newDocumentsPath;
            this.config.settings.nmmModPath = nmmInfoPath;
            this.config.settings.nmmInfoPath = nmmInfoPath;

            // create backup direcotories
            this.createBackupsFolder();

            // save settings
            this.config.saveConfig();

            // update app state
            this.applicationState = this.discoverApplicationState(out this.activeProfile, out this.listDesactivated);

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Creates an active profile from existing instalation. If there is already an active profile,
        /// or if there is no installation, this method does nothing adn returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int activateInactiveProfile(string profileName, string color)
        {
            // update state
            this.updateManagerState();
            if (this.applicationState != SPMState.INACTIVE_PROFILE)
            {
                log.Debug("-- activateInactiveProfile() operation can only be executed if the application state is SPMState.INACTIVE_PROFILE");
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }

            // Create profile object
            int profileNewId = this.config.listProfiles.profiles.Count + 1;
            foreach (var item in this.config.listProfiles.profiles)
            {
                if (profileName.Trim() == item.name.Trim())
                {
                    // name already in use
                    return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS;
                }
            }
            SPProfile newProfile = new SPProfile();
            newProfile.color = color;
            newProfile.id = profileNewId;
            newProfile.isActive = Utils.TRUE;
            newProfile.name = profileName;

            // Create integrity file 
            if (!this.createIntegrityFile(newProfile))
            {
                log.Error("Could not create intregrity file");
                return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE;
            }

            // add to configuration
            this.config.listProfiles.profiles.Add(newProfile);
            this.config.saveConfig();

            // update Manager state
            this.updateManagerState();

            // update 
            return Errors.SUCCESS;
        }

        /// <summary>
        /// If there is no profile installed, set a desactivated profile as active. Otherwise does nothing
        /// and returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int setDesactivatedProfileActive(string profileName)
        {
            // update manager state
            this.updateManagerState();
            if (this.applicationState != SPMState.DESACTIVATED_ONLY)
            {
                log.Warn("-- invalid state for requested operation. This operation may only be completed if the aplication state is SPMState.DESACTIVATED_ONLY.");
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }
            SPProfile profToActivate = null;
            foreach (var item in this.listDesactivated)
            {
                if (item.name == profileName)
                {
                    profToActivate = item;
                    break;
                }
            }
            if (profToActivate == null)
            {
                log.Warn("-- specified profile could not be found, ERR_INVALID_PROFILE_NAME");
                return Errors.ERR_INVALID_PROFILE_NAME;
            }
            string steamDesPath = "";
            string docsDesPath = "";
            string appDirDesPath = "";
            string nmmInfoDesPath = "";
            string nmmModsDesPath = "";

            this.getDesactivatedPaths(profileName, out steamDesPath,
                                      out docsDesPath, out appDirDesPath,
                                      out nmmInfoDesPath, out nmmModsDesPath);
            log.Debug("-- moving folders from backup to root dir");
            int check = Errors.SUCCESS;
            // move steam folder
            check = Utils.safeMove(steamDesPath, this.config.settings.steamPath);
            if (check != Errors.SUCCESS)
            {
                log.Warn("-- Error moving " + steamDesPath + " -> " + this.config.settings.steamPath);
                return check;
            }
            // move appdirfolder
            check = Utils.safeMove(appDirDesPath, this.config.settings.appDataPath);
            if (check != Errors.SUCCESS)
            {
                log.Warn("-- Error moving " + appDirDesPath + " -> " + this.config.settings.appDataPath);
                Utils.safeMove(this.config.settings.steamPathGame(), );
                return check;
            }
            // move doc folder
            Utils.safeMove();
            // move optional
            Utils.safeMove();
            // fazer rowback se algum falhar


            // create integrity file
            createIntegrityFile(profToActivate);

            // set profile as active
            profToActivate.isActive = Utils.TRUE;

            // save settings
            this.config.saveConfig();

            // update Manager state
            this.updateManagerState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int desactivateProfile(SPProfile profile)
        {
            // TODO
            return Errors.ERR_UNKNOWN;
        }

        public int switchProfile(SPProfile active, SPProfile desactivated)
        {
            // TODO
            return Errors.ERR_UNKNOWN;
        }

        #region app_state
        /// <summary>
        /// return a list of current desactivated profiles
        /// </summary>
        /// <returns></returns>
        public List<SPProfile> getDesactivatedProfiles()
        {
            return this.listDesactivated;
        }

        /// <summary>
        /// return the current active profile
        /// </summary>
        /// <returns></returns>
        public SPProfile getActiveProfile()
        {
            return this.activeProfile;
        }

        /// <summary>
        /// returns the application state
        /// </summary>
        /// <returns></returns>
        public SPMState getApplicationState()
        {
            return this.applicationState;
        }

        #endregion app_state

        #region private_methos

        #region paths 

        private string integrityFilePath()
        {
            return this.config.settings.steamPathGame() + "\\" + ACTIVE_INTEGRITY_FILE_NAME;
        }
        private string steamBackup()
        {
            return this.config.settings.steamPath + "\\" + this.backupRoot;
        }
        private string steamBackupGame(string name)
        {
            return steamBackup() + "\\" + 
                   name.Trim() + "\\" +
                   this.config.settings.gameFolder.Trim();
        }
        private string documentsBackup()
        {
            return this.config.settings.documentsPath + "\\" + this.backupRoot;
        }
        private string documentsBackupGame(string name)
        {
            return documentsBackup() + "\\" +
                   name.Trim() + "\\" +
                   this.config.settings.gameFolder.Trim();
        }
        private string appDataBackup()
        {
            return this.config.settings.appDataPath + "\\" + this.backupRoot;
        }
        private string appDataBackupGame(string name)
        {
            return appDataBackup() + "\\" +
                   name.Trim() + "\\" +
                   this.config.settings.gameFolder.Trim();
        }
        private string nmmInfoBackup()
        {
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                return this.config.settings.nmmInfoPath + "\\" + this.backupRoot;
            }
            else
            {
                return "";
            }
        }
        private string nmmInfoBackupGame(string name)
        {
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            { 
                return nmmInfoBackup() + "\\" +
                       name.Trim() + "\\" +
                       this.config.settings.gameFolder.Trim();
            }
            else
            {
                return "";
            }
        }
        private string nmmModBackup()
        {
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                return this.config.settings.nmmModPath + "\\" + this.backupRoot;
            }
            else
            {
                return "";
            }
        }
        private string nmmModBackupGame(string name)
        {
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                return nmmModBackup() + "\\" +
                       name.Trim() + "\\" +
                       this.config.settings.gameFolder.Trim();
            }
            else
            {
                return "";
            }
        }

        #endregion paths 

        #region utils 


        private bool checkDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                log.Warn("Directory " + dir + " does not exist.");
                return false;
            }
            return true;
        }

        #endregion utils 

        #region aux 

        private void getDesactivatedGamePaths(string  profileName, out string steamDesPath, 
                                          out string docsDesPath, out string appDirDesPath,
                                          out string nmmInfoDesPath, out string nmmModsDesPath)
        {
            // root\backupDir\profileName\gameFolder
            string desactivatedPathBase = this.backupRoot + "\\" + profileName + "\\" + config.settings.gameFolder;
            steamDesPath = this.config.settings.steamPath + desactivatedPathBase;
            docsDesPath = this.config.settings.documentsPath + desactivatedPathBase;
            appDirDesPath = this.config.settings.appDataPath + desactivatedPathBase;
            nmmInfoDesPath = "";
            nmmModsDesPath = "";
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                nmmInfoDesPath = this.config.settings.nmmInfoPath + desactivatedPathBase;
            }
            if (!this.config.settings.nmmModPath.Trim().Equals(""))
            {
                nmmModsDesPath = this.config.settings.nmmModPath + desactivatedPathBase;
            }
        }
        /// <summary>
        /// Retuns on the out parameters the paths where the desactivated games are located
        /// without the gamefolder reference
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="steamDesPath"></param>
        /// <param name="docsDesPath"></param>
        /// <param name="appDirDesPath"></param>
        /// <param name="nmmInfoDesPath"></param>
        /// <param name="nmmModsDesPath"></param>
        private void getDesactivatedPaths2(string profileName, out string steamDesPath,
                                          out string docsDesPath, out string appDirDesPath,
                                          out string nmmInfoDesPath, out string nmmModsDesPath)
        {
            // root\backupDir\profileName\gameFolder
            string desactivatedPathBase = this.backupRoot + "\\" + profileName + "\\";
            steamDesPath = this.config.settings.steamPath + desactivatedPathBase;
            docsDesPath = this.config.settings.documentsPath + desactivatedPathBase;
            appDirDesPath = this.config.settings.appDataPath + desactivatedPathBase;
            nmmInfoDesPath = "";
            nmmModsDesPath = "";
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                nmmInfoDesPath = this.config.settings.nmmInfoPath + desactivatedPathBase;
            }
            if (!this.config.settings.nmmModPath.Trim().Equals(""))
            {
                nmmModsDesPath = this.config.settings.nmmModPath + desactivatedPathBase;
            }
        }


        private void createBackupsFolder()
        {
            Directory.CreateDirectory(this.steamBackup());
            Directory.CreateDirectory(this.documentsBackup());
            Directory.CreateDirectory(this.appDataBackup());
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.nmmInfoBackup());
            }
            if (!this.config.settings.nmmModPath.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.nmmModBackup());
            }
        }

        private int countProfiles()
        {
            try
            {
                return this.config.listProfiles.profiles.Count;
            }
            catch (Exception ex)
            {
                log.Warn("No profile. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return 0;
        }

        /// <summary>
        /// Create a integrity file
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        bool createIntegrityFile(SPProfile prof)
        {
            try
            {
                File.WriteAllText(this.integrityFilePath(), prof.id + "," + prof.name + "," + prof.color);
                return true;
            }
            catch (Exception ex)
            {
                log.Warn("Could nor create integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// Delete integrity file.
        /// </summary>
        void deleteIntegrityFile()
        {
            try
            {
                File.Delete(this.integrityFilePath());
            }
            catch (Exception ex)
            {
                log.Warn("** Error deleting integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
        }

        /// <summary>
        /// Returns the application state, and returns by references (if it exists) the 
        /// active profile (null otherwise); and the list of desactivated profiles
        /// (or a empty list otherwise).
        /// </summary>
        /// <param name="activeProfileOk"></param>
        /// <param name="desactivatedProfilesOk"></param>
        /// <returns></returns>
        private SPMState discoverApplicationState(out SPProfile activeProfileOk,
                                               out List<SPProfile> desactivatedProfilesOk)
        {
            activeProfileOk = null;
            desactivatedProfilesOk = new List<SPProfile>();
            bool isInstalled = true;
            bool configStatus = this.checkConfig();
            if (!configStatus)
            {
                // settings are not ok
                return SPMState.NOT_CONFIGURED;
            }
            else
            {
                // check current instalations
                if (!this.checkDir(this.config.settings.steamPathGame()))
                {
                    isInstalled = false;
                }
                else
                {
                    foreach (var item in this.config.listProfiles.profiles)
                    {
                        if (item.isProfileActive())
                        {
                            if (this.checkActiveProfile(item))
                            {
                                activeProfileOk = item;
                                break;
                            }
                        }
                    }
                    // no active profile
                    if (activeProfileOk == null)
                    {
                        this.deleteIntegrityFile();
                    }
                }
                // check desactivated
                foreach (var item in this.config.listProfiles.profiles)
                {
                    if (!item.isProfileActive())
                    {
                        if (this.checkDesactivatedProfile(item))
                        {
                            desactivatedProfilesOk.Add(item);
                        }
                    }
                }
            }
            if (!isInstalled && desactivatedProfilesOk.Count == 0)
            {
                return SPMState.NO_PROFILE;
            }
            else if (isInstalled && activeProfileOk == null)
            {
                return SPMState.INACTIVE_PROFILE;
            }
            else if (activeProfileOk != null && desactivatedProfilesOk.Count == 0)
            {
                return SPMState.ACTIVE_ONLY;
            }
            else if (activeProfileOk != null && desactivatedProfilesOk.Count > 0)
            {
                return SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
            }
            else if (activeProfileOk == null && desactivatedProfilesOk.Count > 0)
            {
                return SPMState.DESACTIVATED_ONLY;
            }
            log.Error("Invalid Option");
            return SPMState.NOT_CONFIGURED;
        }

        #endregion aux 

        #region checkHelpers

        /// <summary>
        /// Check settings. If it fails, the application settings are not valid of corrupted, and the application MUST be
        /// re-configurad to work properly and safaty.
        /// </summary>
        /// <returns></returns>
        private bool checkConfig()
        {
            if (this.config == null)
            {
                log.Error("this.config object not initialized!");
                return false;
            }
            else if (this.config.settings == null)
            {
                log.Error("this.config.settings object not initialized!");
                return false;
            }
            else if (this.config.settings.appDataPath == null || this.config.settings.appDataPath.Trim().Equals("") ||
                     this.config.settings.documentsPath == null || this.config.settings.documentsPath.Trim().Equals("") ||
                     this.config.settings.gameFolder == null || this.config.settings.gameFolder.Trim().Equals("") ||
                     this.config.settings.nmmInfoPath == null || this.config.settings.nmmModPath == null)
            {
                log.Warn("Invalid SP settings, settings must be initialized before used");
                return false;
            }
            else if (!this.checkDir(this.config.settings.appDataPath))
            {
                return false;
            }
            else if (!this.checkDir(this.config.settings.documentsPath))
            {
                return false;
            }
            else if (!this.checkDir(this.config.settings.gameFolder))
            {
                return false;
            }
            string currDir = Directory.GetCurrentDirectory().Trim();
            if (currDir == this.config.settings.appDataPathGame() ||
                currDir == this.config.settings.documentsPathGame() ||
                currDir == this.config.settings.appDataPathGame() ||
                currDir == this.config.settings.nmmInfoPathGame() ||
                currDir == this.config.settings.nmmModPathGame())
            {
                log.Error("INVALID CONFIGURATION PATH!!!!");
                return false;
            }
            else if (this.config.listProfiles == null)
            {
                log.Error("List of profiles must exist");
                return false;
            }
            // arquivo de configuração está ok no formato. 
            // agora entradas redundantes ou invalidas devem ser eliminadas se existirem. 
            return true;
        }

        /// <summary>
        /// Updates the Steam Profile Manager instance state, reloading all class members 
        /// inclusing the list of desactivated and active profiles. It also checks if the paths are still
        /// valid (if the paths are not, the state is set back to SPMSTate.NOT_CONFIGURED)
        /// </summary>
        private void updateManagerState()
        {
            // update state
            this.applicationState = this.discoverApplicationState(out this.activeProfile, out this.listDesactivated);
            // log statte
            log.Debug("-- State: " + this.applicationState.ToString());
            log.Debug("-- Active Profile");
            if (this.activeProfile != null)
            {
                log.Debug("  id:" + this.activeProfile.id + 
                          ", name:" + this.activeProfile.name + ", isActive:" + this.activeProfile.isActive );
            }
            else
            {
                log.Debug("-- no active profile");
            }
            log.Debug("-- Desactivated Profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("  id:" + item.id +
                          ", name:" + item.name + 
                          ", isActive:" + item.isActive);
            }
        }

        /// <summary>
        /// Check all settings of a specified ACTIVE profile. If some settings are inconsistent 
        /// with an active profile, returns false.
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        private bool checkActiveProfile(SPProfile prof)
        {
            // check object 
            if (!prof.isProfileActive())
            {
                log.Warn("Object property isActive is set as false or invalid");
                return false;
            }

            // checar configuração
            if (!this.checkDir(this.config.settings.appDataPathGame()) ||
                !this.checkDir(this.config.settings.documentsPathGame()) ||
                !this.checkDir(this.config.settings.steamPathGame()))
            {
                log.Warn("A game directory does not exist. appDataPathGame:{" + this.config.settings.appDataPathGame() + "}" +
                         ", documentsPathGame:{" + this.config.settings.documentsPathGame() + "}" +
                         ", steamPathGame:{" + this.config.settings.steamPathGame() + "}");
                return false;
            }
            if (!this.checkDir(this.integrityFilePath()))
            {
                log.Warn("Integrity file of active profile does not exist");
                return false;
            }
            string integrityFile = File.ReadAllText(this.integrityFilePath());
            List<string> profData = Utils.splitCsv(integrityFile);
            if (profData.Count < 3)
            {
                log.Warn("Integrity file corrupted {" + integrityFile + "}");
                return false;
            }
            int val0 = 0;
            int.TryParse(profData[0], out val0);
            if (prof.id != val0)
            {
                log.Warn("Ids do not match");
                return false;
            }
            if (prof.name.Trim() != profData[1].Trim())
            {
                log.Warn("Names do not match");
                return false;
            }
            if (prof.color.Trim() != profData[2].Trim())
            {
                log.Warn("Colors do not match");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Chekc all settings of a Desactivated profile. If some settings are inconsistent
        /// with an desactivated profile, returns true.
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        private bool checkDesactivatedProfile(SPProfile prof)
        {
            if (prof.isProfileActive())
            {
                // profile is set as activated
                return false;
            }
            // root\backupDir\ProfName\gameFolder
            string desactivatedPathBase = this.backupRoot + "\\" + prof.name + "\\" + config.settings.gameFolder;
            string steamDesPath = this.config.settings.steamPath + desactivatedPathBase;
            string docsDesPath = this.config.settings.documentsPath + desactivatedPathBase;
            string appDirDesPath = this.config.settings.appDataPath + desactivatedPathBase;
            string nmmInfoDesPath = this.config.settings.nmmInfoPath + desactivatedPathBase;
            string nmmModsDesPath = this.config.settings.nmmModPath + desactivatedPathBase;

            // check mandatory paths
            if (!this.checkDir(steamDesPath))
            {
                return false;
            }
            if (!this.checkDir(docsDesPath))
            {
                return false;
            }
            if (!this.checkDir(appDirDesPath))
            {
                return false;
            }

            // optional: check if is defined
            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                if (!this.checkDir(nmmInfoDesPath))
                {
                    return false;
                }
            }
            if (!this.config.settings.nmmModPath.Trim().Equals(""))
            {
                if (!this.checkDir(nmmModsDesPath))
                {
                    return false;
                }
            }

            log.Debug("-- profile " + prof.name + " is ok");
            return true;
        }



        #endregion checkHelpers

        #endregion private_methos

    }
}
