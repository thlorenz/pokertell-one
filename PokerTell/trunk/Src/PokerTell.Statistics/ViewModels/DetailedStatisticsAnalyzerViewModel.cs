namespace PokerTell.Statistics.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class DetailedStatisticsAnalyzerViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        NotifyPropertyChanged _currentViewModel;

        #endregion

        #region Constructors and Destructors

        public DetailedStatisticsAnalyzerViewModel(INotifyPropertyChanged viewModel)
        {
            ViewModelHistory = new List<NotifyPropertyChanged>();

            CreateCommands();
        }

        #endregion

        #region Properties

        public ICommand Close { get; private set; }

        public NotifyPropertyChanged CurrentViewModel
        {
            get { return _currentViewModel; }
        }

        public ICommand NavigateBack { get; private set; }

        public ICommand NavigateForward { get; private set; }

        public IList<NotifyPropertyChanged> ViewModelHistory { get; private set; }

        public bool Visible
        {
            get { return ViewModelHistory.Count > 0; }
        }

        #endregion

        #region Public Methods

        public void AddViewModel(NotifyPropertyChanged viewModel)
        {
            RemoveAllTableViewsInTableViewHistoryThatAreBehindCurrentTableView();

            ViewModelHistory.Add(viewModel);

        }

        public void NavigateTo(int index)
        {
            if (index > -1 && index < ViewModelHistory.Count)
            {
                _currentViewModel = ViewModelHistory[index];
            }
        }

        #endregion

        #region Methods

        void CreateCommands()
        {
            NavigateBack = new SimpleCommand
                {
                    ExecuteDelegate =
                        xx => NavigateTo(ViewModelHistory.IndexOf(_currentViewModel) - 1),
                    CanExecuteDelegate = xx => ViewModelHistory.IndexOf(_currentViewModel) > 0
                };


            NavigateForward = new SimpleCommand
                {
                    ExecuteDelegate =
                        xx => NavigateTo(ViewModelHistory.IndexOf(_currentViewModel) + 1),
                    CanExecuteDelegate =
                        xx =>
                        ViewModelHistory.IndexOf(_currentViewModel) <
                        ViewModelHistory.Count - 1
                };

            Close = new SimpleCommand
                {
                    ExecuteDelegate = xx => {
                        ViewModelHistory.Clear();
                        RaisePropertyChanged(() => Visible);
                    },
                };
        }

        void RemoveAllTableViewsInTableViewHistoryThatAreBehindCurrentTableView()
        {
            int indexOfCurrentView = ViewModelHistory.IndexOf(_currentViewModel);

            if (indexOfCurrentView > -1 && indexOfCurrentView < ViewModelHistory.Count - 1)
            {
                int removalIndex = indexOfCurrentView + 1;
                while (ViewModelHistory.Count > removalIndex)
                {
                    ViewModelHistory.RemoveAt(removalIndex);
                }
            }
        }

        #endregion
    }
}