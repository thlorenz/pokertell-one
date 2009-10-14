namespace PokerTell.PokerHand.ViewModels.Design
{
    using System;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces;
    using Infrastructure.Interfaces.PokerHand;

    public class HandHistoryViewModel : ViewModels.HandHistoryViewModel
    {
        readonly IConstructor<IConvertedPokerAction> _action;

        readonly IConstructor<IConvertedPokerRound> _round;

        readonly IConstructor<IConvertedPokerPlayer> _player;

        readonly IConstructor<IConvertedPokerHand> _hand;

        public HandHistoryViewModel(
            IConstructor<IConvertedPokerAction> action,
            IConstructor<IConvertedPokerRound> round,
            IConstructor<IConvertedPokerPlayer> player, 
            IConstructor<IConvertedPokerHand> hand)
        {
            _hand = hand;
            _player = player;
            _round = round;
            _action = action;
           
            UpdateWith(CreateSamplePokerHand(12));
        }

        private IConvertedPokerHand CreateSamplePokerHand(int index)
        {
            var player1 = _player.New.InitializeWith("player1", 10 + index, 5, 0, 6, "As Kd");
            player1.AddRound(_round.New);
            player1[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.2));
            player1.AddRound(_round.New);
            player1[Streets.Flop].AddAction(_action.New.InitializeWith(ActionTypes.B, 0.3));
            player1.AddRound(_round.New);
            player1[Streets.Turn].AddAction(_action.New.InitializeWith(ActionTypes.B, 0.5));
            player1[Streets.Turn].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.4));
            player1.AddRound(_round.New);
            player1[Streets.River].AddAction(_action.New.InitializeWith(ActionTypes.B, 0.9));

            var player2 = _player.New.InitializeWith("player2", 12 + index, 4, 1, 6, "9h Qd");
            player2.AddRound(_round.New);
            player2[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.X, 1.0));

            var player3 = _player.New.InitializeWith("player3", 13 + index, 2, 2, 6, "?? ??");
            player3.AddRound(_round.New);
            player3[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.3));
            player3.AddRound(_round.New);
            player3[Streets.Flop].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.2));
            player3.AddRound(_round.New);
            player3[Streets.Turn].AddAction(_action.New.InitializeWith(ActionTypes.R, 3.0));
            player3[Streets.Turn].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.2));
            player3.AddRound(_round.New);
            player3[Streets.River].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.2));

            var player4 = _player.New.InitializeWith("player4", 14 + index, 4, 3, 6, "?? ??");
            player4.AddRound(_round.New);
            player4[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.F, 1.0));

            var player5 = _player.New.InitializeWith("player5", 15 + index, 3, 4, 6, "?? ??");
            player5.AddRound(_round.New);
            player5[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.C, 0.3));

            var player6 = _player.New.InitializeWith("player6", 16 + index, 14, 5, 6, "?? ??");
            player6.AddRound(_round.New);
            player6[Streets.PreFlop].AddAction(_action.New.InitializeWith(ActionTypes.F, 1.0));

            var pokerHand = _hand.New.InitializeWith("PokerStars", (ulong)(32084482 + index), DateTime.Now, 200, 100, 6);
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
