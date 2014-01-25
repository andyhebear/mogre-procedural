
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

//#ifndef PROCEDURAL_PATH_GENERATORS_INCLUDED
#define PROCEDURAL_PATH_GENERATORS_INCLUDED

//use new std wrapper...ok


namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    using ControlPoint = Mogre_Procedural.CubicHermiteSplineControlPoint<Mogre.Vector3>;
    //*
    // * \addtogroup shapegrp
    // * @{
    // 
    //-----------------------------------------------------------------------
    //*
    // * Builds a path from a Catmull-Rom Spline.
    // * Catmull-Rom Spline is the exact equivalent of Ogre's simple spline, ie
    // * a spline for which position is smoothly interpolated between control points
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport CatmullRomSpline3 : public BaseSpline3<CatmullRomSpline3>
    public class CatmullRomSpline3 : BaseSpline3<CatmullRomSpline3>
    {
        private std_vector<Vector3> mPoints = new std_vector<Vector3>();
        /// Default constructor
        public CatmullRomSpline3() {
        }

        /// Copy constructor from an Ogre simplespline
        public CatmullRomSpline3(Mogre.SimpleSpline input) {
            mPoints.resize((int)input.NumPoints);
            for (ushort i = 0; i < input.NumPoints; i++)
                mPoints.push_back(input.GetPoint(i));
        }

        /// Outputs current spline to an Ogre spline
        //
        //ORIGINAL LINE: Ogre::SimpleSpline toSimpleSpline() const
        public SimpleSpline toSimpleSpline() {
            Mogre.SimpleSpline spline = new SimpleSpline();
            for (ushort i = 0; i < mPoints.Count; i++)
                spline.AddPoint(mPoints[i]);
            return spline;
        }

        /// Adds a control point
        public CatmullRomSpline3 addPoint(Vector3 pt) {
            mPoints.push_back(pt);
            return this;
        }

        /// Adds a control point
        public CatmullRomSpline3 addPoint(float x, float y, float z) {
            mPoints.push_back(new Vector3(x, y, z));
            return this;
        }

        /// Safely gets a control point
        //
        //ORIGINAL LINE: inline const Ogre::Vector3& safeGetPoint(uint i) const
        public Vector3 safeGetPoint(uint i) {
            if (mClosed)
                return mPoints[Utils.modulo((int)i, mPoints.Count)];
            return mPoints[Utils.cap((int)i, 0, mPoints.Count - 1)];
        }

        //    *
        //	 * Build a path from Catmull-Rom control points
        //	 
        //-----------------------------------------------------------------------
        public Path realizePath() {
            Path path = new Path();

            int numPoints = mClosed ? mPoints.Count : mPoints.Count - 1;
            for (uint i = 0; i < numPoints; ++i) {
                Vector3 P1 = safeGetPoint(i - 1);
                Vector3 P2 = safeGetPoint(i);
                Vector3 P3 = safeGetPoint(i + 1);
                Vector3 P4 = safeGetPoint(i + 2);
                List<Vector3> lref = path.getPointsReference();
                GlobalMembers.computeCatmullRomPoints(P1, P2, P3, P4, mNumSeg, ref lref);

                if (i == mPoints.size() - 2 && !mClosed)
                    path.addPoint(P3);
            }
            if (mClosed)
                path.close();

            return path;
        }
    }
    //-----------------------------------------------------------------------
    //*
    // * Produces a path from Cubic Hermite control points
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport CubicHermiteSpline3 : public BaseSpline3<CubicHermiteSpline3>
    public class CubicHermiteSpline3 : BaseSpline3<CubicHermiteSpline3>
    {

        //#define ControlPoint_AlternateDefinition1
        private std_vector<ControlPoint> mPoints = new std_vector<CubicHermiteSplineControlPoint<Vector3>>();

        /// Adds a control point
        public CubicHermiteSpline3 addPoint(Vector3 p, Vector3 before, Vector3 after) {
            mPoints.push_back(new ControlPoint(p, before, after));
            return this;
        }
        /// Adds a control point
        public CubicHermiteSpline3 addPoint(Vector3 p, Vector3 tangent) {
            mPoints.push_back(new ControlPoint(p, tangent, tangent));
            return this;
        }
        /// Adds a control point
        public CubicHermiteSpline3 addPoint(Vector3 p) {
            return addPoint(p, CubicHermiteSplineAutoTangentMode.AT_CATMULL);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: inline CubicHermiteSpline3& addPoint(const Ogre::Vector3& p, CubicHermiteSplineAutoTangentMode autoTangentMode = AT_CATMULL)
        public CubicHermiteSpline3 addPoint(Vector3 p, CubicHermiteSplineAutoTangentMode autoTangentMode) {
            ControlPoint cp = new CubicHermiteSplineControlPoint<Vector3>();
            cp.position = p;
            cp.autoTangentBefore = autoTangentMode;
            cp.autoTangentAfter = autoTangentMode;
            mPoints.push_back(cp);
            return this;
        }
        /// Adds a control point
        public CubicHermiteSpline3 addPoint(float x, float y, float z) {
            return addPoint(x, y, z, CubicHermiteSplineAutoTangentMode.AT_CATMULL);
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: inline CubicHermiteSpline3& addPoint(Ogre::float x, Ogre::float y, Ogre::float z, CubicHermiteSplineAutoTangentMode autoTangentMode = AT_CATMULL)
        public CubicHermiteSpline3 addPoint(float x, float y, float z, CubicHermiteSplineAutoTangentMode autoTangentMode) {
            ControlPoint cp = new CubicHermiteSplineControlPoint<Vector3>();
            cp.position = new Vector3(x, y, z);
            cp.autoTangentBefore = autoTangentMode;
            cp.autoTangentAfter = autoTangentMode;
            mPoints.push_back(cp);
            return this;
        }
        /// Safely gets a control point
        //
        //ORIGINAL LINE: inline const CubicHermiteSplineControlPoint<Ogre::Vector3>& safeGetPoint(uint i) const
        public CubicHermiteSplineControlPoint<Vector3> safeGetPoint(uint i) {
            if (mClosed)
                return mPoints[Utils.modulo((int)i, mPoints.Count)];
            return mPoints[Utils.cap((int)i, 0, mPoints.Count - 1)];
        }

        //    *
        //	 * Builds a path from control points
        //	 
        //-----------------------------------------------------------------------
        public Path realizePath() {
            Path path = new Path();
            //Precompute tangents
            for (uint i = 0; i < mPoints.size(); ++i) {
                ControlPoint mp = mPoints[(int)i];
                GlobalMembers.computeTangents(ref mp, safeGetPoint(i - 1).position, safeGetPoint(i + 1).position);
            }
            int numPoints = mClosed ? mPoints.size() : (mPoints.size() - 1);
            for (int i = 0; i < numPoints; ++i) {
                ControlPoint pointBefore = mPoints[i];
                ControlPoint pointAfter = safeGetPoint((uint)i + 1);

                GlobalMembers.computeCubicHermitePoints(pointBefore, pointAfter, mNumSeg, ref path.getPointsReference());

                if (i == mPoints.size() - 2 && !mClosed)
                    path.addPoint(pointAfter.position);

            }
            if (mClosed)
                path.close();

            return path;
        }
    }

    //-----------------------------------------------------------------------
    /// Builds a line Path between 2 points
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport LinePath
    public class LinePath
    {
        private Vector3 mPoint1 = new Vector3();
        private Vector3 mPoint2 = new Vector3();
        private uint mNumSeg;

        /// Default constructor
        public LinePath() {
            mPoint1 = Vector3.ZERO;
            mPoint2 = Vector3.UNIT_Y;
            mNumSeg = 1;
        }

        /// Sets first point
        public LinePath setPoint1(Vector3 point1) {
            //
            //ORIGINAL LINE: mPoint1 = point1;
            mPoint1 = (point1);
            return this;
        }

        /// Sets second point
        public LinePath setPoint2(Vector3 point2) {
            //
            //ORIGINAL LINE: mPoint2 = point2;
            mPoint2 = (point2);
            return this;
        }

        /// Sets the number of segments for this line
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public LinePath setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::LinePath::setNumSeg(unsigned int)", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 segments");
            ;
            mNumSeg = numSeg;
            return this;
        }

        /// Builds a linepath between 2 points
        public LinePath betweenPoints(Vector3 point1, Vector3 point2) {
            //
            //ORIGINAL LINE: mPoint1 = point1;
            mPoint1 = (point1);
            //
            //ORIGINAL LINE: mPoint2 = point2;
            mPoint2 = (point2);
            return this;
        }

        /// Outputs a path
        public Path realizePath() {
            Path p = new Path();
            for (uint i = 0; i <= mNumSeg; ++i) {
                p.addPoint((1 - i / (float)mNumSeg) * mPoint1 + i / (float)mNumSeg * mPoint2);
            }
            return p;
        }

    }
    //-----------------------------------------------------------------------
    //*
    // * Produces a path by rounding corners of a straight-lines path
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport RoundedCornerSpline3 : public BaseSpline3<RoundedCornerSpline3>
    public class RoundedCornerSpline3 : BaseSpline3<RoundedCornerSpline3>
    {
        private float mRadius = 0f;

        private std_vector<Vector3> mPoints = new std_vector<Vector3>();

        /// Default constructor
        public RoundedCornerSpline3() {
            mRadius = 0.1f;
        }

        /// Sets the radius of the corners (default = 0.1)
        public RoundedCornerSpline3 setRadius(float radius) {
            mRadius = radius;
            return this;
        }

        /// Adds a control point
        public RoundedCornerSpline3 addPoint(Vector3 p) {
            mPoints.push_back(p);
            return this;
        }

        /// Adds a control point
        public RoundedCornerSpline3 addPoint(float x, float y, float z) {
            mPoints.push_back(new Vector3(x, y, z));
            return this;
        }

        /// Safely gets a control point
        //
        //ORIGINAL LINE: inline const Ogre::Vector3& safeGetPoint(uint i) const
        public Vector3 safeGetPoint(uint i) {
            if (mClosed)
                return mPoints[Utils.modulo((int)i, mPoints.Count)];
            return mPoints[Utils.cap((int)i, 0, mPoints.Count - 1)];
        }

        //    *
        //	 * Builds a shape from control points
        //	 * \exception Ogre::InvalidStateException The path contains no points
        //	 

        //-----------------------------------------------------------------------
        public Path realizePath() {
            if (mPoints.empty())
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The path contains no points", "Procedural::RoundedCornerSpline3::realizePath()", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 count mPoints");
            ;

            Path path = new Path();
            int numPoints = mClosed ? mPoints.Count : (mPoints.Count - 2);
            if (!mClosed)
                path.addPoint(mPoints[0]);

            for (uint i = 0; i < numPoints; ++i) {
                Vector3 p0 = safeGetPoint(i);
                Vector3 p1 = safeGetPoint(i + 1);
                Vector3 p2 = safeGetPoint(i + 2);

                Vector3 vBegin = p1 - p0;
                Vector3 vEnd = p2 - p1;

                // We're capping the radius if it's too big compared to segment length
                float radius = mRadius;
                float smallestSegLength = System.Math.Min(vBegin.Length, vEnd.Length);
                if (smallestSegLength < 2 * mRadius)
                    radius = smallestSegLength / 2.0f;

                Vector3 pBegin = p1 - vBegin.NormalisedCopy * radius;
                Vector3 pEnd = p1 + vEnd.NormalisedCopy * radius;
                Mogre_Procedural.Plane plane1 = new Plane(vBegin, pBegin);
                Mogre_Procedural.Plane plane2 = new Plane(vEnd, pEnd);
                Line axis = new Line();
                plane1.intersect(plane2, ref axis);

                Vector3 vradBegin = axis.shortestPathToPoint(pBegin);
                Vector3 vradEnd = axis.shortestPathToPoint(pEnd);
                Quaternion q = vradBegin.GetRotationTo(vradEnd);
                Vector3 center = pBegin - vradBegin;
                Radian angleTotal = new Radian();
                Vector3 vAxis = new Vector3();
                q.ToAngleAxis(out angleTotal, out vAxis);

                for (uint j = 0; j <= mNumSeg; j++) {
                    q.FromAngleAxis(angleTotal * (float)j / (float)mNumSeg, vAxis);
                    path.addPoint(center + q * vradBegin);
                }
            }

            if (!mClosed)
                path.addPoint(mPoints[mPoints.size() - 1]);

            if (mClosed)
                path.close();

            return path;
        }
    }

    //-----------------------------------------------------------------------
    //*
    // * Builds a path from a Bezier-Curve.
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport BezierCurve3 : public BaseSpline3<BezierCurve3>
    public class BezierCurve3 : BaseSpline3<BezierCurve3>
    {
        private std_vector<Vector3> mPoints = new std_vector<Vector3>();
        private uint mNumSeg;

        /// Default constructor
        public BezierCurve3() {
            mNumSeg = 8;
        }

        /// Sets number of segments per two control points
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public BezierCurve3 setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BezierCurve3::setNumSeg(unsigned int)", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 segments");
            ;
            mNumSeg = numSeg;
            return this;
        }

        /// Adds a control point
        public BezierCurve3 addPoint(Vector3 pt) {
            mPoints.push_back(pt);
            return this;
        }

        /// Adds a control point
        public BezierCurve3 addPoint(float x, float y, float z) {
            mPoints.push_back(new Vector3(x, y, z));
            return this;
        }

        /// Safely gets a control point
        //
        //ORIGINAL LINE: inline const Ogre::Vector3& safeGetPoint(uint i) const
        public Vector3 safeGetPoint(uint i) {
            if (mClosed)
                return mPoints[Utils.modulo((int)i, mPoints.Count)];
            return mPoints[Utils.cap((int)i, 0, mPoints.Count - 1)];
        }

        //    *
        //	 * Build a path from bezier control points
        //	 * @exception Ogre::InvalidStateException The curve must at least contain 2 points
        //	 

        //-----------------------------------------------------------------------
        public Path realizePath() {
            if (mPoints.size() < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The curve must at least contain 2 points", "Procedural::BezierCurve3::realizePath()", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 mPoints count");
            ;

            uint[] coef = new uint[mPoints.size()];
            if (mPoints.size() == 2) {
                coef[0] = 1;
                coef[1] = 1;
            }
            else if (mPoints.size() == 3) {
                coef[0] = 1;
                coef[1] = 2;
                coef[2] = 1;
            }
            else if (mPoints.size() == 4) {
                coef[0] = 1;
                coef[1] = 3;
                coef[2] = 3;
                coef[3] = 1;
            }
            else {
                for (uint i = 0; i < (int)mPoints.Count; i++)
                    coef[i] = Utils.binom((uint)mPoints.Count - 1, i);
            }

            uint div = (uint)(mPoints.Count - 1) * mNumSeg + 1;
            float dt = 1.0f / (float)div;

            Path path = new Path();
            float t = 0.0f;
            while (t < 1.0f) {
                float x = 0.0f;
                float y = 0.0f;
                float z = 0.0f;
                for (int i = 0; i < (int)mPoints.size(); i++) {
                    float fac = coef[i] * (float)System.Math.Pow(t, i) * (float)System.Math.Pow(1.0f - t, (int)mPoints.Count - 1 - i);
                    x += fac * mPoints[i].x;
                    y += fac * mPoints[i].y;
                    z += fac * mPoints[i].z;
                }
                path.addPoint(x, y, z);
                t += dt;
            }
            coef = null;

            return path;
        }
    }
    //* @} 
}
