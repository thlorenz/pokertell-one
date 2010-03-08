namespace DynamicDataTemplate
{
    using System.Windows.Input;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public interface IGenericViewModel
    {
        #region Properties

        string Display { get; set; }

        #endregion
    }

    public class GridViewModel : IGenericViewModel
    {
        #region Constructors and Destructors

        public GridViewModel()
        {
            Display = "Grid Content";
        }

        #endregion

        #region Properties

        public string Display { get; set; }

        #endregion
    }

    public class HandViewModel : IGenericViewModel
    {
        #region Constructors and Destructors

        public HandViewModel()
        {
            Display = "Hand Content";
        }

        #endregion

        #region Properties

        public string Display { get; set; }

        #endregion
    }

    public class ContainerViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        IGenericViewModel _currentViewModel;

        ICommand _useGridViewModelCommand;

        ICommand _useHandViewModelCommand;

        #endregion

        #region Constructors and Destructors

        public ContainerViewModel()
        {
            CurrentViewModel = new GridViewModel();
        }

        #endregion

        #region Properties

        public IGenericViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged(() => CurrentViewModel);
            }
        }

        public ICommand UseGridViewModelCommand
        {
            get
            {
                return _useGridViewModelCommand ?? (_useGridViewModelCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => CurrentViewModel = new GridViewModel()
                    });
            }
        }

        public ICommand UseHandViewModelCommand
        {
            get
            {
                return _useHandViewModelCommand ?? (_useHandViewModelCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => CurrentViewModel = new HandViewModel()
                    });
            }
        }

        #endregion
    }
}