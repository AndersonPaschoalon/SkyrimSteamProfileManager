﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using ProfileManager.Objects;

namespace ProfileManager
{
    public class ProfileData
    {
        private string _profileName;
        private Color _color;

        //public ProfileData(string name, string color)
        //{ 
        //}

        public ProfileData(string name, Color color)
        {
            this.color = color;
            this.profileName = name;
        }

        public string profileName
        {
            get
            {
                return this._profileName;
            }
            set
            {
                this._profileName = Utils.alphaNumeric(value);
            }
        }

        public Color color
        {
            get 
            {
                return this._color;
            } 
            set
            {
                this._color = value;
            }
        }

        public string colorhex()
        {
            Color c = this._color;
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public void setColor(string hexCode)
        {
            Color col;
            try
            {
                // TODO
                //col = (Color)ColorConverter.ConvertFromString(hexCode);
            }
            catch (Exception ex)
            {
                // TODO
                //col = (Color)ColorConverter.ConvertFromString("#66FFF5");
            }
            // TODO
            //this._color = col;
        }


    }
}
