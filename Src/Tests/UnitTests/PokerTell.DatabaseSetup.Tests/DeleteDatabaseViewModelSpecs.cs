namespace PokerTell.DatabaseSetup.Tests
{
    using System;
    using System.Linq.Expressions;

    using Infrastructure.Events;
    using Infrastructure.Interfaces.DatabaseSetup;

    using Machine.Specifications;

    using Microsoft.Practices.Composite.Events;
    using Microsoft.Practices.Composite.Presentation.Events;

    using Moq;

    using ViewModels;

    using It = Machine.Specifications.It;

    // Resharper disable InconsistentNaming
    public abstract class DeleteDatabaseViewModelSpecs
    {
        protected static IEventAggregator _eventAggregator;

        protected static Mock<IDatabaseManager> _databaseManager_Mock;

        protected static DeleteDatabaseViewModelSut _sut;

        Establish specContext = () => {
            _eventAggregator = new EventAggregator();
            _databaseManager_Mock = new Mock<IDatabaseManager>();
     
            _sut = new DeleteDatabaseViewModelSut(_eventAggregator, _databaseManager_Mock.Object);
        };


        [Subject(typeof(DeleteDatabaseViewModel), "CommitActionCommand")]
        public class when_two_databases_are_available_for_deletion_and_the_user_confirms_to_delete_one : DeleteDatabaseViewModelSpecs
        {
            const string deletedDatabase = "databaseName";
            const string otherDatabase = "other database Name";
            Establish context = () => {
            _sut.AvailableItems.Add(otherDatabase);
            _sut.AvailableItems.Add(deletedDatabase);

            _sut.SelectedItem = deletedDatabase;

            _eventAggregator
                .GetEvent<UserConfirmActionEvent>()
                .Subscribe(arg => arg.ActionToConfirm());
            };

            Because of = () => _sut.CommitActionCommand.Execute(null);

            It should_tell_the_database_manager_to_delete_the_database = () => _databaseManager_Mock.Verify(dm => dm.DeleteDatabase(deletedDatabase));

            It should_remove_the_deleted_database_from_the_available_items = () => _sut.AvailableItems.ShouldNotContain(deletedDatabase);

            It should_not_remove_the_other_database_from_the_available_items = () => _sut.AvailableItems.ShouldContain(otherDatabase);
        }

        [Subject(typeof(DeleteDatabaseViewModel), "RemoveDatabaseInUseFromAvailableItems")]
        public class when_two_databases_are_available_and_it_is_told_to_remove_the_database_in_use_from_them : DeleteDatabaseViewModelSpecs
        {
            const string otherDatabase = "databaseName";
            const string databaseInUse = "databaseInUse";

            Establish context = () => {
                _databaseManager_Mock
                    .Setup(dm => dm.GetDatabaseInUse())
                    .Returns(databaseInUse);

                _sut.AvailableItems.Add(otherDatabase);
                _sut.AvailableItems.Add(databaseInUse);
            };

            Because of = () => _sut.RemoveDatabaseInUseFromAvailableItems();

            It should_remove_the_database_in_use_from_the_available_items = () => _sut.AvailableItems.ShouldNotContain(databaseInUse);

            It should_not_remove_the_other_database_from_the_available_items = () => _sut.AvailableItems.ShouldContain(otherDatabase);
        }

        [Subject(typeof(DeleteDatabaseViewModel), "DetermineSelectedItem")]
        public class when_no_databases_are_available : DeleteDatabaseViewModelSpecs
        {
            Because of = () => _sut.DetermineSelectedItem();

            It should_set_it_to_empty = () => _sut.SelectedItem.ShouldEqual(string.Empty);
        }

        [Subject(typeof(DeleteDatabaseViewModel), "DetermineSelectedItem")]
        public class when_the_first_item_is_null : DeleteDatabaseViewModelSpecs
        {
            Establish context = () => _sut.AvailableItems.Add(null);

            Because of = () => _sut.DetermineSelectedItem();

            It should_set_it_to_empty = () => _sut.SelectedItem.ShouldEqual(string.Empty);
        }

        [Subject(typeof(DeleteDatabaseViewModel), "DetermineSelectedItem")]
        public class when_it_contains_two_available_databases : DeleteDatabaseViewModelSpecs
        {
            const string firstDatabase = "first";
            const string secondDatabase = "second";

            Establish context = () => {
                _sut.AvailableItems.Add(firstDatabase);
                _sut.AvailableItems.Add(secondDatabase);
            };

            Because of = () => _sut.DetermineSelectedItem();

            It should_set_it_to_the_first_database = () => _sut.SelectedItem.ShouldEqual(firstDatabase);
        }

        [Subject(typeof(DeleteDatabaseViewModel), "DeleteDatabase")]
        public class when_an_error_occurs_during_database_deletion : DeleteDatabaseViewModelSpecs
        {
            static Exception raisedException;

            static Exception publishedException;

            static bool successInfoWasPublished;

            Establish context = () => {
                raisedException = new Exception();
                _databaseManager_Mock
                    .Setup(dm => dm.DeleteDatabase(Moq.It.IsAny<string>()))
                    .Throws(raisedException);

                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => publishedException = args.Exception, ThreadOption.PublisherThread, false, args => args.MessageType == UserMessageTypes.Error);

                _eventAggregator
                    .GetEvent<UserMessageEvent>()
                    .Subscribe(args => successInfoWasPublished = true, ThreadOption.PublisherThread, false, args => args.MessageType == UserMessageTypes.Info);
            };

            Because of = () => _sut.DeleteDatabaseAndPublishInfoMessage_Invoke();

            It should_publish_the_exception_as_a_UserErrorMessage = () => publishedException.ShouldBeTheSameAs(raisedException);

            It should_not_publish_a_success_message = () => successInfoWasPublished.ShouldBeFalse();

        }

    }

    public class DeleteDatabaseViewModelSut : DeleteDatabaseViewModel
    {
        public DeleteDatabaseViewModelSut(IEventAggregator eventAggregator, IDatabaseManager databaseManager)
            : base(eventAggregator, databaseManager)
        {
        }

        public void DeleteDatabaseAndPublishInfoMessage_Invoke()
        {
            DeleteDatabaseAndPublishInfoMessage();
        }
    }
}