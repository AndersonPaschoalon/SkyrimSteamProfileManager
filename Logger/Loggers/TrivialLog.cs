using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logger.Loggers
{
    public class TrivialLog : ILogger
    {
        private const string LVDEBUG = "DEBUG";
        private const string LVWARN  = "WARN ";
        private const string LVINFO  = "INFO ";
        private const string LVERROR = "ERROR";
        private const string DEFLOGFILE = "defaultlogfile";
        private string logfile = "";
        // loglevel
        private LogLevel level = LogLevel.DEBUG;
        // static member
        private static Dictionary<string, TrivialLog>  instancesDic = new Dictionary<string, TrivialLog>();
        private static Mutex mutDic = new Mutex();
        private static Mutex mutFile = new Mutex();
        private TrivialLog(string inLogfileName)
        {
            if (inLogfileName == null || inLogfileName.Trim().Equals(""))
            {
                this.logfile = DEFLOGFILE;
            }
            else
            {
                this.logfile = inLogfileName;
            }
            
        }

        #region interface

        public static TrivialLog getInstance(string keyFile)
        {
            TrivialLog tlog = null;
            bool res = false;
            if (keyFile == null || keyFile.Trim().Equals(""))
            {
                keyFile = DEFLOGFILE;
            }

            // critical region Dic
            TrivialLog.mutDic.WaitOne();
            res = TrivialLog.instancesDic.TryGetValue(keyFile, out tlog);
            TrivialLog.mutDic.ReleaseMutex();
            // end critical region Dic

            if (res)
            {
                return tlog;
            }
            else
            {
                tlog = new TrivialLog(keyFile);
                TrivialLog.addNew(keyFile, tlog);
            }
            return tlog;
        }

        public void Debug(string msg, string className)
        {
            if (this.level > LogLevel.DEBUG) return;
            this.WriteLog(msg, TrivialLog.LVDEBUG, className);
        }
        
        public void Info(string msg, string className)
        {
            if (this.level > LogLevel.INFO) return;
            this.WriteLog(msg, TrivialLog.LVINFO, className);
        }

        public void Warn(string msg, string className)
        {
            if (this.level > LogLevel.WARN) return;
            this.WriteLog(msg, TrivialLog.LVWARN, className);
        }
        public void Error(string msg, string className)
        {
            this.WriteLog(msg, TrivialLog.LVERROR, className);
        }

        public void Debug(string msg)
        {
            if (this.level > LogLevel.DEBUG) return;
            this.WriteLog(msg, TrivialLog.LVDEBUG, "");
        }

        public void Info(string msg)
        {
            if (this.level > LogLevel.INFO) return;
            this.WriteLog(msg, TrivialLog.LVINFO, "");
        }

        public void Warn(string msg)
        {
            if (this.level > LogLevel.WARN) return;
            this.WriteLog(msg, TrivialLog.LVWARN, "");
        }
        public void Error(string msg)
        {
            if (this.level > LogLevel.ERROR) return;
            this.WriteLog(msg, TrivialLog.LVERROR, "");
        }

        #endregion interface

        /// <summary>
        /// Add a instance of TivialLog to the dictionary if it does not exists. Thread safe method.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        private static void addNew(string key, TrivialLog val)
        {
            if (key == null || key.Trim().Equals(""))
            {
                return;
            }
            // critical region Dic
            TrivialLog.mutDic.WaitOne();
            if (!TrivialLog.instancesDic.ContainsKey(key))
            {
               TrivialLog.instancesDic.Add(key, val);
            }
            TrivialLog.mutDic.ReleaseMutex();
            // end critical region Dic
        }

        /// <summary>
        /// Write the log in a file. Thread safe method
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="loglevel"></param>
        /// <param name="className"></param>
        private void WriteLog(string msg, string loglevel, string className)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            // logfile name
            string logFilePath = this.logfile + "-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "log";
            // log msg
            string logLine = "";
            if (className == null || className.Trim().Equals(""))
            {
                logLine = System.DateTime.Today.ToString("MM-dd-yyyy") + loglevel  + "    " + msg;
            }
            else
            {
                logLine = System.DateTime.Today.ToString("MM-dd-yyyy") + loglevel  + "  [" + className + "]  " + msg;
            }

            // critical region file
            TrivialLog.mutFile.WaitOne();
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(logLine);
            Console.WriteLine(logLine);
            log.Close();
            TrivialLog.mutFile.ReleaseMutex();
            // end of critical region file
        }

    }
}
