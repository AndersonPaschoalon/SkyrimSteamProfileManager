
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using ProfileManager.Objects;
using ProfileManager.Enum;
using Utils;
using Utils.Loggers;

namespace ProfileManager
{
    /** 
     * Usage:
     * ProfileManager manager = new ProfileManager(Game.SKYRIM);
     */
    public class SteamProfileManager
    {
        // readonly
        private  readonly ILogger log;

        // app state
        private PathsHelper paths;
        private SPConfig config;
        private SPMState applicationState = SPMState.NOT_CONFIGURED;
        private SPProfile activeProfile;
        private List<SPProfile> listDesactivated;

        public SteamProfileManager(Game game)
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            CSharp.setLogger(Log4NetLogger.getInstance(LogAppender.APP_CORE));
            log.Info("###############################################################################");
            log.Info("# SteamProfileManager Core: " + game.ToString());
            log.Info("###############################################################################");
            log.Info("");
            log.Debug("-- load settings");
            this.config = SPConfig.loadConfig();
            this.paths = new PathsHelper(game, this.config.settings);
            log.Debug("-- updateManagerState()");
            this.updateManagerState();
        }

        public SPMState showState()
        {
            this.updateManagerState();
            Console.WriteLine("###############################################################################");
            Console.WriteLine("# STEAM PROFILE APP_CORE");
            Console.WriteLine("###############################################################################");

            Console.WriteLine("# Active profiles");
            if (this.activeProfile != null)
            {
                Console.WriteLine("  name:" + this.activeProfile.name + ", color:" + this.activeProfile.color);
            }
            Console.WriteLine("# Desactivated profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                Console.WriteLine("  name:" + item.name + ", color:" + item.color);

            }
            Console.WriteLine("# configuration file");
            string textConfig = "";
            string configPath = paths.getConfigFilePath();
            if (File.Exists(configPath))
            {
                textConfig = File.ReadAllText(configPath);
                Console.WriteLine(textConfig);
            }
            else
            {
                Console.WriteLine("** ERROR!! CONFIGURATION FILE NOT FOUND!!");
            }
            Console.WriteLine("# APPLICATION STATE:" + this.applicationState.ToString());
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
                                  string nmmInfoPath, string nmmModPath)
        {
            log.Debug("-- updateSettings() ");

            string[] mandatoryPaths = {
                newSteamPath,
                newDocumentsPath,
                newAppDataPath
            };
            string[] optionalPaths = {
                nmmInfoPath,
                nmmModPath
            };

            // check if any optional is not null (this should not be null)
            bool optionalNotNull = CSharp.checkNotNull(optionalPaths);
            if (!optionalNotNull)
            {
                log.Warn("OPTIONAL FIELDS ARE NULL");
                return Errors.ERR_INVALID_SETTINGS;
            }
            // check if any optional is not empty
            bool optionalNotEmpty = CSharp.checkNotEmpty(optionalPaths);
            if (optionalNotEmpty)
            {
                // once they are not empty, they become mandatory
                foreach (var item in optionalPaths)
                {
                    mandatoryPaths.Append(item);
                }
                log.Debug("mandatoryPaths: " + CSharp.arrayToCsv(mandatoryPaths));
            }
            else
            {
                log.Info("Optional are EMPTY");
            }
            // check if mandatory are valid directories
            string errString = "";
            if (!CSharp.checkDirs(mandatoryPaths, out errString))
            {
                log.Warn("** ONE OF THE MANDATORY PATHS DOES NOT EXIST! ERR:" + errString);
                return Errors.ERR_INVALID_SETTINGS;
            }
            else
            {
                log.Debug("all mandatory paths are ok");
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
            log.Debug("# activateInactiveProfile() profileName:" + profileName);
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
            newProfile.isActive = CSharp.TRUE;
            newProfile.name = profileName;

            // Create integrity file 
            log.Debug("-- creating integrity file for profile " + newProfile.name);
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
            log.Debug("# activateDesactivatedProfile() profileName:" + profileName);
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
            string[] sourceDirs = { 
                    this.paths.steamBkpProfGame(profileName),      // steam
                    this.paths.appDataBkpProfGame(profileName),    // appdata
                    this.paths.docsBkpProfGame(profileName),       // docs
                    this.paths.nmmInfoBkpProfGame(profileName),    //
                    this.paths.nmmModBkpProfGame(profileName)      //
            };
            string[] destinationDirs = {
                this.paths.steam,
                this.paths.appData,
                this.paths.docs,
                this.paths.nmmInfo,
                this.paths.nmmMod
            };
            string errMsg = "";
            string errSrcDir = "";
            string errDstDir = "";
            bool sucess = CSharp.stackMv(sourceDirs, destinationDirs, true, LogMethod.LOGGER, 
                                         out errMsg, out errSrcDir, out errDstDir);
            if (!sucess)
            {
                log.Error("** errMsg: " + errMsg);
                log.Error("** errSrcDir:" + errSrcDir);
                log.Error("** errDstDir: " + errDstDir);
                return Errors.ERR_MOVING_DIRECTORIES;
            }

            log.Debug("STEP 4: create integrity file...");
            this.createIntegrityFile(profToActivate);

            log.Debug("STEP 5: update manager state...");
            profToActivate.isActive = CSharp.TRUE;
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
            log.Debug("# desactivateActiveProfile() profileName:" + profileName);
            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) &&
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
            // this.deleteIntegrityFile();

            // create backup dir if does not exit
            this.createBackupProfilesFolder(profileName);

            // move directories to backup dir   sourceDirs
            string[] destinationDirs = {
                    this.paths.steamBkpProf(profileName),      // steam
                    this.paths.appDataBkpProf(profileName),    // appdata
                    this.paths.docsBkpProf(profileName),       // docs
                    this.paths.nmmInfoBkpProf(profileName),    //
                    this.paths.nmmModBkpProf(profileName)      //
            };
            string[] sourceDirs = {
                this.paths.steamGame,
                this.paths.appDataGame,
                this.paths.docsGame,
                this.paths.nmmInfoGame,
                this.paths.nmmModGame
            };
            string errMsg = "";
            string errSrcDir = "";
            string errDstDir = "";
            bool sucess = CSharp.stackMv(sourceDirs, destinationDirs, true, LogMethod.LOGGER,
                                         out errMsg, out errSrcDir, out errDstDir);
            if (!sucess)
            {
                log.Error("** errMsg: " + errMsg);
                log.Error("** errSrcDir:" + errSrcDir);
                log.Error("** errDstDir: " + errDstDir);
                return Errors.ERR_MOVING_DIRECTORIES;
            }

            // set profile as desativated
            foreach (var item in this.config.listProfiles.profiles)
            {
                if (item.name.Trim() == profileName)
                {
                    item.isActive = CSharp.FALSE;
                    break;
                }
            }

            // save settings
            this.config.saveConfig();
            
            return Errors.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activeProf"></param>
        /// <param name="desactivatedProf"></param>
        /// <returns></returns>
        public int switchProfile(string activeProf, string desactivatedProf)
        {
            log.Debug("# switchProfile() activeProf:" + activeProf + ", desactivatedProf:" + desactivatedProf);
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
            log.Debug("# editProfile() profNameOld:" + profNameOld + ", profNameNew:" + profNameNew);
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

                    if (!this.paths.optionalAreSet())
                    {
                        // nmm Info
                        log.Info("-- renaming " + this.paths.nmmInfoBkpProf(profNameOld) + " -> " + profNameNew);
                        FileSystem.RenameDirectory(this.paths.nmmInfoBkpProf(profNameOld), profNameNew);
                        // nmm Mod
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

        //private bool checkDir(string dir)
        //{
        //    if (!Directory.Exists(dir))
        //    {
        //        log.Warn("Directory " + dir + " does not exist.");
        //        return false;
        //    }
        //    return true;
        //}

        #endregion utils 

        #region aux 

        private string integrityFilePath()
        {
            return this.paths.steamGame + "\\" + Consts.ACTIVE_INTEGRITY_FILE_NAME;
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
                                CSharp.safeMove(this.paths.nmmInfoGame, dirDst);
                            }
                            break;
                        }
                    case 3:
                        {
                            log.Info("-- undo My Documents move");
                            dirDst = this.paths.docsBkpProf(profName);
                            CSharp.safeMove(this.paths.docsGame, dirDst);
                            break;
                        }
                    case 2:
                        {
                            log.Info("-- undo AppData move");
                            dirDst = this.paths.appDataBkpProf(profName);
                            CSharp.safeMove(this.paths.appDataGame, dirDst);
                            break;
                        }
                    case 1:
                        {
                            log.Info("-- undo Steam move");
                            dirDst = this.paths.steamBkpProf(profName);
                            CSharp.safeMove(this.paths.steam, dirDst);
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
            if (this.paths.optionalAreSet())
            {
                log.Debug("optional fields are set");
                log.Debug("creating directory this.paths.nmmInfoBkp:" + this.paths.nmmInfoBkp);
                Directory.CreateDirectory(this.paths.nmmInfoBkp);
                log.Debug("creating directory this.paths.nmmMod:" + this.paths.nmmMod);
                Directory.CreateDirectory(this.paths.nmmMod);
            }
        }

        private void createBackupProfilesFolder(string profName)
        {
            Directory.CreateDirectory(this.paths.steamBkpProf(profName));
            Directory.CreateDirectory(this.paths.docsBkpProf(profName));
            Directory.CreateDirectory(this.paths.appDataBkpProf(profName));
            if (this.paths.optionalAreSet())
            {
                log.Debug("optional fields are set");
                log.Debug("creating directory this.paths.nmmInfoBkpProf(profName):" + 
                          this.paths.nmmInfoBkpProf(profName));
                Directory.CreateDirectory(this.paths.nmmInfoBkpProf(profName));
                log.Debug("creating directory this.paths.nmmModBkpProf(profName):" + 
                          this.paths.nmmModBkpProf(profName));
                Directory.CreateDirectory(this.paths.nmmModBkpProf(profName));
            }
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
                string integrityFilePath = this.integrityFilePath();
                log.Debug("integrityFilePath:" + integrityFilePath);
                File.WriteAllText(integrityFilePath, prof.name + "," + prof.color);
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
                List<string> integrityFileElements = CSharp.csvToList(content);
                try
                {
                    name = integrityFileElements[0].Trim();
                    color = integrityFileElements[1].Trim();
                }
                catch (Exception ex)
                {
                    log.Warn("** Could nor create integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                    return Errors.ERR_PARSING_INTEGRITY_FILE;
                }
                
            }
            catch (Exception ex)
            {
                log.Warn("** Could nor create integrity file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
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
                if (!Directory.Exists(this.paths.steamGame))
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
                log.Warn("** could not find profile " + profName + " on DESACTIVATED profiles");
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
                     this.config.settings.steamPath == null || this.config.settings.steamPath.Trim().Equals("") ||
                     this.config.settings.gameFolder == null || this.config.settings.gameFolder.Trim().Equals("") ||
                     this.config.settings.nmmInfoPath == null || this.config.settings.nmmModPath == null)
            {
                log.Warn("Invalid SP settings, settings must be initialized before used");
                return false;
            }
            else if (!Directory.Exists(this.config.settings.appDataPath))
            {
                return false;
            }
            else if (!Directory.Exists(this.config.settings.documentsPath))
            {
                return false;
            }
            else if (!Directory.Exists(this.config.settings.steamPath))
            {
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
            log.Debug("State: " + this.applicationState.ToString());
            log.Debug("Active Profile");
            if (this.activeProfile != null)
            {
                log.Debug("  name:" + this.activeProfile.name + ", isActive:" + this.activeProfile.isActive );
            }
            else
            {
                log.Debug("no active profile");
            }
            log.Debug("Desactivated Profiles [Count:" + this.listDesactivated.Count + "]");
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
                log.Warn("* Object property isActive is set as false or invalid");
                return false;
            }
            // check integrity file
            if (!File.Exists(this.integrityFilePath()))
            {
                log.Warn("* Integrity file of active profile does not exist");
                return false;

            }
            string integrityFile = File.ReadAllText(this.integrityFilePath());
            List<string> profData = CSharp.csvToList(integrityFile);
            if (profData.Count < 2)
            {
                log.Warn("* Integrity file CORRUPTED {" + integrityFile + "}");
                log.Info("-- Profile " + prof.name + " is not ACTIVE");
                return false;
            }
            if (prof.name.Trim() != profData[0].Trim())
            {
                log.Warn("* Names from integrity file [" + profData[0] + "] and profile [" + prof.name + "] do not match");
                log.Info("-- Profile " + prof.name + " is not ACTIVE");
                return false;
            }
            if (prof.color.Trim() != profData[1].Trim())
            {
                log.Warn("* Colors do not match! prof.color:" + prof.color.Trim() + ",  (integrity file)profData[1]:" + profData[1].Trim());
            }
            return true;
        }

        /// <summary>
        /// Check all settings of a Desactivated profile. If some settings are inconsistent
        /// with an desactivated profile, returns true.
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        private bool checkDesactivatedProfile(SPProfile prof)
        {
            log.Debug("checkDesactivatedProfile:" + prof.name);
            if (prof.isProfileActive())
            {
                // profile is set as activated
                return false;
            }
            string profName = prof.name;
            string[] dirsToCheck = {
                this.paths.steamBkpProf(profName),
                this.paths.steamBkpProfGame(profName),
                this.paths.docsBkpProf(profName),
                this.paths.docsBkpProfGame(profName),
                this.paths.appDataBkpProf(profName),
                this.paths.appDataBkpProfGame(profName),
            };
            if (!this.paths.optionalAreSet())
            {
                dirsToCheck.Append(this.paths.nmmInfoBkpProf(profName));
                dirsToCheck.Append(this.paths.nmmInfoBkpProfGame(profName));
                dirsToCheck.Append(this.paths.nmmModBkpProf(profName));
                dirsToCheck.Append(this.paths.nmmModBkpProfGame(profName));
            }
            log.Debug("checking desactivated paths");
            string errDir = "";
            bool checkDir = CSharp.checkDirs(dirsToCheck, out errDir);
            if (!checkDir)
            {
                log.Error("** DIRECTORY NOT FOUND: " + errDir);
                return false;
            }

            log.Debug("profile " + prof.name + " is ok");
            return true;
        }

        #endregion checkHelpers

        #endregion private_methos

    }
}
