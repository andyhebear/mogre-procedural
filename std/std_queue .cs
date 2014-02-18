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



        //

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
            get { return false; }
            //get { throw new NotImplementedException(); }
        }

        public object SyncRoot {
            get { return this; }
            //get { throw new NotImplementedException(); }
        }

        #endregion
    }



    /// <summary>Queue that dequeues items in order of their priority</summary>
//    public class PriorityQueue<ItemType> : ICollection, IEnumerable<ItemType>
//    {

//        #region class Enumerator

//        /// <summary>Enumerates all items contained in a priority queue</summary>
//        private class Enumerator : IEnumerator<ItemType>
//        {

//            /// <summary>Initializes a new priority queue enumerator</summary>
//            /// <param name="priorityQueue">Priority queue to be enumerated</param>
//            public Enumerator(PriorityQueue<ItemType> priorityQueue) {
//                this.priorityQueue = priorityQueue;
//                Reset();
//            }

//            /// <summary>Resets the enumerator to its initial state</summary>
//            public void Reset() {
//                this.index = -1;
//#if DEBUG
//                this.expectedVersion = this.priorityQueue.version;
//#endif
//            }

//            /// <summary>The current item being enumerated</summary>
//            ItemType IEnumerator<ItemType>.Current {
//                get {
//#if DEBUG
//                    checkVersion();
//#endif
//                    return this.priorityQueue.heap[index];
//                }
//            }

//            /// <summary>Moves to the next item in the priority queue</summary>
//            /// <returns>True if a next item was found, false if the end has been reached</returns>
//            public bool MoveNext() {
//#if DEBUG
//                checkVersion();
//#endif
//                if (this.index + 1 == this.priorityQueue.count)
//                    return false;

//                ++this.index;

//                return true;
//            }

//            /// <summary>Releases all resources used by the enumerator</summary>
//            public void Dispose() { }

//#if DEBUG
//            /// <summary>Ensures that the priority queue has not changed</summary>
//            private void checkVersion() {
//                if (this.expectedVersion != this.priorityQueue.version)
//                    throw new InvalidOperationException("Priority queue has been modified");
//            }
//#endif

//            /// <summary>The current item being enumerated</summary>
//            object IEnumerator.Current {
//                get {
//#if DEBUG
//                    checkVersion();
//#endif
//                    return this.priorityQueue.heap[index];
//                }
//            }

//            /// <summary>Index of the current item in the priority queue</summary>
//            private int index;
//            /// <summary>The priority queue whose items this instance enumerates</summary>
//            private PriorityQueue<ItemType> priorityQueue;
//#if DEBUG
//            /// <summary>Expected version of the priority queue</summary>
//            private int expectedVersion;
//#endif

//        }

//        #endregion // class Enumerator

//        /// <summary>
//        ///   Initializes a new priority queue using IComparable for comparing items
//        /// </summary>
//        public PriorityQueue() : this(Comparer<ItemType>.Default) { }

//        /// <summary>Initializes a new priority queue</summary>
//        /// <param name="comparer">Comparer to use for ordering the items</param>
//        public PriorityQueue(IComparer<ItemType> comparer) {
//            this.comparer = comparer;
//            this.capacity = 15; // 15 is equal to 4 complete levels
//            this.heap = new ItemType[this.capacity];
//        }

//        /// <summary>Returns the topmost item in the queue without dequeueing it</summary>
//        /// <returns>The topmost item in the queue</returns>
//        public ItemType Peek() {
//            if (this.count == 0) {
//                throw new InvalidOperationException("No items queued");
//            }

//            return this.heap[0];
//        }

//        /// <summary>Takes the item with the highest priority off from the queue</summary>
//        /// <returns>The item with the highest priority in the list</returns>
//        /// <exception cref="InvalidOperationException">When the queue is empty</exception>
//        public ItemType Dequeue() {
//            if (this.count == 0) {
//                throw new InvalidOperationException("No items available to dequeue");
//            }

//            ItemType result = this.heap[0];
//            --this.count;
//            trickleDown(0, this.heap[this.count]);
//#if DEBUG
//            ++this.version;
//#endif
//            return result;
//        }

//        /// <summary>Puts an item into the priority queue</summary>
//        /// <param name="item">Item to be queued</param>
//        public void Enqueue(ItemType item) {
//            if (this.count == capacity)
//                growHeap();

//            ++this.count;
//            bubbleUp(this.count - 1, item);
//#if DEBUG
//            ++this.version;
//#endif
//        }

//        /// <summary>Removes all items from the priority queue</summary>
//        public void Clear() {
//            this.count = 0;
//#if DEBUG
//            ++this.version;
//#endif
//        }


//        /// <summary>Total number of items in the priority queue</summary>
//        public int Count {
//            get { return this.count; }
//        }

