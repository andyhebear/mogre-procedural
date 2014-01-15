/////////////////////////////////////////////////////////////////////
// File Name               : multimap.cs
//      Created            : 23 7 2012   22:22
//      Author             : Costin S
//
/////////////////////////////////////////////////////////////////////
#define TREE_WITH_PARENT_POINTERS

namespace SortedDictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;

    /// <summary>
    /// multimap implementation based on a self balanced binary avl tree.
    /// check http://code.google.com/p/self-balancing-avl-tree for my standalone implementation of an avl tree including concatenate and split functionality
    /// </summary>
    public class multimap<TKey, TValue> : IDictionary<TKey, ICollection<TValue>>, IDictionary, ICollection<KeyValuePair<TKey, ICollection<TValue>>>, ICollection
    {
        #region Fields

        private IComparer<TKey> keyComparer;
        private IComparer<TValue> valueComparer;
        private AVLTree<TKey, TValue> tree;

        #endregion

        #region C'tor

        public multimap() :
            this(GetComparer<TKey>(), GetComparer<TValue>())
        {
        }

        public multimap(IComparer<TKey> keyComparer)
            : this(keyComparer, GetComparer<TValue>())
        {
        }

        private multimap(IComparer<TKey> keyComparer, IComparer<TValue> valueComparer)
        {
            this.keyComparer = keyComparer;
            this.valueComparer = valueComparer;
            this.tree = new AVLTree<TKey, TValue>(keyComparer, valueComparer);
        }       

        #endregion
               
        #region IDictionary<TKey, ICollection<TValue>> Members

        /// <summary>
        /// Adds the specified collection of values to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public void Add(TKey key, ICollection<TValue> values)
        {
            if (values != null)
            {
                foreach (TValue value in values)
                {
                    this.tree.Add(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        /// <summary>
        /// Adds the specified (key, value) pair.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(TKey key, TValue value)
        {
            this.tree.Add(new KeyValuePair<TKey, TValue>(key, value));            
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.IDictionary`2"></see> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.tree.ContainsKey(key);
        }

        /// <summary>
        /// Removes the key and its associated values from the multimap</see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        public bool Remove(TKey key)
        {
            return this.tree.DeleteKey(key);
        }

        public bool TryGetValue(TKey key, out ICollection<TValue> values)
        {
            if (this.ContainsKey(key))
            {
                values = this[key];
                return true;
            }
            else
            {
                values = null;
                return false;
            }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TKey> Keys
        {
            get 
            {
                return new KeysCollection(this);
            }
        }

        ICollection<ICollection<TValue>> IDictionary<TKey, ICollection<TValue>>.Values
        {
            get 
            {
                return new CollectionOfValuesCollection(this); 
            }
        }

        public ICollection<TValue> this[TKey key]
        {
            get
            {
                return new KeyValuesCollection(this.tree, key);
            }

            set
            {                
                if (this.ContainsKey(key))
                {
                    this.tree.DeleteKey(key);
                }
                
                if (value != null)                
                {                    
                    this.Add(key, value);                    
                }
            }
        }

        public void Add(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            this.tree.Clear();
        }

        public bool Contains(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            var values = item.Value;
            var key = item.Key;

            foreach (TValue value in values)
            {
                if (!this.tree.Contains(key, value))
                {
                    return false;
                }
            }

            return true;
        }

        public void CopyTo(KeyValuePair<TKey, ICollection<TValue>>[] array, int arrayIndex)
        {
            int count = this.Count;

            if (count <= 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "argument 'arrayIndex' cannot be negative");
            }

            if (arrayIndex >= array.Length || count > array.Length - arrayIndex)
            {
                throw new ArgumentException("not enough space in the array", "arrayIndex");
            }

            int index = arrayIndex, i = 0;
            foreach (KeyValuePair<TKey, ICollection<TValue>> item in (ICollection<KeyValuePair<TKey, ICollection<TValue>>>)this)
            {
                if (i >= count)
                {
                    break;
                }

                array[index] = item;
                ++index;
                ++i;
            }
        }

        public int Count
        {
            get { return this.tree.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }
        
        /// <summary>
        /// Removes the specified collection of values associated with the specified key.
        /// </summary>
        /// <param name="item">The (key, values) to remove from the multimap </param>
        /// <returns>
        /// True if at least one (key, value) pair was successfully removed; otherwise, false. If the last value associated with the key is removed, the key will be removed too
        /// </returns>
        public bool Remove(KeyValuePair<TKey, ICollection<TValue>> item)
        {
            var values = item.Value;
            var key = item.Key;

            if (values != null)
            {
                int count = 0;
                foreach (TValue value in values)
                {
                    if (this.tree.Delete(new KeyValuePair<TKey, TValue>(key, value)))
                    {
                        count++;
                    }
                }

                return count > 0;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> GetEnumerator()
        {
            IEnumerator<TKey> keysEnumerator = this.tree.EnumerateKeys();
            if (keysEnumerator != null)
            {                
                while (keysEnumerator.MoveNext())
                {
                    TKey keyCurrent = keysEnumerator.Current;
                    yield return new KeyValuePair<TKey, ICollection<TValue>>(keyCurrent, new KeyValuesCollection(this.tree, keyCurrent));
                }                
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary)this).GetEnumerator();
        }

        #endregion

        #region IDictionary

        void IDictionary.Add(object key, object value)
        {
            if (!(key is TKey))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The key argument must be of type {0} and it is not ", typeof(TKey)), "key");
            }

            if (!(value is TValue))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value argument must be of type {0} and it is not ", typeof(ICollection<TValue>)), "value");
            }

            this.Add((TKey)key, (ICollection<TValue>)value);
        }

        bool IDictionary.Contains(object key)
        {
            if (key is TKey || key == null)
            {
                return this.ContainsKey((TKey)key);
            }
            else
            {
                return false;
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this.GetEnumerator());
        }

        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        void IDictionary.Remove(object key)
        {
            if (key is TKey || key == null)
            {
                this.Remove((TKey)key);
            }
        }

        ICollection IDictionary.Keys
        {
            get { throw new NotImplementedException(); }
        }
        
        ICollection IDictionary.Values
        {
            get { throw new NotImplementedException(); }
        }

        public object this[object key]
        {
            get
            {
                if (key is TKey || key == null)
                {
                    ICollection<TValue> values;
                    if (this.TryGetValue((TKey)key, out values))
                    {
                        return values;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            int count = this.Count;

            if (count <= 0)
            {
                return;
            }

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index, "argument 'index' cannot be negative");
            }

            if (index >= array.Length || count > array.Length - index)
            {
                throw new ArgumentException("not enough space in the array", "index");
            }

            int k = 0;
            foreach (object o in (ICollection)this)
            {
                if (k >= count)
                {
                    break;
                }

                array.SetValue(o, index++);
                k++;
            }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region Properties

        public IComparer<TKey> KeyComparer
        {
            get
            {
                return this.keyComparer;
            }
        }

        public IComparer<TValue> ValueComparer
        {
            get
            {
                return this.valueComparer;
            }
        }

        #endregion

        #region Public Methods

        public void ConsolePrint()
        {
            this.tree.Print();
        }

        #endregion

        #region Private Methods

        private static IComparer<T> GetComparer<T>()
        {
            if (typeof(IComparable<T>).IsAssignableFrom(typeof(T)) || typeof(System.IComparable).IsAssignableFrom(typeof(T)))
            {
                return Comparer<T>.Default;
            }
            else
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The type {0} cannot be compared. It must implement IComparable<T> or IComparable", typeof(T).FullName));
            }
        }

        #endregion

        #region Nested Classes

        private abstract class ReadOnlyCollection<T> : ICollection<T>, ICollection
        {
            #region C'tor

            /// <summary>
            /// Initializes a new instance of the <see cref="multimap&lt;TKey, TValue&gt;.ReadOnlyCollection&lt;T&gt;"/> class.
            /// </summary>
            public ReadOnlyCollection()
            {
            }

            #endregion

            #region ICollection<T> Members

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
            public abstract int Count { get; }

            /// <summary>
            /// Determines whether [contains] [the specified key].
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public abstract bool Contains(T key);

            /// <summary>
            /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
            public void Add(T item)
            {
                throw new NotSupportedException("Cannot modify a readonly collection.");
            }

            /// <summary>
            /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
            public void Clear()
            {
                throw new NotSupportedException("Cannot modify a readonly collection.");
            }

            /// <summary>
            /// Copies to.
            /// </summary>
            /// <param name="array">The array.</param>
            /// <param name="arrayIndex">Index of the array.</param>
            public void CopyTo(T[] array, int arrayIndex)
            {
                int count = this.Count;

                if (count <= 0)
                {
                    return;
                }

                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count", count, "argument 'count' cannot be negative");
                }

                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "argument 'arrayIndex' cannot be negative");
                }

                if (arrayIndex >= array.Length || count > array.Length - arrayIndex)
                {
                    throw new ArgumentException("not enough space in the array", "arrayIndex");
                }

                int index = arrayIndex, i = 0;
                foreach (T item in (ICollection<T>)this)
                {
                    if (i >= count)
                    {
                        break;
                    }

                    array[index] = item;
                    ++index;
                    ++i;
                }
            }

            /// <summary>
            /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
            /// </summary>
            /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
            public bool IsReadOnly
            {
                get { return true; }
            }

            /// <summary>
            /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
            /// <returns>
            /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </returns>
            /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
            public bool Remove(T item)
            {
                throw new NotSupportedException("Cannot modify a readonly collection.");
            }

            #endregion

            #region IEnumerable<T> Members

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public abstract IEnumerator<T> GetEnumerator();

            #endregion

            #region IEnumerable Members

            /// <summary>
            /// Returns an enumerator that iterates through a collection.
            /// </summary>
            /// <returns>
            /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
            /// </returns>
            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (T item in this)
                {
                    yield return item;
                }
            }

            #endregion

            #region ICollection Members

            /// <summary>
            /// Copies the elements of the <see cref="T:System.Collections.ICollection"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException">
            ///   <paramref name="array"/> is null. </exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException">
            ///   <paramref name="index"/> is less than zero. </exception>
            /// <exception cref="T:System.ArgumentException">
            ///   <paramref name="array"/> is multidimensional.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"/> is greater than the available space from <paramref name="index"/> to the end of the destination <paramref name="array"/>. </exception>
            /// <exception cref="T:System.ArgumentException">The type of the source <see cref="T:System.Collections.ICollection"/> cannot be cast automatically to the type of the destination <paramref name="array"/>. </exception>
            void ICollection.CopyTo(Array array, int index)
            {
                int count = this.Count;

                if (count <= 0)
                {
                    return;
                }

                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (index < 0)
                {
                    throw new ArgumentOutOfRangeException("index", index, "argument 'index' cannot be negative");
                }

                if (index >= array.Length || count > array.Length - index)
                {
                    throw new ArgumentException("not enough space in the array", "index");
                }

                int k = 0;
                foreach (object o in (ICollection)this)
                {
                    if (k >= count)
                    {
                        break;
                    }

                    array.SetValue(o, index++);
                    k++;
                }
            }

            /// <summary>
            /// Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe).
            /// </summary>
            /// <returns>true if access to the <see cref="T:System.Collections.ICollection"/> is synchronized (thread safe); otherwise, false.</returns>
            public bool IsSynchronized
            {
                get { return false; }
            }

            /// <summary>
            /// Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.
            /// </summary>
            /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection"/>.</returns>
            public object SyncRoot
            {
                get { return this; }
            }

            #endregion
        }

        private sealed class KeyValuesCollection : ICollection<TValue>, ICollection
        {
            private AVLTree<TKey, TValue> tree;
            private TKey key;

            #region C'tor

            public KeyValuesCollection(AVLTree<TKey, TValue> tree, TKey key)
            {
                this.tree = tree;
                this.key = key;
            }

            #endregion

            #region ICollection<T> Members

            public void Add(TValue item)
            {
                this.tree.Add(new KeyValuePair<TKey, TValue>(this.key, item));
            }

            public void CopyTo(TValue[] array, int arrayIndex)
            {
                int count = this.Count;

                if (count <= 0)
                {
                    return;
                }

                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "argument 'arrayIndex' cannot be negative");
                }

                if (arrayIndex >= array.Length || count > array.Length - arrayIndex)
                {
                    throw new ArgumentException("not enough space in the array", "arrayIndex");
                }

                int index = arrayIndex, i = 0;
                foreach (TValue item in (ICollection<TValue>)this)
                {
                    if (i >= count)
                    {
                        break;
                    }

                    array[index] = item;
                    ++index;
                    ++i;
                }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(TValue item)
            {
                return this.tree.Delete(new KeyValuePair<TKey, TValue>(this.key, item));
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (TValue item in this)
                {
                    yield return item;
                }
            }

            public void Clear()
            {
                this.tree.DeleteKey(this.key);
            }

            public bool Contains(TValue item)
            {
                return this.tree.Contains(this.key, item);
            }

            public int Count
            {
                get
                {
                    return this.tree.GetKeyValuesCount(this.key);
                }
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                IEnumerator<TValue> values;
                if (this.tree.TryEnumerateKeyValues(this.key, out values))
                {
                    while (values.MoveNext())
                    {
                        yield return values.Current;
                    }
                }
                else
                {
                    yield break;
                }
            }

            #endregion

            #region ICollection Members

            public void CopyTo(Array array, int arrayIndex)
            {
                int count = this.Count;

                if (count == 0)
                {
                    return;
                }

                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }

                if (count < 0)
                {
                    throw new ArgumentOutOfRangeException("count", count, "argument 'count' cannot be negative");
                }

                if (arrayIndex < 0)
                {
                    throw new ArgumentOutOfRangeException("arrayIndex", arrayIndex, "argument 'arrayIndex' cannot be negative");
                }

                if (arrayIndex >= array.Length || count > array.Length - arrayIndex)
                {
                    throw new ArgumentException("arrayIndex", "not enough space in the array");
                }

                int index = arrayIndex, i = 0;
                foreach (var item in (ICollection<TValue>)this)
                {
                    if (i >= count)
                    {
                        break;
                    }

                    array.SetValue(item, index);
                    ++index;
                    ++i;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return this;
                }
            }

            #endregion
        }

        /// <summary>
        /// Internal class used to implement the collection of keys in our map
        /// </summary>
        private sealed class KeysCollection : ReadOnlyCollection<TKey>
        {
            #region Fields

            private multimap<TKey, TValue> dictionary;

            #endregion

            #region C'tor

            /// <summary>
            /// Initializes a new instance of the <see cref="multimap&lt;TKey, TValue&gt;.KeysCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The dictionary.</param>
            public KeysCollection(multimap<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            #endregion

            #region Overrides

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
            public override int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            /// <summary>
            /// Determines whether [contains] [the specified key].
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public override bool Contains(TKey key)
            {
                return this.dictionary.ContainsKey(key);
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<TKey> GetEnumerator()
            {
                return this.dictionary.tree.EnumerateKeys();
            }

            #endregion
        }

        /// <summary>
        /// Internal class used to implement the collection of values in our map
        /// </summary>
        private sealed class CollectionOfValuesCollection : ReadOnlyCollection<ICollection<TValue>>
        {
            #region Fields

            private multimap<TKey, TValue> dictionary;

            #endregion

            #region C'tor

            /// <summary>
            /// Initializes a new instance of the <see cref="multimap&lt;TKey, TValue&gt;.CollectionOfValuesCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The dictionary.</param>
            public CollectionOfValuesCollection(multimap<TKey, TValue> dictionary)
            {
                this.dictionary = dictionary;
            }

            #endregion

            #region Overrides

            /// <summary>
            /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
            /// </summary>
            /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
            public override int Count
            {
                get
                {
                    return this.dictionary.Count;
                }
            }

            /// <summary>
            /// Determines whether [contains] [the specified values].
            /// </summary>
            /// <param name="values">The values.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified values]; otherwise, <c>false</c>.
            /// </returns>
            public override bool Contains(ICollection<TValue> values)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<ICollection<TValue>> GetEnumerator()
            {
                using (IEnumerator<TKey> enumerator = this.dictionary.tree.EnumerateKeys())
                {
                    if (enumerator != null)
                    {
                        while (enumerator.MoveNext())
                        {
                            TKey key = enumerator.Current;
                            yield return new KeyValuesCollection(this.dictionary.tree, key);
                        }
                    }
                }
            }

            #endregion
        }

        /// <summary>
        /// IDictionaryEnumerator implementation
        /// </summary>
        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> enumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="multimap&lt;TKey, TValue&gt;.DictionaryEnumerator"/> class.
            /// </summary>
            /// <param name="enumerator">The enumerator.</param>
            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, ICollection<TValue>>> enumerator)
            {
                this.enumerator = enumerator;
            }

            /// <summary>
            /// Gets both the key and the value of the current dictionary entry.
            /// </summary>
            /// <returns>A <see cref="T:System.Collections.DictionaryEntry"/> containing both the key and the value of the current dictionary entry.</returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public DictionaryEntry Entry
            {
                get
                {
                    KeyValuePair<TKey, ICollection<TValue>> pair = this.enumerator.Current;
                    DictionaryEntry entry = new DictionaryEntry();
                    if (pair.Key != null)
                    {
                        entry.Key = pair.Key;
                    }

                    entry.Value = pair.Value;
                    return entry;
                }
            }

            /// <summary>
            /// Gets the key of the current dictionary entry.
            /// </summary>
            /// <returns>The key of the current element of the enumeration.</returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public object Key
            {
                get
                {
                    KeyValuePair<TKey, ICollection<TValue>> current = this.enumerator.Current;
                    return current.Key;
                }
            }

            /// <summary>
            /// Gets the value of the current dictionary entry.
            /// </summary>
            /// <returns>The value of the current element of the enumeration.</returns>
            /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.IDictionaryEnumerator"/> is positioned before the first entry of the dictionary or after the last entry. </exception>
            public object Value
            {
                get
                {
                    KeyValuePair<TKey, ICollection<TValue>> current = this.enumerator.Current;
                    return current.Value;
                }
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            /// <returns>The current element in the collection.</returns>
            /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
            public object Current
            {
                get
                {
                    return this.Entry;
                }
            }
        }       

        /// <summary>
        /// AVLTree class
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the nodes</typeparam>
        private class AVLTree<TypeKey, TypeValue>
        {
            #region Fields

            private IComparer<TypeKey> keyComparer;

            private IComparer<TypeValue> valueComparer;

            private Node Root { get; set; }

            #endregion

            #region Ctor

            /// <summary>
            /// Initializes a new instance of the <see cref="multimap&lt;TKey, TValue&gt;.AVLTree&lt;TypeKey, TypeValue&gt;"/> class.
            /// </summary>
            /// <param name="keyComparer">The key comparer.</param>
            /// <param name="valueComparer">The value comparer.</param>
            public AVLTree(IComparer<TypeKey> keyComparer, IComparer<TypeValue> valueComparer)
            {
                this.keyComparer = keyComparer;
                this.valueComparer = valueComparer;
            }

            #endregion

            #region Delegates

            private delegate void VisitNodeHandler<TNode>(TNode node, int level);

            #endregion

            #region Public Methods

            /// <summary>
            /// Gets the count.
            /// </summary>
            /// <value>
            /// The count.
            /// </value>
            public int Count
            {
                get;
                private set;
            }

            /// <summary>
            /// Adds the specified arg.
            /// </summary>
            /// <param name="arg">The arg.</param>
            /// <returns></returns>
            public bool Add(KeyValuePair<TypeKey, TypeValue> arg)
            {
                bool wasAdded = false;
                bool wasSuccessful = false;

                this.Root = this.Add(this.Root, arg, ref wasAdded, ref wasSuccessful);

                if (wasSuccessful)
                {
                    this.Count++;
                }

                return wasSuccessful;
            }

            /// <summary>
            /// Deletes the specified key.
            /// </summary>
            /// <param name="arg">The key.</param>
            public bool DeleteKey(TypeKey key)
            {
                bool wasSuccessful = false;

                if (this.Root != null)
                {
                    bool wasDeleted = false;
                    this.Root = this.DeleteKey(this.Root, key, ref wasDeleted, ref wasSuccessful);

                    if (wasSuccessful)
                    {
                        this.Count--;
                    }
                }

                return wasSuccessful;
            }

            /// <summary>
            /// Deletes the specified (key, value) pair from the multimap. If the last value associated with a key is removed, the key wil also be removed
            /// </summary>
            /// <param name="arg">The (key, value) pair to be removed.</param>
            public bool Delete(KeyValuePair<TypeKey, TypeValue> arg)
            {
                bool wasSuccessful = false;

                if (this.Root != null)
                {
                    bool wasDeleted = false;
                    this.Root = this.Delete(this.Root, arg.Key, arg.Value, ref wasDeleted, ref wasSuccessful);

                    if (wasSuccessful)
                    {
                        if (!this.ContainsKey(arg.Key))
                        {
                            this.Count--;
                        }
                    }
                }

                return wasSuccessful;
            }

            /// <summary>
            /// Determines whether [contains] [the specified arg].
            /// </summary>
            /// <param name="arg">The arg.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified arg]; otherwise, <c>false</c>.
            /// </returns>
            public bool ContainsKey(TypeKey key)
            {
                return this.SearchKey(this.Root, key) != null;
            }

            /// <summary>
            /// Determines whether [contains] [the specified key].
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public bool Contains(TypeKey key, TypeValue value)
            {
                var node = this.SearchKey(this.Root, key);
                return node != null && this.ContainsValue(node, value);
            }            

            /// <summary>
            /// Returns the height of the tree in O(log N).
            /// </summary>
            /// <returns></returns>
            public int GetHeightLogN()
            {
                return this.GetHeightLogN(this.Root);
            }

            /// <summary>
            /// Clears this instance.
            /// </summary>
            public void Clear()
            {
                this.Root = null;
                this.Count = 0;
            }

            /// <summary>
            /// Tries the enumerate key values.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="values">The values.</param>
            /// <returns></returns>
            public bool TryEnumerateKeyValues(TypeKey key, out IEnumerator<TypeValue> values)
            {
                if (this.ContainsKey(key))
                {
                    values = this.EnumerateKeyValues(key);
                    return true;
                }
                else
                {
                    values = null;
                    return false;
                }
            }                        

            /// <summary>
            /// Prints this instance.
            /// </summary>
            public void Print()
            {
                this.Visit((node, level) =>
                {
                    Console.Write(new string(' ', 2 * level));

                    using (var values = this.EnumerateKeyValues(node.Data.Key))
                    {
                        Console.Write("{0, 6}", node.Data.Key);
                        Console.Write(", [");
                        while (values.MoveNext())
                        {
                            Console.Write("{0}, ", values.Current);
                        }
                        Console.Write("]");
                        Console.WriteLine();
                    }
                });
            }

            /// <summary>
            /// Gets the collection of keys.
            /// </summary>
            internal IEnumerator<TypeKey> EnumerateKeys()
            {
                if (this.Root == null)
                {
                    yield break;
                }

                var p = FindMin(this.Root);
                while (p != null)
                {
                    yield return p.Data.Key;
                    p = Successor(p);
                }
            }

            internal int GetKeyValuesCount(TypeKey key)
            {
                int valueCount = 0;
                IEnumerator<TypeValue> enumeratorValues = null;

                if (this.TryEnumerateKeyValues(key, out enumeratorValues))
                {
                    using (enumeratorValues)
                    {
                        while (enumeratorValues.MoveNext())
                        {
                            ++valueCount;
                        }
                    }
                }

                return valueCount;
            }

            #endregion

            #region Private Methods            

            /// <summary>
            /// Gets the height of the tree in log(n) time.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>The height of the tree. Runs in O(log(n)) where n is the number of nodes in the tree </returns>
            private int GetHeightLogN(Node node)
            {
                if (node == null)
                {
                    return 0;
                }
                else
                {
                    int leftHeight = this.GetHeightLogN(node.Left);
                    if (node.Balance == 1)
                    {
                        leftHeight++;
                    }

                    return 1 + leftHeight;
                }
            }

            /// <summary>
            /// Adds the specified elem.
            /// </summary>
            /// <param name="elem">The elem.</param>
            /// <param name="data">The data.</param>
            /// <returns></returns>
            private Node Add(Node elem, KeyValuePair<TypeKey, TypeValue> data, ref bool wasAdded, ref bool wasSuccessful)
            {
                if (elem == null)
                {
                    elem = new Node { Data = data, Left = null, Right = null, Balance = 0 };

                    wasAdded = true;
                    wasSuccessful = true;
                }
                else
                {
                    if (this.keyComparer.Compare(data.Key, elem.Data.Key) < 0)
                    {
                        elem.Left = Add(elem.Left, data, ref wasAdded, ref wasSuccessful);
                        if (wasAdded)
                        {
                            elem.Balance--;

                            if (elem.Balance == 0)
                            {
                                wasAdded = false;
                            }
                        }

#if TREE_WITH_PARENT_POINTERS
                        elem.Left.Parent = elem;
#endif

                        if (elem.Balance == -2)
                        {
                            if (elem.Left.Balance == 1)
                            {
                                int elemLeftRightBalance = elem.Left.Right.Balance;

                                elem.Left = elem.Left.RotateLeft();
                                elem = elem.RotateRight();

                                elem.Balance = 0;
                                elem.Left.Balance = elemLeftRightBalance == 1 ? -1 : 0;
                                elem.Right.Balance = elemLeftRightBalance == -1 ? 1 : 0;
                            }
                            else if (elem.Left.Balance == -1)
                            {
                                elem = elem.RotateRight();
                                elem.Balance = 0;
                                elem.Right.Balance = 0;
                            }

                            wasAdded = false;
                        }
                    }
                    else if (this.keyComparer.Compare(data.Key, elem.Data.Key) > 0)
                    {
                        elem.Right = Add(elem.Right, data, ref wasAdded, ref wasSuccessful);
                        if (wasAdded)
                        {
                            elem.Balance++;
                            if (elem.Balance == 0)
                            {
                                wasAdded = false;
                            }
                        }

#if TREE_WITH_PARENT_POINTERS
                        elem.Right.Parent = elem;
#endif
                        if (elem.Balance == 2)
                        {
                            if (elem.Right.Balance == -1)
                            {
                                int elemRightLeftBalance = elem.Right.Left.Balance;

                                elem.Right = elem.Right.RotateRight();
                                elem = elem.RotateLeft();

                                elem.Balance = 0;
                                elem.Left.Balance = elemRightLeftBalance == 1 ? -1 : 0;
                                elem.Right.Balance = elemRightLeftBalance == -1 ? 1 : 0;
                            }
                            else if (elem.Right.Balance == 1)
                            {
                                elem = elem.RotateLeft();

                                elem.Balance = 0;
                                elem.Left.Balance = 0;
                            }

                            wasAdded = false;
                        }
                    }
                    else
                    {
                        // we allow multiple values per key
                        if (elem.Range == null)
                        {
                            elem.Range = new List<TypeValue>();
                        }

                        elem.Range.Add(data.Value);
                    }
                }

                return elem;
            }

            /// <summary>
            /// Deletes the key.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <param name="key">The key.</param>
            /// <param name="wasDeleted">dymmy</param>
            /// <param name="wasSuccessful">if set to <c>true</c> [was successful].</param>
            /// <returns></returns>
            private Node DeleteKey(Node node, TypeKey key, ref bool wasDeleted, ref bool wasSuccessful)
            {
                int cmp = this.keyComparer.Compare(key, node.Data.Key);
                if (cmp < 0)
                {
                    if (node.Left != null)
                    {
                        node.Left = this.DeleteKey(node.Left, key, ref wasDeleted, ref wasSuccessful);

                        if (wasDeleted)
                        {
                            node.Balance++;
                        }
                    }
                }
                else if (cmp == 0)
                {                    
                    if (node.Left != null && node.Right != null)
                    {
                        var min = FindMin(node.Right);

                        KeyValuePair<TypeKey, TypeValue> data = node.Data;
                        var range = node.Range;

                        node.Data = min.Data;
                        node.Range = min.Range;

                        min.Data = data;
                        min.Range = range;

                        wasDeleted = false;
                        node.Right = DeleteKey(node.Right, data.Key, ref wasDeleted, ref wasSuccessful);

                        if (wasDeleted)
                        {
                            node.Balance--;
                        }
                    }
                    else if (node.Left == null)
                    {
                        wasSuccessful = true;
                        wasDeleted = true;
                        return node.Right;
                    }
                    else
                    {
                        wasSuccessful = true;
                        wasDeleted = true;
                        return node.Left;
                    }
                }
                else
                {
                    if (node.Right != null)
                    {
                        node.Right = this.DeleteKey(node.Right, key, ref wasDeleted, ref wasSuccessful);
                        if (wasDeleted)
                        {
                            node.Balance--;
                        }
                    }
                }

                if (wasDeleted)
                {
                    if (node.Balance == 1 || node.Balance == -1)
                    {
                        wasDeleted = false;
                    }
                    else if (node.Balance == -2)
                    {
                        if (node.Left.Balance == 1)
                        {
                            int leftRightBalance = node.Left.Right.Balance;

                            node.Left = node.Left.RotateLeft();
                            node = node.RotateRight();

                            node.Balance = 0;
                            node.Left.Balance = (leftRightBalance == 1) ? -1 : 0;
                            node.Right.Balance = (leftRightBalance == -1) ? 1 : 0;
                        }
                        else if (node.Left.Balance == -1)
                        {
                            node = node.RotateRight();
                            node.Balance = 0;
                            node.Right.Balance = 0;
                        }
                        else if (node.Left.Balance == 0)
                        {
                            node = node.RotateRight();
                            node.Balance = 1;
                            node.Right.Balance = -1;

                            wasDeleted = false;
                        }
                    }
                    else if (node.Balance == 2)
                    {
                        if (node.Right.Balance == -1)
                        {
                            int rightLeftBalance = node.Right.Left.Balance;

                            node.Right = node.Right.RotateRight();
                            node = node.RotateLeft();

                            node.Balance = 0;
                            node.Left.Balance = (rightLeftBalance == 1) ? -1 : 0;
                            node.Right.Balance = (rightLeftBalance == -1) ? 1 : 0;
                        }
                        else if (node.Right.Balance == 1)
                        {
                            node = node.RotateLeft();
                            node.Balance = 0;
                            node.Left.Balance = 0;
                        }
                        else if (node.Right.Balance == 0)
                        {
                            node = node.RotateLeft();
                            node.Balance = -1;
                            node.Left.Balance = 1;

                            wasDeleted = false;
                        }
                    }
                }

                return node;
            }

            private bool DeleteValueFromKey(Node node, TypeValue value)
            {
                if (node != null)
                {
                    int position = -1;

                    if (this.valueComparer.Compare(value, node.Data.Value) == 0)
                    {
                        position = 0;
                    }
                    else if (node.Range != null)
                    {
                        int index = node.Range.FindIndex((TypeValue match) => { return this.valueComparer.Compare(value, match) == 0; });
                        if (index != -1)
                        {
                            position = index + 1;
                        }

                    }

                    if (position > 0)
                    {
                        // we're counting the value stored in the KeyValuePair as position 0, all values stored in Range represent position + 1, position + 2, ...etc
                        if (node.Range != null && position - 1 < node.Range.Count)
                        {
                            node.Range.RemoveAt(position - 1);
                            if (node.Range.Count == 0)
                            {
                                node.Range = null;
                            }
                            return true;
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException("value", "The value specified is not associated with this key. Invalid argument.");
                        }
                    }
                    else if (position == 0)
                    {
                        if (node.Range != null && node.Range.Count > 0)
                        {
                            node.Data = new KeyValuePair<TypeKey, TypeValue>(node.Data.Key, node.Range[0]);
                            node.Range.RemoveAt(0);
                            if (node.Range.Count == 0)
                            {
                                node.Range = null;
                            }
                            return true;
                        }
                    }
                }
                return false;
            }

            /// <summary>
            /// Deletes the specified (key, value) pair.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            /// <param name="wasDeleted">if set to <c>true</c> [was deleted].</param>
            /// <param name="wasSuccessful">if set to <c>true</c> [was successful].</param>
            /// <returns></returns>
            private Node Delete(Node node, TypeKey key, TypeValue value, ref bool wasDeleted, ref bool wasSuccessful)
            {
                int cmp = this.keyComparer.Compare(key, node.Data.Key);
                if (cmp < 0)
                {
                    if (node.Left != null)
                    {
                        node.Left = this.Delete(node.Left, key, value, ref wasDeleted, ref wasSuccessful);

                        if (wasDeleted)
                        {
                            node.Balance++;
                        }
                    }
                }
                else if (cmp == 0)
                {
                    if (this.DeleteValueFromKey(node, value))
                    {
                        wasSuccessful = true;
                    }
                    else
                    {
                        if (node.Left != null && node.Right != null)
                        {
                            var min = FindMin(node.Right);

                            KeyValuePair<TypeKey, TypeValue> data = node.Data;
                            var range = node.Range;

                            node.Data = min.Data;
                            node.Range = min.Range;

                            min.Data = data;
                            min.Range = range;

                            wasDeleted = false;
                            node.Right = this.Delete(node.Right, data.Key, data.Value, ref wasDeleted, ref wasSuccessful);

                            if (wasDeleted)
                            {
                                node.Balance--;
                            }
                        }
                        else if (node.Left == null)
                        {
                            wasSuccessful = true;
                            wasDeleted = true;
                            return node.Right;
                        }
                        else
                        {
                            wasSuccessful = true;
                            wasDeleted = true;
                            return node.Left;
                        }
                    }
                }
                else
                {
                    if (node.Right != null)
                    {
                        node.Right = this.Delete(node.Right, key, value, ref wasDeleted, ref wasSuccessful);
                        if (wasDeleted)
                        {
                            node.Balance--;
                        }
                    }
                }

                if (wasDeleted)
                {
                    if (node.Balance == 1 || node.Balance == -1)
                    {
                        wasDeleted = false;
                        return node;
                    }

                    else if (node.Balance == -2)
                    {
                        if (node.Left.Balance == 1)
                        {
                            int leftRightBalance = node.Left.Right.Balance;

                            node.Left = node.Left.RotateLeft();
                            node = node.RotateRight();

                            node.Balance = 0;
                            node.Left.Balance = (leftRightBalance == 1) ? -1 : 0;
                            node.Right.Balance = (leftRightBalance == -1) ? 1 : 0;
                        }
                        else if (node.Left.Balance == -1)
                        {
                            node = node.RotateRight();
                            node.Balance = 0;
                            node.Right.Balance = 0;
                        }
                        else if (node.Left.Balance == 0)
                        {
                            node = node.RotateRight();
                            node.Balance = 1;
                            node.Right.Balance = -1;

                            wasDeleted = false;
                        }
                    }
                    else if (node.Balance == 2)
                    {
                        if (node.Right.Balance == -1)
                        {
                            int rightLeftBalance = node.Right.Left.Balance;

                            node.Right = node.Right.RotateRight();
                            node = node.RotateLeft();

                            node.Balance = 0;
                            node.Left.Balance = (rightLeftBalance == 1) ? -1 : 0;
                            node.Right.Balance = (rightLeftBalance == -1) ? 1 : 0;
                        }
                        else if (node.Right.Balance == 1)
                        {
                            node = node.RotateLeft();
                            node.Balance = 0;
                            node.Left.Balance = 0;
                        }
                        else if (node.Right.Balance == 0)
                        {
                            node = node.RotateLeft();
                            node.Balance = -1;
                            node.Left.Balance = 1;

                            wasDeleted = false;
                        }
                    }
                }
                return node;
            }

            /// <summary>
            /// Finds the min.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private static Node FindMin(Node node)
            {
                while (node != null && node.Left != null)
                {
                    node = node.Left;
                }

                return node;
            }

            /// <summary>
            /// Finds the max.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private static Node FindMax(Node node)
            {
                while (node != null && node.Right != null)
                {
                    node = node.Right;
                }

                return node;
            }

            /// <summary>
            /// Searches the given subtree for the specified key.
            /// </summary>
            /// <param name="subtree">The subtree.</param>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            private Node SearchKey(Node subtree, TypeKey key)
            {
                if (subtree != null)
                {
                    if (this.keyComparer.Compare(key, subtree.Data.Key) < 0)
                    {
                        return this.SearchKey(subtree.Left, key);
                    }
                    else if (this.keyComparer.Compare(key, subtree.Data.Key) > 0)
                    {
                        return this.SearchKey(subtree.Right, key);
                    }
                    else
                    {
                        return subtree;
                    }
                }
                else
                {
                    return null;
                }
            }

            /// <summary>
            /// Precedes this instance.
            /// </summary>
            /// <returns></returns>
            private static Node Predecesor(Node node)
            {
                if (node.Left != null)
                {
                    return FindMax(node.Left);
                }
                else
                {
                    var p = node;
                    while (p.Parent != null && p.Parent.Left == p)
                    {
                        p = p.Parent;
                    }

                    return p.Parent;
                }
            }

            /// <summary>
            /// Succeeds this instance.
            /// </summary>
            /// <returns></returns>
            private static Node Successor(Node node)
            {
                if (node.Right != null)
                {
                    return FindMin(node.Right);
                }
                else
                {
                    var p = node;
                    while (p.Parent != null && p.Parent.Right == p)
                    {
                        p = p.Parent;
                    }

                    return p.Parent;
                }
            }

            private bool ContainsValue(Node node, TypeValue value)
            {
                if (node == null)
                {
                    return false;
                }

                if (this.valueComparer.Compare(node.Data.Value, value) == 0)
                {
                    return true;
                }

                return node.Range != null && node.Range.Exists(
                    (TypeValue match) =>
                    {
                        return this.valueComparer.Compare(match, value) == 0;
                    });
            }

            private IEnumerator<TypeValue> EnumerateKeyValues(TypeKey key)
            {
                var found = this.SearchKey(this.Root, key);
                if (found != null)
                {
                    yield return found.Data.Value;

                    if (found.Range != null)
                    {
                        foreach (var value in found.Range)
                        {
                            yield return value;
                        }
                    }
                }
                else
                {
                    yield break;
                }
            }

            /// <summary>
            /// in order traversal of our balanced binary tree.
            /// </summary>
            /// <param name="visitor">The visitor.</param>
            private void Visit(VisitNodeHandler<Node> visitor)
            {
                if (this.Root != null)
                {
                    this.Root.Visit(visitor, 0);
                }
            }

            #endregion

            /// <summary>
            /// node class
            /// </summary>
            /// <typeparam name="TElem">The type of the elem.</typeparam>
            private class Node
            {
                #region Properties

                public Node Left { get; set; }

                public Node Right { get; set; }

                public KeyValuePair<TypeKey, TypeValue> Data { get; set; }
                public List<TypeValue> Range { get; set; }

                public int Balance { get; set; }

#if TREE_WITH_PARENT_POINTERS
                public Node Parent { get; set; }
#endif

                #endregion

                #region Methods

                /// <summary>
                /// Rotates lefts this instance. 
                /// Assumes that this.Right != null
                /// </summary>
                /// <returns></returns>
                public Node RotateLeft()
                {
                    var right = this.Right;
                    Debug.Assert(this.Right != null);

                    this.Right = right.Left;

#if TREE_WITH_PARENT_POINTERS
                    var parent = this.Parent;
                    if (right.Left != null)
                    {
                        right.Left.Parent = this;
                    }
#endif
                    right.Left = this;

#if TREE_WITH_PARENT_POINTERS
                    this.Parent = right;
                    if (parent != null)
                    {
                        if (parent.Left == this)
                        {
                            parent.Left = right;
                        }
                        else
                        {
                            parent.Right = right;
                        }
                    }

                    right.Parent = parent;
#endif
                    return right;
                }

                /// <summary>
                /// RotateRights this instance. 
                /// Assumes that (this.Left != null)
                /// </summary>
                /// <returns></returns>
                public Node RotateRight()
                {
                    var left = this.Left;
                    Debug.Assert(this.Left != null);

                    this.Left = left.Right;

#if TREE_WITH_PARENT_POINTERS
                    var parent = this.Parent;
                    if (left.Right != null)
                    {
                        left.Right.Parent = this;
                    }
#endif

                    left.Right = this;

#if TREE_WITH_PARENT_POINTERS
                    this.Parent = left;
                    if (parent != null)
                    {
                        if (parent.Left == this)
                        {
                            parent.Left = left;
                        }
                        else
                        {
                            parent.Right = left;
                        }
                    }

                    left.Parent = parent;
#endif
                    return left;
                }

                /// <summary>
                /// Visits (in-order) this node with the specified visitor.
                /// </summary>
                /// <param name="visitor">The visitor.</param>
                /// <param name="level">The level.</param>
                public void Visit(VisitNodeHandler<Node> visitor, int level)
                {
                    if (visitor == null)
                    {
                        return;
                    }

                    if (this.Left != null)
                    {
                        this.Left.Visit(visitor, level + 1);
                    }

                    visitor(this, level);

                    if (this.Right != null)
                    {
                        this.Right.Visit(visitor, level + 1);
                    }
                }

                #endregion
            }
        }

        #endregion
    }    
}
