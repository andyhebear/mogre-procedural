﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Mogre_Procedural.std
{
   // [ComVisible(false)]
   // [Serializable]
   // [DebuggerDisplay("Count={Count}")]
   //// [DebuggerTypeProxy(typeof(CollectionDebuggerView))]
   // public class Stack<T> : IEnumerable<T>, ICollection, IEnumerable
   // {
   //     T[] _array;
   //     int _size;
   //     int _version;

   //     private const int INITIAL_SIZE = 16;

   //     public Stack() {
   //     }

   //     public Stack(int capacity) {
   //         if (capacity < 0)
   //             throw new ArgumentOutOfRangeException("capacity");

   //         _array = new T[capacity];
   //     }

   //     public Stack(IEnumerable<T> collection) {
   //         if (collection == null)
   //             throw new ArgumentNullException("collection");

   //         ICollection<T> col = collection as ICollection<T>;

   //         if (col != null) {
   //             _size = col.Count;
   //             _array = new T[_size];
   //             col.CopyTo(_array, 0);
   //         }
   //         else {
   //             foreach (T t in collection)
   //                 Push(t);
   //         }
   //     }

   //     public void Clear() {
   //         if (_array != null)
   //             Array.Clear(_array, 0, _array.Length);

   //         _size = 0;
   //         _version++;
   //     }

   //     public bool Contains(T item) {
   //         return _array != null && Array.IndexOf(_array, item, 0, _size) != -1;
   //     }

   //     public void CopyTo(T[] array, int arrayIndex) {
   //         if (array == null)
   //             throw new ArgumentNullException("array");
   //         if (arrayIndex < 0)
   //             throw new ArgumentOutOfRangeException("idx");

   //         // this gets copied in the order that it is poped
   //         if (_array != null) {
   //             Array.Copy(_array, 0, array, arrayIndex, _size);
   //             Array.Reverse(array, arrayIndex, _size);
   //         }
   //     }

   //     public T Peek() {
   //         if (_size == 0)
   //             throw new InvalidOperationException();

   //         return _array[_size - 1];
   //     }

   //     public T Pop() {
   //         if (_size == 0)
   //             throw new InvalidOperationException();

   //         _version++;
   //         T popped = _array[--_size];
   //         // clear stuff out to make the GC happy
   //         _array[_size] = default(T);
   //         return popped;
   //     }

   //     public void Push(T item) {
   //         if (_array == null || _size == _array.Length)
   //             Array.Resize<T>(ref _array, _size == 0 ? INITIAL_SIZE : 2 * _size);

   //         _version++;

   //         _array[_size++] = item;
   //     }

   //     public T[] ToArray() {
   //         T[] copy = new T[_size];
   //         CopyTo(copy, 0);
   //         return copy;
   //     }

   //     public void TrimExcess() {
   //         if (_array != null && (_size < _array.Length * 0.9))
   //             Array.Resize<T>(ref _array, _size);
   //         _version++;
   //     }

   //     public int Count {
   //         get { return _size; }
   //     }

   //     bool ICollection.IsSynchronized {
   //         get { return false; }
   //     }

   //     object ICollection.SyncRoot {
   //         get { return this; }
   //     }

   //     void ICollection.CopyTo(Array dest, int idx) {
   //         try {
   //             if (_array != null) {
   //                 Array.Copy(_array, 0, dest, idx, _size);
   //                 Array.Reverse(dest, idx, _size);
   //             }
   //         }
   //         catch (ArrayTypeMismatchException) {
   //             throw new ArgumentException();
   //         }
   //     }

   //     public Enumerator GetEnumerator() {
   //         return new Enumerator(this);
   //     }

   //     IEnumerator<T> IEnumerable<T>.GetEnumerator() {
   //         return GetEnumerator();
   //     }

   //     IEnumerator IEnumerable.GetEnumerator() {
   //         return GetEnumerator();
   //     }

   //     [Serializable]
   //     public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
   //     {
   //         const int NOT_STARTED = -2;

   //         // this MUST be -1, because we depend on it in move next.
   //         // we just decr the _size, so, 0 - 1 == FINISHED
   //         const int FINISHED = -1;

   //         Stack<T> parent;
   //         int idx;
   //         int _version;

   //         internal Enumerator(Stack<T> t) {
   //             parent = t;
   //             idx = NOT_STARTED;
   //             _version = t._version;
   //         }

   //         // for some fucked up reason, MSFT added a useless dispose to this class
   //         // It means that in foreach, we must still do a try/finally. Broken, very
   //         // broken.
   //         public void Dispose() {
   //             idx = FINISHED;
   //         }

   //         public bool MoveNext() {
   //             if (_version != parent._version)
   //                 throw new InvalidOperationException();

   //             if (idx == -2)
   //                 idx = parent._size;

   //             return idx != FINISHED && --idx != FINISHED;
   //         }

   //         public T Current {
   //             get {
   //                 if (idx < 0)
   //                     throw new InvalidOperationException();

   //                 return parent._array[idx];
   //             }
   //         }

   //         void IEnumerator.Reset() {
   //             if (_version != parent._version)
   //                 throw new InvalidOperationException();

   //             idx = NOT_STARTED;
   //         }

   //         object IEnumerator.Current {
   //             get { return Current; }
   //         }

   //     }
   // }
    /// <summary>
    /// like c++ std::stack 
    /// 堆栈
    /// </summary>
    public class std_stack<T> : Stack<T>//IEnumerable<T>, ICollection, IEnumerable
    {
        public std_stack()
            : base() {
        }

        public std_stack(IEnumerable<T> collection)
            : base(collection) {
        }

        public std_stack(int capacity)
            : base(capacity) {
        }
        public bool empty() {
            return base.Count == 0;
        }

        public T pop() {
           return base.Pop();
        }
        public void push(T @value) {
            base.Push(@value);
        }
        public int size() {
            return base.Count;
        }
        public T top() {
            //IEnumerator ie = base.GetEnumerator();
            //return (T)ie.Current;
            return base.Peek();
        }

        public static void swap(ref std_stack<T>_this,ref std_stack<T>_other ) {
            std_stack<T> temp = _this;
            _this = _other;
            _other = temp;
        }
    }
    // public class std_stack<T> : IEnumerable<T>
    //{
    //    private List<T> stack = new List<T>();

    //    public int Count
    //    {
    //        get
    //        {
    //            return stack.Count;
    //        }
    //    }

    //    public T this[int n]
    //    {
    //        get
    //        {
    //            return stack[n];
    //        }
    //    }

    //    public void Push(T item)
    //    {
    //        stack.Add(item);
    //    }

    //    public T Pop()
    //    {
    //        T item = stack[stack.Count - 1];
    //        stack.RemoveAt(stack.Count - 1);
    //        return item;
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        return stack.GetEnumerator();
    //    }

    //    IEnumerator IEnumerable.GetEnumerator()
    //    {
    //        return GetEnumerator();
    //    }
    //}

}
