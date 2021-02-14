
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using ProfileManager.Enum;
using Utils;
using Utils.Loggers;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Diagnostics;
using SpearSettings;

namespace ProfileManager
{
    /** 
     * Usage:
     * ProfileManager manager = new ProfileManager(Game.SKYRIM);
     */
    public class SteamProfileManager
    {
        // readonly
        private readonly ILogger              log;
        private readonly string               theGame;
        private readonly PathsHelper          paths; // helper for generating the right names of the paths
        private readonly IntegrityFileHandler integrityFile;
        // app state
        private SPMState        applicationState = SPMState.NOT_CONFIGURED;
        private SPProfile       activeProfile;      // current active profile (if exist)
        private List<SPProfile> listDesactivated;   // list of desactivated profiles

        public SteamProfileManager(string gameName)
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            CSharp.setLogger(Log4NetLogger.getInstance(LogAppender.APP_CORE));
            log.Info("###############################################################################");
            log.Info("# SteamProfileManager Core: " + gameName);
            log.Info("###############################################################################");
            log.Info("");
            log.Debug("-- load settings");

            if (SettingsFactory.gameExist(gameName))
            {
                this.theGame = gameName;
                log.Debug("-- config.selectSettings() game:" + this.theGame);
                this.paths = SettingsFactory.getPathsHelper(this.theGame);
                this.integrityFile = new IntegrityFileHandler(this.paths);
            }
            this.createBackupsRoot();
            this.updateManagerState();
        }

