namespace PokerTell.UnitTests.Tools
{
    using System;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public static class TestXmlSerialization
    {
        public static TType XmlDeserializedInMemory<TType>(this TType originalObject, bool writeXmlStringToConsole) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, writeXmlStringToConsole);
        }

        public static TType XmlDeserializedInMemory<TType>(this TType originalObject) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject, false);
        }

        private static TType SerializeAndDeserializeInMemory<TType>(TType originalObject, bool writeXmlStringToConsole) where TType : new()
        {
            var memoryStream = new MemoryStream();

            new XmlSerializer(typeof(TType)).Serialize(memoryStream, originalObject);

            string xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

            if (writeXmlStringToConsole)
            {
                Console.WriteLine(xmlizedString);
            }

            memoryStream = new MemoryStream(StringToUTF8ByteArray(xmlizedString));

            return (TType)new XmlSerializer(typeof(TType)).Deserialize(memoryStream);
        }

        private static string UTF8ByteArrayToString(byte[] byteArray)
        {
            var encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(byteArray);
            return constructedString;
        }

        private static byte[] StringToUTF8ByteArray(string xmlString)
        {
            var encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(xmlString);
            return byteArray;
        } 
    }
}