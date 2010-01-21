namespace PokerTell.Infrastructure.Interfaces.Statistics
{
    using System.Collections.Generic;
    using System.Windows.Input;

    using Enumerations.PokerHand;

    public interface IDetailedStatisticsAnalyzerViewModel
    {
        IDetailedStatisticsViewModel CurrentViewModel { get; set; }

        ICommand NavigateBackwardCommand { get; }

        ICommand NavigateForwardCommand { get; }

        IList<IDetailedStatisticsViewModel> ViewModelHistory { get; }

        bool Visible { get; }

        IDetailedStatisticsAnalyzerViewModel AddViewModel(IDetailedStatisticsViewModel viewModel);

        IDetailedStatisticsAnalyzerViewModel NavigateTo(int index);

        IDetailedStatisticsAnalyzerViewModel InitializeWith(IActionSequenceStatisticsSet actionSequenceStatisticsSet);
    }
}