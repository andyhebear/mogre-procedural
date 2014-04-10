

//namespace Game.Utilitys.SdkTrayUI
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Text;

//    public class SdkTrayMathHelper {
//        public const double PI = Math.PI;

//        public const double SQUARED_PI = PI * PI;

//        public const double HALF_PI = 0.5 * PI;

//        public const double QUARTER_PI = 0.5 * HALF_PI;

//        public const double TWO_PI = 2.0 * PI;

//        public const double THREE_PI_HALVES = TWO_PI - HALF_PI;

//        public const double DEGTORAD = PI / 180.0;

//        public const double RADTODEG = 180.0 / PI;

//        public static readonly double SQRTOFTWO = Math.Sqrt(2.0);

//        public static readonly double HALF_SQRTOFTWO = 0.5 * SQRTOFTWO;

//        /**
//        * Gets the difference between two angles
//        * This value is always positive (0 - 180)
//        *
//        * @param angle1
//        * @param angle2
//        * @return the positive angle difference
//        */
//        public static float getAngleDifference(float angle1, float angle2) {
//            return Math.Abs(wrapAngle(angle1 - angle2));
//        }

//        /**
//        * Gets the difference between two radians
//        * This value is always positive (0 - PI)
//        *
//        * @param radian1
//        * @param radian2
//        * @return the positive radian difference
//        */
//        public static double getRadianDifference(double radian1, double radian2) {
//            return Math.Abs(wrapRadian(radian1 - radian2));
//        }

//        /**
//        * Wraps the angle between -180 and 180 degrees
//        *
//        * @param angle to wrap
//        * @return -180 > angle <= 180
//        */
//        public static float wrapAngle(float angle) {
//            angle %= 360f;
//            if (angle <= -180) {
//                return angle + 360;
//            }
//            else if (angle > 180) {
//                return angle - 360;
//            }
//            else {
//                return angle;
//            }
//        }

//        /**
//        * Wraps a byte between 0 and 256
//        *
//        * @param value to wrap
//        * @return 0 >= byte < 256
//        */
//        public static byte wrapByte(int value) {
//            value %= 256;
//            if (value < 0) {
//                value += 256;
//            }
//            return (byte)value;
//        }

//        /**
//        * Wraps the radian between -PI and PI
//        *
//        * @param radian to wrap
//        * @return -PI > radian <= PI
//        */
//        public static double wrapRadian(double radian) {
//            radian %= TWO_PI;
//            if (radian <= -PI) {
//                return radian + TWO_PI;
//            }
//            else if (radian > PI) {
//                return radian - TWO_PI;
//            }
//            else {
//                return radian;
//            }
//        }

//        /**
//        * Rounds a number to the amount of decimals specified
//        *
//        * @param input to round
//        * @param decimals to round to
//        * @return the rounded number
//        */
//        public static double round(double input, int decimals) {
//            double p = Math.Pow(10, decimals);
//            return Math.Round(input * p) / p;
//        }



//        /**
//        * Calculates the value at x using linear interpolation
//        *
//        * @param x the X coord of the value to interpolate
//        * @param x1 the X coord of q0
//        * @param x2 the X coord of q1
//        * @param q0 the first known value (x1)
//        * @param q1 the second known value (x2)
//        * @return the interpolated value
//        */
//        public static double lerp(double x, double x1, double x2, double q0, double q1) { return ((x2 - x) / (x2 - x1)) * q0 + ((x - x1) / (x2 - x1)) * q1; }


//        /**
//* Calculates the value at x,y,z using trilinear interpolation
//*
//* @param x the X coord of the value to interpolate
//* @param y the Y coord of the value to interpolate
//* @param z the Z coord of the value to interpolate
//* @param q000 the first known value (x1, y1, z1)
//* @param q001 the second known value (x1, y2, z1)
//* @param q010 the third known value (x1, y1, z2)
//* @param q011 the fourth known value (x1, y2, z2)
//* @param q100 the fifth known value (x2, y1, z1)
//* @param q101 the sixth known value (x2, y2, z1)
//* @param q110 the seventh known value (x2, y1, z2)
//* @param q111 the eighth known value (x2, y2, z2)
//* @param x1 the X coord of q000, q001, q010 and q011
//* @param x2 the X coord of q100, q101, q110 and q111
//* @param y1 the Y coord of q000, q010, q100 and q110
//* @param y2 the Y coord of q001, q011, q101 and q111
//* @param z1 the Z coord of q000, q001, q100 and q101
//* @param z2 the Z coord of q010, q011, q110 and q111
//* @return the interpolated value
//*/
//        public static double triLerp(double x, double y, double z, double q000, double q001,
//        double q010, double q011, double q100, double q101, double q110, double q111,
//        double x1, double x2, double y1, double y2, double z1, double z2) {
//            double q00 = lerp(x, x1, x2, q000, q100);
//            double q01 = lerp(x, x1, x2, q010, q110);
//            double q10 = lerp(x, x1, x2, q001, q101);
//            double q11 = lerp(x, x1, x2, q011, q111);
//            double q0 = lerp(y, y1, y2, q00, q10);
//            double q1 = lerp(y, y1, y2, q01, q11);
//            return lerp(z, z1, z2, q0, q1);
//        }


//        static public T clamp<T>(T val, T min, T max) where T : IComparable<T> {

//            if (val.CompareTo(min) < 0) { return min; }
//            else if (val.CompareTo(max) > 0) { return max; }
//            else { return val; }
//        }

//        public static int floor(double x) {
//            int y = (int)x;
//            if (x < y) {
//                return y - 1;
//            }
//            return y;
//        }

//        public static int floor(float x) {
//            int y = (int)x;
//            if (x < y) {
//                return y - 1;
//            }
//            return y;
//        }

//        /**
//        * Gets the maximum byte value from two values
//        *
//        * @param value1
//        * @param value2
//        * @return the maximum value
//        */
//        public static byte max(byte value1, byte value2) {
//            return value1 > value2 ? value1 : value2;
//        }

//        /**
//        * Rounds an integer up to the next power of 2.
//        *
//        * @param x
//        * @return the lowest power of 2 greater or equal to x
//        */
//        public static int roundUpPow2(int x) {
//            if (x <= 0) {
//                return 1;
//            }
//            else if (x > 0x40000000) {
//                throw new ArgumentException("Rounding " + x + " to the next highest power of two would exceed the int range");
//            }
//            else {
//                x--;
//                x |= x >> 1;
//                x |= x >> 2;
//                x |= x >> 4;
//                x |= x >> 8;
//                x |= x >> 16;
//                x++;
//                return x;
//            }
//        }

//        public static bool isInBlock(Mogre.Vector3 origin, Mogre.Vector3 p, int side) {
//            return (p.x >= origin.x && p.x < origin.x + side) && (p.y >= origin.y && p.y < origin.y + side) && (p.z >= origin.z - side && p.z < origin.z);
//        }
//    }
//}
