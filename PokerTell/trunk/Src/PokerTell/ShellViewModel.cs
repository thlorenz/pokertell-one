namespace PokerTell
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;

    using log4net;

    using Microsoft.Practices.Composite.Regions;

    using PokerTell.Infrastructure;

    using Tools.WPF;
    using Tools.WPF.Interfaces;
    using Tools.WPF.ViewModels;

    public class ShellViewModel : NotifyPropertyChanged, IShellViewModel
    {
        #region Constants and Fields

        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        readonly IRegionManager _regionManager;

        ICommand _mainRegionCloseSelectedItemCommand;

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
                WindowTitle = ApplicationProperties.ApplicationName;
            }
            catch (Exception excep)
            {
                Log.Error(excep);
            }
        }

        #endregion

        #region Properties

        string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        public ICommand MainRegionCloseSelectedItemCommand
        {
            get
            {
                return _mainRegionCloseSelectedItemCommand ?? (_mainRegionCloseSelectedItemCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            IRegion region = _regionManager.Regions[ApplicationProperties.ShellMainRegion];
                            region.Remove(MainRegionSelectedItem);
                            Status = string.Format("{0} views in MainRegion", region.Views.Count());
                        }, 
                        CanExecuteDelegate = arg => MainRegionSelectedItem != null
                    });
            }
        }

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

        IItemsRegionView _mainRegionSelectedItem;

        public IItemsRegionView MainRegionSelectedItem
        {
            get { return _mainRegionSelectedItem; }
            set
            {
                _mainRegionSelectedItem = value;
                if (_mainRegionSelectedItem != null)
                {
                    WindowTitle = _mainRegionSelectedItem.ActiveAwareViewModel.HeaderInfo;
                }
            }
        }

        string _windowTitle;

        public string WindowTitle
        {
            get { return _windowTitle; }
            set
            {
                _windowTitle = value;
                Log.Info("Title: " + WindowTitle);
                RaisePropertyChanged(() => WindowTitle);
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