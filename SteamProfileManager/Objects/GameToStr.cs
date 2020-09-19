using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProfileManager.Enum;

namespace ProfileManager.Objects
{
    class GameToStr
    {
        public static string str(Game gameEnum)
        {
            switch (gameEnum)
            {
                case Game.SKYRIM:
                    {
                        return Consts.SKYRIM;
                    }
                case Game.SKYRIM_SE:
                    {
                        return Consts.SKYRIM_SE;
                    }
                case Game.MORROWIND:
                    {
                        return Consts.MORROWIND;
                    }
                case Game.AGE_OF_MITOLOGY:
                    {
                        return Consts.AGE_OF_MITOLOGY;
                    }
                default:
                    {
                        return Consts.SKYRIM;
                    }
            }
        }
    }
}
