using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfileManager.Enum
{
    public enum SPMState
    {
        // Only configuration operation is permited
        NOT_CONFIGURED = 0,
        // Only configuration operation is permited
        NO_PROFILE = 1,
        // configuration and activation operation is permited 
        INACTIVE_PROFILE = 2,
        // configuration, desactivation and switch operations are permited
        ACTIVE_AND_DESACTIVATED_PROFILES = 3,
        // configuration and desactivation operations are permited
        ACTIVE_ONLY = 4,
        // configuration and activation operations are permited
        DESACTIVATED_ONLY = 5
    }
}
