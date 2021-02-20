using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SpearSettings
{
    public class UserSettings
    {


        public UserSettings(in string pnmmPath, 
                            in string pvortexPath,
                            in string pnmmGameFolder, 
                            in string pvortexGameFolder,
                            in string pnmmExe, 
                            in string pvortexExe, 
                            in string ptesveditExe)
        {

            this.nmmPath = pnmmPath;
            this.vortexPath = pvortexPath;
            this.nmmGameFolder = pnmmGameFolder;
            this.vortexGameFolder = pvortexGameFolder;
            this.nmmExe = pnmmExe;
            this.vortexExe = pvortexExe;
            this.tesveditExe = ptesveditExe;
        }

        public string nmmPath { get;  private set; }
        public string vortexPath { get; private set; }
        public string nmmGameFolder { get; private set; }
        public string vortexGameFolder { get; private set; }
        public string nmmExe { get; private set; }
        public string vortexExe { get; private set; }
        public string tesveditExe { get; private set; }
        public string vortexPathGameFolder 
        { 
            get 
            {
                return this.vortexPath + "\\" + this.vortexGameFolder;
            } 
        }
        public string nmmPathGameFolder
        {
            get
            {
                return this.nmmPath + "\\" + this.nmmGameFolder;
            }
        }

    }
}
