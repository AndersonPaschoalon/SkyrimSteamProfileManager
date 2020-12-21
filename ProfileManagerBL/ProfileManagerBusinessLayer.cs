using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

using ProfileManagerBL.ViewModel;
using ProfileManager;
using ProfileManager.Objects;
using ProfileManager.Enum;
using Utils;
using Utils.Loggers;
using System.Threading.Tasks;

namespace ProfileManagerBL
{
    public class ProfileManagerBusinessLayer
    {
        private const string HELP_PAGE = "HelpPage.html";
        private readonly ILogger log;
        private readonly string gameStr;
        private ProfileManager.SteamProfileManager manager;


        public static string[] availableGames()
        {
            return SPConfig.arrayGames();
        }

        public ProfileManagerBusinessLayer(string game)
        {
            this.log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            log.Debug("SELECTED GAME: " + game);
            List<string> listGames = SPConfig.listGames();
            if (!listGames.Contains(game))
            {
                log.Warn("** ERROR!! Provided game " + game + " dont exit in the Settings file!");
                game = Consts.GAME_DEFAULT;
            }
            this.manager = new SteamProfileManager(game);
            this.gameStr = game;
        }

        #region bl_helpers

        private List<ProfileViewData> activated { get; set;}

        private List<ProfileViewData> desactivated { get; set;}

        private List<ProfileViewData> inactive { get; set;}

        public void reloadProfiles()
        {
            this.inactive = new List<ProfileViewData>();
            this.activated = new List<ProfileViewData>();
            this.manager.reloadState();
            SPProfile profIn = this.manager.getInactiveProfile();
            SPProfile profAc = this.manager.getActiveProfile();
            if (profIn != null)
            {
                this.inactive.Add(new ProfileViewData(profIn, ProfileType.INACTIVE));
            }
            if (profAc != null)
            {
                this.activated.Add(new ProfileViewData(profAc, ProfileType.ACTIVE));
            }
            List<SPProfile> listDes = this.manager.getDesactivatedProfiles();
            List<ProfileViewData> ldp = new List<ProfileViewData>();
            if (listDes != null)
            {
                foreach (var item in listDes)
                {
                    ldp.Add(new ProfileViewData(item, ProfileType.DESACTIVATED));
                }
            }
            this.desactivated = ldp;
        }

        public List<ProfileViewData> getActiveProfiles()
        {
            if (this.manager.getApplicationState() == SPMState.INACTIVE_PROFILE)
            {
                return this.inactive;
            }
            else
            {
                return this.activated;
            }
        }

        public List<ProfileViewData> getDesactivatedProfiles()
        {
            return this.desactivated;
        }

        public string dateFormat()
        {
            return this.manager.dateFormat();
        }

