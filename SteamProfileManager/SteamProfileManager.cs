
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProfileManager.Objects;
using ProfileManager.Enum;
using SPErrors;
using Logger;
using Logger.Loggers;
using Microsoft.VisualBasic.FileIO;

namespace ProfileManager
{
    /** 
     * Usage:
     * ProfileManager manager = new ProfileManager(Game.SKYRIM);
     */
    public class SteamProfileManager
    {
        // Consts
        private const string ACTIVE_INTEGRITY_FILE_NAME = "active_profile.int";

        // readonly
        private static ILogger log = ConsoleLogger.getInstance();
        //private static Logger.ILogger log = Log4NetLogger.getInstance(Logger.Loggers.LogAppender.MANAGER);

        // state
        private PathsHelper paths;
        private SPConfig config;
        private SPMState applicationState = SPMState.NOT_CONFIGURED;
        private SPProfile activeProfile;
        private List<SPProfile> listDesactivated;

        public SteamProfileManager(Game game)
        {
            log.Debug("-- Constructor for game " + game.ToString());
            log.Debug("-- load settings");
            this.config = SPConfig.loadConfig();
            this.paths = new PathsHelper(game, this.config.settings);
            log.Debug("-- updateManagerState()");
            this.updateManagerState();
        }

        public SPMState showState()
        {
            this.updateManagerState();
            Console.WriteLine("*********************************************");
            Console.WriteLine("** STEAM PROFILE MANAGER");
            Console.WriteLine("*********************************************");
            Console.WriteLine("applicationState:" + this.applicationState.ToString());
            Console.WriteLine("* Active profiles");
            if (this.activeProfile != null)
            {
                Console.WriteLine("  name:" + this.activeProfile.name + ", color:" + this.activeProfile.color);
            }
            Console.WriteLine("* Desactivated profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                Console.WriteLine("  name:" + item.name + ", color:" + item.color);

            }
            return this.applicationState;
        }

        /// <summary>
        /// This method is used to configure the application for the first time, or to update the configuration.
        /// In case of success updates the app settings, otherwise returns an error code, and informs wich parameter
        /// caused the problem. This methods is ment be be called by the User Interface
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

            // update path helper
            this.paths.update(this.config.settings);

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

            return Errors.SUCCESS;
        }

