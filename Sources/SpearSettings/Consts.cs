using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearSettings
{
    public class Consts
    {
        // Directories
        public const string DIR_HTML = "Html\\";
        public const string DIR_LOGS = "Logs\\";
        public const string DIR_SETTINGS = "Settings\\";

        //  File Names
        public const string FILE_SPCONFIG = DIR_SETTINGS + "SPConfig.xml";
        public const string FILE_GITIGNORE = ".gitignore";
        public const string FILE_HELP_PAGE = DIR_HTML + "HelpPage.html";
        public const string FILE_PUSH_GITHUB = DIR_HTML + "CreatGithubRepository.html";
        public const string FILE_INTEGRITYFILE = "spear_profile.int";
        public const string FILE_SKYRIM_INI = "Skyrim.ini";
        public const string EXE_CREATION_KIT = "CreationKit.exe";



        // Web
        public const string WWW_GITHUB_REPOSITORY = "https://github.com/AndersonPaschoalon/SkyrimSteamProfileManager";

        // Games consts
        public const string GAME_DEFAULT = "Skyrim";
        public const string SKYRIM_STR = "Skyrim";

        // Profile Consts
        public const string INACTIVE_NAME = "~INACTIVE";
        public const string INACTIVE_COLOR = "#D3D3D3";
        public const string INACTIVE_CREATION = "-";
        public const int INTEGRITY_FILE_ITEMS = 3;

    }
}
