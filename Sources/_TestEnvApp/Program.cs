using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FormAppTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //string formName = "VORTEX";
            //string formName = "CreationKit.exe";
            //string formName = "skyrim.exe";
            //string formName = "TESVEDIT";
            //string formName = "NMM";
            string formName = "Skyrim";
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(formName));
        }
    }
}
