namespace Tools.Serialization
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Runtime.Serialization.Formatters;
    using System.Runtime.Serialization.Formatters.Binary;

    using log4net;

    /// <summary>
    /// Contains static methods to serialize and deserialize an ObjectGraph using the Binary Format.
    /// </summary>
    public static class BinarySerializer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region SerializeObjectGraph

        /// <summary>
        /// Serialize and object graph to a Binary file.
        /// </summary>
        /// <param name="objGraph">Object Graph</param>
        /// <param name="fileName">Target Binary File</param>
        public static void SerializeObjectGraph(object objGraph, string fileName)
        {
            BinaryFormatter binaryFormat;
            FileStream fs = null;
            try
            {
                binaryFormat = new BinaryFormatter();
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

                binaryFormat.Serialize(fs, objGraph);
            }
            catch (Exception excep)
            {
                Log.Debug(excep.ToString());
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        #endregion

        #region DeserializeObjectGraph overloaded

        /// <summary>
        /// Deserializes a Binary file to an object graph.
        /// Will catch any exceptions, inform the user and then return a default object Graph
        /// </summary>
        /// <param name="fileName">Binary File</param>
        /// <param name="T">Type of the Object Graph - needed to create a default instance in case deserialing is unsuccessful</param>
        /// <returns>An object graph created either from the Binary file or the default instance</returns>
        public static object DeserializeObjectGraph(string fileName, Type T)
        {
            try
            {
                return Deserialize(fileName);
            }
            catch (Exception excep)
            {
                Log.Debug(excep.ToString());
                throw;
            }
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
        public static object DeserializeObjectGraph(string fileName)
        {
            return Deserialize(fileName);
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserializes a Binary file to an object graph.
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
        /// <param name="fileName">Binary File</param>
        /// <returns>Objectgraph</returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        private static object Deserialize(string fileName)
        {
            object objGraph = null;

            using (FileStream fileStream = File.OpenRead(fileName))
            {
                try
                {
                    var binaryFormatter = new BinaryFormatter
                        {
                            AssemblyFormat = FormatterAssemblyStyle.Simple
                        };

                    objGraph = binaryFormatter.Deserialize(fileStream);
                }
                catch
                    (Exception
                        excep)
                {
                    Log.Debug(excep.ToString());
                    throw;
                }
            }

            return objGraph;
        }

        #endregion
    }
}