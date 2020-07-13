using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger.Objects
{
    public class ConsoleLogger : Logger
    {
        private const string LV_DEBUG = "DEBUG   ";
        private const string LV_INFO =  "INFO    ";
        private const string LV_WARN =  "WARN    ";
        private const string LV_ERROR = "ERROR   ";

        private ConsoleLogger()
        {  
        }

        private static ConsoleLogger instance = null;

        private void Write(string level, string msg)
        {
            // critical region 
            Console.Write(level + msg);
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
            this.Write(LV_DEBUG, msg);
        }

        public void Info(string msg)
        {
            this.Write(LV_INFO, msg);
        }

        public void Warn(string msg)
        {
            this.Write(LV_WARN, msg);
        }

        public void Error(string msg)
        {
            this.Write(LV_ERROR, msg);
        }

    }
}
