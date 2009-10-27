namespace Tools.Serialization
{
    using System.IO;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    /// <summary>
    /// Contains static methods to serialize and deserialize an ObjectGraph using the Binary Format.
    /// Do not use automatic properties in serializable types.
    /// See why:
    /// http://blogs.infragistics.com/blogs/josh_smith/archive/2008/02/05/automatic-properties-and-the-binaryformatter.aspx
    /// </summary>
    public static class BinarySerializer
    {
        #region Public Methods

        /// <summary>
        /// Deserializes and objectGraph from the given data.
        /// </summary>
        /// <param name="data">Data of previously serialized objectgraph.</param>
        /// <param name="binder">Binder used to port objects from one namespace to another</param>
        /// <returns>Created Objectgraph</returns>
        public static object Deserialize(byte[] data, SerializationBinder binder)
        {
            var memoryStream = new MemoryStream(data);

            return Deserialize(memoryStream, binder);
        }

        /// <summary>
        /// Deserializes and objectGraph from the given data.
        /// </summary>
        /// <param name="data">Data of previously serialized objectgraph.</param>
        /// <returns>Created Objectgraph</returns>
        public static object Deserialize(byte[] data)
        {
            return Deserialize(data, null);
        }

        /// <summary>
        /// Deserializes a Binary file to an object graph.
        /// It will not handle any of the below exceptions, so they need
        /// to be handled whenever calling this function
        /// </summary>
        /// <param name="fileName">Binary File</param>
        /// <returns>An object graph created from the Binary file or null if unsuccessful</returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        public static object Deserialize(string fileName)
        {
            return Deserialize(fileName, null);
        }

        /// <summary>
        /// Deserializes a Binary file to an object graph.
        /// It will not handle any of the below exceptions, so they need
        /// to be handled whenever calling this function
        /// </summary>
        /// <param name="fileName">Binary File</param>
        /// <param name="binder">Binder used to port objects from one namespace to another</param>
        /// <returns>An object graph created from the Binary file or null if unsuccessful</returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        public static object Deserialize(string fileName, SerializationBinder binder)
        {
            using (FileStream fileStream = File.OpenRead(fileName))
            {
                return Deserialize(fileStream, binder);
            }
        }

        /// <summary>
        /// Serializes ObjectGraph to Memory
        /// </summary>
        /// <param name="objGraph"></param>
        /// <returns>ByteArray of resulting data</returns>
        public static byte[] Serialize(object objGraph)
        {
            var memoryStream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(memoryStream, objGraph);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Serialize an object graph to a Binary file.
        /// </summary>
        /// <param name="objGraph">Object Graph</param>
        /// <param name="fileName">Target Binary File</param>
        public static void Serialize(object objGraph, string fileName)
        {
            var binaryFormatter = new BinaryFormatter();

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binaryFormatter.Serialize(fileStream, objGraph);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes a given stream into an object, optionally using a binder.
        /// </summary>
        /// <remarks>
        /// Source: http://www.diranieh.com/NETSerialization/BinarySerialization.htm
        /// Deserializing
        /// When formatters deserialize an object, they first get the assembly identity and ensure that the assembly is loaded into the executing AppDomain. How this assembly is loaded depends on the value of the formatter's AssemblyFormat:
        ///
        ///    * FormatterAssemblyStyle.Full
        ///      The assembly is loaded using System.Reflection.Assembly.Load which first looks in the GAC and then looks in the application's directory. If the assembly is not found, an exception is thrown and deserialization fails.
        ///    * FormatterAssemblyStyle.Simple
        ///      The assembly is loaded using System.Reflection.Assembly.LoadWithPartialName which first looks in the application's directory, and if not found, looks in the GAC for the highest version numbered assembly with the same file name.
        /// </remarks>
        /// <param name="stream">The source stream</param>
        /// <param name="binder">If given a binder it will be used. Set to null will ignore the binder.</param>
        /// <returns>Deserialized Object</returns>
        static object Deserialize(Stream stream, SerializationBinder binder)
        {
            var binaryFormatter = new BinaryFormatter
                {
                    AssemblyFormat = FormatterAssemblyStyle.Simple, 
                };

            if (binder != null)
            {
                binaryFormatter.Binder = binder;
            }

            return binaryFormatter.Deserialize(stream);
        }

        #endregion
    }
}