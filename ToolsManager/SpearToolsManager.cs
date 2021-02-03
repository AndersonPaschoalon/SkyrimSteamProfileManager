using SpearSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.Loggers;

namespace ToolsManager
{
    public class SpearToolsManager
    {
        // const 
        //private const string FILE_GITIGNORE = ".gitignore";
        private readonly ILogger log;
        // readonly
        private readonly string theGame;
        private readonly SPGame gameSettings;
        // app state
        private PathsHelper paths;                  // helper for generating the right names of the paths
        private SPSettings settings;

        public SpearToolsManager(string gameName)
        {
            log = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            CSharp.setLogger(Log4NetLogger.getInstance(LogAppender.APP_CORE));
            this.theGame = gameName;
            SPConfig config = SPConfig.loadConfig();
            if (config != null)
            {
                log.Debug("-- config.selectSettings() game:" + gameName);
                this.settings = config.settings;
                this.gameSettings = config.selectGame(gameName);
                this.paths = new PathsHelper(this.settings, this.gameSettings);
            }
            else
            {
                log.Warn("COULD NOT LOAD CONFIGURATION FILE");
                this.settings = null;
                this.gameSettings = null;
                this.paths = null;
            }
        }

        public static bool exportLogsAsZip(string dstPath)
        {
            const string zipFile = @".\logs-spear.zip";
            const string zipPath = @".\ZipTempDir\";
            const string cmdDeleteZip = @"/C del /f .\logs-spear.zip";
            const string cmdDeleteDir = @"/C rd /S /Q ZipTempDir";
            const string cmdCreate = @"/C mkdir ZipTempDir";
            const string cmdCopyLogs = @"/C xcopy /Y /s .\Logs\* .\ZipTempDir\";
            const string cmdCopySettings = @"/C xcopy /Y /s .\Settings\* .\ZipTempDir\";
            string cmdMoveToDst = @"move .\logs-spear.zip " + dstPath;

            System.Diagnostics.Process.Start("CMD.exe", cmdDeleteZip);
            System.Diagnostics.Process.Start("CMD.exe", cmdDeleteDir);
            System.Diagnostics.Process.Start("CMD.exe", cmdCreate);
            System.Diagnostics.Process.Start("CMD.exe", cmdCopyLogs);
            System.Diagnostics.Process.Start("CMD.exe", cmdCopySettings);
            ZipFile.CreateFromDirectory(zipPath, zipFile);
            System.Diagnostics.Process.Start("CMD.exe", cmdMoveToDst);

            return true;
        }

        /// <summary>
        /// Kill all steam processes. Requires elevation to execute.
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public static void killAllSteam()
        {
            string killSteam = "taskkill /f /im Steam.exe";
            string killSteamService = "taskkill /f /im SteamService.exe";
            string killSteamHelper = "taskkill /f /im steamwebhelper.exe";
            Process.Start("CMD.exe", killSteam);
            Process.Start("CMD.exe", killSteamService);
            Process.Start("CMD.exe", killSteamHelper);
        }

        public bool gitignoreDetected()
        {
            string gitignorePath = this.paths.gitignore;
            if (File.Exists(gitignorePath))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool createGitignore(out string errMsg)
        {
            log.Debug(" -- createGitignore()");
            string gitignorePath = this.paths.gitignore;
            String gitignoreContent = "";
            try
            {
                if (!File.Exists(gitignorePath))
                {
                    foreach (string file in Directory.EnumerateFiles(this.paths.steamGame,
                                                                     "*.*",
                                                                     System.IO.SearchOption.AllDirectories))
                    {
                        string content = file.Replace(this.paths.steamGame, "");
                        content = content.Replace(@"\", @"/");
                        gitignoreContent += @"." + content + "\n";
                    }
                    File.WriteAllText(gitignorePath, gitignoreContent);
                    errMsg = "";
                    return true;
                }
                errMsg = ".gitignore file already exist!";
            }
            catch (Exception ex)
            {
                log.Error("** ERROR ** Error creating .gitignore file. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                errMsg = "Exception:<" + ex.Message + ">";
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool deleteGitignore(out string errMsg)
        {
            log.Debug(" -- deleteGitignore()");
            string gitignorePath = this.paths.gitignore;
            if (File.Exists(gitignorePath))
            {
                File.Delete(gitignorePath);
                errMsg = "";
                return true;
            }
            errMsg = ".gitignore file does not exist!";
            return false;
        }

    }
}
