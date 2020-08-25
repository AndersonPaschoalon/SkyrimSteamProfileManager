using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private const string LV_DEBUG = "DEBUG   ";
        private const string LV_INFO =  "INFO    ";
        private const string LV_WARN =  "WARN    ";
        private const string LV_ERROR = "ERROR   ";

        // loglevel
        private LogLevel level = LogLevel.DEBUG;

        private ConsoleLogger()
        {  
        }

        private static ConsoleLogger instance = null;

        private void Write(string level, string msg)
        {
            // critical region 
            Console.WriteLine(level + msg);
            // end critical region 
        }

        public static ConsoleLogger getInstance()
        {
            // critical region 
            if (ConsoleLogger.instance == null)
            {
                ConsoleLogger.instance = new ConsoleLogger();
            }
            // end critical region 
            return ConsoleLogger.instance;
        }
        public void Debug(string msg)
        {
            if (this.level > LogLevel.DEBUG) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.Write(LV_DEBUG, callerName + msg);
        }

        public void Info(string msg)
        {
            if (this.level > LogLevel.INFO) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.Write(LV_INFO, callerName + msg);
        }

        public void Warn(string msg)
        {
            if (this.level > LogLevel.WARN) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.Write(LV_WARN, callerName + msg);
        }

        public void Error(string msg)
        {
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.Write(LV_ERROR, callerName + msg);
        }

    }
}
