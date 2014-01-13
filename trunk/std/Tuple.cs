using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Procedural.std
{
    /// <summary>
    ///	Represents two related values
    /// </summary>
    public class Tuple<A, B> : IEquatable<Tuple<A, B>>
    {

        #region Fields and Properties

        /// <summary></summary>
        public readonly A First;

        /// <summary></summary>
        public readonly B Second;



        #endregion Fields and Properties



        #region Construction and Destruction


        public Tuple(A first, B second) {
            this.First = first;
            this.Second = second;
        }



        #endregion Construction and Destruction



        #region IEquatable<Tuple<A,B>> Implementation



        public bool Equals(Tuple<A, B> other) {
            return this.First.Equals(other.First) && this.Second.Equals(other.Second);
        }

        public override bool Equals(object other) {
            if (other is Tuple<A, B>) {
                return Equals((Tuple<A, B>)other);
            }
            return false;
        }

        #endregion IEquatable<Tuple<A,B>> Implementation
    }



    /// <summary>
    /// Represents three related values
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <typeparam name="C"></typeparam>
    public struct Tuple<A, B, C> : IEquatable<Tuple<A, B, C>>
    {

        #region Fields and Properties

        /// <summary></summary>
        public readonly A First;
        /// <summary></summary>
        public readonly B Second;
        /// <summary></summary>
        public readonly C Third;
        #endregion Fields and Properties


        #region Construction and Destruction

        public Tuple(A first, B second, C Third) {
            this.First = first;
            this.Second = second;
            this.Third = Third;
        }

        #endregion Construction and Destruction

        #region IEquatable<Tuple<A,B,C>> Implementation

        public bool Equals(Tuple<A, B, C> other) {
            return this.First.Equals(other.First) && this.Second.Equals(other.Second) && this.Third.Equals(other.Third);
        }

        public override bool Equals(object other) {
            if (other is Tuple<A, B, C>) {
                return Equals((Tuple<A, B, C>)other);
            }
            return false;
        }

        #endregion IEquatable<Tuple<A,B,C>> Implementation
    }



    /// <summary>
    /// Represents four related values
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="D"></typeparam>
    public struct Tuple<A, B, C, D> : IEquatable<Tuple<A, B, C, D>>
    {

        #region Fields and Properties
        /// <summary></summary>
        public readonly A First;
        /// <summary></summary>
        public readonly B Second;
        /// <summary></summary>
        public readonly C Third;
        /// <summary></summary>
        public readonly D Fourth;
        
        #endregion Fields and Properties
        
        #region Construction and Destruction
        
        public Tuple(A first, B second, C third, D fourth) {
            this.First = first;
            this.Second = second;
            this.Third = third;
            this.Fourth = fourth;
        }
        
        #endregion Construction and Destruction
        
        #region IEquatable<Tuple<A,B,C,D>> Implementation
        
        public bool Equals(Tuple<A, B, C, D> other) {
            return this.First.Equals(other.First) && this.Second.Equals(other.Second) && this.Third.Equals(other.Third);
        }
        
        public override bool Equals(object other) {
            if (other is Tuple<A, B, C, D>) {
                return Equals((Tuple<A, B, C, D>)other);
            }
            return false;
        }
        
        #endregion IEquatable<Tuple<A,B,C,D>> Implementation

    }


    //public class Tupple<TA, TB>
    //{
    //    private TA a;
    //    private TB b;

    //    public Tupple(TA a, TB b) {
    //        this.a = a;
    //        this.b = b;
    //    }

    //    public TA A {
    //        get {
    //            return a;
    //        }
    //    }

    //    public TB B {
    //        get {
    //            return b;
    //        }
    //    }

    //    public override int GetHashCode() {
    //        return a.GetHashCode() ^ b.GetHashCode();
    //    }

    //    public bool Equals(Tupple<TA, TB> other) {
    //        return a.Equals(other.a) && b.Equals(other.b);
    //    }

    //    public override bool Equals(object other) {
    //        if (other is Tupple<TA, TB>)
    //            return Equals((Tupple<TA, TB>)other);
    //        else
    //            return false;
    //    }
    //}

    //public class Tupple3<TA, TB, TC>
    //{
    //    private TA a;
    //    private TB b;
    //    private TC c;

    //    public Tupple3(TA a, TB b, TC c) {
    //        this.a = a;
    //        this.b = b;
    //        this.c = c;
    //    }

    //    public TA A {
    //        get {
    //            return a;
    //        }
    //    }

    //    public TB B {
    //        get {
    //            return b;
    //        }
    //    }

    //    public TC C {
    //        get {
    //            return c;
    //        }
    //    }

    //    public override int GetHashCode() {
    //        return a.GetHashCode() ^ b.GetHashCode() ^ c.GetHashCode();
    //    }

    //    public bool Equals(Tupple3<TA, TB, TC> other) {
    //        return a.Equals(other.a) && b.Equals(other.b) && c.Equals(other.c);
    //    }

    //    public override bool Equals(object other) {
    //        if (other is Tupple3<TA, TB, TC>)
    //            return Equals((Tupple3<TA, TB, TC>)other);
    //        else
    //            return false;
    //    }
    //}

    //public class ListTupple
    //{
    //    private object[] items;

    //    public ListTupple(params object[] items) {
    //        this.items = items;
    //    }

    //    public object this[int index] {
    //        get {
    //            return items[index];
    //        }
    //    }

    //    public override int GetHashCode() {
    //        int hash = 0;

    //        foreach (object item in items)
    //            hash ^= item.GetHashCode();

    //        return hash;
    //    }

    //    public bool Equals(ListTupple other) {
    //        if (items.Length != other.items.Length)
    //            return false;

    //        for (int n = 0; n < items.Length; n++) {
    //            if (items[n] != other.items[n])
    //                return false;
    //        }

    //        return true;
    //    }

    //    public override bool Equals(object other) {
    //        if (other is ListTupple)
    //            return Equals((ListTupple)other);
    //        else
    //            return false;
    //    }
    //}

}
