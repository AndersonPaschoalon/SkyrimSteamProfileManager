using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SteamProfileManager.Objects
{
    public class SPSettings
    {

        [XmlAttribute("steamPath")]
        public string steamPath { get; set; }


        [XmlAttribute("documentsPath")]
        public string documentsPath { get; set; }


        [XmlAttribute("appDataPath")]
        public string appDataPath { get; set; }


        [XmlAttribute("nmmModPath")]
        public string nmmModPath { get; set; }


        [XmlAttribute("nmmInfoPath")]
        public string nmmInfoPath { get; set; }

        [XmlAttribute("gameFolder")]
        public string gameFolder { get; set; }

        public string steamPathGame()
        {
            return steamPath + "\\" + this.gameFolder;
        }

        public string documentsPathGame()
        {
            return documentsPath + "\\" + this.gameFolder;
        }

        public string appDataPathGame()
        {
            return appDataPath + "\\" + this.gameFolder;
        }

        public string nmmModPathGame()
        {
            if (this.nmmInfoPath == null || this.nmmInfoPath.Trim().Equals(""))
            {
                return "";
            }
            else
            {
                return this.nmmInfoPath + "\\" + this.gameFolder;
            }
        }

        public string nmmInfoPathGame()
        {
            if (this.nmmInfoPath == null || this.nmmInfoPath.Trim().Equals("") )
            {
                return "";
            }
            else
            {
                return this.nmmInfoPath + "\\" + this.gameFolder;
            }
        }

    }
}
