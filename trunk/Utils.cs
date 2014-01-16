/*
-----------------------------------------------------------------------------
This source file is part of mogre-procedural
For the latest info, see http://code.google.com/p/mogre-procedural/
my blog:http://hi.baidu.com/rainssoft
this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/
   
Copyright (c) 2013-2020 rains soft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    /// Holds a bunch of static utility functions
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Utils
    public partial class Utils
    {
        private static int counter = 0;
        /// Outputs something to the ogre log, with a [PROCEDURAL] prefix
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	static void log(Ogre::String st);


        public static void log(string st) {
            LogManager.Singleton.LogMessage("[PROCEDURAL] " + st);
        }
        /// <summary>
        /// 前缀名+唯一递增数
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static string getName(string prefix) {
            counter++;
            return prefix + (counter).ToString();
        }

        //-----------------------------------------------------------------------
        public static Quaternion _computeQuaternion(Vector3 directionr) {
            return _computeQuaternion(directionr, Vector3.UNIT_Y);
        }
        public static Quaternion _computeQuaternion(Vector3 direction, Vector3 upVector) {
            Quaternion q = new Quaternion();
            Vector3 zVec = direction;
            zVec = zVec.NormalisedCopy;
            Vector3 xVec = upVector.CrossProduct(zVec);
            if (xVec.IsZeroLength)
                xVec = Vector3.UNIT_X;
            xVec = xVec.NormalisedCopy;
            Vector3 yVec = zVec.CrossProduct(xVec);
            yVec = yVec.NormalisedCopy;
            q.FromAxes(xVec, yVec, zVec);
            return q;
        }
        ////////////////////////////////////////
        /// Gets the min of the coordinates between 2 vectors
        public static Vector3 min(Vector3 v1, Vector3 v2) {
            return new Vector3(System.Math.Min(v1.x, v2.x), System.Math.Min(v1.y, v2.y), System.Math.Min(v1.z, v2.z));
        }

        /// Gets the max of the coordinates between 2 vectors
        public static Vector3 max(Vector3 v1, Vector3 v2) {
            return new Vector3(System.Math.Max(v1.x, v2.x), System.Math.Max(v1.y, v2.y), System.Math.Max(v1.z, v2.z));
        }

        /// Gets the min of the coordinates between 2 vectors
        public static Vector2 min(Vector2 v1, Vector2 v2) {
            return new Vector2(System.Math.Min(v1.x, v2.x), System.Math.Min(v1.y, v2.y));
        }

        /// Gets the max of the coordinates between 2 vectors
        public static Vector2 max(Vector2 v1, Vector2 v2) {
            return new Vector2(System.Math.Max(v1.x, v2.x), System.Math.Max(v1.y, v2.y));
        }

        /// Builds an AABB from a list of points
        public static AxisAlignedBox AABBfromPoints(List<Vector3> points) {
            AxisAlignedBox aabb = new AxisAlignedBox();
            if (points.Count == 0)
                return aabb;
            aabb.SetMinimum(points[0]);
            aabb.SetMaximum(points[0]);
            //for (List< Vector3>.Enumerator it = points.GetEnumerator(); it.MoveNext(); ++it.)
            foreach (var it in points) {
                aabb.SetMinimum(min(aabb.Minimum, it));
                aabb.SetMaximum(max(aabb.Maximum, it));
            }

            return aabb;
        }

        /// Generate a name from a prefix and a counter
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	static string getName(string prefix);

        /// Shifts the components of the vector to the right
        public static Vector3 vectorPermute(Vector3 @in) {
            return new Vector3(@in.z, @in.x, @in.y);
        }

        /// Shifts the components of the vector to the left
        public static Vector3 vectorAntiPermute(Vector3 @in) {
            return new Vector3(@in.y, @in.z, @in.x);
        }

        // Rotates a Vector2 by a given oriented angle
        public static Vector2 rotateVector2(Vector2 @in, Radian angleR) {
            float angle = angleR.ValueRadians;
            return new Vector2(@in.x * (float)Math.Cos(angle) - @in.y * (float)Math.Sin(angle), @in.x * (float)Math.Sin(angle) + @in.y * (float)Math.Cos(angle));
        }

        /// Caps n between min and max
        public static int cap(int n, int min, int max) {
            return System.Math.Max(System.Math.Min(n, max), min);
        }

        //    *
        //	 * An extend version of the standard modulo, in that int values are "wrapped"
        //	 * in both directions, whereas with standard modulo, (-1)%2 == -1
        //	 * Always return an int between 0 and cap-1
        //	 
        public static int modulo(int n, int cap) {
            if (n >= 0)
                return n % cap;
            return (cap - 1) - ((1 + n) % cap);
        }

        //    *
        //	 * Equivalent of Ogre::Vector3::angleBetween, applied to Ogre::Vector2
        //	 
        public static Radian angleBetween(Vector2 v1, Vector2 v2) {
            float lenProduct = v1.Length * v2.Length;
            // Divide by zero check
            if (lenProduct < 1e-6f)
                lenProduct = 1e-6f;

            float f = v1.DotProduct(v2) / lenProduct;

            f = Math_Clamp(f, -1.0f, 1.0f);
            return Math.ACos(f);
        }
        public static Radian angleBetween(Vector3 v1, Vector3 dest) {
            float lenProduct = v1.Length * dest.Length;

            // Divide by zero check
            if (lenProduct < 1e-6f)
                lenProduct = 1e-6f;

            float f = v1.DotProduct(dest) / lenProduct;

            f = Clamp(f, (float)-1.0f, 1.0f);
            return Math.ACos(f);
        }
        /// <summary>
        /// 取中间值
        /// </summary>
        /// <param name="val"></param>
        /// <param name="minval"></param>
        /// <param name="maxval"></param>
        /// <returns></returns>
        public static float Clamp(float val, float minval, float maxval) {
            //assert (minval <= maxval && "Invalid clamp range");
            return (float)System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        public static int Clamp(int val, int minval, int maxval) {
            //assert (minval <= maxval && "Invalid clamp range");
            return (int)System.Math.Max(System.Math.Min(val, maxval), minval);
        }
        /// <summary>
        /// 返回3个值得中间值
        /// </summary>
        /// <param name="f"></param>
        /// <param name="p"></param>
        /// <param name="p_3"></param>
        /// <returns></returns>
        private static float Math_Clamp(float value, float max, float min) {
            var result = value;
            if (value.CompareTo(max) > 0) {
                result = max;
            }
            if (value.CompareTo(min) < 0) {
                result = min;
            }
            return result;

        }

        //    *
        //	 * Gives the oriented angle from v1 to v2 in the [0;2PI[ range
        //	 
        public static Radian angleTo(Vector2 v1, Vector2 v2) {
            Radian angle = angleBetween(v1, v2);

            if (v1.CrossProduct(v2) < 0f)
                angle = (Radian)Math.TWO_PI - angle;

            return angle;
        }
 
        //    *
        //	 * Gives the oriented angle from v1 to v2 in the ]-PI;PI] range
        //	 
        public static Radian signedAngleTo(Vector2 v1, Vector2 v2) {
            Radian angle = angleBetween(v1, v2);

            if (v1.CrossProduct(v2) < 0f)
                angle = -angle;

            return angle;
        }

        //    *
        //	 * Computes a quaternion between UNIT_Z and direction.
        //	 * It keeps the "up" vector to UNIT_Y
        //	 
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	static Ogre::Quaternion _computeQuaternion(Ogre::Vector3 direction, Ogre::Vector3 upVector);

        //    *
        //	 * Maps a vector2 to vector3, with Y=0
        //	 
        public static Vector3 vec2ToVec3Y(Vector2 pos) {
            return new Vector3(pos.x, 0, pos.y);
        }

        //    *
        //	 * binomial coefficients (a over b)
        //	 
        public static uint binom(uint a, uint b) {
            uint tmpA;
            uint tmpB;
            if ((b == 0) || (a == b))
                return 1;
            else {
                tmpA = binom(a - 1, b);
                tmpB = binom(a - 1, b - 1);
                return tmpA + tmpB;
            }
        }

        /// Transforms an input vector expressed in the 0,0->1,1 rect towards another rect
        public static Vector2 reframe(RealRect rect, Vector2 input) {
            return new Vector2(rect.left + input.x * rect.Width, rect.top + input.y * rect.Height);
        }

        
        public static void ThrowException(Exception exception) {
            if (exception != null) { throw exception; }
        }
     
        //public static object Clone(object obj, bool deepClone) {
        //    if (deepClone && obj is IDeeplyCloneable) {
        //        return ((IDeeplyCloneable)obj).DeepClone();
        //    }
        //    else if (obj is ICloneable) {
        //        return ((ICloneable)obj).Clone();
        //    }
        //    else {
        //        return obj;
        //    }
        //}


        //public static bool ObjectEquals(object obj1, object obj2, bool deepCmp) {
        //    if (obj1 == null && obj2 == null) { return true; }
        //    else if (obj1 == null || obj2 == null) { return false; }
        //    else if (!obj1.GetType().Equals(obj2.GetType())) { return false; }
        //    else if (deepCmp && obj1 is IContentEquatable) {
        //        return ((IContentEquatable)obj1).ContentEquals(obj2);
        //    }
        //    else {
        //        return obj1.Equals(obj2);
        //    }
        //}

        public static object ChangeType(object obj, Type newType, IFormatProvider fmtProvider) {
            ThrowException(newType == null ? new ArgumentNullException("newType") : null);
            if (newType.IsAssignableFrom(obj.GetType())) {
                return obj;
            }
            else {
                return Convert.ChangeType(obj, newType, fmtProvider); // throws InvalidCastException, FormatException, OverflowException
            }
        }



        //internal static int GetHashCode(object obj) {
        //    throw new NotImplementedException();
        //}
    }
}