namespace PokerTell.PokerHand.Tests.Fakes
{
    using System;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;

    [Serializable]
        internal class DecapsulatedConvertedPlayer : ConvertedPokerPlayer
        {
            #region Properties

            public new string[] Sequence
            {
                get { return base.Sequence; }

                set { base.Sequence = value; }
            }

            public new int[] InPosition
            {
                get { return base.InPosition; }

                set { base.InPosition = value; }
            }

            public new int PreflopRaiseInFrontPos
            {
                get { return base.PreflopRaiseInFrontPos; }

                set { base.PreflopRaiseInFrontPos = value; }
            }

            public new StrategicPositions StrategicPosition
            {
                get { return base.StrategicPosition; }

                set { base.StrategicPosition = value; }
            }
            #endregion
        }
    
}