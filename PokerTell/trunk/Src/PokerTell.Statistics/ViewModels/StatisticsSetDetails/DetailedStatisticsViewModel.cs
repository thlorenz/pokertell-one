namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Windows.Input;

   using Base;

   using Infrastructure.Enumerations.PokerHand;
   using Infrastructure.Interfaces;
   using Infrastructure.Interfaces.PokerHand;
   using Infrastructure.Interfaces.Statistics;

   using Interfaces;

   using Tools.FunctionalCSharp;
   using Tools.Interfaces;
   using Tools.WPF;

    public abstract class DetailedStatisticsViewModel : StatisticsTableViewModel, IDetailedStatisticsViewModel
   {
      protected readonly IHandBrowserViewModel _handBrowserViewModel;


      protected DetailedStatisticsViewModel(IHandBrowserViewModel handBrowserViewModel, string columnHeaderTitle)
         : base(columnHeaderTitle)
      {
         _handBrowserViewModel = handBrowserViewModel;
      }

      /// <summary>
      ///   Assumes that cells have been selected and that they are all in the same row.
      ///   It returns the ActionSequence associated with the row of the first selected cell.
      /// </summary>
      public virtual ActionSequences SelectedActionSequence
      {
         get
         {
            int row = SelectedCells.First().First;

            return ActionSequenceStatisticsSet.ActionSequenceStatistics.ElementAt(row).ActionSequence;
         }
      }

      /// <summary>
      ///   Returns all AnalyzablePokerPlayers whose percentages were selected on the table
      /// </summary>
      public virtual IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
      {
         get
         {
            return SelectedCells.SelectMany(
               selectedCell => {
                  int row = selectedCell.First;
                  int col = selectedCell.Second;
                  return ActionSequenceStatisticsSet.ActionSequenceStatistics.ElementAt(row).MatchingPlayers[col];
               });
         }
      }

      /// <summary>
      ///   Provides the data for the Viewmodel
      ///   Needs to be set via InitializeWith before ViewModel becomes useful
      /// </summary>
      protected IActionSequenceStatisticsSet ActionSequenceStatisticsSet { get; private set; }

      protected string PlayerName
      {
         get { return ActionSequenceStatisticsSet.PlayerName; }
      }

      protected ITuple<int, int> SelectedColumnsSpan
      {
         get
         {
            int lowestSelectedColumnIndex = SelectedCells.Min(cell => cell.Second);
            int highestSelectedColumnIndex = SelectedCells.Max(cell => cell.Second);
            return Tuple.New(lowestSelectedColumnIndex, highestSelectedColumnIndex);
         }
      }

      /// <summary>
      ///   The street for which the given ActionSequenceStatisticsSet applies
      /// </summary>
      protected Streets Street
      {
         get { return ActionSequenceStatisticsSet.Street; }
      }

      /// <summary>
      ///   Needs to be called to fill viewmodel with data
      /// </summary>
      /// <param name="statisticsSet">
      ///   Provides underlying data
      /// </param>
      /// <returns>
      ///   Itself to enable fluent interface
      /// </returns>
      public IDetailedStatisticsViewModel InitializeWith(IActionSequenceStatisticsSet statisticsSet)
      {
         ActionSequenceStatisticsSet = statisticsSet;
         return CreateTableAndDescriptionFor(statisticsSet);
      }

        ICommand _browseHandsCommand;

        public ICommand BrowseHandsCommand
        {
            get
            {
                return _browseHandsCommand ?? (_browseHandsCommand = new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _handBrowserViewModel.InitializeWith(SelectedAnalyzablePlayers);
                            ChildViewModel = _handBrowserViewModel;
                        }, 
                        CanExecuteDelegate = arg => SelectedCells.Count > 0
                    });
            }
        }

      protected abstract IDetailedStatisticsViewModel CreateTableAndDescriptionFor(
         IActionSequenceStatisticsSet statisticsSet);
   }
}