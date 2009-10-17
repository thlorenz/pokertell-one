namespace PokerTell
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using log4net;

    using Microsoft.Practices.Composite.Regions;

    using PokerTell.Infrastructure;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class ShellViewModel : ViewModel, IShellViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRegionManager _regionManager;

        ICommand _developmentCommand;

        ICommand _maximizeWindowCommand;

        ICommand _minimizeWindowCommand;

        ICommand _normalizeWindowCommand;

        WindowState _windowState;

        #endregion

        #region Constructors and Destructors

        public ShellViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
          
            try
            {
                WindowState = WindowState.Normal;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }
        }

        #endregion

        #region Properties

        public ICommand DevelopmentCommand
        {
            get
            {
                return _developmentCommand ?? (_developmentCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => ListActiveViewsOfRegion(ApplicationProperties.ShellMainRegion), 
                        CanExecuteDelegate = arg => true
                    });
            }
        }

        public ICommand MaximizeWindowCommand
        {
            get
            {
                return _maximizeWindowCommand ?? (_maximizeWindowCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => WindowState = WindowState.Maximized
                    });
            }
        }

        public ICommand MinimizeWindowCommand
        {
            get
            {
                return _minimizeWindowCommand ?? (_minimizeWindowCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => WindowState = WindowState.Minimized
                    });
            }
        }

        public ICommand NormalizeWindowCommand
        {
            get
            {
                return _normalizeWindowCommand ?? (_normalizeWindowCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => WindowState = WindowState.Normal
                    });
            }
        }

        public bool ShowMaximize
        {
            get { return !(WindowState == WindowState.Maximized); }
        }

        public bool ShowNormalize
        {
            get { return WindowState == WindowState.Maximized; }
        }

        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                RaisePropertyChanged(() => WindowState);
                RaisePropertyChanged(() => ShowMaximize);
                RaisePropertyChanged(() => ShowNormalize);
            }
        }

        #endregion

        #region Methods

        void ListActiveViewsOfRegion(string regionName)
        {
            Log.InfoFormat("ActiveViews in region {0}:", regionName);
            foreach (object activeView in _regionManager.Regions[regionName].ActiveViews)
            {
                Log.Info(activeView.GetType().Name);
            }
        }

        #endregion
    }
}