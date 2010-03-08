namespace Tools.Tests.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using NUnit.Framework;

    using PokerTell.UnitTests.Tools;

    using Tools.Serialization;

    /// <summary>
    /// Description of PokerRoomsSaver_Test.
    /// </summary>
    [TestFixture]
    public class ThatBinarySerializer
    {
        #region Constants and Fields

        InfoToSaveClass _infoSaved;

        List<InfoToSaveClass> _lstInfoToSave;

        #endregion

        #region Public Methods

        [TestFixtureSetUp]
        public void _Init()
        {
            _infoSaved = new InfoToSaveClass("TestString", 9);
            _lstInfoToSave = new List<InfoToSaveClass>();
            for (int i = 0; i < 5; i++)
            {
                _lstInfoToSave.Add(_infoSaved);
            }
        }

        [Test]
        public void Binary_CanSerializeAndDeserialize()
        {
            Assert.That(_infoSaved.BinaryDeserializedInMemory(), Is.EqualTo(_infoSaved));
        }

        [Test]
        public void Binary_CanSerializeAndDeserializeAGenericList()
        {
            Assert.That(_lstInfoToSave.BinaryDeserializedInMemory(), Is.EqualTo(_lstInfoToSave));
        }

        [Test]
        public void Binary_SerializingFromNonExistingFileReturnsDefaultObject()
        {
            Assert.Throws<FileNotFoundException>(
                () =>
                BinarySerializer.Deserialize("DoesntExist.dat"));
        }

        [Test]
        public void Deserialize_SerializedInAnotherNamespaceWithCustomBinder_ReturnsSameObject()
        {
            var binder = new NameSpaceBinder(
                "Tools.Tests.Serialization", 
                "PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace");

            byte[] data = BinarySerializer.Serialize(_infoSaved);

            var loadInfo =
                (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                BinarySerializer.Deserialize(data, binder);

            Assert.That(loadInfo.theString, Is.EqualTo(_infoSaved.theString));
        }

        [Test]
        public void Deserialize_SerializedInAnotherNamespaceWithoutCustomBincer_ThrowsInvalidCastException()
        {
            // (Binary is not very flexible unless a binder is used  --> see next test)
            byte[] data = BinarySerializer.Serialize(_infoSaved);

            object target;
            Assert.Throws<InvalidCastException>(
                () => target =
                      (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                      BinarySerializer.Deserialize(data));
        }

        #endregion
    }
}