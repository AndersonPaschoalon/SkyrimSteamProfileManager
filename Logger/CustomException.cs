using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils.Loggers;

namespace Utils
{
    public class CustomException
    {
        private CustomException()
        { }

        public static void fatalError(int errorCode, string info, Exception ex)
        {

            // Console logger
            ILogger log = ConsoleLogger.getInstance();
            string errTitle = "Error " + errorCode + ": " + Errors.errMsg(errorCode);
            string errMsg = "";
            if (info == null) info = "";
            if (!info.Trim().Equals(""))
            {
                errMsg = info + ". " + ex.Message;
            }
            log.Error("** " + errTitle);
            log.Error("** " + errMsg);
            log.Error("** Message:" + ex.Message + ", StackTrace:" + ex.StackTrace);
            MessageBox.Show(errMsg, errTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            log.Info("-- closing application");
            Application.Exit();
        }


    }
}
