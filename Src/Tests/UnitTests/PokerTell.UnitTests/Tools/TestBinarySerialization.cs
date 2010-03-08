namespace PokerTell.UnitTests.Tools
{
    using global::Tools.Serialization;

    public static class TestBinarySerialization
    {
        public static TType BinaryDeserializedInMemory<TType>(this TType originalObject) where TType : class
        {
            return SerializeAndDeserializeInMemory(originalObject);
        }

        static TType SerializeAndDeserializeInMemory<TType>(
            TType originalObject) where TType : class
        {
            byte[] data = BinarySerializer.Serialize(originalObject);

            return (TType)BinarySerializer.Deserialize(data);
        }
    }
}