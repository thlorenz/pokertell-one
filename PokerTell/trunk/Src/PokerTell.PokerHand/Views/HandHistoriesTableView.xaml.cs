namespace PokerTell.PokerHand.Views
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    using Analyzation;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.Interfaces;

    using ViewModels;

    /// <summary>
    /// Interaction logic for HandHistoriesTableView.xaml
    /// </summary>
    public partial class HandHistoriesTableView
    {
        #region Constructors and Destructors

        public HandHistoriesTableView()
            : this(CreateSampleHandHistoriesViewModel())
        {
        }

        public HandHistoriesTableView(HandHistoriesTableViewModel viewModel)
        {
            viewModel.SelectedIndex = viewModel.LastHandIndex;
            DataContext = viewModel;
           
            InitializeComponent();
        }

        #endregion

        #region Properties

        public INotifyPropertyChanged ViewModel
        {
            get { return (INotifyPropertyChanged)DataContext; }
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
            player1.Add(new ConvertedPokerRound());
            player1[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player1.Add(new ConvertedPokerRound());
            player1[Streets.Flop].Add(new ConvertedPokerAction(ActionTypes.B, 0.3));
            player1.Add(new ConvertedPokerRound());
            player1[Streets.Turn].Add(new ConvertedPokerAction(ActionTypes.B, 0.5));
            player1[Streets.Turn].Add(new ConvertedPokerAction(ActionTypes.C, 0.4));
            player1.Add(new ConvertedPokerRound());
            player1[Streets.River].Add(new ConvertedPokerAction(ActionTypes.B, 0.9));

            var player2 = new ConvertedPokerPlayer("player2", 12 + index, 4, 1, 6, "9h Qd");
            player2.Add(new ConvertedPokerRound());
            player2[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.X, 1.0));

            var player3 = new ConvertedPokerPlayer("player3", 13 + index, 2, 2, 6, "?? ??");
            player3.Add(new ConvertedPokerRound());
            player3[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));
            player3.Add(new ConvertedPokerRound());
            player3[Streets.Flop].Add(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player3.Add(new ConvertedPokerRound());
            player3[Streets.Turn].Add(new ConvertedPokerAction(ActionTypes.R, 3.0));
            player3[Streets.Turn].Add(new ConvertedPokerAction(ActionTypes.C, 0.2));
            player3.Add(new ConvertedPokerRound());
            player3[Streets.River].Add(new ConvertedPokerAction(ActionTypes.C, 0.2));

            var player4 = new ConvertedPokerPlayer("player4", 14 + index, 4, 3, 6, "?? ??");
            player4.Add(new ConvertedPokerRound());
            player4[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.F, 1.0));

            var player5 = new ConvertedPokerPlayer("player5", 15 + index, 3, 4, 6, "?? ??");
            player5.Add(new ConvertedPokerRound());
            player5[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.C, 0.3));

            var player6 = new ConvertedPokerPlayer("player6", 16 + index, 14, 5, 6, "?? ??");
            player6.Add(new ConvertedPokerRound());
            player6[Streets.PreFlop].Add(new ConvertedPokerAction(ActionTypes.F, 1.0));

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