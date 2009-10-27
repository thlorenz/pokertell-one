namespace PokerTell.PokerHand.ViewModels
{
    using System;
    using System.Collections.Generic;

    using Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    public class HandHistoriesTableViewModel : ViewModel
    {
        #region Constants and Fields

        private readonly IList<IConvertedPokerHand> _hands;

        private int _selectedIndex;

        #endregion

        #region Constructors and Destructors

        public HandHistoriesTableViewModel()
            : this(new List<IConvertedPokerHand>())
        {
        }

        public HandHistoriesTableViewModel(IList<IConvertedPokerHand> hands)
        {
            _hands = hands;
            CurrentHandHistory = new HandHistoryViewModel();
        }

        #endregion

        #region Properties


        public HandHistoryViewModel CurrentHandHistory { get; private set; }

        public int LastHandIndex
        {
            get { return _hands.Count - 1; }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }

            set
            {
                _selectedIndex = value;
                SelectHandHistoryAt(_selectedIndex);
                RaisePropertyChanged(() => SelectedIndex);
            }
        }

        #endregion

        #region Public Methods

        public void AddHand(IConvertedPokerHand hand)
        {
            _hands.Add(hand);
            RaisePropertyChanged(() => LastHandIndex);
        }

        #endregion

        #region Methods

        private void SelectHandHistoryAt(int index)
        {
            if (index < _hands.Count)
            {
                CurrentHandHistory.UpdateWith(_hands[index]);
                RaisePropertyChanged(() => CurrentHandHistory);
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        #endregion
    }
}