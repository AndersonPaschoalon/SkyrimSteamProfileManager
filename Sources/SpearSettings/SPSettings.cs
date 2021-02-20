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

        [XmlAttribute("steamPath")]
        public string steamPath { get; set; }

        [XmlAttribute("documentsPath")]
        public string documentsPath { get; set; }

        [XmlAttribute("appDataPath")]
        public string appDataPath { get; set; }

        [XmlAttribute("nmmPath")]
        public string nmmPath2 { get; set; }

        [XmlAttribute("vortexPath")]
        public string vortexPath2 { get; set; }

        [XmlAttribute("nmmExe")]
        public string nmmExe { get; set; }

        [XmlAttribute("vortexExe")]
        public string vortexExe { get; set; }

        [XmlAttribute("tesvEditExe")]
        public string tesvEditExe { get; set; }

        [XmlAttribute("dateFormat")]
        public string dateFormat { get; set; }

    }
}
