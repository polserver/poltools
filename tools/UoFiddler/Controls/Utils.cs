/***************************************************************************
 *
 * $Author: Turley
 * 
 * "THE BEER-WARE LICENSE"
 * As long as you retain this notice you can do whatever you want with 
 * this stuff. If we meet some day, and you think this stuff is worth it,
 * you can buy me a beer in return.
 *
 ***************************************************************************/

namespace FiddlerControls
{
    public static class Utils
    {
        /// <summary>
        /// Converts string to int with Hex recognition
        /// </summary>
        /// <param name="text">string to parse</param>
        /// <param name="result">out result</param>
        /// <param name="min">minvalue</param>
        /// <param name="max">maxvalue</param>
        /// <returns>bool could convert and between min/max</returns>
        public static bool ConvertStringToInt(string text, out int result, int min, int max)
        {
            bool candone;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                candone = int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);

            if ((result > max) || (result < min))
                candone = false;

            return candone;
        }

        /// <summary>
        /// Converts string to int with Hex recognition
        /// </summary>
        /// <param name="text">string to parse</param>
        /// <param name="result">out result</param>
        /// <returns>bool could convert</returns>
        public static bool ConvertStringToInt(string text, out int result)
        {
            bool candone;
            if (text.Contains("0x"))
            {
                string convert = text.Replace("0x", "");
                candone = int.TryParse(convert, System.Globalization.NumberStyles.HexNumber, null, out result);
            }
            else
                candone = int.TryParse(text, System.Globalization.NumberStyles.Integer, null, out result);
            return candone;
        }
    }
}
