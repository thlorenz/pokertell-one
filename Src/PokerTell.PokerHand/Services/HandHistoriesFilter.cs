namespace PokerTell.PokerHand.Services
{
    using System;

    using PokerTell.Infrastructure.Interfaces.PokerHand;

    using Tools.WPF.ViewModels;

    [Serializable]
    public class HandHistoriesFilter : NotifyPropertyChanged, IHandHistoriesFilter
    {
        string _heroName;

        bool _selectHero;

        bool _showAll;

        bool _showMoneyInvested;

        bool _showPreflopFolds;

        bool _showSawFlop;

        bool _showSelectedOnly;

        [field: NonSerialized]
        public event Action HeroNameChanged;

        [field: NonSerialized]
        public event Action SelectHeroChanged;

        [field: NonSerialized]
        public event Action ShowAllChanged;

        [field: NonSerialized]
        public event Action ShowMoneyInvestedChanged;

        [field: NonSerialized]
        public event Action ShowPreflopFoldsChanged;

        [field: NonSerialized]
        public event Action ShowSawFlopChanged;

        [field: NonSerialized]
        public event Action ShowSelectedOnlyChanged;

        public string HeroName
        {
            get { return _heroName; }
            set
            {
                _heroName = value;
                InvokeHeroNameChanged();
            }
        }

        public bool SelectHero
        {
            get { return _selectHero; }
            set
            {
                _selectHero = value;
                InvokeSelectHeroChanged();
            }
        }

        public bool ShowAll
        {
            get { return _showAll; }
            set
            {
                _showAll = value;
                InvokeShowAllChanged();
            }
        }

        public bool ShowMoneyInvested
        {
            get { return _showMoneyInvested; }
            set
            {
                _showMoneyInvested = value;
                InvokeShowMoneyInvestedChanged();
            }
        }

        public bool ShowPreflopFolds
        {
            get { return _showPreflopFolds; }
            set
            {
                _showPreflopFolds = value;
                InvokeShowPreflopFoldsChanged();
            }
        }

        public bool ShowSawFlop
        {
            get { return _showSawFlop; }
            set
            {
                _showSawFlop = value;
                InvokeShowSawFlopChanged();
            }
        }

        public bool ShowSelectedOnly
        {
            get { return _showSelectedOnly; }
            set
            {
                _showSelectedOnly = value;
                InvokeShowSelectedOnlyChanged();
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(HandHistoriesFilter))
            {
                return false;
            }

            return Equals((HandHistoriesFilter)obj);
        }

        public bool Equals(HandHistoriesFilter other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return other.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = _heroName != null ? _heroName.GetHashCode() : 0;
                result = (result * 397) ^ _selectHero.GetHashCode();
                result = (result * 397) ^ _showAll.GetHashCode();
                result = (result * 397) ^ _showMoneyInvested.GetHashCode();
                result = (result * 397) ^ _showPreflopFolds.GetHashCode();
                result = (result * 397) ^ _showSawFlop.GetHashCode();
                result = (result * 397) ^ _showSelectedOnly.GetHashCode();
                return result;
            }
        }

        public override string ToString()
        {
            return
                string.Format(
                    "HeroName: {0}, SelectHero: {1}, ShowAll: {2}, ShowMoneyInvested: {3}, ShowPreflopFolds: {4}, ShowSawFlop: {5}, ShowSelectedOnly: {6}", 
                    _heroName, 
                    _selectHero, 
                    _showAll, 
                    _showMoneyInvested, 
                    _showPreflopFolds, 
                    _showSawFlop, 
                    _showSelectedOnly);
        }

        void InvokeHeroNameChanged()
        {
            Action changed = HeroNameChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeSelectHeroChanged()
        {
            Action changed = SelectHeroChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeShowAllChanged()
        {
            Action changed = ShowAllChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeShowMoneyInvestedChanged()
        {
            Action changed = ShowMoneyInvestedChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeShowPreflopFoldsChanged()
        {
            Action changed = ShowPreflopFoldsChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeShowSawFlopChanged()
        {
            Action changed = ShowSawFlopChanged;
            if (changed != null)
            {
                changed();
            }
        }

        void InvokeShowSelectedOnlyChanged()
        {
            Action changed = ShowSelectedOnlyChanged;
            if (changed != null)
            {
                changed();
            }
        }
    }
}