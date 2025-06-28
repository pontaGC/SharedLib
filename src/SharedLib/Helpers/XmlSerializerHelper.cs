using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SharedLib.Helpers
{
    /// <summary>
    /// Helper to serialize or deserialize the object related to XML document.
    /// </summary>
    public static class XmlSerializerHelper
    {
        #region Fields

        private static readonly IFileSystem fileSystem = FileSystem.Instance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Serializes the source object to a target stream.
        /// </summary>
        /// <typeparam name="T">The type of a source object.</typeparam>
        /// <param name="source">The object to serialize. If <c>source</c> is <c>null</c>, this method does nothing.</param>
        /// <param name="xmlFilePath">The XML document to serialize.</param>
        /// <param name="namespaces">The namespaces for then generated XML document.</param>
        /// <param name="encoding">The character encoding. The parameter is UTF-8 if it is <c>null</c>.</param>
        /// <exception cref="InvalidOperationException">An error occurred during serialization. The original exception is available using the <c>InnerException</c> property.</exception>
        public static void Serialize<T>(T source, string xmlFilePath, XmlSerializerNamespaces? namespaces, Encoding? encoding)
        {
            try
            {
                using (var fileStream = fileSystem.OpenOrCreateFile(xmlFilePath, FileAccess.Write, FileShare.None))
                {
                    using (var streamWriter = new StreamWriter(fileStream, encoding ?? Encoding.UTF8))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        serializer.Serialize(streamWriter, source, namespaces);
                    }
                }
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                if (invalidOperationEx.InnerException is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                // Serialization error
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Serialization error : {xmlFilePath}", ex);
            }
        }

        /// <summary>
        /// Serializes the source object to a target stream.
        /// </summary>
        /// <typeparam name="T">The type of a source object.</typeparam>
        /// <param name="source">The object to serialize. If <c>source</c> is <c>null</c>, this method does nothing.</param>
        /// <param name="targetStream">The target stream to serialize.</param>
        /// <param name="namespaces">The namespaces for then generated XML document.</param>
        /// <param name="encoding">The character encoding. The parameter is UTF-8 if it is <c>null</c>.</param>
        /// <exception cref="InvalidOperationException">An error occurred during serialization. The original exception is available using the <c>InnerException</c> property.</exception>
        public static void Serialize<T>(T source, Stream targetStream, XmlSerializerNamespaces? namespaces, Encoding? encoding)
        {
            var xmlWriterSettings = new XmlWriterSettings()
            {
                Encoding = encoding ?? Encoding.UTF8,
                Indent = true,
                OmitXmlDeclaration = false,
            };

            try
            {
                using (var xmlWriter = XmlWriter.Create(targetStream, xmlWriterSettings))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(xmlWriter, source, namespaces);
                }
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                if (invalidOperationEx.InnerException is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                // Serialization error
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Try serializing a target object to a target XML file.
        /// </summary>
        /// <typeparam name="T">The type of a source object.</typeparam>
        /// <param name="source">The object to serialize. If <c>source</c> is <c>null</c>, this method does nothing.</param>
        /// <param name="xmlFilePath">The XML document to serialize.</param>
        /// <param name="namespaces">The namespaces for then generated XML document.</param>
        /// <param name="encoding">The character encoding. The parameter is UTF-8 if it is <c>null</c>.</param>
        /// <returns><c>true</c>, if the serialization is success, Otherwise; <c>false</c>.</returns>
        public static bool TrySerialize<T>(T source, string xmlFilePath, XmlSerializerNamespaces? namespaces, Encoding? encoding)
        {
            try
            {
                Serialize(source, xmlFilePath, namespaces, encoding);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Try serializing the source object to a target stream.
        /// </summary>
        /// <typeparam name="T">The type of a source object.</typeparam>
        /// <param name="source">The object to serialize. If <c>source</c> is <c>null</c>, this method does nothing.</param>
        /// <param name="targetStream">The target stream to serialize.</param>
        /// <param name="namespaces">The namespaces for then generated XML document.</param>
        /// <param name="encoding">The character encoding. The parameter is UTF-8 if it is <c>null</c>.</param>
        /// <returns><c>true</c>, if the serialization is success, Otherwise; <c>false</c>.</returns>
        public static bool TrySerialize<T>(T source, Stream targetStream, XmlSerializerNamespaces? namespaces, Encoding? encoding)
        {
            try
            {
                Serialize(source, targetStream, namespaces, encoding);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="xmlFilePath">The XML document to deserialize.</param>
        /// <returns>The deserialized result.</returns>
        /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the <c>InnerException</c> property.</exception>
        public static T Deserialize<T>(string xmlFilePath)
        {
            var serializer = new XmlSerializer(typeof(T));
            try
            {
                using (var fileStream = fileSystem.OpenReadFile(xmlFilePath))
                {
                    using (var xmlReader = XmlReader.Create(fileStream))
                    {
                        return (T)serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (InvalidOperationException invalidOperationEx)
            {
                if (invalidOperationEx.InnerException is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                // Deserialization error
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Deserialization error : {xmlFilePath}", ex);
            }
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="xmlStream">The XML document stream to deserialize.</param>
        /// <returns>The deserialized result.</returns>
        /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the <c>InnerException</c> property.</exception>
        public static T Deserialize<T>(Stream xmlStream)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(xmlStream);
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="textReader">The XML document reader.</param>
        /// <returns>The deserialized result.</returns>
        /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the <c>InnerException</c> property.</exception>
        public static T Deserialize<T>(TextReader textReader)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(textReader);
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="xmlFilePath">The XML document to deserialize.</param>
        /// <param name="deserializedObject">The deserialized object which type is <c>T</c>.</param>
        /// <returns><c>true</c>, if the deserialization is success, Otherwise; <c>false</c>.</returns>        
        public static bool TryDeserialize<T>(string xmlFilePath, out T deserializedObject)
        {
            deserializedObject = default;
            try
            {
                deserializedObject = Deserialize<T>(xmlFilePath);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="xmlStream">The XML document stream to deserialize.</param>
        /// <param name="deserializedObject">The deserialized object which type is <c>T</c>.</param>
        /// <returns><c>true</c>, if the deserialization is success, Otherwise; <c>false</c>.</returns>
        public static bool TryDeserialize<T>(Stream xmlStream, out T deserializedObject)
        {
            deserializedObject = default;
            try
            {
                deserializedObject = Deserialize<T>(xmlStream);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes the specified XML document to create the target object.
        /// </summary>
        /// <typeparam name="T">The type of a target object deserialized.</typeparam>
        /// <param name="textReader">The XML document reader.</param>
        /// <param name="deserializedObject">The deserialized object which type is <c>T</c>.</param>
        /// <returns><c>true</c>, if the deserialization is success, Otherwise; <c>false</c>.</returns>
        public static bool TryDeserialize<T>(TextReader textReader, out T deserializedObject)
        {
            deserializedObject = default;
            try
            {
                deserializedObject = Deserialize<T>(textReader);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}

