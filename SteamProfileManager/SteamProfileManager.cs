
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
using System.Security.Permissions;

namespace ProfileManager
{
    /** 
     * Usage:
     * ProfileManager manager = new ProfileManager(Game.SKYRIM);
     */
    public class SteamProfileManager
    {
        // readonly
        private readonly ILogger log;
        private readonly Game theGame;

        // app state
        private PathsHelper paths; // helper for generating the right names of the paths
        private SPSettings settings;
        private SPMState applicationState = SPMState.NOT_CONFIGURED;
        private SPProfile activeProfile;           // current active profile (if exist)
        private List<SPProfile> listDesactivated;  // list of desactivated profiles

        public SteamProfileManager(Game game)
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            CSharp.setLogger(Log4NetLogger.getInstance(LogAppender.APP_CORE));
            log.Info("###############################################################################");
            log.Info("# SteamProfileManager Core: " + game.ToString());
            log.Info("###############################################################################");
            log.Info("");
            log.Debug("-- load settings");
            SPConfig config = SPConfig.loadConfig();
            if (config != null)
            {
                log.Debug("-- config.selectSettings() game:" + GameToStr.str(game));
                this.settings = config.selectSettings(GameToStr.str(game));
                this.theGame = game;
                this.paths = new PathsHelper(this.theGame, this.settings);
            }
            else
            {
                log.Warn("COULD NOT LOAD CONFIGURATION FILE");
                this.settings = null;
            }
            log.Debug("-- createBackupsRoot()");
            this.createBackupsRoot();
            log.Debug("-- updateManagerState()");
            this.updateManagerState();
        }

        #region interface_methods

