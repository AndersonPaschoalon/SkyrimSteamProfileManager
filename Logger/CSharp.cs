using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using Utils.Loggers;

namespace Utils
{
    /// <summary>
    /// Log method for CSharp class.
    /// </summary>
    public enum LogMethod
    {
        NONE,
        CONSOLE,
        LOGGER
    }

    public class CSharp
    {
        #region private
        private static ILogger log = ConsoleLogger.getInstance();
        #endregion private

        #region consts 
        public const string TRUE = "TRUE";
        public const string FALSE = "FALSE";
        #endregion consts

        #region logger

        public static void setLogger(ILogger newLogger)
        {
            if (newLogger != null)
            {
                log = newLogger;
            }
        }

        #endregion logger

        #region primitiveOperations

        public static bool checkNotEmpty(string[] strs)
        {
            foreach (var item in strs)
            {
                if(item == null || item.Trim().Equals(""))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool checkNotNull(object[] objs)
        {
            foreach (var item in objs)
            {
                if (item == null)
                {
                    return false;
                }
            }
            return true;
        }

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
                    outStr += item;
                }
            }
            return outStr;
        }

        // TODO
        // first char need to be a letter, the other alphanumeric
        public static string varName(string val)
        {
            return val;
        }

        #endregion primitiveOperations

        #region parsingHelpers


