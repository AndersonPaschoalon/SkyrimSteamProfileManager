using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManager.Objects
{
    public class Consts
    {
        // integrity file
        public const string ACTIVE_INTEGRITY_FILE_NAME = "active_profile.int";
        //  configuration files
        public const string SKYRIM_CONFIG_FILE = SETTING_PREF + SKYRIM + SETTING_SUFX;
        public const string SKYRIMSE_CONFIG_FILE = SETTING_PREF + SKYRIM_SE + SETTING_SUFX;
        // Games names
        public const string SKYRIM = "Skyrim";
        public const string SKYRIM_SE = "SkyrimSE";
        // Game Folder
        public const string DIR_SKYRIM = "Skyrim";
        public const string DIR_SKYRIM_SE = "Skyrim SE";

        #region private 
        // settings file prefix and sufix
        private const string SETTING_PREF = "Settings\\SPConfig";
        private const string SETTING_SUFX = ".xml";
        #endregion private 
    }
}
