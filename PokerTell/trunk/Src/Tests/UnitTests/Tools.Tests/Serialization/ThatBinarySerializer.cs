namespace Tools.Tests.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    using global::Tools.Serialization;

    using NUnit.Framework;

    using PokerTell.UnitTests.Tools;

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
        public void Deserializing_SerializedInAnotherNamespace_ThrowsInvalidCastException()
        {
            // Because if we try we get this (Binary is not very flexible)
            // System.InvalidCastException : Unable to cast object of type 'PokerTell.Admin.InfoToSaveClass'
            // to type 'OtherNameSpace.InfoToSaveClass'.
            byte[] data = TestBinarySerialization.SerializeObjectGraph(_lstInfoToSave);

            object target;
            Assert.Throws<InvalidCastException>(
                () => target =
                      (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                      TestBinarySerialization.Deserialize(data));
        }

        [Test]
        public void Deserializing_SerializedInAnotherNamespaceBinderSet_ReturnsSameObject()
        {
            var binaryFormatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple,
                    Binder = new NameSpaceBinder(
                        "Tools.Tests.Serialization",
                        "PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace")
                };

            var loadInfo =
                (PokerTell.Infrastructure.Tests.Serialization.OtherNameSpace.InfoToSaveClass)
                binaryFormatter.Deserialize(File.OpenRead("SaveInOneNamespaceLoadInAnother.dat"));

            Console.WriteLine(loadInfo.theString);
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
            Assert.Throws<FileNotFoundException>(() =>
                BinarySerializer.DeserializeObjectGraph("DoesntExist.dat", typeof(InfoToSaveClass)));
        }

        [Test]
        public void Method_State_Returns()
        {
            var binSerializer = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Full
                };
            binSerializer.Serialize(new FileStream(@"c:\binn.dat", FileMode.OpenOrCreate), _lstInfoToSave);
        }
    }

        #endregion  

    public class NameSpaceBinder : System.Runtime.Serialization.SerializationBinder
    {
        readonly string _originalNameSpace;

        readonly string _currentNameSpace;

        public NameSpaceBinder(string originalNameSpace, string currentNameSpace)
        {
            _originalNameSpace = originalNameSpace;
            _currentNameSpace = currentNameSpace;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (typeName.Contains(_originalNameSpace))
            {
                typeName = typeName.Replace(_originalNameSpace, _currentNameSpace);
            }

            return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));
        }
    }
}