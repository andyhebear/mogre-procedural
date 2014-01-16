using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;

namespace Mogre_Procedural.std
{
    /// <summary>
    ///  like c++ std::multimap
    /// An implementation of a MultiMap, that is a Map which has many values
    /// for a given key.
    /// </summary>
    public class std_multimap<TKey, TValue> : ILookup<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {

        SortedDictionary<TKey, IList<TValue>> _buckets;// = new SortedDictionary<TKey, IList<TValue>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public std_multimap() {
            _buckets = new SortedDictionary<TKey, IList<TValue>>();
        }
        public std_multimap(IComparer<TKey> comparer) {
            _buckets = new SortedDictionary<TKey, IList<TValue>>(comparer);
        }

        public std_multimap(IDictionary<TKey, IList<TValue>> dictionary) {
            _buckets = new SortedDictionary<TKey, IList<TValue>>(dictionary);
        }

        public std_multimap(IDictionary<TKey, IList<TValue>> dictionary, IComparer<TKey> comparer) {
            _buckets = new SortedDictionary<TKey, IList<TValue>>(dictionary, comparer);
        }
        #region IDictionary<TKey,TValue> like Members

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            _buckets[key].Add(value);
            //_count++;
        }
        public void Add(TKey key, TValue[] value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            foreach (var v in value) {
                _buckets[key].Add(v);
                //_count++;
            }
        }
        public void Add(TKey key, IList<TValue> value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            foreach (var v in value) {
                _buckets[key].Add(v);
                //_count++;
            }
        }
        /// <summary>
        /// Determines whether the MultiMap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the MultiMap.</param>
        /// <returns>
        /// 	<c>true</c> if the MultiMap contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key) {
            return _buckets.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing the keys in the MultiMap
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<TKey> Keys {
            get { return _buckets.Keys; }
        }

        /// <summary>
        /// Removes the value with the specified key from the MultiMap.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(TKey key) {
            if (!_buckets.ContainsKey(key)) {
                return false;
            }
            //_count -= multimap[key].Count;
            bool found = _buckets.Remove(key);
            return found;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of value parameter.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out IList<TValue> value) {
            return _buckets.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the values in the MultiMap
        /// 所有的值列表
        /// </summary>
        /// <value>The values.</value>
        public IList<TValue> Values {
            get { return _buckets.Values.SelectMany(x => x).ToList(); }
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IList&lt;TValue&gt;"/> with the specified key.
        /// </summary>
        /// <value></value>
        public IList<TValue> this[TKey key] {
            get {
                return _buckets[key];
            }
        }

        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() {
            this._buckets.Clear();
            //_count = 0;
        }

        /// <summary>
        /// Gets the number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </returns>
        public int Count {
            get { return this._buckets.Count; }
        }
        public int KeyCount {
            get { return this._buckets.Count; }
        }
        /// <summary>
        ///     Number of total items currently in this buckets.
        /// </summary>
        //private int _count;
        /// <summary>
        /// Number of total items currently in this buckets.
        /// </summary>
        public int TotalCount {
            get {
                int count = 0;
                foreach (var v in this._buckets) {
                    count += v.Value.Count;
                }
                return count;
                //return this._count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly {
            get { return (this._buckets as IList).IsReadOnly; }
        }

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion

        #region ILookup<TKey,TValue> Members

        /// <summary>
        /// Determines whether a specified key exists in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </summary>
        /// <param name="key">The key to search for in the <see cref="T:System.Linq.ILookup`2"/>.</param>
        /// <returns>
        /// true if <paramref name="key"/> is in the <see cref="T:System.Linq.ILookup`2"/>; otherwise, false.
        /// </returns>
        public bool Contains(TKey key) {
            return this.ContainsKey(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IEnumerable&lt;TValue&gt;"/> with the specified key.
        /// </summary>
        /// <value></value>
        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] {
            get { return this[key]; }
        }

        #endregion

        #region IEnumerable<IGrouping<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator() {
            foreach (var key in this._buckets) {
                yield return new std_multimapgroup(key.Key, key.Value);
            }
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var key in this._buckets) {
                foreach (var value in key.Value) {
                    yield return new KeyValuePair<TKey, TValue>(key.Key, value);
                }
            }
        }

        private class std_multimapgroup : IGrouping<TKey, TValue>
        {
            #region IGrouping<TKey,TValue> Members

            public TKey Key {
                get;
                private set;
            }

            #endregion

            IList<TValue> values;

            /// <summary>
            /// Initializes a new instance of the MultiMapGroup class.
            /// </summary>
            public std_multimapgroup(TKey key, IList<TValue> value) {
                Key = key;
                values = value;
            }

            #region IEnumerable<TValue> Members

            public IEnumerator<TValue> GetEnumerator() {
                return values.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }



        #region c++

        public int begin() {
            return 0;
        }
        /// <summary>
        /// 返回当前KEY数  最后一个位置+1
        /// </summary>
        /// <returns></returns>
        public int end() {
            return this.KeyCount;// -1;
        }
        public bool empty() {
            return this.KeyCount == 0;
        }
        public int size() {
            return this.TotalCount;
        }
        public int max_size() {
            return int.MaxValue;
        }

        public void clear() {
            this.Clear();
        }
        public static void swap(ref std_multimap<TKey, TValue> _this, ref std_multimap<TKey, TValue> _other) {
            std_multimap<TKey, TValue> temp = _this;
            _this = _other;
            _other = temp;
        }

        public void insert(TKey key, TValue value) {
            this.Add(key, value);
        }
        public void insert(KeyValuePair<TKey, TValue> pair) {
            this.Add(pair.Key, pair.Value);
        }
        public void insert(std_pair<TKey, TValue> pair) {
            this.Add(pair.first, pair.second);
        }
        public void insert(uint pos, KeyValuePair<TKey, TValue> pair) {
            insert(pair);
        }
        public void insert(uint pos, std_pair<TKey, TValue> pair) {
            insert(pair);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="beginpos"></param>
        /// <param name="endpos">当前位置之前</param>
        public void insert(std_multimap<TKey, TValue> array, int beginpos, int beforeendpos) {

            KeyValuePair<TKey, IList<TValue>>[] pairs = new KeyValuePair<TKey, IList<TValue>>[array.KeyCount];
            array._buckets.CopyTo(pairs, 0);
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Add(pairs[i].Key, pairs[i].Value);
            }
        }
        public void insert(std_multimap<TKey, TValue> array) {
            KeyValuePair<TKey, IList<TValue>>[] pairs = new KeyValuePair<TKey, IList<TValue>>[array.KeyCount];
            array._buckets.CopyTo(pairs, 0);
            for (int i = 0; i < pairs.Length; i++) {
                this.Add(pairs[i].Key, pairs[i].Value);
            }
        }
        /// <summary>
        /// 第几个下标位置的KEY删除 
        /// 通常 pos=find("key");
        /// erase(pos);
        /// </summary>
        /// <param name="pos"></param>
        /// <return> returns the number of elements erased. </return>
        public int erase(uint pos) {
            TKey[] pairs = new TKey[this.KeyCount];
            this._buckets.Keys.CopyTo(pairs, 0);
            if (pos >= pairs.Length) return 0;
            int c = this._buckets[pairs[pos]].Count;
            this.Remove(pairs[pos]);
            return c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>returns the number of elements erased.</returns>
        public int erase(TKey key) {
            if (!this.ContainsKey(key)) return 0;
            int c = this._buckets[key].Count;
            this.Remove(key);
            return c;
        }
        public void erase(int beginpos, int beforeendpos) {
            TKey[] pairs = new TKey[this._buckets.Keys.Count];
            this._buckets.Keys.CopyTo(pairs, 0);
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Remove(pairs[i]);
            }
        }
        //
        public void emplace(TKey key, TValue value) {
            this.Add(key, value);
        }
        /// <summary>
        /// 原始是返回key比较器， 这里把所有键返回了
        /// </summary>
        /// <returns></returns>
        public TKey[] key_comp() {
            throw new Exception("返回值比较器异常,没有实现");
            //TKey[] keys = new TKey[this._buckets.Keys.Count];
            //this._buckets.Keys.CopyTo(keys, 0);
            //return keys;
        }
        //      std::multimap<char,int> mymultimap;

        //mymultimap.insert(std::make_pair('x',101));
        //mymultimap.insert(std::make_pair('y',202));
        //mymultimap.insert(std::make_pair('y',252));
        //mymultimap.insert(std::make_pair('z',303));

        //std::cout << "mymultimap contains:\n";

        //std::pair<char,int> highest = *mymultimap.rbegin();          // last element

        //std::multimap<char,int>::iterator it = mymultimap.begin();
        //do {
        //  std::cout << (*it).first << " => " << (*it).second << '\n';
        //} while ( mymultimap.value_comp()(*it++, highest) );

        //  Output:
        //mymultimap contains:
        //x => 101
        //y => 202
        //y => 252
        //z => 303
        /// <summary>
        /// 原始是返回value比较器
        /// 注意这里没有实现。【不要调用】
        /// </summary>
        /// <returns></returns>
        public TValue[] value_comp() {
            throw new Exception("返回值比较器异常,没有实现");
        }
        public KeyValuePair<TKey, IList<TValue>>[] get_allocator() {
            KeyValuePair<TKey, IList<TValue>>[] pairs = new KeyValuePair<TKey, IList<TValue>>[array.buckets.Count];
            this._buckets.CopyTo(pairs, 0);
            return pairs;
        }
        /// <summary>
        /// 查找索引位置
        /// </summary>
        /// <param name="key"></param>
        ///// <param name="pos"></param>
        /// <returns>没有找到返回-1</returns>
        public int find(TKey key) {
            int index = -1;
            foreach (var v in this._buckets.Keys) {
                index++;
                if (v.Equals(key)) {
                    return index;
                    break;
                }
            }
            return -1;

        }
        /// <summary>
        /// 查询key对应的值的个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns>值的个数</returns>
        public int count(TKey key) {
            if (this._buckets.ContainsKey(key)) {
                return this._buckets[key].Count;
            }
            return 0; ;
        }

        //public int lower_bound(TKey key) {
        //    return find(key);
        //}
        //public int upper_bound(TKey key) {
        //    return find(key) + 1;
        //}
        public std_pair<TKey, IList<TValue>> lower_bound(TKey key) {
            std_pair<TKey, IList<TValue>> sp = null;
            if (this._buckets.ContainsKey(key)) {
                sp = new std_pair<TKey, IList<TValue>>(key, this._buckets[key]);
            }
            return sp;
        }
        public std_pair<TKey, IList<TValue>> upper_bound(TKey key) {
            std_pair<TKey, IList<TValue>> sp = null;
            if (this._buckets.ContainsKey(key)) {
                bool find_pre = false;
                foreach (var v in this._buckets.Keys) {
                    if (find_pre) {
                        sp = new std_pair<TKey, IList<TValue>>(v, this._buckets[v]);
                        break;
                    }
                    if (v.Equals(key)) {
                        find_pre = true;
                    }
                }
            }
            return sp;
        }
        public std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> equal_range(TKey key) {
            //return lower bound  and up bound
            if (!this.ContainsKey(key)) {
                return null;
            }
            //KeyValuePair<TKey, TValue> select =new KeyValuePair<TKey,TValue>(key, base[key]);
            KeyValuePair<TKey, TValue> first = new KeyValuePair<TKey, TValue>();
            KeyValuePair<TKey, TValue> second = new KeyValuePair<TKey, TValue>();
            bool find_first = false;
            bool find_second = false;
            //查找下一个
            IEnumerator<KeyValuePair<TKey, TValue>> et = this.GetEnumerator();
            //if (et.Current.Key.Equals(key)) {
            //    first = et.Current;
            //    find_first = true;
            //}
            while (et.MoveNext()) {
                if (find_first) {
                    second = et.Current;
                    find_second = true;
                    break;
                }
                else {
                    if (et.Current.Key.Equals(key)) {
                        first = et.Current;
                        find_first = true;
                    }
                }
            }

            std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> range = new std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>>(
                find_first ? new std_pair<TKey, TValue>(first.Key, first.Value) : null,
                find_second ? new std_pair<TKey, TValue>(second.Key, second.Value) : null);
            return range;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IList<TValue> at(int pos) {
            int index = 0;
            foreach (var v in _buckets.Keys) {
                if (index == pos) {
                    return _buckets[v];
                }
                index++;
            }
            return null;
        }
        /// <summary>
        /// 增加值
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        public void at(int pos, TValue @value) {
            int index = 0;
            foreach (var v in _buckets.Keys) {
                if (index == pos) {
                    _buckets[v].Add(@value);
                    break;
                }
                index++;
            }
        }
        #endregion


    }

    /// <summary>
    /// like c++ std::unordered_multimap
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class std_unordered_multimap<TKey, TValue> : ILookup<TKey, TValue>, IEnumerable<KeyValuePair<TKey, TValue>>
    {

        Dictionary<TKey, IList<TValue>> _buckets;// = new SortedDictionary<TKey, IList<TValue>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiMap&lt;TKey, TValue&gt;"/> class.
        /// </summary>
        public std_unordered_multimap() {
            _buckets = new Dictionary<TKey, IList<TValue>>();
        }
        public std_unordered_multimap(IEqualityComparer<TKey> comparer) {
            _buckets = new Dictionary<TKey, IList<TValue>>(comparer);
        }

        public std_unordered_multimap(IDictionary<TKey, IList<TValue>> dictionary) {
            _buckets = new Dictionary<TKey, IList<TValue>>(dictionary);
        }

        public std_unordered_multimap(IDictionary<TKey, IList<TValue>> dictionary, IEqualityComparer<TKey> comparer) {
            _buckets = new Dictionary<TKey, IList<TValue>>(dictionary, comparer);
        }
        #region IDictionary<TKey,TValue> like Members

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(TKey key, TValue value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            _buckets[key].Add(value);
            //_count++;
        }
        public void Add(TKey key, TValue[] value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            foreach (var v in value) {
                _buckets[key].Add(v);
                //_count++;
            }
        }
        public void Add(TKey key, IList<TValue> value) {
            if (!_buckets.ContainsKey(key)) {
                _buckets.Add(key, new List<TValue>());
            }
            foreach (var v in value) {
                _buckets[key].Add(v);
                //_count++;
            }
        }
        /// <summary>
        /// Determines whether the MultiMap contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the MultiMap.</param>
        /// <returns>
        /// 	<c>true</c> if the MultiMap contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(TKey key) {
            return _buckets.ContainsKey(key);
        }

        /// <summary>
        /// Gets a collection containing the keys in the MultiMap
        /// </summary>
        /// <value>The keys.</value>
        public ICollection<TKey> Keys {
            get { return _buckets.Keys; }
        }

        /// <summary>
        /// Removes the value with the specified key from the MultiMap.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(TKey key) {
            if (!_buckets.ContainsKey(key)) {
                return false;
            }
            //_count -= multimap[key].Count;
            bool found = _buckets.Remove(key);
            return found;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise, the default value for the type of value parameter.</param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out IList<TValue> value) {
            return _buckets.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets a collection containing the values in the MultiMap
        /// 所有的值列表
        /// </summary>
        /// <value>The values.</value>
        public IList<TValue> Values {
            get { return _buckets.Values.SelectMany(x => x).ToList(); }
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IList&lt;TValue&gt;"/> with the specified key.
        /// </summary>
        /// <value></value>
        public IList<TValue> this[TKey key] {
            get {
                return _buckets[key];
            }
        }

        #endregion

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear() {
            this._buckets.Clear();
            //_count = 0;
        }

        /// <summary>
        /// Gets the number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The number of key/value collection pairs in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </returns>
        public int Count {
            get { return this._buckets.Count; }
        }
        public int KeyCount {
            get { return this._buckets.Count; }
        }
        /// <summary>
        ///     Number of total items currently in this buckets.
        /// </summary>
        //private int _count;
        /// <summary>
        /// Number of total items currently in this buckets.
        /// </summary>
        public int TotalCount {
            get {
                int count = 0;
                foreach (var v in this._buckets) {
                    count += v.Value.Count;
                }
                return count;
                //return this._count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly {
            get { return (this._buckets as IList).IsReadOnly; }
        }

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        #endregion

        #region ILookup<TKey,TValue> Members

        /// <summary>
        /// Determines whether a specified key exists in the <see cref="T:System.Linq.ILookup`2"/>.
        /// </summary>
        /// <param name="key">The key to search for in the <see cref="T:System.Linq.ILookup`2"/>.</param>
        /// <returns>
        /// true if <paramref name="key"/> is in the <see cref="T:System.Linq.ILookup`2"/>; otherwise, false.
        /// </returns>
        public bool Contains(TKey key) {
            return this.ContainsKey(key);
        }

        /// <summary>
        /// Gets the <see cref="System.Collections.Generic.IEnumerable&lt;TValue&gt;"/> with the specified key.
        /// </summary>
        /// <value></value>
        IEnumerable<TValue> ILookup<TKey, TValue>.this[TKey key] {
            get { return this[key]; }
        }

        #endregion

        #region IEnumerable<IGrouping<TKey,TValue>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        IEnumerator<IGrouping<TKey, TValue>> IEnumerable<IGrouping<TKey, TValue>>.GetEnumerator() {
            foreach (var key in this._buckets) {
                yield return new std_multimapgroup(key.Key, key.Value);
            }
        }

        #endregion

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() {
            foreach (var key in this._buckets) {
                foreach (var value in key.Value) {
                    yield return new KeyValuePair<TKey, TValue>(key.Key, value);
                }
            }
        }

        private class std_multimapgroup : IGrouping<TKey, TValue>
        {
            #region IGrouping<TKey,TValue> Members

            public TKey Key {
                get;
                private set;
            }

            #endregion

            IList<TValue> values;

            /// <summary>
            /// Initializes a new instance of the MultiMapGroup class.
            /// </summary>
            public std_multimapgroup(TKey key, IList<TValue> value) {
                Key = key;
                values = value;
            }

            #region IEnumerable<TValue> Members

            public IEnumerator<TValue> GetEnumerator() {
                return values.GetEnumerator();
            }

            #endregion

            #region IEnumerable Members

            IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator();
            }

            #endregion
        }



        #region c++

        public int begin() {
            return 0;
        }
        /// <summary>
        /// 返回当前KEY数  最后一个位置+1
        /// </summary>
        /// <returns></returns>
        public int end() {
            return this.KeyCount;// -1;
        }
        public bool empty() {
            return this.KeyCount == 0;
        }
        public int size() {
            return this.TotalCount;
        }
        public int max_size() {
            return int.MaxValue;
        }

        public void clear() {
            this.Clear();
        }
        public static void swap(ref std_unordered_multimap<TKey, TValue> _this, ref std_unordered_multimap<TKey, TValue> _other) {
            std_unordered_multimap<TKey, TValue> temp = _this;
            _this = _other;
            _other = temp;
        }

        public void insert(TKey key, TValue value) {
            this.Add(key, value);
        }
        public void insert(KeyValuePair<TKey, TValue> pair) {
            this.Add(pair.Key, pair.Value);
        }
        public void insert(std_pair<TKey, TValue> pair) {
            this.Add(pair.first, pair.second);
        }
        public void insert(uint pos, KeyValuePair<TKey, TValue> pair) {
            insert(pair);
        }
        public void insert(uint pos, std_pair<TKey, TValue> pair) {
            insert(pair);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="beginpos"></param>
        /// <param name="endpos">当前位置之前</param>
        public void insert(std_unordered_multimap<TKey, TValue> array, int beginpos, int beforeendpos) {

           // KeyValuePair<TKey, IList<TValue>>[] pairs = new KeyValuePair<TKey, IList<TValue>>[array.KeyCount];
            //array._buckets.CopyTo(pairs, 0);
            KeyValuePair<TKey, IList<TValue>>[] pairs = array.get_allocator();
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Add(pairs[i].Key, pairs[i].Value);
            }
        }
        public void insert(std_unordered_multimap<TKey, TValue> array) {
            //KeyValuePair<TKey, IList<TValue>>[] pairs = new KeyValuePair<TKey, IList<TValue>>[array.KeyCount];
            //array._buckets.CopyTo(pairs, 0);
            KeyValuePair<TKey, IList<TValue>>[] pairs = array.get_allocator();
            for (int i = 0; i < pairs.Length; i++) {
                this.Add(pairs[i].Key, pairs[i].Value);
            }
        }
        /// <summary>
        /// 第几个下标位置的KEY删除 
        /// 通常 pos=find("key");
        /// erase(pos);
        /// </summary>
        /// <param name="pos"></param>
        /// <return> returns the number of elements erased. </return>
        public int erase(uint pos) {
            TKey[] pairs = new TKey[this.KeyCount];
            this._buckets.Keys.CopyTo(pairs, 0);
            if (pos >= pairs.Length) return 0;
            int c = this._buckets[pairs[pos]].Count;
            this.Remove(pairs[pos]);
            return c;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>returns the number of elements erased.</returns>
        public int erase(TKey key) {
            if (!this.ContainsKey(key)) return 0;
            int c = this._buckets[key].Count;
            this.Remove(key);
            return c;
        }
        public void erase(int beginpos, int beforeendpos) {
            TKey[] pairs = new TKey[this._buckets.Keys.Count];
            this._buckets.Keys.CopyTo(pairs, 0);
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Remove(pairs[i]);
            }
        }
        //
        public void emplace(TKey key, TValue value) {
            this.Add(key, value);
        }
        /// <summary>
        /// 原始是返回key比较器， 这里把所有键返回了
        /// </summary>
        /// <returns></returns>
        public TKey[] key_comp() {
            throw new Exception("返回值比较器异常,没有实现");
            //TKey[] keys = new TKey[this._buckets.Keys.Count];
            //this._buckets.Keys.CopyTo(keys, 0);
            //return keys;
        }
        //      std::multimap<char,int> mymultimap;

        //mymultimap.insert(std::make_pair('x',101));
        //mymultimap.insert(std::make_pair('y',202));
        //mymultimap.insert(std::make_pair('y',252));
        //mymultimap.insert(std::make_pair('z',303));

        //std::cout << "mymultimap contains:\n";

        //std::pair<char,int> highest = *mymultimap.rbegin();          // last element

        //std::multimap<char,int>::iterator it = mymultimap.begin();
        //do {
        //  std::cout << (*it).first << " => " << (*it).second << '\n';
        //} while ( mymultimap.value_comp()(*it++, highest) );

        //  Output:
        //mymultimap contains:
        //x => 101
        //y => 202
        //y => 252
        //z => 303
        /// <summary>
        /// 原始是返回value比较器
        /// 注意这里没有实现。【不要调用】
        /// </summary>
        /// <returns></returns>
        public TValue[] value_comp() {
            throw new Exception("返回值比较器异常,没有实现");
        }
        public KeyValuePair<TKey, IList<TValue>>[] get_allocator() {
            List<KeyValuePair<TKey, IList<TValue>>> pairs = new  List<KeyValuePair<TKey,IList<TValue>>>();
            foreach (var v in this._buckets) {
                pairs.Add(v);
            }
            return pairs.ToArray();
        }
        /// <summary>
        /// 查找索引位置
        /// </summary>
        /// <param name="key"></param>
        ///// <param name="pos"></param>
        /// <returns>没有找到返回-1</returns>
        public int find(TKey key) {
            int index = -1;
            foreach (var v in this._buckets.Keys) {
                index++;
                if (v.Equals(key)) {
                    return index;
                    break;
                }
            }
            return -1;

        }
        /// <summary>
        /// 查询key对应的值的个数
        /// </summary>
        /// <param name="key"></param>
        /// <returns>值的个数</returns>
        public int count(TKey key) {
            if (this._buckets.ContainsKey(key)) {
                return this._buckets[key].Count;
            }
            return 0; ;
        }

        //public int lower_bound(TKey key) {
        //    return find(key);
        //}
        //public int upper_bound(TKey key) {
        //    return find(key) + 1;
        //}
        public std_pair<TKey, IList<TValue>> lower_bound(TKey key) {
            std_pair<TKey, IList<TValue>> sp = null;
            if (this._buckets.ContainsKey(key)) {
                sp = new std_pair<TKey, IList<TValue>>(key, this._buckets[key]);
            }
            return sp;
        }
        public std_pair<TKey, IList<TValue>> upper_bound(TKey key) {
            std_pair<TKey, IList<TValue>> sp = null;
            if (this._buckets.ContainsKey(key)) {
                bool find_pre = false;
                foreach (var v in this._buckets.Keys) {
                    if (find_pre) {
                        sp = new std_pair<TKey, IList<TValue>>(v, this._buckets[v]);
                        break;
                    }
                    if (v.Equals(key)) {
                        find_pre = true;
                    }
                }
            }
            return sp;
        }
        public std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> equal_range(TKey key) {
            //return lower bound  and up bound
            if (!this.ContainsKey(key)) {
                return null;
            }
            //KeyValuePair<TKey, TValue> select =new KeyValuePair<TKey,TValue>(key, base[key]);
            KeyValuePair<TKey, TValue> first = new KeyValuePair<TKey, TValue>();
            KeyValuePair<TKey, TValue> second = new KeyValuePair<TKey, TValue>();
            bool find_first = false;
            bool find_second = false;
            //查找下一个
            IEnumerator<KeyValuePair<TKey, TValue>> et = this.GetEnumerator();
            //if (et.Current.Key.Equals(key)) {
            //    first = et.Current;
            //    find_first = true;
            //}
            while (et.MoveNext()) {
                if (find_first) {
                    second = et.Current;
                    find_second = true;
                    break;
                }
                else {
                    if (et.Current.Key.Equals(key)) {
                        first = et.Current;
                        find_first = true;
                    }
                }
            }

            std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> range = new std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>>(
                find_first ? new std_pair<TKey, TValue>(first.Key, first.Value) : null,
                find_second ? new std_pair<TKey, TValue>(second.Key, second.Value) : null);
            return range;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public IList<TValue> at(int pos) {
            int index = 0;
            foreach (var v in _buckets.Keys) {
                if (index == pos) {
                    return _buckets[v];
                }
                index++;
            }
            return null;
        }
        /// <summary>
        /// 增加值
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        public void at(int pos, TValue @value) {
            int index = 0;
            foreach (var v in _buckets.Keys) {
                if (index == pos) {
                    _buckets[v].Add(@value);
                    break;
                }
                index++;
            }
        }
        #endregion


    }

    
    
    ///// <summary>
    ///// like c++ std::multimap 相当于Dictionary<TKey,List<TValue>>，该类在C#中不存在的，也需要自己实现
    ///// </summary>
    ///// <summary>
    /////     The MultiMap is a C# conversion of the std::buckets container from the C++ 
    /////     standard library.  
    ///// </summary>
    ///// <remarks>
    /////     A buckets allows multiple values per key, unlike IDictionary<TKey, TValue> which only allows
    /////     unique keys and only a single value per key.  Multiple values assigned to the same
    /////     key are placed in a "bucket", which in this case is a List<TValue>.
    /////     <p/>
    /////     An example of values in a buckets would look like this:
    /////     Key     Value
    /////     "a"     "Alan"
    /////     "a"     "Adam"
    /////     "b"     "Brien"
    /////     "c"     "Chris"
    /////     "c"     "Carl"
    /////     etc
    /////     <p/>
    /////     Currently, enumeration is the only way to iterate through the values, which is
    /////     more pratical in terms of how the MultiMap works internally anyway.  Intial testing showed
    /////     that inserting and iterating through 100,000 items, the Inserts took ~260ms and a full
    /////     enumeration of them all (with unboxing of the value type stored in the buckets) took between 16-30ms.
    ///// </remarks>
    //public class std_multimap<TKey, TValue>
    //    : IDictionary, IDictionary<TKey, List<TValue>>, IEnumerable<TValue>, IEnumerable<List<TValue>>
    //{
    //    #region Fields

    //    private readonly SortedDictionary<TKey, List<TValue>> _buckets;

    //    /// <summary>
    //    ///     Number of total items currently in this buckets.
    //    /// </summary>
    //    private int _count;

    //    #endregion

    //    #region Constructors

    //    /// <summary>
    //    ///     Default constructor.
    //    /// </summary>
    //    public std_multimap() {
    //        this._buckets = new SortedDictionary<TKey, List<TValue>>();
    //    }

    //    public std_multimap(IComparer<TKey> comparer) {
    //        this._buckets = new SortedDictionary<TKey, List<TValue>>(comparer);
    //    }

    //    #endregion

    //    #region Instance Properties

    //    public int Count {
    //        get {
    //            return this._buckets.Count;
    //        }
    //    }

    //    /// <summary>
    //    ///		Gets the number of keys in this map.
    //    /// </summary>
    //    public int KeyCount {
    //        get {
    //            return this._buckets.Count;
    //        }
    //    }

    //    public int TotalCount {
    //        get {
    //            return this._count;
    //        }
    //    }

    //    #endregion

    //    #region Instance Methods

    //    /// <summary>
    //    ///     Inserts a value into a bucket that is specified by the
    //    ///     key.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="val"></param>
    //    public void Add(TKey key, TValue value) {
    //        List<TValue> container;

    //        if (!this._buckets.ContainsKey(key)) {
    //            container = new List<TValue>();
    //            this._buckets.Add(key, container);
    //        }
    //        else {
    //            container = this._buckets[key];
    //        }

    //        // TODO: Doing the contains check is extremely slow, so for now duplicate items are allowed
    //        //if (!container.Contains(value))
    //        //{
    //        container.Add(value);
    //        this._count++;
    //        //}
    //    }
    //    public void Add(TKey key, TValue[] values) {
    //        List<TValue> container;
    //        if (values == null) return;
    //        if (!this._buckets.ContainsKey(key)) {
    //            container = new List<TValue>();
    //            this._buckets.Add(key, container);
    //        }
    //        else {
    //            container = this._buckets[key];
    //        }
    //        container.AddRange(values);
    //        this._count += values.Length;
    //    }
    //    private void Add(TKey key, IList<TValue> value) {
    //        List<TValue> container;

    //        if (!this._buckets.ContainsKey(key)) {
    //            container = new List<TValue>();
    //            this._buckets.Add(key, container);
    //        }
    //        else {
    //            container = this._buckets[key];
    //        }

    //        foreach (TValue i in value) {
    //            // TODO: Doing the contains check is extremely slow, so for now duplicate items are allowed
    //            //if (!container.Contains(i)) {
    //            container.Add(i);
    //            this._count++;
    //            //}
    //        }
    //    }
    //    /// <summary>
    //    ///     Gets the count of objects mapped to the specified key.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public int BucketCount(TKey key) {
    //        if (this._buckets.ContainsKey(key)) {
    //            return this._buckets[key].Count;
    //        }

    //        return 0;
    //    }

    //    public void Clear() {
    //        this._buckets.Clear();
    //        this._count = 0;
    //    }

    //    public void Clear(TKey key) {
    //        if (this._buckets.ContainsKey(key)) {
    //            this._count -= this._buckets[key].Count;
    //        }
    //        this._buckets[key].Clear();
    //    }

    //    /// <summary>
    //    ///     Given a key, Find will return an IEnumerator that allows
    //    ///     you to iterate over all items in the bucket associated
    //    ///     with the key.
    //    /// </summary>
    //    /// <param name="key">Key for look for.</param>
    //    /// <returns>IEnumerator to go through the items assigned to the key.</returns>
    //    public IEnumerator<TValue> Find(TKey key) {
    //        if (this._buckets.ContainsKey(key)) {
    //            return this._buckets[key].GetEnumerator();
    //            //int length = buckets[key].Count;
    //            //IList<TValue> bucket = buckets[key];
    //            //for (int i = 0; i < length; i++)
    //            //{
    //            //    yield return bucket[i];
    //            //}
    //        }
    //        return null;
    //    }

    //    public List<TValue> FindBucket(TKey key) {
    //        if (!this._buckets.ContainsKey(key)) {
    //            return null;
    //        }
    //        return this._buckets[key];
    //    }

    //    /// <summary>
    //    ///     Given a key, FindFirst will return the first item in the bucket
    //    ///     associated with the key.
    //    /// </summary>
    //    /// <param name="key">Key to look for.</param>
    //    public object FindFirst(TKey key) {
    //        if (!this._buckets.ContainsKey(key)) {
    //            return null;
    //        }
    //        else {
    //            return (this._buckets[key])[0];
    //        }
    //    }

    //    public IEnumerator<KeyValuePair<TKey, List<TValue>>> GetBucketsEnumerator() {
    //        foreach (var item in this._buckets) {
    //            yield return item;
    //        }
    //    }



    //    #endregion

    //    #region IDictionary Members

    //    void IDictionary.Add(object key, object value) {
    //        if (key is TKey & value is IList<TValue>) {
    //            Add((TKey)key, (IList<TValue>)value);
    //        }
    //    }

    //    void IDictionary.Clear() {
    //        Clear();
    //    }

    //    bool IDictionary.Contains(object key) {
    //        if (key is TKey) {
    //            return ContainsKey((TKey)key);
    //        }
    //        return false;
    //    }

    //    IDictionaryEnumerator IDictionary.GetEnumerator() {
    //        throw new NotImplementedException();
    //    }

    //    bool IDictionary.IsFixedSize {
    //        get {
    //            return (this._buckets as IDictionary).IsFixedSize;
    //        }
    //    }

    //    bool IDictionary.IsReadOnly {
    //        get {
    //            return (this._buckets as IDictionary).IsReadOnly;
    //        }
    //    }

    //    ICollection IDictionary.Keys {
    //        get {
    //            return this._buckets.Keys;
    //        }
    //    }

    //    void IDictionary.Remove(object key) {
    //        if (key is TKey) {
    //            Remove((TKey)key);
    //        }
    //    }

    //    ICollection IDictionary.Values {
    //        get {
    //            return this._buckets.Values;
    //        }
    //    }

    //    object IDictionary.this[object key] {
    //        get {
    //            if (key is TKey) {
    //                return this[(TKey)key];
    //            }
    //            return null;
    //        }
    //        set {
    //            if (value is IList<TValue>) {
    //                this[(TKey)key] = (List<TValue>)value;
    //            }
    //            else {
    //                throw new ArgumentException("The key must be of type " + typeof(List<TValue>).ToString(), "value");
    //            }
    //        }
    //    }

    //    void ICollection.CopyTo(Array array, int index) {
    //        throw new NotImplementedException();
    //    }

    //    int ICollection.Count {
    //        get {
    //            return this._count;
    //        }
    //    }

    //    bool ICollection.IsSynchronized {
    //        get {
    //            return (this._buckets as ICollection).IsSynchronized;
    //        }
    //    }

    //    object ICollection.SyncRoot {
    //        get {
    //            return (this._buckets as ICollection).SyncRoot;
    //        }
    //    }

    //    #endregion

    //    #region IDictionary<TKey,List<TValue>> Members

    //    void IDictionary<TKey, List<TValue>>.Add(TKey key, List<TValue> value) {
    //        Add(key, value);
    //    }

    //    public bool ContainsKey(TKey key) {
    //        return this._buckets.ContainsKey(key);
    //    }

    //    public ICollection<TKey> Keys {
    //        get {
    //            return this._buckets.Keys;
    //        }
    //    }

    //    public bool Remove(TKey key) {
    //        bool removed = this._buckets.Remove(key);
    //        if (removed) {
    //            this._count--;
    //            return true;
    //        }
    //        return false;
    //    }

    //    public bool TryGetValue(TKey key, out List<TValue> value) {
    //        List<TValue> tvalue;
    //        this._buckets.TryGetValue(key, out tvalue);
    //        value = tvalue;
    //        if (tvalue == null) {
    //            return false;
    //        }
    //        return true;
    //    }

    //    public ICollection<List<TValue>> Values {
    //        get {
    //            return this._buckets.Values;
    //        }
    //    }

    //    public List<TValue> this[TKey key] {
    //        get {
    //            return this._buckets[key];
    //        }
    //        set {
    //            this._buckets[key] = value;
    //        }
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //    void ICollection<KeyValuePair<TKey, List<TValue>>>.Add(KeyValuePair<TKey, List<TValue>> item) {
    //        Add(item.Key, item.Value);
    //    }

    //    void ICollection<KeyValuePair<TKey, List<TValue>>>.Clear() {
    //        Clear();
    //    }

    //    bool ICollection<KeyValuePair<TKey, List<TValue>>>.Contains(KeyValuePair<TKey, List<TValue>> item) {
    //        return this._buckets.ContainsKey(item.Key);
    //    }

    //    void ICollection<KeyValuePair<TKey, List<TValue>>>.CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex) {
    //        throw new NotImplementedException();
    //    }

    //    int ICollection<KeyValuePair<TKey, List<TValue>>>.Count {
    //        get {
    //            return Count;
    //        }
    //    }

    //    bool ICollection<KeyValuePair<TKey, List<TValue>>>.IsReadOnly {
    //        get {
    //            return (this._buckets as IDictionary).IsReadOnly;
    //        }
    //    }

    //    bool ICollection<KeyValuePair<TKey, List<TValue>>>.Remove(KeyValuePair<TKey, List<TValue>> item) {
    //        return Remove(item.Key);
    //    }

    //    IEnumerator<KeyValuePair<TKey, List<TValue>>> IEnumerable<KeyValuePair<TKey, List<TValue>>>.GetEnumerator() {
    //        return this._buckets.GetEnumerator();
    //    }

    //    #endregion

    //    #region IEnumerable<List<TValue>> Members

    //    IEnumerator<List<TValue>> IEnumerable<List<TValue>>.GetEnumerator() {
    //        foreach (var item in this._buckets) {
    //            yield return item.Value;
    //        }
    //    }

    //    #endregion

    //    #region IEnumerable<TValue> Members

    //    public IEnumerator<TValue> GetEnumerator() {
    //        foreach (IList<TValue> item in this._buckets.Values) {
    //            int length = item.Count;
    //            for (int i = 0; i < length; i++) {
    //                yield return item[i];
    //            }
    //        }
    //    }

    //    #endregion


    //    public int begin() {
    //        return 0;
    //    }
    //    /// <summary>
    //    /// 返回的是当中个数 比最后的位置大1
    //    /// </summary>
    //    /// <returns></returns>
    //    public int end() {
    //        return this.KeyCount;// -1;
    //    }
    //    public bool empty() {
    //        return this.KeyCount == 0;
    //    }
    //    public int size() {
    //        return this.KeyCount;
    //    }
    //    public int max_size() {
    //        return int.MaxValue;
    //    }

    //    public void clear() {
    //        this.Clear();
    //    }
    //    public static void swap(ref std_multimap<TKey, TValue> _this, ref std_multimap<TKey, TValue> _other) {
    //        std_multimap<TKey, TValue> temp = _this;
    //        _this = _other;
    //        _other = temp;
    //    }

    //    public void insert(TKey key, TValue value) {
    //        this.Add(key, value);
    //    }
    //    public void insert(KeyValuePair<TKey, TValue> pair) {
    //        this.Add(pair.Key, pair.Value);
    //    }
    //    public void insert(std_pair<TKey, TValue> pair) {
    //        this.Add(pair.first, pair.second);
    //    }
    //    public void insert(uint pos, KeyValuePair<TKey, TValue> pair) {
    //        insert(pair);
    //    }
    //    public void insert(uint pos, std_pair<TKey, TValue> pair) {
    //        insert(pair);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="array"></param>
    //    /// <param name="beginpos"></param>
    //    /// <param name="endpos">当前位置之前</param>
    //    public void insert(std_multimap<TKey, TValue> array, int beginpos, int beforeendpos) {
    //        //System.Collections.IEnumerator er = array.buckets.Keys.GetEnumerator();

    //        //int index = 0;
    //        //bool find_first=false;
    //        //bool find_end=false;
    //        //while (er.MoveNext()) {
    //        //    if (beginpos == index) {
    //        //        find_first = true;
    //        //    }
    //        //    if (beforeendpos == index) {
    //        //        find_end = true;
    //        //    }
    //        //    if (find_first && !find_end) {
    //        //        foreach (var v in array.buckets[(TKey)er.Current]) {
    //        //            this.Add((TKey)er.Current, v);  
    //        //        }
    //        //        //
    //        //    }
    //        //}

    //        KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array._buckets.Count];
    //        array._buckets.CopyTo(pairs, 0);
    //        for (int i = beginpos; i < beforeendpos; i++) {
    //            this.Add(pairs[i].Key, pairs[i].Value.ToArray());
    //        }
    //    }
    //    public void insert(std_multimap<TKey, TValue> array) {
    //        KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array._buckets.Count];
    //        array._buckets.CopyTo(pairs, 0);
    //        for (int i = 0; i < pairs.Length; i++) {
    //            this.Add(pairs[i].Key, pairs[i].Value.ToArray());
    //        }
    //    }
    //    /// <summary>
    //    /// 第几个下标位置的KEY删除 
    //    /// 通常 pos=find("key");
    //    /// erase(pos);
    //    /// </summary>
    //    /// <param name="pos"></param>
    //    /// <return> returns the number of elements erased. </return>
    //    public int erase(uint pos) {
    //        TKey[] pairs = new TKey[this._buckets.Keys.Count];
    //        this._buckets.Keys.CopyTo(pairs, 0);
    //        if (pos >= pairs.Length) return 0;
    //        int c = this._buckets[pairs[pos]].Count;
    //        this.Remove(pairs[pos]);
    //        return c;
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns>returns the number of elements erased.</returns>
    //    public int erase(TKey key) {
    //        if (!this.ContainsKey(key)) return 0;
    //        int c = this._buckets[key].Count;
    //        this.Remove(key);
    //        return c;
    //    }
    //    public void erase(int beginpos, int beforeendpos) {
    //        TKey[] pairs = new TKey[this._buckets.Keys.Count];
    //        this._buckets.Keys.CopyTo(pairs, 0);
    //        for (int i = beginpos; i < beforeendpos; i++) {
    //            this.Remove(pairs[i]);
    //        }
    //    }
    //    //
    //    public void emplace(TKey key, TValue value) {
    //        this.Add(key, value);
    //    }
    //    /// <summary>
    //    /// 原始是返回key比较器， 这里把所有键返回了
    //    /// </summary>
    //    /// <returns></returns>
    //    public TKey[] key_comp() {
    //        TKey[] keys = new TKey[this._buckets.Keys.Count];
    //        this._buckets.Keys.CopyTo(keys, 0);
    //        return keys;
    //    }
    //    //      std::multimap<char,int> mymultimap;

    //    //mymultimap.insert(std::make_pair('x',101));
    //    //mymultimap.insert(std::make_pair('y',202));
    //    //mymultimap.insert(std::make_pair('y',252));
    //    //mymultimap.insert(std::make_pair('z',303));

    //    //std::cout << "mymultimap contains:\n";

    //    //std::pair<char,int> highest = *mymultimap.rbegin();          // last element

    //    //std::multimap<char,int>::iterator it = mymultimap.begin();
    //    //do {
    //    //  std::cout << (*it).first << " => " << (*it).second << '\n';
    //    //} while ( mymultimap.value_comp()(*it++, highest) );

    //    //  Output:
    //    //mymultimap contains:
    //    //x => 101
    //    //y => 202
    //    //y => 252
    //    //z => 303
    //    /// <summary>
    //    /// 原始是返回value比较器
    //    /// 注意这里没有实现。【不要调用】
    //    /// </summary>
    //    /// <returns></returns>
    //    public TValue[] value_comp() {
    //        throw new Exception("返回值比较器异常");
    //    }
    //    public KeyValuePair<TKey, List<TValue>>[] get_allocator() {
    //        KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array.buckets.Count];
    //        this._buckets.CopyTo(pairs, 0);
    //        return pairs;
    //    }
    //    /// <summary>
    //    /// 查找索引位置
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="pos"></param>
    //    /// <returns>没有找到返回-1</returns>
    //    public int find(TKey key, bool pos) {
    //        int index = -1;
    //        foreach (var v in this._buckets.Keys) {
    //            index++;
    //            if (v.Equals(key)) {
    //                break;
    //            }
    //        }
    //        return index;
    //    }
    //    /// <summary>
    //    /// 查询key对应的值的个数
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns>值的个数</returns>
    //    public int count(TKey key) {
    //        if (this._buckets.ContainsKey(key)) {
    //            return this._buckets[key].Count;
    //        }
    //        return 0;
    //    }

    //    public int lower_bound(TKey key) {
    //        return find(key, true);
    //    }
    //    public int upper_bound(TKey key) {
    //        return find(key, true) + 1;
    //    }
    //}




    ///// <summary>
    /////		Map class
    ///// </summary>
    ///// <remarks>
    /////     A map allows multiple values per key, unlike the Hashtable which only allows
    /////     unique keys and only a single value per key.  Multiple values assigned to the same
    /////     key are placed in a "bucket", which in this case is an ArrayList.
    /////     <p/>
    /////     An example of values in a map would look like this:
    /////     Key     Value
    /////     "a"     "Alan"
    /////     "a"     "Adam"
    /////     "b"     "Brien"
    /////     "c"     "Chris"
    /////     "c"     "Carl"
    /////     etc
    /////     <p/>
    /////     Currently, enumeration is the only way to iterate through the values, which is
    /////     more pratical in terms of how the Map works internally anyway.  Intial testing showed
    /////     that inserting and iterating through 100,000 items, the Inserts took ~260ms and a full
    /////     enumeration of them all (with unboxing of the value type stored in the map) took between 16-30ms.
    ///// </remarks>
    //public class std_multimap
    //{
    //    #region Fields

    //    /// <summary>
    //    ///     Number of total items currently in this map.
    //    /// </summary>
    //    protected int _count;

    //    /// <summary>
    //    ///     A list of buckets.
    //    /// </summary>
    //    //public Hashtable _buckets;         
    //    public SortedList _buckets;
    //    #endregion Fields

    //    #region Constructor

    //    /// <summary>
    //    ///     Default constructor.
    //    /// </summary>
    //    public std_multimap() {
    //        this._buckets = new SortedList();//new Hashtable();
    //    }

    //    #endregion Constructor

    //    /// <summary>
    //    ///     Clears this map of all contained objects.
    //    /// </summary>
    //    public void Clear() {
    //        this._buckets.Clear();
    //        this._count = 0;
    //    }

    //    /// <summary>
    //    ///     Clears the bucket with given key.
    //    /// </summary>
    //    public void Clear(object key) {
    //        var bucket = (ArrayList)this._buckets[key];
    //        if (bucket != null) {
    //            this._count -= bucket.Count;
    //            this._buckets.Remove(key);
    //        }
    //    }

    //    /// <summary>
    //    ///     Given a key, Find will return an IEnumerator that allows
    //    ///     you to iterate over all items in the bucket associated
    //    ///     with the key.
    //    /// </summary>
    //    /// <param name="key">Key for look for.</param>
    //    /// <returns>IEnumerator to go through the items assigned to the key.</returns>
    //    public IEnumerator Find(object key) {
    //        if (this._buckets[key] == null) {
    //            return null;
    //        }
    //        else {
    //            return ((ArrayList)this._buckets[key]).GetEnumerator();
    //        }
    //    }

    //    public IList FindBucket(object key) {
    //        if (this._buckets[key] == null) {
    //            return null;
    //        }
    //        else {
    //            return (ArrayList)this._buckets[key];
    //        }
    //    }

    //    /// <summary>
    //    ///     Given a key, FindFirst will return the first item in the bucket
    //    ///     associated with the key.
    //    /// </summary>
    //    /// <param name="key">Key to look for.</param>
    //    public object FindFirst(object key) {
    //        if (this._buckets[key] == null) {
    //            return null;
    //        }
    //        else {
    //            return ((ArrayList)this._buckets[key])[0];
    //        }
    //    }

    //    /// <summary>
    //    ///     Gets the count of objects mapped to the specified key.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public int Count(object key) {
    //        if (this._buckets[key] != null) {
    //            return ((ArrayList)this._buckets[key]).Count;
    //        }

    //        return 0;
    //    }

    //    /// <summary>
    //    ///     Inserts a value into a bucket that is specified by the
    //    ///     key.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="val"></param>
    //    public void Add(object key, object val) {
    //        ArrayList container = null;

    //        if (this._buckets[key] == null) {
    //            container = new ArrayList();
    //            this._buckets.Add(key, container);
    //        }
    //        else {
    //            container = (ArrayList)this._buckets[key];
    //        }

    //        // TODO: Doing the contains check is extremely slow, so for now duplicate items are allowed
    //        //if(!container.Contains(val)) {
    //        container.Add(val);
    //        this._count++;
    //        //}
    //    }

    //    /// <summary>
    //    ///     Gets the total count of all items contained within the map.
    //    /// </summary>
    //    public int TotalCount {
    //        get {
    //            return this._count;
    //        }
    //    }
    //    public int KeyCount {
    //        get {
    //            return this._buckets.Count;
    //        }
    //    }
    //    /// <summary>
    //    ///     Gets an appropriate enumerator for the map, customized to go
    //    ///     through each key in the map and return a Pair of the key and
    //    ///     an ArrayList of the values associated with it.
    //    /// </summary>
    //    /// <returns></returns>
    //    public IEnumerator GetBucketEnumerator() {
    //        return this._buckets.Keys.GetEnumerator();
    //    }


    //    #region
    //    public int begin() {
    //        return 0;
    //    }
    //    /// <summary>
    //    /// 返回的是当中个数 比最后的位置大1
    //    /// </summary>
    //    /// <returns></returns>
    //    public int end() {
    //        return this.KeyCount;// -1;
    //    }
    //    public bool empty() {
    //        return this.KeyCount == 0;
    //    }
    //    public int size() {
    //        return this.KeyCount;
    //    }
    //    public int max_size() {
    //        return int.MaxValue;
    //    }

    //    public void clear() {
    //        this.Clear();
    //    }
    //    public static void swap(ref std_multimap _this, ref std_multimap _other) {
    //        std_multimap temp = _this;
    //        _this = _other;
    //        _other = temp;
    //    }

    //    public void insert(object key, object value) {
    //        this.Add(key, value);
    //    }
    //    //public void insert(KeyValuePair<TKey, TValue> pair) {
    //    //    this.Add(pair.Key, pair.Value);
    //    //}
    //    public void insert(std_pair pair) {
    //        this.Add(pair.first, pair.second);
    //    }
    //    //public void insert(uint pos, KeyValuePair<TKey, TValue> pair) {
    //    //    insert(pair);
    //    //}
    //    public void insert(uint pos, std_pair pair) {
    //        insert(pair);
    //    }
    //    ///// <summary>
    //    ///// 
    //    ///// </summary>
    //    ///// <param name="array"></param>
    //    ///// <param name="beginpos"></param>
    //    ///// <param name="endpos">当前位置之前</param>
    //    //public void insert(std_multimap<TKey, TValue> array, int beginpos, int beforeendpos) {
    //    //    //System.Collections.IEnumerator er = array.buckets.Keys.GetEnumerator();

    //    //    //int index = 0;
    //    //    //bool find_first=false;
    //    //    //bool find_end=false;
    //    //    //while (er.MoveNext()) {
    //    //    //    if (beginpos == index) {
    //    //    //        find_first = true;
    //    //    //    }
    //    //    //    if (beforeendpos == index) {
    //    //    //        find_end = true;
    //    //    //    }
    //    //    //    if (find_first && !find_end) {
    //    //    //        foreach (var v in array.buckets[(TKey)er.Current]) {
    //    //    //            this.Add((TKey)er.Current, v);  
    //    //    //        }
    //    //    //        //
    //    //    //    }
    //    //    //}

    //    //    KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array._buckets.Count];
    //    //    array._buckets.CopyTo(pairs, 0);
    //    //    for (int i = beginpos; i < beforeendpos; i++) {
    //    //        this.Add(pairs[i].Key, pairs[i].Value.ToArray());
    //    //    }
    //    //}
    //    //public void insert(std_multimap<TKey, TValue> array) {
    //    //    KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array._buckets.Count];
    //    //    array._buckets.CopyTo(pairs, 0);
    //    //    for (int i = 0; i < pairs.Length; i++) {
    //    //        this.Add(pairs[i].Key, pairs[i].Value.ToArray());
    //    //    }
    //    //}
    //    /// <summary>
    //    /// 第几个下标位置的KEY删除 
    //    /// 通常 pos=find("key");
    //    /// erase(pos);
    //    /// </summary>
    //    /// <param name="pos"></param>
    //    public void erase(uint pos) {
    //        this._buckets.RemoveAt((int)pos);
    //    }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="key"></param>
    //    public int erase(TKey key) {
    //        if (!this._buckets.ContainsKey(key)) return 0;
    //        int c = this._buckets[key].Count;
    //        this._buckets.Remove(key);
    //        return c;
    //    }
    //    public void erase(int beginpos, int beforeendpos) {
    //        for (int i = beforeendpos - 1; i >= beginpos; i--) {
    //            this._buckets.RemoveAt(i);
    //        }
    //    }
    //    //
    //    public void emplace(object key, object value) {
    //        this.Add(key, value);
    //    }
    //    ///// <summary>
    //    ///// 原始是返回key比较器， 这里把所有键返回了
    //    ///// </summary>
    //    ///// <returns></returns>
    //    //public TKey[] key_comp() {
    //    //    TKey[] keys = new TKey[this._buckets.Keys.Count];
    //    //    this._buckets.Keys.CopyTo(keys, 0);
    //    //    return keys;
    //    //}
    //    //      std::multimap<char,int> mymultimap;

    //    //mymultimap.insert(std::make_pair('x',101));
    //    //mymultimap.insert(std::make_pair('y',202));
    //    //mymultimap.insert(std::make_pair('y',252));
    //    //mymultimap.insert(std::make_pair('z',303));

    //    //std::cout << "mymultimap contains:\n";

    //    //std::pair<char,int> highest = *mymultimap.rbegin();          // last element

    //    //std::multimap<char,int>::iterator it = mymultimap.begin();
    //    //do {
    //    //  std::cout << (*it).first << " => " << (*it).second << '\n';
    //    //} while ( mymultimap.value_comp()(*it++, highest) );

    //    //  Output:
    //    //mymultimap contains:
    //    //x => 101
    //    //y => 202
    //    //y => 252
    //    //z => 303
    //    /// <summary>
    //    /// 原始是返回value比较器
    //    /// 注意这里没有实现。【不要调用】
    //    /// </summary>
    //    /// <returns></returns>
    //    public TValue[] value_comp() {
    //        throw new Exception("返回值比较器异常");
    //    }
    //    //public ArrayList get_allocator() {
    //    //    KeyValuePair<TKey, List<TValue>>[] pairs = new KeyValuePair<TKey, List<TValue>>[array.buckets.Count];
    //    //    this._buckets.CopyTo(pairs, 0);
    //    //    return pairs;
    //    //}
    //    /// <summary>
    //    /// 查找索引位置
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="pos"></param>
    //    /// <returns>没有找到返回-1</returns>
    //    public int find(TKey key, bool pos) {
    //        int index = -1;
    //        foreach (var v in this._buckets.Keys) {
    //            index++;
    //            if (v.Equals(key)) {
    //                break;
    //            }
    //        }
    //        return index;
    //    }
    //    /// <summary>
    //    /// 查询key对应的值的个数
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns>值的个数</returns>
    //    public int count(object key) {
    //        if (this._buckets.ContainsKey(key)) {
    //            return ((ArrayList)this._buckets[key]).Count;
    //        }
    //        return 0;
    //    }

    //    public int lower_bound(TKey key) {
    //        return find(key, true);
    //    }
    //    public int upper_bound(TKey key) {
    //        return find(key, true) + 1;
    //    }

    //    #endregion

    //}
}
