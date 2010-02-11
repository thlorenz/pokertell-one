namespace PokerTell.Statistics.Tests.ViewModels.StatisticsSetDetails
{
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;
    using Infrastructure.Interfaces.Statistics;

    using Interfaces;

    using Statistics.ViewModels.StatisticsSetDetails;

    using Tools.Interfaces;

    public class DetailedStatisticsViewModelImpl : DetailedStatisticsViewModel
    {
        public DetailedStatisticsViewModelImpl(IHandBrowserViewModel handBrowserViewModel)
            : base(handBrowserViewModel, "columnHeaderTitle")
        {
        }

        public ITuple<int, int> SelectedColumnsSpanGet
        {
            get { return SelectedColumnsSpan; }
        }

        internal List<IAnalyzablePokerPlayer> SelectedAnalyzablePlayersSet { private get; set; }

        public override IEnumerable<IAnalyzablePokerPlayer> SelectedAnalyzablePlayers
        {
            get
            {
                return SelectedAnalyzablePlayersSet;
            }
        }

        protected override IDetailedStatisticsViewModel CreateTableAndDescriptionFor(IActionSequenceStatisticsSet statisticsSet)
        {
            return this;
        }
    }
}