        public static string listToCsv(List<string> list)
        {
            if (list == null) return "";

            string outStr = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    outStr += list[i].Trim();
                    break;
                }
                else
                {
                    outStr += list[i].Trim() + ",";
                }
            }
            return outStr;
        }

        public static string arrayToCsv(string[] arr)
        {
            if (arr == null) return "";

            string outStr = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == arr.Length - 1)
                {
                    outStr += arr[i].Trim();
                    break;
                }
                else
                {
                    outStr += arr[i].Trim() + ",";
                }
            }
            return outStr;
        }


        public static List<string> csvToList(string csv)
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

        #endregion parsingHelpers

        #region fileSystemOperations

        public static int safeMove(string sourceDirName, string destDirName)
        {
            LogMethod logMethod = LogMethod.NONE;
            return safeMove(sourceDirName, destDirName, logMethod);
        }

        public static int safeMove(string sourceDirName, string destDirName, LogMethod logMethod)
        {
            try
            {
                Directory.Move(sourceDirName, destDirName);
                return Errors.SUCCESS;
            }
            catch (PathTooLongException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("The specified path, file name, or both exceed the system-defined maximum length.");
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("The specified path, file name, or both exceed the system-defined maximum length.");
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }



                return Errors.PATH_TOO_LONG;
            }
            catch (DirectoryNotFoundException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                    log.Warn("The path specified by sourceDirName is invalid (for example, it is on an unmapped drive).");
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                    Console.WriteLine("The path specified by sourceDirName is invalid (for example, it is on an unmapped drive).");
                }

                return Errors.DIRECTORY_NOT_FOUND;
            }
            catch (IOException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("An attempt was made to move a directory to a different volume. - or - " +
                             "destDirName already exists. See the Note in the Remarks section. - or - " +
                             "The sourceDirName and destDirName parameters refer to the same file or directory. - or - " +
                             "The directory or a file within it is being used by another process.");
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("An attempt was made to move a directory to a different volume. - or - " +
                             "destDirName already exists. See the Note in the Remarks section. - or - " +
                             "The sourceDirName and destDirName parameters refer to the same file or directory. - or - " +
                             "The directory or a file within it is being used by another process.");
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }

                return Errors.IO_EXCEPTION;
            }
            catch (UnauthorizedAccessException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("The caller does not have the required permission.");
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("The caller does not have the required permission.");
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                return Errors.UNAUTHORIZED_ACCESS;
            }
            catch (ArgumentNullException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("sourceDirName or destDirName is null.");
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("sourceDirName or destDirName is null.");
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                return Errors.ARGUMENT_NULL;
            }
            catch (ArgumentException ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("sourceDirName or destDirName is a zero-length string, contains only white space, " +
                             "or contains one or more invalid characters. You can query for invalid characters with " +
                             " the GetInvalidPathChars() method.");
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("sourceDirName or destDirName is a zero-length string, contains only white space, " +
                             "or contains one or more invalid characters. You can query for invalid characters with " +
                             " the GetInvalidPathChars() method.");
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                return Errors.ARGUMENT_EXCEPTION;
            }
            catch (Exception ex)
            {
                if (logMethod == LogMethod.LOGGER)
                {
                    log.Warn("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
                else if (logMethod == LogMethod.CONSOLE)
                {
                    Console.WriteLine("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
                }
            }
            return Errors.ERR_UNKNOWN;
        }

        /// <summary>
        /// Execute a sequence of dirMv operations. if any fail, it will undo all operations executed.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="destinations"></param>
        /// <param name="showUi"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool stackMv(string[] sources, string[] destinations, bool showUi,
                                   out string errMsg)
        {
            LogMethod logMethod = LogMethod.NONE;
            return stackMv(sources, destinations, showUi, logMethod, out errMsg);
        }


        /// <summary>
        /// Execute a sequence of dirMv operations. if any fail, it will undo all operations executed.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="destinations"></param>
        /// <param name="showUi"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool stackMv(string[] sources, string[] destinations, bool showUi,
                                   LogMethod logMethod, out string errMsg)
        {
            string errSrc = "";
            string errDst = "";
            return stackMv(sources, destinations, showUi, logMethod, 
                           out errMsg, out errSrc, out errDst);
        }

        /// <summary>
        /// Execute a sequence of dirMv operations. if any fail, it will undo all operations executed.
        /// </summary>
        /// <param name="sources"></param>
        /// <param name="destinations"></param>
        /// <param name="showUi"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public static bool stackMv(string[] sources, string[] destinations, bool showUi,
                                   LogMethod logMethod, out string errMsg, out string errSrcDir, 
                                   out string errDstDir)
        {
            if (sources == null || destinations == null)
            {
                errMsg = "NULL VECTOR ERROR";
                errSrcDir = "";
                errDstDir = "";
                return false;
            }
            if (sources.Length != destinations.Length)
            {
                errMsg = "SOURCE and DESTINATION stack sizes do not match! src:" + sources.Length +
                         ", dst:" + destinations.Length;
                errSrcDir = "";
                errDstDir = "";
                return false;
            }
            Stack<string> dirStackSrc = new Stack<string>();
            Stack<string> dirStackDst = new Stack<string>();
            for (int i = 0; i < sources.Length; i++)
            {
                try
                {
                    if (logMethod == LogMethod.LOGGER)
                    {
                        log.Debug("mv -r \"" + sources[i] + "\" \"" + destinations[i] + "\"");
                    }
                    else if (logMethod == LogMethod.CONSOLE)
                    {
                        Console.WriteLine("mv -r \"" + sources[i] + "\" \"" + destinations[i] + "\"");
                    }
                    
                    if (!sources[i].Trim().Equals("") && !destinations[i].Trim().Equals(""))
                    {
                        dirMv(sources[i], destinations[i], showUi);
                        dirStackSrc.Push(sources[i]);
                        dirStackDst.Push(destinations[i]);
                    }
                }
                catch (Exception ex)
                {
                    while (dirStackSrc.Count > 0)
                    {
                        undoMv(dirStackSrc.Pop(), dirStackDst.Pop(), showUi);
                    }
                    errSrcDir = sources[i];
                    errDstDir = destinations[i];
                    errMsg = ex.Message;
                    return false;
                }
            }
            errSrcDir = "";
            errDstDir = "";
            errMsg = "SUCCESS!";
            return true;
        }

        /// <summary>
        /// Undo the operation executed by a dirMv() method.
        /// </summary>
        /// <param name="usedSource"></param>
        /// <param name="usedDestination"></param>
        /// <param name="showUi"></param>
        public static void undoMv(string usedSource, string usedDestination, bool showUi)
        {
            // move src  dst : move "TestDir\\t2"  "TestDir\\t1"  
            // before "TestDir\\t2" "TestDir\\t1" => after: "TestDir\\t1" "TestDir\\t1\\t2"
            // undomove: move source "TestDir\\t1\\t2" ; move destination "TestDir\\"
            // new source
            char[] charSeparators = new char[] { '\\' };
            string[] dirsSrc = usedSource.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            string lastElementSrc = dirsSrc[dirsSrc.Length - 1];
            string newSource = usedDestination + "\\" + lastElementSrc;
            // new dst
            string[] dirsDst = usedDestination.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            string newDst = "";
            for (int i = 0; i < dirsDst.Length - 1; i++)
            {
                if (i == 0)
                {
                    newDst = dirsDst[0];
                }
                else
                {
                    newDst = "\\" + dirsDst[i];
                }
            }
            newDst = newDst + "\\";
            dirMv(newSource, newDst, showUi);

        }

        /// <summary>
        /// Execute the move operation using the linux mv -r sintax, where the destination will be 
        /// the parent directory of the moved one.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="showUi"></param>
        public static void dirMv(string source, string destination, bool showUi)
        {
            char[] charSeparators = new char[] { '\\' };
            string[] dirs = source.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            string lastElement = dirs[dirs.Length - 1];
            destination = destination + "\\" + lastElement + "\\";

            if (showUi)
            {
                FileSystem.MoveDirectory(source,
                                         destination,
                                         UIOption.AllDialogs,
                                         UICancelOption.DoNothing);
            }
            else
            {
                Directory.Move(source, destination);
            }
        }

        /// <summary>
        /// Check if a list of directories exist. If one of them does not exist, it is returned
        /// as out parameter and the method returns false.
        /// </summary>
        /// <param name="dirs"></param>
        /// <param name="errDir"></param>
        /// <returns></returns>
        public static bool checkDirs(string[] dirs, out string errDir)
        {
            foreach (var item in dirs)
            {
                if (!Directory.Exists(item))
                {
                    errDir = item;
                    return false;
                }
            }
            errDir = "";
            return true;
        }

        #endregion fileSystemOperations

    }
}