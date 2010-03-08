namespace Tools.Tests.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    using NUnit.Framework;

    using PokerTell.UnitTests.Tools;

    using Tools.Serialization;

    /// <summary>
    /// Description of PokerRoomsSaver_Test.
    /// </summary>
    [TestFixture]
    public class ThatXmlSerializer
    {
        #region Constants and Fields

        InfoToSaveClass _infoSaved;

        List<InfoToSaveClass> _lstInfoToSave;

        #endregion

        #region Public Methods

        [TestFixtureTearDown]
        public void _CleanUp()
        {
            _infoSaved = null;
            _lstInfoToSave = null;
        }

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
        public void Xml_CanSerializeAndDeserialize()
        {
            Assert.That(_infoSaved.XmlDeserializedInMemory(), Is.EqualTo(_infoSaved));
        }

        [Test]
        public void Xml_CanSerializeAndDeserializeAGenericList()
        {
            Assert.That(_lstInfoToSave.XmlDeserializedInMemory(), Is.EqualTo(_lstInfoToSave));
        }

        [Test]
        public void Xml_CanSerializeInOneNamespaceAndDeserializeInAnother()
        {
            XmlStandardSerializer.SerializeObjectGraph(_infoSaved, "SaveInOneNamespaceLoadInAnother.xml");

            var loadInfo =
                (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                XmlStandardSerializer.DeserializeObjectGraph(
                    "SaveInOneNamespaceLoadInAnother.xml", 
                    typeof(PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass));

            Assert.That(loadInfo.ToString(), Is.EqualTo(_infoSaved.ToString()));
        }

        [Test]
        public void Xml_SerializingFromNonExistingFileThrowsFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(
                () => XmlStandardSerializer.DeserializeObjectGraph("DoesntExist.xml", typeof(InfoToSaveClass)));
        }

        #endregion
    }
}