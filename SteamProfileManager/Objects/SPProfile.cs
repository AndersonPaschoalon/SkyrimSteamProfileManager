﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProfileManager.Objects
{
    public class SPProfile
    {
        public SPProfile()
        {
            this.name = "";
            this.isActive = "TRUE";
        }

        //  name={alphanumeric string}
        [XmlAttribute("name")]
        public string name;

        // isActive={true/false}
        [XmlAttribute("isActive")]
        public string isActive
        {
            get 
            {
                return Utils.boolToStr(this._isActive);
            }
            set
            {
                this._isActive = Utils.strToBool(value);
            }
        }

        [XmlAttribute("color")]
        public string color;

        public bool isProfileActive()
        {
            return this._isActive;
        }
     
        private bool _isActive = false;

    }
}