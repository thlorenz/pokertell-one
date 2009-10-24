namespace PokerTell.PokerHand.Views
{
    using System;
    using System.Reflection;
    using System.Windows.Controls;

    using log4net;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    /// <summary>
    /// Interaction logic for HandHistoriesView.xaml
    /// </summary>
    public partial class HandHistoriesView : IHandHistoriesView
    {
        static readonly ILog Log =
            LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructors and Destructors

        public HandHistoriesView()
        {
            InitializeComponent();
        }

        public HandHistoriesView(IHandHistoriesViewModel viewModel)
            : this()
        {
            DataContext = viewModel;

            viewModel.PageTurn += () => HandsScrollViewer.ScrollToHome();
           
            InitializeComponent();
           
        }

        #endregion

        #region Properties

        public IHandHistoriesViewModel ViewModel
        {
            get { return (IHandHistoriesViewModel)DataContext; }
        }

        #endregion
    }
}