using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Errors
    {

        // SUCCESS
        public const int SUCCESS = 000;
        // GENERAL ERROS
        public const int ERR_READ_WRITE_PERM = 001;
        public const int ERR_PATH_NOT_EXIST = 002;
        public const int ERR_FILE_NOT_EXIST = 003;
        public const int ARGUMENT_EXCEPTION = 004;
        public const int ARGUMENT_NULL = 005;
        public const int UNAUTHORIZED_ACCESS = 006;
        public const int IO_EXCEPTION = 007;
        public const int DIRECTORY_NOT_FOUND = 008;
        public const int PATH_TOO_LONG = 009;
        public const int ERR_MOVING_DIRECTORIES = 010;
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
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION = 110;
        public const int ERR_PARSING_INTEGRITY_FILE = 111;
        public const int ERR_COULD_NOT_OPEN_INTEGRIY_FILE = 112;
        public const int ERR_ACTIVE_PROFILE_CORRUPTED = 113;
        public const int ERR_STEAM_DIRRECTORY_MISSING = 114;
        public const int ERR_DOCUMENTS_DIRRECTORY_MISSING = 115;
        public const int ERR_APPDATA_DIRRECTORY_MISSING = 116;
        public const int ERR_STEAMBKP_DIRRECTORY_MISSING = 117;
        public const int ERR_DOCUMENTSBKP_DIRRECTORY_MISSING = 118;
        public const int ERR_APPDATABKP_DIRRECTORY_MISSING = 119;
        public const int ERR_STEAMGAME_DIRRECTORY_MISSING = 200;
        public const int ERR_DOCUMENTSGAME_DIRRECTORY_MISSING = 201;
        public const int ERR_APPDATAGAME_DIRRECTORY_MISSING = 202;
        public const int ERR_NMMINFO_DIRRECTORY_MISSING = 203;
        public const int ERR_NMMINFOBKP_DIRRECTORY_MISSING = 204;
        public const int ERR_NMMINFOGAME_DIRRECTORY_MISSING = 205;
        public const int ERR_NMMMOD_DIRRECTORY_MISSING = 206;
        public const int ERR_NMMMODBKP_DIRRECTORY_MISSING = 207;
        public const int ERR_NMMMODGAME_DIRRECTORY_MISSING = 208;
        public const int ERR_NMMDIRRECTORY_MISSING = 209;
        public const int ERR_NMMBKP_DIRRECTORY_MISSING = 210;
        // invalid arguments
        public const int ERR_INVALID_GAME = 200;
        public const int ERR_INVALID_GAME_FOLDER = 201;
        // NOT DEFINED
        public const int ERR_UNKNOWN = 9999;


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
                case ERR_PARSING_INTEGRITY_FILE:
                    {
                        return "ERR_PARSING_INTEGRITY_FILE";
                    }
                case ERR_COULD_NOT_OPEN_INTEGRIY_FILE:
                    {
                        return "ERR_COULD_NOT_OPEN_INTEGRIY_FILE";
                    }
                case ERR_ACTIVE_PROFILE_CORRUPTED:
                    {
                        return "ERR_ACTIVE_PROFILE_CORRUPTED";
                    }
                case ERR_STEAM_DIRRECTORY_MISSING:
                    {
                        return "ERR_STEAM_DIRRECTORY_MISSING";
                    }
                case ERR_DOCUMENTS_DIRRECTORY_MISSING:
                    {
                        return "ERR_DOCUMENTS_DIRRECTORY_MISSING";
                    }
                case ERR_APPDATA_DIRRECTORY_MISSING:
                    {
                        return "ERR_APPDATA_DIRRECTORY_MISSING";
                    }
                case ERR_STEAMBKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_STEAMBKP_DIRRECTORY_MISSING";
                    }
                case ERR_DOCUMENTSBKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_DOCUMENTSBKP_DIRRECTORY_MISSING";
                    }
                case ERR_APPDATABKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_APPDATABKP_DIRRECTORY_MISSING";
                    }
                case ERR_STEAMGAME_DIRRECTORY_MISSING:
                    {
                        return "ERR_STEAMGAME_DIRRECTORY_MISSING";
                    }
                case ERR_DOCUMENTSGAME_DIRRECTORY_MISSING:
                    {
                        return "ERR_DOCUMENTSGAME_DIRRECTORY_MISSING";
                    }
                case ERR_APPDATAGAME_DIRRECTORY_MISSING:
                    {
                        return "ERR_APPDATAGAME_DIRRECTORY_MISSING";
                    }
                case ERR_NMMINFO_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMINFO_DIRRECTORY_MISSING";
                    }
                case ERR_NMMINFOBKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMINFOBKP_DIRRECTORY_MISSING";
                    }
                case ERR_NMMINFOGAME_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMINFOGAME_DIRRECTORY_MISSING";
                    }
                case ERR_NMMMOD_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMMOD_DIRRECTORY_MISSING";
                    }
                case ERR_NMMMODBKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMMODBKP_DIRRECTORY_MISSING";
                    }
                case ERR_NMMMODGAME_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMMODGAME_DIRRECTORY_MISSING";
                    }
                case ERR_NMMDIRRECTORY_MISSING:
                    {
                        return "ERR_NMMDIRRECTORY_MISSING";
                    }
                case ERR_NMMBKP_DIRRECTORY_MISSING:
                    {
                        return "ERR_NMMBKP_DIRRECTORY_MISSING";
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
