namespace Tools.Serialization
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using log4net;

    /// <summary>
    /// Contains static methods to serialize and deserialize an ObjectGraph using the xml Format.
    /// </summary>
    public static class XmlStandardSerializer
    {
        #region Constants and Fields

        static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes an xml file to an object
        /// Since we can specify the type, it is much more flexible than soap and binary Serializers
        /// It allows us to change the objects and their namespaces, while still retaining compatiblity
        /// to xml files serialized from older versions of these objects.
        /// Fields not found in the file will be determined by the default constructor.
        /// Fields not contained in the newer object, but found in the xml file will be ignored.
        /// So as long of the name of the object is retained it will be compatible.
        /// Therefore this should be the serializer of choice unless crossplatform and cross web 
        /// compatibility is desired (use soap) or space is an issue (use binary)
        /// Of course we need to make sure, that the default constructors create a safe object
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="theTypeOfObject">Type of the object</param>
        /// <returns></returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        public static object DeserializeObjectGraph(string fileName, Type theTypeOfObject)
        {
            try
            {
                using (FileStream fileStream = File.OpenRead(fileName))
                {
                    return DeserializeObjectGraph(fileStream, theTypeOfObject);
                }
            }
            catch (Exception excep)
            {
                Log.Debug(excep.ToString());
                throw;
            }
        }

        public static object DeserializeObjectGraph(Stream stream, Type theTypeOfObject)
        {
            var xmlSerializer = new XmlSerializer(theTypeOfObject);

            return xmlSerializer.Deserialize(stream);
        }

        /// <summary>
        /// Serialize and object graph to a Binary file.
        /// </summary>
        /// <param name="objGraph">Object Graph</param>
        /// <param name="fileName">Target Binary File</param>
        public static void SerializeObjectGraph(object objGraph, string fileName)
        {
            try
            {
                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    SerializeObjectGraph(objGraph, fileStream);
                }
            }
            catch (Exception excep)
            {
                Log.Debug(excep.ToString());
                throw;
            }
        }

        public static void SerializeObjectGraph(object objGraph, Stream stream)
        {
            var xmlSerializer = new XmlSerializer(objGraph.GetType());
            
            xmlSerializer.Serialize(stream, objGraph);
        }

        #endregion
    }
}