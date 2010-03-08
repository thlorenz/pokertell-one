namespace PokerTell.Repository.ViewModels
{
    using System;
    using System.IO;
    using System.Windows.Forms;
    using System.Windows.Input;

    using Infrastructure.Events;
    using Infrastructure.Properties;

    using Microsoft.Practices.Composite.Events;

    using PokerTell.Repository.Interfaces;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class ImportHandHistoriesViewModel : NotifyPropertyChanged
    {
        #region Constants and Fields

        readonly IHandHistoriesDirectoryImporter _handHistoriesDirectoryImporter;

        ICommand _browseCommand;

        string _handHistoriesDirectory;

        ICommand _importCommand;

        bool _importing;

        readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public ImportHandHistoriesViewModel(
            IEventAggregator eventAggregator,
            IHandHistoriesDirectoryImporter handHistoriesDirectoryImporter)
        {
            _eventAggregator = eventAggregator;
            _handHistoriesDirectoryImporter = handHistoriesDirectoryImporter;
            const Environment.SpecialFolder programsFolder = Environment.SpecialFolder.ProgramFiles;
            HandHistoriesDirectory = Environment.GetFolderPath(programsFolder);

           // HandHistoriesDirectory = @"C:\Program Files\PokerStars\HandHistory\renniweg";
        }

        #endregion

        #region Properties

        public ICommand BrowseCommand
        {
            get
            {
                return _browseCommand ?? (_browseCommand = new SimpleCommand
                    {
                        ExecuteDelegate = BrowseForDirectory, 
                        CanExecuteDelegate = arg => ! Importing
                    });
            }
        }

        public string HandHistoriesDirectory
        {
            get { return _handHistoriesDirectory; }
            set
            {
                _handHistoriesDirectory = value;
                RaisePropertyChanged(() => HandHistoriesDirectory);
            }
        }

        public ICommand ImportCommand
        {
            get
            {
                return _importCommand ?? (_importCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            Importing = true;
                            
                            _handHistoriesDirectoryImporter
                                .InitializeWith(ReportProgress,
                                                ReportCompleted)
                                .ImportDirectory(_handHistoriesDirectory);

                            ReportProgress(0);
                        },
                        CanExecuteDelegate = arg => ! Importing && new DirectoryInfo(_handHistoriesDirectory).Exists
                    });
            }
        }

        public bool Importing
        {
            get { return _importing; }
            private set
            {
                _importing = value;
                RaisePropertyChanged(() => Importing);
            }
        }

        #endregion

        #region Methods

        void BrowseForDirectory(object arg)
        {
            Console.WriteLine(Importing);
            using (var browserDialog = new FolderBrowserDialog
                {
                    SelectedPath = HandHistoriesDirectory, ShowNewFolderButton = false 
                })
            {
                browserDialog.ShowDialog();
                HandHistoriesDirectory = browserDialog.SelectedPath;
            }
        }

        void ReportCompleted(int numberOfHandsImported)
        {
            Importing = false;

            string message = string.Format(Properties.Resources.Info_HandHistoriesDirectoryImportCompleted,
                                           numberOfHandsImported);

            var userMessage = new UserMessageEventArgs(UserMessageTypes.Info, message);
            _eventAggregator
                .GetEvent<UserMessageEvent>()
                .Publish(userMessage);

            ReportProgress(100);
        }

        void ReportProgress(int percentage)
        {
            var progressUpdate = new ProgressUpdateEventArgs(ProgressTypes.HandHistoriesDirectoryImport, percentage);
            _eventAggregator
                .GetEvent<ProgressUpdateEvent>()
                .Publish(progressUpdate);
        }

        #endregion
    }
}