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
        private readonly ILogger log;
        private readonly string theGame;
        private readonly SPGame gameSettings;
        private readonly PathsHelper paths;                  // helper for generating the right names of the paths
        private readonly SPSettings settings;

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
            ILogger logger = Log4NetLogger.getInstance(LogAppender.APP_CORE);
            try
            {
                Process.Start("CMD.exe", killSteam);
            }
            catch (Exception ex)
            {
                logger.Warn(" ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            try
            {
                Process.Start("CMD.exe", killSteamService);
            }
            catch (Exception ex)
            {
                logger.Warn(" ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            try
            {
                Process.Start("CMD.exe", killSteamHelper);
            }
            catch (Exception ex)
            {
                logger.Warn(" ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
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
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool createGitignore(out string errMsg)
        {
            log.Debug(" -- createGitignore()");
            string gitignorePath = this.paths.gitignore;
            string gitignoreContent = "";
            string content = "";
            try
            {
                if (!File.Exists(gitignorePath))
                {
                    foreach (string file in Directory.EnumerateFiles(this.paths.steamGame,
                                                                     "*.*",
                                                                     System.IO.SearchOption.AllDirectories))
                    {
                        content = "";
                        content = file.Replace(this.paths.steamGame, "").Trim();
                        // replace \ bar by / bar
                        content = content.Replace(@"\", @"/"); 
                        // add scape characters whitespace " ", "[", "]"
                        content = content.Replace(" ", @"\ ").Replace("[", @"\[").Replace("]", @"\]");
                        // if the first character is /, skip it
                        if (content[0] == '/')
                        {
                            content = content.Substring(1);
                        }
                        // skip spear profile file
                        if (content != Consts.FILE_INTEGRITYFILE)
                        {
                            // skip line 
                            gitignoreContent += content + "\n";
                        }
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
