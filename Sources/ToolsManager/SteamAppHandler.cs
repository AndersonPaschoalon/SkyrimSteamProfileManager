using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace ToolsManager
{
    class SteamAppHandler
    {

        /// <summary>
        /// Kill all steam processes. Requires elevation to execute.
        /// </summary>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        public static void killAllSteam(out string errMsg)
        {
            errMsg = "";
            string killSteam = "taskkill /f /im Steam.exe";
            string killSteamService = "taskkill /f /im SteamService.exe";
            string killSteamHelper = "taskkill /f /im steamwebhelper.exe";
            try
            {
                Process.Start("CMD.exe", killSteam);
            }
            catch (Exception ex)
            {
                errMsg += "<" + killSteam + "> ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace + "\r\n";
            }
            try
            {
                Process.Start("CMD.exe", killSteamService);
            }
            catch (Exception ex)
            {
                errMsg += "<" + killSteamService + "> ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace + "\r\n";
            }
            try
            {
                Process.Start("CMD.exe", killSteamHelper);
            }
            catch (Exception ex)
            {
                errMsg += "<" + killSteamHelper + "> ** Exception on killAllSteam. Message:" + ex.Message + ", StackTrace:" + ex.StackTrace + "\r\n";
            }
        }

    }
}
