using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace SharedLib.Helpers
{
    /// <summary>
    /// Helper for JSON serialization using <see cref="JsonSerializer" />.
    /// </summary>
    public static class JsonSerializerHelper
    {
        /// <summary>
        /// Converts the value of a type specified by a generic type parameter into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">Options to control serialization behavior.</param>        
        /// <returns>A JSON string representation of the value.</returns>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        public static string Serialize<T>(T value, JsonSerializerOptions? options)
        {
            try
            {
                return JsonSerializer.Serialize(value, options);
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Serialization error.", unexpectedEx);
            }
        }

        /// <summary>
        /// Converts the provided value to UTF-8 encoded JSON text and write it to the Stream.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="utf8Json">The UTF-8 Stream to write to.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="options">Options to control the conversion behavior.</param>
        /// <exception cref="ArgumentNullException"><paramref name="utf8Json"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        public static void Serialize<T>(Stream utf8Json, T value, JsonSerializerOptions? options)
        {
            ArgumentNullException.ThrowIfNull(utf8Json);

            try
            {
                JsonSerializer.Serialize(utf8Json, value, options);
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Serialization error.", unexpectedEx);
            }
        }

        /// <summary>
        /// Converts the provided value to UTF-8 encoded JSON text and write it to the Stream.
        /// </summary>
        /// <typeparam name="T">The type of the value to serialize.</typeparam>
        /// <param name="jsonWriter">The UTF-8 Stream to write to.</param>
        /// <param name="value">The value to convert.</param>
        /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
        /// <exception cref="ArgumentNullException"><paramref name="jsonWriter"/> or <paramref name="jsonTypeInfo"/> is <c>null</c>.</exception>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        public static void Serialize<T>(Utf8JsonWriter jsonWriter, T value, JsonTypeInfo<T> jsonTypeInfo)
        {
            ArgumentNullException.ThrowIfNull(jsonWriter);
            ArgumentNullException.ThrowIfNull(jsonTypeInfo);

            try
            {
                JsonSerializer.Serialize(jsonWriter, value, jsonTypeInfo);
            }
            catch (NotSupportedException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Serialization error.", unexpectedEx);
            }
        }

        /// <summary>
        /// Parses the text representing a single JSON value into an instance of the type specified by a generic type parameter.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
        /// <param name="json">The JSON text to parse.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <exception cref="ArgumentNullException"><paramref name="json"/> is <c>null</c>.</exception>
        /// <exception cref="JsonException">The JSON is invalid, or <c>T</c> is not compatible with the JSON, or  There is remaining data in the string beyond a single JSON value.</exception>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        /// <returns>A <c>T</c> representation of the JSON value.</returns>
        public static T? Deserialize<T>(string json, JsonSerializerOptions? options)
        {
            ArgumentNullException.ThrowIfNull(json);

            try
            {
                return JsonSerializer.Deserialize<T>(json, options);
            }
            catch (Exception knownEx) when (knownEx is NotSupportedException || knownEx is JsonException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Deserialization error.", unexpectedEx);
            }
        }

        /// <summary>
        /// Reads the UTF-8 encoded text representing a single JSON value into a TValue. The Stream will be read to completion.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
        /// <param name="utf8Json">JSON data to parse.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <returns>A <c>T</c> representation of the JSON value.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="utf8Json"/> is <c>null</c>.</exception>
        /// <exception cref="JsonException">The JSON is invalid, <c>T</c> is not compatible with the JSON, or there is remaining data in the Stream.</exception>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        public static T? Deserialize<T>(Stream utf8Json, JsonSerializerOptions? options)
        {
            ArgumentNullException.ThrowIfNull(utf8Json);

            try
            {
                return JsonSerializer.Deserialize<T>(utf8Json, options);
            }
            catch (Exception expectableEx) when (expectableEx is NotSupportedException || expectableEx is JsonException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Deserialization error.", unexpectedEx);
            }
        }

        /// <summary>
        /// Reads one JSON value (including objects or arrays) from the provided reader into a <c>T</c>.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the JSON value into.</typeparam>
        /// <param name="jsonReader">The reader to read.</param>
        /// <param name="jsonTypeInfo">Metadata about the type to convert.</param>
        /// <returns>A <c>T</c> representation of the JSON value.</returns>
        /// <exception cref="ArgumentException"><paramref name="jsonReader"/> is using unsupported options.</exception>
        /// <exception cref="JsonException">The JSON is invalid, <c>T</c> is not compatible with the JSON, or a value could not be read from the reader.</exception>
        /// <exception cref="NotSupportedException">There is no compatible <see cref="JsonConverter" /> for <c>T</c> or its serializable members.</exception>"
        /// <exception cref="InvalidOperationException">The unexepcted error occurs. The original exception is available using the <c>InnerException</c> property.</exception>
        public static T? Deserialize<T>(ref Utf8JsonReader jsonReader, JsonTypeInfo<T> jsonTypeInfo)
        {
            try
            {
                return JsonSerializer.Deserialize(ref jsonReader, jsonTypeInfo);
            }
            catch (Exception expectableEx) when (expectableEx is ArgumentException || expectableEx is NotSupportedException || expectableEx is JsonException)
            {
                throw;
            }
            catch (Exception unexpectedEx)
            {
                if (unexpectedEx is OutOfMemoryException)
                {
                    GCHelper.FullCollect();
                }

                throw new InvalidOperationException("Deserialization error.", unexpectedEx);
            }
        }
    }
}