//        /// <summary>Copies the contents of the priority queue into an array</summary>
//        /// <param name="array">Array to copy the priority queue into</param>
//        /// <param name="index">Starting index for the destination array</param>
//        public void CopyTo(Array array, int index) {
//            Array.Copy(this.heap, 0, array, index, this.count);
//        }

//        /// <summary>
//        ///   Obtains an object that can be used to synchronize accesses to the priority queue
//        ///   from different threads
//        /// </summary>
//        public object SyncRoot {
//            get { return this; }
//        }

//        /// <summary>Whether operations performed on this priority queue are thread safe</summary>
//        public bool IsSynchronized {
//            get { return false; }
//        }

//        /// <summary>Returns a typesafe enumerator for the priority queue</summary>
//        /// <returns>A new enumerator for the priority queue</returns>
//        public IEnumerator<ItemType> GetEnumerator() {
//            return new Enumerator(this);
//        }

//        /// <summary>Moves an item upwards in the heap tree</summary>
//        /// <param name="index">Index of the item to be moved</param>
//        /// <param name="item">Item to be moved</param>
//        private void bubbleUp(int index, ItemType item) {
//            int parent = getParent(index);

//            // Note: (index > 0) means there is a parent
//            while ((index > 0) && (this.comparer.Compare(this.heap[parent], item) < 0)) {
//                this.heap[index] = this.heap[parent];
//                index = parent;
//                parent = getParent(index);
//            }

//            this.heap[index] = item;
//        }

//        /// <summary>Move the item downwards in the heap tree</summary>
//        /// <param name="index">Index of the item to be moved</param>
//        /// <param name="item">Item to be moved</param>
//        private void trickleDown(int index, ItemType item) {
//            int child = getLeftChild(index);

//            while (child < this.count) {

//                bool needsToBeMoved =
//                  ((child + 1) < this.count) &&
//                  (this.comparer.Compare(heap[child], this.heap[child + 1]) < 0);

//                if (needsToBeMoved)
//                    ++child;

//                this.heap[index] = this.heap[child];
//                index = child;
//                child = getLeftChild(index);

//            }
           
//            bubbleUp(index, item);
//        }

//        /// <summary>Obtains the left child item in the heap tree</summary>
//        /// <param name="index">Index of the item whose left child to return</param>
//        /// <returns>The left child item of the provided parent item</returns>
//        private int getLeftChild(int index) {
//            return (index * 2) + 1;
//        }

//        /// <summary>Calculates the parent entry of the item on the heap</summary>
//        /// <param name="index">Index of the item whose parent to calculate</param>
//        /// <returns>The index of the parent to the specified item</returns>
//        private int getParent(int index) {
//            return (index - 1) / 2;
//        }

//        /// <summary>Increases the size of the priority collection's heap</summary>
//        private void growHeap() {
//            this.capacity = (capacity * 2) + 1;

//            ItemType[] newHeap = new ItemType[this.capacity];
//            Array.Copy(this.heap, 0, newHeap, 0, this.count);
//            this.heap = newHeap;
//        }

//        /// <summary>Returns an enumerator for the priority queue</summary>
//        /// <returns>A new enumerator for the priority queue</returns>
//        IEnumerator IEnumerable.GetEnumerator() {
//            return new Enumerator(this);
//        }

//        /// <summary>Comparer used to order the items in the priority queue</summary>
//        private IComparer<ItemType> comparer;
//        /// <summary>Total number of items in the priority queue</summary>
//        private int count;
//        /// <summary>Available space in the priority queue</summary>
//        private int capacity;
//        /// <summary>Tree containing the items in the priority queue</summary>
//        private ItemType[] heap;
//#if DEBUG
//        /// <summary>Incremented whenever the priority queue is modified</summary>
//        private int version;
//#endif

