namespace PokerTell.LiveTracker.Tests.Tracking
{
    using System.Collections.Generic;
    using System.Linq;

    using LiveTracker.Tracking;

    using Machine.Specifications;

    // Resharper disable InconsistentNaming
    public abstract class WatchedDirectoriesOptimizerSpecs
    {
        protected static WatchedDirectoriesOptimizer _sut;

        protected static IList<string> _allFullPaths;

        protected static IEnumerable<string> _optimizedPaths;

        Establish specContext = () => _sut = new WatchedDirectoriesOptimizer();

        public class when_optimizing_an_empty_collection : WatchedDirectoriesOptimizerSpecs
        {
            Establish context = () => _allFullPaths = new string[] { };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It should_return_empty_collection = () => _optimizedPaths.ShouldBeEmpty();
        }

        public class when_optimizing_a_collection_containing_one_path : WatchedDirectoriesOptimizerSpecs
        {
            const string path = @"c:\somepath\";

            Establish context = () => _allFullPaths = new List<string> { path };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It should_return_collection_containing_only_that_one_file = () => _optimizedPaths.ShouldContainOnly(path);
        }

        public class when_the_first_folder_is_a_subirectory_of_the_second_folder : WatchedDirectoriesOptimizerSpecs
        {
            const string firstPath = @"c:\somepath\subdir";
            const string secondPath = @"c:\somepath\";

            Establish context = () => _allFullPaths = new List<string> { firstPath, secondPath };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It should_return_collection_containing_only_the_second_path = () => _optimizedPaths.ShouldContainOnly(secondPath);
        }

        public class when_the_first_folder_is_a_subirectory_of_the_second_folder_but_drive_letters_have_different_case : WatchedDirectoriesOptimizerSpecs
        {
            const string firstPath = @"c:\somepath\subdir";
            const string secondPath = @"C:\somepath\";

            Establish context = () => _allFullPaths = new List<string> { firstPath, secondPath };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It should_return_collection_containing_only_the_second_path = () => _optimizedPaths.ShouldContainOnly(secondPath);
        }

        public class when_the_first_folder_is_not_a_subirectory_of_the_second_folder : WatchedDirectoriesOptimizerSpecs
        {
            const string firstPath = @"c:\someOtherpath\subdir";
            const string secondPath = @"C:\somepath\";

            Establish context = () => _allFullPaths = new List<string> { firstPath, secondPath };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It returned_collection_should_contain_the_first_path = () => _optimizedPaths.ShouldContain(firstPath);

            It returned_collection_should_contain_the_second_path = () => _optimizedPaths.ShouldContain(secondPath);
        }

        public class when_first_path_is_equals_the_second_path : WatchedDirectoriesOptimizerSpecs
        {

            const string firstPath = @"c:\somepath\";
            const string secondPath = @"c:\somepath\";

            Establish context = () => _allFullPaths = new List<string> { firstPath, secondPath };

            Because of = () => _optimizedPaths = _sut.Optimize(_allFullPaths);

            It should_only_return_one_path = () => _optimizedPaths.Count().ShouldEqual(1);
        }
    }
}