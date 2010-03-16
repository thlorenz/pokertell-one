namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using log4net;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Events;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.PokerHand.Analyzation;

    using Tools.FunctionalCSharp;
    using Tools.WPF;

    public class NewHandCreatorViewModel : IEnumerable<PlayerViewModel>
    {
        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string PathToHandHistoryFile = "some File";
            //@"C:\Documents and Settings\Owner.LapThor\Desktop\hh\history.txt";

        public NewHandCreatorViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;

            LoadDefaultHand();
        }

        void LoadDefaultHand()
        {
            TableName = "PokerTable";

            Player1 = new PlayerViewModel("Greystoke-11", 1, true, "As Kh");
            Player2 = new PlayerViewModel("salemorguy", 2, true, "As Kh");
            Player3 = new PlayerViewModel("Kushum Peng", 3, true, "As Kh");
            Player4 = new PlayerViewModel("roto19", 4, true, "As Kh");
            Player5 = new PlayerViewModel("MegVSPrime", 5, true, "As Kh");
            Player6 = new PlayerViewModel("bigbiskit", 6, true, "As Kh");
        }

        public string TableName { get; set; }

        public PlayerViewModel Player1 { get; set; }

        public PlayerViewModel Player2 { get; set; }

        public PlayerViewModel Player3 { get; set; }

        public PlayerViewModel Player4 { get; set; }

        public PlayerViewModel Player5 { get; set; }

        public PlayerViewModel Player6 { get; set; }

        ICommand _sendCommand;

        readonly IEventAggregator _eventAggregator;

        readonly IUnityContainer _container;

        IGamesTracker _gamesTracker;

        public ICommand SendCommand
        {
            get
            {
                return _sendCommand ?? (_sendCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => CreateHandAndTriggerEvent()
                    });
            }
        }

        void CreateHandAndTriggerEvent()
        {
            var hand = new ConvertedPokerHand(PokerSites.PokerStars, 1, DateTime.Now, 30, 15, 6)
                {
                    TotalSeats = 6,
                    TableName = TableName, 
                    HeroName = Player1.Name, 
                    Board = "Ah Ks Qh", 
                };

            this.ForEach(p => {
                if (p.IsPresent) {
                    var player = new ConvertedPokerPlayer(p.Name, 10, 10 + p.SeatNumber, p.SeatNumber - 1, 6, p.HoleCards)
                        { SeatNumber = p.SeatNumber };
                    player.Add(new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.C, 1.0)));
                    player.Position = p.SeatNumber - 1;
                    player.SetStrategicPosition(6);
                    hand.AddPlayer(player);
                };
            });

            Log.Debug(hand.ToString());

            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Publish(new NewHandEventArgs(PathToHandHistoryFile, hand));
        }

        public void StartTracking()
        {
            _gamesTracker = _container
                .Resolve<IGamesTracker>()
                .InitializeWith(_container.Resolve<ILiveTrackerSettingsViewModel>().LoadSettings())
                .StartTracking(PathToHandHistoryFile);
        }

        public IEnumerator<PlayerViewModel> GetEnumerator()
        {
            yield return Player1;
            yield return Player2;
            yield return Player3;
            yield return Player4;
            yield return Player5;
            yield return Player6;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class PlayerViewModel
    {
        public PlayerViewModel(string name, int seatNumber, bool isPresent, string holeCards)
        {
            Name = name;
            SeatNumber = seatNumber;
            IsPresent = isPresent;
            HoleCards = holeCards;
        }

        public int SeatNumber { get; set; }

        public string Name { get; set; }

        public string HoleCards { get; set; }

        public bool IsPresent { get; set; }
    }
}