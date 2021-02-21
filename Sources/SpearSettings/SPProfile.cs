using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utils;
using Utils.Loggers;

namespace SpearSettings
{
    public class SPProfile
    {
        private readonly ILogger log = Log4NetLogger.getInstance(LogAppender.APP_SETTINGS);

        /// <summary>
        /// Default constructor. Instantiate a empty profile
        /// </summary>
        public SPProfile()
        {
            this.setAsInactive();
        }

        /// <summary>
        /// This constructors takes the content of a integrity file in csv format, and
        /// instantiate a new profile. If the content of the integrity file is corrupted
        /// or is on the wrong format (name,color,creatinDate) it instantiate an empty
        /// profile
        /// </summary>
        /// <param name="settingsCsv"></param>
        public SPProfile(string settingsCsv)
        {
            this.setProfile(settingsCsv);
        }

        /// <summary>
        /// Load/updates the content of a profile with the content of a integrity 
        /// file. If the content of the integrity file is on the right format
        ///  (name,color,creatinDate) it loads the Profile object. Otherwise, it will
        ///  load an empty object instead
        /// </summary>
        /// <param name="settingsCsv"></param>
        /// <returns></returns>
        public bool setProfile(string settingsCsv)
        {
            log.Debug("SPProfile: " + settingsCsv);
            List<string> items = CSharp.csvToList(settingsCsv);
            if (items.Count == Consts.INTEGRITY_FILE_ITEMS)
            {
                this.name = items[0];
                this.color = items[1];
                this.creationDate = items[2];
                this.isReady = true;
            }
            else
            {
                log.Warn("* CSV CONTENT IS ON THE WRONG FORMAT(name,color,creatinDate): " + settingsCsv);
                log.Info("* LOADING AN EMPTY OBJECT INSTEAD");
                this.setAsInactive();
            }
            return this.isReady;
        }

        /// <summary>
        /// Retuns the contents to be written in an integrity file.
        /// </summary>
        /// <returns></returns>
        public string getSettings()
        {
            return this.name + "," + this.color + "," + this.creationDate;
        }

        public bool isReady { get; set; }

        public string name { get; set; }

        public string color { get; set; }
    
        public string creationDate { get; set; }

        /// <summary>
        /// Set profile with Inactive settings.
        /// </summary>
        private void setAsInactive()
        {
            this.name = Consts.INACTIVE_NAME;
            this.color = Consts.INACTIVE_COLOR;
            this.creationDate = Consts.INACTIVE_CREATION;
            this.isReady = false;
        }

        #region static_methods

        /// <summary>
        /// Receive as parameter the Steam Game Folder where the game is installed or 
        /// should be installed. Returns a profile object is the profile is activated, otherwise
        /// returns a empty list
        /// </summary>
        /// <param name="steamGameFolder"></param>
        /// <returns></returns>
        public static SPProfile loadActivatedProfile(string steamGameFolder)
        {
            ILogger slog = Log4NetLogger.getInstance(LogAppender.APP_SETTINGS);
            slog.Debug("-- loadActiveProfiles() steamGameFolder:" + steamGameFolder);
            SPProfile prof = new SPProfile();
            string pathFile = steamGameFolder + "\\" + Consts.FILE_INTEGRITYFILE;
            slog.Debug("pathFile:" + pathFile);
            if (File.Exists(pathFile))
            {
                string content = File.ReadAllText(pathFile);
                slog.Debug("integrity file content:{" + content + "}");
                prof = new SPProfile(content);
            }
            return prof;
        }

        /// <summary>
        /// Receive as parameter the steam backup folder where the desactivated games are saved, 
        /// and the game folder. Search on all directories of the backup folder for game instalations
        /// with integrity files. Returns a list of desactivated profiles. 
        /// </summary>
        /// <param name="bakcupFolder"></param>
        /// <param name="gameFolder"></param>
        /// <returns></returns>
        public static List<SPProfile> loadDesactivatedProfiles(string backupFolder, string gameFolder)
        {
            ILogger slog = Log4NetLogger.getInstance(LogAppender.APP_SETTINGS);
            List<SPProfile> listDesactivated = new List<SPProfile>();
            string[] allDirs = Directory.GetDirectories(backupFolder);
            slog.Debug("* allDirs:{" + CSharp.arrayToCsv(allDirs) + "}");
            slog.Debug("* allDirs.Length:{" + allDirs.Length + "}");
            if ( (allDirs != null) && (allDirs.Length > 0))
            {
                foreach (var item in allDirs)
                {
                    string itemPath = item + "\\" + gameFolder + "\\" + Consts.FILE_INTEGRITYFILE;
                    slog.Info("-- Loding profile from: {" + itemPath + "}");
                    if (File.Exists(itemPath))
                    {
                        string content = File.ReadAllText(itemPath);
                        SPProfile prof = new SPProfile(content);
                        if (prof.isReady)
                        {
                            listDesactivated.Add(prof);
                        }
                        else
                        {
                            slog.Warn("** ERROR LOADING PROFILE FROM itemPath:" + itemPath);
                        }
                    }
                    else
                    {
                        slog.Warn("** INTEGRITY FILE DOES NOT EXIT itemPath:" + itemPath);
                    }
                }
            }
            return listDesactivated;
        }

        #endregion static_methods
    }
}