        public string dateFormat()
        {
            return this.settings.dateFormat;
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
            log.Debug("## manager.updateSettings() ####################################################");

            List<string> mandatoryPaths = new List<string>{
                newSteamPath,
                newDocumentsPath,
                newAppDataPath
            };
            List<string> optionalPaths = new List<string>{
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
                    mandatoryPaths.Add(item);
                }
                log.Debug("mandatoryPaths: " + CSharp.listToCsv(mandatoryPaths));
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
            this.settings.appDataPath = newAppDataPath;
            this.settings.steamPath = newSteamPath;
            this.settings.documentsPath = newDocumentsPath;
            this.settings.nmmInfoPath = nmmInfoPath;
            this.settings.nmmModPath = nmmModPath;
            

            // update path helper
            this.paths.update(this.settings);

            // create backup direcotories
            this.createBackupsRoot();

            // save settings
            SPConfig config = SPConfig.loadConfig();
            config.updateSettings(GameToStr.str(this.theGame), this.settings);
            config.saveConfig();

            // update app state
            this.applicationState = this.discoverApplicationState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Creates an active profile from existing instalation. If there is already an active profile,
        /// or if there is no installation, this method does nothing adn returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int activateInactiveProfile(string profileName, string color, string creatinDate)
        {
            log.Debug("## manager.activateInactiveProfile() ###########################################");
            log.Debug("# activateInactiveProfile() profileName:" + profileName);
            // update state
            this.updateManagerState();
            if (this.applicationState != SPMState.INACTIVE_PROFILE)
            {
                log.Debug("-- activateInactiveProfile() operation can only be executed if the application state is SPMState.INACTIVE_PROFILE");
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }

            // Create profile object
            if (profileName.Trim() == Consts.INACTIVE_NAME)
            {
                // name already in use
                return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS;
            }
            foreach (var item in this.listDesactivated)
            {
                if (profileName.Trim() == item.name.Trim())
                {
                    // name already in use
                    return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS;
                }
            }
            if (this.activeProfile != null)
            {
                if (this.activeProfile.name.Trim() == profileName.Trim())
                {
                    // name already in use
                    return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS;
                }
            }
            SPProfile newProfile = new SPProfile();
            newProfile.color = color;
            newProfile.name = profileName;
            newProfile.creationDate = creatinDate;
            newProfile.isReady = true;

            // Create integrity file 
            log.Debug("-- creating integrity file for profile " + newProfile.name);
            string errMsg = "";
            string errPath = "";
            if (!this.paths.updateActiveIntegrityFile(newProfile, out errMsg, out errPath))
            {
                log.Error("Could not create intregrity file");
                log.Error("** ERROR errMsg:"+ errMsg + ", errPath:" + errPath);
                return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE;
            }

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
            log.Debug("## manager.activateDesactivatedProfile() #######################################");
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
            string errPath = "";
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
            //this.createIntegrityFile(profToActivate);
            errMsg = "";
            errPath = "";
            if (!this.paths.updateActiveIntegrityFile(profToActivate, out errMsg, out errPath))
            {
                log.Error("Could not create intregrity file");
                log.Error("** ERROR errMsg:" + errMsg + ", errPath:" + errPath);
                return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE;
            }

            log.Debug("STEP 5: update manager state...");
            profToActivate.isReady = true;
            this.updateManagerState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int desactivateActiveProfile(string profileName)
        {
            log.Debug("## manager.desactivateActiveProfile() ##########################################");
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
            if (!this.paths.activeIntegrityFile())
            {
                log.Error("* INTEGRITY FILE FOR PROFILE " + profileName + " DOES NOT EXIST. CANNOT COMPLETE ACTION");
                return Errors.ERR_FILE_NOT_EXIST;
            }
            List<string> intItem = this.paths.activeIntegrityFileItems();
            if (intItem.Count < Consts.INTEGRITY_FILE_ITEMS)
            {
                log.Error("* INTEGRITY FILE FOR PROFILE " + profileName + " IS NOT ON THE RIGHT FORMAT");
                return Errors.ERR_ACTIVE_PROFILE_CORRUPTED;
            }
            //int ret = this.readIntegrityFile(out name, out color);
            if (intItem[0].Trim() != profileName)
            {
                log.Warn("active profile corrupted");
                this.paths.deleteActiveIntegrityFile();
                this.updateManagerState();
                return Errors.ERR_ACTIVE_PROFILE_CORRUPTED;
            }
            log.Debug("-- integrity file OK! " + id + ", " + name + ", " + color);
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
            log.Debug("## manager.switchProfile() #####################################################");
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
            this.updateManagerState();

            return Errors.SUCCESS;
        }

        /// <summary>
        /// Rename a ACTIVE or DESACTIVATED profile. Have no effect on INACTIVE profile.
        /// </summary>
        /// <param name="profNameOld"></param>
        /// <param name="profNameNew"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public int editProfile(string profNameOld, string profNameNew, string colorNew)
        {
            log.Debug("## manager.editProfile() #######################################################");
            log.Debug("# editProfile() profNameOld:" + profNameOld + ", profNameNew:" + profNameNew);
            log.Debug("* profNameOld:" + profNameOld);
            log.Debug("* profNameNew:" + profNameNew);
            log.Debug("* colorNew:" + colorNew);
            bool ret = false;
            string errMsg = "";
            string errDir = "";
            string errName = "";
            string errPath = "";
            
            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) &&
                (this.applicationState != SPMState.ACTIVE_ONLY) &&
                (this.applicationState != SPMState.DESACTIVATED_ONLY))
            {
                log.Warn("-- invalid state for requested operation editProfile. State:" + this.applicationState);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION;
            }
            int profileType = 0;
            profNameOld.Trim();
            SPProfile prof = this.searchProfile(profNameOld, 0, out profileType); // any profile
            log.Debug("- Profile-Old-Name:" + profNameOld + 
                      ", profileType:" + profileType + "(1:Active/2:Desactivated/0:NULL)");
            if (prof != null)
            {
                prof.name = profNameNew;
                prof.color = colorNew;
                // creatin date is not changed

                if (profileType == 1) // ACTIVE profile
                {
                    ret = this.paths.updateActiveIntegrityFile(prof, out errMsg, out errPath);
                    if (ret)
                    {
                        log.Debug("Success Updating integrity file");
                        log.Info("New Integrity File:{" + paths.activeIntegrityFileContent() + "}");
                        return Errors.SUCCESS;
                    }
                    else
                    {
                        log.Error("** CANNOT UPDATE ACTIVE INTEGRITY FILE");
                        log.Error("** errMsg:" + errMsg + ", errPath:" + errPath);
                        return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE;
                    }
                }
                else if (profileType == 2) // DESACTIVATED profile 
                {
                    List<string> dirsToCheck = new List<string>{
                        this.paths.steamBkpProf(profNameOld),
                        this.paths.docsBkpProf(profNameOld),
                        this.paths.appDataBkpProf(profNameOld)
                    };
                    List<string> names = new List<string>
                    {
                        profNameNew,
                        profNameNew,
                        profNameNew
                    };
                    if (this.paths.optionalAreSet())
                    {
                        dirsToCheck.Add(this.paths.nmmInfoBkpProf(profNameOld));
                        dirsToCheck.Add(this.paths.nmmModBkpProf(profNameOld));
                        names.Add(profNameNew);
                        names.Add(profNameNew);
                    }
                    errMsg = "";
                    errDir = "";
                    errName = "";
                    // OK! rename dirs
                    ret = CSharp.stackRename(dirsToCheck, names, out errMsg, out errDir, out errName);
                    if (ret)
                    {
                        // update integrity file
                        ret = this.paths.updateDesactivatedIntegrityFile(prof, out errMsg, out errPath);
                        if (ret)
                        {
                            log.Debug("Success Updating integrity file");
                            log.Info("New Integrity File:{" + paths.activeIntegrityFileContent() + "}");
                        }
                        else
                        {
                            log.Error("** CANNOT UPDATE ACTIVE INTEGRITY FILE");
                            log.Error("** errMsg:" + errMsg + ", errPath:" + errPath);
                            return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE;
                        }
                    }
                    else
                    {
                        log.Error("** ERROR RENAMING DIRECTORIES errMsg:" + errMsg +
                            ", errDir:" + errDir + ", errName:" + errName);
                        return Errors.ERR_MOVING_DIRECTORIES;
                    }
                }
                this.updateManagerState();
                return Errors.SUCCESS;
            }
            log.Warn("-- invalid profile name profNameOld:" + profNameOld);
            return Errors.ERR_INVALID_PROFILE_NAME;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
                Console.WriteLine("  name:" + item.name + ", color:" + item.color + ", creationDate:" + item.creationDate);
            }
            Console.WriteLine("# configuration file");
            string textConfig = "";
            string configPath = PathsHelper.getConfigFileName();
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

