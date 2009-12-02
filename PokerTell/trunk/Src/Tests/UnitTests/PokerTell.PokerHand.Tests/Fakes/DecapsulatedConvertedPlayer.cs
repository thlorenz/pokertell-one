namespace PokerTell.PokerHand.Tests.Fakes
{
    using System;

    using Infrastructure.Enumerations.PokerHand;

    using PokerTell.PokerHand.Analyzation;

    [Serializable]
        internal class DecapsulatedConvertedPlayer : ConvertedPokerPlayer
        {
            #region Properties

          public new string[] SequenceStrings
            {
                get { return base.SequenceStrings; }

                set { base.SequenceStrings = value; }
            }

            public new bool?[] InPosition
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