        #region interface_methods

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
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_1;
            }

            // Create profile object
            if (profileName.Trim() == Consts.INACTIVE_NAME)
            {
                // name already in use
                return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS_1;
            }
            foreach (var item in this.listDesactivated)
            {
                if (profileName.Trim() == item.name.Trim())
                {
                    // name already in use
                    return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS_2;
                }
            }
            if (this.activeProfile != null)
            {
                if (this.activeProfile.name.Trim() == profileName.Trim())
                {
                    // name already in use
                    return Errors.ERR_PROFILE_NAME_ALREADY_EXISTS_3;
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
            //if (!this.paths.updateActiveIntegrityFile(newProfile, out errMsg, out errPath))
            if (!this.integrityFile.updateActiveIntegrityFile(newProfile, out errMsg, out errPath))
            {
                log.Error("Could not create intregrity file");
                log.Error("** ERROR errMsg:"+ errMsg + ", errPath:" + errPath);
                return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE_1;
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
        public int activateDesactivatedProfile(string profileName, out string outErrMsg)
        {
            log.Debug("## manager.activateDesactivatedProfile() #######################################");
            log.Debug("# activateDesactivatedProfile() profileName:" + profileName);
            log.Debug("STEP 1: Check Settings...");
            this.updateManagerState();
            if (this.applicationState != SPMState.DESACTIVATED_ONLY)
            {
                outErrMsg = "Invalid state for requested operation. This operation may only be completed if the aplication state is SPMState.DESACTIVATED_ONLY.";
                log.Warn(" -- " + outErrMsg);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_5;
            }
            profileName = profileName.Trim();
            if (profileName == null || profileName.Equals(""))
            {
                outErrMsg = "profile name is empty";
                log.Warn(" -- " + outErrMsg);
                return Errors.ERR_INVALID_PROFILE_NAME_1;
            }

            log.Debug("STEP 2: search profile to activate...");
            SPProfile profToActivate = null;
            foreach (var item in this.listDesactivated)
            {
                if (item.name.Trim() == profileName.Trim())
                {
                    profToActivate = item;
                    break;
                }
            }
            if (profToActivate == null)
            {
                outErrMsg = "Specified profile could not be found, ERR_INVALID_PROFILE_NAME";
                log.Warn(" -- " + outErrMsg);
                return Errors.ERR_INVALID_PROFILE_NAME_2;
            }

            log.Debug("STEP 3: moving folders from backup to root dir...");
            ListPaths listPathsDst = this.paths.getAllPaths_App();
            ListPaths listPathsSrc = this.paths.getAllPaths_BkpProfGame(profileName);
            // check consinstency
            string errLabel1 = "";
            string errLabel2 = "";
            if (!listPathsSrc.checkLabels(listPathsDst, out errLabel1, out errLabel2))
            {
                outErrMsg = "sourceType: " + errLabel1 + ", destination:" + errLabel2;
                log.Error(" ** Source and destination folder are mislabeled!!");
                log.Error(" ** errLabel1:" + errLabel1);
                log.Error(" ** errLabel2:" + errLabel2);
                log.Error(" ** outErrMsg:" + outErrMsg);
                return Errors.ERR_SOURCE_DESTINATION_DONT_MATCH_ACTIVATEDESACTIVATED;
            }
            string[] destinationDirs = listPathsDst.vecPaths;
            string[] sourceDirs = listPathsSrc.vecPaths;
            // this.paths.steam,
            //string[] destinationDirs = this.paths.getAllPaths_App().vecPaths;
            // this.paths.steamBkpProfGame(profileName),
            //string[] sourceDirs = this.paths.getAllPaths_BkpProfGame(profileName).vecPaths;
            /*
            if (this.paths.optionalAreSet())
            {
                destinationDirs = new string[]  {
                    this.paths.steam,
                    this.paths.appData,
                    this.paths.docs,
                    this.paths.nmm
                };
                sourceDirs = new string[] {
                    this.paths.steamBkpProfGame(profileName),      // steam
                    this.paths.appDataBkpProfGame(profileName),    // appdata
                    this.paths.docsBkpProfGame(profileName),       // docs
                    this.paths.nmmBkpProfGame(profileName)
                };
            }
            else
            {
                log.Info(" -- Optional dirs are not set. They will not be moved.");
                destinationDirs = new string[]  {
                    this.paths.steam,
                    this.paths.appData,
                    this.paths.docs
                };
                sourceDirs = new string[] {
                    this.paths.steamBkpProfGame(profileName),      // steam
                    this.paths.appDataBkpProfGame(profileName),    // appdata
                    this.paths.docsBkpProfGame(profileName)        // docs
                };
            }
            */
            string errMsg = "";
            string errSrcDir = "";
            string errDstDir = "";
            string errPath = "";
            bool sucess = CSharp.stackMv(sourceDirs, destinationDirs, true, LogMethod.LOGGER, 
                                         out errMsg, out errSrcDir, out errDstDir);
            if (!sucess)
            {
                outErrMsg = "Error moving directories: SrcDir<" + errSrcDir + ">, DstDir<" + errDstDir + ">. Error Message:" + errMsg;
                log.Error(" ** " + outErrMsg);
                log.Error(" ** errMsg: " + errMsg);
                log.Error(" ** errSrcDir:" + errSrcDir);
                log.Error(" ** errDstDir: " + errDstDir);
                return Errors.ERR_MOVING_DIRECTORIES_1;
            }

            log.Debug("STEP 4: create integrity file...");
            errMsg = "";
            errPath = "";
            //if (!this.paths.updateActiveIntegrityFile(profToActivate, out errMsg, out errPath))
            if (!this.integrityFile.updateActiveIntegrityFile(profToActivate, out errMsg, out errPath))
            {
                outErrMsg = "Could not create intregrity file. Message:"  + errMsg + ". Error Path:" + errPath;
                log.Error(" ** outErrMsg:" + outErrMsg);
                log.Error(" ** ERROR errMsg:" + errMsg + ", errPath:" + errPath);
                return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE_2;
            }

            log.Debug("STEP 5: update manager state...");
            profToActivate.isReady = true;
            this.updateManagerState();

            outErrMsg = "";
            return Errors.SUCCESS;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int desactivateActiveProfile(string profileName, out string outMsg)
        {
            log.Debug("## manager.desactivateActiveProfile() ##########################################");
            log.Debug("# desactivateActiveProfile() profileName:" + profileName);
            int ret = Errors.SUCCESS;
            string id = "";
            string name = "";
            string color = "";
            string errMsg = "";
            bool retVal = false;

            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) &&
                (this.applicationState != SPMState.ACTIVE_ONLY))
            {
                outMsg = "Invalid state for desactivateActiveProfile operation! State:" + this.applicationState;
                log.Error(" ** " + outMsg);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_2;
            }

            // check if integrity file information matches
            profileName.Trim();
            if (!this.integrityFile.activeIntegrityFileExists())
                {
                outMsg = "INTEGRITY FILE FOR PROFILE " + profileName + " DOES NOT EXIST. CANNOT COMPLETE ACTION.";
                log.Error(" ** " + outMsg);
                return Errors.ERR_FILE_NOT_EXIST;
            }
            List<string> intItem = this.integrityFile.activeIntegrityFileItems();
            if (intItem.Count < Consts.INTEGRITY_FILE_ITEMS)
            {
                outMsg = "INTEGRITY FILE FOR PROFILE " + profileName + " IS NOT ON THE RIGHT FORMAT.";
                log.Error(" ** " + outMsg);
                return Errors.ERR_ACTIVE_PROFILE_CORRUPTED_1;
            }
            if (intItem[0].Trim() != profileName)
            {
                outMsg = "Active profile corrupted. Error parsing integrity file.";
                log.Warn(" ** " + outMsg);
                retVal = this.integrityFile.deleteActiveIntegrityFile(out errMsg);
                if (!retVal)
                {
                    log.Error(" ** ERROR @ this.integrityFile.deleteActiveIntegrityFile(). Could not delete integrity file. Message: " + errMsg);
                }
                this.updateManagerState();
                return Errors.ERR_ACTIVE_PROFILE_CORRUPTED_2;
            }
            log.Debug(" -- integrity file OK! " + id + ", " + name + ", " + color);

            // create backup dir if does not exit
            ret = this.createBackupProfilesFolder(profileName, out errMsg);
            if (ret != Errors.SUCCESS)
            {
                outMsg = "Could not create backup profile Folder: " + errMsg;
                log.Error(" ** " + outMsg);
                return ret;
            }
            log.Debug(" -- createBackupProfilesFolder OK!");

            ListPaths listPathsDst = this.paths.getAllPaths_BkpProf(profileName);
            ListPaths listPathsSrc = this.paths.getAllPaths_AppGame();
            // check consinstency
            string errLabel1 = "";
            string errLabel2 = "";
            if (!listPathsSrc.checkLabels(listPathsDst, out errLabel1, out errLabel2))
            {
                outMsg = "sourceType: " + errLabel1 + ", destination:" + errLabel2;
                log.Error(" ** Source and destination folder are mislabeled!!");
                log.Error(" ** errLabel1:" + errLabel1);
                log.Error(" ** errLabel2:" + errLabel2);
                log.Error(" ** outErrMsg:" + outMsg);
                return Errors.ERR_SOURCE_DESTINATION_DONT_MATCH_DESACTIVATEACTIVE;
            }
            string[] destinationDirs = listPathsDst.vecPaths;
            string[] sourceDirs = listPathsSrc.vecPaths;

            /*
            // move directories to backup dir   sourceDirs
            string[] destinationDirs = this.paths.getAllPaths_BkpProf(profileName).vecPaths;
            string[] sourceDirs = this.paths.getAllPaths_AppGame().vecPaths;
            // string[] destinationDirs = this.paths.getAllPaths_BkpProf(profileName).ToArray();
            // string[] sourceDirs = this.paths.getAllPaths_AppGame().ToArray();
            if (destinationDirs.Length != sourceDirs.Length)
            {
                outMsg = "List of source and destination directories are inconsistent.";
                log.Error(" ** Error: " + outMsg);
                log.Error(" ** destinationDirs.Length:" + destinationDirs.Length + ", sourceDirs.Length:" + sourceDirs.Length);
                log.Error(" ** Source      Dirs:" + CSharp.arrayToCsv(sourceDirs));
                log.Error(" ** Destination Dirs:" + CSharp.arrayToCsv(destinationDirs));
                return Errors.ERR_INCONSISTENT_SRC_DST_DIR_NUMBER;
            }
            */

            string errSrcDir = "";
            string errDstDir = "";
            bool sucess = CSharp.stackMv(sourceDirs, destinationDirs, true, LogMethod.LOGGER,
                                         out errMsg, out errSrcDir, out errDstDir);
            if (!sucess)
            {
                outMsg = errMsg + " errSrcDir:<" + errSrcDir + ">" + ", errDstDir:<" + errDstDir + ">";
                log.Error("** outMsg: " + outMsg);
                log.Error("** errMsg: " + errMsg);
                log.Error("** errSrcDir:" + errSrcDir);
                log.Error("** errDstDir: " + errDstDir);
                return Errors.ERR_MOVING_DIRECTORIES_2;
            }
            outMsg = "";
            return Errors.SUCCESS;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activeProf"></param>
        /// <param name="desactivatedProf"></param>
        /// <returns></returns>
        public int switchProfile(string activeProf, string desactivatedProf, out string errMsg)
        {
            int ret;
            int ret2 = 0;
            string errMsg2 = "";
            log.Debug("## manager.switchProfile() #####################################################");
            log.Debug("# switchProfile() activeProf:" + activeProf + ", desactivatedProf:" + desactivatedProf);

            // check state
            this.updateManagerState();
            if (this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES)
            {
                errMsg = "Switch Error: Invalid state for desactivateActiveProfile operation! State:" + this.applicationState;
                log.Error(" ** Error after this.updateManagerState()");
                log.Error(" ** " + errMsg);
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_3;
            }

            // desactivate profile
            activeProf.Trim();
            desactivatedProf.Trim();
            ret = this.desactivateActiveProfile(activeProf, out errMsg);
            if (ret != Errors.SUCCESS)
            {
                errMsg = "Switch Error: " + errMsg;
                log.Error(" ** Error @ this.desactivateActiveProfile()" + ", ret:" + ret);
                log.Error(" ** errMsg:" + errMsg);
                log.Error(" ** Error desactivating profile activeProf:" + activeProf + ", ret:" + ret);
                return ret;
            }

            // activate profile
            ret = this.activateDesactivatedProfile(desactivatedProf, out errMsg);
            if (ret != Errors.SUCCESS)
            {
                errMsg = "Switch Error: " + errMsg;
                log.Error(" ** Error @this.activateDesactivatedProfile()" + ", ret:" + ret);
                log.Error(" ** " + errMsg + ", ret:" +  ret);
                log.Info(" -- UNDOING THE LAST DESACTIVATION!!");
                ret2 = this.activateDesactivatedProfile(activeProf, out errMsg2);
                log.Info(" -- errMsg2:" + errMsg2 + ", ret2:" + ret2);
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
        public int editProfile(string profNameOld, string profNameNew, string colorNew, out string errMsgRet)
        {
            bool ret = false;
            string errMsg = "";
            string errDir = "";
            string errName = "";
            string errPath = "";

            log.Debug("## manager.editProfile() #######################################################");
            log.Debug("# editProfile() profNameOld:" + profNameOld + ", profNameNew:" + profNameNew);
            log.Debug("* profNameOld:" + profNameOld);
            log.Debug("* profNameNew:" + profNameNew);
            log.Debug("* colorNew:" + colorNew);

            // check state
            this.updateManagerState();
            if ((this.applicationState != SPMState.ACTIVE_AND_DESACTIVATED_PROFILES) &&
                (this.applicationState != SPMState.ACTIVE_ONLY) &&
                (this.applicationState != SPMState.DESACTIVATED_ONLY))
            {
                log.Warn("-- invalid state for requested operation editProfile. State:" + this.applicationState);
                errMsgRet = "Invalid state for Edit operation. State:" + this.applicationState;
                return Errors.ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_4;
            }

            profNameNew = this.safeNewName(profNameNew);
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
                    ret = this.integrityFile.updateActiveIntegrityFile(prof, out errMsg, out errPath);
                    if (ret)
                    {
                        log.Info("Success Updating integrity file! New Integrity File:{" + this.integrityFile.activeIntegrityFileContent() + "}");
                        errMsgRet = "SUCCESS";
                        return Errors.SUCCESS;
                    }
                    else
                    {
                        log.Error("** CANNOT UPDATE ACTIVE INTEGRITY FILE");
                        log.Error("** errMsg:" + errMsg + ", errPath:" + errPath);
                        errMsgRet = "SUCCESS";
                        return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE_3;
                    }
                }
                else if (profileType == 2) // DESACTIVATED profile 
                {
                    List<string> dirsToCheck = this.paths.getAllPaths_BkpProf(profNameOld).listPaths;
                    List<string> names = new List<string>();
                    int nDirs = dirsToCheck.Count;
                    for (int i = 0; i < nDirs; i++)
                    {
                        names.Add(profNameNew);
                    }
                    log.Debug(" -- editProfile for DESACTIVATED profile ");
                    log.Debug(" -- dirsToCheck:{" + CSharp.listToCsv(dirsToCheck) + "}");
                    log.Debug(" -- names:{" + CSharp.listToCsv(names) + "}");

                    /*
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
                        dirsToCheck.Add(this.paths.nmmBkpProf(profNameOld));
                        names.Add(profNameNew);
                        names.Add(profNameNew);
                    }
                    */
                    errMsg = "";
                    errDir = "";
                    errName = "";
                    // OK! rename dirs
                    ret = CSharp.stackRename(dirsToCheck, names, out errMsg, out errDir, out errName);
                    if (ret)
                    {
                        // update integrity file
                        ret = this.integrityFile.updateDesactivatedIntegrityFile(prof, out errMsg, out errPath);
                        if (ret)
                        {
                            log.Info("Success Updating integrity file! New Integrity File:{" + this.integrityFile.activeIntegrityFileContent() + "}");
                        }
                        else
                        {
                            log.Error("** CANNOT UPDATE ACTIVE INTEGRITY FILE");
                            log.Error("** errMsg:" + errMsg + ", errPath:" + errPath);
                            errMsgRet = errMsg;
                            return Errors.ERR_CANNOT_CREATE_INTEGRITY_FILE_4;
                        }
                    }
                    else
                    {
                        log.Error("** ERROR RENAMING DIRECTORIES errMsg:" + errMsg +
                            ", errDir:" + errDir + ", errName:" + errName);
                        errMsgRet = errMsg;
                        return Errors.ERR_MOVING_DIRECTORIES_3;
                    }
                }
                this.updateManagerState();
                errMsgRet = "SUCCESS";
                return Errors.SUCCESS;
            }
            log.Warn("-- invalid profile name profNameOld:" + profNameOld);
            errMsgRet = "SUCCESS";
            return Errors.ERR_INVALID_PROFILE_NAME_3;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SPMState reloadState()
        {
            log.Debug(" -- reloadState");
            this.updateManagerState();
            if (this.activeProfile != null)
            {
                log.Debug("Active profiles =>  name:" + this.activeProfile.name + ", color:" + this.activeProfile.color);
            }
            log.Debug("Desactivated profiles [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("    name:" + item.name + ", color:" + item.color + ", creationDate:" + item.creationDate);
            }
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
            log.Info("# APPLICATION STATE:" + this.applicationState.ToString());
            return this.applicationState;
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

        // ok
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
            else
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

        // Backup folder sided with the instalation folders. Ex: for an skyrim instalation 
        // root folder: ".\Steam\" 
        // backup root: ".\Steam\SkyrimBackups\"
        private void createBackupsRoot()
        {
            log.Debug(" -- createBackupsRoot");
            List<string> pathsList = this.paths.getAllPaths_AppBkp().listPaths;
            log.Debug(" -- createBackupsRoot:{" + CSharp.listToCsv(pathsList) + "}");
            foreach (var item in pathsList)
            {
                if (!item.Trim().Equals(""))
                {
                    log.Debug(" -- creating directory " + item);
                    Directory.CreateDirectory(item);
                }
                else
                {
                    log.Warn(" ** WARNING ** createBackupsRoot ELEMENT IS EMPTY");
                }
            }
        }

        // folders with the profile name inside backup folder. For example, for Skyrim instalation, for
        // a profile nammed Vanilla:
        //     game instalation: .\Steam\
        // BackupProfilesFolder: .\Steam\SkyrimBackups\Vanilla\
        private int createBackupProfilesFolder(string profName, out string errMsg)
        {
            log.Debug(" -- createBackupProfilesFolder");
            List<string> listDirs = this.paths.getAllPaths_BkpProf(profName).listPaths;
            log.Debug(" -- createBackupProfilesFolder:{" + CSharp.listToCsv(listDirs) + "}");

            foreach (var dir in listDirs)
            {
                try
                {
                    if (!dir.Trim().Equals(""))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    else
                    {
                        log.Warn(" ** WARNING ** createBackupProfilesFolder ELEMENT IS EMPTY");
                    }
                }
                catch (Exception ex)
                {
                    log.Error(" ** Error -- creating backup directory for path:<" + dir + ">");
                    log.Error(" ** Error -- Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                    errMsg = dir;
                    return Errors.ERR_CANNOT_CREATE_DIRECTORY;
                }
            }
            errMsg = "";
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
        private SPMState discoverApplicationState()
        {
            bool isInstalled = true;
            int configStatus = SettingsFactory.checkConfig(this.theGame);
            if (configStatus != Errors.SUCCESS)
            {
                // settings are not ok
                return SPMState.NOT_CONFIGURED;
            }
            else
            {
                // load desactivated installations
                this.listDesactivated = SPProfile.loadDesactivatedProfiles(this.paths.steamBkp,  this.paths.gameFolderName);
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
                if (this.activeProfile != null && this.activeProfile.name == profName)
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
                if (this.activeProfile != null && this.activeProfile.name == profName)
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

        private string safeNewName(string newName)
        {
            List<SPProfile> profilesInUse = this.getDesactivatedProfiles();
            profilesInUse.Add(this.getActiveProfile());
            List<string> profNamesInUse = new List<string>();
            foreach (var item in profilesInUse)
            {
                if (item != null)
                {
                    profNamesInUse.Add(item.name);
                }
            }
            string safeNewName = this.paths.safeNewProfileName(newName, profNamesInUse);
            log.Debug("safeNewName:" + safeNewName);
            return safeNewName;
        }


        /// <summary>
        /// Updates the Steam Profile Manager instance state, reloading all class members 
        /// inclusing the list of desactivated and active profiles. It also checks if the paths are still
        /// valid (if the paths are not, the state is set back to SPMSTate.NOT_CONFIGURED)
        /// </summary>
        private void updateManagerState()
        {
            log.Debug(" -- updateManagerState");
            // update state
            this.applicationState = this.discoverApplicationState();
            // log statte
            log.Debug("State: " + this.applicationState.ToString());
            if (this.activeProfile != null)
            {
                log.Debug("Active Profile => name:" + this.activeProfile.name + ", isReady:" + this.activeProfile.isReady);
            }
            else
            {
                log.Debug("No active profile");
            }
            log.Debug("Desactivated Profiles => [Count:" + this.listDesactivated.Count + "]");
            foreach (var item in this.listDesactivated)
            {
                log.Debug("    name:" + item.name +
                          ", isReady:" + item.isReady);
            }
        }
        #endregion aux 

        #endregion private_methos

    }
}
