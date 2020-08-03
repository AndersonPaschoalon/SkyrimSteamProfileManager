using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media;
using SteamProfileManager.Objects;

namespace SteamProfileManager
{
    public class ProfileData
    {
        private string _profileName;
        private Color _color;

        public ProfileData(string name, string color)
        { 
        }

        public ProfileData(string name, Color color)
        {
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
                col = (Color)ColorConverter.ConvertFromString(hexCode);
            }
            catch (Exception ex)
            {
                col = (Color)ColorConverter.ConvertFromString("#66FFF5");
            }
            this._color = col;
        }


    }
}
