using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Logger.Objects
{
    public class Log4NetLogger : ILogger
    {
        private Log4NetLogger()
        {
            this.logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            this.logger.Info("** Log4Net initialized successefully!");
        }

        private readonly log4net.ILog logger;
        private static Log4NetLogger instance = null;


        public static Log4NetLogger getInstance()
        {
            // critical region 
            if (Log4NetLogger.instance == null)
            {
                Log4NetLogger.instance = new Log4NetLogger();
            }
            // end critical region 
            return Log4NetLogger.instance;
        }

        void ILogger.Debug(string msg)
        {
            this.logger.Debug(msg);
        }

        void ILogger.Info(string msg)
        {
            this.logger.Info(msg);
        }

        void ILogger.Warn(string msg)
        {
            this.logger.Warn(msg);
        }

        void ILogger.Error(string msg)
        {
            this.logger.Error(msg);
        }
    }
}
