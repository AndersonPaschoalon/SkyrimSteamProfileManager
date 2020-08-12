using ProfileManagerBL.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfileManager;

namespace ProfileManagerBL
{
    public class ProfileManagerBusinessLayer
    {
        private const string HELP_PAGE = "HelpPage.html";
        private ProfileViewData checkedActiveProfile = null;
        private ProfileViewData checkedDesactivatedProfile = null;
        private ProfileManager.SteamProfileManager manager;
        private ProfileManager.Enum.SPMState managerState;

        public ProfileManagerBusinessLayer(ViewGame game)
        {
            this.manager = new SteamProfileManager((ProfileManager.Enum.Game)game);
        }

        #region bl_helpers

        public EnabledOp allowedOperations 
        {
            get
            {
                EnabledOp eop = EnabledOp.getInstance();

                eop.editProfile = false;
                eop.activateProfile = false;
                eop.desactivateProfile = false;
                eop.editProfile = false;
                int nIn = this.countCheckedInactive();
                int nAc = this.countCheckedActive();
                int nDe = this.countCheckedDesactivated();
                ProfileManager.Enum.SPMState state = this.manager.showState();
                switch (state)
                {
                    case ProfileManager.Enum.SPMState.NO_PROFILE:
                        {
                            return eop;
                        }
                    case ProfileManager.Enum.SPMState.NOT_CONFIGURED:
                        {
                            return eop;
                        }
                    case ProfileManager.Enum.SPMState.INACTIVE_PROFILE:
                        // configuration and activation operation is permited
                        {
                            if (nIn == 1)
                            {
                                eop.activateProfile = true;
                            }
                            return eop;
                        }
                    case ProfileManager.Enum.SPMState.DESACTIVATED_ONLY:
                        // configuration and activation operations are permited
                        {
                            if (nDe == 1)
                            {
                                eop.activateProfile = true;
                                eop.editProfile = true;
                            }
                            return eop;
                        }
                    case ProfileManager.Enum.SPMState.ACTIVE_ONLY:
                        // configuration and desactivation operations are permited
                        {
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
                            if (nAc == 1 && nDe <= 0)
                            {
                                eop.desactivateProfile = true;
                                eop.editProfile = true;
                            }
                            else if (nAc == 1 && nDe == 1)
                            {
                                eop.switchProfile = true;
                            }
                            else if (nAc <= 0 && nDe == 1)
                            {
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
        }

        public List<ProfileViewData> active { get; private set; }

        public List<ProfileViewData> desactivated { get; private set; }

        public void openHelpPage()
        {
            try
            {
                System.Diagnostics.Process.Start(HELP_PAGE);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Process.Start("Chrome", Uri.EscapeDataString(HELP_PAGE));
            }
        }

        public SettingsViewData getSettingsData()
        {
            SettingsViewData se = new SettingsViewData();
            return se;
        }

        #endregion bl_helpers

        #region bl_actions
        public void action_updateSettings(SettingsViewData s)
        {
        }

        public void action_updateProfile(ProfileViewData p)
        {
        }

        public void action_activateInactive()
        {
        }

        public void action_activateDesactivated()
        {
        }

        public void action_desactivateProfile()
        {
        }

        public void action_switchProfiles()
        {
        }

        #endregion bl_actions

        #region test_methods

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
                        break;
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
            this.active = lp;
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
            switch (test)
            {
                case 0:
                    {
                        // 0 - ACTIVE_AND_DESACTIVATED_PROFILES
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
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
                        break;
                    }
                case 6:
                    {
                        // 6 - ACTIVE_AND_DESACTIVATED_PROFILES 
                        managerState = ProfileManager.Enum.SPMState.ACTIVE_AND_DESACTIVATED_PROFILES;
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
        }


        public List<ProfileViewData> test_getDesactivated(int test)
        {
            List<ProfileViewData> lp = new List<ProfileViewData>();

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
            foreach (var item in this.active)
            {
                if (item.isChecked == true && item.state == ProfileType.INACTIVE)
                {
                    i++;
                }
            }
            return i;
        }

        private int countCheckedActive()
        {
            int i = 0;
            foreach (var item in this.active)
            {
                if (item.isChecked == true && item.state == ProfileType.ACTIVE)
                {
                    i++;
                }
            }
            return i;
        }

        private int countCheckedDesactivated()
        {
            int i = 0;
            foreach (var item in this.desactivated)
            {
                if (item.isChecked == true && item.state == ProfileType.DESACTIVATED)
                {
                    i++;
                }
            }
            return i;
        }

        #endregion private_methods
    }
}
