namespace PokerTell.Statistics.ViewModels.Analyzation
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;

    using Base;

    using Interfaces;

    using Tools.FunctionalCSharp;

    public class PreFlopStartingHandsVisualizerViewModel : StatisticsTableViewModel, IPreFlopStartingHandsVisualizerViewModel 
    {
        readonly IPreFlopStartingHandsVisualizer _startingHandsVisualizer;

        public PreFlopStartingHandsVisualizerViewModel(IPreFlopStartingHandsVisualizer startingHandsVisualizer)
            : base("not used")
        {
            _startingHandsVisualizer = startingHandsVisualizer;
            _startingHandsVisualizer.InitializeWith(20, 30);
            StartingHands = new ObservableCollection<IStartingHand>();
            StatisticsDescription = DescribeStatistics();
        }

        public IPreFlopStartingHandsVisualizerViewModel Visualize(IEnumerable<string> startingHands)
        {
            _startingHandsVisualizer.Visualize(startingHands);
            
            StartingHands.Clear();
            _startingHandsVisualizer.StartingHands.Values.ForEach(sh => StartingHands.Add(sh));
            
            return this;
        }

        static string DescribeStatistics()
        {
            var sb = new StringBuilder();
            return sb
                .AppendLine("The table shows all hands that were found at least once as white, others as black.")
                .AppendLine("The more often they were found, the more red their background is.")
                .AppendLine("Cards to the left of the pairs are offsuit, the ones to the right are suited.")
                .ToString();
        }

        public IList<IStartingHand> StartingHands { get; protected set; }
    }
}