using System;
using System.Collections.Generic;
using System.Text;

namespace POL_Dice_Generator
{
    class Dice
    {
        static public string dicestring(ulong min, ulong max)
        {
            if (min < 1 || max < 1)
                return "not valid";

            if (min == max)
            {
                return min.ToString() + "d1";
            }

            if (min > max) /* swap them */
            {
                ulong tmp = min;
                min = max;
                max = tmp;
            }

            ulong x = 0, y = 0, z = 0;
            find_dice(min, max, ref x, ref y, ref z);

            return String.Format("{0}d{1}{2}", x, y, (z == 0) ? "" : "+" + z.ToString()); 
        }
        static public void find_dice(ulong min, ulong max, ref ulong x, ref ulong y, ref ulong z)
        {
            for (z = 0; z < min; ++z)
            {
                x = (min - z);
                if ((max - z) % x == 0)
                {
                    y = (max - z) / x;
                    return;
                }
            }
        }
    }
}
