namespace PokerTell.Statistics.ViewModels.StatisticsSetDetails
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;

    using Infrastructure.Enumerations.PokerHand;

    using Tools.WPF;

    public class DetailedPostFlopReactionStatisticsViewModel : DetailedStatisticsViewModel
    {
        #region Constants and Fields

        ICommand _investigateCommand;

        #endregion

        #region Constructors and Destructors

        public DetailedPostFlopReactionStatisticsViewModel()
            : base("Bet Size")
        {
        }

        #endregion

        #region Properties

        public ICommand InvestigateCommand
        {
            get
            {
                return _investigateCommand ?? (_investigateCommand = new SimpleCommand
                    {
                        ExecuteDelegate = _ => {
                            var sb = new StringBuilder();
                            sb.AppendLine("Investigating: ");
                            SelectedCells.ToList().ForEach(coord => sb.Append(coord + "; "));
                            Console.WriteLine(sb);
                        },
                        CanExecuteDelegate = _ => SelectedCells.FirstOrDefault(tuple => tuple.First != (int)ReactionTypes.Fold) != null
                    });
            }
        }

        #endregion
    }
}