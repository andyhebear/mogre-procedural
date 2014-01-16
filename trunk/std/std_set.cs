
# define NET_4_0


namespace Mogre_Procedural.std
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Diagnostics;


    /// <summary>
    /// like c++ std::set 
    /// </summary>
    public class std_set<T> : SortedSet<T>
    {
        public std_set()
            : base() {

        }

        public std_set(IComparer<T> comparer)
            : base(comparer) {
        }

        public std_set(IEnumerable<T> collection)
            : base(collection) {
        }

        public std_set(IEnumerable<T> collection, IComparer<T> comparer)
            : base(collection, comparer) {

        }

        public int begin() {
            return 0;
        }

        public int end() {
            return base.Count;
        }
        public bool empty() {
            return base.Count == 0;
        }
        public int size() {
            return base.Count;
        }
        public int max_size() {
            return int.MaxValue;
        }
        public void clear() {
            base.Clear();
        }
        //
        public bool insert(T value) {
            return base.Add(value);
        }
        public bool insert(uint pos, T value) {
            return base.Add(value);
        }
        public void insert(T[] array, int beginpos, int beforeendpos) {
            for (int i = beginpos; i < beforeendpos; i++) {
                base.Add(array[i]);
            }
        }

        public bool erase(int pos, bool index) {
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            return base.Remove(array[pos]);
        }
        /// <summary>
        /// 原始返回 0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool erase(T value) {
            return base.Remove(value);
        }
        public void erase(int beginpos, int beforeendpos) {
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            for (int i = beforeendpos - 1; i >= beginpos; i--) {
                base.Remove(array[i]);
            }
        }

        /// <summary>
        /// T type must same
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_other"></param>
        public static void swap(std_set<T> _this, std_set<T> _other) {
            std_set<T> temp = _this;
            _this = _other;
            _other = temp;
        }

        public bool emplace(T value) {
            return base.Add(value);
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
            Enumerator rt = base.GetEnumerator();
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
        /// 因为是无重复 所以返回0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int count(T value) {
            int f = find(value);
            return f == -1 ? 0 : 1;
        }

        public int lower_bound(T key) { 
            int f=find(key);
            return f;
        }
        public int upper_bound(T key) {
            int f = find(key);
            return f+1;
        }
        public std_pair<int, int> equal_range(T key) { 
             int f=find(key);
             return new std_pair<int, int>(f,f+1);
        }
        public T[] get_allocator() {
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            return array;
        }
    }

    /// <summary>
    /// like std::unordered_set
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class std_unordered_set<T> : HashSet<T>
    {
        public std_unordered_set()
            : base() {

        }

        public std_unordered_set(IEqualityComparer<T> comparer)
            : base(comparer) {
        }

        public std_unordered_set(IEnumerable<T> collection)
            : base(collection) {
        }

        public std_unordered_set(IEnumerable<T> collection, IEqualityComparer<T> comparer)
            : base(collection, comparer) {

        }

        public int begin() {
            return 0;
        }

        public int end() {
            return base.Count;
        }
        public bool empty() {
            return base.Count == 0;
        }
        public int size() {
            return base.Count;
        }
        public int max_size() {
            return int.MaxValue;
        }
        public void clear() {
            base.Clear();
        }

        //
        public bool insert(T value) {
            return base.Add(value);
        }
        public bool insert(uint pos, T value) {
            return base.Add(value);
        }
        public void insert(T[] array, int beginpos, int beforeendpos) {
            for (int i = beginpos; i < beforeendpos; i++) {
                base.Add(array[i]);
            }
        }

        public bool erase(int pos, bool index) {
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            return base.Remove(array[pos]);
        }
        /// <summary>
        /// 原始返回 0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool erase(T value) {
            return base.Remove(value);
        }
        public void erase(int beginpos, int beforeendpos) {
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            for (int i = beforeendpos - 1; i >= beginpos; i--) {
                base.Remove(array[i]);
            }
        }

        /// <summary>
        /// T type must same
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_other"></param>
        public static void swap(std_unordered_set<T> _this, std_unordered_set<T> _other) {
            std_unordered_set<T> temp = _this;
            _this = _other;
            _other = temp;
        }

        public bool emplace(T value) {
            return base.Add(value);
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
            Enumerator rt = base.GetEnumerator();
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
        /// 因为是无重复 所以返回0或者1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int count(T value) {
            int f = find(value);
            return f == -1 ? 0 : 1;
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
            T[] array = new T[base.Count];
            base.CopyTo(array, 0);
            return array;
        }
    }


    ///// <summary>
    ///// std:set 相当 HashSet<T>类，但这里需要明确，STL中的set是以红黑树作为底层数据结构，
    ///// 而C#中HashSet<T>类是以哈希表作为底层数据结构，因为其两者使用数据结构的不同，
    ///// 从而导致查询效率不同，set查找的花费时间为O（logn），这也是红黑树查询时间，
    ///// 而HashSet的查询花费时间为O（1）。
    ///// 一个集合(set)是一个容器，它其中所包含的元素的值是唯一的。
    ///// 这在收集一个数据的具体值的时候是有用的。
    ///// 集合中的元素按一定的顺序排列，并被作为集合中的实例。一个集合通过一个链表来组织，
    ///// 在插入操作和删除操作上比向量(vector)快，但查找或添加末尾的元素时会有些慢。
    ///// 具体实现采用了红黑树的平衡二叉树的数据结构。
    ///// </summary>
    //public interface Istd_set<T>
    //{
    //    /// <summary>
    //    /// 返回一个迭代器的第一个元素的容器
    //    /// </summary>
    //    /// <returns>迭代器第一个元素</returns>
    //    public T[] begin();
    //    /// <summary>
    //    /// 返回一个迭代器的第一个元素的容器c++11
    //    /// </summary>
    //    /// <returns>迭代器第一个元素</returns>
    //    // public T[] cbegin();
    //    /// <summary>
    //    /// 返回一个迭代的最后一个元素的容器元素.  这个元素作为一个占位符;试图访问它导致未定义的行为
    //    /// </summary>
    //    /// <returns>迭代器的最后一个元素的元素</returns>
    //    public T[] end();
    //    /// <summary>
    //    /// 返回一个迭代的最后一个元素的容器元素.  这个元素作为一个占位符;试图访问它导致未定义的行为c++11
    //    /// </summary>
    //    /// <returns>迭代器的最后一个元素的元素 </returns>
    //    // public T[] cend();

    //    //
    //    /// <summary>
    //    /// 相反的容器返回一个反向迭代器的第一个元素。它对应的非反转的容器中的最后一个元素
    //    /// </summary>
    //    /// <returns>反向迭代器第一个元素</returns>
    //    //public T[] rbegin();
    //    /// <summary>
    //    /// 相反的容器返回一个反向迭代器的第一个元素。它对应的非反转的容器中的最后一个元素c++11
    //    /// </summary>
    //    /// <returns>反向迭代器第一个元素</returns>
    //    //public T[] crbegin();
    //    /// <summary>
    //    /// 返回一个反向迭代器的反转容器的最后一个元素的元素。它对应于前面的元素的非反转的容器中的第一个元素。这个元素作为一个占位符，试图访问它导致未定义的行为. 
    //    /// </summary>
    //    /// <returns>反向迭代的最后一个元素的元素</returns>
    //    // public T[] rend();
    //    /// <summary>
    //    /// 返回一个反向迭代器的反转容器的最后一个元素的元素。它对应于前面的元素的非反转的容器中的第一个元素。这个元素作为一个占位符，试图访问它导致未定义的行为.c++11 
    //    /// </summary>
    //    /// <returns>反向迭代的最后一个元素的元素</returns>
    //    // public T[] crend();
    //    /// <summary>
    //    /// 检查，如果容器有任何元素，即是否begin() == end().
    //    /// </summary>
    //    /// <returns>true如果容器是空的</returns>
    //    public bool empty();
    //    /// <summary>
    //    /// Returns the number of elements in the container在容器中的元素的数量
    //    /// </summary>
    //    /// <returns>std::distance(begin(), end())</returns>
    //    public int size();
    //    /// <summary>
    //    /// 返回元素的最大数量的容器是能够容纳由于系统或库实现限制，即std::distance(begin(), end())最大的集装箱
    //    /// </summary>
    //    /// <returns></returns>
    //    public int max_size();
    //    /// <summary>
    //    /// 从容器中移除所有元素。 过去的end迭代器不会失效.
    //    /// </summary>
    //    public void clear();
    //    //begin() 返回指向第一个元素的迭代器
    //    //clear() 清除所有元素
    //    //public void insert(ICollection<T> @set,int start,int end);
    //    //public void insert(int first, int last);
    //    //public void insert(ICollection<T> @set);
    //    //public T insert(T @set);
    //    //public T insert(T @set, int pos);
    //    //public void erase(int pos);
    //    //public void erase(T first, T last);
    //    //public void erase(T key);
    //    ///// <summary>
    //    ///// 容器内容交换
    //    ///// </summary>
    //    ///// <param name="ther"></param>
    //    //public void swap(std_set<T> @ther);
    //    /// <summary>
    //    /// 返回某个值元素的个数
    //    /// </summary>
    //    /// <returns></returns>
    //    public int count(T key);
    //    public int find(T key);
    //    ///// <summary>
    //    ///// Returns a range containing all elements with the given key in the container. 
    //    ///// The range is defined by two iterators, one pointing to the first element 
    //    ///// that is not less than key and another pointing to the first element
    //    ///// greater than key. The first iterator may be alternatively obtained with
    //    ///// lower_bound(), the second - with upper_bound(). 
    //    ///// </summary>
    //    ///// <param name="key"></param>
    //    ///// <returns></returns>
    //    //public KeyValuePair<T, T> equal_range(T key);
    //    /// <summary>
    //    /// 返回一个迭代器指向的第一个元素是“不低于”比key.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns>迭代器指向的第一个元素是“少”比key。如果没有这样的元素被发现，过去的结束迭代器（见end()）返回</returns>
    //    //public T lower_bound(T key);
    //    //
    //    /// <summary>
    //    /// 迭代器指向的第一个元素是“更大的”key。如果没有这样的元素被发现，过去的结束（见end()）返回迭代器.
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    //public T upper_bound(T key);
    //    //end() 返回指向最后一个元素之后的迭代器，不是最后一个元素

    //    //equal_range() 返回集合中与给定值相等的上下限的两个迭代器

    //    //erase() 删除集合中的元素

    //    //find() 返回一个指向被查找到元素的迭代器

    //    //get_allocator() 返回集合的分配器

    //    //insert() 在集合中插入元素

    //    //lower_bound() 返回指向大于（或等于）某值的第一个元素的迭代器

    //    //key_comp() 返回一个用于元素间值比较的函数

    //    //max_size() 返回集合能容纳的元素的最大限值

    //    //rbegin() 返回指向集合中最后一个元素的反向迭代器

    //    //rend() 返回指向集合中第一个元素的反向迭代器

    //    //size() 集合中元素的数目

    //    //swap() 交换两个集合变量

    //    //upper_bound() 返回大于某个值元素的迭代器

    //    //value_comp() 返回一个用于比较元素间的值的函数
    //    //std::set_intersection() :这个函数是求两个集合的交集。
    //    //std::set_union() :求两个集合的并集
    //    //std::set_difference（）：差集
    //    //std::set_symmetric_difference（）：得到的结果是第一个迭代器相对于第二个的差集并 上第二个相当于第一个的差集
    //}




    //public class std_set<T> : IEnumerable<T>
    //{
    //    private SortedDictionary<T, object> items = new SortedDictionary<T, object>();

    //    public std_set() {
    //    }

    //    public std_set(params T[] items) {
    //        foreach (T item in items)
    //            Add(item);
    //    }


    //    public bool Add(T item) {
    //        if (Contains(item))
    //            return false;

    //        items[item] = null;
    //        return true;
    //    }

    //    public bool Contains(T item) {
    //        return items.ContainsKey(item);
    //    }

    //    public void Remove(T item) {
    //        items.Remove(item);
    //    }

    //    public void Remove(IEnumerable<T> itemList) {
    //        foreach (T item in itemList)
    //            items.Remove(item);
    //    }

    //    public int Count {
    //        get {
    //            return items.Count;
    //        }
    //    }

    //    public IEnumerator<T> GetEnumerator() {
    //        return items.Keys.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //}


    //public class std_unordered_set<T> : IEnumerable<T>
    //{
    //    private Dictionary<T, object> items = new Dictionary<T, object>();

    //    public std_set() {
    //    }

    //    public std_set(params T[] items) {
    //        foreach (T item in items)
    //            Add(item);
    //    }


    //    public bool Add(T item) {
    //        if (Contains(item))
    //            return false;

    //        items[item] = null;
    //        return true;
    //    }

    //    public bool Contains(T item) {
    //        return items.ContainsKey(item);
    //    }

    //    public void Remove(T item) {
    //        items.Remove(item);
    //    }

    //    public void Remove(IEnumerable<T> itemList) {
    //        foreach (T item in itemList)
    //            items.Remove(item);
    //    }

    //    public int Count {
    //        get {
    //            return items.Count;
    //        }
    //    }

    //    public IEnumerator<T> GetEnumerator() {
    //        return items.Keys.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //}









