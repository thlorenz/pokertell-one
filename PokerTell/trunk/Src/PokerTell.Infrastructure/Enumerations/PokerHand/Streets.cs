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
                return Streets.PreFlop.To(Streets.River);
            }
        }

        public static IEnumerable<Streets> GetAllPostFlop
        {
            get { return Streets.Flop.To(Streets.River); }
        }

        #endregion

        public static IEnumerable<Streets> To(this Streets from, Streets to)
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
    }
}