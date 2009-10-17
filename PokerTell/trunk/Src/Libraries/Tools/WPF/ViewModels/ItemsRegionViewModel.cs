namespace Tools.WPF.ViewModels
{
    using System;

    using Interfaces;

    public class ItemsRegionViewModel : ViewModel, IItemsRegionViewModel
    {
        string _headerInfo;

        public string HeaderInfo
        {
            get { return _headerInfo ?? "No Title"; }
            set
            {
                _headerInfo = value;
                RaisePropertyChanged(() => HeaderInfo);
            }
        }

        bool _isActive;

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                OnIsActiveChanged();
            }
        }

        public event EventHandler IsActiveChanged = delegate { };

        protected virtual void OnIsActiveChanged()
        {
        }
    }
}