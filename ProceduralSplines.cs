

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


//#ifndef PROCEDURAL_SPLINES_INCLUDED
#define PROCEDURAL_SPLINES_INCLUDED
//use new std wrapper...ok

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;



    public enum CubicHermiteSplineAutoTangentMode : int
    {
        AT_NONE,
        AT_STRAIGHT,
        AT_CATMULL
    }

    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template <class T>

    //*
    // * Template control point for Cubic Hermite splines.
    // 
    public class CubicHermiteSplineControlPoint<T>
    {
        /// Position of the control point
        public T position;
        /// Tangent just before the control point
        public T tangentBefore;
        /// Tangent just after the control point
        public T tangentAfter;
        /// Auto tangent mode for the tangent just before the control point
        public CubicHermiteSplineAutoTangentMode autoTangentBefore;
        /// Auto tangent mode for the tangent just after the control point
        public CubicHermiteSplineAutoTangentMode autoTangentAfter;

        /// Default constructor
        public CubicHermiteSplineControlPoint() {
        }

        /// Constructor with arguments
        public CubicHermiteSplineControlPoint(T p, T before, T after) {
            position = p;
            tangentBefore = before;
            tangentAfter = after;
            autoTangentBefore = CubicHermiteSplineAutoTangentMode.AT_NONE;
            autoTangentAfter = CubicHermiteSplineAutoTangentMode.AT_NONE;
        }
    }



    //namespace Procedural
    //{


    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template <class T>

    //*
    // * Template control point for KochanekBartels's splines
    // 
    public class KochanekBartelsSplineControlPoint<T>
    {
        /// Position of the control point
        public T position;

        /// Tension of the control point
        public float tension = 0f;

        /// Bias of the control point
        public float bias = 0f;

        /// Continuity of the control point
        public float continuity = 0f;

        /// Constructor with arguments
        public KochanekBartelsSplineControlPoint(T p, float t, float b, float c) {
            position = p;
            tension = t;
            bias = b;
            continuity = c;
        }
        /// Constructor with tension=bias=continuity=0
        public KochanekBartelsSplineControlPoint(T p) {
            position = p;
            tension = 0f;
            bias = 0f;
            continuity = 0f;
        }
    }
    //public class KochanekBartelsSplineControlPoint_vector2 : KochanekBartelsSplineControlPoint<Vector2> { 

    //}
    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template<class T>



    public static partial class GlobalMembers
    {

        //public static void computeTangents<T>(ref CubicHermiteSplineControlPoint<T> point, T pointBefore, T pointAfter){
        //    switch (point.autoTangentBefore) {
        //        case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
        //            point.tangentBefore = point.position - pointBefore;
        //            break;
        //        case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
        //            point.tangentBefore = pointAfter - pointBefore;
        //            break;
        //        case CubicHermiteSplineAutoTangentMode.AT_NONE:
        //            break;
        //    }

        //    switch (point.autoTangentAfter) {
        //        case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
        //            point.tangentAfter = pointAfter - point.position;
        //            break;
        //        case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
        //            point.tangentAfter = pointAfter - pointBefore;
        //            break;
        //        case CubicHermiteSplineAutoTangentMode.AT_NONE:
        //            break;
        //    }
        //}
        public static void computeTangents(ref CubicHermiteSplineControlPoint<Real> point, Real pointBefore, Real pointAfter) {
            switch (point.autoTangentBefore) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentBefore = point.position - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentBefore = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }

            switch (point.autoTangentAfter) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentAfter = pointAfter - point.position;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentAfter = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }
        }
        public static void computeTangents(ref CubicHermiteSplineControlPoint<Vector2> point, Vector2 pointBefore, Vector2 pointAfter) {
            switch (point.autoTangentBefore) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentBefore = point.position - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentBefore = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }

            switch (point.autoTangentAfter) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentAfter = pointAfter - point.position;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentAfter = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }
        }
        public static void computeTangents(ref CubicHermiteSplineControlPoint<Vector3> point, Vector3 pointBefore, Vector3 pointAfter) {
            switch (point.autoTangentBefore) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentBefore = point.position - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentBefore = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }

            switch (point.autoTangentAfter) {
                case CubicHermiteSplineAutoTangentMode.AT_STRAIGHT:
                    point.tangentAfter = pointAfter - point.position;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_CATMULL:
                    point.tangentAfter = pointAfter - pointBefore;
                    break;
                case CubicHermiteSplineAutoTangentMode.AT_NONE:
                    break;
            }
        }
        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template<class T>

        // Computes the Cubic Hermite interpolation between 2 control points
        // Warning : does not compute auto-tangents, as AUTOTANGENT_CATMULL depend on context
        // Auto-Tangents should be computed before that call
        //public static void computeCubicHermitePoints<T>(CubicHermiteSplineControlPoint<T> pointBefore, CubicHermiteSplineControlPoint<T> pointAfter, uint numSeg, ref List<T> pointList) {
        //    T p0 = pointBefore.position;
        //    T m0 = pointBefore.tangentAfter;
        //    T p1 = pointAfter.position;
        //    T m1 = pointAfter.tangentBefore;

        //    for (uint j = 0; j < numSeg; ++j) {
        //        float t = (float)j / (float)numSeg;
        //        float t2 = t * t;
        //        float t3 = t2 * t;
        //        T P = (2 * t3 - 3 * t2 + 1) * p0 + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * p1 + (t3 - t2) * m1;
        //        pointList.Add(P);
        //    }
        //}
        public static void computeCubicHermitePoints(CubicHermiteSplineControlPoint<Vector2> pointBefore, CubicHermiteSplineControlPoint<Vector2> pointAfter, uint numSeg, ref std_vector<Vector2> pointList) {
            Vector2 p0 = pointBefore.position;
            Vector2 m0 = pointBefore.tangentAfter;
            Vector2 p1 = pointAfter.position;
            Vector2 m1 = pointAfter.tangentBefore;

            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t2 * t;
                Vector2 P = (2 * t3 - 3 * t2 + 1) * p0 + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * p1 + (t3 - t2) * m1;
                pointList.Add(P);
            }
        }
     
        public static void computeCubicHermitePoints(CubicHermiteSplineControlPoint<Vector3> pointBefore, CubicHermiteSplineControlPoint<Vector3> pointAfter, uint numSeg, ref std_vector<Vector3> pointList) {
            Vector3 p0 = pointBefore.position;
            Vector3 m0 = pointBefore.tangentAfter;
            Vector3 p1 = pointAfter.position;
            Vector3 m1 = pointAfter.tangentBefore;

            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t2 * t;
                Vector3 P = (2 * t3 - 3 * t2 + 1) * p0 + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * p1 + (t3 - t2) * m1;
                pointList.Add(P);
            }
        }
        public static void computeCubicHermitePoints(CubicHermiteSplineControlPoint<Real> pointBefore, CubicHermiteSplineControlPoint<Real> pointAfter, uint numSeg, ref std_vector<Real> pointList) {
            Real p0 = pointBefore.position;
            Real m0 = pointBefore.tangentAfter;
            Real p1 = pointAfter.position;
            Real m1 = pointAfter.tangentBefore;

            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t2 * t;
                Real P = (2 * t3 - 3 * t2 + 1) * p0 + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * p1 + (t3 - t2) * m1;
                pointList.Add(P);
            }
        }
        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template<class T>

        //public static void computeCatmullRomPoints<T>(T P1, T P2, T P3, T P4, uint numSeg, ref List<T> pointList) {
        //    for (uint j = 0; j < numSeg; ++j) {
        //        float t = (float)j / (float)numSeg;
        //        float t2 = t * t;
        //        float t3 = t * t2;
        //        T P = 0.5f * ((-t3 + 2.0f * t2 - t) * P1 + (3.0f * t3 - 5.0f * t2 + 2.0f) * P2 + (-3.0f * t3 + 4.0f * t2 + t) * P3 + (t3 - t2) * P4);
        //        pointList.Add(P);
        //    }
        //}
        public static void computeCatmullRomPoints(Real P1, Real P2, Real P3, Real P4, uint numSeg, ref std_vector<Real> pointList) {
            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t * t2;
                Real P = 0.5f * ((-t3 + 2.0f * t2 - t) * P1 + (3.0f * t3 - 5.0f * t2 + 2.0f) * P2 + (-3.0f * t3 + 4.0f * t2 + t) * P3 + (t3 - t2) * P4);
                pointList.push_back(P);
            }
        }
        public static void computeCatmullRomPoints(Vector2 P1, Vector2 P2, Vector2 P3, Vector2 P4, uint numSeg, ref std_vector<Vector2> pointList) {
            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t * t2;
                Vector2 P = 0.5f * ((-t3 + 2.0f * t2 - t) * P1 + (3.0f * t3 - 5.0f * t2 + 2.0f) * P2 + (-3.0f * t3 + 4.0f * t2 + t) * P3 + (t3 - t2) * P4);
                pointList.push_back(P);
            }
        }
        public static void computeCatmullRomPoints(Vector3 P1, Vector3 P2, Vector3 P3, Vector3 P4, uint numSeg, ref std_vector<Vector3> pointList) {
            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t * t2;
                Vector3 P = 0.5f * ((-t3 + 2.0f * t2 - t) * P1 + (3.0f * t3 - 5.0f * t2 + 2.0f) * P2 + (-3.0f * t3 + 4.0f * t2 + t) * P3 + (t3 - t2) * P4);
                pointList.push_back(P);
            }
        }
        //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
        //ORIGINAL LINE: template <class T>

        //public static void computeKochanekBartelsPoints<T>(KochanekBartelsSplineControlPoint<T> P1, KochanekBartelsSplineControlPoint<T> P2, KochanekBartelsSplineControlPoint<T> P3, KochanekBartelsSplineControlPoint<T> P4, uint numSeg, ref List<T> pointList) {
        //    Vector2 m0 = (1 - P2.tension) * (1 + P2.bias) * (1 + P2.continuity) / 2.0f * (P2.position - P1.position) + (1 - P2.tension) * (1 - P2.bias) * (1 - P2.continuity) / 2.0f * (P3.position - P2.position);
        //    Vector2 m1 = (1 - P3.tension) * (1 + P3.bias) * (1 - P3.continuity) / 2.0f * (P3.position - P2.position) + (1 - P3.tension) * (1 - P3.bias) * (1 + P3.continuity) / 2.0f * (P4.position - P3.position);

        //    for (uint j = 0; j < numSeg; ++j) {
        //        float t = (float)j / (float)numSeg;
        //        float t2 = t * t;
        //        float t3 = t2 * t;
        //        T P = (2 * t3 - 3 * t2 + 1) * P2.position + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * P3.position + (t3 - t2) * m1;
        //        pointList.Add(P);
        //    }
        //}  
        public static void computeKochanekBartelsPoints(KochanekBartelsSplineControlPoint<Vector2> P1, KochanekBartelsSplineControlPoint<Vector2> P2, KochanekBartelsSplineControlPoint<Vector2> P3, KochanekBartelsSplineControlPoint<Vector2> P4, uint numSeg, ref std_vector<Vector2> pointList) {
            Vector2 m0 = (1 - P2.tension) * (1 + P2.bias) * (1 + P2.continuity) / 2.0f * (P2.position - P1.position) + (1 - P2.tension) * (1 - P2.bias) * (1 - P2.continuity) / 2.0f * (P3.position - P2.position);
            Vector2 m1 = (1 - P3.tension) * (1 + P3.bias) * (1 - P3.continuity) / 2.0f * (P3.position - P2.position) + (1 - P3.tension) * (1 - P3.bias) * (1 + P3.continuity) / 2.0f * (P4.position - P3.position);

            for (uint j = 0; j < numSeg; ++j) {
                float t = (float)j / (float)numSeg;
                float t2 = t * t;
                float t3 = t2 * t;
                Vector2 P = (2 * t3 - 3 * t2 + 1) * P2.position + (t3 - 2 * t2 + t) * m0 + (-2 * t3 + 3 * t2) * P3.position + (t3 - t2) * m1;
                pointList.push_back(P);
            }
        }
    }
}