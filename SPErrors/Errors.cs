using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPErrors
{

    public static class Errors
    {
        
        // SUCCESS
        public const int SUCCESS                                          = 000;
        // GENERAL ERROS
        public const int ERR_READ_WRITE_PERM                              = 001;
        public const int ERR_PATH_NOT_EXIST                               = 002;
        public const int ERR_FILE_NOT_EXIST                               = 003;
        public const int ARGUMENT_EXCEPTION                               = 004;
        public const int ARGUMENT_NULL                                    = 005;
        public const int UNAUTHORIZED_ACCESS                              = 006;
        public const int IO_EXCEPTION                                     = 007;
        public const int DIRECTORY_NOT_FOUND                              = 008;
        public const int PATH_TOO_LONG                                    = 009;
        // PROFILES OPERATION ERRORS
        public const int ERR_INACTIVE_PROFILE_DOES_NOT_EXIST              = 100;
        public const int ERR_INACTIVE_PROFILE_CORRUPETED                  = 101;
        public const int ERR_CANNOT_CREATE_BACKUP_OF_ACTIVE               = 102;
        public const int ERR_ACTIVE_PROFILE_ALREADY_EXISTS                = 103;
        public const int ERR_INVALID_PROFILE_NAME                         = 104;
        public const int ERR_PROFILE_NAME_ALREADY_EXISTS                  = 105;
        public const int ERR_NO_INSTALLATION_TO_CREATE_PROFILE            = 106;
        public const int ERR_INVALID_COLOR_NAME                           = 107;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE                 = 108;
        public const int ERR_INVALID_SETTINGS                             = 109;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION        = 110;
        // invalid arguments
        public const int ERR_INVALID_GAME                                 = 200;
        public const int ERR_INVALID_GAME_FOLDER                          = 201;
        // NOT DEFINED
        public const int ERR_UNKNOWN                                      = 9999;


        public static string errMsg(int errNumber)
        {
            switch (errNumber)
            {
                //SUCCESS
                case SUCCESS:
                    {
                        return "SUCCESS";
                    }
                //GENERAL ERROS
                case ERR_READ_WRITE_PERM:
                    {
                        return "ERR_READ_WRITE_PERM";
                    }
                case ERR_PATH_NOT_EXIST:
                    {
                        return "ERR_PATH_NOT_EXIST";
                    }
                case ERR_FILE_NOT_EXIST:
                    {
                        return "ERR_FILE_NOT_EXIST";
                    }
                case ARGUMENT_EXCEPTION:
                    {
                        return "ARGUMENT_EXCEPTION";
                    }
                case ARGUMENT_NULL:
                    {
                        return "ARGUMENT_NULL";
                    }
                case UNAUTHORIZED_ACCESS:
                    {
                        return "UNAUTHORIZED_ACCESS";
                    }
                case IO_EXCEPTION:
                    {
                        return "IO_EXCEPTION";
                    }
                case DIRECTORY_NOT_FOUND:
                    {
                        return "DIRECTORY_NOT_FOUND";
                    }
                case PATH_TOO_LONG:
                    {
                        return "PATH_TOO_LONG";
                    }
                //PROFILESOPERATIONERRORS
                case ERR_INACTIVE_PROFILE_DOES_NOT_EXIST:
                    {
                        return "ERR_INACTIVE_PROFILE_DOES_NOT_EXIST";
                    }
                case ERR_INACTIVE_PROFILE_CORRUPETED:
                    {
                        return "ERR_INACTIVE_PROFILE_CORRUPETED";
                    }
                case ERR_CANNOT_CREATE_BACKUP_OF_ACTIVE:
                    {
                        return "ERR_CANNOT_CREATE_BACKUP_OF_ACTIVE";
                    }
                case ERR_ACTIVE_PROFILE_ALREADY_EXISTS:
                    {
                        return "ERR_ACTIVE_PROFILE_ALREADY_EXISTS";
                    }
                case ERR_INVALID_PROFILE_NAME:
                    {
                        return "ERR_INVALID_PROFILE_NAME";
                    }
                case ERR_PROFILE_NAME_ALREADY_EXISTS:
                    {
                        return "ERR_PROFILE_NAME_ALREADY_EXISTS";
                    }
                case ERR_NO_INSTALLATION_TO_CREATE_PROFILE:
                    {
                        return "ERR_NO_INSTALLATION_TO_CREATE_PROFILE";
                    }
                case ERR_INVALID_COLOR_NAME:
                    {
                        return "ERR_INVALID_COLOR_NAME";
                    }
                case ERR_CANNOT_CREATE_INTEGRITY_FILE:
                    {
                        return "ERR_CANNOT_CREATE_INTEGRITY_FILE";
                    }
                case ERR_INVALID_SETTINGS:
                    {
                        return "ERR_INVALID_SETTINGS";
                    }
                case ERR_INVALID_STATE_FOR_REQUESTED_OPERATION:
            
                    {
                        return "ERR_INVALID_STATE_FOR_REQUESTED_OPERATION";
                    }
                // NOT DEFINED
                default:
                    {
                        return "ERR_UNKNOWN";
                    }
            }
        }
    }
}
