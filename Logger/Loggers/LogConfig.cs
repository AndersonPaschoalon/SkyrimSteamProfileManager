using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utils.Loggers
{
    [XmlRoot("LOG", IsNullable = false)]
    class LogConfig
    {
        public LogConfig()
        { }

        [XmlAttribute("loglevel")]
        public string loglevel { get; set; }
    }
}
