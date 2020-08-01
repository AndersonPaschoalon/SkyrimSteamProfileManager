using Logger.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class LoggerFactory
    {
        public enum LogType
        {
            CONSOLE,
            LOG4NET,
            TRIVIAL_LOG
        }


        private LoggerFactory()
        { 
        }

        public static ILogger getLogger(LogType ltype)
        {
            ILogger logger = null;
            switch(ltype)
            {
                case LogType.CONSOLE: 
                    {
                        logger = ConsoleLogger.getInstance();
                        break;
                    }
                case LogType.LOG4NET:
                    {
                        logger = ConsoleLogger.getInstance();
                        break;
                    }
                default:
                    {
                        logger = ConsoleLogger.getInstance();
                        break;
                    }
            }
            return logger;
        }
    }
}
