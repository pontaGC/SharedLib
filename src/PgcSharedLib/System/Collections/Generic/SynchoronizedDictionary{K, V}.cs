using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    /// <summary>
    /// Thread-safe dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys.</typeparam>
    /// <typeparam name="TValue">The type of values.</typeparam>
    public class SynchoronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        #region Fields

        private readonly Dictionary<TKey, TValue> dictionary;
        private readonly object syncRoot;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        public SynchoronizedDictionary()
            : this(0, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that <see cref="SynchoronizedDictionary{TKey, TValue}"/> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SynchoronizedDictionary(int capacity)
            : this(capacity, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        public SynchoronizedDictionary(IEqualityComparer<TKey>? comparer)
            : this(0, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that <see cref="SynchoronizedDictionary{TKey, TValue}"/> can contain.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SynchoronizedDictionary(int capacity, IEqualityComparer<TKey>? comparer)
            : this(new object(), capacity, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="syncRoot">The object that can be used to synchronize access to data in the dictionary.</param>
        /// <param name="capacity">The initial number of elements that <see cref="SynchoronizedDictionary{TKey, TValue}"/> can contain.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SynchoronizedDictionary(object syncRoot, int capacity, IEqualityComparer<TKey>? comparer)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
            this.syncRoot = syncRoot;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="dictionary"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(IDictionary<TKey, TValue> dictionary)
            : this(dictionary, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="dictionary"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
            : this(new object(), dictionary, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="syncRoot">The object that can be used to synchronize access to data in the dictionary.</param>
        /// <param name="dictionary">The <see cref="IDictionary{TKey, TValue}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> or <paramref name="dictionary"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="dictionary"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(object syncRoot, IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            this.dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
            this.syncRoot = syncRoot;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="collection"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
            : this(collection, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="collection"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
            : this(new object(), collection, comparer) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="syncRoot">The object that can be used to synchronize access to data in the dictionary.</param>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> whose elements are copied to the new <see cref="SynchoronizedDictionary{TKey, TValue}"/>.</param>
        /// <param name="comparer">The key comparer. If it is <c>null</c> to use the default <see cref="IEqualityComparer{T}"/> for the type of the key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> or <paramref name="collection"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="collection"/> contains one or more duplicated keys.</exception>
        public SynchoronizedDictionary(object syncRoot, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            dictionary = new Dictionary<TKey, TValue>(collection, comparer);
            this.syncRoot = syncRoot;
        }

        #endregion

        #region Indexer

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary[key];
                }
            }

            set
            {
                lock (syncRoot)
                {
                    dictionary[key] = value;
                }
            }
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public int Count
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary.Count;
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary.Keys;
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary.Values;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality of keys for the dictionary.
        /// </summary>
        /// <remarks>
        /// <see cref="Dictionary{TKey, TValue}"/> requires an equality implementation to determine whether keys are equal.
        /// You can specify an implementation of the <see cref="IEqualityComparer{T}"/> generic interface by using a constructor 
        /// that accepts a <c>comparer</c> parameter; if you do not specify one,
        /// the default generic equality comparer <see cref="EqualityComparer{T}.Default"/> is used.
        /// Getting the value of this property is an O(1) operation.
        /// </remarks>
        public IEqualityComparer<TKey> Comparer => dictionary.Comparer;

        private IDictionary CastIDictionary => dictionary;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                dictionary.Add(key, value);
            }
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (syncRoot)
            {
                dictionary.Clear();
            }
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                return dictionary.Contains(item);
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            lock (syncRoot)
            {
                return dictionary.ContainsKey(key);
            }
        }

        /// <summary>
        /// Determines whether this dictionary contains a specific value.
        /// </summary>
        /// <param name="value">The value to locate in this dictionary. The value can be <c>null</c> for reference types.</param>
        /// <returns><c>true</c> if this dictionary contains an element with the specified value; otherwise, <c>false</c>.</returns>
        /// <remarks>
        /// This method determines equality using <see cref="EqualityComparer{T}.Default"/>
        /// for <c>TValue</c>, the type of values in the dictionary.
        /// This method performs a linear search; therefore, the average execution time is proportional to <see cref="Count"/>.
        /// That is, this method is an O(n) operation, where n is <see cref="Count"/>.
        /// </remarks>
        public bool ContainsValue(TValue value)
        {
            lock (syncRoot)
            {
                return dictionary.ContainsValue(value);
            }
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (syncRoot)
            {
                CastIDictionary.CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            lock (syncRoot)
            {
                return dictionary.Remove(key);
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            lock (syncRoot)
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (syncRoot)
            {
                return dictionary.GetEnumerator();
            }
        }

        /// <summary>
        /// Attempts to add the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add. It can be <c>null</c>.</param>
        /// <returns><c>true</c> if the key/value pair was added to the dictionary successfully; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <remarks>
        /// Unlike the <see cref="Add(TKey, TValue)"/> method, this method doesn't throw an exception if the element with the given key exists in the dictionary.
        /// Unlike the Dictionary indexer, <c>TryAdd</c> doesn't override the element if the element with the given key exists in the dictionary.
        /// If the key already exists, <c>TryAdd</c> does nothing and returns <c>false</c>.
        /// </remarks>
        public bool TryAdd(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                return dictionary.TryAdd(key, value);
            }
        }

        /// <summary>
        /// Ensures that the dictionary can hold up to a specified number of entries without any further expansion of its backing storage.
        /// </summary>
        /// <param name="capacity">The number of entries.</param>
        /// <returns>The current capacity of this dictionary.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public int EnsureCapacity(int capacity)
        {
            lock (syncRoot)
            {
                return dictionary.EnsureCapacity(capacity);
            }
        }

        /// <summary>
        /// Sets the capacity of this dictionary to hold up a specified number of entries
        /// without any further expansion of its backing storage.
        /// </summary>
        /// <param name="capacity">The new capacity.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than <see cref="Count"/>.</exception>
        /// <remarks>
        /// This method can be used to minimize the memory overhead
        /// once it is known that no new elements will be added.
        /// </remarks>
        public void TrimExcess(int capacity)
        {
            lock (syncRoot)
            {
                dictionary.TrimExcess(capacity);
            }
        }

        /// <summary>
        /// Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its entries.
        /// </summary>
        public void TrimExcess()
        {
            TrimExcess(Count);
        }

        #endregion

        #region Exlicit interface implementation

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((IDictionary<TKey, TValue>)dictionary).IsReadOnly;

        #region IDictionary

        object? IDictionary.this[object key]
        {
            get
            {
                lock (syncRoot)
                {
                    return CastIDictionary[key];
                }
            }

            set
            {
                lock (syncRoot)
                {
                    CastIDictionary[key] = value;
                }
            }
        }

        bool IDictionary.IsFixedSize => CastIDictionary.IsFixedSize;

        bool IDictionary.IsReadOnly => CastIDictionary.IsReadOnly;

        ICollection IDictionary.Keys => CastIDictionary.Keys;

        ICollection IDictionary.Values => CastIDictionary.Values;

        int ICollection.Count => Count;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => syncRoot;

        void ICollection.CopyTo(Array array, int index)
        {
            lock (syncRoot)
            {
                CastIDictionary.CopyTo(array, index);
            }
        }

        void IDictionary.Add(object key, object? value)
        {
            lock (syncRoot)
            {
                CastIDictionary.Add(key, value);
            }
        }

        void IDictionary.Clear()
        {
            lock (syncRoot)
            {
                CastIDictionary.Clear();
            }
        }

        bool IDictionary.Contains(object key)
        {
            lock (syncRoot)
            {
                return CastIDictionary.Contains(key);
            }
        }

        void IDictionary.Remove(object key)
        {
            lock (syncRoot)
            {
                CastIDictionary.Remove(key);
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            lock (syncRoot)
            {
                return CastIDictionary.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IReadOnlyDictionary<TKey, TValue>

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        #endregion

        #endregion
    }
}
