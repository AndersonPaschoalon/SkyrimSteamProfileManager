using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Logger;
using Logger.Objects;
using SPErrors;

namespace SteamProfileManager.Objects
{
    public class Utils
    {
        private static readonly ILogger log = ConsoleLogger.getInstance();

        #region csharp

        public const string TRUE = "TRUE";
        public const string FALSE = "FALSE";

        /// <summary>
        /// Converts a string value to boolean. "false" (upper and lower case), 
        /// "0" and an empty string is considered false. Any other value
        /// is considered true by default.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool strToBool(string val)
        {
            if (val == null) val = "";
            if (val.Trim().ToUpper() == FALSE || val == "0" || val.Trim().Equals(""))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Converts a boolean value to string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string boolToStr(bool val)
        {
            return (val) ? TRUE : FALSE;
        }

        /// <summary>
        /// removes all white-spaces from a string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string strNoWhite(string val)
        {
            if (val == null) val = "";
            return val.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        }

        /// <summary>
        /// Removel all non alphanumeric elements from a string
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string alphaNumeric(string val)
        {
            string outStr = "";
            foreach (char item in val)
            {
                if (char.IsLetterOrDigit(item))
                {
                    outStr.Append(item);
                }
            }
            return outStr;
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

        public static int safeMove(string sourceDirName, string destDirName)
        {
            try
            {
                Directory.Move(sourceDirName, destDirName);
                return Errors.SUCCESS;
            }
            catch (PathTooLongException ex)
            {
                log.Warn("The specified path, file name, or both exceed the system-defined maximum length.");
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.PATH_TOO_LONG;
            }
            catch (DirectoryNotFoundException ex)
            {
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                log.Warn("The path specified by sourceDirName is invalid (for example, it is on an unmapped drive).");
                return Errors.DIRECTORY_NOT_FOUND;
            }
            catch (IOException ex)
            {
                log.Warn("An attempt was made to move a directory to a different volume. - or - " +
                         "destDirName already exists. See the Note in the Remarks section. - or - " +
                         "The sourceDirName and destDirName parameters refer to the same file or directory. - or - " +
                         "The directory or a file within it is being used by another process.");
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.IO_EXCEPTION;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Warn("The caller does not have the required permission.");
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.UNAUTHORIZED_ACCESS;
            }
            catch (ArgumentNullException ex)
            {
                log.Warn("sourceDirName or destDirName is null.");
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ARGUMENT_NULL;
            }
            catch (ArgumentException ex)
            {
                log.Warn("sourceDirName or destDirName is a zero-length string, contains only white space, " + 
                         "or contains one or more invalid characters. You can query for invalid characters with " + 
                         " the GetInvalidPathChars() method.");
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                return Errors.ARGUMENT_EXCEPTION;
            }
            catch (Exception ex)
            {
                log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            }
            return Errors.ERR_UNKNOWN;
        }

        #endregion csharp
    }
}
