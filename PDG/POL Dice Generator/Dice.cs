/*

Reason why this works:

    Suppose a dice-string: XdY + Z

    max = if all dice come up Y = x*y + z              (1)
    min = if all dice come up 1 = x*1 + z = x + z      (2)

    Solving for x in (2), we get:
 
        x = min - z               (3)

    Solving for y in (1):

        xy + z = max
            xy = max - z
             y = (max - z) / x    (4)

    Subst. (3) in (4):

      y = (max-z)/(min-z)         (5)


    With (3) and (5), we have two equations for
 three variables. But, we need y to be integer (otherwise,
 there'd be no dice-string)

    Then, we vary z from 0 to a-1 and see which is the first z
 to allow y to be an integer. Having this z, we're done. Just
 need to substitute everything.

	Z = the one from the iterations
	X = min - Z
        Y = (max-z)/X

	And we have the dice-string: XdY + Z  :) 

--
Fernando Rozenblit (rozenblit@gmail.com) - 2009-03-10	
*/

/* For uniform distribution, just rescale it:
 *  1dY + Z:
 *   
 *  Min: 1+Z -> z = min - 1
 *  Max: Y+Z -> y = max - z = max - min + 1  
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace POL_Dice_Generator
{
    class Dice
    {
        static public string dicestring(ulong min, ulong max)
        {
            return dicestring(min, max, false);
        }
        static public string dicestring(ulong min, ulong max, bool uniform)
        {
            if (min < 1 || max < 1)
                return "not valid";

            if (min == max)
            {
                return (uniform) ? "1d1+" + min : min.ToString() + "d1";
            }

            if (min > max) /* swap them */
            {
                ulong tmp = min;
                min = max;
                max = tmp;
            }

            ulong x = 0, y = 0, z = 0;

            if (uniform)
            {
                x = 1;
                find_uniform_dice(min, max, ref y, ref z);
            }
            else
                find_largest_dice(min, max, ref x, ref y, ref z);

            return String.Format("{0}d{1}{2}", x, y, (z == 0) ? "" : "+" + z.ToString()); 
        }

        static public void find_uniform_dice(ulong min, ulong max, ref ulong y, ref ulong z)
        {
            z = min - 1;
            y = (max - min + 1);
            return;
        }

        // Finds the largest number of dice that will produce the result.
        static public void find_largest_dice(ulong min, ulong max, ref ulong x, ref ulong y, ref ulong z)
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
