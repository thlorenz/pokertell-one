namespace PokerTell.Plugins.PlayerPeeker
{
    using System.Linq;

    using Infrastructure.Events;
    using Infrastructure.Interfaces;

    using Interfaces;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    public class PlayerPeekerService
    {
        readonly IEventAggregator _eventAggregator;

        readonly IConstructor<IPlayerPeekerForm> _playerPeekerFormMake;

        IPlayerPeekerForm _playerPeekerForm;

        public PlayerPeekerService(IEventAggregator eventAggregator, IConstructor<IPlayerPeekerForm> playerPeekerFormMake)
        {
            _playerPeekerFormMake = playerPeekerFormMake;
            _eventAggregator = eventAggregator;

            RegisterEvents();
        }

        void RegisterEvents()
        {
            const bool keepMeAlive = true;

            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Subscribe(AddNewPlayersToPlayerPeekerForm, ThreadOption.UIThread, keepMeAlive);
        }

        void CreateAndShowPlayerPeekerForm()
        {
            _playerPeekerForm = _playerPeekerFormMake.New;

            _playerPeekerForm.ResetRequested += delegate {
                _playerPeekerForm.Close();
                _playerPeekerForm.Dispose();

                CreateAndShowPlayerPeekerForm();
            };
            _playerPeekerForm.Show();
        }

        void AddNewPlayersToPlayerPeekerForm(NewHandEventArgs args)
        {
            if (_playerPeekerForm == null)
                CreateAndShowPlayerPeekerForm();
            
            _playerPeekerForm.NewPlayersFound(args.ConvertedPokerHand.Players.Select(p => p.Name));           
        }
    }
}