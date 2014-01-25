using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Mogre_Procedural.std
{

    /// <summary>
    /// 	A simple container class for returning a pair of objects from a method call
    /// 	(similar to std::pair).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class std_pair<T> : IEquatable<std_pair<T>>
    {
        private Tuple<T, T> data;
        public T first {
                        get {
                return this.data.First;
            }
            set {
                this.data = new  Tuple<T, T>(value, this.data.Second);
            }
        }
        public T second {
            get {
                return this.data.Second;
            }
            set {
                this.data = new  Tuple<T, T>(this.data.First, value);
            }

        }



        public std_pair(T first, T second) {

            this.data = new  Tuple<T, T>(first, second);

        }
        public std_pair(KeyValuePair<T, T> kp) {

            this.data = new Tuple<T, T>(kp.Key, kp.Value);

        }
        #region IEquatable<Pair<T>> Implementation

        public bool Equals(std_pair<T> other) {

            return this.data.Equals(other.data);

        }

        public override bool Equals(object other) {
            if (other is std_pair<T>) {
                return Equals((std_pair<T>)other);

            }
            return false;

        }



        #endregion IEquatable<Pair<T>> Implementation



        #region System.Object Implementation

        public override int GetHashCode() {

            return first.GetHashCode() ^ second.GetHashCode();

        }

        #endregion System.Object Implementation
        public static bool operator ==(std_pair<T> _this, std_pair<T> _other) {
            return _this.Equals(_other);
        }
        public static bool operator !=(std_pair<T> _this, std_pair<T> _other) {
            return !_this.Equals(_other);
        }
    }
    public class std_pair<T,V> : IEquatable<std_pair<T,V>>
    {
        private Tuple<T, V> data;
        public T first {
            get {
                return this.data.First;
            }
            set {
                this.data = new Tuple<T, V>(value, this.data.Second);
            }
        }
        public V second {
            get {
                return this.data.Second;
            }
            set {
                this.data = new Tuple<T, V>(this.data.First, value);
            }

        }



        public std_pair(T first, V second) {

            this.data = new Tuple<T, V>(first, second);

        }
        public std_pair(KeyValuePair<T,V> kp) {

            this.data = new Tuple<T, V>(kp.Key,kp.Value);

        }
        #region IEquatable<Pair<T>> Implementation

        public bool Equals(std_pair<T,V> other) {

            return this.data.Equals(other.data);

        }

        public override bool Equals(object other) {
            if (other is std_pair<T,V>) {
                return Equals((std_pair<T,V>)other);

            }
            return false;

        }
       


        #endregion IEquatable<Pair<T>> Implementation



        #region System.Object Implementation

        public override int GetHashCode() {

            return first.GetHashCode() ^ second.GetHashCode();

        }

        #endregion System.Object Implementation
        public static bool operator ==(std_pair<T, V> _this, std_pair<T, V> _other) {
            return _this.Equals(_other);
        }
        public static bool operator !=(std_pair<T, V> _this, std_pair<T, V> _other) {
            return !_this.Equals(_other);
        }
    }
    /// <summary>
	/// 	A simple container class for returning a pair of objects from a method call 
	/// 	(similar to std::pair, minus the templates).
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class std_pair 
	{
		public object first;
		public object second;
		public std_pair( object first, object second )
		{
			this.first = first;
			this.second = second;
		}
	}


    //public interface IPair<FirstT, SecondT>
    //{
    //    FirstT First { get; }
    //    SecondT Second { get; }
    //}

    //public struct Pair<FirstT, SecondT> : IPair<FirstT, SecondT>, IDeeplyCloneable<Pair<FirstT, SecondT>>, IContentEquatable<Pair<FirstT, SecondT>>,
    //   IComparable<Pair<FirstT, SecondT>>, IComparable, ISerializable
    //{
    //    private FirstT mFirst;
    //    private SecondT mSecond;


    //    public Pair(BinarySerializer reader) {
    //        mFirst = default(FirstT);
    //        mSecond = default(SecondT);
    //        Load(reader); // throws ArgumentNullException, serialization-related exceptions
    //    }


    //    public Pair(FirstT first, SecondT second) {
    //        mFirst = first;
    //        mSecond = second;
    //    }


    //    public FirstT First {
    //        get { return mFirst; }
    //        set { mFirst = value; }
    //    }


    //    public SecondT Second {
    //        get { return mSecond; }
    //        set { mSecond = value; }
    //    }


    //    public override string ToString() {
    //        return string.Format("( {0} {1} )", mFirst, mSecond);
    //    }


    //    public override bool Equals(object obj) {
    //        return base.Equals(obj);
    //    }


    //    public override int GetHashCode() {
    //        return base.GetHashCode();
    //    }


    //    public static bool operator ==(Pair<FirstT, SecondT> first, Pair<FirstT, SecondT> second) {
    //        return first.Equals(second);
    //    }


    //    public static bool operator !=(Pair<FirstT, SecondT> first, Pair<FirstT, SecondT> second) {
    //        return !first.Equals(second);
    //    }


    //    // *** IDeeplyCloneable interface implementation ***


    //    public Pair<FirstT, SecondT> DeepClone() {
    //        return new Pair<FirstT, SecondT>((FirstT)Utils.Clone(mFirst, /*deepClone=*/true), (SecondT)Utils.Clone(mSecond, /*deepClone=*/true));
    //    }


    //    object IDeeplyCloneable.DeepClone() {
    //        return DeepClone();
    //    }


    //    // *** IContentEquatable<Pair<FirstT, SecondT>> interface implementation ***


    //    public bool ContentEquals(Pair<FirstT, SecondT> other) {
    //        return Utils.ObjectEquals(mFirst, other.mFirst, /*deepCmp=*/true) && Utils.ObjectEquals(mSecond, other.mSecond, /*deepCmp=*/true);
    //    }


    //    bool IContentEquatable.ContentEquals(object other) {
    //        Utils.ThrowException(other == null ? new ArgumentNullException("other") : null);
    //        Utils.ThrowException(!(other is Pair<FirstT, SecondT>) ? new ArgumentTypeException("other") : null);
    //        return ContentEquals((Pair<FirstT, SecondT>)other);
    //    }


    //    // *** IComparable<Pair<FirstT, SecondT>> interface implementation ***


    //    public int CompareTo(Pair<FirstT, SecondT> other) {
    //        if (mFirst == null && other.mFirst == null) { return 0; }
    //        else if (mFirst == null) { return 1; }
    //        else if (other.mFirst == null) { return -1; }
    //        else {
    //            int val = ((IComparable<FirstT>)mFirst).CompareTo(other.mFirst); // throws InvalidCastException
    //            if (val != 0) { return val; }
    //            else if (mSecond == null && other.mSecond == null) { return 0; }
    //            else if (mSecond == null) { return 1; }
    //            else if (other.mSecond == null) { return -1; }
    //            else { return ((IComparable<SecondT>)mSecond).CompareTo(other.mSecond); } // throws InvalidCastException
    //        }
    //    }


    //    // *** IComparable interface implementation ***


    //    int IComparable.CompareTo(object obj) {
    //        Utils.ThrowException(!(obj == null || obj is Pair<FirstT, SecondT>) ? new ArgumentTypeException("obj") : null);
    //        return CompareTo((Pair<FirstT, SecondT>)obj);
    //    }


    //    // *** ISerializable interface implementation ***


    //    public void Save(BinarySerializer writer) {
    //        Utils.ThrowException(writer == null ? new ArgumentNullException("writer") : null);
    //        // the following statements throw serialization-related exceptions
    //        writer.WriteValueOrObject<FirstT>(mFirst);
    //        writer.WriteValueOrObject<SecondT>(mSecond);
    //    }


    //    public void Load(BinarySerializer reader) {
    //        Utils.ThrowException(reader == null ? new ArgumentNullException("reader") : null);
    //        // the following statements throw serialization-related exceptions
    //        mFirst = reader.ReadValueOrObject<FirstT>();
    //        mSecond = reader.ReadValueOrObject<SecondT>();
    //    }


    //    // *** Equality comparer ***


    //    public static IEqualityComparer<Pair<FirstT, SecondT>> GetEqualityComparer() {
    //        return GenericEqualityComparer<Pair<FirstT, SecondT>>.Instance;
    //    }
    //}



    //public class GenericEqualityComparer<T> : IEqualityComparer<T>, IEqualityComparer where T : ISerializable
    //{
    //    private static GenericEqualityComparer<T> mInstance
    //        = new GenericEqualityComparer<T>();


    //    public static GenericEqualityComparer<T> Instance {
    //        get { return mInstance; }
    //    }


    //    public bool Equals(T x, T y) {
    //        return Utils.ObjectEquals(x, y, /*deepCmp=*/true);
    //    }


    //    public int GetHashCode(T obj) {
    //        return Utils.GetHashCode(obj); // throws ArgumentNullException
    //    }


    //    bool IEqualityComparer.Equals(object x, object y) {
    //        Utils.ThrowException((x != null && !(x is T)) ? new ArgumentException("x") : null);
    //        Utils.ThrowException((y != null && !(y is T)) ? new ArgumentException("y") : null);
    //        return Equals((T)x, (T)y);
    //    }


    //    int IEqualityComparer.GetHashCode(object obj) {
    //        Utils.ThrowException((obj != null && !(obj is T)) ? new ArgumentException("obj") : null);
    //        return GetHashCode((T)obj); // throws ArgumentNullException
    //    }
    //}

}
