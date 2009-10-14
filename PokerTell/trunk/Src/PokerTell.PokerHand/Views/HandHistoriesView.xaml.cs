namespace PokerTell.PokerHand.Views
{
    using System;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    using ViewModels;

    /// <summary>
    /// Interaction logic for HandHistoriesView.xaml
    /// </summary>
    public partial class HandHistoriesView
    {
        #region Constructors and Destructors

        public HandHistoriesView()
            : this(CreateSampleHandHistoriesViewModel())
        {
        }

        public HandHistoriesView(HandHistoriesTableViewModel viewModel)
        {
            viewModel.SelectedIndex = viewModel.LastHandIndex;
            DataContext = viewModel;
           
            InitializeComponent();
        }

        #endregion

        #region Properties

        public IViewModel ViewModel
        {
            get { return (IViewModel)DataContext; }
        }

        #endregion

        #region Methods

        private static HandHistoriesTableViewModel CreateSampleHandHistoriesViewModel()
        {
            var sampleModel = new HandHistoriesTableViewModel();
            for (int i = 0; i < 100; i++)
            {
                sampleModel.AddHand(CreateSamplePokerHand(i));
            }

            return sampleModel;
        }

        private static IConvertedPokerHand CreateSamplePokerHand(int index)
        {
            var player1 = new ConvertedPokerPlayer("player1", 10 + index, 5, 0, 6, "As Kd");
            player1.AddRound(new ConvertedPokerRound());
            player1[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player1.AddRound(new ConvertedPokerRound());
            player1[Streets.Flop].AddAction(new ConvertedPokerAction(ActionTypes.B, 0.3));
            player1.AddRound(new ConvertedPokerRound());
            player1[Streets.Turn].AddAction(new ConvertedPokerAction(ActionTypes.B, 0.5));
            player1[Streets.Turn].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.4));
            player1.AddRound(new ConvertedPokerRound());
            player1[Streets.River].AddAction(new ConvertedPokerAction(ActionTypes.B, 0.9));

            var player2 = new ConvertedPokerPlayer("player2", 12 + index, 4, 1, 6, "9h Qd");
            player2.AddRound(new ConvertedPokerRound());
            player2[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.X, 1.0));

            var player3 = new ConvertedPokerPlayer("player3", 13 + index, 2, 2, 6, "?? ??");
            player3.AddRound(new ConvertedPokerRound());
            player3[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.3));
            player3.AddRound(new ConvertedPokerRound());
            player3[Streets.Flop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player3.AddRound(new ConvertedPokerRound());
            player3[Streets.Turn].AddAction(new ConvertedPokerAction(ActionTypes.R, 3.0));
            player3[Streets.Turn].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player3.AddRound(new ConvertedPokerRound());
            player3[Streets.River].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.2));

            var player4 = new ConvertedPokerPlayer("player4", 14 + index, 4, 3, 6, "?? ??");
            player4.AddRound(new ConvertedPokerRound());
            player4[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.F, 1.0));

            var player5 = new ConvertedPokerPlayer("player5", 15 + index, 3, 4, 6, "?? ??");
            player5.AddRound(new ConvertedPokerRound());
            player5[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player6 = new ConvertedPokerPlayer("player6", 16 + index, 14, 5, 6, "?? ??");
            player6.AddRound(new ConvertedPokerRound());
            player6[Streets.PreFlop].AddAction(new ConvertedPokerAction(ActionTypes.F, 1.0));

            var pokerHand = new ConvertedPokerHand("PokerStars", (ulong)(32084482 + index), DateTime.Now, 200, 100, 6);
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

        #endregion

        private void ScrollBar_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var scrollBar = (ScrollBar)sender;
            var newValue = scrollBar.Value - ((double) e.Delta / 120);
           
            if (newValue < scrollBar.Minimum)
            {
                newValue = scrollBar.Minimum;
            }

            if (newValue > scrollBar.Maximum)
            {
                newValue = scrollBar.Maximum;
            }

            scrollBar.Value = newValue;
        }
    }
}