#if NET_20 || WINDOWS_PHONE || XBOX


    //public class HashSet<T> : ICollection<T>
    //{

    //    private readonly Dictionary<T, object> dict;



    //    public HashSet() {

    //        dict = new Dictionary<T, object>();

    //    }



    //    public HashSet(IEnumerable<T> items)

    //        : this() {

    //        if (items == null) {

    //            return;

    //        }



    //        foreach (T item in items) {

    //            Add(item);

    //        }

    //    }



    //    public HashSet<T> NullSet { get { return new HashSet<T>(); } }



    //    #region ICollection<T> Members



    //    public void Add(T item) {

    //        if (null == item) {

    //            throw new ArgumentNullException("item");

    //        }



    //        dict[item] = null;

    //    }



    //    /// <summary>

    //    /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.

    //    /// </summary>

    //    /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>

    //    public void Clear() {

    //        dict.Clear();

    //    }



    //    public bool Contains(T item) {

    //        return dict.ContainsKey(item);

    //    }



    //    /// <summary>

    //    /// Copies the items of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.

    //    /// </summary>

    //    /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the items copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of items in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type T cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>

    //    public void CopyTo(T[] array, int arrayIndex) {

    //        if (array == null) throw new ArgumentNullException("array");

    //        if (arrayIndex < 0 || arrayIndex >= array.Length || arrayIndex >= Count) {

    //            throw new ArgumentOutOfRangeException("arrayIndex");

    //        }



    //        dict.Keys.CopyTo(array, arrayIndex);

    //    }



    //    /// <summary>

    //    /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.

    //    /// </summary>

    //    /// <returns>

    //    /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.

    //    /// </returns>

    //    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>

    //    public bool Remove(T item) {

    //        return dict.Remove(item);

    //    }



    //    /// <summary>

    //    /// Gets the number of items contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.

    //    /// </summary>

    //    /// <returns>

    //    /// The number of items contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.

    //    /// </returns>

    //    public int Count {

    //        get { return dict.Count; }

    //    }



    //    /// <summary>

    //    /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.

    //    /// </summary>

    //    /// <returns>

    //    /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.

    //    /// </returns>

    //    public bool IsReadOnly {

    //        get {

    //            return false;

    //        }

    //    }



    //    #endregion



    //    public HashSet<T> Union(HashSet<T> set) {

    //        HashSet<T> unionSet = new HashSet<T>(this);



    //        if (null == set) {

    //            return unionSet;

    //        }



    //        foreach (T item in set) {

    //            if (unionSet.Contains(item)) {

    //                continue;

    //            }



    //            unionSet.Add(item);

    //        }



    //        return unionSet;

    //    }



    //    public HashSet<T> Subtract(HashSet<T> set) {

    //        HashSet<T> subtractSet = new HashSet<T>(this);



    //        if (null == set) {

    //            return subtractSet;

    //        }



    //        foreach (T item in set) {

    //            if (!subtractSet.Contains(item)) {

    //                continue;

    //            }



    //            subtractSet.dict.Remove(item);

    //        }



    //        return subtractSet;

    //    }



    //    public bool IsSubsetOf(HashSet<T> set) {

    //        HashSet<T> setToCompare = set ?? NullSet;



    //        foreach (T item in this) {

    //            if (!setToCompare.Contains(item)) {

    //                return false;

    //            }

    //        }



    //        return true;

    //    }



    //    public HashSet<T> Intersection(HashSet<T> set) {

    //        HashSet<T> intersectionSet = NullSet;



    //        if (null == set) {

    //            return intersectionSet;

    //        }



    //        foreach (T item in this) {

    //            if (!set.Contains(item)) {

    //                continue;

    //            }



    //            intersectionSet.Add(item);

    //        }



    //        foreach (T item in set) {

    //            if (!Contains(item) || intersectionSet.Contains(item)) {

    //                continue;

    //            }



    //            intersectionSet.Add(item);

    //        }



    //        return intersectionSet;

    //    }



    //    public bool IsProperSubsetOf(HashSet<T> set) {

    //        HashSet<T> setToCompare = set ?? NullSet;



    //        // A is a proper subset of a if the b is a subset of a and a != b

    //        return (IsSubsetOf(setToCompare) && !setToCompare.IsSubsetOf(this));

    //    }



    //    public bool IsSupersetOf(HashSet<T> set) {

    //        HashSet<T> setToCompare = set ?? NullSet;



    //        foreach (T item in setToCompare) {

    //            if (!Contains(item)) {

    //                return false;

    //            }

    //        }



    //        return true;

    //    }



    //    public bool IsProperSupersetOf(HashSet<T> set) {

    //        HashSet<T> setToCompare = set ?? NullSet;



    //        // B is a proper superset of a if b is a superset of a and a != b

    //        return (IsSupersetOf(setToCompare) && !setToCompare.IsSupersetOf(this));

    //    }



    //    public List<T> ToList() {

    //        return new List<T>(this);

    //    }



    //    //#region Implementation of ISerializable



    //    ///// <summary>

    //    ///// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.

    //    ///// </summary>

    //    ///// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>

    //    //public void GetObjectData(SerializationInfo info, StreamingContext context)

    //    //{

    //    //    if (info == null) throw new ArgumentNullException("info");

    //    //    dict.GetObjectData(info, context);

    //    //}



    //    //#endregion



    //    //#region Implementation of IDeserializationCallback



    //    ///// <summary>

    //    ///// Runs when the entire object graph has been deserialized.

    //    ///// </summary>

    //    ///// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented. </param>

    //    //public void OnDeserialization(object sender)

    //    //{

    //    //    dict.OnDeserialization(sender);

    //    //}



    //    //#endregion



    //    #region Implementation of IEnumerable



    //    IEnumerator IEnumerable.GetEnumerator() {

    //        return dict.Keys.GetEnumerator();

    //    }



    //    public Dictionary<T, object>.KeyCollection.Enumerator GetEnumerator() {

    //        return dict.Keys.GetEnumerator();

    //    }



    //    /// <summary>

    //    /// Returns an enumerator that iterates through a collection.

    //    /// </summary>

    //    /// <returns>

    //    /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.

    //    /// </returns>

    //    /// <filterpriority>2</filterpriority>

    //    IEnumerator<T> IEnumerable<T>.GetEnumerator() {

    //        // note: this causes boxing - it is just here because the

    //        // IEnumerable<T> contract demands it.

    //        Debug.Assert(false);

    //        return dict.Keys.GetEnumerator();

    //    }







    //    #endregion



    //}





