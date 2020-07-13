using Logger_.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger_
{
    public class LoggerFactory
    {
        public enum LogType
        {
            CONSOLE,
            LOG4NET
        }


        private LoggerFactory()
        { 
        }

        public static Logger getLogger(LogType ltype)
        {
            Logger logger = null;
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
