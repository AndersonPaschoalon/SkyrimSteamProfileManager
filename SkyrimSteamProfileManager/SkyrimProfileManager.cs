using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkyrimSteamProfileManager.Objects;
using SSPErrors;

namespace SkyrimSteamProfileManager
{
    class SkyrimProfileManager
    {
        private SSPConfig config;
        private const string ACTIVE_INTEGRITY_FILE_NAME = "active_profile.int";

        public SkyrimProfileManager()
        {
            this.config = SSPConfig.getConfig();
        }

        /// <summary>
        /// Creates an active profile from existing instalation. If there is already an active profile,
        /// or if there is no installation, this method does nothing adn returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int CreateActiveProfileFromInstalation(string profileName, string color)
        {
            int profileNewId = this.config.listProfiles.profiles.Count + 1;

            foreach (var item in this.config.listProfiles.profiles)
            {
                if (profileName.Trim() == item.name.Trim())
                {
                    // name already in use
                    return Error.ERR_PROFILE_NAME_ALREADY_EXISTS;
                }
            }
            if (!this.IsColorValid(color))
            {
                return Error.ERR_INVALID_COLOR_NAME;
            }

            SSPProfile newProfile = new SSPProfile();
            newProfile.color = color;
            newProfile.id = profileNewId.ToString();
            newProfile.isActive = true.ToString();
            newProfile.name = profileName;




            // TODO
            return Error.ERR_UNKNOWN;
        }

        /// <summary>
        /// If there is no profile installed, set an innactive profile as active. Otherwise does nothing
        /// and returns an error code.
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int SetProfileActive(string profileName)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        /// <summary>
        /// Desactivate an active profile. If cannot complete the operation, does nothing and return an error code.
        /// </summary>
        /// <returns></returns>
        public int DesactivateProfile()
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        public int SwitchProfile(string profileName)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        /// <summary>
        /// Check if all the paths on the profile do exist, if all the backup paths do exist, 
        /// and if the profile is set as active the integrity file does exist in all paths. The integrity file is
        /// a file written in the root directory of paths called  active_profile_check.ssp which is a l
        /// </summary>
        /// <param name="profileName">Alpanumeric string</param>
        /// <returns></returns>
        public int CheckProfile(string profileName)
        {


            return Error.ERR_UNKNOWN;
        }


        #region private_methos

        /// <summary>
        /// Check if the settings paths are ok
        /// </summary>
        /// <returns></returns>
        int CheckSettings()
        {
            if (this.config.settings.appDataPath == null ||
                this.config.settings.documentsPath == null ||
                this.config.settings.steamPath == null)
            {
                return 1;
            }

            if (!Directory.Exists(this.config.settings.appDataPath))
            {
                return 1;
            }
            if (!Directory.Exists(this.config.settings.documentsPath))
            {
                return 1;
            }
            if (!Directory.Exists(this.config.settings.steamPath))
            {
                return 1;
            }

            if (this.config.settings.nmmInfoPath != null && !this.config.settings.nmmInfoPath.Trim().Equals(""))
            {
                if (!Directory.Exists(this.config.settings.nmmInfoPath))
                {
                    return 1;
                }
            }
            if (this.config.settings.nmmModPath != null && !this.config.settings.nmmModPath.Trim().Equals(""))
            {
                if (!Directory.Exists(this.config.settings.nmmModPath))
                {
                    return 1;
                }
            }

            // TODO
            return Error.ERR_UNKNOWN;
        }

        int CheckActiveProfile(string profileName)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        int  CheckInactiveProfile(string profileName)
        {
            // TODO
            return Error.ERR_UNKNOWN;
        }

        bool IsColorValid(string colorName)
        {
            return true;
        }

        bool CreateIntegrityFile(SSPProfile prof)
        {
            string path = this.config.settings.steamPath + "//" + this.config.settings.gameFolder + "//" + ACTIVE_INTEGRITY_FILE_NAME;
            string fileContent = prof.id + "," + prof.name + "," + prof.color;
            return false;
        }


        #endregion private_methos

    }
}
