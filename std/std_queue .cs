using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{
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
    public class std_priority_queue<T, TPriority>
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
    }
}
