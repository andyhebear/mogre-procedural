/////////////////////////////////////////////////////////////////////
// File Name               : map.cs
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
    /// map implementation based on a self balanced binary AVL tree
    /// check http://code.google.com/p/self-balancing-avl-tree for a standalone implementation of an AVL tree including concatenate and split functionality
    /// </summary>
    public class map<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, ICollection<KeyValuePair<TKey, TValue>>, ICollection
    {
        #region Fields

        private IComparer<KeyValuePair<TKey, TValue>> keyvalueComparer;
        private AVLTree<KeyValuePair<TKey, TValue>> tree;

        #endregion

        #region C'tor

        public map() :
            this(null, new KeyValueComparer<TKey, TValue>(GetComparer<TKey>()))
        {            
        }

        public map(IEnumerable<KeyValuePair<TKey, TValue>> keysAndValues) :
            this(keysAndValues, new KeyValueComparer<TKey, TValue>(GetComparer<TKey>()))
        {
        }

        public map(IComparer<KeyValuePair<TKey, TValue>> keyvalueComparer)
            : this(null, keyvalueComparer)
        {
        }

        private map(IEnumerable<KeyValuePair<TKey, TValue>> keysAndValues, IComparer<KeyValuePair<TKey, TValue>> keyvalueComparer)
        {
            this.keyvalueComparer = keyvalueComparer;
            this.tree = new AVLTree<KeyValuePair<TKey, TValue>>(keysAndValues, keyvalueComparer);
        }        

        #endregion

        #region Enums

        /// <summary>
        /// When splitting a map, this enumeration determines where (and if) the split value will reside after splittingi.e. in the left map., in the right map or the split value if to not be included in the result.
        /// The split value, as the name suggests, is the value used to split the map into two maps: the left map containing all values less than the split value and the right part containing all those values greater than the split value
        /// </summary>
        public enum SplitOperationMode
        {
            /// <summary>
            /// Include the split value into the left map
            /// </summary>
            IncludeSplitValueToLeftSubtree,

            /// <summary>
            /// Include the split value into the right map
            /// </summary>
            IncludeSplitValueToRightSubtree,

            /// <summary>
            /// Do not include the split value to either left or right maps
            /// </summary>
            DoNotIncludeSplitValue
        }

        #endregion

        #region IDictionary<TKey, TValue>

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// If an element with the key provided already exists in the tree, it overrides the value associated with that key
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentException">An element with the same key already exists in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
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
            return this.tree.Contains(new KeyValuePair<TKey, TValue>(key, default(TValue)));
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TKey> Keys
        {
            get { return new KeysCollection(this); }
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        /// true if the element is successfully removed; otherwise, false.  This method also returns false if key was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        public bool Remove(TKey key)
        {
            return this.tree.Delete(new KeyValuePair<TKey, TValue>(key, default(TValue)));
        }

        /// <summary>
        /// Tries the get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>a boolean value indicating the success or failure in retrieving the value</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            KeyValuePair<TKey, TValue> kvp;
            if (this.tree.TryGetValue(new KeyValuePair<TKey, TValue>(key, default(TValue)), out kvp))
            {
                value = kvp.Value;
                return true;
            }

            value = default(TValue);
            return false;
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        public ICollection<TValue> Values
        {
            get { return new ValuesCollection(this); }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>The element with the specified key.</returns>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
        public TValue this[TKey key]
        {
            get
            {
                bool wasFound;
                TValue value;

                wasFound = this.TryGetValue(key, out value);
                if (wasFound)
                {
                    return value;
                }
                else
                {
                    throw new KeyNotFoundException("Could not find the specified key");
                }
            }

            set
            {
                this.tree.TrySetValue(new KeyValuePair<TKey, TValue>(key, value));                
            }
        }

        #endregion

        #region IDictionary

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IDictionary"/> object has a fixed size.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.IDictionary"/> object has a fixed size; otherwise, false.</returns>
        bool IDictionary.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        bool IDictionary.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the keys of the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        ICollection IDictionary.Keys
        {
            get { return new KeysCollection(this); }
        }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the <see cref="T:System.Collections.Generic.IDictionary`2"></see>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.ICollection`1"></see> containing the values in the object that implements <see cref="T:System.Collections.Generic.IDictionary`2"></see>.</returns>
        ICollection IDictionary.Values
        {
            get { return new ValuesCollection(this); }
        }

        /// <summary>
        /// Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>The element with the specified key.</returns>
        /// <exception cref="T:System.ArgumentNullException">key is null.</exception>
        /// <exception cref="T:System.ArgumentException">key must have the correct type.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
        object IDictionary.this[object key]
        {
            get
            {
                if (key is TKey || key == null)
                {
                    TValue value;
                    if (this.TryGetValue((TKey)key, out value))
                    {
                        return value;
                    }
                    else
                    {
                        throw new KeyNotFoundException("Could not find the specified key");
                    }
                }
                else
                {
                    if (key == null)
                    {
                        throw new ArgumentNullException("The specified key is null");
                    }
                    else
                    {
                        throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The key argument must be of type {0} and it is not", typeof(TKey)), "key");
                    }
                }
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <param name="key">The <see cref="T:System.Object"/> to use as the key of the element to add.</param>
        /// <param name="value">The <see cref="T:System.Object"/> to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"> the key is null. </exception>
        /// <exception cref="T:System.ArgumentException">the key and value must have correct type </exception>
        void IDictionary.Add(object key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("The specified key is null");
            }

            if (!(key is TKey))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The key argument must be of type {0}", typeof(TKey)), "key");
            }
            
            if (!(value is TValue))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value argument must be of type {0}", typeof(TValue)), "value");
            }

            this.Add((TKey)key, (TValue)value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        void IDictionary.Clear()
        {
            this.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IDictionary"/> object contains an element with the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary"/> object.</param>
        /// <returns>
        /// true if the <see cref="T:System.Collections.IDictionary"/> contains an element with the key; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="key"/> is null. </exception>
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

        /// <summary>
        /// Returns an <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionaryEnumerator"/> object for the <see cref="T:System.Collections.IDictionary"/> object.
        /// </returns>
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator(this.GetEnumerator());
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException">is null. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IDictionary"/> object is read-only.-or- The <see cref="T:System.Collections.IDictionary"/> has a fixed size. </exception>
        void IDictionary.Remove(object key)
        {
            if (key is TKey || key == null)
            {
                this.Remove((TKey)key);
            }
        }
                        
        #endregion
        
        #region ICollection<T> Members

        public void Clear()
        {
            this.tree.Clear();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.tree.Add(item);
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.tree.Contains(item);
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
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
            foreach (KeyValuePair<TKey, TValue> item in (ICollection<KeyValuePair<TKey, TValue>>)this)
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
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count
        {
            get
            {
                return this.tree.Count;
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.GetTreeEnumerator();
        }
        
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary)this).GetEnumerator();
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

        public IComparer<KeyValuePair<TKey, TValue>> Comparer
        {
            get
            {
                return this.keyvalueComparer;
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

        private IEnumerator<KeyValuePair<TKey, TValue>> GetTreeEnumerator()
        {
            return this.tree.ValuesCollection.GetEnumerator();
        }

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
            /// Initializes a new instance of the <see cref="map&lt;TKey, TValue&gt;.ReadOnlyCollection&lt;T&gt;"/> class.
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
            /// Copies the collection to the specified array beggining at the specified index.
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

        /// <summary>
        /// Internal class used to implement the collection of keys in our map
        /// </summary>
        private sealed class KeysCollection : ReadOnlyCollection<TKey>
        {
            #region Fields
            
            private IDictionary<TKey, TValue> dictionary;

            #endregion

            #region C'tor

            /// <summary>
            /// Initializes a new instance of the <see cref="map&lt;TKey, TValue&gt;.KeysCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The dictionary.</param>
            public KeysCollection(IDictionary<TKey, TValue> dictionary)
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
                foreach (KeyValuePair<TKey, TValue> pair in this.dictionary)
                {
                    yield return pair.Key;
                }
            }

            #endregion
        }

        /// <summary>
        /// Internal class used to implement the collection of values in our map
        /// </summary>
        private sealed class ValuesCollection : ReadOnlyCollection<TValue>
        {
            #region Fields

            private IDictionary<TKey, TValue> dictionary;
            
            #endregion

            #region C'tor

            /// <summary>
            /// Initializes a new instance of the <see cref="map&lt;TKey, TValue&gt;.ValuesCollection"/> class.
            /// </summary>
            /// <param name="dictionary">The dictionary.</param>
            public ValuesCollection(IDictionary<TKey, TValue> dictionary)
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
            /// Determines whether the map contains the specified value.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified key]; otherwise, <c>false</c>.
            /// </returns>
            public override bool Contains(TValue value)
            {
                IEqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
                foreach (TValue i in this)
                {
                    if (equalityComparer.Equals(i, value))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// Returns an enumerator that iterates through the collection.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
            /// </returns>
            public override IEnumerator<TValue> GetEnumerator()
            {
                foreach (KeyValuePair<TKey, TValue> pair in this.dictionary)
                {
                    yield return pair.Value;
                }
            }

            #endregion
        }

        /// <summary>
        /// Private class used to implement IDictionary.GetEnumerator. Wraps an IEnumerator<T> and exposes an IDictionaryEnumerator
        /// </summary>
        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private IEnumerator<KeyValuePair<TKey, TValue>> enumerator;

            /// <summary>
            /// Initializes a new instance of the <see cref="map&lt;TKey, TValue&gt;.DictionaryEnumerator"/> class.
            /// </summary>
            /// <param name="enumerator">The enumerator.</param>
            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
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
                    KeyValuePair<TKey, TValue> pair = this.enumerator.Current;
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
                    KeyValuePair<TKey, TValue> current = this.enumerator.Current;
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
                    KeyValuePair<TKey, TValue> current = this.enumerator.Current;
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

        private class KeyValueComparer<TypeKey, TypeValue> : IComparer<KeyValuePair<TypeKey, TypeValue>>
        {
            private IComparer<TypeKey> keyComparer;

            /// <summary>
            /// Initializes a new instance of the <see cref="map&lt;TKey, TValue&gt;.KeyValueComparer&lt;TypeKey, TypeValue&gt;"/> class.
            /// </summary>
            /// <param name="keyComparer">The key comparer.</param>
            public KeyValueComparer(IComparer<TypeKey> keyComparer)
            {
                this.keyComparer = keyComparer;
            }

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
            /// </returns>
            public int Compare(KeyValuePair<TypeKey, TypeValue> x, KeyValuePair<TypeKey, TypeValue> y)
            {
                return this.keyComparer.Compare(x.Key, y.Key);
            }

            /// <summary>
            /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
            /// </summary>
            /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
            /// <returns>
            ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (obj is KeyValueComparer<TypeKey, TypeValue>)
                {
                    return object.Equals(this.keyComparer, (obj as KeyValueComparer<TypeKey, TypeValue>).keyComparer);
                }
                else
                {
                    return false;
                }
            }

            /// <summary>
            /// Returns a hash code for this instance.
            /// </summary>
            /// <returns>
            /// A hash code for this instance
            /// </returns>
            public override int GetHashCode()
            {
                return this.keyComparer.GetHashCode();
            }
        }

        /// <summary>
        /// Dictionary class
        /// </summary>
        /// <typeparam name="T">The type of the data stored in the nodes</typeparam>
        private class AVLTree<T>
        {
            #region Fields

            private IComparer<T> comparer;

            private Node<T> Root { get; set; }

            #endregion

            #region Ctor

            /// <summary>
            /// Initializes a new instance of the <see cref="AVLTree&lt;T&gt;"/> class.
            /// </summary>
            public AVLTree()
                : this(null, GetComparer<T>())
            {
            }

            public AVLTree(IComparer<T> comparer)
                : this(null, comparer)
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="AVLTree&lt;T&gt;"/> class.
            /// </summary>
            /// <param name="elems">The elems.</param>
            public AVLTree(IEnumerable<T> elems, IComparer<T> comparer)
            {
                this.comparer = comparer;

                if (elems != null)
                {
                    foreach (var elem in elems)
                    {
                        this.Add(elem);
                    }
                }
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
            public bool Add(T arg)
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
            /// Deletes the specified arg.
            /// </summary>
            /// <param name="arg">The arg.</param>
            public bool Delete(T arg)
            {
                bool wasSuccessful = false;

                if (this.Root != null)
                {
                    bool wasDeleted = false;
                    this.Root = this.Delete(this.Root, arg, ref wasDeleted, ref wasSuccessful);

                    if (wasSuccessful)
                    {
                        this.Count--;
                    }
                }

                return wasSuccessful;
            }            

            /// <summary>
            /// Gets the min.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>
            public bool GetMin(out T value)
            {
                if (this.Root != null)
                {
                    var min = FindMin(this.Root);
                    if (min != null)
                    {
                        value = min.Data;
                        return true;
                    }
                }

                value = default(T);
                return false;
            }            

            /// <summary>
            /// Gets the max.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <returns></returns>        
            public bool GetMax(out T value)
            {
                if (this.Root != null)
                {
                    var max = FindMax(this.Root);
                    if (max != null)
                    {
                        value = max.Data;
                        return true;
                    }
                }

                value = default(T);
                return false;
            }

            /// <summary>
            /// Determines whether [contains] [the specified arg].
            /// </summary>
            /// <param name="arg">The arg.</param>
            /// <returns>
            ///   <c>true</c> if [contains] [the specified arg]; otherwise, <c>false</c>.
            /// </returns>
            public bool Contains(T arg)
            {
                return this.Search(this.Root, arg) != null;
            }

            /// <summary>
            /// Tries the get value.
            /// </summary>
            /// <param name="arg">The arg.</param>
            /// <param name="kvp">The KVP.</param>
            /// <returns></returns>
            public bool TryGetValue(T arg, out T kvp)
            {
                kvp = default(T);

                var nodeFound = this.Search(this.Root, arg);
                if (nodeFound != null)
                {
                    kvp = nodeFound.Data;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Tries to set the value.
            /// </summary>
            /// <param name="arg">The arg.</param>
            /// <param name="kvp">The KVP.</param>
            /// <returns></returns>
            public bool TrySetValue(T argAndvalue)
            {
                bool wasAdded = false;
                bool wasSuccessful = false;

                this.Root = this.Add(this.Root, argAndvalue, ref wasAdded, ref wasSuccessful);

                if (wasSuccessful)
                {
                    this.Count++;
                }

                return true;
            } 

            /// <summary>
            /// Deletes the min.
            /// </summary>
            public bool DeleteMin()
            {
                if (this.Root != null)
                {
                    bool wasDeleted = false, wasSuccessful = false;
                    this.Root = this.DeleteMin(this.Root, ref wasDeleted, ref wasSuccessful);

                    if (wasSuccessful)
                    {
                        this.Count--;
                    }

                    return wasSuccessful;
                }

                return false;
            }            

            /// <summary>
            /// Deletes the max.
            /// </summary>
            public bool DeleteMax()
            {
                if (this.Root != null)
                {
                    bool wasDeleted = false, wasSuccessful = false;
                    this.Root = this.DeleteMax(this.Root, ref wasDeleted, ref wasSuccessful);

                    if (wasSuccessful)
                    {
                        this.Count--;
                    }

                    return wasSuccessful;
                }

                return false;
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
            /// Gets the collection of values.
            /// </summary>
            public IEnumerable<T> ValuesCollection
            {
                get
                {
                    if (this.Root == null)
                    {
                        yield break;
                    }

                    var p = FindMin(this.Root);
                    while (p != null)
                    {
                        yield return p.Data;
                        p = Successor(p);
                    }
                }
            }

            /// <summary>
            /// Gets the collection of values (descending order)
            /// </summary>
            public IEnumerable<T> ValuesCollectionDescending
            {
                get
                {
                    if (this.Root == null)
                    {
                        yield break;
                    }

                    var p = FindMax(this.Root);
                    while (p != null)
                    {
                        yield return p.Data;
                        p = Predecesor(p);
                    }
                }
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
            /// Prints this instance.
            /// </summary>
            public void Print()
            {
                this.Visit((node, level) =>
                {
                    Console.Write(new string(' ', 2 * level));
                    Console.WriteLine("{0, 6}", node.Data);
                });
            }

            #endregion

            #region Private Methods
            
            /// <summary>
            /// Gets the height of the tree in log(n) time.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns>The height of the tree. Runs in O(log(n)) where n is the number of nodes in the tree </returns>
            private int GetHeightLogN(Node<T> node)
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
            private Node<T> Add(Node<T> elem, T data, ref bool wasAdded, ref bool wasSuccessful)
            {
                if (elem == null)
                {
                    elem = new Node<T> { Data = data, Left = null, Right = null, Balance = 0 };

                    wasAdded = true;
                    wasSuccessful = true;
                }
                else
                {
                    if (this.comparer.Compare(data, elem.Data) < 0)
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
                    else if (this.comparer.Compare(data, elem.Data) > 0)
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
                        elem.Data = data;
                    }
                }

                return elem;
            }

            /// <summary>
            /// Deletes the specified node.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <param name="arg">The arg.</param>
            /// <returns></returns>
            private Node<T> Delete(Node<T> node, T arg, ref bool wasDeleted, ref bool wasSuccessful)
            {
                int cmp = this.comparer.Compare(arg, node.Data);
                if (cmp < 0)
                {
                    if (node.Left != null)
                    {
                        node.Left = this.Delete(node.Left, arg, ref wasDeleted, ref wasSuccessful);

                        if (wasDeleted)
                        {
                            node.Balance++;
                        }
                    }
                }
                else if (cmp == 0)
                {
                    wasDeleted = true;
                    if (node.Left != null && node.Right != null)
                    {
                        var min = FindMin(node.Right);
                        T data = node.Data;
                        node.Data = min.Data;
                        min.Data = data;

                        wasDeleted = false;
                        node.Right = this.Delete(node.Right, data, ref wasDeleted, ref wasSuccessful);

                        if (wasDeleted)
                        {
                            node.Balance--;
                        }
                    }
                    else if (node.Left == null)
                    {
                        wasSuccessful = true;
                        return node.Right;
                    }
                    else
                    {
                        wasSuccessful = true;
                        return node.Left;
                    }
                }
                else
                {
                    if (node.Right != null)
                    {
                        node.Right = this.Delete(node.Right, arg, ref wasDeleted, ref wasSuccessful);
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

            /// <summary>
            /// Finds the min.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private static Node<T> FindMin(Node<T> node)
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
            private static Node<T> FindMax(Node<T> node)
            {
                while (node != null && node.Right != null)
                {
                    node = node.Right;
                }

                return node;
            }

            /// <summary>
            /// Searches the specified subtree.
            /// </summary>
            /// <param name="subtree">The subtree.</param>
            /// <param name="data">The data.</param>
            /// <returns></returns>
            private Node<T> Search(Node<T> subtree, T data)
            {
                if (subtree != null)
                {
                    if (this.comparer.Compare(data, subtree.Data) < 0)
                    {
                        return this.Search(subtree.Left, data);
                    }
                    else if (this.comparer.Compare(data, subtree.Data) > 0)
                    {
                        return this.Search(subtree.Right, data);
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
            /// Deletes the min.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private Node<T> DeleteMin(Node<T> node, ref bool wasDeleted, ref bool wasSuccessful)
            {
                if (node.Left == null)
                {
                    wasDeleted = true;
                    wasSuccessful = true;
                    return node.Right;
                }

                node.Left = this.DeleteMin(node.Left, ref wasDeleted, ref wasSuccessful);
                if (wasDeleted)
                {
                    node.Balance++;
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

            /// <summary>
            /// Deletes the min.
            /// </summary>
            /// <param name="node">The node.</param>
            /// <returns></returns>
            private Node<T> DeleteMax(Node<T> node, ref bool wasDeleted, ref bool wasSuccessful)
            {
                if (node.Right == null)
                {
                    wasDeleted = true;
                    wasSuccessful = true;
                    return node.Left;
                }

                node.Right = this.DeleteMax(node.Right, ref wasDeleted, ref wasSuccessful);
                if (wasDeleted)
                {
                    node.Balance--;
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

            /// <summary>
            /// Precedes this instance.
            /// </summary>
            /// <returns></returns>
            private static Node<T> Predecesor(Node<T> node)
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
            private static Node<T> Successor(Node<T> node)
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

            /// <summary>
            /// in order traversal of our balanced binary tree.
            /// </summary>
            /// <param name="visitor">The visitor.</param>
            private void Visit(VisitNodeHandler<Node<T>> visitor)
            {
                if (this.Root != null)
                {
                    this.Root.Visit(visitor, 0);
                }
            }

            #endregion

            #region Nested Classes
            
            /// <summary>
            /// node class
            /// </summary>
            /// <typeparam name="TElem">The type of the elem.</typeparam>
            private class Node<TElem>
            {
                #region Properties

                public Node<TElem> Left { get; set; }

                public Node<TElem> Right { get; set; }

                public TElem Data { get; set; }

                public int Balance { get; set; }

#if TREE_WITH_PARENT_POINTERS
                public Node<TElem> Parent { get; set; }
#endif

                #endregion

                #region Methods

                /// <summary>
                /// Rotates lefts this instance. 
                /// Assumes that this.Right != null
                /// </summary>
                /// <returns></returns>
                public Node<TElem> RotateLeft()
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
                public Node<TElem> RotateRight()
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
                public void Visit(VisitNodeHandler<Node<TElem>> visitor, int level)
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

            #endregion
        }

        #endregion
    }    
}