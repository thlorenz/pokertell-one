namespace Tools.Extensions
{
    using System;

    public static class MathExtensions
    {
        /// <summary>
        /// Rounds number using AwayFromZero rounding
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Rounded number</returns>
        public static double Round(this double number)
        {
            return Math.Round(number, MidpointRounding.AwayFromZero);
        }
   
        /// <summary>
        /// Rounds number using AwayFromZero rounding and then converts it to int
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Correctly rounded integer</returns>
        public static int ToInt(this double number)
        {
            return (int)number.Round();
        }
    }
}