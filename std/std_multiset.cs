using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Mogre_Procedural.std
{
    /// <summary>
    /// liek c++ std::multiset 包含指定KEY数量的列表 相当于Dictionary<TKey,int>（第二个参数存储着Key的数量）
    /// </summary>
    public class std_multiset<T> : IEnumerable<T>
    {

        private SortedDictionary<T, int> _items = new SortedDictionary<T, int>();

        public std_multiset() {
        }

        public std_multiset(params T[] items) {
            foreach (T item in items)
                Add(item);
        }


        public void Add(T item) {
            if (Contains(item))
                _items[item]++;
            else
                _items[item] = 1;
        }

        public bool Contains(T item) {
            return _items.ContainsKey(item);
        }

        public bool Remove(T item) {
            if (!Contains(item)) {
                return false;
                throw new Exception();
            }
            if (--_items[item] == 0) {
                _items.Remove(item);
            }
            return true;
        }

        public void Remove(IEnumerable<T> itemList) {
            foreach (T item in itemList)
                _items.Remove(item);
        }

        public int Count {
            get {
                int count = 0;

                foreach (int n in _items.Values)
                    count += n;

                return count;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (KeyValuePair<T, int> entry in _items) {
                for (int n = 0; n < entry.Value; n++)
                    yield return entry.Key;
            }

            /*List<T> enumerated = new List<T>();
            
            foreach (KeyValuePair<T, int> entry in items)
            {
                for (int n = 0; n < entry.Value; n++)
                    enumerated.Add(entry.Key);
            }
            
            return enumerated.GetEnumerator();*/
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }




        public int begin() {
            return 0;
        }

        public int end() {
            return this.Count;
        }
        public bool empty() {
            return this.Count == 0;
        }
        public int size() {
            return this.Count;
        }
        public int max_size() {
            return int.MaxValue;
        }
        public void clear() {
            this._items.Clear();
        }
        //
        //
        public void insert(T value) {
              this.Add(value);
        }
        public void insert(uint pos, T value) {
             this.Add(value);
        }
        public void insert(T[] array, int beginpos, int beforeendpos) {
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Add(array[i]);
            }
        }

        public bool erase(int pos, bool index) {
            T[] array =get_allocator();            
            return this.Remove(array[pos]);
        }
        /// <summary>
        /// 原始返回 0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool erase(T value) {
            return this._items.Remove(value);
        }
        public void erase(int beginpos, int beforeendpos) {
            T[] array = get_allocator();
            //base.CopyTo(array, 0);
            for (int i = beforeendpos - 1; i >= beginpos; i--) {
                this.Remove(array[i]);
            }
        }

        /// <summary>
        /// T type must same
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_other"></param>
        public static void swap(std_multiset<T> _this, std_multiset<T> _other) {
            std_multiset<T> temp = _this;
            _this = _other;
            _other = temp;
        }

        public void emplace(T value) {
             this.Add(value);
        }
        /// <summary>
        /// 原始是返回KEY比较器 这里没有实现
        /// </summary>
        /// <returns></returns>
        public T[] key_comp() {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 原始是返回值比较器 这里没有实现
        /// </summary>
        /// <returns></returns>
        public T[] value_comp() {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 没有找到返回-1
        /// 返回查找的位置
        /// </summary>
        /// <returns>if no find,return -1</returns>
        public int find(T value) {
            
            IEnumerator<T> rt = this.GetEnumerator();
            int index = -1;
            while (rt.MoveNext()) {
                index++;
                if (rt.Current.Equals(value)) {
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 返回指定值的数量 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int count(T value) {
            if (_items.ContainsKey(value))
                return _items[value];
            return 0;
        }

        public int lower_bound(T key) {
            int f = find(key);
            return f;
        }
        public int upper_bound(T key) {
            int f = find(key);
            return f + 1;
        }
        public std_pair<int, int> equal_range(T key) {
            int f = find(key);
            return new std_pair<int, int>(f, f + 1);
        }
        public T[] get_allocator() {
            T[] array = new T[this.Count];
            int index = 0;
            foreach (var v in this._items) {
                int len = v.Value;
                for (int i = 0; i < len; i++) {
                    array[index++] = v.Key;
                }
            }
           return array;
        }
    }
   
    /// <summary>
    /// like c++ std::unordered_multiset
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class std_unordered_multiset<T> :  IEnumerable<T>
    {

        private Dictionary<T, int> _items = new Dictionary<T, int>();

        public std_unordered_multiset() {
        }

        public std_unordered_multiset(params T[] items) {
            foreach (T item in items)
                Add(item);
        }


        public void Add(T item) {
            if (Contains(item))
                _items[item]++;
            else
                _items[item] = 1;
        }

        public bool Contains(T item) {
            return _items.ContainsKey(item);
        }

        public bool Remove(T item) {
            if (!Contains(item)) {
                return false;
                throw new Exception();
            }
            if (--_items[item] == 0)
                _items.Remove(item);
            return true;
        }

        public void Remove(IEnumerable<T> itemList) {
            foreach (T item in itemList)
                _items.Remove(item);
        }

        public int Count {
            get {
                int count = 0;

                foreach (int n in _items.Values)
                    count += n;

                return count;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            foreach (KeyValuePair<T, int> entry in _items) {
                for (int n = 0; n < entry.Value; n++)
                    yield return entry.Key;
            }

            /*List<T> enumerated = new List<T>();
            
            foreach (KeyValuePair<T, int> entry in items)
            {
                for (int n = 0; n < entry.Value; n++)
                    enumerated.Add(entry.Key);
            }
            
            return enumerated.GetEnumerator();*/
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }


        //
        public void insert(T value) {
            this.Add(value);
        }
        public void insert(uint pos, T value) {
            this.Add(value);
        }
        public void insert(T[] array, int beginpos, int beforeendpos) {
            for (int i = beginpos; i < beforeendpos; i++) {
                this.Add(array[i]);
            }
        }

        public bool erase(int pos, bool index) {
            T[] array = get_allocator();
            return this.Remove(array[pos]);
        }
        /// <summary>
        /// 原始返回 0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool erase(T value) {
            return this._items.Remove(value);
        }
        public void erase(int beginpos, int beforeendpos) {
            T[] array = get_allocator();
            //base.CopyTo(array, 0);
            for (int i = beforeendpos - 1; i >= beginpos; i--) {
                this.Remove(array[i]);
            }
        }

        /// <summary>
        /// T type must same
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_other"></param>
        public static void swap(std_unordered_multiset<T> _this, std_unordered_multiset<T> _other) {
            std_unordered_multiset<T> temp = _this;
            _this = _other;
            _other = temp;
        }

        public void emplace(T value) {
            this.Add(value);
        }
        /// <summary>
        /// 原始是返回KEY比较器 这里没有实现
        /// </summary>
        /// <returns></returns>
        public T[] key_comp() {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 原始是返回值比较器 这里没有实现
        /// </summary>
        /// <returns></returns>
        public T[] value_comp() {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 没有找到返回-1
        /// 返回查找的位置
        /// </summary>
        /// <returns>if no find,return -1</returns>
        public int find(T value) {

            IEnumerator<T> rt = this.GetEnumerator();
            int index = -1;
            while (rt.MoveNext()) {
                index++;
                if (rt.Current.Equals(value)) {
                    return index;
                }
            }
            return -1;
        }
        /// <summary>
        /// 返回指定值的数量 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int count(T value) {
            if (_items.ContainsKey(value))
                return _items[value];
            return 0;
        }

        public int lower_bound(T key) {
            int f = find(key);
            return f;
        }
        public int upper_bound(T key) {
            int f = find(key);
            return f + 1;
        }
        public std_pair<int, int> equal_range(T key) {
            int f = find(key);
            return new std_pair<int, int>(f, f + 1);
        }
        public T[] get_allocator() {
            T[] array = new T[this.Count];
            int index = 0;
            foreach (var v in this._items) {
                int len = v.Value;
                for (int i = 0; i < len; i++) {
                    array[index++] = v.Key;
                }
            }
            return array;
        }

    }
    //public struct KeyDat<KeyT, DatT> : IPair<KeyT, DatT>, IComparable<KeyDat<KeyT, DatT>>, IComparable, IEquatable<KeyDat<KeyT, DatT>>, ISerializable where KeyT : IComparable<KeyT>
    //{
    //    private KeyT mKey;
    //    private DatT mDat;


    //    public KeyDat(BinarySerializer reader) {
    //        mKey = default(KeyT);
    //        mDat = default(DatT);
    //        Load(reader); // throws ArgumentNullException, serialization-related exceptions
    //    }


    //    public KeyDat(KeyT key, DatT dat) {
    //        Utils.ThrowException(key == null ? new ArgumentNullException("key") : null);
    //        mKey = key;
    //        mDat = dat;
    //    }


    //    public KeyDat(KeyT key) {
    //        Utils.ThrowException(key == null ? new ArgumentNullException("key") : null);
    //        mKey = key;
    //        mDat = default(DatT);
    //    }


    //    public KeyT Key {
    //        get { return mKey; }
    //        set {
    //            Utils.ThrowException(value == null ? new ArgumentNullException("Key") : null);
    //            mKey = value;
    //        }
    //    }


    //    public DatT Dat {
    //        get { return mDat; }
    //        set { mDat = value; }
    //    }


    //    public override int GetHashCode() {
    //        return mKey.GetHashCode();
    //    }


    //    public override string ToString() {
    //        return string.Format("( {0} {1} )", mKey, mDat);
    //    }


    //    public static bool operator ==(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) == 0;
    //    }


    //    public static bool operator !=(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) != 0;
    //    }


    //    public static bool operator >(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) > 0;
    //    }


    //    public static bool operator <(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) < 0;
    //    }


    //    public static bool operator >=(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) >= 0;
    //    }


    //    public static bool operator <=(KeyDat<KeyT, DatT> a, KeyDat<KeyT, DatT> b) {
    //        return a.mKey.CompareTo(b.mKey) <= 0;
    //    }


    //    // *** IPair<KeyT, DatT> interface implementation ***


    //    public KeyT First {
    //        get { return mKey; }
    //    }


    //    public DatT Second {
    //        get { return mDat; }
    //    }


    //    // *** IComparable<KeyDat<KeyT, DatT>> interface implementation ***


    //    public int CompareTo(KeyDat<KeyT, DatT> other) {
    //        return mKey.CompareTo(other.Key);
    //    }


    //    // *** IComparable interface implementation ***


    //    int IComparable.CompareTo(object obj) {
    //        Utils.ThrowException(!(obj is KeyDat<KeyT, DatT>) ? new ArgumentTypeException("obj") : null);
    //        return CompareTo((KeyDat<KeyT, DatT>)obj);
    //    }


    //    // *** IEquatable<KeyDat<KeyT, DatT>> interface implementation ***


    //    public bool Equals(KeyDat<KeyT, DatT> other) {
    //        return other.mKey.Equals(mKey);
    //    }


    //    public override bool Equals(object obj) {
    //        Utils.ThrowException(!(obj is KeyDat<KeyT, DatT>) ? new ArgumentTypeException("obj") : null);
    //        return Equals((KeyDat<KeyT, DatT>)obj);
    //    }


    //    // *** ISerializable interface implementation ***


    //    public void Save(BinarySerializer writer) {
    //        Utils.ThrowException(writer == null ? new ArgumentNullException("writer") : null);
    //        // the following statements throw serialization-related exceptions
    //        writer.WriteValueOrObject<KeyT>(mKey);
    //        writer.WriteValueOrObject<DatT>(mDat);
    //    }


    //    public void Load(BinarySerializer reader) {
    //        Utils.ThrowException(reader == null ? new ArgumentNullException("reader") : null);
    //        // the following statements throw serialization-related exceptions
    //        mKey = reader.ReadValueOrObject<KeyT>();
    //        mDat = reader.ReadValueOrObject<DatT>();
    //    }
    //}


    //public class MultiSet<T> : ICollection<T>, ICollection, IEnumerable<KeyValuePair<T, int>>, ICloneable<MultiSet<T>>, IDeeplyCloneable<MultiSet<T>>, IContentEquatable<MultiSet<T>>, ISerializable
    //{
    //    private Dictionary<T, int> mItems
    //        = new Dictionary<T, int>();


    //    public MultiSet() {
    //    }


    //    public MultiSet(IEqualityComparer<T> comparer) {
    //        mItems = new Dictionary<T, int>(comparer);
    //    }


    //    public MultiSet(BinarySerializer reader) {
    //        Load(reader); // throws ArgumentNullException, serialization-related exceptions
    //    }


    //    public MultiSet(BinarySerializer reader, IEqualityComparer<T> comparer) {
    //        mItems = new Dictionary<T, int>(comparer);
    //        Load(reader); // throws ArgumentNullException
    //    }


    //    public MultiSet(IEnumerable<T> items) {
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public MultiSet(IEnumerable<T> items, IEqualityComparer<T> comparer) {
    //        mItems = new Dictionary<T, int>(comparer);
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public void SetItems(IEnumerable<T> items) {
    //        mItems.Clear();
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public void AddRange(IEnumerable<T> items) {
    //        Utils.ThrowException(items == null ? new ArgumentNullException("items") : null);
    //        foreach (T item in items) {
    //            int count;
    //            if (mItems.TryGetValue(item, out count)) // throws ArgumentNullException
    //            {
    //                mItems[item] = count + 1;
    //            }
    //            else {
    //                mItems.Add(item, 1);
    //            }
    //        }
    //    }


    //    private void AddRange(IEnumerable<KeyValuePair<T, int>> items) {
    //        foreach (KeyValuePair<T, int> item in items) {
    //            int count;
    //            if (mItems.TryGetValue(item.Key, out count)) {
    //                mItems[item.Key] = count + item.Value;
    //            }
    //            else {
    //                mItems.Add(item.Key, item.Value);
    //            }
    //        }
    //    }


    //    public IEqualityComparer<T> Comparer {
    //        get { return mItems.Comparer; }
    //    }


    //    public static MultiSet<T> Union(MultiSet<T> a, MultiSet<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        MultiSet<T> c = a.Clone(); // *** inherits comparer from a (b is expected to have the same comparer)
    //        c.AddRange((IEnumerable<KeyValuePair<T, int>>)b);
    //        return c;
    //    }


    //    public static MultiSet<T> Union(MultiSet<T>.ReadOnly a, MultiSet<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Union(a.Inner, b.Inner);
    //    }


    //    public static MultiSet<T> Intersection(MultiSet<T> a, MultiSet<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        MultiSet<T> c = new MultiSet<T>(a.Comparer); // *** inherits comparer from a (b is expected to have the same comparer)
    //        if (b.Count < a.Count) { MultiSet<T> tmp; tmp = a; a = b; b = tmp; }
    //        foreach (KeyValuePair<T, int> item in a) {
    //            int bCount = b.GetCount(item.Key);
    //            if (bCount > 0) { c.mItems.Add(item.Key, Math.Min(item.Value, bCount)); }
    //        }
    //        return c;
    //    }


    //    public static MultiSet<T> Intersection(MultiSet<T>.ReadOnly a, MultiSet<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Intersection(a.Inner, b.Inner);
    //    }


    //    public static MultiSet<T> Difference(MultiSet<T> a, MultiSet<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        MultiSet<T> c = new MultiSet<T>(a.Comparer); // *** inherits comparer from a (b is expected to have the same comparer)
    //        foreach (KeyValuePair<T, int> item in a) {
    //            int bCount = b.GetCount(item.Key);
    //            int cCount = item.Value - bCount;
    //            if (cCount > 0) { c.mItems.Add(item.Key, cCount); }
    //        }
    //        return c;
    //    }


    //    public static MultiSet<T> Difference(MultiSet<T>.ReadOnly a, MultiSet<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Difference(a.Inner, b.Inner);
    //    }


    //    public static double JaccardSimilarity(MultiSet<T> a, MultiSet<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        MultiSet<T> c = Intersection(a, b);
    //        double div = (double)(a.Count + b.Count - c.Count);
    //        if (div == 0) { return 1; } // *** if both sets are empty, the similarity is 1
    //        return (double)c.Count / div;
    //    }


    //    public static double JaccardSimilarity(MultiSet<T>.ReadOnly a, MultiSet<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return JaccardSimilarity(a.Inner, b.Inner);
    //    }


    //    private int GetCount() {
    //        int sum = 0;
    //        foreach (int count in mItems.Values) {
    //            sum += count;
    //        }
    //        return sum;
    //    }


    //    public T[] ToArray() {
    //        T[] array = new T[GetCount()];
    //        CopyTo(array, 0);
    //        return array;
    //    }


    //    public T Any {
    //        get {
    //            foreach (KeyValuePair<T, int> item in mItems) {
    //                return item.Key;
    //            }
    //            throw new InvalidOperationException();
    //        }
    //    }


    //    public NewT[] ToArray<NewT>() {
    //        return ToArray<NewT>(/*fmtProvider=*/null); // throws InvalidCastException, FormatException, OverflowException
    //    }


    //    public NewT[] ToArray<NewT>(IFormatProvider fmtProvider) {
    //        NewT[] array = new NewT[GetCount()];
    //        int i = 0;
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            for (int j = 0; j < item.Value; j++) {
    //                array[i++] = (NewT)Utils.ChangeType(item.Key, typeof(NewT), fmtProvider); // throws InvalidCastException, FormatException, OverflowException
    //            }
    //        }
    //        return array;
    //    }


    //    public void RemoveRange(IEnumerable<T> items) {
    //        Utils.ThrowException(items == null ? new ArgumentNullException("items") : null);
    //        foreach (T item in items) {
    //            int count;
    //            if (mItems.TryGetValue(item, out count)) // throws ArgumentNullException
    //            {
    //                if (count - 1 > 0) {
    //                    mItems[item] = count - 1;
    //                }
    //                else {
    //                    mItems.Remove(item);
    //                }
    //            }
    //        }
    //    }


    //    public int CountUnique {
    //        get { return mItems.Count; }
    //    }


    //    public int GetCount(T item) {
    //        int count;
    //        if (mItems.TryGetValue(item, out count)) // throws ArgumentNullException
    //        {
    //            return count;
    //        }
    //        return 0;
    //    }


    //    public bool RemoveAll(T item) {
    //        if (mItems.ContainsKey(item)) // throws ArgumentNullException
    //        {
    //            mItems.Remove(item);
    //            return true;
    //        }
    //        return false;
    //    }


    //    public void Add(T item, int count) {
    //        Utils.ThrowException(count < 0 ? new ArgumentOutOfRangeException("count") : null);
    //        if (count == 0) { return; }
    //        int oldCount;
    //        if (mItems.TryGetValue(item, out oldCount)) // throws ArgumentNullException
    //        {
    //            mItems[item] = oldCount + count;
    //        }
    //        else {
    //            mItems.Add(item, count);
    //        }
    //    }


    //    public bool Remove(T item, int count) {
    //        Utils.ThrowException(count < 0 ? new ArgumentOutOfRangeException("count") : null);
    //        if (count == 0) { return false; }
    //        int oldCount;
    //        if (mItems.TryGetValue(item, out oldCount)) // throws ArgumentNullException
    //        {
    //            if (count < oldCount) {
    //                mItems[item] = oldCount - count;
    //            }
    //            else {
    //                mItems.Remove(item);
    //            }
    //            return true;
    //        }
    //        return false;
    //    }


    //    public ArrayList<KeyDat<int, T>> ToList() {
    //        ArrayList<KeyDat<int, T>> list = new ArrayList<KeyDat<int, T>>();
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            list.Add(new KeyDat<int, T>(item.Value, item.Key));
    //        }
    //        return list;
    //    }


    //    public override string ToString() {
    //        StringBuilder str = new StringBuilder("{");
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            for (int j = 0; j < item.Value; j++) {
    //                str.Append(" ");
    //                str.Append(item.Key);
    //            }
    //        }
    //        str.Append(" }");
    //        return str.ToString();
    //    }


    //    // *** ICollection<T> interface implementation ***


    //    public void Add(T item) {
    //        int count;
    //        if (mItems.TryGetValue(item, out count)) // throws ArgumentNullException
    //        {
    //            mItems[item] = count + 1;
    //        }
    //        else {
    //            mItems.Add(item, 1);
    //        }
    //    }


    //    public void Clear() {
    //        mItems.Clear();
    //    }


    //    public bool Contains(T item) {
    //        return mItems.ContainsKey(item); // throws ArgumentNullException
    //    }


    //    public void CopyTo(T[] array, int index) {
    //        Utils.ThrowException(array == null ? new ArgumentNullException("array") : null);
    //        Utils.ThrowException(index + GetCount() > array.Length ? new ArgumentOutOfRangeException("index") : null);
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            for (int j = 0; j < item.Value; j++) {
    //                array.SetValue(item.Key, index++);
    //            }
    //        }
    //    }


    //    public int Count {
    //        get { return GetCount(); }
    //    }


    //    public bool IsReadOnly {
    //        get { return false; }
    //    }


    //    public bool Remove(T item) {
    //        int count;
    //        if (mItems.TryGetValue(item, out count)) // throws ArgumentNullException
    //        {
    //            if (count - 1 > 0) {
    //                mItems[item] = count - 1;
    //            }
    //            else {
    //                mItems.Remove(item);
    //            }
    //            return true;
    //        }
    //        return false;
    //    }


    //    // *** ICollection interface implementation ***


    //    void ICollection.CopyTo(Array array, int index) {
    //        Utils.ThrowException(array == null ? new ArgumentNullException("array") : null);
    //        Utils.ThrowException(index + GetCount() > array.Length ? new ArgumentOutOfRangeException("index") : null);
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            for (int j = 0; j < item.Value; j++) {
    //                array.SetValue(item, index++);
    //            }
    //        }
    //    }


    //    bool ICollection.IsSynchronized {
    //        get { throw new NotSupportedException(); }
    //    }


    //    object ICollection.SyncRoot {
    //        get { throw new NotSupportedException(); }
    //    }


    //    // *** IEnumerable interface implementation ***


    //    public IEnumerator<KeyValuePair<T, int>> GetEnumerator() {
    //        return mItems.GetEnumerator();
    //    }


    //    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
    //        throw new NotImplementedException();
    //    }


    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }


    //    // *** ICloneable interface implementation ***


    //    public MultiSet<T> Clone() {
    //        MultiSet<T> clone = new MultiSet<T>(mItems.Comparer);
    //        clone.AddRange(mItems);
    //        return clone;
    //    }


    //    object ICloneable.Clone() {
    //        return Clone();
    //    }


    //    // *** IDeeplyCloneable interface implementation ***


    //    public MultiSet<T> DeepClone() {
    //        MultiSet<T> clone = new MultiSet<T>(mItems.Comparer);
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            clone.mItems.Add((T)Utils.Clone(item.Key, /*deepClone=*/true), item.Value);
    //        }
    //        return clone;
    //    }


    //    object IDeeplyCloneable.DeepClone() {
    //        return DeepClone();
    //    }


    //    // *** IContentEquatable<MultiSet<T>> interface implementation ***


    //    public bool ContentEquals(MultiSet<T> other) {
    //        if (other == null || Count != other.Count) { return false; }
    //        foreach (KeyValuePair<T, int> item in mItems) {
    //            int count;
    //            if (other.mItems.TryGetValue(item.Key, out count)) {
    //                if (count != item.Value) { return false; }
    //            }
    //            else {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }


    //    bool IContentEquatable.ContentEquals(object other) {
    //        Utils.ThrowException((other != null && !(other is MultiSet<T>)) ? new ArgumentTypeException("other") : null);
    //        return ContentEquals((MultiSet<T>)other);
    //    }


    //    // *** ISerializable interface implementation ***


    //    public void Save(BinarySerializer writer) {
    //        Utils.ThrowException(writer == null ? new ArgumentNullException("writer") : null);
    //        Utils.SaveDictionary(mItems, writer); // throws serialization-related exceptions 
    //    }


    //    public void Load(BinarySerializer reader) {
    //        Utils.ThrowException(reader == null ? new ArgumentNullException("reader") : null);
    //        mItems = Utils.LoadDictionary<T, int>(reader); // throws serialization-related exceptions 
    //    }


    //    // *** Implicit cast to a read-only adapter ***


    //    public static implicit operator MultiSet<T>.ReadOnly(MultiSet<T> set) {
    //        if (set == null) { return null; }
    //        return new MultiSet<T>.ReadOnly(set);
    //    }


    //    // *** Equality comparer ***


    //    public static IEqualityComparer<MultiSet<T>> GetEqualityComparer() {
    //        return MultiSetEqualityComparer<T>.Instance;
    //    }


    //    /* .-----------------------------------------------------------------------
    //       |
    //       |  Class MultiSet<T>.ReadOnly
    //       |
    //       '-----------------------------------------------------------------------
    //    */
    //    public class ReadOnly : IReadOnlyAdapter<MultiSet<T>>, ICollection, IEnumerable<KeyValuePair<T, int>>, IEnumerable, IContentEquatable<MultiSet<T>.ReadOnly>, ISerializable
    //    {
    //        private MultiSet<T> mSet;


    //        public ReadOnly(MultiSet<T> set) {
    //            Utils.ThrowException(set == null ? new ArgumentNullException("set") : null);
    //            mSet = set;
    //        }


    //        public ReadOnly(BinarySerializer reader) {
    //            mSet = new MultiSet<T>(reader); // throws ArgumentNullException, serialization-related exceptions
    //        }


    //        public IEqualityComparer<T> Comparer {
    //            get { return mSet.Comparer; }
    //        }


    //        public T[] ToArray() {
    //            return mSet.ToArray();
    //        }


    //        public T Any {
    //            get { return mSet.Any; }
    //        }


    //        public NewT[] ToArray<NewT>() {
    //            return mSet.ToArray<NewT>();
    //        }


    //        public NewT[] ToArray<NewT>(IFormatProvider fmtProvider) {
    //            return mSet.ToArray<NewT>(fmtProvider);
    //        }


    //        public int CountUnique {
    //            get { return mSet.CountUnique; }
    //        }


    //        public int GetCount(T item) {
    //            return mSet.GetCount(item);
    //        }


    //        public List<KeyDat<int, T>> ToList() {
    //            return mSet.ToList();
    //        }


    //        public override string ToString() {
    //            return mSet.ToString();
    //        }


    //        // *** IReadOnlyAdapter interface implementation ***


    //        public MultiSet<T> GetWritableCopy() {
    //            return mSet.Clone();
    //        }


    //        object IReadOnlyAdapter.GetWritableCopy() {
    //            return GetWritableCopy();
    //        }


    //        public MultiSet<T> Inner {
    //            get { return mSet; }
    //        }


    //        object IReadOnlyAdapter.Inner {
    //            get { return Inner; }
    //        }


    //        // *** Partial ICollection<T> interface implementation ***


    //        public bool Contains(T item) {
    //            return mSet.Contains(item);
    //        }


    //        public void CopyTo(T[] array, int index) {
    //            mSet.CopyTo(array, index);
    //        }


    //        public int Count {
    //            get { return mSet.Count; }
    //        }


    //        public bool IsReadOnly {
    //            get { return true; }
    //        }


    //        // *** ICollection interface implementation ***


    //        void ICollection.CopyTo(Array array, int index) {
    //            ((ICollection)mSet).CopyTo(array, index);
    //        }


    //        bool ICollection.IsSynchronized {
    //            get { throw new NotSupportedException(); }
    //        }


    //        object ICollection.SyncRoot {
    //            get { throw new NotSupportedException(); }
    //        }


    //        // *** IEnumerable interface implementation ***


    //        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() {
    //            return mSet.GetEnumerator();
    //        }


    //        IEnumerator IEnumerable.GetEnumerator() {
    //            return ((IEnumerable)mSet).GetEnumerator();
    //        }


    //        // *** IContentEquatable<MultiSet<T>.ReadOnly> interface implementation ***


    //        public bool ContentEquals(MultiSet<T>.ReadOnly other) {
    //            return other != null && mSet.ContentEquals(other.Inner);
    //        }


    //        bool IContentEquatable.ContentEquals(object other) {
    //            Utils.ThrowException((other != null && !(other is MultiSet<T>.ReadOnly)) ? new ArgumentTypeException("other") : null);
    //            return ContentEquals((MultiSet<T>.ReadOnly)other);
    //        }


    //        // *** ISerializable interface implementation ***


    //        public void Save(BinarySerializer writer) {
    //            mSet.Save(writer);
    //        }


    //        // *** Equality comparer ***


    //        public static IEqualityComparer<MultiSet<T>.ReadOnly> GetEqualityComparer() {
    //            return MultiSetEqualityComparer<T>.Instance;
    //        }
    //    }

   
}