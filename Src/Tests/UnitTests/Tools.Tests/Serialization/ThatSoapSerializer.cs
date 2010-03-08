namespace Tools.Tests.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization;

    using NUnit.Framework;

    using Tools.Serialization;

    public class ThatSoapSerializer
    {
        #region Constants and Fields

        InfoToSaveClass _infoSaved;

        List<InfoToSaveClass> _lstInfoToSave;

        #endregion

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
                SoapSerializer.DeserializeObjectGraph("DoesntExist.soap", typeof(InfoToSaveClass));
            }
            catch (FileNotFoundException)
            {
                Assert.Pass();
            }

            Assert.Fail("Should have thrown FileNotFoundException");
        }
    }
}