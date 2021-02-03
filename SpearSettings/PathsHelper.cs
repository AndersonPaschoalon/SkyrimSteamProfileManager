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
                             gameSettings.gameFolder, gameSettings.backupFolder, 
                             gameSettings.boolDocumentsPathIsOptional, gameSettings.boolAppDataPathIsOptional);
            this.execUpdate2(settings.nmmPath2, gameSettings.nmmGameFolder, 
                              settings.vortexPath2, gameSettings.vortexGameFolder);
            this.execUpdate3(settings.vortexExe, settings.nmmExe, settings.tesvEditExe, gameSettings.gameExe);
            this.execUpdate4(gameSettings.gameLogPath, gameSettings.gameLogExt);
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
        public string activeIntegrityFilePath
        {
            get 
            {
                return this.steamGame + "\\" + Consts.FILE_INTEGRITYFILE;
            }
        }

        public bool activeIntegrityFile()
        {
            string intFile = this.activeIntegrityFilePath;
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
                string intFile = this.activeIntegrityFilePath;
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
            string filePath = this.activeIntegrityFilePath;
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
                File.Delete(this.activeIntegrityFilePath);
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
        public bool isAppdataDirOptional { get { return this._isAppDirOptional; } }

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
        public bool isDocsDirOptional { get { return this._isDocsOptional; } }

        #endregion documents

        #region NMM&Vortex

        // nmmMod
        public string nmm { get { return this._nmm; } }
        public string nmmGame { get { return this._nmmGame; } }

        public string nmmGameFolder { get { return this._nmmGameFolder; } }

        public string nmmBkp { get { return this._nmmBackup; } }
        public string nmmBkpProf(string prof)
        {
            return (this.nmmEmpty) ? "" : this._nmmBackup + "\\" + prof;
        }
        public string nmmBkpProfGame(string prof)
        {
            return (this.nmmEmpty) ? "" : this._nmmBackup + "\\" + prof + "\\" + this._nmmGameFolder;
        }
        public bool nmmEmpty { get; private set; }

        // Vortex
        public string vortex { get { return this._vortex; } }
        public string vortexGame { get { return this._vortexGame; } }
        public string vortexGameFolder { get { return this._vortexGameFolder; } }
        public string vortexBkp { get { return this._vortexBackup; } }
        public string vortexBkpProf(string prof)
        {
            return (this.vortexEmpty) ? "" : this._vortexBackup + "\\" + prof;
        }
        public string vortexBkpProfGame(string prof)
        {
            return (this.vortexEmpty) ? "" : this._vortexBackup + "\\" + prof + "\\" + this._vortexGameFolder;
        }
        public bool vortexEmpty { get; private set; }


        public bool optionalAreSet()
        {
            if (this.nmmEmpty && this.vortexEmpty)
            {
                return false;
            }
            return true;
        }

        #endregion NMM&Vortex

        #region Exe

        public string gameExe
        {
            get
            {
                return this.steamGame + "\\" + this._gameExe;
            }
        }

        public string nmmExe { get { return this._nmmExe; } }

        public string vortexExe { get { return this._vortexExe; } }

        public string tesvEditExe { get { return this._tesvEditExe; } }

        public string creationKitExe
        {
            get
            {
                string creationKit = "";
                if (this.gameName().ToUpper().Contains(Consts.SKYRIM_STR.ToUpper()))
                {
                    creationKit = this.steamGame + "\\" + Consts.EXE_CREATION_KIT;
                }
                return creationKit;
            }
        }


        #endregion Exe

        #region files

        public string gitignore
        {
            get 
            {
                return this.steamGame + "\\" + Consts.FILE_GITIGNORE;
            }
        }

        public List<string> gameLogsList
        {
            get 
            {
                List<string> listLogs = new List<string>();
                string logPath;
                if (!this._gameLogsPath.Trim().Equals("") &&
                    !this._gameLogsExt.Trim().Equals("") &&
                    Directory.Exists(this._gameLogsPath.Trim()))
                {
                    try
                    {
                        foreach (string file in Directory.EnumerateFiles(this._gameLogsPath,
                                                                         "*.*",
                                                                         System.IO.SearchOption.AllDirectories))
                        {
                            if (file.EndsWith(this._gameLogsExt.Trim()))
                            {
                                listLogs.Add(file);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                return listLogs;
            }
        }

        public string gameLogsPath
        {
            get 
            {
                return this._gameLogsPath.Trim();
            }
        }

        #endregion files

        #region pathsList

        public List<string> getOptionalPaths_BkpProfGame(string profileName)
        {
            List<string> optBkpPaths = new List<string>();

            // if is optional, add
            if (this._isDocsOptional)
            {
                optBkpPaths.Add(this.appDataBkpProfGame(profileName));
            }
            if (this._isAppDirOptional)
            {
                optBkpPaths.Add(this.docsBkpProfGame(profileName));
            }
            // if not empty, add
            if (!this.nmmBkpProfGame(profileName).Trim().Equals("") &&
                !this.nmm.Trim().Equals(""))
            {
                optBkpPaths.Add(this.nmmBkpProfGame(profileName));
            }
            if (!this.vortexBkpProfGame(profileName).Trim().Equals(""))
            {
                optBkpPaths.Add(this.vortexBkpProfGame(profileName));
            }
            return optBkpPaths;
        }

        public List<string> getMandatoryPaths_BkpProfGame(string profileName)
        {
            List<string> manBkpPaths = new List<string>();

            // always mandatory
            manBkpPaths.Add(this.steamBkpProfGame(profileName));
            // if not optional add
            if (!this._isDocsOptional)
            {
                manBkpPaths.Add(this.appDataBkpProfGame(profileName));
            }
            if (!this._isAppDirOptional)
            {
                manBkpPaths.Add(this.docsBkpProfGame(profileName));
            }
            return manBkpPaths;
        }

        public List<string> getAllPaths_BkpProfGame(string profileName)
        {
            List<string> list = new List<string>();
            list.AddRange(this.getMandatoryPaths_BkpProfGame(profileName));
            list.AddRange(this.getOptionalPaths_BkpProfGame(profileName));
            return list;
        }

        public List<string> getOptionalPaths_BkpProf(string profileName)
        {
            List<string> optBkpPaths = new List<string>();

            // if is optional, add
            if (this._isDocsOptional)
            {
                optBkpPaths.Add(this.appDataBkpProf(profileName));
            }
            if (this._isAppDirOptional)
            {
                optBkpPaths.Add(this.docsBkpProf(profileName));
            }
            // if not empty, add
            if (!this.nmmBkpProf(profileName).Trim().Equals(""))
            {
                optBkpPaths.Add(this.nmmBkpProf(profileName));
            }
            if (!this.vortexBkpProf(profileName).Trim().Equals(""))
            {
                optBkpPaths.Add(this.vortexBkpProf(profileName));
            }
            return optBkpPaths;
        }

        public List<string> getMandatoryPaths_BkpProf(string profileName)
        {
            List<string> manBkpPaths = new List<string>();

            // always mandatory
            manBkpPaths.Add(this.steamBkpProf(profileName));
            // if not optional add
            if (!this._isDocsOptional)
            {
                manBkpPaths.Add(this.appDataBkpProf(profileName));
            }
            if (!this._isAppDirOptional)
            {
                manBkpPaths.Add(this.docsBkpProf(profileName));
            }
            return manBkpPaths;
        }

        public List<string> getAllPaths_BkpProf(string profileName)
        {
            List<string> list = new List<string>();
            list.AddRange(this.getMandatoryPaths_BkpProf(profileName));
            list.AddRange(this.getOptionalPaths_BkpProf(profileName));
            return list;
        }

        public List<string> getOptionalPaths_App()
        {
            List<string> optAppPaths = new List<string>();

            // if is optional, add
            if (this._isDocsOptional)
            {
                optAppPaths.Add(this.appData);
            }
            if (this._isAppDirOptional)
            {
                optAppPaths.Add(this.docs);
            }
            // if not empty, add
            if (!this.nmm.Trim().Equals(""))
            {
                optAppPaths.Add(this.nmm);
            }
            if (!this.vortex.Trim().Equals(""))
            {
                optAppPaths.Add(this.vortex);
            }

            return optAppPaths;
        }

        public List<string> getMandatoryPaths_App()
        {
            List<string> manAppPaths = new List<string>();

            // always mandatory
            manAppPaths.Add(this.steam);
            // if not optional add
            if (!this._isDocsOptional)
            {
                manAppPaths.Add(this.appData);
            }
            if (!this._isAppDirOptional)
            {
                manAppPaths.Add(this.docs);
            }

            return manAppPaths;
        }

        public List<string> getAllPaths_App()
        {
            List<string> list = new List<string>();
            list.AddRange(this.getMandatoryPaths_App());
            list.AddRange(this.getOptionalPaths_App());
            return list;
        }

        public List<string> getOptionalPaths_AppGame()
        {
            List<string> optAppPaths = new List<string>();

            // if is optional, add
            if (this._isDocsOptional)
            {
                optAppPaths.Add(this.appDataGame);
            }
            if (this._isAppDirOptional)
            {
                optAppPaths.Add(this.docsGame);
            }
            // if not empty, add
            if (!this.nmm.Trim().Equals(""))
            {
                optAppPaths.Add(this.nmmGame);
            }
            if (!this.vortex.Trim().Equals(""))
            {
                optAppPaths.Add(this.vortexGame);
            }

            return optAppPaths;
        }

        public List<string> getMandatoryPaths_AppGame()
        {
            List<string> manAppPaths = new List<string>();

            // always mandatory
            manAppPaths.Add(this.steamGame);
            // if not optional add
            if (!this._isDocsOptional)
            {
                manAppPaths.Add(this.appDataGame);
            }
            if (!this._isAppDirOptional)
            {
                manAppPaths.Add(this.docsGame);
            }

            return manAppPaths;
        }

        public List<string> getAllPaths_AppGame()
        {
            List<string> list = new List<string>();
            list.AddRange(this.getMandatoryPaths_AppGame());
            list.AddRange(this.getOptionalPaths_AppGame());
            return list;
        }


        #endregion pathsList

        #region profile

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

        #endregion profile

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
        private bool _isAppDirOptional = false;

        private string _docs = "";
        private string _docsGame = "";
        private string _docsBackup = "";
        private bool _isDocsOptional = false;

        private string _nmm = "";
        private string _nmmGameFolder = "";
        private string _nmmGame = "";
        private string _nmmBackup = "";

        private string _vortex = "";
        private string _vortexGameFolder = "";
        private string _vortexGame = "";
        private string _vortexBackup = "";

        private string _gameExe = "";
        private string _nmmExe = "";
        private string _vortexExe = "";
        private string _tesvEditExe = "";
        private string _gameLogsPath = "";
        private string _gameLogsExt = "";
        
        // update paths related with steam, appData, documents, NMM, and game/backup
        private void execUpdate1(string steam, string appData, string myDocs,
                                string gameFolder, string backupFolder, 
                                bool isDocOpt, bool isAppOpt)
        {
            steam.Trim();
            appData.Trim();
            myDocs.Trim();
            gameFolder = CSharp.alphaNumeric(gameFolder);
            backupFolder = CSharp.alphaNumeric(backupFolder);

            this._gameFolder = gameFolder;
            this._backupFolder = backupFolder;
            this._isAppDirOptional = isAppOpt;
            this._isDocsOptional = isDocOpt;

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
        }

        private void execUpdate2(string nmmPath, string nmmGameFolder,
                                 string vortexPath, string vortexGameFolder)
        {
            this._nmm = nmmPath.Trim();
            this._nmmGameFolder = nmmGameFolder.Trim();
            this._vortex = vortexPath.Trim();
            this._vortexGameFolder = vortexGameFolder.Trim();

            // NMM
            this.nmmEmpty = (this._nmm.Equals("")) ? true : false;
            if (!this.nmmEmpty)
            {
                this._nmmGame = this._nmm + "\\" + _nmmGameFolder;
                this._nmmBackup = this._nmm + "\\" + this._backupFolder;
            }
            // Vortex
            this.vortexEmpty = (this._vortex.Equals("")) ? true : false;
            if (!this.vortexEmpty)
            {
                this._vortexGame = this._vortex + "\\" + this._vortexGameFolder;
                this._vortexBackup = this._vortex + "\\" + this._backupFolder;
            }
        }

        // Update information about exe files
        private void execUpdate3(string vortexExe, string nmmExe, string tesvEditExe, string gameExe)
        {
            // Tools
            if (File.Exists(vortexExe))
            {
                this._vortexExe = vortexExe;
            }
            if (File.Exists(nmmExe))
            {
                this._nmmExe = nmmExe;
            }
            if (File.Exists(tesvEditExe))
            {
                this._tesvEditExe = tesvEditExe;
            }
            // Game
            this._gameExe =  gameExe;
        }

        // game logs
        private void execUpdate4(string gameLogPath, string gameLogEx)
        {
            if (Directory.Exists(gameLogPath))
            {
                this._gameLogsPath = gameLogPath;
                this._gameExe = gameLogEx;
            }
        }

        #endregion private 
    }
}
