namespace Moq
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using PokerTell.UnitTests;

    [TestFixture]
    internal class ThatStubBuilder
    {
        [Test]
        public void Out_Interface_ObjectOfTypeInterface()
        {
            var foo = new StubBuilder().Out<IFoo>();
            Assert.That(foo, Is.Not.Null);
        }

        [Test]
        public void Out_SetupGetForId_StubsId()
        {
            const int stubbedValue = 2;
            var foo = new StubBuilder()
                .Setup<IFoo>().Get(f => f.Id).Returns(stubbedValue).Out;

            Assert.That(foo.Id, Is.EqualTo(stubbedValue));
        }

        [Test]
        public void Out_SetupGetForIdAndName_StubsName()
        {
            const int stubbedInt = 2;
            const string stubbedString = "string";

            var foo = new StubBuilder()
                .Setup<IFoo>()
                .Get(f => f.Id).Returns(stubbedInt)
                .Get(f => f.Name).Returns(stubbedString)
                .Out;

            Assert.That(foo.Name, Is.EqualTo(stubbedString));
        }

        [Test]
        public void Out_SetupGetForIdAndName_StubsId()
        {
            const int stubbedInt = 2;
            const string stubbedString = "string";

            var foo = new StubBuilder()
                .Setup<IFoo>().Get(f => f.Id).Returns(stubbedInt)
                .Get(f => f.Name).Returns(stubbedString)
                .Out;

            Assert.That(foo.Id, Is.EqualTo(stubbedInt));
        }

        [Test]
        public void OutForKey_AddedStubForTheKey_ReturnsStubbedValue()
        {
            const int stubValue = 11;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue);

            Assert.That(stub.Out<int>(For.GameId), Is.EqualTo(stubValue));
        }

        [Test]
        public void GetStubForKey_AddedStubForTheKey_ReturnsStubbedValue()
        {
            const int stubValue = 11;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue);

            Assert.That(stub.Get<int>(For.GameId), Is.EqualTo(stubValue));
        }

        [Test]
        public void OutForKey_DidNotAddStubForTheKey_ReturnsDefaultValue()
        {
            var stub = new StubBuilder();

            Assert.That(stub.Out<int>(For.GameId), Is.EqualTo(default(int)));
        }

        [Test]
        public void GetStubForKey_DidNotAddStubForTheKey_ThrowsKeyNotFoundException()
        {
            var stub = new StubBuilder();

            TestDelegate invokeGetStub = () => stub.Get<double>(For.GameId);

            Assert.Throws<KeyNotFoundException>(invokeGetStub);
        }

        [Test]
        public void OutForKey_TargetTypeNotEqualStubbedType_ThrowsArgumentException()
        {
            const int stubValue = 11;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue);

            TestDelegate invokeOut = () => stub.Out<double>(For.GameId);

            Assert.Throws<ArgumentException>(invokeOut);
        }

        [Test]
        public void GetStubForKey_TargetTypeNotEqualStubbedType_ThrowsArgumentException()
        {
            const int stubValue = 11;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue);

            TestDelegate invokeGetStub = () => stub.Get<double>(For.GameId);

            Assert.Throws<ArgumentException>(invokeGetStub);
        }

        [Test]
        public void OutForKey_AddedStubForTheKeyTwice_ReturnsLastStubbedValue()
        {
            const int stubValue1 = 11;
            const int stubValue2 = 111;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue1);
            stub.Value(For.GameId).Is(stubValue2);

            Assert.That(stub.Out<int>(For.GameId), Is.EqualTo(stubValue2));
        }

        [Test]
        public void GetStubForKey_AddedStubForTheKeyTwice_ReturnsLastStubbedValue()
        {
            const int stubValue1 = 11;
            const int stubValue2 = 111;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue1);
            stub.Value(For.GameId).Is(stubValue2);

            Assert.That(stub.Get<int>(For.GameId), Is.EqualTo(stubValue2));
        }

        [Test]
        public void OutForKey1_AddedTwoStubForDifferentKeys_ReturnsStubbedValueForKey1()
        {
            const int stubValue1 = 11;
            const double stubValue2 = 111.0;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue1);
            stub.Value(For.SB).Is(stubValue2);

            Assert.That(stub.Out<int>(For.GameId), Is.EqualTo(stubValue1));
        }

        [Test]
        public void OutForKey2_AddedTwoStubForDifferentKeys_ReturnsStubbedValueForKey2()
        {
            const int stubValue1 = 11;
            const double stubValue2 = 111.0;
            var stub = new StubBuilder();
            stub.Value(For.GameId).Is(stubValue1);
            stub.Value(For.SB).Is(stubValue2);

            Assert.That(stub.Out<double>(For.SB), Is.EqualTo(stubValue2));
        }

    }

    public interface IFoo
    {
        int Id { get; }

        string Name { get; }
    }
}