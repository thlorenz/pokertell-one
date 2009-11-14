namespace PokerTell.Repository.Database
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;

    using Infrastructure.Interfaces.Repository;

    using Interfaces;

    public class HandHistoriesDirectoryImporter : IHandHistoriesDirectoryImporter
    {
        readonly IRepository _repository;

        DirectoryInfo _directoryInfo;

        Action<int> _reportProgress;

        Action<int> _reportCompletion;

        public HandHistoriesDirectoryImporter(IRepository repository)
        {
            _repository = repository;
        }

        public IHandHistoriesDirectoryImporter InitializeWith(
            Action<int> reportProgress,
            Action<int> reportCompletion)
        {
            _reportCompletion = reportCompletion;
            _reportProgress = reportProgress;
           
            return this;
        }

        public HandHistoriesDirectoryImporter ImportDirectory(string directoryPath)
        {
            _directoryInfo = new DirectoryInfo(directoryPath);
            Action importDelegate = ImportDirectoryInBackground;
            importDelegate.BeginInvoke(null, null);
            return this;
        }

        void ImportDirectoryInBackground()
        {
            const string handHistoriesExtensionPattern = "*.txt";
            var filesToImport = _directoryInfo.GetFiles(handHistoriesExtensionPattern);
            int numberOfFilesToImport = filesToImport.Length;
            int numberOfImportedHands = 0;
           
            for (int fileIndex = 0; fileIndex < numberOfFilesToImport; fileIndex++)
            {
                var handsInCurrentFile = _repository.RetrieveHandsFromFile(filesToImport[fileIndex].FullName);
               
                _repository.InsertHands(handsInCurrentFile);
               
                numberOfImportedHands += handsInCurrentFile.Count(); 

                int currentPercentage = (fileIndex * 100) / numberOfFilesToImport;

                _reportProgress(currentPercentage);
            }

            _reportCompletion(numberOfImportedHands);
        }
    }
}