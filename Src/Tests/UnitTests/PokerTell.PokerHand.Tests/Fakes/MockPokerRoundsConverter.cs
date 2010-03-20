namespace PokerTell.PokerHand.Tests.Fakes
{
    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    using Interfaces;

    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Services;

    using Services;

    internal class MockPokerRoundsConverter : PokerRoundsConverter
    {
        #region Constructors and Destructors

        public MockPokerRoundsConverter(
            IConstructor<IConvertedPokerActionWithId> convertedActionWithId, 
            IConstructor<IConvertedPokerRound> convertedRound, 
            IPokerActionConverter actionConverter)
            : base(convertedActionWithId, convertedRound, actionConverter)
        {
            ActionCountProp = 0;
            FoundActionProp = false;
            PotProp = 0;
            SequenceForCurrentRoundProp = new ConvertedPokerRound();
            SequenceSoFarProp = string.Empty;
            ToCallProp = 0;
        }

        #endregion

        #region Properties

        public int ActionCountProp
        {
            get { return ActionCount; }
            set { ActionCount = value; }
        }

        public bool FoundActionProp
        {
            get { return FoundAction; }
            set { FoundAction = value; }
        }

        public double PotProp
        {
            get { return Pot; }
            set { Pot = value; }
        }

        public IConvertedPokerRound SequenceForCurrentRoundProp
        {
            get { return SequenceForCurrentRound; }
            set { SequenceForCurrentRound = value; }
        }

        public string SequenceSoFarProp
        {
            get { return SequenceSoFar; }
            set { SequenceSoFar = value; }
        }

        public double ToCallProp
        {
            get { return ToCall; }
            set { ToCall = value; }
        }

        #endregion

        #region Public Methods

        public void InvokeProcessRound(Streets street, IAquiredPokerRound aquiredPokerRound, IConvertedPokerPlayer convertedPlayer)
        {
            ProcessRound(street, aquiredPokerRound, convertedPlayer);
        }

        #endregion
    }
}