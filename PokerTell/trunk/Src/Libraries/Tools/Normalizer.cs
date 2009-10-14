//Date: 3/13/2009



using System;

namespace Tools
{
	/// <summary>
	/// Description of Normailizer.
	/// </summary>
	public class Normalizer
	{
		/// <summary>
		/// 
		/// Assumes more then 1 keyvalue and that they are ordered
		/// </summary>
		/// <param name="KeyValues"></param>
		/// <param name="Value"></param>
		/// <returns>A Value normalize to one of the Key values</returns>
		public static double NormalizeToKeyValues(double[] KeyValues, double Value)
		{
			double Divide;
			
			//Try to find closest keyvalue to actual value
			for(int i = 1; i < (KeyValues.Length);i++)
			{
				Divide =  KeyValues[i -1] + (( KeyValues[i] -  KeyValues[i -1])/2.0);
				if(Value < Divide)
				{
					return KeyValues[i -1];
				}
			}
			//Value is closest to biggest keyvalue
			return KeyValues[KeyValues.Length -1];
		}
	}
}
