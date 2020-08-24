using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utils.Loggers
{
    [XmlRoot("LOG", IsNullable = false)]
    public class LogConfig
    {
        private const string LOG_CONFIG = "Utils.logConfig.xml";
        private const string LV_DEBUG = "DEBUG";
        private const string LV_INFO  = "INFO";
        private const string LV_WARN  = "WARN";
        private const string LV_ERROR = "ERROR";

        public static LogLevel getLogLevel()
        {
            // read xml into a string
            //string LOG_CONFIG = "Utils.logConfig.xml";
            string readText = "";
            if (File.Exists(LOG_CONFIG))
            {
                readText = File.ReadAllText(LOG_CONFIG);
            }
            else
            {
                string createText = "<?xml version=\"1.0\" encoding=\"utf - 8\" ?>" +
                                    Environment.NewLine +
                                    "< LOG loglevel = \"DEBUG\" />" +
                                    Environment.NewLine;
                File.WriteAllText(LOG_CONFIG, createText, Encoding.UTF8);
                readText = File.ReadAllText(LOG_CONFIG);
            }

            // deserialize XML string
            LogConfig logConfig;
            var serializer = new XmlSerializer(typeof(LogConfig));
            using (TextReader reader = new StringReader(readText))
            {
                logConfig = (LogConfig)serializer.Deserialize(reader);
                reader.Close();
            }
            string logLevel =  logConfig.loglevel;
            if (logLevel == null) logLevel = LV_DEBUG;
            logLevel.ToUpper();

            switch (logLevel)
            {
                case LV_DEBUG:
                    {
                        return LogLevel.DEBUG;
                    }
                case LV_INFO:
                    {
                        return LogLevel.INFO;
                    }
                case LV_WARN:
                    {
                        return LogLevel.WARN;
                    }
                case LV_ERROR:
                    {
                        return LogLevel.ERROR;
                    }
                default:
                    {
                        return LogLevel.DEBUG;
                    }
            }
        }


        public LogConfig()
        { }

        [XmlAttribute("loglevel")]
        public string loglevel { get; set; }
    }
}
