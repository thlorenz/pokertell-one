namespace PokerTell.Statistics.Views
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    using Infrastructure.Interfaces.Statistics;

    using Tools.WPF.ViewModels;

    public partial class AnalyzablePokerPlayersFilterViewTemplate
    {
    // When the user opens the expander we automatically select its filter - it is more intuitive that way
        void Filter_Expanded(object sender, RoutedEventArgs e)
        {
            var expander = (Expander) sender;
            var filter = (IRangeFilterViewModel) expander.DataContext;
            filter.IsActive = true;
        }
    }
}