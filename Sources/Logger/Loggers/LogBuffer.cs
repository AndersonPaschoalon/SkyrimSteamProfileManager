using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Utils.Loggers
{
    public static class LogBuffer
    {
        public class LogMsg
        {
            public LogMsg(LogLevel level, string msg)
            {
                this.level = level;
                this.msg = msg;
            }

            public LogLevel level { get; private set; }
            public string msg { get; private set; }
        }

        private static Mutex mutexlogBufferList = new Mutex();
        private static List<LogMsg> logBufferList{get; set;}

        public static void appendToBuffer(LogLevel lv, string msg)
        {
            LogBuffer.mutexlogBufferList.WaitOne();
            if(logBufferList != null) logBufferList.Add(new LogMsg(lv, msg));
            LogBuffer.mutexlogBufferList.ReleaseMutex();
        }

        public static List<LogMsg> consumeBuffer()
        {
            List<LogMsg> outList = new List<LogMsg>();
            if (logBufferList!= null && logBufferList.Count > 0)
            {
                LogBuffer.mutexlogBufferList.WaitOne();
                outList = LogBuffer.logBufferList.ConvertAll(buffer => new LogMsg(buffer.level, buffer.msg));
                LogBuffer.logBufferList.Clear();
                LogBuffer.mutexlogBufferList.ReleaseMutex();
            }
            return outList;
        }

        public static void initBuffer()
        {
            if (LogBuffer.logBufferList == null)
            {
                LogBuffer.logBufferList = new List<LogMsg>();
            }
        }
    }
}
