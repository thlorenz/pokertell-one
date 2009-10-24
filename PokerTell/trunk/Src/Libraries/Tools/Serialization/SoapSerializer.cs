#region Using Directives

using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

using log4net;

#endregion

namespace Tools.Serialization
{
    /// <summary>
    /// Contains static methods to serialize and deserialize an ObjectGraph using the Soap Format.
    /// </summary>
    public static class SoapSerializer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region SerializeObjectGraph

        /// <summary>
        /// Serialize and object graph to a soap file.
        /// </summary>
        /// <param name="objGraph">Object Graph</param>
        /// <param name="FileName">Target Soap File</param>
        public static void SerializeObjectGraph(object objGraph, string FileName)
        {
            SoapFormatter soapFormat;
            FileStream fs = null;
            try
            {
                soapFormat = new SoapFormatter();
                fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);

                soapFormat.Serialize(fs, objGraph);
            } catch (Exception excep)
            {
                Log.Error("Unexpected", excep);
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
        /// Deserializes a Soap file to an object graph.
        /// Will catch any exceptions, inform the user and then return a default onject Graph
        /// </summary>
        /// <param name="fileName">Soap File</param>
        /// <param name="T">Type of the Object Graph - needed to create a default instance in case deserialing is unsuccessful</param>
        /// <returns>An object graph created either from the Soap file or the default instance</returns>
        public static object DeserializeObjectGraph(string fileName, Type T)
        {
            object objGraph;
            try
            {
                objGraph = Deserialize(fileName);
                return objGraph;
            } catch (Exception excep)
            {
                Log.Debug(excep.ToString());
                throw;
            }
        }

        /// <summary>
        /// Deserializes a Soap file to an object graph.
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
        /// Deserializes a Soap file to an object graph.
        /// </summary>
        /// <param name="FileName">Soap File</param>
        /// <returns>Objectgraph</returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.DriveNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.IO.FileNotFoundException">Problem locating Serialization File</exception>
        /// <exception cref="System.UnauthorizedAccessException ">Problem accessing Serialization File</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">Serialization file is found but invalid</exception>
        private static object Deserialize(string FileName)
        {
            SoapFormatter soapFormat;
            FileStream fs = null;
            object objGraph = null;
            try
            {
                soapFormat = new SoapFormatter();
                fs = File.OpenRead(FileName);

                objGraph = soapFormat.Deserialize(fs);
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