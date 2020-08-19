using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfileManager.Enum;

namespace ProfileManagerBL.ViewModel
{
    public class ViewGame
    {
        public const string SKYRIM = "Skyrim";
        public const string SKYRIM_SE = "Skyrim SE";
        public const string MORROWIND = "Morrowind";
        public const string AGE_OF_MITOLOGY = "Age of Mitology";

        public static Game enumGame(string gameString)
        {
            Game theGame = Game.SKYRIM;
            switch (gameString)
            {
                case SKYRIM:
                    {
                        theGame = Game.SKYRIM;
                        break;
                    }
                case SKYRIM_SE:
                    {
                        theGame = Game.SKYRIM_SE;
                        break;
                    }
                case MORROWIND:
                    {
                        theGame = Game.MORROWIND;
                        break;
                    }
                case AGE_OF_MITOLOGY:
                    {
                        theGame = Game.AGE_OF_MITOLOGY;
                        break;
                    }
                default:
                    {
                        theGame = Game.SKYRIM;
                        break;
                    }
            }
            return theGame;
        }
    }
}
