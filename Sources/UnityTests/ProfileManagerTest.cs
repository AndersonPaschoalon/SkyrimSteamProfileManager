using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utils;
using ProfileManager;
using SpearSettings;

namespace UnityTests
{
    [TestClass]
    public class ProfileManagerTest
    {
        const string GAME01 = "TestGame01";
        const string GAME02 = "TestGame02";
        const string GAME01_PROF1 = "Vanilla";
        const string GAME01_PROF2 = "Dev";
        const string GAME01_PROF3 = "Vortex";
        const string GAME01_PROF4 = "Expansion";
        const string GAME02_PROF1 = "Vanilla";
        const string GAME02_PROF2 = "Dev";
        const string GAME02_PROF3 = "Vortex";
        const string GAME02_PROF4 = "Expansion";
        const string GAME_COLOR = "#FF00FF";
        const string GAME_CREATION_DATA = "2021/02/18";
        readonly string ENV_ROOT = Environment.CurrentDirectory + @"\\UNITY_TEST_ENVIROMENT\\";
        SteamProfileManager manager;// = new SteamProfileManager()

        #region TestGame01

        [TestMethod]
        public void TestMethod01_ActivateInactive_Prof1_TestGame01()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install prof1
            this.helperCleanupEnv();
            this.helperInstallTestGame01();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // activate prof1
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME01_PROF1, 
                                                          ProfileManagerTest.GAME_COLOR, 
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF1,
                                                    out errMsg, 
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);
        }

        [TestMethod]
        public void TestMethod02_DesactivateActive_Prof1_TestGame01()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // desactivate prof1
            retVal = this.manager.desactivateActiveProfile(ProfileManagerTest.GAME01_PROF1,
                                                           out errMsg);
            Assert.AreEqual(Errors.SUCCESS, retVal, errMsg);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF1,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);

        }

        [TestMethod]
        public void TestMethod03_ActivateInactive_Prof2_TestGame01()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install game
            this.helperInstallTestGame01();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // activate prof2
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME01_PROF2,
                                                          ProfileManagerTest.GAME_COLOR,
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF2,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);

        }

        [TestMethod]
        public void TestMethod04_DesactivateActive_Prof2_TestGame01()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // desactivate prof2
            retVal = this.manager.desactivateActiveProfile(ProfileManagerTest.GAME01_PROF2,
                                                           out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof2
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF2,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);
        }

        [TestMethod]
        public void TestMethod05_ActivateInactive_Prof3_TestGame01()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install game
            this.helperInstallTestGame01();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // activate prof3
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME01_PROF3,
                                                          ProfileManagerTest.GAME_COLOR,
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof3
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF3,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);
        }

        [TestMethod]
        public void TestMethod06_SwitchProfiles_Prof31_TestGame01()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;


            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // switch  3 -> 1
            retVal = this.manager.switchProfile(ProfileManagerTest.GAME01_PROF3, 
                                                ProfileManagerTest.GAME01_PROF1,
                                                out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // check prof3
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF3,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);

            // check prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF1,
                                                    out errMsg,
                                                    out errlabel);
            msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);

        }

        [TestMethod]
        public void TestMethod07_EditProfiles_Prof1_TestGame01()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME01);

            // switch  3 -> 1
            retVal = this.manager.editProfile(ProfileManagerTest.GAME01_PROF1,
                                              ProfileManagerTest.GAME01_PROF4,
                                              ProfileManagerTest.GAME_COLOR,
                                              out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // check prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME01_PROF4,
                                                    out errMsg,
                                                    out errlabel);
            string msg = "errMsg:" + errMsg + ", errlabel:" + errlabel;
            Assert.AreEqual(Errors.SUCCESS, retVal, msg);
        }

        [TestMethod]
        public void TestMethod08_Cleanup_TestGame01()
        {
            //this.helperCleanupEnv();
        }

        #endregion TestGame01

        #region TestGame02

        [TestMethod]
        public void TestMethod09_ActivateInactive_Prof1_TestGame02()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install prof1
            this.helperInstallTestGame02();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // activate prof1
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME02_PROF1,
                                                          ProfileManagerTest.GAME_COLOR,
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF1,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);
        }

        [TestMethod]
        public void TestMethod10_DesactivateActive_Prof1_TestGame02()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // desactivate prof1
            retVal = this.manager.desactivateActiveProfile(ProfileManagerTest.GAME02_PROF1,
                                                           out errMsg);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF1,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);

        }

        [TestMethod]
        public void TestMethod11_ActivateInactive_Prof2_TestGame02()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install game
            this.helperInstallTestGame02();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // activate prof2
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME02_PROF2,
                                                          ProfileManagerTest.GAME_COLOR,
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF2,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);

        }

        [TestMethod]
        public void TestMethod12_DesactivateActive_Prof2_TestGame02()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // desactivate prof2
            retVal = this.manager.desactivateActiveProfile(ProfileManagerTest.GAME02_PROF2,
                                                           out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof2
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF2,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);
        }

        [TestMethod]
        public void TestMethod13_ActivateInactive_Prof3_TestGame02()
        {
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // install game
            this.helperInstallTestGame02();

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // activate prof3
            retVal = this.manager.activateInactiveProfile(ProfileManagerTest.GAME02_PROF3,
                                                          ProfileManagerTest.GAME_COLOR,
                                                          ProfileManagerTest.GAME_CREATION_DATA);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // checar prof3
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF3,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);
        }

        [TestMethod]
        public void TestMethod14_SwitchProfiles_Prof31_TestGame02()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // switch  3 -> 1
            retVal = this.manager.switchProfile(ProfileManagerTest.GAME02_PROF3,
                                                ProfileManagerTest.GAME02_PROF1,
                                                out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // check prof3
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF3,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);

            // check prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF1,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);

        }

        [TestMethod]
        public void TestMethod15_EditProfiles_Prof1_TestGame02()
        {
            // checar prof2
            string errMsg = "";
            string errlabel = "";
            int retVal = Errors.SUCCESS;

            // instantiate the profile manager
            this.manager = new SteamProfileManager(ProfileManagerTest.GAME02);

            // switch  3 -> 1
            retVal = this.manager.editProfile(ProfileManagerTest.GAME02_PROF1,
                                              ProfileManagerTest.GAME02_PROF4,
                                              ProfileManagerTest.GAME_COLOR,
                                              out errMsg);

            Assert.AreEqual(Errors.SUCCESS, retVal);

            // check prof1
            retVal = this.manager.checkInstallation(ProfileManagerTest.GAME02_PROF4,
                                                    out errMsg,
                                                    out errlabel);
            Assert.AreEqual(Errors.SUCCESS, retVal);
        }

        [TestMethod]
        public void TestMethod16_Cleanup_TestGame02()
        {
            //this.helperCleanupEnv();
        }


        #endregion TestGame02

        #region envBuilders

        private bool helperInstallTestGame01()
        {
            this.helperCreateTestEnv();
            bool ret = false;
            string steamPath = @"UnityTests\Steam\Commons\TestGame01\";
            List<string> steamFiles = new List<string>{ "TestGame01.txt", "steam.txt", "TestGame01.exe" };
            if(!this.helperCreateDirAndFiles(steamPath, steamFiles))
            {
                
                return false;
            }

            string docsPath = @"UnityTests\Docs\My Games\TestGame01\";
            List<string> docsFiles = new List<string> { "TestGame01.txt", "docs.txt", "docs.xml" };
            if(!this.helperCreateDirAndFiles(docsPath, docsFiles))
            {
                return false;
            }

            string appDataPath = @"UnityTests\AppData\TestGame01\";
            List<string> appDataFiles = new List<string> { "TestGame01.txt", "appData.txt", "appData.xml" };
            if(!this.helperCreateDirAndFiles(appDataPath, appDataFiles))
            {
                return false;
            }

            string nmmPath = @"UnityTests\NMM\TestGame01\";
            List<string> nmmFiles = new List<string> { "TestGame01.txt", "nmm.txt", "nmm.jpg" };
            if(!this.helperCreateDirAndFiles(nmmPath, nmmFiles))
            {
                return false;
            }

            return true;
        }

        private bool helperInstallTestGame02()
        {
            this.helperCreateTestEnv();
            bool ret = false;
            string steamPath = @"UnityTests\Steam\Commons\TestGame02\";
            List<string> steamFiles = new List<string> { "TestGame02.txt", "steam.txt", "TestGame02.exe" };
            if (!this.helperCreateDirAndFiles(steamPath, steamFiles))
            {
                return false;
            }
            return true;
        }

        private bool helperCreateTestEnv()
        {
            try
            {
                Directory.CreateDirectory(this.ENV_ROOT);
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Message: " + ex.Message + ", StackTrace: " + ex.StackTrace);
                return false;
            }
            return true;
        }

        bool helperCreateDirAndFiles(string dir, List<string> files)
        {
            try
            {
                string dirPath = this.ENV_ROOT + dir;
                Directory.CreateDirectory(dirPath);
                foreach (var item in files)
                {
                    string fileToCreate = dirPath + item;
                    using (FileStream fs = File.Create(fileToCreate))
                    {
                        byte[] info = new UTF8Encoding(true).GetBytes("File Content:" + fileToCreate);
                        // Add some information to the file.
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return false;
            }
            return true;
        }

        bool helperCleanupEnv()
        {
            try 
            {
                Directory.Delete(this.ENV_ROOT, true);
            }
            catch (Exception ex)
            {
                Logger.LogMessage("Message: " + ex.Message + ", StackTrace: " + ex.StackTrace);
                return false;
            }
            
            return true;
        }

        #endregion envBuilders
    }
}
