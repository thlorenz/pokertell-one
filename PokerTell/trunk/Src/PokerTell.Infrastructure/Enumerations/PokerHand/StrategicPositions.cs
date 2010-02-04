namespace PokerTell.Infrastructure.Enumerations.PokerHand
{
    using System;
    using System.Collections.Generic;

    using Tools.FunctionalCSharp;

    /// <summary>
    /// Enumeration of all strategic Players Positions
    /// </summary>
    /// <description>
    /// The absolute position of a player is mapped to just
    /// 7 strategic positions depending on the distance from the Button
    /// Here they are given for abstract 9 player Table and a 10 player Table respectively
    /// </description>
    public enum StrategicPositions
    {
        // TableSize		9 Players	10 Players
   
        /// <summary>
        /// Small Blind 	0			0
        /// </summary>
        SB, 

        /// <summary>
        /// BigBlind 		1			1
        /// </summary>
        BB, 

        /// <summary>
        /// Early Position	2-3			2-4
        /// </summary>
        EA, 

        /// <summary>
        /// Middle Position 4-5			5-6
        /// </summary>
        MI, // Middle 		

        /// <summary>
        /// Late Position	6			7
        /// </summary>
        LT, // Late			6			7

        /// <summary>
        /// Cutoff 			7			8
        /// </summary>
        CO, 

        /// <summary>
        /// Button 			8			9
        /// </summary>
        BU
    }

    public static class StrategicPositionsUtility
    {
        public static IEnumerable<StrategicPositions> To(this StrategicPositions from, StrategicPositions to)
        {
            if (from < to)
            {
                while (from <= to)
                {
                    yield return from++;
                }
            }
            else
            {
                while (from >= to)
                {
                    yield return from--;
                }
            }
        }
        
        public static IEnumerable<StrategicPositions> GetAllPositionsInOrder()
        {
            return StrategicPositions.SB.To(StrategicPositions.BU);
            
        }

        public static string NamePosition(StrategicPositions strategicPosition)
        {
            return
                strategicPosition.Match()
                .With(p => p ==StrategicPositions.SB, _ => "the small blind")
                .With(p => p ==StrategicPositions.BB, _ => "the big blind")
                .With(p => p ==StrategicPositions.EA, _ => "early position")
                .With(p => p ==StrategicPositions.MI, _ => "middle position")
                .With(p => p ==StrategicPositions.LT, _ => "late position")
                .With(p => p ==StrategicPositions.CO, _ => "the cut-off")
                .With(p => p ==StrategicPositions.BU, _ => "the button")
                .Do();

        }
    }
}