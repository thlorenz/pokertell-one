namespace PokerTell.PokerHand
{
    using System.Reflection;

    using Conditions;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure;
    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.ViewModels;
    using PokerTell.PokerHand.Views;

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
                .RegisterConstructor<IAquiredPokerAction, AquiredPokerAction>()
                .RegisterConstructor<IAquiredPokerRound, AquiredPokerRound>()
                .RegisterConstructor<IAquiredPokerPlayer, AquiredPokerPlayer>()
                .RegisterConstructor<IAquiredPokerHand, AquiredPokerHand>()
                .RegisterConstructor<IConvertedPokerAction, ConvertedPokerAction>()
                .RegisterConstructor<IConvertedPokerActionWithId, ConvertedPokerActionWithId>()
                .RegisterConstructor<IConvertedPokerRound, ConvertedPokerRound>()
                .RegisterConstructor<IConvertedPokerPlayer, ConvertedPokerPlayer>()
                .RegisterConstructor<IConvertedPokerHand, ConvertedPokerHand>()
                .RegisterType<IInvestedMoneyCondition, InvestedMoneyCondition>(new ContainerControlledLifetimeManager())
                .RegisterType<ISawFlopCondition, SawFlopCondition>(new ContainerControlledLifetimeManager())
                .RegisterTypeAndConstructor<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IHoleCardsViewModel, HoleCardsViewModel>()
                .RegisterType<IBoardViewModel, BoardViewModel>()
                .RegisterTypeAndConstructor<IHandHistoryViewModel, HandHistoryViewModel>()
                .RegisterType<IHandHistoriesViewModel, HandHistoriesViewModel>()
                .RegisterType<IHandHistoriesView, HandHistoriesView>();

            _regionManager
                .RegisterViewWithRegion(
                ApplicationProperties.HandHistoriesRegion, () => _container.Resolve<IHandHistoriesView>());
        }

        #endregion
    }
}