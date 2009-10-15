namespace PokerTell.PokerHand
{
    using System;
    using System.Reflection;

    using log4net;

    using Microsoft.Practices.Composite.Modularity;
    using Microsoft.Practices.Composite.Regions;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces.PokerHand;
    using PokerTell.PokerHand.Analyzation;
    using PokerTell.PokerHand.Aquisition;
    using PokerTell.PokerHand.ViewModels;
    using PokerTell.PokerHand.Views;

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
            try
            {
                _regionManager.AddToRegion("Shell.MainRegion", _container.Resolve<HandHistoriesView>());
                _regionManager.AddToRegion("Shell.MainRegion", _container.Resolve<HandHistoryView>());
                _regionManager.AddToRegion("Shell.MainRegion", _container.Resolve<HandHistoriesView>());
                _regionManager.AddToRegion("Shell.MainRegion", _container.Resolve<HandHistoriesView>());
            }
            catch (Exception excep)
            {
                Log.Error("Resolving", excep);
            }
          
            Log.Info("Initialized PokerHandModule");
        }

        #endregion

        #endregion

        #region Methods

        protected void RegisterViewsAndServices()
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
                .RegisterTypeAndConstructor<IHandHistoryViewModel, ViewModels.Design.HandHistoryViewModel>()

                .RegisterType<IHoleCardsViewModel, HoleCardsViewModel>()
                .RegisterType<IBoardViewModel, BoardViewModel>()
                .RegisterTypeAndConstructor<IHandHistoryViewModel, ViewModels.Design.HandHistoryViewModel>()
                .RegisterType<IHandHistoriesViewModel, ViewModels.Design.HandHistoriesViewModel>();
        }

        #endregion
    }
}