namespace Tools
{
    /// <summary>
    /// Description of Normailizer.
    /// </summary>
    public class Normalizer
    {
        /// <summary>
        /// Assumes more then 1 keyvalue and that they are ordered
        /// </summary>
        /// <param name="keyValues"></param>
        /// <param name="value"></param>
        /// <returns>A Value normalize to one of the Key values</returns>
        public static double NormalizeToKeyValues(double[] keyValues, double value)
        {
            // Try to find closest keyvalue to actual value
            for (int i = 1; i < keyValues.Length; i++)
            {
                double divide = keyValues[i - 1] + ((keyValues[i] - keyValues[i - 1]) / 2.0);
                if (value < divide)
                {
                    return keyValues[i - 1];
                }
            }

            // Value is closest to biggest keyvalue
            return keyValues[keyValues.Length - 1];
        }
    }
}