using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ProfileManager.Enum;
using Utils;

namespace SpearSettings
{
    /// <summary>
    /// This class helps to hold and generate paths used in the project, encapsulating 
    /// the process of string checking and concatenation. Also, it holds the rules to recover
    /// the configuration files paths.
    /// </summary>
    public class PathsHelper
    {
        #region static_helpers

        public static string getConfigFileName()
        {
            return Consts.FILE_SPCONFIG;
        }

        public static string validPath(string path)
        {
            if (File.Exists(path))
            {
                return path;
            }
            else if (Directory.Exists(path))
            {
                return path;
            }
            return "";
        }

        #endregion static_helpers

        public PathsHelper(SPSettings settings, SPGame gameSettings)
        {
            this.update(settings, gameSettings);
        }

        public string gameName()
        {
            return this._game;
        }

        /// <summary>
        /// Update the paths values
        /// </summary>
        /// <param name="settings"></param>
        public void update(SPSettings settings, SPGame gameSettings)
        {
            this._game = gameSettings.game;
            this.execUpdate1(settings.steamPath, settings.appDataPath, settings.documentsPath,
                             settings.nmmPath, gameSettings.gameFolder, gameSettings.backupFolder);
            this.execUpdate2(settings.vortexPath, settings.tesvEditPath, gameSettings.gameExe);
        }

        public int checkSettings()
        {
            if (!Directory.Exists(this.steam))
            {
                return Errors.ERR_STEAM_DIRRECTORY_MISSING;
            }
            if (!Directory.Exists(this.steamBkp))
            {
                return Errors.ERR_STEAMBKP_DIRRECTORY_MISSING;
            }
            if (!Directory.Exists(this.docs))
            {
                return Errors.ERR_DOCUMENTS_DIRRECTORY_MISSING;
            }
            if (!Directory.Exists(this.docsBkp))
            {
                return Errors.ERR_DOCUMENTSBKP_DIRRECTORY_MISSING; 
            }
            if (!Directory.Exists(this.appData))
            {
                return Errors.ERR_APPDATA_DIRRECTORY_MISSING;
            }
            if (!Directory.Exists(this.appDataBkp))
            {
                return Errors.ERR_APPDATABKP_DIRRECTORY_MISSING;
            }
            if (!this.optionalAreSet())
            {
                if (!Directory.Exists(this.nmm))
                {
                    return Errors.ERR_NMMDIRRECTORY_MISSING;
                }
                if (!Directory.Exists(this.nmmBkp))
                {
                    return Errors.ERR_NMMBKP_DIRRECTORY_MISSING;
                }
            }
            return Errors.SUCCESS;
        }

        #region integrity_files 

        // active integrity helpers
        public string activeIntegrityFilePath()
        {
            return this.steamGame + "\\" + Consts.FILE_INTEGRITYFILE;
        }

        public bool activeIntegrityFile()
        {
            string intFile = this.activeIntegrityFilePath();
            if (File.Exists(intFile))
            {
                return true;
            }
            return false;
        }

        public string activeIntegrityFileContent()
        {
            if (this.activeIntegrityFile())
            {
                string intFile = this.activeIntegrityFilePath();
                string integrityFileContent = File.ReadAllText(intFile);
                return integrityFileContent;
            }
            return "";
        }

        public List<string> activeIntegrityFileItems()
        {
            string content = this.activeIntegrityFileContent();
            return CSharp.csvToList(content);
        }

        public bool updateActiveIntegrityFile(SPProfile prof, out string errMsg, out string errPath)
        {
            string content = prof.name + "," + prof.color + "," + prof.creationDate;
            string filePath = this.activeIntegrityFilePath();
            try
            {
                File.WriteAllText(filePath, content);
                errMsg = "SUCCESS";
                errPath = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errPath = filePath;
                return false;
            }
        }

