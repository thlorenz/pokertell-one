namespace PokerTell.Infrastructure.Enumerations.PokerHand
{
    using System.Collections.Generic;

    /// <summary>
    /// Streets enumeration
    /// </summary>
    public enum Streets
    {
        /// <summary>
        /// Preflop
        /// </summary>
        PreFlop, 

        /// <summary>
        /// Flop
        /// </summary>
        Flop, 

        /// <summary>
        /// Turn
        /// </summary>
        Turn, 

        /// <summary>
        /// River
        /// </summary>
        River
    }

    public static class StreetsUtility
    {
        #region Properties

        public static IEnumerable<Streets> GetAll
        {
            get
            {
                yield return Streets.PreFlop;

                foreach (Streets item in GetAllPostFlop)
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<Streets> GetAllPostFlop
        {
            get { return new[] { Streets.Flop, Streets.Turn, Streets.River }; }
        }

        #endregion
    }
}