#endif

    //public class OrderedSet<T> : IEnumerable<T>
    //{
    //    private std_set<T> items = new std_set<T>();
    //    private List<T> order = new List<T>();

    //    public void Add(T item) {
    //        if (items.Add(item))
    //            order.Add(item);
    //    }

    //    public T this[int index] {
    //        get {
    //            return order[index];
    //        }
    //    }


    //    public int Count {
    //        get {
    //            return items.Count;
    //        }
    //    }

    //    public IEnumerator<T> GetEnumerator() {
    //        return order.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }
    //}





    //public interface ICloneable<T> : ICloneable
    //{
    //    new T Clone();
    //}
    //public interface IContentEquatable
    //{
    //    bool ContentEquals(object other);
    //}

    //public interface IContentEquatable<T> : IContentEquatable
    //{
    //    bool ContentEquals(T other);
    //}
    //public interface IDeeplyCloneable
    //{
    //    object DeepClone();
    //}

    //public interface IDeeplyCloneable<T> : IDeeplyCloneable
    //{
    //    new T DeepClone();
    //}
    //public interface IEnumerableList : IEnumerable
    //{
    //    object this[int index] { get; }
    //    int Count { get; }
    //}
    //public interface IEnumerableList<T> : IEnumerable<T>, IEnumerableList
    //{
    //    new T this[int index] { get; }
    //}
    //public interface IReadOnlyAdapter
    //{
    //    object GetWritableCopy();
    //    object Inner { get; }
    //}
    //public interface IReadOnlyAdapter<T> : IReadOnlyAdapter
    //{
    //    new T GetWritableCopy();
    //    new T Inner { get; }
    //}
    //public interface ISerializable
    //{
    //    // *** note that you need to implement the constructor that loads an instance if the class implements ISerializable
    //    void Save(BinarySerializer writer);
    //}
    //public interface IXmlSerializable
    //{
    //    // *** note that you should implement a constructor that loads an instance if the class implements IXmlSerializable
    //    void SaveXml(XmlWriter writer);
    //}

    //public class Set<T> : ICollection<T>, ICollection, IEnumerable<T>, ICloneable<Set<T>>, IDeeplyCloneable<Set<T>>, IContentEquatable<Set<T>>, ISerializable
    //{
    //    private Dictionary<T, object> mItems
    //        = new Dictionary<T, object>();


    //    public Set() {
    //    }


    //    public Set(IEqualityComparer<T> comparer) {
    //        mItems = new Dictionary<T, object>(comparer);
    //    }


    //    //public Set(BinarySerializer reader) {
    //    //    Load(reader); // throws ArgumentNullException, serialization-related exceptions
    //    //}


    //    //public Set(BinarySerializer reader, IEqualityComparer<T> comparer) {
    //    //    mItems = new Dictionary<T, object>(comparer);
    //    //    Load(reader); // throws ArgumentNullException
    //    //}


    //    public Set(IEnumerable<T> items) {
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public Set(IEnumerable<T> items, IEqualityComparer<T> comparer) {
    //        mItems = new Dictionary<T, object>(comparer);
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public void SetItems(IEnumerable<T> items) {
    //        mItems.Clear();
    //        AddRange(items); // throws ArgumentNullException
    //    }


    //    public void AddRange(IEnumerable<T> items) {
    //        Utils.ThrowException(items == null ? new ArgumentNullException("items") : null);
    //        foreach (T item in items) {
    //            if (!mItems.ContainsKey(item)) // throws ArgumentNullException
    //            {
    //                mItems.Add(item, null);
    //            }
    //        }
    //    }


    //    public IEqualityComparer<T> Comparer {
    //        get { return mItems.Comparer; }
    //    }

    //    /// <summary>
    //    /// 并集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Union(Set<T> a, Set<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        Set<T> c = a.Clone(); // *** inherits comparer from a (b is expected to have the same comparer)
    //        c.AddRange(b);
    //        return c;
    //    }

    //    /// <summary>
    //    /// 并集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Union(Set<T>.ReadOnly a, Set<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Union(a.Inner, b.Inner);
    //    }

    //    /// <summary>
    //    /// 交集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Intersection(Set<T> a, Set<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        Set<T> c = new Set<T>(a.Comparer); // *** inherits comparer from a (b is expected to have the same comparer)
    //        if (b.Count < a.Count) { Set<T> tmp; tmp = a; a = b; b = tmp; }
    //        foreach (T item in a) {
    //            if (b.Contains(item)) { c.Add(item); }
    //        }
    //        return c;
    //    }

    //    /// <summary>
    //    /// 交集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Intersection(Set<T>.ReadOnly a, Set<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Intersection(a.Inner, b.Inner);
    //    }

    //    /// <summary>
    //    /// 差集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Difference(Set<T> a, Set<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        Set<T> c = new Set<T>(a.Comparer); // *** inherits comparer from a (b is expected to have the same comparer)
    //        foreach (T item in a) {
    //            if (!b.Contains(item)) { c.Add(item); }
    //        }
    //        return c;
    //    }

    //    /// <summary>
    //    /// 差集
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static Set<T> Difference(Set<T>.ReadOnly a, Set<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return Difference(a.Inner, b.Inner);
    //    }

    //    /// <summary>
    //    /// 相似度
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static double JaccardSimilarity(Set<T> a, Set<T> b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        Set<T> c = Intersection(a, b);
    //        double div = (double)(a.Count + b.Count - c.Count);
    //        if (div == 0) { return 1; } // *** if both sets are empty, the similarity is 1
    //        return (double)c.Count / div;
    //    }

    //    /// <summary>
    //    /// 相似度
    //    /// </summary>
    //    /// <param name="a"></param>
    //    /// <param name="b"></param>
    //    /// <returns></returns>
    //    public static double JaccardSimilarity(Set<T>.ReadOnly a, Set<T>.ReadOnly b) {
    //        Utils.ThrowException(a == null ? new ArgumentNullException("a") : null);
    //        Utils.ThrowException(b == null ? new ArgumentNullException("b") : null);
    //        return JaccardSimilarity(a.Inner, b.Inner);
    //    }


    //    public T[] ToArray() {
    //        T[] array = new T[mItems.Count];
    //        CopyTo(array, 0);
    //        return array;
    //    }


    //    public T Any {
    //        get {
    //            foreach (KeyValuePair<T, object> item in mItems) {
    //                return item.Key;
    //            }
    //            throw new InvalidOperationException();
    //        }
    //    }


    //    public NewT[] ToArray<NewT>() {
    //        return ToArray<NewT>(/*fmtProvider=*/null); // throws InvalidCastException, FormatException, OverflowException
    //    }


    //    public NewT[] ToArray<NewT>(IFormatProvider fmtProvider) {
    //        NewT[] array = new NewT[mItems.Count];
    //        int i = 0;
    //        foreach (T item in mItems.Keys) {
    //            array[i++] = (NewT)Utils.ChangeType(item, typeof(NewT), fmtProvider); // throws InvalidCastException, FormatException, OverflowException
    //        }
    //        return array;
    //    }


    //    public void RemoveRange(IEnumerable<T> items) {
    //        Utils.ThrowException(items == null ? new ArgumentNullException("items") : null);
    //        foreach (T item in items) {
    //            mItems.Remove(item); // throws ArgumentNullException
    //        }
    //    }


    //    public override string ToString() {
    //        StringBuilder str = new StringBuilder("{");
    //        foreach (T item in mItems.Keys) {
    //            str.Append(" ");
    //            str.Append(item);
    //        }
    //        str.Append(" }");
    //        return str.ToString();
    //    }


    //    // *** ICollection<T> interface implementation ***


    //    public void Add(T item) {
    //        if (!mItems.ContainsKey(item)) // throws ArgumentNullException
    //        {
    //            mItems.Add(item, null);
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
    //        Utils.ThrowException(index + mItems.Count > array.Length ? new ArgumentOutOfRangeException("index") : null);
    //        foreach (T item in mItems.Keys) {
    //            array.SetValue(item, index++);
    //        }
    //    }


    //    public int Count {
    //        get { return mItems.Count; }
    //    }


    //    public bool IsReadOnly {
    //        get { return false; }
    //    }


    //    public bool Remove(T item) {
    //        return mItems.Remove(item); // throws ArgumentNullException
    //    }


    //    // *** ICollection interface implementation ***


    //    void ICollection.CopyTo(Array array, int index) {
    //        Utils.ThrowException(array == null ? new ArgumentNullException("array") : null);
    //        Utils.ThrowException(index + mItems.Count > array.Length ? new ArgumentOutOfRangeException("index") : null);
    //        foreach (T item in mItems.Keys) {
    //            array.SetValue(item, index++);
    //        }
    //    }


    //    bool ICollection.IsSynchronized {
    //        get { throw new NotSupportedException(); }
    //    }


    //    object ICollection.SyncRoot {
    //        get { throw new NotSupportedException(); }
    //    }


    //    // *** IEnumerable<T> interface implementation ***


    //    public IEnumerator<T> GetEnumerator() {
    //        return mItems.Keys.GetEnumerator();
    //    }


    //    // *** IEnumerable interface implementation ***


    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }


    //    // *** ICloneable interface implementation ***


    //    public Set<T> Clone() {
    //        return new Set<T>(mItems.Keys, mItems.Comparer);
    //    }


    //    object ICloneable.Clone() {
    //        return Clone();
    //    }


    //    // *** IDeeplyCloneable interface implementation ***


    //    public Set<T> DeepClone() {
    //        Set<T> clone = new Set<T>(mItems.Comparer);
    //        foreach (T item in mItems.Keys) {
    //            clone.Add((T)Utils.Clone(item, /*deepClone=*/true));
    //        }
    //        return clone;
    //    }


    //    object IDeeplyCloneable.DeepClone() {
    //        return DeepClone();
    //    }


    //    // *** IContentEquatable<Set<T>> interface implementation ***


    //    public bool ContentEquals(Set<T> other) {
    //        if (other == null || Count != other.Count) { return false; }
    //        foreach (T item in mItems.Keys) {
    //            if (!other.Contains(item)) { return false; }
    //        }
    //        return true;
    //    }


    //    bool IContentEquatable.ContentEquals(object other) {
    //        Utils.ThrowException((other != null && !(other is Set<T>)) ? new ArgumentException("other") : null);
    //        return ContentEquals((Set<T>)other);
    //    }


    //    // *** ISerializable interface implementation ***


    //    //public void Save(BinarySerializer writer) {
    //    //    Utils.ThrowException(writer == null ? new ArgumentNullException("writer") : null);
    //    //    // the following statements throw serialization-related exceptions 
    //    //    writer.WriteInt(mItems.Count);
    //    //    foreach (KeyValuePair<T, object> item in mItems) {
    //    //        writer.WriteValueOrObject<T>(item.Key);
    //    //    }
    //    //}


    //    //public void Load(BinarySerializer reader) {
    //    //    Utils.ThrowException(reader == null ? new ArgumentNullException("reader") : null);
    //    //    mItems.Clear();
    //    //    // the following statements throw serialization-related exceptions 
    //    //    int count = reader.ReadInt();
    //    //    for (int i = 0; i < count; i++) {
    //    //        mItems.Add(reader.ReadValueOrObject<T>(), null);
    //    //    }
    //    //}


    //    // *** Implicit cast to a read-only adapter ***


    //    public static implicit operator Set<T>.ReadOnly(Set<T> set) {
    //        if (set == null) { return null; }
    //        return new Set<T>.ReadOnly(set);
    //    }


    //    // *** Equality comparer ***


    //    public static IEqualityComparer<Set<T>> GetEqualityComparer() {
    //        return SetEqualityComparer<T>.Instance;
    //    }


    //    /* .-----------------------------------------------------------------------
    //       |
    //       |  Class Set<T>.ReadOnly
    //       |
    //       '-----------------------------------------------------------------------
    //    */
    //    public class ReadOnly : IReadOnlyAdapter<Set<T>>, ICollection, IEnumerable<T>, IEnumerable, IContentEquatable<Set<T>.ReadOnly>, ISerializable
    //    {
    //        private Set<T> mSet;


    //        public ReadOnly(Set<T> set) {
    //            Utils.ThrowException(set == null ? new ArgumentNullException("set") : null);
    //            mSet = set;
    //        }


    //        //public ReadOnly(BinarySerializer reader) {
    //        //    mSet = new Set<T>(reader); // throws ArgumentNullException, serialization-related exceptions
    //        //}


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


    //        public override string ToString() {
    //            return mSet.ToString();
    //        }


    //        // *** IReadOnlyAdapter interface implementation ***


    //        public Set<T> GetWritableCopy() {
    //            return mSet.Clone();
    //        }


    //        object IReadOnlyAdapter.GetWritableCopy() {
    //            return GetWritableCopy();
    //        }


    //        public Set<T> Inner {
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


    //        // *** IEnumerable<T> interface implementation ***


    //        public IEnumerator<T> GetEnumerator() {
    //            return mSet.GetEnumerator();
    //        }


    //        // *** IEnumerable interface implementation ***


    //        IEnumerator IEnumerable.GetEnumerator() {
    //            return ((IEnumerable)mSet).GetEnumerator();
    //        }


    //        // *** IContentEquatable<Set<T>.ReadOnly> interface implementation ***


    //        public bool ContentEquals(Set<T>.ReadOnly other) {
    //            return other != null && mSet.ContentEquals(other.Inner);
    //        }


    //        bool IContentEquatable.ContentEquals(object other) {
    //            Utils.ThrowException((other != null && !(other is Set<T>.ReadOnly)) ? new ArgumentTypeException("other") : null);
    //            return ContentEquals((Set<T>.ReadOnly)other);
    //        }


    //        // *** ISerializable interface implementation ***


    //        //public void Save(BinarySerializer writer) {
    //        //    mSet.Save(writer);
    //        //}


    //        // *** Equality comparer ***


    //        public static IEqualityComparer<Set<T>.ReadOnly> GetEqualityComparer() {
    //            return SetEqualityComparer<T>.Instance;
    //        }
    //    }
    //}

    //public class SetEqualityComparer<T> : IEqualityComparer<Set<T>>, IEqualityComparer<Set<T>.ReadOnly>, IEqualityComparer
    //{
    //    private static SetEqualityComparer<T> mInstance
    //        = new SetEqualityComparer<T>();


    //    public static SetEqualityComparer<T> Instance {
    //        get { return mInstance; }
    //    }


    //    public bool Equals(Set<T> x, Set<T> y) {
    //        if (x == null && y == null) { return true; }
    //        if (x == null || y == null) { return false; }
    //        return x.Count == y.Count && Set<T>.Difference(x, y).Count == 0;
    //    }


    //    public bool Equals(Set<T>.ReadOnly x, Set<T>.ReadOnly y) {
    //        if (x == null && y == null) { return true; }
    //        if (x == null || y == null) { return false; }
    //        return Equals(x.Inner, y.Inner);
    //    }


    //    public int GetHashCode(Set<T> obj) {
    //        Utils.ThrowException(obj == null ? new ArgumentNullException("obj") : null);
    //        int hashCode = 0;
    //        foreach (T item in obj) { hashCode ^= item.GetHashCode(); }
    //        return hashCode;
    //    }


    //    public int GetHashCode(Set<T>.ReadOnly obj) {
    //        Utils.ThrowException(obj == null ? new ArgumentNullException("obj") : null);
    //        return GetHashCode(obj.Inner);
    //    }


    //    bool IEqualityComparer.Equals(object x, object y) {
    //        if (x is Set<T>) { x = new Set<T>.ReadOnly((Set<T>)x); }
    //        if (y is Set<T>) { y = new Set<T>.ReadOnly((Set<T>)y); }
    //        Utils.ThrowException((x != null && !(x is Set<T>.ReadOnly)) ? new ArgumentException("x") : null);
    //        Utils.ThrowException((y != null && !(y is Set<T>.ReadOnly)) ? new ArgumentException("y") : null);
    //        return Equals((Set<T>.ReadOnly)x, (Set<T>.ReadOnly)y);
    //    }


    //    int IEqualityComparer.GetHashCode(object obj) {
    //        if (obj is Set<T>) { obj = new Set<T>.ReadOnly((Set<T>)obj); }
    //        Utils.ThrowException((obj != null && !(obj is Set<T>.ReadOnly)) ? new ArgumentException("obj") : null);
    //        return GetHashCode((Set<T>.ReadOnly)obj); // throws ArgumentNullException
    //    }
    //}

}
