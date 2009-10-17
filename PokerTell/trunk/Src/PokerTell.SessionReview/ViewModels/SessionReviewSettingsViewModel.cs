namespace PokerTell.SessionReview.ViewModels
{
    using System.Reflection;
    using System.Windows.Input;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF;
    using Tools.WPF.ViewModels;

    public class SessionReviewSettingsViewModel : ViewModel
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Constants and Fields

        readonly IHandHistoriesViewModel _handHistoriesViewModel;

        readonly IInvestedMoneyCondition _investedMoneyCondition;

        readonly ISawFlopCondition _sawFlopCondition;

        string _heroName;

        #endregion

        #region Constructors and Destructors

        public SessionReviewSettingsViewModel(
            IHandHistoriesViewModel handHistoriesViewModel, 
            IInvestedMoneyCondition investedMoneyCondition, 
            ISawFlopCondition sawFlopCondition)
        {
            _sawFlopCondition = sawFlopCondition;
            _investedMoneyCondition = investedMoneyCondition;
            _handHistoriesViewModel = handHistoriesViewModel;
        }

        #endregion

        #region Properties

        public string HeroName
        {
            get { return _heroName; }
            set
            {
                _heroName = value;
                RaisePropertyChanged(() => HeroName);
            }
        }

        public ICommand MoneyInvestedCommand
        {
            get
            {
                return new SimpleCommand
                    {
                        ExecuteDelegate = arg => {
                            _investedMoneyCondition.AppliesTo(HeroName);
                            _handHistoriesViewModel.ApplyFilterCompositeAction.Invoke(_investedMoneyCondition);
                        }
                    };
            }
        }

        public ICommand SawFlopCommand
        {
            get
            {
                return new SimpleCommand
                {
                    ExecuteDelegate = arg =>
                    {
//                        _sawFlopCondition.AppliesTo(HeroName);
//                        _handHistoriesViewModel.ApplyFilterCompositeAction.Invoke(_sawFlopCondition);

                        Log.Info("SawFlop " + _handHistoriesViewModel.GetHashCode());
                    }
                };
            }
        }

        #endregion
    }
}