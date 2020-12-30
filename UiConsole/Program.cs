using ProfileManager;
using ProfileManager.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.Loggers;

namespace UiConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            ILogger log = Log4NetLogger.getInstance(LogAppender.APP_UI);
            log.Info("###############################################################################");
            log.Info("# SteamProfileManager Console");
            log.Info("###############################################################################");

            SteamProfileManager manager = new SteamProfileManager("Skyrim");
            bool quit = false;
            while (true)
            {
                displayMenu();
                string opt = Console.ReadLine();
                switch (opt)
                {
                    // 00   Menu
                    case "00":
                        {
                            displayMenu();
                            break;
                        }
                    // 01   Show State
                    case "01":
                        {
                            showState(manager);
                            break;
                        }
                    // 02   Show Config File
                    case "02":
                        {
                            Console.WriteLine("TODO");
                            break;
                        }
                    // 03   Configure List
                    case "03":
                        {
                            updateConfig(manager);
                            break;
                        }
                    // 04   configureCsv
                    case "04":
                        {
                            updateConfigCsv(manager);
                            break;
                        }
                    // 05   configure Default
                    case "05":
                        {
                            updateConfigCsvDefault(manager);
                            break;
                        }
                    case "06":
                        {
                            listAllFiles();
                            break;
                        }
                    // 20   activateInactiveProfile
                    case "20":
                        {
                            exec_activateInactiveProfile(manager);
                            break;
                        }
                    // 30   activateDesactivatedProfile
                    case "30":
                        {
                            exec_activateDesactivatedProfile(manager);
                            break;
                        }
                    // 40   desactivateActiveProfile
                    case "40":
                        {
                            exec_desactivateActiveProfile(manager);
                            break;
                        }
                    // 50   switchProfile
                    case "50":
                        {
                            exec_switchProfile(manager);
                            break;
                        }
                    // 60   editProfile
                    case "60":
                        {
                            exec_editProfile(manager);
                            break;
                        }
                    case "70":
                        {
                            exec_killAll(manager);
                            break;
                        }
                    case "99":
                        {
                            quit = true;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("> Invalid Option {" + opt + "}");
                            break;
                        }
                }
                if (quit)
                {
                    Console.WriteLine("Exiting Process!");
                    break;
                }
            }

        }

        private static void displayMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("00   Menu");
            Console.WriteLine("01   Show State");
            Console.WriteLine("02   Show Config File");
            Console.WriteLine("03   Configure List");
            Console.WriteLine("04   configureCsv");
            Console.WriteLine("05   configure Default");
            Console.WriteLine("06   list all files");

            Console.WriteLine("20   activateInactiveProfile");
            Console.WriteLine("30   activateDesactivatedProfile");
            Console.WriteLine("40   desactivateActiveProfile");
            Console.WriteLine("50   switchProfile");
            Console.WriteLine("60   editProfile");
            Console.WriteLine("70   killAllSteam");

            Console.WriteLine("99   QUIT");
            Console.WriteLine("PATH: " + Directory.GetCurrentDirectory());
        }

        private static void showState(SteamProfileManager manager)
        {
            SPMState s =  manager.showState();
            Console.WriteLine("State: " + s.ToString());
        }

        // TestEnviroment\Test01\Docs

        private static void updateConfig(SteamProfileManager manager)
        {
            Console.WriteLine("> updateConfig");
            Console.WriteLine(@"TestEnviroment\Test01\Steam\,TestEnviroment\Test01\Docs\,TestEnviroment\Test01\AppData\,TestEnviroment\Test01\NMM\nmmInfo,TestEnviroment\Test01\NMM\nmmMod");
            string newSteamPath = readOption("SteamPath");
            string newDocumentsPath = readOption("DocumentsPath"); 
            string newAppDataPath = readOption("AppDataPath");
            //string nmmInfoPath = readOption("nmmInfoPath");
            //string nmmModPath = readOption("nmmModPath");
            string nmmPath = readOption("nmmPath");
            Console.WriteLine("    Steam: " + newSteamPath);
            Console.WriteLine("Documents: " + newDocumentsPath);
            Console.WriteLine("  AppData: " + newAppDataPath);
            //Console.WriteLine(" NMM Info: " + nmmInfoPath);
            //Console.WriteLine("  NMM Mod: " + nmmModPath);
            Console.WriteLine("      NMM: " + nmmPath);
            Console.WriteLine("---------------------------------");
            string confirmOption = readOption("Confirm Option?y/n");
            if (confirmOption.Trim().ToUpper() == "Y")
            {
                //manager.updateSettings(newSteamPath, newDocumentsPath, newAppDataPath, nmmInfoPath, nmmModPath);
                manager.updateSettings(newSteamPath, newDocumentsPath, newAppDataPath, nmmPath);

            }
            else
            {
                Console.WriteLine("> abort updateSettings");
            }
        }

        // 
        private static void updateConfigCsv(SteamProfileManager manager)
        {
            Console.WriteLine(@"TestEnviroment\Test01\Steam\,TestEnviroment\Test01\Docs\,TestEnviroment\Test01\AppData\,TestEnviroment\Test01\NMM\nmmInfo,TestEnviroment\Test01\NMM\nmmMod");
            Console.WriteLine("> updateConfigCsv");
            List<string> opts = splitCsv(readOption("Steam,Docs,AppData,nmmInfo,nmmMod:"));
            Console.WriteLine("    Steam: " + opts[0]);
            Console.WriteLine("Documents: " + opts[1]);
            Console.WriteLine("  AppData: " + opts[2]);
            Console.WriteLine("      NMM: " + opts[3]);
            //Console.WriteLine(" NMM Info: " + opts[3]);
            //Console.WriteLine("  NMM Mod: " + opts[4]);
            Console.WriteLine("---------------------------------");
            string confirmOption = readOption("Confirm Option?y/n");
            if (confirmOption.Trim().ToUpper() == "Y")
            {
                //manager.updateSettings(opts[0], opts[1], opts[2], opts[3], opts[4]);
                manager.updateSettings(opts[0], opts[1], opts[2], opts[3]);
            }
            else
            {
                Console.WriteLine("> abort updateSettings");
            }
        }

        private static void updateConfigCsvDefault(SteamProfileManager manager)
        {
            Console.WriteLine("> updateConfigCsvDefault");
            //List<string> opts = csvToList(readOption("Steam,Docs,AppData,nmmInfo,nmmMod:"));
            List<string> opts = splitCsv(@"TestEnviroment\Test01\Steam\Commons\,TestEnviroment\Test01\Docs,TestEnviroment\Test01\AppData,TestEnviroment\Test01\NMM\nmmInfo,TestEnviroment\Test01\NMM\nmmMod");
            Console.WriteLine("    Steam: " + opts[0]);
            Console.WriteLine("Documents: " + opts[1]);
            Console.WriteLine("  AppData: " + opts[2]);
            //Console.WriteLine(" NMM Info: " + opts[3]);
            //Console.WriteLine("  NMM Mod: " + opts[4]);
            Console.WriteLine("      NMM: " + opts[3]);
            Console.WriteLine("---------------------------------");
            //manager.updateSettings(opts[0], opts[1], opts[2], opts[3], opts[4]);
            manager.updateSettings(opts[0], opts[1], opts[2], opts[3]);
        }

        private static void listAllFiles()
        {
            String gitignoreFile = "";
            foreach (string file in Directory.EnumerateFiles(".\\", "*.*", SearchOption.AllDirectories))
            {
                gitignoreFile += file.Replace(@"\", @"/") + "\n";
            }
            File.WriteAllText(@".gitignore", gitignoreFile);
            Console.WriteLine(gitignoreFile);

        }

        private static void exec_activateInactiveProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_activateInactiveProfile");
            string profName = readOption("Profile Name");
            string hexColor = "#8332A8";
            manager.activateInactiveProfile(profName, hexColor, "15/08/2020");
        }

        private static void exec_activateDesactivatedProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_activateDesactivatedProfile");
            string profName = readOption("Profile Name");
            manager.activateDesactivatedProfile(profName);
        }

        private static void exec_desactivateActiveProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_desactivateActiveProfile");
            string profName = readOption("Profile Name");
            manager.desactivateActiveProfile(profName);
        }

        private static void exec_switchProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_switchProfile");
            string ap = readOption("Active Profile");
            string dp = readOption("Desactivated Profile");
            manager.switchProfile(ap, dp);
        }

        private static void exec_editProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_editProfile");
            string oldp = readOption("Old Profile");
            string newp = readOption("New Profile Name");
            string newc = readOption("New Color");
            string errMsg = "";
            manager.editProfile(oldp, newp, newc, out errMsg);
            Console.WriteLine("Msg:" + errMsg);
        }

        private static void exec_killAll(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_killAll");
            SteamProfileManager.killAllSteam();
        }

        private static string readOption(string optionName)
        {
            Console.Write(optionName + ":");
            string opt = Console.ReadLine();
            return opt;
        }

        public static List<string> splitCsv(string csv)
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

    }
}
