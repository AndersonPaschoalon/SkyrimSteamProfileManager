using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger_.Objects
{
    class ConsoleLogger : Logger
    {
        private ConsoleLogger()
        {
            
        }

        private static ConsoleLogger instance = null;

        //private void Write(string level, string msg)
        //{
        //    System.
        //    Console.Write("Hello ");
        //}

        public static ConsoleLogger getInstance()
        {
            if (ConsoleLogger.instance == null)
            {
                ConsoleLogger.instance = new ConsoleLogger();
            }
            return ConsoleLogger.instance;
        }
        public void Debug()
        { 
        }

        public void Info()
        {
        }

        public void Warn()
        {
        }

        public void Error()
        {
        }



    }
}