        public bool deleteActiveIntegrityFile()
        {
            try
            {
                File.Delete(this.activeIntegrityFilePath());
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        // desactivated integrity file helpers
        public string desactivatedIntegrityFilePath(string prof)
        {
            return this.steamBkpProfGame(prof) + "\\" + Consts.FILE_INTEGRITYFILE;
        }

        public bool desactivatedIntegrityFile(string prof)
        {
            string intFile = this.desactivatedIntegrityFilePath(prof);
            if (File.Exists(intFile))
            {
                return true;
            }
            return false;
        }

        public string desactivatedIntegrityFileContent(string prof)
        {
            if (this.desactivatedIntegrityFile(prof))
            {
                string intFile = this.desactivatedIntegrityFilePath(prof);
                string integrityFileContent = File.ReadAllText(intFile);
                return integrityFileContent;
            }
            return "";
        }

        public List<string> desactivatedIntegrityFileItems(string prof)
        {
            string content = this.desactivatedIntegrityFileContent(prof);
            return CSharp.csvToList(content);
        }

        public bool updateDesactivatedIntegrityFile(SPProfile prof, out string errMsg, out string errPath)
        {
            string content = prof.name + "," + prof.color + "," + prof.creationDate;
            string filePath = this.desactivatedIntegrityFilePath(prof.name);
            try
            {
                File.WriteAllText(filePath, content);
                errMsg = "SUCCESS";
                errPath = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errPath = filePath;
                return false;
            }
        }

        #endregion integrity_files 

        #region steam 
        // steam
        public string steam { get { return this._steam; } }
        public string steamGame { get { return this._steamGame; } }
        public string steamBkp { get { return this._steamBackup; } }
        public string steamBkpProf(string prof)
        { 
            return this._steamBackup + "\\" + prof; 
        }
        public string steamBkpProfGame(string prof) 
        { 
            return this._steamBackup + "\\" + prof + "\\" + this._gameFolder; 
        }

        #endregion steam 

        #region appData

        // appData
        public string appData { get { return this._appData; } }
        public string appDataGame { get { return this._appDirGame; } }
        public string appDataBkp { get { return this._appDirBackup; } }
        public string appDataBkpProf(string prof) 
        { 
            return this._appDirBackup + "\\" + prof; 
        }
        public string appDataBkpProfGame(string prof) 
        { 
            return this._appDirBackup + "\\" + prof + "\\" + this._gameFolder; 
        }

        #endregion appData

        #region documents

        // docs
        public string docs { get { return this._docs; } }
        public string docsGame { get { return this._docsGame; } }
        public string docsBkp { get { return this._docsBackup; } }
        public string docsBkpProf(string prof) 
        { 
            return this._docsBackup + "\\" + prof; 
        }
        public string docsBkpProfGame(string prof) 
        { 
            return this._docsBackup + "\\" + prof + "\\" + this._gameFolder; 
        }

        #endregion documents

        #region NMM

        // nmmMod
        public string nmm { get { return this._nmm; } }
        public string nmmGame { get { return this._nmmGame; } }
        public string nmmBkp { get { return this._nmmBackup; } }
        public string nmmBkpProf(string prof)
        {
            return (this.nmmEmpty) ? "" : this._nmmBackup + "\\" + prof;
        }
        public string nmmBkpProfGame(string prof)
        {
            return (this.nmmEmpty) ? "" : this._nmmBackup + "\\" + prof + "\\" + this._gameFolder;
        }
        public bool nmmEmpty { get; private set; }

        public bool optionalAreSet()
        {
            if (nmmEmpty)
            {
                return false;
            }
            return true;
        }

        #endregion NMM

        #region files

        public string gitignore()
        {
            return this.steamGame + "\\" + Consts.FILE_GITIGNORE;
        }

        public string gameExe()
        {
            return this.steamGame + "\\" + this._gameExe;
        }

        public List<string> gameLogsList()
        {
            List<string> listLogs = new List<string>();
            string logPath;
            // logfiles for skyrim
            if (this.gameName().ToUpper() == Consts.SKYRIM_STR.ToUpper())
            {
                logPath = this.skyrimLogPath();
                if (File.Exists(logPath))
                {
                    foreach (string file in Directory.EnumerateFiles(logPath,
                                                                     "*.*",
                                                                     System.IO.SearchOption.AllDirectories))
                    {
                        if (file.EndsWith(".log"))
                        {
                            listLogs.Add(file);
                        }
                    }
                }

            }
            return listLogs;
        }

        public string creationKitExe()
        {
            string creationKit = "";
            if (this.gameName().ToUpper() == Consts.SKYRIM_STR.ToUpper())
            {
                creationKit = this.steamGame + "\\" + Consts.EXE_CREATION_KIT;
            }
            return creationKit;
        }


        public string nmmExe()
        {
            // TODO retornar valor correto
            return this.nmm;
        }

        public string vortex()
        {
            return this._vortexExe;
        }

        public string tesvedit()
        {
            return this._tesvEditExe;
        }

        public string skyrimLogPath()
        {
            string logPath = "";
            if (this.gameName().ToUpper() == Consts.SKYRIM_STR.ToUpper())
            {
                logPath = this.docsGame + "\\Logs\\";
            }
            return logPath;
        }

        #endregion files

            /// <summary>
            /// Returns a valid profile name.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="inUse"></param>
            /// <returns></returns>
            public string safeNewProfileName(string name, List<string> inUse)
        {
            // check if name is empty
            if (name == null) name = "";
            if (name.Trim().Equals(""))
            {
                name = "NO_NAME";
            }
            // make sure is alphanumeric
            CSharp.alphaNumeric(name);
            //append to list of inUse Values
            List<string> lg = SPConfig.listGameNames();
            if (lg != null)
            {
                foreach (var item in lg)
                {
                    inUse.Add(item);
                }
            }
            inUse.Add(this._gameFolder);
            inUse.Add(this._backupFolder);
            // if value already exist, create a counter
            int sufix = 0;
            string cleanName = name;
            while (inUse.Contains(name))
            {
                sufix++;
                name = cleanName + "_" + sufix.ToString();
            }
            return name;
        }

        #region private 

        private string _game;
        private string _gameFolder = "";
        private string _backupFolder = "";

        private string _steam = "";
        private string _steamGame = "";
        private string _steamBackup = "";

        private string _appData = "";
        private string _appDirGame = "";
        private string _appDirBackup = "";

        private string _docs = "";
        private string _docsGame = "";
        private string _docsBackup = "";

        private string _nmm = "";
        private string _nmmGame = "";
        private string _nmmBackup = "";

        private string _gameExe;
        private string _vortexExe = "";
        private string _tesvEditExe = "";
        private string _creationKitExe = "";
        private string _skyrimLogsPath = "";
        private string _skyrimIniFile = "";
        
        // update paths related with steam, appData, documents, NMM, and game/backup
        private void execUpdate1(string steam, string appData, string myDocs, string nmm,
                                string gameFolder, string backupFolder)
        {
            steam.Trim();
            appData.Trim();
            myDocs.Trim();
            nmm.Trim();
            gameFolder = CSharp.alphaNumeric(gameFolder);
            backupFolder = CSharp.alphaNumeric(backupFolder);

            this._gameFolder = gameFolder;
            this._backupFolder = backupFolder;

            if (!steam.Trim().Equals(""))
            {
                this._steam = steam;
                this._steamGame = steam + "\\" + gameFolder;
                this._steamBackup = steam + "\\" + this._backupFolder;
            }
            if (!appData.Trim().Equals(""))
            {
                this._appData = appData;
                this._appDirGame = appData + "\\" + gameFolder;
                this._appDirBackup = appData + "\\" + this._backupFolder;
            }
            if (!myDocs.Trim().Equals(""))
            {
                this._docs = myDocs;
                this._docsGame = myDocs + "\\" + gameFolder;
                this._docsBackup = myDocs + "\\" + this._backupFolder;
            }

            this._nmm = nmm;
            this.nmmEmpty = (nmm.Trim().Equals("")) ? true : false;
            if (!this.nmmEmpty)
            {
                this._nmmGame = nmm + "\\" + gameFolder;
                this._nmmBackup = nmm + "\\" + this._backupFolder;
            }
        }

        // Update information about aditional toosl Skyrim .ini and logs
        private void execUpdate2(string vortex, string tesvEdit, string gameExe)
        {
            // Tools
            if (File.Exists(vortex))
            {
                this._vortexExe = vortex;
            }
            if (File.Exists(tesvEdit))
            {
                this._tesvEditExe = tesvEdit;
            }

            // Game
            this._gameExe =  gameExe;

            // Skyrim
            this._skyrimIniFile = this.docsGame + "\\" + Consts.FILE_SKYRIM_INI;
            this._skyrimLogsPath = this.docsGame + "\\Logs\\";
        }

        #endregion private 
    }
}
