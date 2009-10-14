namespace Tools.Tests.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    using NUnit.Framework;

    using Tools.Serialization;

    /// <summary>
    /// Description of PokerRoomsSaver_Test.
    /// </summary>
    [TestFixture]
    public class ThatSerializers
    {
        #region Constants and Fields

        InfoToSaveClass _infoSaved;

        List<InfoToSaveClass> _lstInfoToSave;

        #endregion

        #region Public Methods

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void Binary_CannotSerializeInOneNamespaceAndDeserializeInAnother()
        {
            // Because if we try we get this (Binary is not very flexible)
            // System.InvalidCastException : Unable to cast object of type 'PokerTell.Admin.InfoToSaveClass'
            // to type 'OtherNameSpace.InfoToSaveClass'.
            BinarySerializer.SerializeObjectGraph(_infoSaved, "SaveInOneNamespaceLoadInAnother.dat");

            var loadInfo =
                (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                BinarySerializer.DeserializeObjectGraph("SaveInOneNamespaceLoadInAnother.dat");
        }

        [Test]
        public void Binary_CanSerializeAndDeserialize()
        {
            BinarySerializer.SerializeObjectGraph(_infoSaved, "SaveAndLoad.dat");

            var loadInfo = (InfoToSaveClass)BinarySerializer.DeserializeObjectGraph("SaveAndLoad.dat");
        }

        [Test]
        public void Binary_CanSerializeAndDeserializeAGenericList()
        {
            BinarySerializer.SerializeObjectGraph(_lstInfoToSave, "SaveAndLoad.xml");

            var lstLoad =
                (List<InfoToSaveClass>)
                BinarySerializer.DeserializeObjectGraph("SaveAndLoad.xml", typeof(List<InfoToSaveClass>));

            for (int i = 0; i < _lstInfoToSave.Count; i++)
            {
                Assert.That(lstLoad[i].Equals(_lstInfoToSave[i]));
            }
        }

        [Test]
        public void Binary_SerializingFromNonExistingFileReturnsDefaultObject()
        {
            try
            {
                var infLoad =
                    (InfoToSaveClass)BinarySerializer.DeserializeObjectGraph("DoesntExist.dat", typeof(InfoToSaveClass));
            }
            catch (FileNotFoundException)
            {
                Assert.Pass();
            }

            Assert.Fail("Should have thrown FileNotFoundException");
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            _infoSaved = null;
            _lstInfoToSave = null;
        }

        [TestFixtureSetUp]
        public void Init()
        {
            _infoSaved = new InfoToSaveClass("TestString", 9);
            _lstInfoToSave = new List<InfoToSaveClass>();
            for (int i = 0; i < 5; i++)
            {
                _lstInfoToSave.Add(_infoSaved);
            }
        }

        [Test]
        public void Soap_CannnotSerializeAGenericList()
        {
            // So we got this:
            // System.Runtime.Serialization.SerializationException:
            // Soap Serializer does not support serializing Generic Types
            // which will cause: System.Xml.XmlException
            try
            {
                SoapSerializer.SerializeObjectGraph(_lstInfoToSave, "SaveAndLoad.xml");
            }
            catch (SerializationException)
            {
                Assert.Pass();
            }

            Assert.Fail("Should have thrown SerializationException");
        }

        [Test]
        [ExpectedException(typeof(InvalidCastException))]
        public void Soap_CannotSerializeInOneNamespaceAndDeserializeInAnother()
        {
            // Because if we try we get this (Soap is not very flexible)
            // System.InvalidCastException : Unable to cast object of type 'PokerTell.Admin.InfoToSaveClass'
            // to type 'OtherNameSpace.InfoToSaveClass'.
            SoapSerializer.SerializeObjectGraph(_infoSaved, "SaveInOneNamespaceLoadInAnother.soap");

            var loadInfo =
                (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                SoapSerializer.DeserializeObjectGraph("SaveInOneNamespaceLoadInAnother.soap");
        }

        [Test]
        public void Soap_CanSerializeAndDeserialize()
        {
            SoapSerializer.SerializeObjectGraph(_infoSaved, "SaveAndLoad.soap");

            var loadInfo = (InfoToSaveClass)SoapSerializer.DeserializeObjectGraph("SaveAndLoad.soap");

            Assert.That(loadInfo.Equals(_infoSaved));
        }

        [Test]
        public void Soap_SerializingFromNonExistingFileThrowsFileNotFoundException()
        {
            try
            {
                var infLoad =
                    (InfoToSaveClass)SoapSerializer.DeserializeObjectGraph("DoesntExist.soap", typeof(InfoToSaveClass));
            }
            catch (FileNotFoundException)
            {
                Assert.Pass();
            }

            Assert.Fail("Should have thrown FileNotFoundException");
        }

        [Test]
        public void Xml_CanSerializeAndDeserialize()
        {
            XmlStandardSerializer.SerializeObjectGraph(_infoSaved, "SaveAndLoad.xml");

            var loadInfo =
                (InfoToSaveClass)
                XmlStandardSerializer.DeserializeObjectGraph("SaveAndLoad.xml", typeof(InfoToSaveClass));

            Assert.That(loadInfo, Is.EqualTo(_infoSaved));
        }

        [Test]
        public void Xml_CanSerializeAndDeserializeAGenericList()
        {
            XmlStandardSerializer.SerializeObjectGraph(_lstInfoToSave, "SaveAndLoad.xml");

            var lstLoad =
                (List<InfoToSaveClass>)
                XmlStandardSerializer.DeserializeObjectGraph("SaveAndLoad.xml", typeof(List<InfoToSaveClass>));

            for (int i = 0; i < _lstInfoToSave.Count; i++)
            {
                Assert.That(lstLoad[i].Equals(_lstInfoToSave[i]));
            }
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
            var infDefault = new InfoToSaveClass();
            try
            {
                var infLoad =
                    (InfoToSaveClass)
                    XmlStandardSerializer.DeserializeObjectGraph("DoesntExist.xml", typeof(InfoToSaveClass));
            }
            catch (FileNotFoundException)
            {
                Assert.Pass();
            }

            Assert.Fail("Should have thrown FileNotFoundException");
        }

        #endregion
    }
}