        /// <summary>
        /// If there is no profile installed, set a desactivated profile as active. Otherwise does nothing
        /// and returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int activateDesactivatedProfile(string profileName)
        {
            log.Debug("STEP 1: Check Settings...");
            this.updateManagerState();
            if (this.applicationState != SPMState.DESACTIVATED_ONLY)
            {
                log.Warn("-- invalid state for requested operation. This operation may only be completed if the aplication state is SPMState.DESACTIVATED_ONLY.");
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }
            profileName = profileName.Trim();
            if (profileName == null || profileName.Equals(""))
            {
                log.Warn("-- profile name is empty");
                return Errors.ERR_INVALID_PROFILE_NAME;
            }

            log.Debug("STEP 2: search profile to activate...");
            SPProfile profToActivate = null;
            foreach (var item in this.listDesactivated)
            {
                if (item.name.Trim() == profileName)
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

            log.Debug("STEP 3: moving folders from backup to root dir...");
            int moveCounter = 0;
            int check = Errors.SUCCESS;
            string srcDir = this.paths.steamBkpProfGame(profileName);

            // move steam folder
            check = Utils.safeMove(srcDir, this.paths.steam);
            if (check != Errors.SUCCESS)
            {
                log.Warn("-- Error moving " + srcDir + " -> " + this.paths.steam);
                return check;
            }
            moveCounter++;

            // move appData folder
            srcDir = this.paths.appDataBkpProfGame(profileName);
            check = Utils.safeMove(srcDir, this.paths.appData);
            if (check != Errors.SUCCESS)
            {
                log.Warn("-- Error moving " + srcDir + " -> " + this.paths.appData);
                this.undoDesactiveActivation(moveCounter, profileName);
                return check;
            }
            moveCounter++;

            // move doc folder
            srcDir = this.paths.docsBkpProfGame(profileName);
            check = Utils.safeMove(srcDir, this.paths.docs);
            if (check != Errors.SUCCESS)
            {
                log.Warn("-- Error moving " + srcDir + " -> " + this.paths.docs);
                this.undoDesactiveActivation(moveCounter, profileName);
                return check;
            }
            moveCounter++;

            srcDir = this.paths.nmmInfoBkpProfGame(profileName);
            if (!this.paths.nmmInfo.Trim().Equals(""))
            {
                check = Utils.safeMove(srcDir, this.paths.nmmInfo);
                if (check != Errors.SUCCESS)
                {
                    log.Warn("-- Error moving " + srcDir + " -> " + this.paths.nmmInfo);
                    this.undoDesactiveActivation(moveCounter, profileName);
                    return check;
                }
            }
            moveCounter++;

            srcDir = this.paths.nmmModBkpProfGame(profileName);
            if (!this.paths.nmmMod.Trim().Equals(""))
            {
                check = Utils.safeMove(srcDir, this.paths.nmmMod);
                if (check != Errors.SUCCESS)
                {
                    log.Warn("-- Error moving " + srcDir + " -> " + this.paths.nmmMod);
                    this.undoDesactiveActivation(moveCounter, profileName);
                    return check;
                }
            }
            moveCounter++;

            log.Debug("STEP 4: create integrity file...");
            this.createIntegrityFile(profToActivate);

            log.Debug("STEP 5: update manager state...");
            profToActivate.isActive = Utils.TRUE;
            this.config.saveConfig();
            this.updateManagerState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int desactivateActiveProfile(string profileName)
        {
            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) ||
                (this.applicationState != SPMState.ACTIVE_ONLY))
            {
                log.Warn("-- invalid state for desactivateActiveProfile operation! State:" + this.applicationState);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }

            // check if integrity file information matches
            profileName.Trim();
            string id = "";
            string name = "";
            string color = "";
            int ret = this.readIntegrityFile(out name, out color);
            if (name != profileName)
            {
                log.Warn("active profile corrupted");
                this.deleteIntegrityFile();
                this.updateManagerState();
                return Errors.ERR_ACTIVE_PROFILE_CORRUPTED;
            }
            log.Debug("-- integrity file OK! " + id + ", " + name + ", " + color);

            // delete integrity file
            this.deleteIntegrityFile();

            // create backup dir if does not exit
            this.createBackupProfilesFolder(profileName);

            // move directories to backup dir
            Utils.safeMove(this.paths.steamGame, this.paths.steamBkpProf(profileName));
            Utils.safeMove(this.paths.docsGame, this.paths.docsBkpProf(profileName));
            Utils.safeMove(this.paths.appDataGame, this.paths.appDataBkpProf(profileName));
            if (this.paths.nmmInfo != "")
            {
                Utils.safeMove(this.paths.nmmInfoGame, this.paths.nmmInfoBkpProf(profileName));
            }
            if (this.paths.nmmMod != "")
            {
                Utils.safeMove(this.paths.nmmModGame, this.paths.nmmModBkpProf(profileName));
            }

            // set profile as desativated
            foreach (var item in this.config.listProfiles.profiles)
            {
                if (item.name.Trim() == profileName)
                {
                    item.isActive = Utils.FALSE;
                    break;
                }
            }

            // save settings
            this.config.saveConfig();
            
            return Errors.ERR_UNKNOWN;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activeProf"></param>
        /// <param name="desactivatedProf"></param>
        /// <returns></returns>
        public int switchProfile(string activeProf, string desactivatedProf)
        {
            // check state
            this.updateManagerState();
            if (this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES)
            {
                log.Warn("-- invalid state for desactivateActiveProfile operation! State:" + this.applicationState);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }

            int ret;
            activeProf.Trim();
            desactivatedProf.Trim();
            ret = this.desactivateActiveProfile(activeProf);
            if (ret != Errors.SUCCESS)
            {
                log.Error("-- Error desactivating profile activeProf:" + activeProf + ", ret:" + ret);
                return ret;
            }

            ret = this.activateDesactivatedProfile(desactivatedProf);
            if (ret != Errors.SUCCESS)
            {
                log.Error("-- Error activating profile desactivatedProf:" + desactivatedProf + ", ret:" + ret);
                log.Info("-- UNDOING THE LAST DESACTIVATION!!");
                this.activateDesactivatedProfile(activeProf);
                return ret;
            }

            // update settings
            this.config.saveConfig();
            this.updateManagerState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profNameOld"></param>
        /// <param name="profNameNew"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public int editProfile(string profNameOld, string profNameNew, string color)
        {
            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) ||
                (this.applicationState != SPMState.ACTIVE_ONLY) ||
                (this.applicationState != SPMState.DESACTIVATED_ONLY))
            {
                log.Warn("-- invalid state for requested operation editProfile. State:" + this.applicationState);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }

            profNameOld.Trim();
            SPProfile prof = this.searchProfile(profNameOld, 0); // any profile
            if (prof != null)
            {
                prof.name = profNameNew;
                prof.color = color;

                if (prof.isProfileActive())
                {
                    // update integrity file
                    this.createIntegrityFile(prof);
                }
                else
                {
                    // rename backup folders
                    // Steam bkp
                    log.Info("-- renaming " + this.paths.steamBkpProf(profNameOld) + " -> " + profNameNew);
                    FileSystem.RenameDirectory(this.paths.steamBkpProf(profNameOld), profNameNew);

                    //Docs bkp
                    log.Info("-- renaming " + this.paths.docsBkpProf(profNameOld) + " -> " + profNameNew);
                    FileSystem.RenameDirectory(this.paths.docsBkpProf(profNameOld), profNameNew);

                    // appData bkp
                    log.Info("-- renaming " + this.paths.appDataBkpProf(profNameOld) + " -> " + profNameNew);
                    FileSystem.RenameDirectory(this.paths.appDataBkpProf(profNameOld), profNameNew);

                    // nmm Info
                    if (!this.paths.nmmInfoEmpty)
                    {
                        log.Info("-- renaming " + this.paths.nmmInfoBkpProf(profNameOld) + " -> " + profNameNew);
                        FileSystem.RenameDirectory(this.paths.nmmInfoBkpProf(profNameOld), profNameNew);
                    }

                    // nmm Mod
                    if (!this.paths.nmmModEmpty)
                    {
                        log.Info("-- renaming " + this.paths.nmmModBkpProf(profNameOld) + " -> " + profNameNew);
                        FileSystem.RenameDirectory(this.paths.nmmModBkpProf(profNameOld), profNameNew);
                    }
                }

                this.config.saveConfig();
                this.updateManagerState();
                return Errors.SUCCESS;
            }
            log.Warn("-- invalid profile name profNameOld:" + profNameOld);
            return Errors.ERR_INVALID_PROFILE_NAME;
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

        private string integrityFilePath()
        {
            return this.paths.steamGame + "\\" + ACTIVE_INTEGRITY_FILE_NAME;
        }

        private void undoDesactiveActivation(int moveCounter, string profName)
        {
            if (moveCounter >= 5)
            {
                log.Info("-- all moves were done rightly");
                return;
            }
            for (int i = moveCounter; i > 0; i--)
            {
                string dirDst = "";
                log.Info("-- move back " + i);
                switch (i)
                {
                    case 4:
                        {
                            if (!this.paths.nmmInfo.Trim().Equals(""))
                            {
                                log.Info("-- undo NMM Info move");
                                dirDst = this.paths.nmmInfoBkpProf(profName);
                                Utils.safeMove(this.paths.nmmInfoGame, dirDst);
                            }
                            break;
                        }
                    case 3:
                        {
                            log.Info("-- undo My Documents move");
                            dirDst = this.paths.docsBkpProf(profName);
                            Utils.safeMove(this.paths.docsGame, dirDst);
                            break;
                        }
                    case 2:
                        {
                            log.Info("-- undo AppData move");
                            dirDst = this.paths.appDataBkpProf(profName);
                            Utils.safeMove(this.paths.appDataGame, dirDst);
                            break;
                        }
                    case 1:
                        {
                            log.Info("-- undo Steam move");
                            dirDst = this.paths.steamBkpProf(profName);
                            Utils.safeMove(this.paths.steam, dirDst);
                            break;
                        }
                    default:
                        {
                            log.Warn("-- invalid undo operation");
                            break;
                        }
                }
            }
        }

        private void createBackupsFolder()
        {
            Directory.CreateDirectory(this.paths.steamBkp);
            Directory.CreateDirectory(this.paths.docsBkp);
            Directory.CreateDirectory(this.paths.appDataBkp);
            if (!this.paths.nmmInfoBkp.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.paths.nmmInfoBkp);
            }
            if (!this.paths.nmmMod.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.paths.nmmMod);
            }
        }

        private void createBackupProfilesFolder(string profName)
        {
            Directory.CreateDirectory(this.paths.steamBkpProf(profName));
            Directory.CreateDirectory(this.paths.docsBkpProf(profName));
            Directory.CreateDirectory(this.paths.appDataBkpProf(profName));
            if (!this.paths.nmmInfoBkp.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.paths.nmmInfoBkpProf(profName));
            }
            if (!this.paths.nmmMod.Trim().Equals(""))
            {
                Directory.CreateDirectory(this.paths.nmmModBkpProf(profName));
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
                File.WriteAllText(this.integrityFilePath(), prof.name + "," + prof.color);
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

        int readIntegrityFile(out string name, out string color)
        {
            name = "";
            color = "";
            try
            {
                string content = File.ReadAllText(this.integrityFilePath());
                List<string> integrityFileElements = Utils.splitCsv(content);
                try
                {
                    name = integrityFileElements[0].Trim();
                    color = integrityFileElements[1].Trim();
                }
                catch (Exception ex)
                {
                    log.Warn("-- Could nor create integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                    return Errors.ERR_PARSING_INTEGRITY_FILE;
                }
                
            }
            catch (Exception ex)
            {
                log.Warn("-- Could nor create integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_COULD_NOT_OPEN_INTEGRIY_FILE;
            }
            return Errors.SUCCESS;

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
                if (!this.checkDir(this.paths.steamGame))
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

        /// <summary>
        /// Search for profile on the Manager profile list.
        /// </summary>
        /// <param name="profName"></param>
        /// <param name="option">1 for active, 2 for desactivated, otherwise both</param>
        /// <returns></returns>
        private SPProfile searchProfile(string profName, int option)
        {
            profName.Trim();
            this.updateManagerState();
            if (option == 1)
            {
                if (this.activeProfile.name == profName)
                {
                    return activeProfile;
                }
                log.Warn("-- could not find profile " + profName + " on ACTIVE profiles");
            }
            else if (option == 2)
            {
                foreach (var item in this.listDesactivated)
                {
                    if (item.name == profName)
                    {
                        return item;
                    }
                }
                log.Warn("-- could not find profile " + profName + " on DESACTIVATED profiles");
            }
            else
            {
                if (this.activeProfile.name == profName)
                {
                    return activeProfile;
                }
                foreach (var item in this.listDesactivated)
                {
                    if (item.name == profName)
                    {
                        return item;
                    }
                }
                log.Warn("-- could not find profile " + profName + " on ACTIVE/DESACTIVATED profiles");
            }
            return null;
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
            //string currDir = Directory.GetCurrentDirectory().Trim();
            // TODO
//            if (currDir == this.config.settings.appDataPathGame() ||
//                currDir == this.config.settings.documentsPathGame() ||
//                currDir == this.config.settings.appDataPathGame() ||
//                currDir == this.config.settings.nmmInfoPathGame() ||
//                currDir == this.config.settings.nmmModPathGame())
//            {
//                log.Error("INVALID CONFIGURATION PATH!!!!");
//                return false;
//            }
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
                log.Debug("  name:" + this.activeProfile.name + ", isActive:" + this.activeProfile.isActive );
            }
            else
            {
                log.Debug("-- no active profile");
            }
            log.Debug("-- Desactivated Profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("  name:" + item.name + 
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
            //TODO
            // checar configuração
//           if (!this.checkDir(this.config.settings.appDataPathGame()) ||
//               !this.checkDir(this.config.settings.documentsPathGame()) ||
//               !this.checkDir(this.config.settings.steamPathGame()))
//           {
//               log.Warn("A game directory does not exist. appDataPathGame:{" + this.config.settings.appDataPathGame() + "}" +
//                        ", documentsPathGame:{" + this.config.settings.documentsPathGame() + "}" +
//                        ", steamPathGame:{" + this.config.settings.steamPathGame() + "}");
//               return false;
//           }
            if (!this.checkDir(this.integrityFilePath()))
            {
                log.Warn("Integrity file of active profile does not exist");
                return false;
            }
            string integrityFile = File.ReadAllText(this.integrityFilePath());
            List<string> profData = Utils.splitCsv(integrityFile);
            if (profData.Count < 2)
            {
                log.Warn("Integrity file corrupted {" + integrityFile + "}");
                return false;
            }
            if (prof.name.Trim() != profData[0].Trim())
            {
                log.Warn("Names do not match");
                return false;
            }
            if (prof.color.Trim() != profData[1].Trim())
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
// TODO
//            string desactivatedPathBase = this.backupRoot + "\\" + prof.name + "\\" + config.settings.gameFolder;
//            string steamDesPath = this.config.settings.steamPath + desactivatedPathBase;
//            string docsDesPath = this.config.settings.documentsPath + desactivatedPathBase;
//            string appDirDesPath = this.config.settings.appDataPath + desactivatedPathBase;
//            string nmmInfoDesPath = this.config.settings.nmmInfoPath + desactivatedPathBase;
//            string nmmModsDesPath = this.config.settings.nmmModPath + desactivatedPathBase;
//
//           // check mandatory paths
//           if (!this.checkDir(steamDesPath))
//           {
//               return false;
//           }
//           if (!this.checkDir(docsDesPath))
//           {
//               return false;
//           }
//           if (!this.checkDir(appDirDesPath))
//           {
//               return false;
//           }
//
//            // optional: check if is defined
//            if (!this.config.settings.nmmInfoPath.Trim().Equals(""))
//            {
//                if (!this.checkDir(nmmInfoDesPath))
//                {
//                    return false;
//                }
//            }
//            if (!this.config.settings.nmmModPath.Trim().Equals(""))
//            {
//                if (!this.checkDir(nmmModsDesPath))
//                {
//                    return false;
//                }
//            }

            log.Debug("-- profile " + prof.name + " is ok");
            return true;
        }

        #endregion checkHelpers

        #endregion private_methos

    }
}
