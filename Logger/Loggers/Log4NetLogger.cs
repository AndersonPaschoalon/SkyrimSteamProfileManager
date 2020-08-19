using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Logger.Loggers
{
    public enum LogAppender
    {
        MANAGER,
        APP_UI
    }

    public class Log4NetLogger : ILogger
    {
        private readonly LogAppender logAppender;
        // loggers
        private static ILog loggerManager = LogManager.GetLogger("FileAppenderManager");
        private static ILog loggerUi = LogManager.GetLogger("FileAppenderUi");
        // instances
        private static Log4NetLogger instanceUi = null;
        private static Log4NetLogger instanceManager = null;
        // loglevel
        private LogLevel level = LogLevel.DEBUG;

        private Log4NetLogger(LogAppender app)
        {
            this.logAppender = app;
        }

        public static Log4NetLogger getInstance(LogAppender appender)
        {
            if (appender == LogAppender.APP_UI)
            {
                if (Log4NetLogger.instanceUi == null)
                {
                    Log4NetLogger.instanceUi = new Log4NetLogger(LogAppender.APP_UI);
                }
                return Log4NetLogger.instanceUi;
            }
            else // LogAppender.MANAGER
            {
                if (Log4NetLogger.instanceManager == null)
                {
                    Log4NetLogger.instanceManager = new Log4NetLogger(LogAppender.MANAGER);
                }
                return Log4NetLogger.instanceManager;
            }
        }

        public static void shutdown()
        {
            if (loggerManager != null)
            {
                loggerManager.Info("Turning OFF loggerManager");
                loggerManager = null;
            }
            if (loggerUi != null)
            {
                loggerUi.Info("Turning OFF loggerUi");
                loggerUi = null;
            }
            LogManager.Shutdown();
        }

        void ILogger.Debug(string msg)
        {
            if (this.level > LogLevel.DEBUG) return;

            StackTrace stackTrace = new StackTrace();
            //string callerName = "[" + this.filterFileName(stackTrace.GetFrame(1).GetFileName()) +
            //                    " "+ stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName + 
            //                    " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Debug(callerName + msg);
            }
            else // LogAppender.MANAGER
            {
                Log4NetLogger.loggerManager.Debug(callerName + msg);
            }
        }

        void ILogger.Info(string msg)
        {
            if (this.level > LogLevel.INFO) return;
            StackTrace stackTrace = new StackTrace();
            //String callerName = "[" + this.filterFileName(stackTrace.GetFrame(1).GetFileName()) +
            //                    " " + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
            //                    " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Info(callerName + msg);
            }
            else // LogAppender.MANAGER
            {
                Log4NetLogger.loggerManager.Info(callerName + msg);
            }
        }

        void ILogger.Warn(string msg)
        {
            if (this.level > LogLevel.WARN) return;
            StackTrace stackTrace = new StackTrace();
            //String callerName = "[" + this.filterFileName(stackTrace.GetFrame(1).GetFileName()) +
            //                    " " + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
            //                    " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Warn(callerName + msg);
            }
            else // LogAppender.MANAGER
            {
                Log4NetLogger.loggerManager.Warn(callerName + msg);
            }
        }

        void ILogger.Error(string msg)
        {
            if (this.level > LogLevel.ERROR) return;
            StackTrace stackTrace = new StackTrace();
            //String callerName = "[" + this.filterFileName(stackTrace.GetFrame(1).GetFileName()) +
            //                    " " + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
            //                    " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                " " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Error(callerName + msg);
            }
            else // LogAppender.MANAGER
            {
                Log4NetLogger.loggerManager.Error(callerName + msg);
            }
        }

        private string filterFileName(string fileName)
        {
            if (fileName == null)
            {
                return "";
            }
            string[] directories = fileName.Split(Path.DirectorySeparatorChar);
            int len = directories.Length;
            if (directories.Length == 0)
            {
                return "";
            }
            return directories[directories.Length - 1];
        }
    }
}
