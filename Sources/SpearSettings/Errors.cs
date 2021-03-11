using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpearSettings
{
    public static class Errors
    {

        // SUCCESS
        public const int SUCCESS = 000;
        // GENERAL ERROS
        public const int ERR_PATH_NOT_EXIST = 001;
        public const int ERR_FILE_NOT_EXIST = 002;
        public const int ERR_ARGUMENT_NULL = 003;
        public const int ERR_MOVING_DIRECTORIES_1 = 004;
        public const int ERR_MOVING_DIRECTORIES_2 = 005;
        public const int ERR_MOVING_DIRECTORIES_3 = 006;
        public const int ERR_INVALID_PROFILE_NAME_1 = 007;
        public const int ERR_INVALID_PROFILE_NAME_2 = 008;
        public const int ERR_INVALID_PROFILE_NAME_3 = 009;
        public const int ERR_PROFILE_NAME_ALREADY_EXISTS_1 = 010;
        public const int ERR_PROFILE_NAME_ALREADY_EXISTS_2 = 011;
        public const int ERR_PROFILE_NAME_ALREADY_EXISTS_3 = 012;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE_1 = 013;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE_2 = 014;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE_3 = 015;
        public const int ERR_CANNOT_CREATE_INTEGRITY_FILE_4 = 116;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_1 = 017;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_2 = 018;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_3 = 019;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_4 = 020;
        public const int ERR_INVALID_STATE_FOR_REQUESTED_OPERATION_5 = 021;
        public const int ERR_ACTIVE_PROFILE_CORRUPTED_1 = 022;
        public const int ERR_ACTIVE_PROFILE_CORRUPTED_2 = 023;
        public const int ERR_STEAM_DIRRECTORY_MISSING_1 = 024;
        public const int ERR_STEAM_DIRRECTORY_MISSING_2 = 025;
        public const int ERR_DOCUMENTS_DIRRECTORY_MISSING_1 = 026;
        public const int ERR_DOCUMENTS_DIRRECTORY_MISSING_2 = 027;
        public const int ERR_APPDATA_DIRRECTORY_MISSING_1 = 028;
        public const int ERR_APPDATA_DIRRECTORY_MISSING_2 = 028;
        public const int ERR_STEAMBKP_DIRRECTORY_MISSING = 029;
        public const int ERR_DOCUMENTSBKP_DIRRECTORY_MISSING = 030;
        public const int ERR_APPDATABKP_DIRRECTORY_MISSING = 031;
        public const int ERR_NMMDIRRECTORY_MISSING = 032;
        public const int ERR_NMMBKP_DIRRECTORY_MISSING = 033;
        public const int ERR_CANNOT_CREATE_DIRECTORY = 034;
        public const int ERR_INCONSISTENT_SRC_DST_DIR_NUMBER = 035;
        public const int ERR_INVALID_GAME_NAME_1 = 036;
        public const int ERR_INVALID_GAME_NAME_2 = 037;
        public const int ERR_INVALID_GAME_NAME_3 = 038;
        public const int ERR_SAVING_CONFIGURATION_FILE = 039;
        public const int ERR_INVALID_GAME_FOLDER = 040;
        public const int ERR_INVALID_GAME_EXE = 041;
        public const int ERR_INVALID_BACKUP_FOLDER = 042;
        public const int ERR_EXCEPTION_1 = 043;
        public const int ERR_EXCEPTION_2 = 044;
        public const int ERR_EXCEPTION_3 = 045;
        public const int ERR_EXCEPTION_4 = 046;
        public const int ERR_EXCEPTION_5 = 046;
        public const int ERR_EXCEPTION_6 = 046;
        public const int ERR_VORTEXDIRRECTORY_MISSING = 047;
        public const int ERR_VORTEXBKP_DIRRECTORY_MISSING = 048;
        public const int ERR_SOURCE_DESTINATION_DONT_MATCH_ACTIVATEDESACTIVATED = 049;
        public const int ERR_SOURCE_DESTINATION_DONT_MATCH_DESACTIVATEACTIVE = 050;
        public const int ERR_PATHS_LABELS_SIZE_DONT_MATCH_1 = 051;
        public const int ERR_PATH_NOT_FOUND_STEAM = 052;
        public const int ERR_PATH_NOT_FOUND_DOCUMENTS = 053;
        public const int ERR_PATH_NOT_FOUND_APPDATA = 054;
        public const int ERR_PATH_NOT_FOUND_NMM = 055;
        public const int ERR_PATH_NOT_FOUND_VORTEX = 056;
        public const int ERR_PATH_NOT_FOUND_DEFAULT = 057;
        public const int ERR_PATH_NULL_OR_EMPTY = 058;
        // INFO
        public const int INFO_OPERATION_CANCELLED_BY_USER = 100;


        public static string errMsg(int errNumber)
        {
            return errNumber.ToString();
            /*
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
                case ERR_CANNOT_SAVE_SETTINGS:
                    {
                        return "ERR_CANNOT_SAVE_SETTINGS";
                    }
                case ERR_CANNOT_CREATE_DIRECTORY:
                    {
                        return "ERR_CANNOT_CREATE_DIRECTORY";
                    }
                case ERR_INCONSISTENT_SRC_DST_DIR_NUMBER:
                    {
                        return "ERR_INCONSISTENT_SRC_DST_DIR_NUMBER";
                    }
                // NOT DEFINED
                default:
                    {
                        return "ERR_UNKNOWN";
                    }
            }
            */
        }
    }
}
