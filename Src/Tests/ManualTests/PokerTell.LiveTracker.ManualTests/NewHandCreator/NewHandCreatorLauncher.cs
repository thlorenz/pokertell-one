namespace PokerTell.LiveTracker.ManualTests.NewHandCreator
{
    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Unity;

    using PokerTell.Infrastructure.Interfaces.Repository;

    public class NewHandCreatorLauncher
    {
        readonly IEventAggregator _eventAggregator;

        readonly IUnityContainer _container;

        NewHandCreatorViewModel _newHandCreatorViewModel;

        NewHandCreatorView _newHandCreatorView;

        readonly IRepository _repository;

        public NewHandCreatorLauncher(IUnityContainer container, IEventAggregator eventAggregator, IRepository repository)
        {
            _repository = repository;
            _container = container;
            _eventAggregator = eventAggregator;
        }

        public NewHandCreatorLauncher Launch()
        {
            _newHandCreatorViewModel = new NewHandCreatorViewModel(_container, _eventAggregator, _repository);
            _newHandCreatorView = new NewHandCreatorView { Topmost = true, DataContext = _newHandCreatorViewModel };
            _newHandCreatorViewModel.StartTracking();
            _newHandCreatorView.Show();
            return this;
        }
    }
}