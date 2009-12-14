namespace CSharpLanguage.Tests
{
    using NUnit.Framework;

    using PokerTell.UnitTests.Tools;

    public class ReferenceTypesTests
    {
        [Test]
        public void ChangingPropertyOf_InstancePassedByReference_ChangeIsPresentInTheInstanceItWasPassedINto()
        {
            const string test = "Test";
            var bar = new Bar(test);
            var foo = new Foo(bar);

            bar.MyString = "changed";

            foo.BarString.IsEqualTo(bar.MyString);
        }

        [Test]
        public void RecreatingThenChangingPropertyOf_InstancePassedByReference_ChangeIsNotPresentInTheInstanceItWasPassedINto()
        {
            const string test = "Test";
            var bar = new Bar(test);
            var foo = new Foo(bar);

            bar = new Bar(test) { MyString = "changed" };

            foo.BarString.IsNotEqualTo(bar.MyString);
        }

        class Foo
        {
            readonly Bar _bar;

            public Foo(Bar bar)
            {
                _bar = bar;
            }

            public string BarString
            {
                get { return _bar.MyString; }
            }
        }

        class Bar
        {
            public string MyString;

            public Bar(string myString)
            {
                MyString = myString;
            }
        }
    }
}