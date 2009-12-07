namespace PokerTell.PokerHand
{
    using System.Reflection;

    using Dao;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.Conditions;
    using PokerTell.PokerHand.Services;
    using PokerTell.PokerHand.ViewModels;
    using PokerTell.PokerHand.Views;

    using Tools;
    using Tools.Interfaces;

    using BoardViewModel = PokerTell.PokerHand.ViewModels.BoardViewModel;
    using HandHistoriesViewModel = PokerTell.PokerHand.ViewModels.Design.HandHistoriesViewModel;
    using HandHistoryViewModel = PokerTell.PokerHand.ViewModels.Design.HandHistoryViewModel;

    public class PokerHandModule : IModule
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(
            MethodBase.GetCurrentMethod().DeclaringType);

        readonly IUnityContainer _container;

        readonly IRegionManager _regionManager;

        #endregion

        #region Constructors and Destructors

        public PokerHandModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        #endregion

        #region Implemented Interfaces

        #region IModule

        public void Initialize()
        {
            RegisterViewsAndServices();

            Log.Info("got initialized.");
        }

        #endregion

        #endregion

        #region Methods

        void RegisterViewsAndServices()
        {
            _container

                // Aquired Constructors
                .RegisterConstructor<IAquiredPokerAction, AquiredPokerAction>()
                .RegisterConstructor<IAquiredPokerRound, AquiredPokerRound>()
                .RegisterConstructor<IAquiredPokerPlayer, AquiredPokerPlayer>()
                .RegisterConstructor<IAquiredPokerHand, AquiredPokerHand>()

                // Converted Constructors
                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterConstructor<IConvertedPokerPlayer, ConvertedPokerPlayer>()
                .RegisterConstructor<IConvertedPokerHand, ConvertedPokerHand>()

                // Daos 
                .RegisterType<IPlayerIdentityDao, PlayerIdentityDao>()
                .RegisterType<IConvertedPokerHandDao, ConvertedPokerHandDao>()

                // Conditions
                .RegisterType<IInvestedMoneyCondition, InvestedMoneyCondition>()
                .RegisterType<ISawFlopCondition, SawFlopCondition>()
                .RegisterType<IAlwaysTrueCondition, AlwaysTrueCondition>()

                // Converters
                .RegisterType<IPokerActionConverter, PokerActionConverter>()
                .RegisterType<IPokerRoundsConverter, PokerRoundsConverter>()
                .RegisterTypeAndConstructor<IPokerHandConverter, PokerHandConverter>(() => 
                    _container.Resolve<IPokerHandConverter>())
                .RegisterType<IPokerHandStringConverter, PokerHandStringConverter>()

                // ViewModels
                .RegisterTypeAndConstructor<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterTypeAndConstructor<IHandHistoriesViewModel, HandHistoriesViewModel>()
                .RegisterType<IHoleCardsViewModel, HoleCardsViewModel>()
                .RegisterType<IBoardViewModel, BoardViewModel>()
                .RegisterTypeAndConstructor<IHandHistoryViewModel, HandHistoryViewModel>()

                // Views
                .RegisterType<IHandHistoriesView, HandHistoriesView>()

                // Helpers
                .RegisterType<IItemsPagesManager<IHandHistoryViewModel>, ItemsPagesManager<IHandHistoryViewModel>>()
                .RegisterType<IHandHistoriesFilter, HandHistoriesFilter>();
        }

        #endregion
    }
}