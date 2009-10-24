namespace Tools.WPF.Views
{
    using System;
    using System.Windows.Controls;

    using Interfaces;

    using Microsoft.Practices.Composite;

    public class ItemsRegionView : UserControl, IItemsRegionView
    {
        bool _isActive;

        public event EventHandler IsActiveChanged = delegate { };

        public IItemsRegionViewModel ActiveAwareViewModel
        {
            get { return (IItemsRegionViewModel)DataContext; }
        }

        public bool IsActive
        {
            get { return _isActive; }

            set
            {
                _isActive = value;
               
                var activeAwareViewModel = DataContext as IActiveAware;
                if (activeAwareViewModel != null)
                {
                    activeAwareViewModel.IsActive = value;
                }
            }
        }
    }
}