namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;
    using Infrastructure.Interfaces.LiveTracker;
    using Infrastructure.Interfaces.Repository;

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

        ulong _gameId = 0;

        public NewHandCreatorViewModel(IUnityContainer container, IEventAggregator eventAggregator, IRepository repository)
        {
            _repository = repository;
            _container = container;
            _eventAggregator = eventAggregator;

            LoadDefaultHand();
        }

        void LoadDefaultHand()
        {
            TableName = "PokerTable";
            TotalSeats = 9;

            Player1 = new PlayerViewModel("Greystoke-11", 1, true, "As Kh");
            Player2 = new PlayerViewModel("salemorguy", 2, true, "As Kh");
            Player3 = new PlayerViewModel("Kushum Peng", 3, true, "As Kh");
            Player4 = new PlayerViewModel("bigbiskit", 4, true, "As Kh");
            Player5 = new PlayerViewModel("MegVSPrime", 5, true, "As Kh");
            Player6 = new PlayerViewModel("PokerHnd", 6, true, "As Kh");
            Player7 = new PlayerViewModel("primzahl", 7, false, "As Kh");
            Player8 = new PlayerViewModel("Przemo High", 8, false, "As Kh");
            Player9 = new PlayerViewModel("Tx Maniac", 9, false, "As Kh");
            Player10 = new PlayerViewModel("PokerHnd", 10, false, "As Kh");
        }

        public string TableName { get; set; }

        public int TotalSeats { get; set; }

        public PlayerViewModel Player1 { get; set; }

        public PlayerViewModel Player2 { get; set; }

        public PlayerViewModel Player3 { get; set; }

        public PlayerViewModel Player4 { get; set; }

        public PlayerViewModel Player5 { get; set; }

        public PlayerViewModel Player6 { get; set; }

        public PlayerViewModel Player7 { get; set; }

        public PlayerViewModel Player8 { get; set; }

        public PlayerViewModel Player9 { get; set; }

        public PlayerViewModel Player10 { get; set; }

        ICommand _sendCommand;

        readonly IEventAggregator _eventAggregator;

        readonly IUnityContainer _container;

        readonly IRepository _repository;

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
            var hand = new ConvertedPokerHand(PokerSites.PokerStars, _gameId++, DateTime.Now, 30, 15, TotalSeats)
                {
                    TotalSeats = TotalSeats,
                    TableName = TableName, 
                    HeroName = Player1.Name, 
                    Board = "Ah Ks Qh", 
                };

            this.ForEach(p => {
                if (p.IsPresent && p.SeatNumber <= TotalSeats) {
                    var player = new ConvertedPokerPlayer(p.Name, 10, 10 + p.SeatNumber, p.SeatNumber - 1, TotalSeats, p.HoleCards)
                        { SeatNumber = p.SeatNumber };
                    player.Add(new ConvertedPokerRound().Add(new ConvertedPokerAction(ActionTypes.C, 1.0)));
                    player.Position = p.SeatNumber - 1;
                    player.SetStrategicPosition(TotalSeats);
                    hand.AddPlayer(player);
                };
            });

            Log.Debug(hand.ToString());

            _repository.InsertHand(hand);

            Log.Debug("Inserted into database");

            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Publish(new NewHandEventArgs(PathToHandHistoryFile, hand));
        }

        public void StartTracking()
        {
           _container
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
            yield return Player7;
            yield return Player8;
            yield return Player9;
            yield return Player10;
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