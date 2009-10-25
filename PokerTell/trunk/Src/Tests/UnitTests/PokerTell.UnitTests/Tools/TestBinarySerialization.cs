namespace PokerTell.UnitTests.Tools
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    public static class TestBinarySerialization
    {
        #region Public Methods

        public static TType BinaryDeserializedInMemory<TType>(this TType originalObject) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, false, null);
        }

        public static TType BinaryDeserializedInMemory<TType>(
            this TType originalObject, bool writeBinaryStringToConsole) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, writeBinaryStringToConsole, null);
        }

        public static TType BinaryDeserializedInMemory<TType>(
            this TType originalObject, bool writeBinaryStringToConsole, SerializationBinder binder) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, writeBinaryStringToConsole, binder);
        }

        public static TType BinaryDeserializedInMemory<TType>(this TType originalObject, SerializationBinder binder)
            where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, false, binder);
        }

        public static object Deserialize(byte[] data, SerializationBinder binder)
        {
            var streamMemory = new MemoryStream(data);
            var formatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple, 
                    Binder = binder
                };

            return formatter.Deserialize(streamMemory);
        }

        public static object Deserialize(byte[] data)
        {
            var streamMemory = new MemoryStream(data);
            var binaryFormatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple, 
                };

            return binaryFormatter.Deserialize(streamMemory);
        }

        public static byte[] SerializeObjectGraph(object objGraph)
        {
            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, objGraph);
            return memoryStream.ToArray();
        }

        #endregion

        #region Methods

        static TType SerializeAndDeserializeInMemory<TType>(
            TType originalObject, bool writeBinaryStringToConsole, SerializationBinder binder) where TType : new()
        {
            byte[] data = SerializeObjectGraph(originalObject);

            if (writeBinaryStringToConsole)
            {
                byte[] utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, data);
                var utf8Chars = new char[Encoding.UTF8.GetCharCount(utf8Bytes, 0, utf8Bytes.Length)];
                Encoding.UTF8.GetChars(utf8Bytes, 0, utf8Bytes.Length, utf8Chars, 0);

                Console.WriteLine(new string(utf8Chars));
            }

            if (binder != null)
            {
                return (TType)Deserialize(data, binder);
            }

            return (TType)Deserialize(data);
        }

        #endregion
    }
}