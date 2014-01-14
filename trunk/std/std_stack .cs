using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Mogre_Procedural.std
{
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