        public EnabledOp allowedOperations()
        {
            int nIn = this.countCheckedInactive();
            int nAc = this.countCheckedActive();
            int nDe = this.countCheckedDesactivated();
            ProfileManager.Enum.SPMState state = this.manager.getApplicationState();
            EnabledOp eop = new EnabledOp(); // return obj
            // Profile Operations
            switch (state)
            {
                case ProfileManager.Enum.SPMState.NO_PROFILE:
                    {
                        log.Debug("No Profile, No Operations Allowed");
                        break;

                    }
                case ProfileManager.Enum.SPMState.NOT_CONFIGURED:
                    {
                        log.Debug("Not Configured, No Operations Allowed");
                        break;
                    }
                case ProfileManager.Enum.SPMState.INACTIVE_PROFILE:
                    // configuration and activation operation is permited
                    {
                        log.Debug("INACTIVE_PROFILE, activation is permited if selected");
                        if (nIn == 1)
                        {
                            log.Debug("One INACTIVE selected");
                            eop.activateProfile = true;
                        }
                        break;
                    }
                case ProfileManager.Enum.SPMState.DESACTIVATED_ONLY:
                    // configuration and activation operations are permited
                    {
                        log.Debug("DESACTIVATED_ONLY edit and activation is permited if ONE is selected");
                        if (nDe == 1)
                        {
                            log.Debug("one is selected");
                            eop.activateProfile = true;
                            eop.editProfile = true;
                        }
                        break;
                    }
                case ProfileManager.Enum.SPMState.ACTIVE_ONLY:
                    // configuration and desactivation operations are permited
                    {
                        log.Debug("ACTIVE_ONLY if one is selected desactivation and edit is permited");
                        if (nAc == 1)
                        {
                            eop.desactivateProfile = true;
                            eop.editProfile = true;
                        }
                        break;
                    }
                case ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES:
                    // configuration, desactivation and switch operations are permited
                    {
                        log.Debug("ACTIVE_AND_DESACTIVATED_PROFILES edit, switch, desactive and edit permited");
                        if (nAc == 1 && nDe <= 0)
                        {
                            log.Debug("active selected: edit, desactive");
                            eop.desactivateProfile = true;
                            eop.editProfile = true;
                        }
                        else if (nAc == 1 && nDe == 1)
                        {
                            log.Debug("active+desactivated selected: switch");
                            eop.switchProfile = true;
                        }
                        else if (nAc <= 0 && nDe == 1)
                        {

                            log.Debug("desactivated selected: edit, active");
                            eop.editProfile = true;
                        }
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            // Developer Operations
            if (nIn == 1 || nAc == 1)
            {
                eop.createGitignore = !this.manager.gitignoreDetected();
                eop.deleteGitignore = this.manager.gitignoreDetected();
            }

            return eop;
        }

        public SettingsViewData getSettingsData()
        {
            SettingsViewData se = new SettingsViewData();
            return se;
        }

        public string gameName()
        {
            return this.gameStr;
        }

        public static string listProfileViewToString(List<ProfileViewData> lprof)
        {
            string str = "(List<ProfileViewData> [\n";
            foreach (var item in lprof)
            {
                str += "    name:" + item.name +
                       ", isChecked:" + item.isChecked +
                       ", state:" + item.state +
                       ", colorHex:" + item.colorHex +
                       ", creatingDate:" + item.creatingDate + "\n";
            }
            str += "]";
            return str;
        }

        #endregion bl_helpers

        #region bl_actions

        public SettingsViewData action_getSettings()
        {
            SettingsViewData settings = new SettingsViewData();
            SPSettings spsettings = manager.getProfileSettings();
            settings.appData = spsettings.appDataPath;
            settings.docs = spsettings.documentsPath;
            settings.steam = spsettings.steamPath;
            settings.nmmInfo = spsettings.nmmInfoPath;
            settings.nmmMod = spsettings.nmmModPath;
            return settings;
        }

        public int action_updateSettings(SettingsViewData s)
        {
            int ret = Errors.SUCCESS;
            try
            {
                this.manager.updateSettings(s.steam, s.docs, s.appData, s.nmmInfo, s.nmmMod);
                this.manager.reloadState();
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_UNKNOWN;
            }
            return ret;
        }

        public int action_updateProfile(ProfileViewData pUpdated, ProfileViewData pOld, out string errMsg)
        {
            int ret = Errors.SUCCESS;
            try
            {
                ret = manager.editProfile(pOld.name, pUpdated.name, pUpdated.colorHex,out errMsg);
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                errMsg = ex.Message;
                return Errors.ERR_UNKNOWN;
            }
            return ret;
        }

        public int action_activateInactive(ProfileViewData p)
        {
            int ret = Errors.SUCCESS;
            try
            {
                ret = manager.activateInactiveProfile(p.name, p.colorHex, p.creatingDate);
                this.manager.reloadState();
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_UNKNOWN;
            }
            return ret;
        }

        public int action_activateDesactivated(ProfileViewData p)
        {
            int ret = Errors.SUCCESS;
            try
            {
                ret = manager.activateDesactivatedProfile(p.name);
                this.manager.reloadState();
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_UNKNOWN;
            }
            return ret;
        }

        public int action_desactivateProfile(ProfileViewData p)
        {
            int ret = Errors.SUCCESS;
            try
            {
                ret = manager.desactivateActiveProfile(p.name);
                this.manager.reloadState();
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_UNKNOWN;
            }
            return ret;

        }

        public int action_switchProfiles(ProfileViewData ap, ProfileViewData dp)
        {
            int ret = Errors.SUCCESS;
            try
            {
                ret = manager.switchProfile(ap.name, dp.name);
                this.manager.reloadState();
            }
            catch (Exception ex)
            {
                log.Error("** EXCEPTION Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ERR_UNKNOWN;
            }
            return ret;
        }

        #endregion bl_actions

        #region bl_tools

        public void openHelpPage()
        {
            string htmlPage = Environment.CurrentDirectory + "\\" + HELP_PAGE;
            try
            {
                System.Diagnostics.Process.Start(htmlPage);
            }
            catch (Exception)
            {
                System.Diagnostics.Process.Start("Chrome", Uri.EscapeDataString(htmlPage));
            }
        }

        public void openLogFiles()
        {
            List<string> logFiles = new List<string>
            {"Logs\\app_core.log","Logs\\app_ui.log"};
            foreach (var item in logFiles)
            {
                try
                {
                    Process.Start(item);
                }
                catch (Exception)
                {
                    Process.Start("notepad.exe", item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool gitignoreDetected()
        {
            return this.manager.gitignoreDetected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool createGitignore(out string errMsg)
        {
            log.Debug(" -- createGitignore()");
            if (!this.manager.gitignoreDetected())
            {
                log.Debug(" -- gitignore not detected");
                return this.manager.createGitignore(out errMsg);
            }
            errMsg = ".gitignore file was detected!";
            return false;
        }

        public bool deleteGitignore(out string errMsg)
        {
            log.Debug(" -- deleteGitignore()");
            if (this.manager.gitignoreDetected())
            {
                log.Debug(" -- gitignore DETECTED");
                return this.manager.deleteGitignore(out errMsg);
            }
            errMsg = ".gitignore file was not detected!";
            return false;
        }

        #endregion bl_tools

        #region test_methods

        /// <summary>
        /// Generate manager states
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public EnabledOp test_getAllowed(int test)
        {
            log.Debug("-- test_getAllowed: " + test);
            log.Debug("0 - ACTIVE_AND_DESACTIVATED_PROFILES");
            log.Debug("1 - NOT_CONFIGURED ");
            log.Debug("2 - NO_PROFILE ");
            log.Debug("3 - INACTIVE_PROFILE ");
            log.Debug("4 - ACTIVE_ONLY ");
            log.Debug("5 - DESACTIVATED_ONLY ");
            log.Debug("6 - ACTIVE_AND_DESACTIVATED_PROFILES ");
            int nIn = this.countCheckedInactive();
            int nAc = this.countCheckedActive();
            int nDe = this.countCheckedDesactivated();
            log.Debug("nIn:" + nIn + ", nAc:" + nAc + ", nDe:" + nDe);

            ProfileManager.Enum.SPMState state;
            switch (test)
            {
                case 0:
                    {
                        state = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
                case 1:
                    {
                        state = ProfileManager.Enum.SPMState.NOT_CONFIGURED;
                        break;
                    }
                case 2:
                    {
                        state = ProfileManager.Enum.SPMState.NO_PROFILE;
                        break;
                    }
                case 3:
                    {
                        state = ProfileManager.Enum.SPMState.INACTIVE_PROFILE;
                        break;
                    }
                case 4:
                    {
                        state = ProfileManager.Enum.SPMState.ACTIVE_ONLY;
                        break;
                    }
                case 5:
                    {
                        state = ProfileManager.Enum.SPMState.DESACTIVATED_ONLY;
                        break;
                    }
                case 6:
                    {
                        state = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
                case 7:
                    {
                        state = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
                default:
                    {
                        state = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
            }
            log.Debug("STATE:" + state);

            EnabledOp eop = new EnabledOp(); // return obj
            switch (state)
            {
                case ProfileManager.Enum.SPMState.NO_PROFILE:
                    {
                        log.Debug("No Profile, No Operations Allowed");
                        return eop;
                    }
                case ProfileManager.Enum.SPMState.NOT_CONFIGURED:
                    {
                        log.Debug("Not Configured, No Operations Allowed");
                        return eop;
                    }
                case ProfileManager.Enum.SPMState.INACTIVE_PROFILE:
                    // configuration and activation operation is permited
                    {
                        log.Debug("INACTIVE_PROFILE, activation is permited if selected");
                        if (nIn == 1)
                        {
                            log.Debug("One INACTIVE selected");
                            eop.activateProfile = true;
                        }
                        return eop;
                    }
                case ProfileManager.Enum.SPMState.DESACTIVATED_ONLY:
                    // configuration and activation operations are permited
                    {
                        log.Debug("DESACTIVATED_ONLY edit and activation is permited if ONE is selected");
                        if (nDe == 1)
                        {
                            log.Debug("one is selected");
                            eop.activateProfile = true;
                            eop.editProfile = true;
                        }
                        return eop;
                    }
                case ProfileManager.Enum.SPMState.ACTIVE_ONLY:
                    // configuration and desactivation operations are permited
                    {
                        log.Debug("ACTIVE_ONLY if one is selected desactivation and edit is permited");
                        if (nAc == 1)
                        {
                            eop.desactivateProfile = true;
                            eop.editProfile = true;
                        }
                        return eop;
                    }
                case ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES:
                    // configuration, desactivation and switch operations are permited
                    {
                        log.Debug("ACTIVE_AND_DESACTIVATED_PROFILES edit, switch, desactive and edit permited");
                        if (nAc == 1 && nDe <= 0)
                        {
                            log.Debug("active selected: edit, desactive");
                            eop.desactivateProfile = true;
                            eop.editProfile = true;
                        }
                        else if (nAc == 1 && nDe == 1)
                        {
                            log.Debug("active+desactivated selected: switch");
                            eop.switchProfile = true;
                        }
                        else if (nAc <= 0 && nDe == 1)
                        {

                            log.Debug("desactivated selected: edit, active");
                            eop.editProfile = true;
                        }
                        return eop;
                    }
                default:
                    {
                        return eop;
                    }
            }
        }

        public List<ProfileViewData> test_getActive(int test)
        {
            // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
            // 1 - NOT_CONFIGURED  
            // 2 - NO_PROFILE 
            // 3 - INACTIVE_PROFILE 
            // 4 - ACTIVE_ONLY 
            // 5 - DESACTIVATED_ONLY 
            // 6 - ACTIVE_AND_DESACTIVATED_PROFILES 
            // 7 - 

            List<ProfileViewData> lp = new List<ProfileViewData>();
            SPMState managerState = this.manager.getApplicationState();
            switch (test)
            {
                case 0:
                    {
                        // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "Trest 00", state = ProfileType.ACTIVE });
                        break;
                    }
                case 1:
                    {
                        // 1 - NOT_CONFIGURED  
                        managerState = ProfileManager.Enum.SPMState.NOT_CONFIGURED;
                        break;
                    }
                case 2:
                    {
                        // 2 - NO_PROFILE 
                        managerState = ProfileManager.Enum.SPMState.NO_PROFILE;
                        break;
                    }
                case 3:
                    {
                        // 3 - INACTIVE_PROFILE 
                        managerState = ProfileManager.Enum.SPMState.INACTIVE_PROFILE;
                        return this.activated;
                    }
                case 4:
                    {
                        // 4 - ACTIVE_ONLY 
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "Vanilla01 - active only", state = ProfileType.ACTIVE });
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_ONLY;
                        break;
                    }
                case 5:
                    {
                        // 5 - DESACTIVATED_ONLY 
                        managerState = ProfileManager.Enum.SPMState.DESACTIVATED_ONLY;
                        break;
                    }
                case 6:
                    {
                        // 6 - ACTIVE_AND_DESACTIVATED_PROFILES 
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "Vanilla01 - active/des", state = ProfileType.ACTIVE });
                        break;
                    }
                case 7:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            this.activated = lp;
            return lp;
        }

        public void test_updateManager(int test)
        {
            // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
            // 1 - NOT_CONFIGURED  
            // 2 - NO_PROFILE 
            // 3 - INACTIVE_PROFILE 
            // 4 - ACTIVE_ONLY 
            // 5 - DESACTIVATED_ONLY 
            // 6 - ACTIVE_AND_DESACTIVATED_PROFILES 
            // 7 - 
            SPMState managerState = this.manager.getApplicationState();
            switch (test)
            {
                case 0:
                    {
                        // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
                        MessageBox.Show("ACTIVE_AND_DESACTIVATED_PROFILES");
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
                case 1:
                    {
                        // 1 - NOT_CONFIGURED  
                        MessageBox.Show("NOT_CONFIGURED");
                        managerState = ProfileManager.Enum.SPMState.NOT_CONFIGURED;
                        break;
                    }
                case 2:
                    {
                        // 2 - NO_PROFILE 
                        MessageBox.Show("NO_PROFILE");
                        managerState = ProfileManager.Enum.SPMState.NO_PROFILE;
                        break;
                    }
                case 3:
                    {
                        // 3 - INACTIVE_PROFILE 
                        MessageBox.Show("INACTIVE_PROFILE");
                        managerState = ProfileManager.Enum.SPMState.INACTIVE_PROFILE;
                        break;
                    }
                case 4:
                    {
                        // 4 - ACTIVE_ONLY 
                        MessageBox.Show("ACTIVE_ONLY");
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_ONLY;
                        break;
                    }
                case 5:
                    {
                        // 5 - DESACTIVATED_ONLY 
                        MessageBox.Show("DESACTIVATED_ONLY");
                        managerState = ProfileManager.Enum.SPMState.DESACTIVATED_ONLY;
                        break;
                    }
                case 6:
                    {
                        // 6 - ACTIVE_AND_DESACTIVATED_PROFILES
                        MessageBox.Show("ACTIVE_AND_DESACTIVATED_PROFILES");
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        public List<ProfileViewData> test_getDesactivated(int test)
        {
            List<ProfileViewData> lp = new List<ProfileViewData>();
            SPMState managerState = this.manager.getApplicationState();
            switch (test)
            {
                case 0:
                    {
                        // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "Test 00", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName01", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("BlueViolet"), name = "TestName02", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Blue"), name = "TestName03", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Coral"), name = "TestName04", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkBlue"), name = "TestName05", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Cyan"), name = "TestName06", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Crimson"), name = "TestName07", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOliveGreen	"), name = "TestName08", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName09", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOrange"), name = "TestName10", state = ProfileType.DESACTIVATED });
                        break;
                    }
                case 1:
                    {
                        // 1 - NOT_CONFIGURED  
                        managerState = ProfileManager.Enum.SPMState.NOT_CONFIGURED;
                        break;
                    }
                case 2:
                    {
                        // 2 - NO_PROFILE 
                        managerState = ProfileManager.Enum.SPMState.NO_PROFILE;
                        break;
                    }
                case 3:
                    {
                        // 3 - INACTIVE_PROFILE
                        managerState = ProfileManager.Enum.SPMState.INACTIVE_PROFILE;
                        break;
                    }
                case 4:
                    {
                        // 4 - ACTIVE_ONLY 
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_ONLY;
                        break;
                    }
                case 5:
                    {
                        // 5 - DESACTIVATED_ONLY 
                        managerState = ProfileManager.Enum.SPMState.DESACTIVATED_ONLY;
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName01 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("BlueViolet"), name = "TestName02 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Blue"), name = "TestName03 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Coral"), name = "TestName04 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkBlue"), name = "TestName05 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Cyan"), name = "TestName06 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Crimson"), name = "TestName07 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOliveGreen	"), name = "TestName08 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName09 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOrange"), name = "TestName10 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName11 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkSalmon"), name = "TestName12 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName13 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName14 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName15 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName16 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName17 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName18 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName19 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName20 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName20 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName21 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName22 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName23 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName24 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName25 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName26 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName27 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName28 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName29 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName30 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName31 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName32 desOnly", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName33 desOnly", state = ProfileType.DESACTIVATED });
                        break;
                    }
                case 6:
                    {
                        // 6 - ACTIVE_AND_DESACTIVATED_PROFILES 
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName01 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("BlueViolet"), name = "TestName02 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Blue"), name = "TestName03 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Coral"), name = "TestName04 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkBlue"), name = "TestName05 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Cyan"), name = "TestName06 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("Crimson"), name = "TestName07 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOliveGreen	"), name = "TestName08 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName09 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkOrange"), name = "TestName10 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName11 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("DarkSalmon"), name = "TestName12 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName13 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName14 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName15 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName16 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName17 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName18 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName19 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName20 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName20 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName21 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName22 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName23 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName24 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName25 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName26 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName27 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName28 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName29 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName30 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName31 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName32 desAct", state = ProfileType.DESACTIVATED });
                        lp.Add(new ProfileViewData { color = Color.FromName("SlateBlue"), name = "TestName33 desAct", state = ProfileType.DESACTIVATED });

                        break;
                    }
                case 7:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            this.desactivated = lp;
            return lp;
        }

        public void test_updateBl(int test, ref List<ProfileViewData> lpa, ref List<ProfileViewData> lpd)
        {
            test_updateManager(test);
            lpa = test_getActive(test);
            lpd = test_getDesactivated(test);
        }

        #endregion test_methods

        #region private_methods

        private int countCheckedInactive()
        {
            int i = 0;
            if (this.inactive != null)
            {
                foreach (var item in this.inactive)
                {
                    if (item.isChecked == true && item.state == ProfileType.INACTIVE)
                    {
                        i++;
                    }
                }
            }
            return i;
        }

        private int countCheckedActive()
        {
            int i = 0;
            if (this.activated != null)
            {
                foreach (var item in this.activated)
                {
                    if (item.isChecked == true && item.state == ProfileType.ACTIVE)
                    {
                        i++;
                    }
                }
            }

            return i;
        }

        private int countCheckedDesactivated()
        {
            int i = 0;
            if (this.desactivated != null)
            {
                foreach (var item in this.desactivated)
                {
                    if (item.isChecked == true && item.state == ProfileType.DESACTIVATED)
                    {
                        i++;
                    }
                }
            }
            return i;
        }

        #endregion private_methods
    }
}
