using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{

    /// <summary>
    /// 带头结点的单链表
    /// </summary>
    /// <typeparam name="T">泛型T</typeparam>
    public class std_forward_list<T> : IEnumerable<T> where T : IComparable<T>
    {
        private std_forward_list_node<T> head;   //头结点

        #region 构造函数

        public std_forward_list() {
            head = new std_forward_list_node<T>();
        }

        #endregion

        #region 属性

        public std_forward_list_node<T> Head {
            get { return head; }
            set { head = value; }
        }

        #endregion

        #region 单链表常规操作

        /// <summary>
        /// 求单链表长度
        /// </summary>
        /// <returns>长度</returns>
        public int GetLength() {
            std_forward_list_node<T> p = head.Next;
            int length = 0;
            while (p != null) {
                length++;
                p = p.Next;
            }
            return length;
        }

        /// <summary>
        /// 清空单链表
        /// </summary>
        public void clear() {
            head.Next = null;
        }

        /// <summary>
        /// 判断单链表是否为空
        /// </summary>
        /// <returns>true or false</returns>
        public bool empty() {
            return head.Next == null;
        }

        /// <summary>
        /// 在单链表的末尾添加新元素
        /// </summary>
        /// <param name="item">元素</param>
        public void Add(T item) {
            std_forward_list_node<T> willToInsert = new std_forward_list_node<T>(item);
            std_forward_list_node<T> currentNode = new std_forward_list_node<T>();
            if (head.Next == null) {
                head.Next = willToInsert;
                return;
            }

            currentNode = head.Next;
            while (currentNode.Next != null) {
                currentNode = currentNode.Next;
            }

            currentNode.Next = willToInsert;
        }


        /// <summary>
        /// 在单链表的第i个结点的位置前插入一个值为item的结点
        /// </summary>
        /// <param name="i">位置，从0开始的索引值</param>
        /// <param name="item">元素值</param>
        public void Insert(int i, T item) {
            if (empty() || i < 0) {
                throw new InvalidOperationException("InvalidOperationException:链表为空或参数小于0，无法执行插入操作");

            }

            if (i >= this.GetLength()) {
                throw new InvalidOperationException("InvalidOperationException:插入位置超出范围，无法执行插入操作");

            }

            if (i == 0) {
                std_forward_list_node<T> willToInsert = new std_forward_list_node<T>(item);

                willToInsert.Next = head.Next;
                head.Next = willToInsert;
                return;
            }

            std_forward_list_node<T> currentNode = head.Next;
            std_forward_list_node<T> tmp = new std_forward_list_node<T>();
            int j = 0;

            while (currentNode.Next != null && j < i) {
                tmp = currentNode;
                currentNode = currentNode.Next;
                j++;
            }

            if (j == i) {
                std_forward_list_node<T> willToInsert = new std_forward_list_node<T>(item);

                willToInsert.Next = currentNode;
                tmp.Next = willToInsert;
            }

        }


        /// <summary>
        /// 在单链表的第i个结点的位置后插入一个值为item的结点
        /// </summary>
        /// <param name="i">位置，从0开始的索引值</param>
        /// <param name="item">元素值</param>
        public void InsertPost(int i, T item) {
            if (empty() || i < 0) {
                throw new InvalidOperationException("InvalidOperationException:链表为空或参数小于0，无法执行向后插入操作");

            }
            if (i >= this.GetLength()) {
                throw new InvalidOperationException("InvalidOperationException:插入位置超出范围，无法执行向后插入操作");
            }


            if (i == 0) {
                std_forward_list_node<T> willToInsert = new std_forward_list_node<T>(item);

                willToInsert.Next = head.Next.Next;
                head.Next.Next = willToInsert;
                return;
            }

            std_forward_list_node<T> currentNode = head.Next;

            int j = 0;
            while (currentNode != null && j < i) {
                currentNode = currentNode.Next;
                j++;
            }
            if (j == i) {
                std_forward_list_node<T> willToInsert = new std_forward_list_node<T>(item);

                willToInsert.Next = currentNode.Next;
                currentNode.Next = willToInsert;
            }
        }



        /// <summary>
        /// 删除单链表第i个结点
        /// </summary>
        /// <param name="i">索引值，从0开始</param>
        /// <returns>删除的结点的值</returns>
        public T Delete(int i) {
            if (empty() || i < 0) {
                throw new ArgumentException("ArgumentException:参数不合法，无法执行删除操作");

            }
            std_forward_list_node<T> tmp = new std_forward_list_node<T>();

            if (i == 0) {
                tmp = head.Next;
                head.Next = tmp.Next;
                return tmp.Data;
            }

            std_forward_list_node<T> currentNode = head.Next;

            int j = 0;
            while (currentNode.Next != null && j < i) {

                j++;
                tmp = currentNode;
                currentNode = currentNode.Next;
            }

            if (j == i) {
                tmp.Next = currentNode.Next;
                return currentNode.Data;
            }
            else {
                throw new InvalidOperationException("InvalidOperationException:结点不存在，无法执行删除操作");

            }

        }

        /// <summary>
        /// 获取第i个元素
        /// </summary>
        /// <param name="i">索引值，从0开始</param>
        /// <returns>返回结点的数据域</returns>
        public T GetElem(int i) {
            if (empty() || i < 0 || i > this.GetLength() - 1) {

                throw new ArgumentException("ArgumentException:参数不合法，无法获取元素");

            }
            std_forward_list_node<T> currentNode = new std_forward_list_node<T>();

            currentNode = head.Next;
            int j = 0;
            while (currentNode.Next != null && j < i) {

                j++;
                currentNode = currentNode.Next;
            }

            return currentNode.Data;
        }
        public std_forward_list_node<T> GetElemNode(int i) {
            if (empty() || i < 0 || i > this.GetLength() - 1) {

                throw new ArgumentException("ArgumentException:参数不合法，无法获取元素");

            }
            std_forward_list_node<T> currentNode = new std_forward_list_node<T>();

            currentNode = head.Next;
            int j = 0;
            while (currentNode.Next != null && j < i) {
                j++;
                currentNode = currentNode.Next;
            }

            return currentNode;
        }

        /// <summary>
        /// 在单链表中查找值为item的结点
        /// </summary>
        /// <param name="item">需要查找的结点值</param>
        /// <returns>元素所在的位置，从0开始的索引值</returns>
        public int Locate(T item) {
            if (empty()) {
                throw new InvalidOperationException("InvalidOperationException:单链表为空，无法执行定位操作");

            }
            std_forward_list_node<T> currentNode = head.Next;
            int i = 0;
            while (!currentNode.Data.Equals(item) && currentNode.Next != null) {

                currentNode = currentNode.Next;
                i++;
            }
            if (currentNode.Data.CompareTo(item) == 0)
                return i;
            else
                return -1;

        }



        /// <summary>
        /// 倒置操作
        /// </summary>
        public void ReversLinkList() {
            std_forward_list_node<T> p = Head.Next;
            std_forward_list_node<T> q = new std_forward_list_node<T>();

            Head.Next = null;
            while (p != null) {
                q = p;
                p = p.Next;
                q.Next = Head.Next;
                Head.Next = q;
            }

        }

        #endregion


        #region 单链表的建立


        /// <summary>
        /// 静态方法，在单链表的头部插入结点建立单链表，造成链表中的数据与逻辑顺序相反
        /// </summary>
        /// <param name="items">数组</param>
        /// <returns></returns>
        public static std_forward_list<T> CreateListHead(T[] items) {

            std_forward_list<T> list = new std_forward_list<T>();

            for (int i = 0; i < items.Length; i++) {
                std_forward_list_node<T> p = new std_forward_list_node<T>(items[i]);

                p.Next = list.Head.Next;
                list.Head.Next = p;
            }
            return list;
        }



        /// <summary>
        /// 静态方法，在单链表的尾部插入结点建立单链表，一般使用这个方法，符合逻辑
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static std_forward_list<T> CreateListTail(T[] items) {

            std_forward_list_node<T> node = new std_forward_list_node<T>();

            std_forward_list<T> list = new std_forward_list<T>();
            node = list.Head.Next;

            for (int i = 0; i < items.Length; i++) {

                std_forward_list_node<T> p = new std_forward_list_node<T>(items[i]);

                if (list.Head.Next == null) {
                    list.Head.Next = p;
                }
                else {
                    node.Next = p;
                }
                node = p;
            }

            if (node != null) {
                node.Next = null;
            }
            return list;
        }

        #endregion





        #region 合并操作



        /// <summary>
        /// 使用该方法的前提条件是，两个链表均以按非递减顺序排列
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="other"></param>
        /// <returns></returns>
        public std_forward_list<T> merge<TItem>(std_forward_list<T> other) where TItem : T {

            std_forward_list<T> linklist = new std_forward_list<T>();

            std_forward_list_node<T> p = Head.Next;

            std_forward_list_node<T> q = other.Head.Next;

            std_forward_list_node<T> s = new std_forward_list_node<T>();



            linklist.Head = Head;
            linklist.Head.Next = null;


            while (p != null && q != null) {
                if (p.Data.CompareTo(q.Data) < 0) {
                    s = p;
                    p = p.Next;
                }
                else {
                    s = q;
                    q = q.Next;
                }

                linklist.Add(s.Data);

            }

            if (p == null) {

                p = q;

            }

            while (p != null) {
                s = p;
                p = p.Next;
                linklist.Add(s.Data);
            }

            return linklist;

        }

        #endregion
        public T[] get_allocator() {
            List<T> members = new List<T>();
            std_forward_list_node<T> p = head.Next;
            int length = 0;
            while (p != null) {
                members.Add(p.Data);
                length++;
                p = p.Next;
            }
            return members.ToArray();
        }
        public void reverse() {
            this.ReversLinkList();
        }

        /// <summary>
        /// 删除重复项
        /// </summary>
        //public void unique() { 
        //}
        ///// <summary>
        ///// 条件删除
        ///// </summary>
        ///// <param name="ondelete"></param>
        //public void remove_if(Action<T> ondelete) { 

        //}
        ///// <summary>
        ///// 排序
        ///// </summary>
        //public void sort() { 
        //}
        public int before_begin() {
            return 0;
        }
        public int begin() {
            return 1;
        }
        public int end() {
            return this.GetLength() - 1;
        }
        public T this[int index] {
            get {
                return GetElem(index);
            }
            set {
                std_forward_list_node<T> node = GetElemNode(index);
                node.Data = value;
            }
        }
        public T front() {
            return head.Next.Data;
        }
        public int erase_after(int pos) {
            //std_forward_list_node<T> node = GetElemNode(pos);
            //std_forward_list_node<T> next = node.Next;
            //if (next != null) {
            //    std_forward_list_node<T> next2 = next.Next;
            //    if (next2 != null) {
            //        node.Next = next2;
            //    }
            //}
            Delete(pos + 1);
            return pos + 1;
        }
        public int erase_after(int beginpos, int endpos) {
            //std_forward_list_node<T> begin = GetElemNode(beginpos);
            //std_forward_list_node<T> end = GetElemNode(endpos+1);
            //begin.Next = end;
            for (int i = endpos; i >= beginpos; i++) {
                Delete(i);
            }
            return beginpos + 1;
        }
        public int insert_after(int pos, T @value) {
            //std_forward_list_node<T> node = GetElemNode(pos);
            //std_forward_list_node<T> next_old = node.Next;
            //std_forward_list_node<T> insert = new std_forward_list_node<T>(@value);
            //node.Next = insert;
            //insert.Next = next_old;
            InsertPost(pos, @value);
            return pos + 1;
        }
        public int insert_after(int pos, uint num, T @value) {
            //std_forward_list_node<T> node = GetElemNode(pos);
            //std_forward_list_node<T> next_old = node.Next;
            //std_forward_list_node<T> before = null;
            for (int i = 0; i < num; i++) {
                //std_forward_list_node<T> insert = new std_forward_list_node<T>(@value);

                //if (i == num - 1) {
                //    insert.Next = next_old;
                //}
                //if (i == 0) {
                //    node.Next = insert;
                //}
                //else {
                //    before.Next = insert;
                //}
                //before = insert;
                InsertPost(pos + i, @value);
            }
            return pos + (int)num;
        }
        public int insert_after(int pos, T[] array) {
            //std_forward_list_node<T> node = GetElemNode(pos);
            //std_forward_list_node<T> next_old = node.Next;
            //std_forward_list_node<T> before = null;
            int num = array.Length;
            for (int i = 0; i < num; i++) {
                //std_forward_list_node<T> insert = new std_forward_list_node<T>(array[i]);

                //if (i == num - 1) {
                //    insert.Next = next_old;
                //}
                //if (i == 0) {
                //    node.Next = insert;
                //}
                //else {
                //    before.Next = insert;
                //}
                //before = insert;
                InsertPost(pos + i, array[i]);
            }
            return pos + num;
        }
        public void remove(int pos) {
            Delete(pos);
        }
        public void pop_front() {
            Delete(0);
        }
        public void push_front(T @value) {
            Insert(0, @value);
        }
        public static void swap(ref std_forward_list<T> _this, ref std_forward_list<T> _other) {
            std_forward_list<T> temp = _this;
            _this = _other;
            _other = temp;
        }




        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator() {
            std_forward_list_node<T> p = head.Next;
            //int length = 0;
            while (p != null) {
                yield return p.Data;
                //length++;
                p = p.Next;
            }
        }

        #endregion

        #region IEnumerable 成员

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            std_forward_list_node<T> p = head.Next;
            //int length = 0;
            while (p != null) {
                yield return p;
                //length++;
                p = p.Next;
            }
        }

        #endregion
    }

    public class std_forward_list_node<T>
    {
        private T data;         //数据域
        private std_forward_list_node<T> next;   //引用域
        #region 构造函数
        public std_forward_list_node() {
            this.data = default(T);
            this.next = null;
        }
        public std_forward_list_node(T data) {
            this.data = data;
            this.next = null;
        }
        public std_forward_list_node(std_forward_list_node<T> next) {
            this.next = next;
        }
        public std_forward_list_node(T data, std_forward_list_node<T> next) {
            this.data = data;
            this.next = next;
        }

        #endregion

        #region 属性

        public T Data {
            get { return data; }
            set { data = value; }
        }

        public std_forward_list_node<T> Next {
            get { return next; }
            set { next = value; }
        }

        #endregion

    }

}
