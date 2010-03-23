namespace PokerTell.DatabaseSetup.Tests
{
    using System;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using Moq;

    using ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class CreateDatabaseViewModelSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IDatabaseManager> _databaseManager_Mock;

        protected static CreateDatabaseViewModel _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _databaseManager_Mock = new Mock<IDatabaseManager>();
     
            _sut = new CreateDatabaseViewModel(_eventAggregator, _databaseManager_Mock.Object);
        };

        [Subject(typeof(CreateDatabaseViewModel), "CommitActionCommand")]
        public class when_SelectedItem_contains_leading_and_trailing_spaces : CreateDatabaseViewModelSpecs
        {
            const string trimmedDatabaseName = "some database";
            Establish context = () => _sut.SelectedItem = "  " + trimmedDatabaseName + "  ";
        
            Because of = () => _sut.CommitActionCommand.Execute(null);

            It should_tell_the_database_manager_to_create_a_database_with_the_trimmed_name 
                = () => _databaseManager_Mock.Verify(dm => dm.CreateDatabase(trimmedDatabaseName));
        }

        [Subject(typeof(CreateDatabaseViewModel), "CommitActionCommand")]
        public class when_during_database_creation_the_database_manager_encounters_an_error : CreateDatabaseViewModelSpecs
        {
            static Exception raisedException;

            static Exception publishedException;

            static bool userWasInformed;

            static bool successMessageWasPublished;

            static UserMessageTypes messageType;

            Establish context = () => {
                raisedException = new Exception();
                _databaseManager_Mock.Setup(dm => dm.CreateDatabase(Moq.It.IsAny<string>())).Throws(raisedException);

                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => {
                        userWasInformed = true;
                        messageType = args.MessageType;
                        publishedException = args.Exception;
                    },
                               ThreadOption.PublisherThread,
                               false,
                               args => args.MessageType == UserMessageTypes.Error);

                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => successMessageWasPublished = true, ThreadOption.PublisherThread, false, args => args.MessageType == UserMessageTypes.Info);

                _sut.SelectedItem = "some database";
            };

            Because of = () => _sut.CommitActionCommand.Execute(null);

            It should_let_the_user_know_about_it = () => userWasInformed.ShouldBeTrue();

            It should_publish_it_as_an_error = () => messageType.ShouldEqual(UserMessageTypes.Error);

            It should_publish_the_contained_exception = () => publishedException.ShouldBeTheSameAs(raisedException);

            It should_not_publish_a_success_message = () => successMessageWasPublished.ShouldBeFalse();

        }
    }
}