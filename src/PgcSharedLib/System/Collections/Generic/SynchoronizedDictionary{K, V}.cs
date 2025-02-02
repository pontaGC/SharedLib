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
            : this (0, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchoronizedDictionary{TKey, TValue}"/> class.
        /// </summary>
        /// <param name="capacity">The initial number of elements that <see cref="SynchoronizedDictionary{TKey, TValue}"/> can contain.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public SynchoronizedDictionary(int capacity)
            : this (capacity, null) { }

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

            this.dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
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

            this.dictionary = new Dictionary<TKey, TValue>(collection, comparer);
            this.syncRoot = syncRoot;
        }

        #endregion

        #region Indexer

        /// <inheritdoc />
        public TValue this[TKey key]
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.dictionary[key];
                }
            }

            set
            {
                lock (this.syncRoot)
                {
                    this.dictionary[key] = value;
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
                lock (this.syncRoot)
                {
                    return this.dictionary.Count;
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TKey> Keys
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.dictionary.Keys;
                }
            }
        }

        /// <inheritdoc />
        public ICollection<TValue> Values
        {
            get
            {
                lock (this.syncRoot)
                {
                    return this.dictionary.Values;
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
        public IEqualityComparer<TKey> Comparer => this.dictionary.Comparer;

        private IDictionary CastIDictionary => this.dictionary;

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Add(TKey key, TValue value)
        {
            lock (this.syncRoot)
            {
                this.dictionary.Add(key, value);
            }
        }

        /// <inheritdoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (this.syncRoot)
            {
                this.dictionary.Clear();
            }
        }

        /// <inheritdoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (this.syncRoot)
            {
                return this.dictionary.Contains(item);
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(TKey key)
        {
            lock (this.syncRoot)
            {
                return this.dictionary.ContainsKey(key);
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
            lock (this.syncRoot)
            {
                return this.dictionary.ContainsValue(value);
            }
        }

        /// <inheritdoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (this.syncRoot)
            {
                this.CastIDictionary.CopyTo(array, arrayIndex);
            }
        }

        /// <inheritdoc />
        public bool Remove(TKey key)
        {
            lock (this.syncRoot)
            {
                return this.dictionary.Remove(key);
            }
        }

        /// <inheritdoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <inheritdoc />
        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            lock (this.syncRoot)
            {
                return this.dictionary.TryGetValue(key, out value);
            }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (this.syncRoot)
            {
                return this.dictionary.GetEnumerator();
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
            lock (this.syncRoot)
            {
                return this.dictionary.TryAdd(key, value);
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
            lock (this.syncRoot)
            {
                return this.dictionary.EnsureCapacity(capacity);
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
            lock (this.syncRoot)
            {
                this.dictionary.TrimExcess(capacity);
            }
        }

        /// <summary>
        /// Sets the capacity of this dictionary to what it would be if it had been originally initialized with all its entries.
        /// </summary>
        public void TrimExcess()
        {
            this.TrimExcess(this.Count);
        }

        #endregion

        #region Exlicit interface implementation

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((IDictionary<TKey, TValue>)this.dictionary).IsReadOnly;

        #region IDictionary

        object? IDictionary.this[object key] 
        { 
            get
            {
                lock (this.syncRoot)
                {
                    return this.CastIDictionary[key];
                }
            }

            set
            {
                lock (this.syncRoot)
                {
                    this.CastIDictionary[key] = value;
                }
            } 
        }

        bool IDictionary.IsFixedSize => this.CastIDictionary.IsFixedSize;

        bool IDictionary.IsReadOnly => this.CastIDictionary.IsReadOnly;

        ICollection IDictionary.Keys => this.CastIDictionary.Keys;

        ICollection IDictionary.Values => this.CastIDictionary.Values;

        int ICollection.Count => this.Count;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => this.syncRoot;

        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.syncRoot)
            {
                this.CastIDictionary.CopyTo(array, index);
            }
        }

        void IDictionary.Add(object key, object? value)
        {
            lock (this.syncRoot)
            {
               this.CastIDictionary.Add(key, value);
            }
        }

        void IDictionary.Clear()
        {
            lock (this.syncRoot)
            {
                this.CastIDictionary.Clear();
            }
        }

        bool IDictionary.Contains(object key)
        {
            lock (this.syncRoot)
            {
                return this.CastIDictionary.Contains(key);
            }
        }

        void IDictionary.Remove(object key)
        {
            lock (this.syncRoot)
            {
                this.CastIDictionary.Remove(key);
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            lock (this.syncRoot)
            {
                return this.CastIDictionary.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IReadOnlyDictionary<TKey, TValue>

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.Values;

        #endregion

        #endregion
    }
}
