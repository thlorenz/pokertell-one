namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using System;
    using System.Windows;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using Moq;

    public class NewHandCreatorLauncher
    {

        readonly IEventAggregator _eventAggregator;

        readonly IUnityContainer _container;

        NewHandCreatorViewModel _newHandCreatorViewModel;

        NewHandCreatorView _newHandCreatorView;

        public NewHandCreatorLauncher(IUnityContainer container, IEventAggregator eventAggregator)
        {
            _container = container;
            _eventAggregator = eventAggregator;
        }
       
        public NewHandCreatorLauncher Launch()
        {
            _newHandCreatorViewModel = new NewHandCreatorViewModel(_container, _eventAggregator);
            _newHandCreatorView = new NewHandCreatorView { Topmost = true, DataContext = _newHandCreatorViewModel };
            _newHandCreatorView.ShowDialog();
            return this;
        }

        public void StartTracking()
        {
            _newHandCreatorViewModel.StartTracking();
        }
    }
}