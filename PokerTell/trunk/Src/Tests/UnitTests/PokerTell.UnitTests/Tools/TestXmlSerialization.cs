namespace PokerTell.UnitTests.Tools
{
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;

    public static class TestXmlSerialization
    {
        public static TType DeserializedInMemory<TType>(this TType originalObject) where TType : new()
        {
            return SerializeAndDeserializeInMemory(originalObject);
        }

        private static TType SerializeAndDeserializeInMemory<TType>(TType originalObject) where TType : new()
        {
            var memoryStream = new MemoryStream();

            new XmlSerializer(typeof(TType)).Serialize(memoryStream, originalObject);

            string xmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());

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