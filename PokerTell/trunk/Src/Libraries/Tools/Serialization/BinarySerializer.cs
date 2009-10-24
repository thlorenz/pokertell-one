namespace Tools.Serialization
{
    using System;
    using System.IO;
    using System.Reflection;
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
        /// <param name="FileName">Target Binary File</param>
        public static void SerializeObjectGraph(object objGraph, string FileName)
        {
            BinaryFormatter BinaryFormat;
            FileStream fs = null;
            try
            {
                BinaryFormat = new BinaryFormatter();
                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);

                BinaryFormat.Serialize(fs, objGraph);

            } catch (Exception excep)
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
        /// <param name="FileName">Binary File</param>
        /// <param name="T">Type of the Object Graph - needed to create a default instance in case deserialing is unsuccessful</param>
        /// <returns>An object graph created either from the Binary file or the default instance</returns>
        public static object DeserializeObjectGraph(string FileName, Type T)
        {
            object objGraph;
            try
            {
                objGraph = Deserialize(FileName);
                return objGraph;
            } catch (Exception excep)
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
        /// <param name="FileName">Binary File</param>
        /// <returns>Objectgraph</returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        private static object Deserialize(string FileName)
        {
            BinaryFormatter binaryFormat;
            FileStream fs = null;
            object objGraph = null;

            try
            {
                binaryFormat = new BinaryFormatter();
                fs = File.OpenRead(FileName);

                objGraph = binaryFormat.Deserialize(fs);
            } catch (Exception excep)
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

            return objGraph;
        }

        #endregion
    }
}