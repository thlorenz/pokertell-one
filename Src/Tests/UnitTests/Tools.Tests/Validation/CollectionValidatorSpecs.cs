namespace Tools.Tests.Validation
{
    using Machine.Specifications;

    using Tools.Interfaces;
    using Tools.Validation;

    // Resharper disable InconsistentNaming
    public abstract class CollectionValidatorSpecs
    {
        protected static ICollectionValidator _sut;

        Establish specContext = () => _sut = new CollectionValidator();

        [Subject(typeof(CollectionValidator), "GetValidIndexForCollection")]
        public class when_obtaining_a_valid_index_and_passing_index_and_items_count : CollectionValidatorSpecs
        {
            It should_return___0___for_index___0___and_itemsCount___0__ = () => _sut.GetValidIndexForCollection(0, 0).ShouldEqual(0);

            It should_return___0___for_index___0___and_itemsCount___1__ = () => _sut.GetValidIndexForCollection(0, 1).ShouldEqual(0);

            It should_return___0___for_index___1___and_itemsCount___1__ = () => _sut.GetValidIndexForCollection(1, 1).ShouldEqual(0);

            It should_return___0___for_index___negative_1___and_itemsCount___1__ = () => _sut.GetValidIndexForCollection(-1, 1).ShouldEqual(0);

            It should_return___0___for_index___negative_1___and_itemsCount___0__ = () => _sut.GetValidIndexForCollection(-1, 0).ShouldEqual(0);

            It should_return___1___for_index___3___and_itemsCount___2__ = () => _sut.GetValidIndexForCollection(3, 2).ShouldEqual(1);
        }
    }
}