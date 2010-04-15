namespace PokerTell.Plugins.PlayerPeeker
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Commands;
    using Microsoft.Practices.Composite.Presentation.Events;

    using PokerTell.Infrastructure.Events;
    using PokerTell.Infrastructure.Interfaces;
    using PokerTell.Plugins.PlayerPeeker.Interfaces;

    using Tools.WPF;

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

        public Button ShowPlayerPeekerButton
        {
            get
            {
                return new Button
                    {
                        Command = new SimpleCommand
                            {
                                ExecuteDelegate = args => _playerPeekerForm.ActivateAndRestore(), 
                                CanExecuteDelegate = args => _playerPeekerForm != null
                            }, 
                        Content = "P", 
                        Width = 20, 
                        Height = 20, 
                        Padding = new Thickness(3)
                    };
            }
        }

        void AddNewPlayersToPlayerPeekerForm(NewHandEventArgs args)
        {
            if (_playerPeekerForm == null)
                CreateAndShowPlayerPeekerForm();

            _playerPeekerForm.NewPlayersFound(args.ConvertedPokerHand.Players.Select(p => p.Name));
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

        void RegisterEvents()
        {
            const bool keepMeAlive = true;

            _eventAggregator
                .GetEvent<NewHandEvent>()
                .Subscribe(AddNewPlayersToPlayerPeekerForm, ThreadOption.UIThread, keepMeAlive);
        }
    }
}