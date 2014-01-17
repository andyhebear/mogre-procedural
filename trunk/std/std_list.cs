using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{
    /// <summary>
    /// like c++ std:list 双向链表 相当于LinkedList类
    /// </summary>
    public class std_list<T> : LinkedList<T>
    {

        public std_list()
            : base() {
        }
        public std_list(IEnumerable<T> collection)
            : base(collection) {
        }

        public void assign(uint num, T @value) {
            T[] values = new T[num];
            for (int i = 0; i < num; i++) {
                values[i] = @value;
            }
            base.Clear();
            for (int i = 0; i < num; i++) {
                base.AddFirst(values[i]);
            }
        }
        public void assign(T[] values) {
            base.Clear();
            for (int i = 0; i < values.Length; i++) {
                base.AddFirst(values[i]);
            }
        }
        public void assign(T[] values,int beginpos,int beforeendpos) {
            base.Clear();
            for (int i = beginpos; i < beforeendpos; i++) {
                base.AddFirst(values[i]);
            }
        }
        public int begin() {
            return 0;
        }
        public int end() {
            return base.Count ;
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
        public T front() {
            return base.First.Value;
        }
        public void front(T @value) {
            base.First.Value = @value;
        }
        public T back() {
            return base.Last.Value;
        }
        public void back(T @value) {
            base.Last.Value = @value;
        }
        public void clear() {
            base.Clear();
        }
        public void emplace(int pos, T @value) {
            LinkedListNode<T> node = getElement(pos);
            base.AddBefore(node, @value);
        }
        public void emplace_front(T @value) {
            base.AddFirst(@value);
        }
        public void emplace_back(T @value) {
            base.AddLast(@value);
        }
        /// <summary>
        /// 元素从列表移除到另一个列表
        /// </summary>
        //public void splice() { 
        //}
        public int erase(int pos) {
            if (pos == 0) {
                base.RemoveFirst();
                return pos ;
            }
            int index = 0;
            LinkedListNode<T> first = base.First;
            LinkedListNode<T> remove = null;
            while (first.Next != null) {                 
                index++;
                if (index == pos) {
                    remove = first.Next;
                    break;
                }
                first=first.Next;
            }
            if (remove == null) {
                return -1;
            }
            base.Remove(remove);
            return pos;
        }

        public int erase(int beginpos, int beforeendpos) {
            int index = 0;
            int i = 0;
            LinkedListNode<T> first = base.First;
            LinkedListNode<T>[] removes = new LinkedListNode<T>[beforeendpos - beginpos];
            if (beginpos == 0) {
                removes[0] = first;
                i++;
            }          
            while (first.Next != null) {
                index++;
                if (index == beginpos) {
                    removes[i++] = first.Next;                    
                }
                else if (index > beginpos && index < beforeendpos) {
                    removes[i++] = first.Next;
                }
                else if (index == beforeendpos) { 
                    //removes[i++] = first.Next;
                    break;
                }
                first = first.Next;
            }
            foreach (var v in removes) {
                base.Remove(v);
            }
            return beforeendpos ;
        }
        public static int advance(int pos, int add) {
            return pos + add;
        }

        public T[] get_allocator() {
            T[] temp = new T[base.Count];
            base.CopyTo(temp, 0);
            return temp;
        }
        public LinkedListNode<T> getElement(int pos) {
            if (pos == 0)
                return base.First;
            int index = 0;
            LinkedListNode<T> node=base.First;
            while (node.Next != null) {
                index++;
                if (pos == index) {
                    return node.Next;
                }
                node = node.Next;
            }
            return null;
        }
        public void insert(int pos ,T @value) {
            LinkedListNode<T> node = getElement(pos);
            base.AddBefore(node, @value);          
        }
        public void insert(int pos, uint num, T @value) {
            LinkedListNode<T> node = getElement(pos);
            for (int i = 0; i < num; i++) {
                base.AddBefore(node, @value);
            }
        }
        public void insert(int pos, T[] values) {
            LinkedListNode<T> node = getElement(pos);
            int num = values.Length;
            for (int i = 0; i < num; i++) {
                base.AddBefore(node, values[i]);
            }
        }
        public void insert(int pos, T[] values, int beginpos, int beforeendpos) {
            LinkedListNode<T> node = getElement(pos);
            for(int i=beginpos;i<beforeendpos;i++){
                base.AddBefore(node, values[i]);
            }            
        }

        /// <summary>
        /// 内部自动排序
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="_other"></param>
        public static void merge(ref std_list<T> _this,std_list<T> _other,IComparer<T>compare) {
            T[] _tv = _this.get_allocator();
            T[] _ov = _other.get_allocator();
            List<T> sv = new List<T>();
            sv.AddRange(_tv);
            sv.AddRange(_ov);
            if (compare != null) {
                sv.Sort(compare);
            }
            else {
                sv.Sort();
            }
            _this = new std_list<T>(sv);
        }
        public void pop_back() {
            base.RemoveLast();
        }

        public void pop_front() {
            base.RemoveFirst();
        }

        public void push_back(T @value) {
            base.AddLast(@value);
        }
        public void push_front(T @value) {
            base.AddFirst(@value);
        }

        public void remove(T @value) {
            base.Remove(@value);
        }

        //public void remove_if(Action<T> ondelete) { 
        
        //}
        //public void resize(int size) { 
        //}
        //public void resize(int size, T @defaultvalue) { 
            
        //} 
        public int size() {
            return base.Count;
        }

        public static void sort(ref std_list<T> sortlist) {
            T[] values = sortlist.get_allocator();
            List<T> sv = new List<T>(values);
            sv.Sort();
            std_list<T> sl = new std_list<T>(sv);
            sortlist = sl;
        }
        public static void reverse(ref std_list<T>_this) {
            T[] values = _this.get_allocator();
            int len = values.Length;
            _this = new std_list<T>();
            foreach (var v in values) {
                _this.AddFirst(v);
            }
        }

       
        public static void swap(ref std_list<T> _this, ref std_list<T> _other) {
            std_list<T> temp = _this;
            _this = _other;
            _other = temp;
        }
        /// <summary>
        /// 移除重复项目
        /// Remove duplicate values
        /// </summary>
        /// <param name="_this"></param>
        public static void unique(ref std_list<T> _this) {
            T[] values = _this.get_allocator();
            List<T> ul = new List<T>();
            foreach (var v in values) {
                if (!ul.Contains(v)) {
                    ul.Add(v);
                }
            }
            _this = new std_list<T>(ul);
        }
    }
}
