using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using Logger.Loggers;
using ProfileManager;
using ProfileManager.Enum;

namespace UiConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            ILogger log = Log4NetLogger.getInstance(LogAppender.APP_UI);

            SteamProfileManager manager = new SteamProfileManager(Game.SKYRIM);
            bool quit = false;
            while (true)
            {
                displayMenu();
                string opt = Console.ReadLine();
                switch (opt)
                {
                    case "00":
                        {
                            displayMenu();
                            break;
                        }
                    case "01":
                        {
                            showState(manager);
                            break;
                        }
                    case "02":
                        {
                            Console.WriteLine("TODO");
                            break;
                        }
                    case "03":
                        {
                            updateConfig(manager);
                            break;
                        }
                    case "04":
                        {
                            updateConfigCsv(manager);
                            break;
                        }
                    case "05":
                        {
                            updateConfigCsvDefault(manager);
                            break;
                        }
                    case "20":
                        {
                            exec_activateInactiveProfile(manager);
                            break;
                        }
                    case "30":
                        {
                            exec_activateDesactivatedProfile(manager);
                            break;
                        }
                    case "40":
                        {
                            exec_desactivateActiveProfile(manager);
                            break;
                        }
                    case "50":
                        {
                            exec_switchProfile(manager);
                            break;
                        }
                    case "60":
                        {
                            exec_editProfile(manager);
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

            Console.WriteLine("20   activateInactiveProfile");
            Console.WriteLine("30   activateDesactivatedProfile");
            Console.WriteLine("40   desactivateActiveProfile");
            Console.WriteLine("50   switchProfile");
            Console.WriteLine("60   editProfile");

            Console.WriteLine("99   QUIT");
            Console.WriteLine("PATH: " + Directory.GetCurrentDirectory());
        }

        private static void showState(SteamProfileManager manager)
        {
            manager.showState();
        }

        // TestEnviroment\Test01\Docs

        private static void updateConfig(SteamProfileManager manager)
        {
            Console.WriteLine("> updateConfig");
            string newSteamPath = readOption("SteamPath");
            string newDocumentsPath = readOption("DocumentsPath"); 
            string newAppDataPath = readOption("AppDataPath");
            string nmmInfoPath = readOption("nmmInfoPath");
            string nmmModPath = readOption("nmmModPath");
            bool isSteamOk = false;
            bool isDocOk = false; 
            bool isAppdataOk = false; 
            bool isNmmInfoOk = false;
            bool isNmmModOk = false;
            manager.updateSettings(newSteamPath,  newDocumentsPath,  newAppDataPath, nmmInfoPath, nmmModPath, 
                                   out  isSteamOk, out  isDocOk, out  isAppdataOk, out  isNmmInfoOk, out  isNmmModOk);
        }

        private static void updateConfigCsv(SteamProfileManager manager)
        {
            Console.WriteLine("> updateConfigCsv");
            List<string> opts = splitCsv(readOption("Steam,Docs,AppData,nmmInfo,nmmMod:"));
            Console.WriteLine("    Steam: " + opts[0]);
            Console.WriteLine("Documents: " + opts[1]);
            Console.WriteLine("  AppData: " + opts[2]);
            Console.WriteLine(" NMM Info: " + opts[3]);
            Console.WriteLine("  NMM Mod: " + opts[4]);
            bool isSteamOk = false;
            bool isDocOk = false;
            bool isAppdataOk = false;
            bool isNmmInfoOk = false;
            bool isNmmModOk = false;
            manager.updateSettings(opts[0], opts[1], opts[2], opts[3], opts[4],
                       out isSteamOk, out isDocOk, out isAppdataOk, out isNmmInfoOk, out isNmmModOk);
        }

        private static void updateConfigCsvDefault(SteamProfileManager manager)
        {
            Console.WriteLine("> updateConfigCsvDefault");
            //List<string> opts = splitCsv(readOption("Steam,Docs,AppData,nmmInfo,nmmMod:"));
            List<string> opts = splitCsv(@"TestEnviroment\Test01\Steam,TestEnviroment\Test01\Docs,TestEnviroment\Test01\AppData,TestEnviroment\Test01\nmmInfo,TestEnviroment\Test01\nmmMod");
            Console.WriteLine("    Steam: " + opts[0]);
            Console.WriteLine("Documents: " + opts[1]);
            Console.WriteLine("  AppData: " + opts[2]);
            Console.WriteLine(" NMM Info: " + opts[3]);
            Console.WriteLine("  NMM Mod: " + opts[4]);
            bool isSteamOk = false;
            bool isDocOk = false;
            bool isAppdataOk = false;
            bool isNmmInfoOk = false;
            bool isNmmModOk = false;
            manager.updateSettings(opts[0], opts[1], opts[2], opts[3], opts[4],
                       out isSteamOk, out isDocOk, out isAppdataOk, out isNmmInfoOk, out isNmmModOk);
        }

        private static void exec_activateInactiveProfile(SteamProfileManager manager)
        {
            Console.WriteLine("> exec_activateInactiveProfile");
            string profName = readOption("Profile Name");
            string hexColor = "#8332A8";
            manager.activateInactiveProfile(profName, hexColor);
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
            manager.editProfile(oldp, newp, newc);
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
