using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamSlotUsagesTool
{
    internal class Usage
    {
        int doubles;
        int standard;
        int combined;

        public bool HasExceededUsage()
        {
            if (doubles > 7 || standard > 7)
                return true;
            if (combined > 12)
                return true;
            return false;
        }

        public void Use(GameMode gameMode)
        {
            combined++;
            if (gameMode == GameMode.DOUBLES)
                doubles++;
            else
                standard++;
        }

        public int GetDoublesUsage() => doubles;
        public int GetStandardUsage() => standard;
        public int GetCombinedUsage() => combined;

        public override string ToString()
        {
            return "Doubles " + doubles + " Standard: " + standard + " Combined: " + combined;
        }
    }
}
