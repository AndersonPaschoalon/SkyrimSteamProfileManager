using SteamProfileManager.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfileManager
{
    interface IProfielManager
    {

        int updateSettings(string newSteamPath, string newDocumentsPath, string newAppDataPath,
                           string nmmInfoPath, string nmmModPath, out bool isSteamOk,
                           out bool isDocOk, out bool isAppdataOk, out bool isNmmInfoOk,
                           out bool isNmmModOk);

        int activateInactiveProfile(string profileName, string color);

        int activateDesactivatedProfile(string profileName);

        int editProfile(string newProfileName, string newColor);

        int desactivateProfile(SPProfile profile);

        int switchProfile(SPProfile active, SPProfile desactivated)


    }
}
