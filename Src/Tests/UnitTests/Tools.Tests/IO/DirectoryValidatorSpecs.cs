namespace Tools.Tests.IO
{
    using Machine.Specifications;

    using Tools.IO;

    // Resharper disable InconsistentNaming
    public abstract class DirectoryValidatorSpecs
    {
        [Subject(typeof(DirectoryValidator), "IsValidDirectory")]
        public class when_asking_if_a_directory_is_valid
        {
            static readonly string nullString = null;

            It should_return_false_for_null_string = () => nullString.IsValidDirectory().ShouldBeFalse();

            It should_return_false_for_empty_string = () => string.Empty.IsValidDirectory().ShouldBeFalse();

            It should_return_true_for_root_directory = () => @"c:\".IsValidDirectory().ShouldBeTrue();
            
            It should_return_true_for_root_directory_with_leading_white_spaces = () => @"  c:\".IsValidDirectory().ShouldBeTrue();

            It should_return_true_for__nonExistingButValid__ = () => "nonExistingButValid".IsValidDirectory().ShouldBeTrue();

            It should_return_false_for_______ = () => " ".IsValidDirectory().ShouldBeFalse();
        }

        [Subject(typeof(DirectoryValidator), "IsExistingDirectory")]
        public class when_asking_if_a_directory_is_existing
        {
            static readonly string nullString = null;

            It should_return_false_for_null_string = () => nullString.IsExistingDirectory().ShouldBeFalse();

            It should_return_false_for_empty_string = () => string.Empty.IsExistingDirectory().ShouldBeFalse();

            It should_return_true_for_root_directory = () => @"c:\".IsExistingDirectory().ShouldBeTrue();
            
            It should_return_true_for_root_directory_with_leading_white_spaces = () => @"  c:\".IsExistingDirectory().ShouldBeTrue();

            It should_return_false_for__nonExistingButValid__ = () => "nonExistingButValid".IsExistingDirectory().ShouldBeFalse();

            It should_return_false_for_______ = () => " ".IsExistingDirectory().ShouldBeFalse();
        }
    }
}