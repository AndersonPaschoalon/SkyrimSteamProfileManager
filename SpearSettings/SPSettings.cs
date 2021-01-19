using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpearSettings
{
    /**
  <SETTINGS steamPath=""
            documentsPath=""
            appData="" 
            nmmPath="" 
            vortexPath="" 
            tesveditPath="" 
            dateFormat=""/>
     **/

    public class SPSettings
    {
        //[XmlAttribute("game")]
        //public string game { get; set; }

        [XmlAttribute("steamPath")]
        public string steamPath { get; set; }

        [XmlAttribute("documentsPath")]
        public string documentsPath { get; set; }

        [XmlAttribute("appDataPath")]
        public string appDataPath { get; set; }

        [XmlAttribute("nmmPath")]
        public string nmmPath { get; set; }

        [XmlAttribute("vortexPath")]
        public string vortexPath { get; set; }

        [XmlAttribute("tesvEditPath")]
        public string tesvEditPath { get; set; }

        [XmlAttribute("dateFormat")]
        public string dateFormat { get; set; }

    }
}
