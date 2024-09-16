//-----------------------------------------------------------------------------
// The MIT License (MIT)

// Copyright (c) Microsoft Corporation

// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions: 

// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software. 

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
// SOFTWARE.
//
// Source: https://github.com/microsoft/referencesource/blob/master/System.ServiceModel/System/ServiceModel/SynchronizedCollection.cs
//-----------------------------------------------------------------------------

namespace System.Collections.Generic
{
    using System;
    using System.Collections;

    /// <summary>
    /// Thread-safe list.
    /// </summary>
    /// <typeparam name="T">The type of item.</typeparam>
    [System.Runtime.InteropServices.ComVisible(false)]
    public class SynchronizedList<T> : IList<T>, IList
    {
        #region Fields

        private readonly List<T> items;
        private readonly object sync;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}"/> class.
        /// </summary>
        public SynchronizedList()
        {
            this.items = new List<T>();
            this.sync = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> is <c>null</c>.</exception>
        public SynchronizedList(object syncRoot)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);

            this.items = new List<T>();
            this.sync = syncRoot;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}"/> class.
        /// </summary>
        /// <param name="syncRoot">The object used to synchronize access to the thread-safe collection.</param>
        /// <param name="list">The <see cref="IEnumerable{T}"/> collection of elements used to initialize the thread-safe collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> or <paramref name="list"/> is <c>null</c>.</exception>
        public SynchronizedList(object syncRoot, IEnumerable<T> list)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);
            ArgumentNullException.ThrowIfNull(list);

            this.items = new List<T>(list);
            this.sync = syncRoot;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}"/> class.
        /// </summary>
        /// <param name="syncRoot">The object used to synchronize access to the thread-safe collection.</param>
        /// <param name="list">The <c>Array</c> of type T elements used to initialize the thread-safe collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="syncRoot"/> or <paramref name="list"/> is <c>null</c>.</exception>
        public SynchronizedList(object syncRoot, params T[] list)
        {
            ArgumentNullException.ThrowIfNull(syncRoot);
            ArgumentNullException.ThrowIfNull(list);

            this.items = new List<T>(list.Length);
            for (int i = 0; i < list.Length; i++)
                this.items.Add(list[i]);

            this.sync = syncRoot;
        }

        #endregion

        #region Properties

        protected List<T> Items
        {
            get { return this.items; }
        }

        protected object SyncRoot
        {
            get { return this.sync; }
        }

        #endregion

        #region IEnumerator

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            lock (this.sync)
            {
                return this.items.GetEnumerator();
            }
        }

        #endregion

        #region IList<T>

        /// <inheritdoc />
        public int Count
        {
            get { lock (this.sync) { return this.items.Count; } }
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                lock (this.sync)
                {
                    return this.items[index];
                }
            }
            set
            {
                lock (this.sync)
                {
                    if (index < 0 || index >= this.items.Count)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range");
                    }

                    this.SetItem(index, value);
                }
            }
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            lock (this.sync)
            {
                int index = this.items.Count;
                this.InsertItem(index, item);
            }
        }

        /// <inheritdoc />
        public void Clear()
        {
            lock (this.sync)
            {
                this.ClearItems();
            }
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int index)
        {
            lock (this.sync)
            {
                this.items.CopyTo(array, index);
            }
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            lock (this.sync)
            {
                return this.items.Contains(item);
            }
        }


        /// <inheritdoc />
        public int IndexOf(T item)
        {
            lock (this.sync)
            {
                return this.InternalIndexOf(item);
            }
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            lock (this.sync)
            {
                if (index < 0 || index > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range");
                }

                this.InsertItem(index, item);
            }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            lock (this.sync)
            {
                int index = this.InternalIndexOf(item);
                if (index < 0)
                    return false;

                this.RemoveItem(index);
                return true;
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            lock (this.sync)
            {
                if (index < 0 || index >= this.items.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), index, "Index was out of range");
                }

                this.RemoveItem(index);
            }
        }

        #endregion

        #region Explict interface implementations

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList)this.items).GetEnumerator();
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return this.sync; }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.sync)
            {
                ((IList)this.items).CopyTo(array, index);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        int IList.Add(object value)
        {
            VerifyValueType(value);

            lock (this.sync)
            {
                this.Add((T)value);
                return this.Count - 1;
            }
        }

        bool IList.Contains(object value)
        {
            VerifyValueType(value);
            return this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            VerifyValueType(value);
            return this.IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);
            this.Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            VerifyValueType(value);
            this.Remove((T)value);
        }

        #endregion

        #region Protected Methods

        protected virtual void ClearItems()
        {
            this.items.Clear();
        }

        protected virtual void InsertItem(int index, T item)
        {
            this.items.Insert(index, item);
        }

        protected virtual void RemoveItem(int index)
        {
            this.items.RemoveAt(index);
        }

        protected virtual void SetItem(int index, T item)
        {
            this.items[index] = item;
        }

        protected static void VerifyValueType(object value)
        {
            if (value == null)
            {
                if (typeof(T).IsValueType)
                {
                    throw new ArgumentException("The type of item must not be NULL in SynchronizedList.", nameof(value));
                }
            }
            else if (value is not T)
            {
                throw new ArgumentException("The type of item and T are different in SynchronizedList.", nameof(value));
            }
        }

        #endregion

        #region Private Methods

        private int InternalIndexOf(T item)
        {
            int count = items.Count;

            for (int i = 0; i < count; i++)
            {
                if (object.Equals(items[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        #endregion
    }
}