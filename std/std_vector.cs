using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{
    public interface Istd_vector<T>
    {
        void push_back(T value);
        void insert(int pos, T value);
        T at(int pos);
        void pop_back();
        void erase(T value);
        void erase(int pos);
        void clear();
        bool empty();
        int capacity();
        int size();
       
    }
    /// <summary>
    /// like c++ std::vector 相当于List类
    /// </summary>
    public class std_vector<T> : List<T>, Istd_vector<T>
    {
        public std_vector()
            : base() {
        }
        public std_vector(IEnumerable<T> collection)
            : base() {
        }
        public std_vector(int capacity)
            : base() {
        }
        public std_vector(int count, T @value) : base() {
            T[] values = new T[count];
            for (int i = 0; i < count; i++) {
                values[i] = value;
            }
            base.AddRange(values);
        }
        #region Istd_vector<T> 成员

        //public List<T>.Enumerator begin() {            
        //    return base.GetEnumerator();
        //}
        //public List<T>.Enumerator end() {
        //    List<T>.Enumerator er = base.GetEnumerator();
        //    while (er.MoveNext()) {             
        //    }
        //    return er;
        //}
        public int begin() {
            return 0;
        }
        //public T begin(bool getref) {
        //    return front();
        //}
        public int end() {
            return base.Count ;
        }
        //public T end(bool getref) {
        //    return back();
        //}
        public void push_back(T value) {
            base.Add(value);
        }
        /// <summary>
        /// 返回插入的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int insert(int pos, T value) {
            base.Insert(pos, value);
            return pos;
        }

        public int insert(int pos, int count, T value) { 
            T[]values=new T[count];
            for(int i=0;i<count;i++){
                values[i]=value;
            }
            base.InsertRange(pos, values);
            return pos;
        }
        public int insert(int pos, T[]array,int firstpos, int beforelastpos) {
            int len = beforelastpos - firstpos ;
            T[] data = new T[len];
            int index=0;
            for (int i = firstpos; i < beforelastpos; i++) {
                data[index++] = array[i];
            }
            base.InsertRange(pos, data);
            return pos;
        }
        public int insert(int pos, T[] array) {
            base.InsertRange(pos, array);
            return pos;
        }
        /// <summary>
        /// pos如果超出索引 将报错
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public T at(int pos) {
            return base[pos];
        }
        public void at(int pos, T @set) {
            base[pos] = @set;
        }
        public std_vector<T> data() {
            return this;
        }
        /// <summary>
        /// 返回关联分配器
        /// </summary>
        /// <returns></returns>
        public std_vector<T> get_allocator() {
            std_vector<T> copy = new std_vector<T>(this.ToArray());
            return copy;
        }
        /// <summary>
        /// 不检查数据 如果为空，则抛出异常
        /// </summary>
        /// <returns></returns>
        public T front() {
            return base[0];
        }
        /// <summary>
        /// 不检查数据 如果为空，则抛出异常
        /// </summary>
        /// <returns></returns>
        public T back() {
            return base[base.Count - 1];
        }
        public void assign(int @capacity, T value) {
            base.Clear();
            T[] values = new T[capacity];
            for (int i = 0; i < capacity; i++) {
                values[i] = value;
            }
            base.AddRange(values);
        }
        public void assign(T[] array, int beginpos, int beforeendpos) {
            //
            int len = (beforeendpos - beginpos );
            System.Diagnostics.Debug.Assert(len < array.Length);
            T[] values = new T[len];
            int index = 0;
            for (int i = beginpos; i < beforeendpos; i++) {
                values[index++] = array[i];
            }
            base.Clear();
            base.AddRange(values);
        }
        public void pop_back() {
            base.RemoveAt(base.Count - 1);
        }

        public void erase(T value, bool dataref) {
            base.Remove(value);
        }

        public T erase(int pos) {
            T obj = base[pos];
            base.RemoveAt(pos);
            return obj;
        }
        public T[] erase(int beginpos, int beforelastpos) {
            int len = beforelastpos - beginpos ;
            T[] array = new T[len];
            base.CopyTo(beginpos, array, 0, len);
            base.RemoveRange(beginpos, len);
            return array;
        }

        public void clear() {
            base.Clear();
        }

        public bool empty() {
            return base.Count == 0;
        }

        public int capacity() {
            return base.Capacity;
        }

        public int size() {
            return base.Count;
        }
        public void resize(int size) {
            //no do            
        }
        public void reserve(int count) {
            this.Capacity = count;
            //no do
        }
        public void shrink_to_fit() {
            //no do
        }
        public int max_size() {
            return int.MaxValue;
        }
   

        #endregion

        public static void swap(ref std_vector<T> _this,ref std_vector<T> _other) {
            std_vector<T> temp = _this;
            _this = _other;
            _other = temp;
        }
    }
}
