using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ProfileManager;
using Utils;
using Utils.Loggers;

namespace Tester
{
    public class Tests
    {
        public static int countAcc;
        public static int countErr;

        public static void run()
        {
            countAcc = 0;
            countErr = 0;

            bool test01 = false;
            bool test02 = true;

            if (test01)
                testSPConfigXml();
            if (test02)
                testLogger();
        }

        public static void testHeader(string msg)
        {
            Console.WriteLine("");
            Console.WriteLine("*******************************************************");
            Console.WriteLine("* " + msg);
            Console.WriteLine("*******************************************************");
        }

        public static void testSPConfigXml()
        {
            testHeader("SPConfig.xml");

            /*
            string configFile = "SPConfig_Test.xml";
            XmlSerializer serializer = new XmlSerializer(typeof(SPConfig));
            FileStream fs = new FileStream(configFile, FileMode.Open);
            SPConfig config;
            config = (SPConfig)serializer.Deserialize(fs);

            if(!Assert.Equals("appDataPath-3", config.settings.appDataPath, "appDataPath do not match")) 
                return;
            if (!Assert.Equals("documentsPath-2", config.settings.documentsPath, "appDataPath do not match"))
                return;
            if (!Assert.Equals(3, config.listProfiles.profiles.Count, "number of profiles do not match"))
                return;

            SPProfile testProf = new SPProfile();
            testProf.color = "AZUL";
            testProf.isReady = "TRUE";
            testProf.name = "Test_Serialization_back_to_file";
            config.listProfiles.profiles.Add(testProf);
            config.saveConfig("saveConfigTest_SPConfig_Test.xml");
            */
            countAcc++;
        }


        public static void testLogger()
        {
            ILogger logConsole = ConsoleLogger.getInstance();
            ILogger logLog4net = Log4NetLogger.getInstance(LogAppender.APP_CORE, ".\\");
            ILogger logLog4netUi = Log4NetLogger.getInstance(LogAppender.APP_UI, ".\\");
            ILogger logTlog = TrivialLog.getInstance("triviallogTest.log");

            logLog4net.Debug("TEST LOG4NET Debug");
            logLog4net.Info("TEST LOG4NET Info");
            logLog4net.Warn("TEST LOG4NET Warn");
            logLog4net.Error("TEST LOG4NET Error");

            logLog4netUi.Debug("TEST LOG4NET Debug");
            logLog4netUi.Info("TEST LOG4NET Info");
            logLog4netUi.Warn("TEST LOG4NET Warn");
            logLog4netUi.Error("TEST LOG4NET Error");
        }
    }
}
