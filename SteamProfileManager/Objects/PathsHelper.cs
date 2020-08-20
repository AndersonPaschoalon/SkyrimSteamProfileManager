using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfileManager.Enum;
using Utils;

namespace ProfileManager.Objects
{
    /// <summary>
    /// This class helps to hold and generate paths used in the project, encapsulating 
    /// the process of string checking and concatenation. Also, it holds the rules to recover
    /// the configuration files paths.
    /// </summary>
    public class PathsHelper
    {
        //#region consts
        // settings file prefix and sufix
        //private const string SETTING_PREF = "Settings\\SPConfig";
        //private const string SETTING_SUFX = ".xml";
        // Games names used to build the Settings file
        //private const string SKYRIM = "Skyrim";
        //private const string SKYRIM_SE = "SkyrimSE";
        //#endregion

        #region static_helpers

        public static string getConfigFileName()
        {
            //return   "Settings\\SPConfigSkyrim.xml";
            return getConfigFileName(Game.SKYRIM);
        }

        public static string getConfigFileName(Game game)
        {
            switch (game)
            {
                case Game.SKYRIM:
                    {
                        return Consts.SKYRIM_CONFIG_FILE;
                    }
                case Game.SKYRIM_SE:
                    {
                        return Consts.SKYRIMSE_CONFIG_FILE;
                    }
                default:
                    {
                        throw new Exception("INVALID GAME SELECTED, CONFIG FILE DOES NOT EXIST!: " + game);
                    }
            }
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

        public PathsHelper(Game game, SPSettings settings)
        {
            this._game = game;
            this.update(settings);
        }

        /// <summary>
        /// Update the paths values
        /// </summary>
        /// <param name="settings"></param>
        public void update(SPSettings settings)
        {
            this.execUpdate(settings.steamPath, settings.appDataPath, settings.documentsPath,
                            settings.nmmInfoPath, settings.nmmModPath, settings.gameFolder, 
                            settings.backupFolder);
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
            if (!this.nmmInfoEmpty)
            {
                if (!Directory.Exists(this.nmmInfo))
                {
                    return Errors.ERR_NMMINFO_DIRRECTORY_MISSING;
                }
                if (!Directory.Exists(this.nmmInfoBkp))
                {
                    return Errors.ERR_NMMINFOBKP_DIRRECTORY_MISSING;
                }
            }
            if (!this.nmmModEmpty)
            {
                if (!Directory.Exists(this.nmmMod))
                {
                    return Errors.ERR_NMMMOD_DIRRECTORY_MISSING;
                }
                if (!Directory.Exists(this.nmmModBkp))
                {
                    return Errors.ERR_NMMMODBKP_DIRRECTORY_MISSING;
                }
            }
            return Errors.SUCCESS;
        }

        // Config file
        public string getConfigFilePath()
        {
            return PathsHelper.getConfigFileName(this._game);
        }

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

        // nmmInfo
        public string nmmInfo { get { return this._nmmInfo; } }
        public string nmmInfoGame { get { return this._nmmInfoGame; } }
        public string nmmInfoBkp { get { return this._nmmInfoBackup; } }
        public string nmmInfoBkpProf(string prof) 
        { 
            return (this.nmmInfoEmpty)? "" : this._nmmInfoBackup + "\\" + prof; 
        }
        public string nmmInfoBkpProfGame(string prof) 
        { 
            return (this.nmmInfoEmpty) ? "" : this._nmmInfoBackup + "\\" + prof + "\\" + this._gameFolder; 
        }
        public bool nmmInfoEmpty { get; private set; }

        // nmmMod
        public string nmmMod { get { return this._nmmMod; } }
        public string nmmModGame { get { return this._nmmModGame; } }
        public string nmmModBkp { get { return this._nmmModBackup; } }
        public string nmmModBkpProf(string prof) 
        { 
            return (this.nmmModEmpty) ? "" : this._nmmModBackup + "\\" + prof; 
        }
        public string nmmModBkpProfGame(string prof) 
        { 
            return (this.nmmModEmpty) ? "" : this._nmmModBackup + "\\" + prof + "\\" + this._gameFolder; 
        }
        public bool nmmModEmpty { get; private set; }

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
            inUse.Add(Consts.SKYRIM);
            inUse.Add(Consts.SKYRIM_SE);
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

        private Game _game;
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

        private string _nmmInfo = "";
        private string _nmmInfoGame = "";
        private string _nmmInfoBackup = "";

        private string _nmmMod = "";
        private string _nmmModGame = "";
        private string _nmmModBackup = "";

        private void execUpdate(string steam, string appData, string myDocs, string nmmInfo,
                                string nmmMod, string gameFolder, string backupFolder)
        {
            steam.Trim();
            appData.Trim();
            myDocs.Trim();
            nmmInfo.Trim();
            nmmMod.Trim();
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

            this._nmmInfo = nmmInfo;
            this._nmmMod = nmmMod;
            this.nmmInfoEmpty = (nmmInfo.Trim().Equals("")) ? true : false;
            this.nmmModEmpty = (nmmMod.Trim().Equals("")) ? true : false;
            if (!this.nmmInfoEmpty)
            {
                this._nmmInfoGame = nmmInfo + "\\" + gameFolder;
                this._nmmInfoBackup = nmmInfo + "\\" + this._backupFolder;
            }
            if (!this.nmmModEmpty)
            {
                this._nmmModGame = nmmMod + "\\" + gameFolder;
                this._nmmModBackup = nmmMod + "\\" + this._backupFolder;
            }
        }

        #endregion private 
    }


}
