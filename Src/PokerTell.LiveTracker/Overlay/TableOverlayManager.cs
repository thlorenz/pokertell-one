namespace PokerTell.LiveTracker.Overlay
{
    using System;
    using System.Linq;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.LiveTracker.Interfaces;
    using PokerTell.LiveTracker.PokerRooms;

    using Tools.Interfaces;
    using Tools.WPF.Interfaces;

    public class TableOverlayManager : ITableOverlayManager
    {
        readonly ITableOverlayViewModel _tableOverlay;

        readonly IOverlayToTableAttacher _overlayToTableAttacher;

        readonly ISeatMapper _seatMapper;

        readonly ILayoutManager _layoutManager;

        readonly IPokerRoomInfoLocator _pokerRoomInfoLocator;

        IWindowManager _tableOverlayWindow;

        public TableOverlayManager(
            IPokerRoomInfoLocator pokerRoomInfoLocator, 
            ILayoutManager layoutManager, 
            ISeatMapper seatMapper, 
            IOverlayToTableAttacher overlayToTableAttacher, 
            ITableOverlayViewModel tableOverlay)
        {
            _pokerRoomInfoLocator = pokerRoomInfoLocator;
            _layoutManager = layoutManager;
            _seatMapper = seatMapper;
            _overlayToTableAttacher = overlayToTableAttacher;
            _tableOverlay = tableOverlay;
        }

        public string HeroName { get; protected set; }

        public event Action TableClosed = delegate { };

        public ITableOverlayManager InitializeWith(
            IWindowManager tableOverlayWindow, 
            IGameHistoryViewModel gameHistory, 
            IPokerTableStatisticsViewModel pokerTableStatistics, 
            int showHoleCardsDuration, 
            IConvertedPokerHand firstHand)
        {
            _tableOverlayWindow = tableOverlayWindow;

            HeroName = firstHand.HeroName;

            _seatMapper.InitializeWith(firstHand.TotalSeats);

            ITableOverlaySettingsViewModel overlaySettings = SetupOverlaySettings(firstHand);

            _tableOverlay.InitializeWith(_seatMapper, overlaySettings, gameHistory, pokerTableStatistics, showHoleCardsDuration);

            _tableOverlayWindow.DataContext = _tableOverlay;
            _tableOverlayWindow.Show();

            var watchTableTimer = new DispatcherTimerAdapter { Interval = TimeSpan.FromMilliseconds(1000) };
            var waitThenTryToFindTableAgainTimer = new DispatcherTimerAdapter { Interval = TimeSpan.FromMilliseconds(3000) };
            _overlayToTableAttacher.InitializeWith(_tableOverlayWindow, 
                                                   watchTableTimer, 
                                                   waitThenTryToFindTableAgainTimer, 
                                                   _pokerRoomInfoLocator.GetPokerRoomInfoFor(firstHand.Site), 
                                                   firstHand.TableName);
            
            UpdateTableOverlay(firstHand);
            UpdateSeatMapper(firstHand);
            UpdateOverlayToTableAttacher(firstHand);

            RegisterEvents();
            return this;
        }

        public ITableOverlayManager UpdateWith(IConvertedPokerHand newHand)
        {
            UpdateTableOverlay(newHand);
            UpdateSeatMapper(newHand);
            UpdateOverlayToTableAttacher(newHand);
            return this;
        }

        ITableOverlaySettingsViewModel SetupOverlaySettings(IConvertedPokerHand convertedPokerHand)
        {
            var overlaySettings = _layoutManager.Load(convertedPokerHand.Site, convertedPokerHand.TotalSeats);
            overlaySettings.SaveChanges += () => _layoutManager.Save(overlaySettings, convertedPokerHand.Site);
            overlaySettings.UndoChanges += revertTo => revertTo(_layoutManager.Load(convertedPokerHand.Site, convertedPokerHand.TotalSeats));
            return overlaySettings;
        }

        void UpdateSeatMapper(IConvertedPokerHand convertedPokerHand)
        {
            var actualSeatOfHero = convertedPokerHand.Players.First(p => p.Name == HeroName).SeatNumber;
            _seatMapper.UpdateWith(actualSeatOfHero);
        }

        void UpdateOverlayToTableAttacher(IConvertedPokerHand convertedPokerHand)
        {
            _overlayToTableAttacher.TableName = convertedPokerHand.TableName;
        }

        void UpdateTableOverlay(IConvertedPokerHand convertedPokerHand)
        {
            _tableOverlay.UpdateWith(convertedPokerHand.Players, convertedPokerHand.Board);
        }

        void RegisterEvents()
        {
            _overlayToTableAttacher.TableClosed += () => TableClosed();
        }

        public void Dispose()
        {
            _tableOverlayWindow.Dispose();
            _overlayToTableAttacher.Dispose();
        }
    }
}