using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{
    /// <summary>
    /// like c++ std::map  字典 1对1 相当于SortedDictionary<TKey,TValue>类
    /// </summary>
    public class std_map<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        public std_map()
            : base() {
        }
        public std_map(IComparer<TKey> comparer)
            : base(comparer) {
        }

        public std_map(IDictionary<TKey, TValue> dictionary)
            : base(dictionary) {
        }

        public std_map(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer) :
            base(dictionary, comparer) {
            Dictionary<TKey, TValue> aa;
        }

        //public std_pair<TKey,TValue> begin() {
        //    Enumerator et = base.GetEnumerator();
        //    return new std_pair<TKey,TValue>(et.Current.Key,et.Current.Value);
        //}
        public int begin() {
            return 0;
        }
        //public std_pair<TKey, TValue> end() {
        //    Enumerator et = base.GetEnumerator();
        //    KeyValuePair<TKey, TValue> kp = et.Current;
        //    while (et.MoveNext()) {
        //        kp = et.Current;
        //    }
        //    return new std_pair<TKey, TValue>(kp.Key,kp.Value);

        //}
        public int end() {
            return base.Count - 1;
        }
        public TValue at(TKey key) {
            return base[key];
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void at(TKey key, TValue @value) {
            base[key] = @value;
        }
        public void clear() {
            base.Clear();
        }
        /// <summary>
        /// return 0 or 1
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int count(TKey key) {
            if (base.ContainsKey(key))
                return 1;
            return 0;
        }

        public void emplace(TKey key, TValue @value) {
            base.Add(key, @value);
        }
        public bool empty() {
            return base.Count == 0;
        }
        public std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> equal_range(TKey key) {
            //return lower bound  and up bound
            if (!base.ContainsKey(key)) {
                return null;
            }
            //KeyValuePair<TKey, TValue> select =new KeyValuePair<TKey,TValue>(key, base[key]);
            KeyValuePair<TKey, TValue> first = new KeyValuePair<TKey, TValue>();
            KeyValuePair<TKey, TValue> second = new KeyValuePair<TKey, TValue>();
            bool find_first = false;
            bool find_second = false;
            //查找下一个
            Enumerator et = base.GetEnumerator();
            if (et.Current.Key.Equals(key)) {
                first = et.Current;
                find_first = true;
            }
            while (et.MoveNext()) {
                if (find_first) {
                    second = et.Current;
                    find_second = true;
                    break;
                }
                else {
                    if (et.Current.Key.Equals(key)) {
                        first = et.Current;
                        find_first = true;
                    }
                }
            }

            std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>> range = new std_pair<std_pair<TKey, TValue>, std_pair<TKey, TValue>>(
                find_first ? new std_pair<TKey, TValue>(first.Key, first.Value) : null,
                find_second ? new std_pair<TKey, TValue>(second.Key, second.Value) : null);
            return range;
        }


        public void erase(int pos, bool index) {

            int len = base.Keys.Count;
            TKey[] keys = new TKey[len];
            base.Keys.CopyTo(keys, 0);
            base.Remove(keys[pos]);
            //int i = 0;
            //object key=null ;
            //foreach (var v in base.Keys) {
            //    if (i == pos) {
            //        key = v;
            //        break;
            //    }
            //    i++;
            //}
            //if (key != null) {
            //    base.Remove((TKey)key);
            //}
        }
        public void erase(int it_beginpos, int it_endpos) {
            int len = base.Keys.Count;
            TKey[] keys = new TKey[len];
            base.Keys.CopyTo(keys, 0);

            for (int i = it_beginpos; i <= it_endpos; i++) {
                base.Remove(keys[i]);
            }
        }
        public void erase(TKey key) {
            if (base.ContainsKey(key)) {
                base.Remove(key);
            }
        }
        /// <summary>
        /// not like c++ find
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool find(TKey key) {
            //TValue tv;
            //bool find= base.TryGetValue(key, out tv);
            //if (find) { 

            //}
            //return key;
            return base.ContainsKey(key);
        }

        public KeyValuePair<TKey, TValue>[] get_allocator() {
            KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[base.Count];
            base.CopyTo(array, 0);
            return array;
        }
        public static std_pair<TKey, TValue>[] get_allocator(std_map<TKey, TValue> _this) {
            KeyValuePair<TKey, TValue>[] array = new KeyValuePair<TKey, TValue>[_this.Count];
            _this.CopyTo(array, 0);
            std_pair<TKey, TValue>[] sps = new std_pair<TKey, TValue>[_this.Count];
            for (int i = 0; i < _this.Count; i++) {
                sps[i] = new std_pair<TKey, TValue>(array[i]);
            }
            return sps;

        }

        public std_pair<KeyValuePair<TKey, TValue>, bool> insert(TKey key, TValue @value) {
            std_pair<KeyValuePair<TKey, TValue>, bool> ret = new std_pair<KeyValuePair<TKey, TValue>, bool>(new KeyValuePair<TKey, TValue>(key, @value), false);
            if (!base.ContainsKey(key)) {
                base.Add(key, @value);
                ret.Second = true;
            }
            else {
                ret = new std_pair<KeyValuePair<TKey, TValue>, bool>(new KeyValuePair<TKey, TValue>(key, base[key]), false);
            }
            return ret;
        }

        public void insert(int pos, TKey key, TValue value) {
            insert(key, value);
        }
        public void insert(std_map<TKey, TValue> _other, int beginpos, int endpos) {
            int index = 0;
            bool find_begin = false;
            bool find_end = false;
            foreach (var v in _other) {
                if (index == beginpos) {
                    find_begin = true;
                }
                if (index > endpos) {
                    find_end = true;
                }
                if (find_begin && !find_end) {
                    if (!base.ContainsKey(v.Key)) {
                        base.Add(v.Key, v.Value);
                    }
                }
                index++;
            }

        }

        public TKey[] key_comp() {
            TKey[] keys = new TKey[base.Count];
            base.Keys.CopyTo(keys, 0);
            return keys;
        }
        public TValue[] value_comp() {
            TValue[] keys = new TValue[base.Count];
            base.Values.CopyTo(keys, 0);
            return keys;
        }
        public std_pair<TKey, TValue> lower_bound(TKey key) {
            std_pair<TKey, TValue> sp = null;
            if (base.ContainsKey(key)) {
                sp = new std_pair<TKey, TValue>(key, base[key]);
            }
            return sp;
        }
        public std_pair<TKey, TValue> upper_bound(TKey key) {
            std_pair<TKey, TValue> sp = null;
            if (base.ContainsKey(key)) {
                bool find_pre = false;
                foreach (var v in base.Keys) {
                    if (find_pre) {
                        sp = new std_pair<TKey, TValue>(v, base[v]);
                        break;
                    }
                    if (v.Equals(key)) {
                        find_pre = true;
                    }
                }
            }
            return sp;
        }


        public int max_size() {
            return int.MaxValue;
        }
        public int size() {
            return base.Count;
        }

        public static void swap(ref std_map<TKey, TValue> _this, ref std_map<TKey, TValue> _other) {
            std_map<TKey, TValue> temp = _this;
            _this = _other;
            _other = temp;
        }
    }
}
