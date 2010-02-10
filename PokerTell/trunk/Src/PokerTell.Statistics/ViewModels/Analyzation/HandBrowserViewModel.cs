namespace PokerTell.Statistics.ViewModels.Analyzation
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   using PokerTell.Infrastructure.Interfaces.PokerHand;
   using PokerTell.Infrastructure.Interfaces.Statistics;
   using PokerTell.Statistics.Interfaces;

   using Tools.WPF.ViewModels;

   public class HandBrowserViewModel : NotifyPropertyChanged, IHandBrowserViewModel
   {
      readonly IHandBrowser _handBrowser;

      int _currentHandIndex;

      public HandBrowserViewModel(IHandBrowser handBrowser, IHandHistoryViewModel handHistoryViewModel)
      {
         CurrentHandHistory = handHistoryViewModel;
         _handBrowser = handBrowser;
      }

      // This event does not occur here because this ViewModel the last in the chain
      public event Action<IDetailedStatisticsAnalyzerContentViewModel> ChildViewModelChanged = delegate { };

      public IHandHistoryViewModel CurrentHandHistory { get; protected set; }

      public int HandCount { get; protected set; }

      public int CurrentHandIndex
      {
         get { return _currentHandIndex; }
         set {
            _currentHandIndex = value;
            CurrentHandHistory.UpdateWith(_handBrowser.Hand(_currentHandIndex)); }
      }

      public IHandBrowserViewModel InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers)
      {
         var reversedHandIds = analyzablePokerPlayers.Select(p => p.HandId).Reverse();

         _handBrowser.InitializeWith(reversedHandIds);

         CurrentHandIndex = 0;

         HandCount = _handBrowser.PotentialHandsCount;
         return this;
      }
   }
}