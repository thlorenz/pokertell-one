namespace PokerTell.PokerHand.ViewModels.Design
{
    using System;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Services;

    internal class DesignHelper
    {
        readonly IConstructor<IConvertedPokerAction> _actionMake;

        readonly IConstructor<IConvertedPokerRound> _roundMake;

        readonly IConstructor<IConvertedPokerPlayer> _playerMake;

        readonly IConstructor<IConvertedPokerHand> _handMake;

        public DesignHelper()
            : this(
                new Constructor<IConvertedPokerAction>(() => new ConvertedPokerAction()),
                new Constructor<IConvertedPokerRound>(() => new ConvertedPokerRound()),
                new Constructor<IConvertedPokerPlayer>(() => new ConvertedPokerPlayer()),
                new Constructor<IConvertedPokerHand>(() => new ConvertedPokerHand()))
        {
        }

        public DesignHelper(
            IConstructor<IConvertedPokerAction> actionMake,
            IConstructor<IConvertedPokerRound> roundMake,
            IConstructor<IConvertedPokerPlayer> playerMake,
            IConstructor<IConvertedPokerHand> handMake)
        {
            _handMake = handMake;
            _playerMake = playerMake;
            _roundMake = roundMake;
            _actionMake = actionMake;   
        }

        internal IConvertedPokerHand CreateSamplePokerHand(int index)
        {
            var player1 = _playerMake.New.InitializeWith("player1", 10 + index, 5, 0, 6, "As Kd");
            player1.AddRound(_roundMake.New);
            player1[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.2));
            player1.AddRound(_roundMake.New);
            player1[Streets.Flop].AddAction(_actionMake.New.InitializeWith(ActionTypes.B, 0.3));
            player1.AddRound(_roundMake.New);
            player1[Streets.Turn].AddAction(_actionMake.New.InitializeWith(ActionTypes.B, 0.5));
            player1[Streets.Turn].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.4));
            player1.AddRound(_roundMake.New);
            player1[Streets.River].AddAction(_actionMake.New.InitializeWith(ActionTypes.B, 0.9));

            var player2 = _playerMake.New.InitializeWith("player2", 12 + index, 4, 1, 6, "9h Qd");
            player2.AddRound(_roundMake.New);
            player2[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.X, 1.0));

            var player3 = _playerMake.New.InitializeWith("player3", 13 + index, 2, 2, 6, "?? ??");
            player3.AddRound(_roundMake.New);
            player3[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.3));
            player3.AddRound(_roundMake.New);
            player3[Streets.Flop].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.2));
            player3.AddRound(_roundMake.New);
            player3[Streets.Turn].AddAction(_actionMake.New.InitializeWith(ActionTypes.R, 3.0));
            player3[Streets.Turn].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.2));
            player3.AddRound(_roundMake.New);
            player3[Streets.River].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.2));

            var player4 = _playerMake.New.InitializeWith("player4", 14 + index, 4, 3, 6, "?? ??");
            player4.AddRound(_roundMake.New);
            player4[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.F, 1.0));

            var player5 = _playerMake.New.InitializeWith("player5", 15 + index, 3, 4, 6, "?? ??");
            player5.AddRound(_roundMake.New);
            player5[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.C, 0.3));

            var player6 = _playerMake.New.InitializeWith("player6", 16 + index, 14, 5, 6, "?? ??");
            player6.AddRound(_roundMake.New);
            player6[Streets.PreFlop].AddAction(_actionMake.New.InitializeWith(ActionTypes.F, 1.0));

            var pokerHand = _handMake.New.InitializeWith("PokerStars", (ulong)(32084482 + index), DateTime.Now, 200, 100, 6);
            pokerHand.AddPlayer(player1);
            pokerHand.AddPlayer(player2);
            pokerHand.AddPlayer(player3);
            pokerHand.AddPlayer(player4);
            pokerHand.AddPlayer(player5);
            pokerHand.AddPlayer(player6);

            pokerHand.Ante = 50;
            pokerHand.TournamentId = 1244353 + (ulong)index;
            pokerHand.Board = "As Kd 9h 3h Qd";

            return pokerHand;
        }
    }
}