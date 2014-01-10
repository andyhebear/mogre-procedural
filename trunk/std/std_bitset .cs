using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Mogre_Procedural.std
{
    public interface Istd_bitset
    {
        /// <summary>
        /// b中在pos处的二进制位是否为1？
        /// </summary>
        /// <returns></returns>
        public bool test(int pos);//访问特定位
        /// <summary>
        /// 是否所有都是置为1的二进制位
        /// </summary>
        /// <returns></returns>
        public bool all();//检查是否所有，任何或没有位被设置为true 
        /// <summary>
        /// 是否存在置为1的二进制位？
        /// </summary>
        /// <returns></returns>
        public bool any();
        /// <summary>
        /// b中不存在置为1的二进制位吗？
        /// </summary>
        /// <returns></returns>
        public bool none();
        /// <summary>
        /// the count of "true"
        /// </summary>
        public int count();
        /// <summary>
        /// returns the size number of bits that the bitset can hold
        /// 返回的大小，位的bitset可容纳的数
        /// </summary>
        /// <returns></returns>
        public int size();
        /// <summary>
        /// 把b中所有二进制位都置为1
        /// </summary>
        public void set(); //Set bits (public member function )
        /// <summary>
        /// 把b中在pos处的二进制位置为1
        /// </summary>
        /// <param name="pos"></param>
        public void set(int pos);
        /// <summary>
        /// 把b中所有二进制位都置为0
        /// </summary>
        public void reset();//Reset bits (public member function )
        /// <summary>
        /// 把b中在pos处的二进制位置为0
        /// </summary>
        /// <param name="pos"></param>
        public void reset(int pos);
        /// <summary>
        /// 把b中所有二进制位逐位取反
        /// </summary>
        public void flip();//Flip bits (public member function )
        /// <summary>
        /// 把b中在pos处的二进制位取反
        /// </summary>
        /// <param name="pos"></param>
        public void flip(int pos);

        public string to_string();//Convert to string (public member function )
        /// <summary>
        /// 用b中同样的二进制位返回一个unsigned int值
        /// </summary>
        /// <returns></returns>
        public uint to_ulong();//Convert to unsigned long integer (public member function )
        /// <summary>
        /// 用b中同样的二进制位返回一个unsigned long值
        /// </summary>
        /// <returns></returns>
        public ulong to_ullong();// Convert to unsigned long long (public member function )
    }
    /// <summary>
    ///like c++ std::bitset
    ///wrapper bit operat method
    /// </summary>
    public sealed class std_bitset : Istd_bitset, ICollection, IEnumerable, ICloneable
    {
        protected BitArray bitarray;
        public std_bitset(std_bitset @stdbitset) {
            System.Diagnostics.Debug.Assert(stdbitset != null);
            bitarray = new BitArray(stdbitset.bitarray);
        }
        public std_bitset(bool[] values) {
            bitarray = new BitArray(values);
        }
        public std_bitset(byte[] bytes) {
            bitarray = new BitArray(bytes);
        }
        public std_bitset(int[] bytes) {
            bitarray = new BitArray(bytes);
        }

        public std_bitset(int len) {
            bitarray = new BitArray(len);
        }
        public std_bitset(int len, bool defaultvalue) {
            bitarray = new BitArray(len, defaultvalue);
        }

        #region Istd_bitset 成员
        /// <summary>
        /// b中在pos处的二进制位是否为1？
        /// </summary>
        /// <returns></returns>
        public bool test(int pos)//访问特定位
        {
           return bitarray.Get(pos);
        }
        /// <summary>
        /// 是否所有都是置为1的二进制位
        /// </summary>
        /// <returns></returns>
        public bool all()//检查是否所有，任何或没有位被设置为true 
        {            
            int c = bitarray.Count;
            for (int i = 0; i < c; i++) {
                if (!bitarray.Get(i)) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否存在置为1的二进制位？
        /// </summary>
        /// <returns></returns>
        public bool any() {
            int c = bitarray.Count;
            for (int i = 0; i < c; i++) {
                if (bitarray.Get(i)) {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// b中不存在置为1的二进制位吗？
        /// </summary>
        /// <returns></returns>
        public bool none() {
            int c = bitarray.Count;
            for (int i = 0; i < c; i++) {
                if (bitarray.Get(i)) {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// the count of "true"
        /// </summary>
        public int count() {
            int c = bitarray.Count;
            int tc = 0;
            for (int i = 0; i < c; i++) {
                if (bitarray.Get(i)) {
                    tc++;
                }
            }
            return tc;
        }
        /// <summary>
        /// returns the size number of bits that the bitset can hold
        /// 返回的大小，位的bitset可容纳的数
        /// </summary>
        /// <returns></returns>
        public int size() {
            return bitarray.Count;
        }
        /// <summary>
        /// 把b中所有二进制位都置为1
        /// </summary>
        public void set() { //Set bits (public member function )
            bitarray.SetAll(true);
        }
        /// <summary>
        /// 把b中在pos处的二进制位置为1
        /// </summary>
        /// <param name="pos"></param>
        public void set(int pos) {
            bitarray.Set(pos, true);
        }
        /// <summary>
        /// 把b中所有二进制位都置为0
        /// </summary>
        public void reset() {//Reset bits (public member function )
            bitarray.SetAll(false);
        }
        /// <summary>
        /// 把b中在pos处的二进制位置为0
        /// </summary>
        /// <param name="pos"></param>
        public void reset(int pos) {
            bitarray.Set(pos, false);
        }
        /// <summary>
        /// 把b中所有二进制位逐位取反
        /// </summary>
        public void flip() {//Flip bits (public member function )
            bitarray.Not();
        }
        /// <summary>
        /// 把b中在pos处的二进制位取反
        /// </summary>
        /// <param name="pos"></param>
        public void flip(int pos) {
            bool pv = bitarray.Get(pos);
            bitarray.Set(pos, !pv);
        }

        public string to_string() {//Convert to string (public member function )
            //TODO:
            return bitarray.ToString();
        }
        /// <summary>
        /// 用b中同样的二进制位返回一个unsigned int值
        /// 位的长度>32将出错
        /// </summary>
        /// <returns></returns>
        public uint to_ulong() {//Convert to unsigned long integer (public member function )
            int c = bitarray.Count;
            byte[] buf = new byte[c];
            bitarray.CopyTo(buf, 0);
            return BitConverter.ToUInt32(buf, 0);
        }
        /// <summary>
        /// 用b中同样的二进制位返回一个unsigned long值
        /// 位的长度>64将出错
        /// </summary>
        /// <returns></returns>
        public ulong to_ullong() {// Convert to unsigned long long (public member function )
            int c = bitarray.Count;
            byte[] buf = new byte[c];
            bitarray.CopyTo(buf, 0);
            return BitConverter.ToUInt64(buf, 0);           
        }

        #endregion

        #region ICollection 成员
        /// <summary>
        /// copy到2进制数组中
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(Array array, int index) {
            bitarray.CopyTo(array, index);
        }
        /// <summary>
        /// 1的个数
        /// </summary>
        public int Count {
            get { return count(); }
        }

        public bool IsSynchronized {
            get { throw new NotImplementedException(); }
        }

        public object SyncRoot {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IEnumerable 成员

        public IEnumerator GetEnumerator() {
            return this.bitarray.GetEnumerator();
        }

        #endregion

        #region ICloneable 成员

        public object Clone() {
            std_bitset sbs = new std_bitset(this);
            return sbs;
        }

        #endregion

    }

    /*
* A BitSet to replace java.util.BitSet.
* Primary differences are that most set operators return new sets
* as opposed to oring and anding "in place". Further, a number of
* operations were added. I cannot contain a BitSet because there
* is no way to access the internal bits (which I need for speed)
* and, because it is final, I cannot subclass to add functionality.
* Consider defining set degree. Without access to the bits, I must
* call a method n times to test the ith bit...ack!
*
* Also seems like or() from util is wrong when size of incoming set is bigger
* than this.bits.length.
*
* @author Terence Parr
* @author <br><a href="mailto:pete@yamuna.demon.co.uk">Pete Wells</a>
*/
//    private class BitSet : ICloneable
//    {
//        protected internal const int BITS = 64; // number of bits / long
//        protected internal const int NIBBLE = 4;
//        protected internal const int LOG_BITS = 6; // 2^6 == 64

//        /*
//* We will often need to do a mod operator (i mod nbits). Its
//* turns out that, for powers of two, this mod operation is
//* same as (i & (nbits-1)). Since mod is slow, we use a
//* precomputed mod mask to do the mod instead.
//*/
//        protected internal static readonly int MOD_MASK = BITS - 1;

//        /* The actual data bits */
//        protected internal long[] dataBits;

//        /* Construct a bitset of size one word (64 bits) */
//        public BitSet()
//            : this(BITS) {
//        }

//        /* Construction from a static array of longs */
//        public BitSet(long[] bits_) {
//            dataBits = bits_;
//        }

//        /*
//* Construct a bitset given the size
//* @param nbits The size of the bitset in bits
//*/
//        public BitSet(int nbits) {
//            dataBits = new long[((nbits - 1) >> LOG_BITS) + 1];
//        }

//        /* OR this element into this set (grow as necessary to accommodate) */
//        public virtual void Add(int el) {
//            int n = wordNumber(el);
//            if (n >= dataBits.Length) {
//                GrowToInclude(el);
//            }
//            dataBits[n] |= BitMask(el);
//        }

//        public virtual BitSet And(BitSet a) {
//            BitSet s = (BitSet)this.Clone();
//            s.AndInPlace(a);
//            return s;
//        }

//        public virtual void AndInPlace(BitSet a) {
//            int min = (int)(Math.Min(dataBits.Length, a.dataBits.Length));
//            for (int i = min - 1; i >= 0; i--) {
//                dataBits[i] &= a.dataBits[i];
//            }
//            // clear all bits in this not present in a (if this bigger than a).
//            for (int i = min; i < dataBits.Length; i++) {
//                dataBits[i] = 0;
//            }
//        }

//        protected static long BitMask(int bitNumber) {
//            int bitPosition = bitNumber & MOD_MASK; // bitNumber mod BITS
//            return 1L << bitPosition;
//        }

//        public virtual void Clear() {
//            for (int i = dataBits.Length - 1; i >= 0; i--) {
//                dataBits[i] = 0;
//            }
//        }

//        public virtual void Clear(int el) {
//            int n = wordNumber(el);
//            if (n >= dataBits.Length) {
//                // grow as necessary to accommodate
//                GrowToInclude(el);
//            }
//            dataBits[n] &= ~BitMask(el);
//        }

//        public virtual object Clone() {
//            BitSet s;
//            try {
//                s = new BitSet();
//                s.dataBits = new long[dataBits.Length];
//                Array.Copy(dataBits, 0, s.dataBits, 0, dataBits.Length);
//            }
//            catch //(System.Exception e)
//            {
//                throw new System.ApplicationException();
//            }
//            return s;
//        }

//        public virtual int Degree() {
//            int deg = 0;
//            for (int i = dataBits.Length - 1; i >= 0; i--) {
//                long word = dataBits[i];
//                if (word != 0L) {
//                    for (int bit = BITS - 1; bit >= 0; bit--) {
//                        if ((word & (1L << bit)) != 0) {
//                            deg++;
//                        }
//                    }
//                }
//            }
//            return deg;
//        }

//        override public int GetHashCode() {
//            return dataBits.GetHashCode();
//        }
//        //    public override int GetHashCode() {
//        //        ulong result = 1;
//        //        for (uint i = 0; i < _data.Length; i++) {
//        //            if (_data[i] != 0) {
//        //                result = result * 31 ^ i;
//        //                result = result * 31 ^ _data[i];
//        //            }
//        //        }


//        //        return result.GetHashCode();
//        //    }
//        /* Code "inherited" from java.util.BitSet */
//        override public bool Equals(object obj) {
//            if ((obj != null) && (obj is BitSet)) {
//                BitSet bset = (BitSet)obj;

//                int n = (int)(System.Math.Min(dataBits.Length, bset.dataBits.Length));
//                for (int i = n; i-- > 0; ) {
//                    if (dataBits[i] != bset.dataBits[i]) {
//                        return false;
//                    }
//                }
//                if (dataBits.Length > n) {
//                    for (int i = (int)(dataBits.Length); i-- > n; ) {
//                        if (dataBits[i] != 0) {
//                            return false;
//                        }
//                    }
//                }
//                else if (bset.dataBits.Length > n) {
//                    for (int i = (int)(bset.dataBits.Length); i-- > n; ) {
//                        if (bset.dataBits[i] != 0) {
//                            return false;
//                        }
//                    }
//                }
//                return true;
//            }
//            return false;
//        }

//        /*
//* Grows the set to a larger number of bits.
//* @param bit element that must fit in set
//*/
//        public virtual void GrowToInclude(int bit) {
//            int newSize = (int)(System.Math.Max(dataBits.Length << 1, numWordsToHold(bit)));
//            long[] newbits = new long[newSize];
//            Array.Copy(dataBits, 0, newbits, 0, dataBits.Length);
//            dataBits = newbits;
//        }

//        public virtual bool Member(int el) {
//            int n = wordNumber(el);
//            if (n >= dataBits.Length)
//                return false;
//            return (dataBits[n] & BitMask(el)) != 0;
//        }

//        public virtual bool Nil() {
//            for (int i = dataBits.Length - 1; i >= 0; i--) {
//                if (dataBits[i] != 0)
//                    return false;
//            }
//            return true;
//        }

//        public virtual BitSet Not() {
//            BitSet s = (BitSet)this.Clone();
//            s.NotInPlace();
//            return s;
//        }

//        public virtual void NotInPlace() {
//            for (int i = dataBits.Length - 1; i >= 0; i--) {
//                dataBits[i] = ~dataBits[i];
//            }
//        }

//        /* Complement bits in the range 0..maxBit. */
//        public virtual void NotInPlace(int maxBit) {
//            NotInPlace(0, maxBit);
//        }

//        /* Complement bits in the range minBit..maxBit.*/
//        public virtual void NotInPlace(int minBit, int maxBit) {
//            // Make sure that we have room for maxBit
//            GrowToInclude(maxBit);
//            for (int i = minBit; i <= maxBit; i++) {
//                int n = wordNumber(i);
//                dataBits[n] ^= BitMask(i);
//            }
//        }

//        private int numWordsToHold(int el) {
//            return (el >> LOG_BITS) + 1;
//        }

//        public static BitSet of(int el) {
//            BitSet s = new BitSet(el + 1);
//            s.Add(el);
//            return s;
//        }

//        /* Return this | a in a new set. */
//        public virtual BitSet Or(BitSet a) {
//            BitSet s = (BitSet)this.Clone();
//            s.OrInPlace(a);
//            return s;
//        }

//        public virtual void OrInPlace(BitSet a) {
//            // If this is smaller than a, grow this first
//            if (a.dataBits.Length > dataBits.Length) {
//                setSize((int)(a.dataBits.Length));
//            }
//            int min = (int)(System.Math.Min(dataBits.Length, a.dataBits.Length));
//            for (int i = min - 1; i >= 0; i--) {
//                dataBits[i] |= a.dataBits[i];
//            }
//        }

//        /* Remove this element from this set. */
//        public virtual void Remove(int el) {
//            int n = wordNumber(el);
//            if (n >= dataBits.Length) {
//                GrowToInclude(el);
//            }
//            dataBits[n] &= ~BitMask(el);
//        }

//        /*
//* Sets the size of a set.
//* @param nwords how many words the new set should be
//*/
//        private void setSize(int nwords) {
//            long[] newbits = new long[nwords];
//            int n = (int)(System.Math.Min(nwords, dataBits.Length));
//            Array.Copy(dataBits, 0, newbits, 0, n);
//            dataBits = newbits;
//        }

//        public virtual int size() {
//            return dataBits.Length << LOG_BITS; // num words * bits per word
//        }

//        /*
//* Return how much space is being used by the dataBits array not
//* how many actually have member bits on.
//*/
//        public virtual int LengthInLongWords() {
//            return dataBits.Length;
//        }

//        /* Is this contained within a? */
//        public virtual bool Subset(BitSet a) {
//            if (a == null) //(a == null || !(a is BitSet))
//                return false;
//            return this.And(a).Equals(this);
//        }

//        /*
//* Subtract the elements of 'a' from 'this' in-place.
//* Basically, just turn off all bits of 'this' that are in 'a'.
//*/
//        public virtual void SubtractInPlace(BitSet a) {
//            if (a == null)
//                return;
//            // for all words of 'a', turn off corresponding bits of 'this'
//            for (int i = 0; i < dataBits.Length && i < a.dataBits.Length; i++) {
//                dataBits[i] &= ~a.dataBits[i];
//            }
//        }

//        public virtual int[] ToArray() {
//            int[] elems = new int[Degree()];
//            int en = 0;
//            for (int i = 0; i < (dataBits.Length << LOG_BITS); i++) {
//                if (Member(i)) {
//                    elems[en++] = i;
//                }
//            }
//            return elems;
//        }

//        public virtual long[] ToPackedArray() {
//            return dataBits;
//        }

//        override public string ToString() {
//            return ToString(",");
//        }

//        /*
//* Transform a bit set into a string by formatting each element as an integer
//* @separator The string to put in between elements
//* @return A commma-separated list of values
//*/
//        public virtual string ToString(string separator) {
//            string str = "";
//            for (int i = 0; i < (dataBits.Length << LOG_BITS); i++) {
//                if (Member(i)) {
//                    if (str.Length > 0) {
//                        str += separator;
//                    }
//                    str = str + i;
//                }
//            }
//            return str;
//        }

//        /*
//* Create a string representation where instead of integer elements, the
//* ith element of vocabulary is displayed instead. Vocabulary is a Vector
//* of Strings.
//* @separator The string to put in between elements
//* @return A commma-separated list of character constants.
//*/
//        public virtual string ToString(string separator, ArrayList vocabulary) {
//            if (vocabulary == null) {
//                return ToString(separator);
//            }
//            string str = "";
//            for (int i = 0; i < (dataBits.Length << LOG_BITS); i++) {
//                if (Member(i)) {
//                    if (str.Length > 0) {
//                        str += separator;
//                    }
//                    if (i >= vocabulary.Count) {
//                        str += "<bad element " + i + ">";
//                    }
//                    else if (vocabulary[i] == null) {
//                        str += "<" + i + ">";
//                    }
//                    else {
//                        str += (string)vocabulary[i];
//                    }
//                }
//            }
//            return str;
//        }

//        /*
//* Dump a comma-separated list of the words making up the bit set.
//* Split each 64 bit number into two more manageable 32 bit numbers.
//* This generates a comma-separated list of C++-like unsigned long constants.
//*/
//        public virtual string ToStringOfHalfWords() {
//            string s = new string("".ToCharArray());
//            for (int i = 0; i < dataBits.Length; i++) {
//                if (i != 0)
//                    s += ", ";
//                long tmp = dataBits[i];
//                tmp &= 0xFFFFFFFFL;
//                s += (tmp + "UL");
//                s += ", ";
//                tmp = (uint)dataBits[i] >> 32;
//                tmp &= 0xFFFFFFFFL;
//                s += (tmp + "UL");
//            }
//            return s;
//        }

//        /*
//* Dump a comma-separated list of the words making up the bit set.
//* This generates a comma-separated list of Java-like long int constants.
//*/
//        public virtual string ToStringOfWords() {
//            string s = new string("".ToCharArray());
//            for (int i = 0; i < dataBits.Length; i++) {
//                if (i != 0)
//                    s += ", ";
//                s += (dataBits[i] + "L");
//            }
//            return s;
//        }

//        private static int wordNumber(int bit) {
//            return bit >> LOG_BITS; // bit / BITS
//        }
//    }



    //public class BitSet
    //{
    //    private static readonly ulong[] EmptyBits = new ulong[0];
    //    private const int BitsPerElement = 8 * sizeof(ulong);


    //    private ulong[] _data = EmptyBits;


    //    public BitSet() {
    //    }


    //    public BitSet(int nbits) {
    //        if (nbits < 0)
    //            throw new ArgumentOutOfRangeException("nbits");


    //        if (nbits > 0) {
    //            int length = (nbits + BitsPerElement - 1) / BitsPerElement;
    //            _data = new ulong[length];
    //        }
    //    }


    //    private static int GetBitCount(ulong[] value) {
    //        int data = 0;
    //        uint size = (uint)value.Length;
    //        const ulong m1 = 0x5555555555555555;
    //        const ulong m2 = 0x3333333333333333;
    //        const ulong m4 = 0x0F0F0F0F0F0F0F0F;
    //        const ulong m8 = 0x00FF00FF00FF00FF;
    //        const ulong m16 = 0x0000FFFF0000FFFF;
    //        const ulong h01 = 0x0101010101010101;


    //        uint bitCount = 0;
    //        uint limit30 = size - size % 30;


    //        // 64-bit tree merging (merging3)
    //        for (uint i = 0; i < limit30; i += 30, data += 30) {
    //            ulong acc = 0;
    //            for (uint j = 0; j < 30; j += 3) {
    //                ulong count1 = value[data + j];
    //                ulong count2 = value[data + j + 1];
    //                ulong half1 = value[data + j + 2];
    //                ulong half2 = half1;
    //                half1 &= m1;
    //                half2 = (half2 >> 1) & m1;
    //                count1 -= (count1 >> 1) & m1;
    //                count2 -= (count2 >> 1) & m1;
    //                count1 += half1;
    //                count2 += half2;
    //                count1 = (count1 & m2) + ((count1 >> 2) & m2);
    //                count1 += (count2 & m2) + ((count2 >> 2) & m2);
    //                acc += (count1 & m4) + ((count1 >> 4) & m4);
    //            }


    //            acc = (acc & m8) + ((acc >> 8) & m8);
    //            acc = (acc + (acc >> 16)) & m16;
    //            acc = acc + (acc >> 32);
    //            bitCount += (uint)acc;
    //        }


    //        // count the bits of the remaining bytes (MAX 29*8) using 
    //        // "Counting bits set, in parallel" from the "Bit Twiddling Hacks",
    //        // the code uses wikipedia's 64-bit popcount_3() implementation:
    //        // http://en.wikipedia.org/wiki/Hamming_weight#Efficient_implementation
    //        for (uint i = 0; i < size - limit30; i++) {
    //            ulong x = value[data + i];
    //            x = x - ((x >> 1) & m1);
    //            x = (x & m2) + ((x >> 2) & m2);
    //            x = (x + (x >> 4)) & m4;
    //            bitCount += (uint)((x * h01) >> 56);
    //        }


    //        return (int)bitCount;
    //    }


    //    private static readonly int[] index64 =
    //    {
    //        0, 47,  1, 56, 48, 27,  2, 60,
    //       57, 49, 41, 37, 28, 16,  3, 61,
    //       54, 58, 35, 52, 50, 42, 21, 44,
    //       38, 32, 29, 23, 17, 11,  4, 62,
    //       46, 55, 26, 59, 40, 36, 15, 53,
    //       34, 51, 20, 43, 31, 22, 10, 45,
    //       25, 39, 14, 33, 19, 30,  9, 24,
    //       13, 18,  8, 12,  7,  6,  5, 63
    //    };


    //    private static int BitScanForward(ulong value) {
    //        if (value == 0)
    //            return -1;


    //        const ulong debruijn64 = 0x03f79d71b4cb0a89;
    //        return index64[((value ^ (value - 1)) * debruijn64) >> 58];
    //    }


    //    public BitSet Clone() {
    //        BitSet result = new BitSet();
    //        result._data = (ulong[])_data.Clone();
    //        return result;
    //    }


    //    public void Clear(int index) {
    //        if (index < 0)
    //            throw new ArgumentOutOfRangeException("index");


    //        int element = index / BitsPerElement;
    //        if (element >= _data.Length)
    //            return;


    //        _data[element] &= ~(1UL << (index % BitsPerElement));
    //    }


    //    public bool Get(int index) {
    //        if (index < 0)
    //            throw new ArgumentOutOfRangeException("index");


    //        int element = index / BitsPerElement;
    //        if (element >= _data.Length)
    //            return false;


    //        return (_data[element] & (1UL << (index % BitsPerElement))) != 0;
    //    }


    //    public void Set(int index) {
    //        if (index < 0)
    //            throw new ArgumentOutOfRangeException("index");


    //        int element = index / BitsPerElement;
    //        if (element >= _data.Length)
    //            Array.Resize(ref _data, Math.Max(_data.Length * 2, element + 1));


    //        _data[element] |= 1UL << (index % BitsPerElement);
    //    }


    //    public bool IsEmpty() {
    //        for (int i = 0; i < _data.Length; i++) {
    //            if (_data[i] != 0)
    //                return false;
    //        }


    //        return true;
    //    }


    //    public int Cardinality() {
    //        return GetBitCount(_data);
    //    }


    //    public int NextSetBit(int fromIndex) {
    //        if (fromIndex < 0)
    //            throw new ArgumentOutOfRangeException("fromIndex");


    //        if (IsEmpty())
    //            return -1;


    //        int i = fromIndex / BitsPerElement;
    //        if (i >= _data.Length)
    //            return -1;


    //        ulong current = _data[i] & ~((1UL << (fromIndex % BitsPerElement)) - 1);


    //        while (true) {
    //            int bit = BitScanForward(current);
    //            if (bit >= 0)
    //                return bit + i * BitsPerElement;


    //            i++;
    //            if (i >= _data.Length)
    //                break;


    //            current = _data[i];
    //        }


    //        return -1;
    //    }


    //    public void And(BitSet set) {
    //        if (set == null)
    //            throw new ArgumentNullException("set");


    //        int length = Math.Min(_data.Length, set._data.Length);
    //        for (int i = 0; i < length; i++)
    //            _data[i] &= set._data[i];


    //        for (int i = length; i < _data.Length; i++)
    //            _data[i] = 0;
    //    }


    //    public void Or(BitSet set) {
    //        if (set == null)
    //            throw new ArgumentNullException("set");


    //        if (set._data.Length > _data.Length)
    //            Array.Resize(ref _data, set._data.Length);


    //        for (int i = 0; i < set._data.Length; i++)
    //            _data[i] |= set._data[i];
    //    }


    //    public override bool Equals(object obj) {
    //        BitSet other = obj as BitSet;
    //        if (other == null)
    //            return false;


    //        if (IsEmpty())
    //            return other.IsEmpty();


    //        int minLength = Math.Min(_data.Length, other._data.Length);
    //        for (int i = 0; i < minLength; i++) {
    //            if (_data[i] != other._data[i])
    //                return false;
    //        }


    //        for (int i = minLength; i < _data.Length; i++) {
    //            if (_data[i] != 0)
    //                return false;
    //        }


    //        for (int i = minLength; i < other._data.Length; i++) {
    //            if (other._data[i] != 0)
    //                return false;
    //        }


    //        return true;
    //    }


    //    public override int GetHashCode() {
    //        ulong result = 1;
    //        for (uint i = 0; i < _data.Length; i++) {
    //            if (_data[i] != 0) {
    //                result = result * 31 ^ i;
    //                result = result * 31 ^ _data[i];
    //            }
    //        }


    //        return result.GetHashCode();
    //    }


    //    public override string ToString() {
    //        StringBuilder builder = new StringBuilder();
    //        builder.Append('{');


    //        for (int i = NextSetBit(0); i >= 0; i = NextSetBit(i + 1)) {
    //            if (builder.Length > 1)
    //                builder.Append(", ");


    //            builder.Append(i);
    //        }


    //        builder.Append('}');
    //        return builder.ToString();
    //    }
    //}
}