//    }


    //dotnet 4.0
    /// <summary>
    /// PriorityQueue provides a stack-like interface, except that objects
    /// "pushed" in arbitrary order are "popped" in order of priority, i.e.,
    /// from least to greatest as defined by the specified comparer.
    /// </summary>
    /// <remarks>
    /// Push and Pop are each O(log N). Pushing N objects and them popping
    /// them all is equivalent to performing a heap sort and is O(N log N).
    /// </remarks>
    internal class PriorityQueue<T> 
    {
        //
        // The _heap array represents a binary tree with the "shape" property.
        // If we number the nodes of a binary tree from left-to-right and top-
        // to-bottom as shown,
        //
        //             0
        //           /   \
        //          /     \
        //         1       2
        //       /  \     / \
        //      3    4   5   6
        //     /\    /
        //    7  8  9
        //
        // The shape property means that there are no gaps in the sequence of
        // numbered nodes, i.e., for all N > 0, if node N exists then node N-1
        // also exists. For example, the next node added to the above tree would
        // be node 10, the right child of node 4.
        //
        // Because of this constraint, we can easily represent the "tree" as an
        // array, where node number == array index, and parent/child relationships
        // can be calculated instead of maintained explicitly. For example, for
        // any node N > 0, the parent of N is at array index (N - 1) / 2.
        //
        // In addition to the above, the first _count members of the _heap array
        // compose a "heap", meaning each child node is greater than or equal to
        // its parent node; thus, the root node is always the minimum (i.e., the
        // best match for the specified style, weight, and stretch) of the nodes
        // in the heap.
        //
        // Initially _count < 0, which means we have not yet constructed the heap.
        // On the first call to MoveNext, we construct the heap by "pushing" all
        // the nodes into it. Each successive call "pops" a node off the heap
        // until the heap is empty (_count == 0), at which time we've reached the
        // end of the sequence.
        //

        #region constructors

        internal PriorityQueue(int capacity, IComparer<T> comparer) {
            _heap = new T[capacity > 0 ? capacity : DefaultCapacity];
            _count = 0;
            _comparer = comparer;
        }

        #endregion

        #region internal members

        /// <summary>
        /// Gets the number of items in the priority queue.
        /// </summary>
        internal int Count {
            get { return _count; }
        }

        /// <summary>
        /// Gets the first or topmost object in the priority queue, which is the
        /// object with the minimum value.
        /// </summary>
        internal T Top {
            get {
                Debug.Assert(_count > 0);
                return _heap[0];
            }
        }

        /// <summary>
        /// Adds an object to the priority queue.
        /// </summary>
        internal void Push(T value) {
            // Increase the size of the array if necessary.
            if (_count == _heap.Length) {
                T[] temp = new T[_count * 2];
                for (int i = 0; i < _count; ++i) {
                    temp[i] = _heap[i];
                }
                _heap = temp;
            }

            // Loop invariant:
            //
            //  1.  index is a gap where we might insert the new node; initially
            //      it's the end of the array (bottom-right of the logical tree).
            //
            int index = _count;
            while (index > 0) {
                int parentIndex = HeapParent(index);
                if (_comparer.Compare(value, _heap[parentIndex]) < 0) {
                    // value is a better match than the parent node so exchange
                    // places to preserve the "heap" property.
                    _heap[index] = _heap[parentIndex];
                    index = parentIndex;
                }
                else {
                    // we can insert here.
                    break;
                }
            }

            _heap[index] = value;
            _count++;
        }

        /// <summary>
        /// Removes the first node (i.e., the logical root) from the heap.
        /// </summary>
        internal void Pop() {
            Debug.Assert(_count != 0);
            
            if (_count > 1) {
                // Loop invariants:
                //
                //  1.  parent is the index of a gap in the logical tree
                //  2.  leftChild is
                //      (a) the index of parent's left child if it has one, or
                //      (b) a value >= _count if parent is a leaf node
                //
                int parent = 0;
                int leftChild = HeapLeftChild(parent);

                while (leftChild < _count) {
                    int rightChild = HeapRightFromLeft(leftChild);
                    int bestChild =
                        (rightChild < _count && _comparer.Compare(_heap[rightChild], _heap[leftChild]) < 0) ?
                        rightChild : leftChild;

                    // Promote bestChild to fill the gap left by parent.
                    _heap[parent] = _heap[bestChild];

                    // Restore invariants, i.e., let parent point to the gap.
                    parent = bestChild;
                    leftChild = HeapLeftChild(parent);
                }

                // Fill the last gap by moving the last (i.e., bottom-rightmost) node.
                _heap[parent] = _heap[_count - 1];
            }

            _count--;
        }

        #endregion

        #region private members

        /// <summary>
        /// Calculate the parent node index given a child node's index, taking advantage
        /// of the "shape" property.
        /// </summary>
        private static int HeapParent(int i) {
            return (i - 1) / 2;
        }

        /// <summary>
        /// Calculate the left child's index given the parent's index, taking advantage of
        /// the "shape" property. If there is no left child, the return value is >= _count.
        /// </summary>
        private static int HeapLeftChild(int i) {
            return (i * 2) + 1;
        }

        /// <summary>
        /// Calculate the right child's index from the left child's index, taking advantage
        /// of the "shape" property (i.e., sibling nodes are always adjacent). If there is
        /// no right child, the return value >= _count.
        /// </summary>
        private static int HeapRightFromLeft(int i) {
            return i + 1;
        }

        private T[] _heap;
        private int _count;
        private IComparer<T> _comparer;
        private const int DefaultCapacity = 6;

        #endregion
    }
}
