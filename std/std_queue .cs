using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Mogre_Procedural.std
{
    //[ComVisible(false)]
    //[Serializable]
    //[DebuggerDisplay("Count={Count}")]
    ////[DebuggerTypeProxy(typeof(CollectionDebuggerView))]
    //public class Queue<T> : IEnumerable<T>, ICollection, IEnumerable
    //{
    //    T[] _array;
    //    int _head;
    //    int _tail;
    //    int _size;
    //    int _version;

    //    public Queue() {
    //        _array = new T[0];
    //    }

    //    public Queue(int capacity) {
    //        if (capacity < 0)
    //            throw new ArgumentOutOfRangeException("capacity");

    //        _array = new T[capacity];
    //    }

    //    public Queue(IEnumerable<T> collection) {
    //        if (collection == null)
    //            throw new ArgumentNullException("collection");

    //        var icoll = collection as ICollection<T>;
    //        var size = icoll != null ? icoll.Count : 0;

    //        _array = new T[size];

    //        foreach (T t in collection)
    //            Enqueue(t);
    //    }

    //    public void Clear() {
    //        Array.Clear(_array, 0, _array.Length);

    //        _head = _tail = _size = 0;
    //        _version++;
    //    }

    //    public bool Contains(T item) {
    //        if (item == null) {
    //            foreach (T t in this)
    //                if (t == null)
    //                    return true;
    //        }
    //        else {
    //            foreach (T t in this)
    //                if (item.Equals(t))
    //                    return true;
    //        }

    //        return false;
    //    }

    //    public void CopyTo(T[] array, int arrayIndex) {
    //        if (array == null)
    //            throw new ArgumentNullException();

    //        ((ICollection)this).CopyTo(array, arrayIndex);
    //    }

    //    void ICollection.CopyTo(Array array, int idx) {
    //        if (array == null)
    //            throw new ArgumentNullException();

    //        if ((uint)idx > (uint)array.Length)
    //            throw new ArgumentOutOfRangeException();

    //        if (array.Length - idx < _size)
    //            throw new ArgumentOutOfRangeException();

    //        if (_size == 0)
    //            return;

    //        try {
    //            int contents_length = _array.Length;
    //            int length_from_head = contents_length - _head;

    //            Array.Copy(_array, _head, array, idx, Math.Min(_size, length_from_head));
    //            if (_size > length_from_head)
    //                Array.Copy(_array, 0, array,
    //                         idx + length_from_head,
    //                         _size - length_from_head);
    //        }
    //        catch (ArrayTypeMismatchException) {
    //            throw new ArgumentException();
    //        }
    //    }

    //    public T Dequeue() {
    //        T ret = Peek();

    //        // clear stuff out to make the GC happy
    //        _array[_head] = default(T);

    //        if (++_head == _array.Length)
    //            _head = 0;
    //        _size--;
    //        _version++;

    //        return ret;
    //    }

    //    public T Peek() {
    //        if (_size == 0)
    //            throw new InvalidOperationException();

    //        return _array[_head];
    //    }

    //    public void Enqueue(T item) {
    //        if (_size == _array.Length || _tail == _array.Length)
    //            SetCapacity(Math.Max(Math.Max(_size, _tail) * 2, 4));

    //        _array[_tail] = item;

    //        if (++_tail == _array.Length)
    //            _tail = 0;

    //        _size++;
    //        _version++;
    //    }

    //    public T[] ToArray() {
    //        T[] t = new T[_size];
    //        CopyTo(t, 0);
    //        return t;
    //    }

    //    public void TrimExcess() {
    //        if (_size < _array.Length * 0.9)
    //            SetCapacity(_size);
    //    }

    //    void SetCapacity(int new_size) {
    //        if (new_size == _array.Length)
    //            return;

    //        if (new_size < _size)
    //            throw new InvalidOperationException("shouldnt happen");

    //        T[] new_data = new T[new_size];
    //        if (_size > 0)
    //            CopyTo(new_data, 0);

    //        _array = new_data;
    //        _tail = _size;
    //        _head = 0;
    //        _version++;
    //    }

    //    public int Count {
    //        get { return _size; }
    //    }

    //    bool ICollection.IsSynchronized {
    //        get { return false; }
    //    }

    //    object ICollection.SyncRoot {
    //        get { return this; }
    //    }

    //    public Enumerator GetEnumerator() {
    //        return new Enumerator(this);
    //    }

    //    IEnumerator<T> IEnumerable<T>.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator() {
    //        return GetEnumerator();
    //    }

    //    [Serializable]
    //    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    //    {
    //        const int NOT_STARTED = -2;

    //        // this MUST be -1, because we depend on it in move next.
    //        // we just decr the _size, so, 0 - 1 == FINISHED
    //        const int FINISHED = -1;

    //        Queue<T> q;
    //        int idx;
    //        int ver;

    //        internal Enumerator(Queue<T> q) {
    //            this.q = q;
    //            idx = NOT_STARTED;
    //            ver = q._version;
    //        }

    //        // for some fucked up reason, MSFT added a useless dispose to this class
    //        // It means that in foreach, we must still do a try/finally. Broken, very
    //        // broken.
    //        public void Dispose() {
    //            idx = NOT_STARTED;
    //        }

    //        public bool MoveNext() {
    //            if (ver != q._version)
    //                throw new InvalidOperationException();

    //            if (idx == NOT_STARTED)
    //                idx = q._size;

    //            return idx != FINISHED && --idx != FINISHED;
    //        }

    //        public T Current {
    //            get {
    //                if (idx < 0)
    //                    throw new InvalidOperationException();

    //                return q._array[(q._size - 1 - idx + q._head) % q._array.Length];
    //            }
    //        }

    //        void IEnumerator.Reset() {
    //            if (ver != q._version)
    //                throw new InvalidOperationException();

    //            idx = NOT_STARTED;
    //        }

    //        object IEnumerator.Current {
    //            get { return Current; }
    //        }

    //    }
    //}
    /// <summary>
    ///like c++ std::queue 普通列队
    /// </summary>
    public class std_queue<T>:Queue<T>
    {
        public std_queue()
            : base() { 
        }

        public std_queue(IEnumerable<T> collection)
            : base(collection) { 
        }

        public std_queue(int capacity)
            : base(capacity) { 
        }

        public bool empty() {
            return base.Count == 0;

        }
        public int size() {
            return base.Count;
        }

        public T front() {
            return base.Peek();
        }
        public T pop() {
           return base.Dequeue();
        }

        public void push(T @value) {
            base.Enqueue(@value);
        }

        public static void swap(std_queue<T> _this, std_queue<T> _other) {
            std_queue<T> temp = _this;
            _this = _other;
            _other = temp;
            
        }
    }

    //std_priority_queue
    /// <summary>
    /// Represents a queue of items that are sorted based on individual priorities.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the queue.</typeparam>
    /// <typeparam name="TPriority">Specifies the type of object representing the priority.</typeparam>
    //[SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class std_priority_queue<T, TPriority>:  IEnumerable<T>, ICollection, IEnumerable
    {
        private readonly List<KeyValuePair<T, TPriority>> heap = new List<KeyValuePair<T, TPriority>>();
        private readonly Dictionary<T, int> indexes = new Dictionary<T, int>();

        private readonly IComparer<TPriority> comparer;
        private readonly bool invert;

        public std_priority_queue()
            : this(false) {
        }

        public std_priority_queue(bool invert)
            : this(Comparer<TPriority>.Default) {
            this.invert = invert;
        }

        public std_priority_queue(IComparer<TPriority> comparer) {
            this.comparer = comparer;
            heap.Add(default(KeyValuePair<T, TPriority>));
        }

        public void Enqueue(T item, TPriority priority) {
            KeyValuePair<T, TPriority> tail = new KeyValuePair<T, TPriority>(item, priority);
            heap.Add(tail);

            MoveUp(tail, Count);
        }

        public KeyValuePair<T, TPriority> Dequeue() {
            int bound = Count;
            if (bound < 1)
                throw new InvalidOperationException("Queue is empty.");

            KeyValuePair<T, TPriority> head = heap[1];
            KeyValuePair<T, TPriority> tail = heap[bound];

            heap.RemoveAt(bound);

            if (bound > 1)
                MoveDown(tail, 1);

            indexes.Remove(head.Key);

            return head;
        }

        public KeyValuePair<T, TPriority> Peek() {
            if (Count < 1)
                throw new InvalidOperationException("Queue is empty.");

            return heap[1];
        }

        public bool TryGetValue(T item, out TPriority priority) {
            int index;
            if (indexes.TryGetValue(item, out index)) {
                priority = heap[indexes[item]].Value;
                return true;
            }
            else {
                priority = default(TPriority);
                return false;
            }
        }

        public TPriority this[T item] {
            get {
                return heap[indexes[item]].Value;
            }
            set {
                int index;

                if (indexes.TryGetValue(item, out index)) {
                    int order = comparer.Compare(value, heap[index].Value);
                    if (order != 0) {
                        if (invert)
                            order = ~order;

                        KeyValuePair<T, TPriority> element = new KeyValuePair<T, TPriority>(item, value);
                        if (order < 0)
                            MoveUp(element, index);
                        else
                            MoveDown(element, index);
                    }
                }
                else {
                    KeyValuePair<T, TPriority> element = new KeyValuePair<T, TPriority>(item, value);
                    heap.Add(element);

                    MoveUp(element, Count);
                }
            }
        }

        public int Count {
            get {
                return heap.Count - 1;
            }
        }

        private void MoveUp(KeyValuePair<T, TPriority> element, int index) {
            while (index > 1) {
                int parent = index >> 1;

                if (IsPrior(heap[parent], element))
                    break;

                heap[index] = heap[parent];
                indexes[heap[parent].Key] = index;

                index = parent;
            }

            heap[index] = element;
            indexes[element.Key] = index;
        }

        private void MoveDown(KeyValuePair<T, TPriority> element, int index) {
            int count = heap.Count;

            while (index << 1 < count) {
                int child = index << 1;
                int sibling = child | 1;

                if (sibling < count && IsPrior(heap[sibling], heap[child]))
                    child = sibling;

                if (IsPrior(element, heap[child]))
                    break;

                heap[index] = heap[child];
                indexes[heap[child].Key] = index;

                index = child;
            }

            heap[index] = element;
            indexes[element.Key] = index;
        }

        private bool IsPrior(KeyValuePair<T, TPriority> element1, KeyValuePair<T, TPriority> element2) {
            int order = comparer.Compare(element1.Value, element2.Value);
            if (invert)
                order = ~order;
            return order < 0;
        }





        public void emplace(T value) {
            this.Enqueue(value, default(TPriority));
        }
        public bool empty() {
            return this.Count == 0;

        }
        public int size() {
            return this.Count;
        }
        public T top() {
            return this.Peek().Key;
        }
        public T pop() {
            return this.Dequeue().Key;
        }

        public void push(T @value) {
            this.Enqueue(@value,default(TPriority));
        }
        public void push(T @value,TPriority p) {
            this.Enqueue(@value, p);
        }
        public static void swap(std_queue<T> _this, std_queue<T> _other) {
            std_queue<T> temp = _this;
            _this = _other;
            _other = temp;

        }




        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator() {
            int len = heap.Count;
            for(int i=1;i<len;i++){
                yield return heap[i].Key;
            }
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator() {
            int len = heap.Count;
            for (int i = 1; i < len; i++) {
                yield return heap[i].Value;
            }
        }

        #endregion

        #region ICollection 成员

        public void CopyTo(Array array, int index) {
            KeyValuePair<T,TPriority>[]arr=new KeyValuePair<T,TPriority>[heap.Count];
            heap.CopyTo(arr, 0);
            array = arr;
        }

        public bool IsSynchronized {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
