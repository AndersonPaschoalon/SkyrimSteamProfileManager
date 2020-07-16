using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyrimSteamProfileManager.Objects;
using SSPErrors;
using Logger;
using Logger.Objects;

namespace SkyrimSteamProfileManager
{
    class SkyrimProfileManager
    {
        enum States
        {
            // Only configuration operation is permited
            NOT_CONFIGURED = 0,
            // Only configuration operation is permited
            NO_PROFILE = 1,
            // configuration and activation operation is permited
            INACTIVE_PROFILE = 2,
            // configuration, desactivation and switch operations are permited
            ACTIVE_AND_DESACTIVATED_PROFILES = 3,
            // configuration and desactivation operations are permited
            ACTIVE_ONLY = 4,
            // configuration and activation operations are permited
            DESACTIVATED_ONLY = 5
        }

        // Consts
        private const string ACTIVE_INTEGRITY_FILE_NAME = "active_profile.int";
        private const string SSP_BACKUP = "SkyrimBackups";
        private readonly ILogger log = ConsoleLogger.getInstance();
        // state
        private SSPConfig config;
        private bool isAppConfigured = false;
        private States applicationState = States.NOT_CONFIGURED;

        public SkyrimProfileManager()
        {
            // load settings
            this.config = SSPConfig.getConfig();
            this.isAppConfigured = this.checkConfig();
            log.Debug("Is app Configured? " + this.isAppConfigured);
            if (this.isAppConfigured)
            {
                this.applicationState = this.discoverAppState();
            }
            else
            {
                this.applicationState = States.NOT_CONFIGURED;
            }
        }

        public int updateSettings(string newSteamPath, string newDocumentsPath, string newAppDataPath, 
                                  string nmmInfoPath, string nmmModPath)
        {
            if (newSteamPath == null ||
                newDocumentsPath == null ||
                newAppDataPath == null ||
                nmmInfoPath == null ||
                nmmModPath == null ||
                newSteamPath.Trim().Equals("") ||
                newDocumentsPath.Trim().Equals("") ||
                newAppDataPath.Trim().Equals(""))
            {
                return Error.ERR_INVALID_SETTINGS;
            }
            else
            {
                if (!this.checkDir(newSteamPath) ||
                    !this.checkDir(newDocumentsPath) ||
                    !this.checkDir(newAppDataPath))
                {
                    return Error.ERR_PATH_NOT_EXIST;
                }
                if (nmmInfoPath != "")
                {
                    if (!this.checkDir(nmmInfoPath))
                    {
                        return Error.ERR_PATH_NOT_EXIST;
                    }
                }
                if (nmmModPath != "")
                {
                    if (!this.checkDir(nmmModPath))
                    {
                        return Error.ERR_PATH_NOT_EXIST;
                    }
                }

                // update settings 
                this.config.settings.appDataPath = newAppDataPath;
                this.config.settings.steamPath = newSteamPath;
                this.config.settings.documentsPath = newDocumentsPath;
                this.config.settings.nmmModPath = nmmInfoPath;
                this.config.settings.nmmInfoPath = nmmInfoPath;

                // create backup direcotories
                this.createBackupsFolder();

                // save settings
                this.config.saveConfig();

                return Error.SUCCESS;

            }
        }

        /// <summary>
        /// Check if there is an inactive profile. 
        /// </summary>
        /// <returns></returns>
        public bool checkInactiveProfile()
        {
            // TODO
            return false;
        }

        /// <summary>
        /// Creates an active profile from existing instalation. If there is already an active profile,
        /// or if there is no installation, this method does nothing adn returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int activateInactiveProfile(string profileName, string color)
        {
            // check settings
            if (this.checkConfig())
            {
                log.Equals("Settings are not valid, impossible to finish operation. Configure the Application settings again.");
                return Error.ERR_INVALID_SETTINGS;
            }

            // Create profile object
            int profileNewId = this.config.listProfiles.profiles.Count + 1;
            foreach (var item in this.config.listProfiles.profiles)
            {
                if (profileName.Trim() == item.name.Trim())
                {
                    // name already in use
                    return Error.ERR_PROFILE_NAME_ALREADY_EXISTS;
                }
            }
            SSPProfile newProfile = new SSPProfile();
            newProfile.color = color;
            newProfile.id = profileNewId;
            newProfile.isActive = "TRUE";
            newProfile.name = profileName;

            // Create integrity file 
            if (!this.createIntegrityFile(newProfile))
            {
                log.Error("Could not create intregrity file");
                return Error.ERR_CANNOT_CREATE_INTEGRITY_FILE;
            }

            // add to configuration
            this.config.listProfiles.profiles.Add(newProfile);
            this.config.saveConfig();

            return Error.SUCCESS;
        }

        /// <summary>
        /// If there is no profile installed, set a desactivated profile as active. Otherwise does nothing
        /// and returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int setDesactivatedProfileActive(SSPProfile profile)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int desactivateProfile(SSPProfile profile)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        public int switchProfile(SSPProfile active, SSPProfile desactivated)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        #region private_methos

        #region paths 

        private string integrityFilePath()
        {
            return this.config.settings.steamPathGame() + "\\" + ACTIVE_INTEGRITY_FILE_NAME;
        }
        private string steamBackup()
        {
            return this.config.settings.steamPath + "\\" + SSP_BACKUP;
        }
        private string steamBackupGame(string name)
        {
            return steamBackup() + "\\" + 
                   name.Trim() + "\\" +
                   this.config.settings.gameFolder.Trim();
        }

        private string documentsBackup()
        {
            return this.config.settings.documentsPath + "\\" + SSP_BACKUP;
        }

        private string documentsBackupGame(string name)
        {
            return documentsBackup() + "\\" +
                   name.Trim() + "\\" +
                   this.config.settings.gameFolder.Trim();
        }

        private string appDataBackup()
        {
            return this.config.settings.appDataPath + "\\" + SSP_BACKUP;
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
                return this.config.settings.nmmInfoPath + "\\" + SSP_BACKUP;
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
                return this.config.settings.nmmModPath + "\\" + SSP_BACKUP;
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

        private List<string> splitCsv(string csv)
        {
            List<string> elements = new List<string>();
            string[] line = csv.Split(',');
            foreach (string item in line)
            {
                string a = item.Trim();
                elements.Add(a);
            }
            return elements;
        }

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

        bool createIntegrityFile(SSPProfile prof)
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

        private States discoverAppState()
        {
            // TODO
            return States.ACTIVE_ONLY;
        }

        private int checkAllLoadedProfiles()
        {
            return 0;
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
                log.Warn("Invalid SSP settings, settings must be initialized before used");
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
        /// Check all settings of a specified ACTIVE profile. If some settings are inconsistent 
        /// with an active profile, returns false.
        /// </summary>
        /// <param name="prof"></param>
        /// <returns></returns>
        private bool checkActiveProfile(SSPProfile prof)
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
            List<string> profData = this.splitCsv(integrityFile);
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
        private bool checkDesactivatedProfile(SSPProfile prof)
        {
            // TODO
            return false;
        }


        #endregion checkHelpers

        #endregion private_methos

    }
}
