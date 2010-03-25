namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Enumerations.PokerHand;

    using PokerHand;

    public interface IDetailedStatisticsAnalyzerViewModel
    {
        IDetailedStatisticsAnalyzerContentViewModel CurrentViewModel { get; set; }

        ICommand NavigateBackwardCommand { get; }

        ICommand NavigateForwardCommand { get; }

        IList<IDetailedStatisticsAnalyzerContentViewModel> ViewModelHistory { get; }

        bool Visible { get; }

        IDetailedStatisticsAnalyzerViewModel AddViewModel(IDetailedStatisticsAnalyzerContentViewModel viewModel);

        IDetailedStatisticsAnalyzerViewModel NavigateTo(int index);

        IDetailedStatisticsAnalyzerViewModel InitializeWith(IActionSequenceStatisticsSet actionSequenceStatisticsSet);

        IDetailedStatisticsAnalyzerViewModel InitializeWith(IEnumerable<IAnalyzablePokerPlayer> analyzablePokerPlayers, string playerName);
    }
}