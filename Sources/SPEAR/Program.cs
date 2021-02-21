using SpearSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spear
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            catch (Exception ex)
            {
                string logFile = ".\\"+ Consts.DIR_LOGS + "app_exception.log";
                string logContent = "[EXCEPTION] Message:" + ex.Message + ", StackTrace:" + ex.StackTrace + Environment.NewLine;

                MessageBox.Show(logContent, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // This text is added only once to the file.
                if (!File.Exists(logFile))
                {
                    // Create a file to write to.
                    string createText = "Hello and Welcome" + Environment.NewLine;                    File.WriteAllText(logFile, logContent);
                }
                else
                {
                    File.AppendAllText(logFile, logContent);
                }
            }
        }
    }
}