        public SPMState reloadState()
        {
            this.updateManagerState();
            log.Debug("###############################################################################");
            log.Debug("# STEAM PROFILE APP_CORE");
            log.Debug("###############################################################################");

            log.Debug("# Active profiles");
            if (this.activeProfile != null)
            {
                log.Debug("  name:" + this.activeProfile.name + ", color:" + this.activeProfile.color);
            }
            log.Debug("# Desactivated profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("  name:" + item.name + ", color:" + item.color + ", creationDate:" + item.creationDate);
            }
            log.Debug("# configuration file");
            string textConfig = "";
            string configPath = PathsHelper.getConfigFileName();
            if (File.Exists(configPath))
            {
                textConfig = File.ReadAllText(configPath);
                log.Debug(textConfig);
            }
            else
            {
                log.Debug("** ERROR!! CONFIGURATION FILE NOT FOUND!!");
            }
            log.Debug("# APPLICATION STATE:" + this.applicationState.ToString());
            return this.applicationState;
        }


        /// <summary>
        /// Kill all steam processes. Requires elevation to execute.
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public void killAllSteam()
        {
            // batch commands
            // taskkill / f / im Steam.exe
            // taskkill / f / im SteamService.exe
            // taskkill / f / im steamwebhelper.exe
            string killSteam = "taskkill /f /im Steam.exe";
            string killSteamService = "taskkill /f /im SteamService.exe";
            string killSteamHelper = "taskkill /f /im steamwebhelper.exe";
            System.Diagnostics.Process.Start("CMD.exe", killSteam);
            System.Diagnostics.Process.Start("CMD.exe", killSteamService);
            System.Diagnostics.Process.Start("CMD.exe", killSteamHelper);
        }

        #endregion interface_methods

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

