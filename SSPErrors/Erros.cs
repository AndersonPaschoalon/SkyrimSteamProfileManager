using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPErrors
{

    public static class Error
    {
        // SUCCESS
        public const int SUCCESS = 0;

        // GENERAL ERROS
        public const int ERR_READ_WRITE_PERM = 001;
        public const int ERR_PATH_NOT_EXIST= 002;
        public const int ERR_FILE_NOT_EXIST = 003;

        // PROFILES OPERATION ERRORS
        public const int ERR_INACTIVE_PROFILE_DOES_NOT_EXIST = 100;
        public const int ERR_INACTIVE_PROFILE_CORRUPETED = 101;
        public const int ERR_CANNOT_CREATE_BACKUP_OF_ACTIVE = 102;
        public const int ERR_ACTIVE_PROFILE_ALREADY_EXISTS = 103;
        public const int ERR_INVALID_PROFILE_NAME = 104;
        public const int ERR_PROFILE_NAME_ALREADY_EXISTS = 105;
        public const int ERR_NO_INSTALLATION_TO_CREATE_PROFILE = 106;
        public const int ERR_INVALID_COLOR_NAME = 107;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE = 108;
        public const int ERR_INVALID_SETTINGS = 109;

        // NOT DEFINED
        public const int ERR_UNKNOWN = 9999;


        public static string ErrMsg(int errNumber)
        {
            switch(errNumber)
            {
                // sucesss
                case SUCCESS:
                {
                    return "Success!";
                }

                // PROFILES OPERATION ERRORS
                case ERR_READ_WRITE_PERM:
                {
                    return "Error reading/writing file";
                }

                // PROFILES OPERATION ERRORS
                case ERR_INACTIVE_PROFILE_DOES_NOT_EXIST:
                {
                    return "Inactive profile does not exist!";
                }
                case ERR_INACTIVE_PROFILE_CORRUPETED:
                {
                    return "Inactive profile is corrupted!";
                }
                default:
                {
                    break;
                }
            }
            return "Unknown error!";
        }
    }


}
