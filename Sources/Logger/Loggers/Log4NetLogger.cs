using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Utils.Loggers
{
    public enum LogAppender
    {
        APP_CORE,
        APP_UI,
        APP_SETTINGS,
        APP_DEFFAULT,
    }

    public class Log4NetLogger : ILogger
    {

        #region members_variables
        // appender
        private readonly LogAppender logAppender;
        // loggers
        private static ILog loggerCore = LogManager.GetLogger("FileAppenderAppCore");
        private static ILog loggerUi = LogManager.GetLogger("FileAppenderAppUi");
        private static ILog loggerSettings = LogManager.GetLogger("FileAppenderSettings");
        private static ILog loggerDefault = LogManager.GetLogger("FileAppenderDefault");
        // instances
        private static Log4NetLogger instanceUi = null;
        private static Log4NetLogger instanceManager = null;
        private static Log4NetLogger instanceSettings = null;
        private static Log4NetLogger instanceDefault = null;
        // bugger
        private bool appendCoreToBuffer = true;
        private bool appendUiToBuffer = false;
        private bool appendSettingsToBuffer = false;
        private bool appendDefaultToBuffer = false;
        // loglevel
        private LogLevel level = LogLevel.DEBUG;
        #endregion members_variables

        #region builders

        private Log4NetLogger(LogAppender appender, string configPath)
        {
            this.logAppender = appender;
            this.level = LogConfig.getLogLevel(configPath);
        }

        ~Log4NetLogger()
        {
            Log4NetLogger.shutdown();
        }

        public static Log4NetLogger getInstance(LogAppender appender, string configPath)
        {
            LogBuffer.initBuffer(); 
            switch (appender)
            {
                case LogAppender.APP_CORE:
                    {
                        if (Log4NetLogger.instanceManager == null)
                        {
                            Log4NetLogger.instanceManager = new Log4NetLogger(LogAppender.APP_CORE, configPath);
                        }
                        return Log4NetLogger.instanceManager;
                    }
                case LogAppender.APP_UI:
                    {
                        if (Log4NetLogger.instanceUi == null)
                        {
                            Log4NetLogger.instanceUi = new Log4NetLogger(LogAppender.APP_UI, configPath);
                        }
                        return Log4NetLogger.instanceUi;
                    }
                case LogAppender.APP_SETTINGS:
                    {
                        if (Log4NetLogger.instanceSettings == null)
                        {
                            Log4NetLogger.instanceSettings = new Log4NetLogger(LogAppender.APP_SETTINGS, configPath);
                        }
                        return Log4NetLogger.instanceSettings;
                    }
                default: //LogAppender.APP_DEFFAULT:
                    {
                        if (Log4NetLogger.instanceDefault == null)
                        {
                            Log4NetLogger.instanceDefault = new Log4NetLogger(LogAppender.APP_DEFFAULT, configPath);
                        }
                        return Log4NetLogger.instanceDefault;
                    }
            }
        }

        private static void shutdown()
        {
            if (Log4NetLogger.loggerCore != null)
            {
                Log4NetLogger.loggerCore.Info("Turning OFF loggerCore");
                Log4NetLogger.loggerCore = null;
            }
            if (Log4NetLogger.loggerUi != null)
            {
                Log4NetLogger.loggerUi.Info("Turning OFF loggerUi");
                Log4NetLogger.loggerUi = null;
            }
            if (Log4NetLogger.loggerSettings != null)
            {
                Log4NetLogger.loggerSettings.Info("Turning OFF loggerUi");
                Log4NetLogger.loggerSettings = null;
            }
            if (Log4NetLogger.loggerDefault != null)
            {
                Log4NetLogger.loggerDefault.Info("Turning OFF loggerUi");
                Log4NetLogger.loggerDefault = null;
            }
            LogManager.Shutdown();
        }

        #endregion builders

        #region loggers 

        void ILogger.Debug(string msg)
        {
            if (this.level > LogLevel.DEBUG) return;

            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.sendLogToLogger(LogLevel.DEBUG, callerName + msg);
            this.appenOnBuffer(LogLevel.DEBUG, msg);
            /*
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Debug(callerName + msg);
                if (this.appendUiToBuffer) appenOnBuffer(LogLevel.DEBUG, msg);
            }
            else // LogAppender.APP_CORE
            {
                Log4NetLogger.loggerCore.Debug(callerName + msg);
                if (this.appendCoreToBuffer) appenOnBuffer(LogLevel.DEBUG, msg);
            }
            */
        }

        void ILogger.Info(string msg)
        {
            if (this.level > LogLevel.INFO) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.sendLogToLogger(LogLevel.INFO, callerName + msg);
            this.appenOnBuffer(LogLevel.INFO, msg);
            /*
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Info(callerName + msg);
                if (this.appendUiToBuffer) appenOnBuffer(LogLevel.INFO, msg);
            }
            else // LogAppender.APP_CORE
            {
                Log4NetLogger.loggerCore.Info(callerName + msg);
                if (this.appendCoreToBuffer) appenOnBuffer(LogLevel.INFO, msg);
            }
            */
        }

        void ILogger.Warn(string msg)
        {
            if (this.level > LogLevel.WARN) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.sendLogToLogger(LogLevel.WARN, callerName + msg);
            this.appenOnBuffer(LogLevel.WARN, msg);
            /*
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Warn(callerName + msg);
                if (this.appendUiToBuffer) appenOnBuffer(LogLevel.WARN, msg);
            }
            else // LogAppender.APP_CORE
            {
                Log4NetLogger.loggerCore.Warn(callerName + msg);
                if (this.appendCoreToBuffer) appenOnBuffer(LogLevel.WARN, msg);
            }
            */
        }

        void ILogger.Error(string msg)
        {
            if (this.level > LogLevel.ERROR) return;
            StackTrace stackTrace = new StackTrace();
            string callerName = "[" + stackTrace.GetFrame(1).GetMethod().ReflectedType.FullName +
                                "." + stackTrace.GetFrame(1).GetMethod().Name +
                                "()  " + stackTrace.GetFrame(1).GetFileLineNumber() + "]  ";
            this.sendLogToLogger(LogLevel.ERROR, callerName + msg);
            this.appenOnBuffer(LogLevel.ERROR, msg);
            /*
            if (this.logAppender == LogAppender.APP_UI)
            {
                Log4NetLogger.loggerUi.Error(callerName + msg);
                if (this.appendUiToBuffer) appenOnBuffer(LogLevel.ERROR, msg);
            }
            else // LogAppender.APP_CORE
            {
                Log4NetLogger.loggerCore.Error(callerName + msg);
                if (this.appendCoreToBuffer) appenOnBuffer(LogLevel.ERROR, msg);
            }
            */
        }

        #endregion loggers 

        #region helpers 

        private void sendLogToLogger(LogLevel lv, string msg)
        {
            // switch appender
            switch (this.logAppender)
            {
                case LogAppender.APP_CORE:
                    {
                        this.writeLog(Log4NetLogger.loggerCore, lv, msg);
                        break;
                    }
                case LogAppender.APP_UI:
                    {
                        this.writeLog(Log4NetLogger.loggerUi, lv, msg);
                        break;
                    }
                case LogAppender.APP_SETTINGS:
                    {
                        this.writeLog(Log4NetLogger.loggerSettings, lv, msg);
                        break;
                    }
                case LogAppender.APP_DEFFAULT:
                    {
                        this.writeLog(Log4NetLogger.loggerDefault, lv, msg);
                        break;
                    }
            }
        }

        private void writeLog(ILog logger, LogLevel lv, string msg)
        {
            switch (lv)
            {
                case LogLevel.DEBUG:
                    {
                        logger.Debug(msg);
                        break;
                    }
                case LogLevel.INFO:
                    {
                        logger.Info(msg);
                        break;
                    }
                case LogLevel.WARN:
                    {
                        logger.Warn(msg);
                        break;
                    }
                case LogLevel.ERROR:
                    {
                        logger.Error(msg);
                        break;
                    }
            }
        }

        private void appenOnBuffer(LogLevel lv, string msg)
        {
            switch (this.logAppender)
            {
                case LogAppender.APP_CORE:
                    {
                        if (this.appendCoreToBuffer)
                        {
                            LogBuffer.appendToBuffer(lv, msg);
                        }
                        break;
                    }
                case LogAppender.APP_UI:
                    {
                        if (this.appendUiToBuffer)
                        {
                            LogBuffer.appendToBuffer(lv, msg);
                        }
                        break;
                    }
                case LogAppender.APP_SETTINGS:
                    {
                        if (this.appendSettingsToBuffer)
                        {
                            LogBuffer.appendToBuffer(lv, msg);
                        }
                        break;
                    }
                case LogAppender.APP_DEFFAULT:
                    {
                        if (this.appendDefaultToBuffer)
                        {
                            LogBuffer.appendToBuffer(lv, msg);
                        }
                        break;
                    }
            }
        }

        #endregion helpers 

        /*
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
        */
    }
}
