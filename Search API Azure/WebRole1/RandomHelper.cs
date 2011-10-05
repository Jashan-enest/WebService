using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace WebRole1
{
    public class RandomHelper
    {
        private static Random randomSeed = new Random();

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        public static string RandomString(int size, bool lowerCase)
        {
            // StringBuilder is faster than using strings (+=)
            StringBuilder RandStr = new StringBuilder(size);

            // Ascii start position (65 = A / 97 = a)
            int Start = (lowerCase) ? 97 : 65;

            // Add random chars
            for (int i = 0; i < size; i++)
                RandStr.Append((char)(26 * randomSeed.NextDouble() + Start));

            return RandStr.ToString();
        }

        /// <summary>
        /// Returns a random number.
        /// </summary>
        /// <param name="min">Minimal result</param>
        /// <param name="max">Maximal result</param>
        /// <returns>Random number</returns>
        public static int RandomNumber(int Minimal, int Maximal)
        {
            return randomSeed.Next(Minimal, Maximal);
        }

        /// <summary>
        /// Returns a random boolean value
        /// </summary>
        /// <returns>Random boolean value</returns>
        public static bool RandomBool()
        {
            return (randomSeed.NextDouble() > 0.5);
        }

        /// <summary>
        /// Returns a random color
        /// </summary>
        /// <returns></returns>
        public static System.Drawing.Color RandomColor()
        {
            return System.Drawing.Color.FromArgb(
                (randomSeed.Next(0, 32) * 8) % 256,
                (randomSeed.Next(0, 32) * 8) % 256,
                (randomSeed.Next(0, 32) * 8) % 256
            );
        }
    }
}