namespace PokerTell.DatabaseSetup.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Infrastructure.Events;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.DatabaseSetup.Properties;
    using PokerTell.Infrastructure.Interfaces.DatabaseSetup;

    using Tools.WPF;

    public class ChooseDataProviderViewModel : IComboBoxDialogViewModel
    {
        #region Constants and Fields

        readonly IList<IDataProviderInfo> _availableProviders;

        readonly IEventAggregator _eventAggregator;

        readonly IDatabaseSettings _databaseSettings;

        #endregion

        #region Constructors and Destructors

        public ChooseDataProviderViewModel(IEventAggregator eventAggregator, IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
            _eventAggregator = eventAggregator;
            _availableProviders = new List<IDataProviderInfo>(_databaseSettings.GetAvailableProviders());
            InitAvailableItems();
        }

        void PublishWarning()
        {
            var userMessage = new UserMessageEventArgs(
                UserMessageTypes.Warning, Resources.Warning_NoConfiguredDataProvidersFoundInSettings);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        #endregion

        #region Properties

       void InitAvailableItems()
       {
           AvailableItems = new ObservableCollection<string>(
               from providerInfo in _availableProviders
               orderby providerInfo.NiceName
               select providerInfo.NiceName);
        }

       public ObservableCollection<string> AvailableItems { get; private set; }
       
        ICommand _saveSettingsCommand;

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

        void SaveSettings()
        {
            var selectedProvider = (from providerInfo in _availableProviders
                                   where providerInfo.NiceName.Equals(SelectedItem)
                                   select providerInfo).First();
            
            _databaseSettings.SetCurrentDataProviderTo(selectedProvider);
        }

        void PublishInfoMessage()
        {
            string msg = string.Format(Resources.Info_DataProviderChosen, SelectedItem);
            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, msg);
            _eventAggregator.GetEvent<UserMessageEvent>().Publish(userMessage);
        }

        public string SelectedItem { get; set; }

        public string Title
        {
            get { return Resources.ChooseDataProviderViewModel_Title; }
        }

        public string ActionName
        {
            get { return Resources.Commands_Save; }
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

        public bool IsValid { get; private set; }

        #endregion
    }
}