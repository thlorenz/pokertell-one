namespace PokerTell.DatabaseSetup.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Interfaces;
    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF;

    public class ChooseDataProviderViewModel : IComboBoxDialogViewModel
    {
        readonly IList<IDataProviderInfo> _availableProviders;

        readonly IDatabaseSettings _databaseSettings;

        readonly IEventAggregator _eventAggregator;

        ICommand _saveSettingsCommand;

        public ChooseDataProviderViewModel(IEventAggregator eventAggregator, IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
            _eventAggregator = eventAggregator;
            _availableProviders = new List<IDataProviderInfo>(_databaseSettings.GetAvailableProviders());
            InitAvailableItems();
        }

        public string ActionName
        {
            get { return Infrastructure.Properties.Resources.Commands_Save; }
        }

        public ObservableCollection<string> AvailableItems { get; private set; }

        public ICommand CommitActionCommand
        {
            get
            {
                return _saveSettingsCommand ?? (_saveSettingsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            SaveSettings();
                            PublishInfoMessage();
                        }
                    });
            }
        }

        public bool IsValid { get; private set; }

        public string SelectedItem { get; set; }

        public string Title
        {
            get { return Resources.ChooseDataProviderViewModel_Title; }
        }

        public IComboBoxDialogViewModel DetermineSelectedItem()
        {
            if (_availableProviders.Count > 0)
            {
                IsValid = true;
                var currentProvider = _databaseSettings.GetCurrentDataProvider();
                SelectedItem = currentProvider != null ? currentProvider.NiceName : AvailableItems.First();
            }
            else
            {
                IsValid = false;
                PublishWarning();
            }

            return this;
        }

        void InitAvailableItems()
        {
            AvailableItems = new ObservableCollection<string>(
                from providerInfo in _availableProviders
                orderby providerInfo.NiceName
                select providerInfo.NiceName);
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DataProviderChosen, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void PublishWarning()
        {
            var userMessage = new UserMessageEventArgs(
                UserMessageTypes.Warning, Resources.Warning_NoConfiguredDataProvidersFoundInSettings);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        void SaveSettings()
        {
            var selectedProvider = (from providerInfo in _availableProviders
                                    where providerInfo.NiceName.Equals(SelectedItem)
                                    select providerInfo).First();

            _databaseSettings.SetCurrentDataProviderTo(selectedProvider);
        }
    }
}