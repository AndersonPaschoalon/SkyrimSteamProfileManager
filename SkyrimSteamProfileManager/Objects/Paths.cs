using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamProfileManager.Enum;

namespace SteamProfileManager.Objects
{
    public class Paths
    {
        #region consts
        // settings files
        private const string SETTING_PREF = "Settings\\SPConfig";
        private const string SETTING_SUFX = ".xml";
        //Games
        private const string SKYRIM = "Skyrim";
        private const string SKYRIM_SE = "SkyrimSE";
        // Backup folders
        private const string SKYRIM_BKP = "Skyrim_Backups";
        private const string SKYRIM_SE_BKP = "SkyrimSE_Backups";
        #endregion

        #region static_helpers

        public static string getConfigFileName(Game game)
        {
            switch (game)
            {
                case Game.SKYRIM:
                    {
                        return SETTING_PREF + SKYRIM + SETTING_SUFX;
                    }
                case Game.SKYRIM_SE:
                    {
                        return SETTING_PREF + SKYRIM_SE + SETTING_SUFX;
                    }
                default:
                    {
                        throw new Exception("INVALID GAME SELECTED, CONFIG FILE DOES NOT EXIST!: " + game);
                    }
            }
        }

        public static string getBackupRoot(Game game)
        {
            switch (game)
            {
                case Game.SKYRIM:
                    {
                        return SKYRIM_BKP;
                    }
                case Game.SKYRIM_SE:
                    {
                        return SKYRIM_SE_BKP;
                    }
                default:
                    {
                        throw new Exception("INVALID GAME SELECTED, CONFIG FILE DOES NOT EXIST!: " + game);
                    }
            }
        }

        public static string safeNewProfileName(string name, List<string> inUse)
        {
            // check if name is empty
            if (name == null) name = "";
            if (name.Trim().Equals(""))
            {
                name = "NO_NAME";
            }
            // make sure is alphanumeric
            Utils.alphaNumeric(name);
            //append to list of inUse Values
            inUse.Add(SKYRIM);
            inUse.Add(SKYRIM_SE);
            inUse.Add(SKYRIM_BKP);
            inUse.Add(SKYRIM_SE_BKP);
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

        #endregion static_helpers

        private string _gameFolder;

        private string _steam;
        private string _steamGame;
        private string _steamBackup;

        private string _appDir;
        private string _appDirGame;
        private string _appDirBackup;

        private string _docs;
        private string _docsGame;
        private string _docsBackup;

        private string _nmmInfo;
        private string _nmmInfoGame;
        private string _nmmInfoBackup;

        private string _nmmMod;
        private string _nmmModGame;
        private string _nmmModBackup;

        private bool nmmInfoEmpty;
        private bool nmmModEmpty;

        public Paths(string steam, string appDir, string myDocs, string nmmInfo, string nmmMod, string gameFolder)
        {
            if (steam == null) steam = "";
            if (appDir == null) appDir = "";
            if (myDocs == null) myDocs = "";
            if (nmmInfo == null) nmmInfo = "";
            if (nmmMod == null) nmmMod = "";
            if (gameFolder == null) gameFolder = "";

            this._gameFolder = gameFolder;

            this._steam = steam;
            this._steamGame;
            this._steamBackup;

            this._appDir = appDir;
            this._appDirGame;
            this._appDirBackup;

            this._docs = myDocs;
            this._docsGame;
            this._docsBackup;

            this._nmmInfo = nmmInfo;
            this._nmmMod = nmmMod;
            this.nmmInfoEmpty = (nmmInfo.Trim().Equals("")) ? true : false;
            this.nmmModEmpty = (nmmMod.Trim().Equals("")) ? true : false;

            this._nmmInfoGame;
            this._nmmInfoBackup;
            this._nmmModGame;
            this._nmmModBackup;

        }
        public Paths(SPSettings settings): this(settings.steamPath, settings.appDataPath, 
                                                settings.documentsPath, settings.nmmInfoPath,
                                                settings.nmmModPath, settings.gameFolder)
        {}

        // steam
        public string steam { get { return this._steam; } }
        public string steamGame { get { return this._steamGame; } }
        public string steamBkp { get { return this._steamBackup; } }
        public string steamBkpProf(string prof){ return this._steamBackup + "\\" + prof; }
        public string steamBkpProfGame(string prof) { return this._steamBackup + "\\" + prof + "\\" + this._gameFolder; }

        // appDir
        public string appDir { get { return this._appDir; } }
        public string appDirGame { get { return this._appDirGame; } }
        public string appDirBkp { get { return this._appDirBackup; } }
        public string appDirBkpProf(string prof) { return this._appDirBackup + "\\" + prof; }
        public string appDirBkpProfGame(string prof) { return this._appDirBackup + "\\" + prof + "\\" + this._gameFolder; }

        // docs
        public string docs { get { return this._docs; } }
        public string docsGame { get { return this._docsGame; } }
        public string docsBkp { get { return this._docsBackup; } }
        public string docsBkpProf(string prof) { return this._docsBackup + "\\" + prof; }
        public string docsBkpProfGame(string prof) { return this._docsBackup + "\\" + prof + "\\" + this._gameFolder; }

        // nmmInfo
        public string nmmInfo { get { return this._nmmInfo; } }
        public string nmmInfoGame { get { return this._nmmInfoGame; } }
        public string nmmInfoBkp { get { return this._nmmInfoBackup; } }
        public string nmmInfoBkpProf(string prof) { return (this.nmmInfoEmpty)? "" : this._nmmInfoBackup + "\\" + prof; }
        public string nmmInfoBkpProfGame(string prof) { return (this.nmmInfoEmpty) ? "" : this._nmmInfoBackup + "\\" + prof + "\\" + this._gameFolder; }


        // nmmMod
        public string nmmMod { get { return this.nmmMod; } }
        public string nmmModGame { get { return this._nmmModGame; } }
        public string nmmModBkp { get { return this._nmmModBackup; } }
        public string nmmModBkpProf(string prof) { return (this.nmmModEmpty) ? "" : this._nmmModBackup + "\\" + prof; }
        public string nmmModBkpProfGame(string prof) { return (this.nmmModEmpty) ? "" : this._nmmModBackup + "\\" + prof + "\\" + this._gameFolder; }


    }


}