        public SPProfile getInactiveProfile()
        {
            if ((this.applicationState == SPMState.NO_PROFILE) ||
                (this.applicationState == SPMState.NOT_CONFIGURED) ||
                (this.applicationState == SPMState.ACTIVE_ONLY) ||
                (this.applicationState == SPMState.ACTIVE_AND_DESACTIVATED_PROFILES))
            {
                return null;
            }
            else if (this.applicationState == SPMState.DESACTIVATED_ONLY) 
            {
                bool isInstalled = Directory.Exists(this.paths.steamGame);
                if (!isInstalled)
                {
                    return null;
                }
                else
                {
                    return new SPProfile();
                }
            }
            else // (this.applicationState == SPMState.INACTIVE_PROFILE)
            {
                return new SPProfile();
            }
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

        #region aux 

        private void createBackupsRoot()
        {
            log.Debug("-- createBackupsRoot");
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
        /// Returns the application state, and returns by references (if it exists) the 
        /// active profile (null otherwise); and the list of desactivated profiles
        /// (or a empty list otherwise).
        /// </summary>
        /// <param name="activeProfileOk"></param>
        /// <param name="desactivatedProfilesOk"></param>
        /// <returns></returns>
        private SPMState discoverApplicationState()
        {
            bool isInstalled = true;
            bool configStatus = this.checkConfig();
            if (!configStatus)
            {
                // settings are not ok
                return SPMState.NOT_CONFIGURED;
            }
            else
            {
                // load desactivated installations
                this.listDesactivated = SPProfile.loadDesactivatedProfiles(this.paths.steamBkp,
                    this.settings.gameFolder);
                // load active instalations
                if (!Directory.Exists(this.paths.steamGame))
                {
                    this.activeProfile = null;
                    isInstalled = false;
                }
                else
                {
                    this.activeProfile = SPProfile.loadActivatedProfile(this.paths.steamGame);

                }
            }
            if (!isInstalled)
            {
                if (this.listDesactivated.Count == 0)
                {
                    return SPMState.NO_PROFILE;
                }
                else
                {
                    return SPMState.DESACTIVATED_ONLY;
                }
            }
            else
            {
                if (!this.activeProfile.isReady)
                {
                    return SPMState.INACTIVE_PROFILE;
                }
                else if (this.listDesactivated.Count == 0)
                {
                    return SPMState.ACTIVE_ONLY;
                }
                else // if(this.listDesactivated.Count > 0)
                {
                    return SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                }
            }
        }

        /// <summary>
        /// Search for profile on the Manager profile list.
        /// </summary>
        /// <param name="profName"></param>
        /// <param name="option">1 for active, 2 for desactivated, otherwise both</param>
        /// <param name="profType">1 for active, 2 for desactivated, 0 if does not exit</param>
        /// <returns></returns>
        private SPProfile searchProfile(string profName, int option, out int profType)
        {
            profName.Trim();
            this.updateManagerState();
            if (option == 1)
            {
                if (this.activeProfile.name == profName)
                {
                    profType = 1;
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
                        profType = 2;
                        return item;
                    }
                }
                log.Warn("** could not find profile " + profName + " on DESACTIVATED profiles");
            }
            else
            {
                if (this.activeProfile.name == profName)
                {
                    profType = 1;
                    return activeProfile;
                }
                foreach (var item in this.listDesactivated)
                {
                    if (item.name == profName)
                    {
                        profType = 2;
                        return item;
                    }
                }
                log.Warn("-- could not find profile " + profName + " on ACTIVE/DESACTIVATED profiles");
            }
            profType = 0;
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
            if (this.settings == null)
            {
                log.Error("this.config.settings object not initialized!");
                return false;
            }
            else if (this.settings.appDataPath == null || this.settings.appDataPath.Trim().Equals("") ||
                     this.settings.documentsPath == null || this.settings.documentsPath.Trim().Equals("") ||
                     this.settings.steamPath == null || this.settings.steamPath.Trim().Equals("") ||
                     this.settings.gameFolder == null || this.settings.gameFolder.Trim().Equals("") ||
                     this.settings.nmmInfoPath == null || this.settings.nmmModPath == null)
            {
                log.Warn("Invalid SP settings, settings must be initialized before used");
                return false;
            }
            else if (!Directory.Exists(this.settings.appDataPath))
            {
                log.Warn("* DIRECTORY " + this.settings.appDataPath + " DOES NOT EXIT!");
                return false;
            }
            else if (!Directory.Exists(this.settings.documentsPath))
            {
                log.Warn("* DIRECTORY " + this.settings.documentsPath + " DOES NOT EXIT!");
                return false;
            }
            else if (!Directory.Exists(this.settings.steamPath))
            {
                log.Warn("* DIRECTORY " + this.settings.steamPath + " DOES NOT EXIT!");
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
            this.applicationState = this.discoverApplicationState();
            // log statte
            log.Debug("State: " + this.applicationState.ToString());
            log.Debug("Active Profile");
            if (this.activeProfile != null)
            {
                log.Debug("  name:" + this.activeProfile.name + ", isActive:" + this.activeProfile.isReady );
            }
            else
            {
                log.Debug("no active profile");
            }
            log.Debug("Desactivated Profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("  name:" + item.name + 
                          ", isActive:" + item.isReady);
            }
        }

        #endregion checkHelpers

        #endregion private_methos

    }
}
