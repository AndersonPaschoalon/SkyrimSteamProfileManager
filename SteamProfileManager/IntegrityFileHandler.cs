using SpearSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace ProfileManager
{
    public class IntegrityFileHandler
    {

        private PathsHelper pathsHelper;

        public IntegrityFileHandler(PathsHelper paths)
        {
            this.pathsHelper = paths;
        }

        public string activeIntegrityFilePath()
        {
            return this.pathsHelper.activeIntegrityFilePath;
        }

        public bool activeIntegrityFileExists()
        {
            string intFile = this.pathsHelper.activeIntegrityFilePath;
            if (File.Exists(intFile))
            {
                return true;
            }
            return false;
        }

        public string activeIntegrityFileContent()
        {
            if (this.activeIntegrityFileExists())
            {
                string intFile = this.pathsHelper.activeIntegrityFilePath;
                string integrityFileContent = File.ReadAllText(intFile);
                return integrityFileContent;
            }
            return "";
        }

        public List<string> activeIntegrityFileItems()
        {
            string content = this.activeIntegrityFileContent();
            return CSharp.csvToList(content);
        }

        public bool deleteActiveIntegrityFile(out string errMsg)
        {
            try
            {
                File.Delete(this.pathsHelper.activeIntegrityFilePath);
                errMsg = "";
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return false;
            }
            return true;
        }

        public string desactivatedIntegrityFilePath(string prof)
        {
            return this.pathsHelper.desactivatedIntegrityFilePath(prof);
        }

        public bool updateActiveIntegrityFile(SPProfile prof, out string errMsg, out string errPath)
        {
            string content = prof.name + "," + prof.color + "," + prof.creationDate;
            string filePath = this.pathsHelper.activeIntegrityFilePath;
            try
            {
                File.WriteAllText(filePath, content);
                errMsg = "SUCCESS";
                errPath = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errPath = filePath;
                return false;
            }
        }

        public bool updateDesactivatedIntegrityFile(SPProfile prof, out string errMsg, out string errPath)
        {
            string content = prof.name + "," + prof.color + "," + prof.creationDate;
            string filePath = this.desactivatedIntegrityFilePath(prof.name);
            try
            {
                File.WriteAllText(filePath, content);
                errMsg = "SUCCESS";
                errPath = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                errPath = filePath;
                return false;
            }
        }

    }
}
