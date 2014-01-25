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
    using PlaneSide = Mogre.Plane.Side;
    using System.Runtime.InteropServices;
    public static class GlobalMembersProceduralGeometryHelpers
    {
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: Vector3 Line::shortestPathToPoint(const Vector3& point) const
        //public static Vector3 Line.shortestPathToPoint(Vector3 point)
        //{
        //    Vector3 projection = (point-mPoint).dotProduct(mDirection) * mDirection;
        //    Vector3 vec = -projection+point-mPoint;
        //    return vec;
        //}
        //-----------------------------------------------------------------------
        //void isect(Real VV0,Real VV1,Real VV2,Real D0, Real D1,Real D2,Real& isect0,Real& isect1)
        public static void isect(float VV0, float VV1, float VV2, float D0, float D1, float D2, ref float isect0, ref float isect1) {
            isect0 = VV0 + (VV1 - VV0) * D0 / (D0 - D1);
            isect1 = VV0 + (VV2 - VV0) * D0 / (D0 - D2);
        }
        public static void computeIntervals(float VV0, float VV1, float VV2, float D0, float D1, float D2, float D0D1, float D0D2, ref float isect0, ref float isect1) {
            if (D0D1 > 0.0f) {
                // here we know that D0D2<=0.0 
                // that is D0, D1 are on the same side, D2 on the other or on the plane 
                isect(VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1);
            }
            else if (D0D2 > 0.0f) {
                // here we know that d0d1<=0.0 
                isect(VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1);
            }
            else if (D1 * D2 > 0.0f || D0 != 0.0f) {
                // here we know that d0d1<=0.0 or that D0!=0.0 
                isect(VV0, VV1, VV2, D0, D1, D2, ref isect0, ref isect1);
            }
            else if (D1 != 0.0f) {
                isect(VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1);
            }
            else if (D2 != 0.0f) {
                isect(VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1);
            }
            else {
                // triangles are coplanar 
                //return coplanar_tri_tri(N1,V0,V1,V2,U0,U1,U2);
            }
        }
    }





    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //struct Line;
    //-----------------------------------------------------------------------
    /// Represents a 2D circle
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Circle
    [StructLayout(LayoutKind.Sequential)]
    public class Circle
    {
        private Vector2 mCenter;
        private float mRadius = 0f;


        public Circle() {
            mCenter = new Vector2(0f, 0f);
            mRadius = 1;
        }

        /// Contructor with arguments
        public Circle(Vector2 center, float radius) {
            //
            //ORIGINAL LINE: mCenter = center;
            mCenter = (center);
            mRadius = radius;
        }

        /// Contructor with arguments
        //-----------------------------------------------------------------------
        public Circle(Vector2 p1, Vector2 p2, Vector2 p3) {
            Vector2 c1 = 0.5f * (p1 + p2);
            Vector2 d1 = (p2 - p1).Perpendicular;//垂直 垂线 正交 

            float a1 = d1.y;
            float b1 = -d1.x;
            float g1 = d1.x * c1.y - d1.y * c1.x;

            Vector2 c3 = 0.5f * (p2 + p3);
            Vector2 d3 = (p3 - p2).Perpendicular;
            float a2 = d3.y;
            float b2 = -d3.x;
            float g2 = d3.x * c3.y - d3.y * c3.x;


            float intersectx = (b2 * g1 - b1 * g2) / (b1 * a2 - b2 * a1);
            float intersecty = (a2 * g1 - a1 * g2) / (a1 * b2 - a2 * b1);
            Vector2 intersect = new Vector2(intersectx, intersecty);

            mCenter = intersect;
            mRadius = (intersect - p1).Length;
        }

        /// Tells whether that point is inside the circle or not
        //
        //ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& p) const
        public bool isPointInside(Vector2 p) {
            return (p - mCenter).Length < mRadius;
        }
    }
    //-----------------------------------------------------------------------
    /// Extends the Ogre::Plane class to be able to compute the intersection between 2 planes
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Plane : public Ogre::Plane
    [StructLayout(LayoutKind.Sequential)]
    public class Plane
    {
        #region Fields
        /// <summary>
        /// Distance from the origin.
        /// </summary>
        public float D;
        /// <summary>
        /// Direction the plane is facing.
        /// </summary>
        public Vector3 Normal;
        private static readonly Plane nullPlane = new Plane(Vector3.ZERO, 0);
        public static Plane Null {
            get {
                return nullPlane;
            }
        }
        #endregion Fields
        #region Constructors
        //public Plane()
        //{
        // this.Normal = Vector3.Zero;
        // this.D = Real.NaN;
        //}
        public Plane(Plane plane) {
            this.Normal = plane.Normal;
            this.D = plane.D;
        }
        /// <summary>
        /// Construct a plane through a normal, and a distance to move the plane along the normal.
        /// </summary>
        /// <param name="normal"></param>
        /// <param name="constant"></param>
        public Plane(Vector3 normal, float constant) {
            this.Normal = normal;
            this.D = -constant;
        }
        public Plane(Vector3 normal, Vector3 point) {
            this.Normal = normal;
            this.D = -normal.DotProduct(point);
        }
        /// <summary>
        /// Construct a plane from 3 coplanar points.
        /// </summary>
        /// <param name="point0">First point.</param>
        /// <param name="point1">Second point.</param>
        /// <param name="point2">Third point.</param>
        public Plane(Vector3 point0, Vector3 point1, Vector3 point2) {
            var edge1 = point1 - point0;
            var edge2 = point2 - point0;
            this.Normal = edge1.CrossProduct(edge2);
            this.Normal = this.Normal.NormalisedCopy;
            this.D = -this.Normal.DotProduct(point0);
        }


        public Plane(float a, float b, float c, float d) {
            this.Normal = new Vector3(a, b, c).NormalisedCopy;
            this.D = d;
        }
        #endregion
        #region Methods
        /// <summary>
        ///
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public PlaneSide GetSide(Vector3 point) {
            var distance = GetDistance(point);
            if (distance < 0.0f) {
                return PlaneSide.NEGATIVE_SIDE;//否定的 负面 消极的 

            }
            if (distance > 0.0f) {
                return PlaneSide.POSITIVE_SIDE;
            }
            return PlaneSide.NO_SIDE;
        }
        /// <summary>
        /// Returns the side where the aligneBox is. the flag Both indicates an intersecting box.
        /// one corner ON the plane is sufficient to consider the box and the plane intersecting.
        /// 注意如果BOX为无穷大，则即在左边也在plane的右边PlaneSide.NEGATIVE_SIDE| Mogre.Plane.Side.POSITIVE_SIDE
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public PlaneSide GetSide(AxisAlignedBox box) {
            if (box.IsNull) {
                return PlaneSide.NO_SIDE;
            }
            if (box.IsInfinite)//无限大的
{
                return PlaneSide.NEGATIVE_SIDE | Mogre.Plane.Side.POSITIVE_SIDE;
            }
            return GetSide(box.Center, box.HalfSize);
        }
        /// <summary>
        /// Returns which side of the plane that the given box lies on.
        /// The box is defined as centre/half-size pairs for effectively.
        /// </summary>
        /// <param name="centre">The centre of the box.</param>
        /// <param name="halfSize">The half-size of the box.</param>
        /// <returns>
        /// Positive if the box complete lies on the "positive side" of the plane,
        /// Negative if the box complete lies on the "negative side" of the plane,
        /// and Both if the box intersects the plane.
        /// </returns>
        public PlaneSide GetSide(Vector3 centre, Vector3 halfSize) {
            // Calculate the distance between box centre and the plane
            var dist = GetDistance(centre);
            // Calculate the maximise allows absolute distance for
            // the distance between box centre and plane
            var maxAbsDist = Math.Abs(this.Normal.DotProduct(halfSize));
            if (dist < -maxAbsDist) {
                return PlaneSide.NEGATIVE_SIDE;
            }
            if (dist > +maxAbsDist) {
                return PlaneSide.POSITIVE_SIDE;
            }
            return PlaneSide.NEGATIVE_SIDE | Mogre.Plane.Side.POSITIVE_SIDE;
        }
        /// <summary>
        /// This is a pseudodistance. The sign of the return value is
        /// positive if the point is on the positive side of the plane,
        /// negative if the point is on the negative side, and zero if the
        /// point is on the plane.
        /// The absolute value of the return value is the true distance only
        /// when the plane normal is a unit length vector.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public float GetDistance(Vector3 point) {
            return this.Normal.DotProduct(point) + this.D;
        }
        /// <summary>
        /// Redefine this plane based on a normal and a point.
        /// </summary>
        /// <param name="rkNormal">Normal vector</param>
        /// <param name="rkPoint">Point vector</param>
        public void Redefine(Vector3 rkNormal, Vector3 rkPoint) {
            this.Normal = rkNormal;
            this.D = -rkNormal.DotProduct(rkPoint);
        }
        /// <summary>
        /// Construct a plane from 3 coplanar points.
        /// </summary>
        /// <param name="point0">First point.</param>
        /// <param name="point1">Second point.</param>
        /// <param name="point2">Third point.</param>
        public void Redefine(Vector3 point0, Vector3 point1, Vector3 point2) {
            var edge1 = point1 - point0;
            var edge2 = point2 - point0;
            this.Normal = edge1.CrossProduct(edge2);
            this.Normal = this.Normal.NormalisedCopy;
            this.D = -this.Normal.DotProduct(point0);
        }
        /// <summary>
        /// Project a point onto the plane.
        /// </summary>
        public Vector3 ProjectVector(Vector3 point) {
            // We know plane normal is unit length, so use simple method
            Matrix3 xform = new Matrix3();
            xform.m00 = 1.0f - this.Normal.x * this.Normal.x;
            xform.m01 = -this.Normal.x * this.Normal.y;
            xform.m02 = -this.Normal.x * this.Normal.z;
            xform.m10 = -this.Normal.y * this.Normal.x;
            xform.m11 = 1.0f - this.Normal.y * this.Normal.y;
            xform.m12 = -this.Normal.y * this.Normal.z;
            xform.m20 = -this.Normal.z * this.Normal.x;
            xform.m21 = -this.Normal.z * this.Normal.y;
            xform.m22 = 1.0f - this.Normal.z * this.Normal.z;
            return xform * point;
        }
        #endregion Methods
        #region Object overrides
        /// <summary>
        /// Object method for testing equality.
        /// </summary>
        /// <param name="obj">Object to test.</param>
        /// <returns>True if the 2 planes are logically equal, false otherwise.</returns>
        public override bool Equals(object obj) {
            return obj is Plane && this == (Plane)obj;
        }
        /// <summary>
        /// Gets the hashcode for this Plane.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return this.D.GetHashCode() ^ this.Normal.GetHashCode();
        }
        /// <summary>
        /// Returns a string representation of this Plane.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return string.Format("Distance: {0} Normal: {1}", this.D, this.Normal);
        }
        #endregion
        #region Operator Overloads
        /// <summary>
        /// Compares 2 Planes for equality.
        /// </summary>
        /// <param name="left">First plane.</param>
        /// <param name="right">Second plane.</param>
        /// <returns>true if equal, false if not equal.</returns>
        public static bool operator ==(Plane left, Plane right) {
            return (left.D == right.D) && (left.Normal == right.Normal);
        }
        /// <summary>
        /// Compares 2 Planes for inequality.
        /// </summary>
        /// <param name="left">First plane.</param>
        /// <param name="right">Second plane.</param>
        /// <returns>true if not equal, false if equal.</returns>
        public static bool operator !=(Plane left, Plane right) {
            return (left.D != right.D) || (left.Normal != right.Normal);
        }
        #endregion
        public bool intersect(Plane plane, ref Line outputLine) {
            //TODO : handle the case where the plane is perpendicular to T
            Vector3 point1 = new Vector3(0f, 0f, 0f);
            Vector3 direction = Normal.CrossProduct(plane.Normal);
            if (direction.SquaredLength < 1e-08)
                return false;

            float cp = Normal.x * plane.Normal.y - plane.Normal.x * Normal.y;
            if (cp != 0) {
                float denom = 1.0f / cp;
                point1.x = (Normal.y * plane.D - plane.Normal.y * D) * denom;
                point1.y = (plane.Normal.x * D - Normal.x * plane.D) * denom;
                point1.z = 0f;
            }
            else if ((cp = Normal.y * plane.Normal.z - plane.Normal.y * Normal.z) != 0) {
                //special case #1
                float denom = 1.0f / cp;
                point1.x = 0;
                point1.y = (Normal.z * plane.D - plane.Normal.z * D) * denom;
                point1.z = (plane.Normal.y * D - Normal.y * plane.D) * denom;
            }
            else if ((cp = Normal.x * plane.Normal.z - plane.Normal.x * Normal.z) != 0) {
                //special case #2
                float denom = 1.0f / cp;
                point1.x = (Normal.z * plane.D - plane.Normal.z * D) * denom;
                point1.y = 0;
                point1.z = (plane.Normal.x * D - Normal.x * plane.D) * denom;
            }

            outputLine = new Line(point1, direction);

            return true;
        }
    }
    //-----------------------------------------------------------------------
    // Represents a line in 3D
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Line
    [StructLayout(LayoutKind.Sequential)]
    public class Line
    {
        public Vector3 mPoint = new Vector3();
        public Vector3 mDirection = new Vector3();

        public Line() {
        }

        /// Contructor with arguments
        /// @param point a point on the line
        /// @param direction a normalized vector representing the direction of that line
        public Line(Vector3 point, Vector3 direction) {
            //
            //ORIGINAL LINE: mPoint = point;
            mPoint = (point);
            mDirection = direction.NormalisedCopy;
        }

        /// Builds the line between 2 points
        public void setFrom2Points(Vector3 a, Vector3 b) {
            //
            //ORIGINAL LINE: mPoint = a;
            mPoint = (a);
            mDirection = (b - a).NormalisedCopy;
        }

        /// Finds the shortest vector between that line and a point
        //
        //ORIGINAL LINE: Ogre::Vector3 shortestPathToPoint(const Ogre::Vector3& point) const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	Ogre::Vector3 shortestPathToPoint(Ogre::Vector3 point);
        public Vector3 shortestPathToPoint(Vector3 point) {
            Vector3 projection = (point - mPoint).DotProduct(mDirection) * mDirection;
            Vector3 vec = -projection + point - mPoint;
            return vec;
        }
    }
    //-----------------------------------------------------------------------
    /// Represents a line in 2D
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Line2D
    [StructLayout(LayoutKind.Sequential)]
    public class Line2D
    {
        private Vector2 mPoint = new Vector2();
        private Vector2 mDirection = new Vector2();

        public Line2D() {
        }

        /// Contructor with arguments
        /// @param point a point on the line
        /// @param direction a normalized vector representing the direction of that line
        public Line2D(Vector2 point, Vector2 direction) {
            //
            //ORIGINAL LINE: mPoint = point;
            mPoint = (point);
            mDirection = direction.NormalisedCopy;
        }

        /// Builds the line between 2 points
        public void setFrom2Points(Vector2 a, Vector2 b) {
            //
            //ORIGINAL LINE: mPoint = a;
            mPoint = (a);
            mDirection = (b - a).NormalisedCopy;
        }

        //    *
        //	 * Computes the interesction between current segment and another segment
        //	 * @param other the other segment
        //	 * @param intersection the point of intersection if outputed there if it exists
        //	 * @return true if segments intersect, false otherwise
        //	 
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool findIntersect(const Line2D& STLAllocator<U, AllocPolicy>, Vector2& intersection) const
        public bool findIntersect(Line2D line2d, ref Vector2 intersection) {
            Vector2 p1 = mPoint;
            //const Vector2& p2 = mPoint+mDirection;
            Vector2 p3 = line2d.mPoint;
            //const Vector2& p4 = other.mPoint+other.mDirection;

            Vector2 d1 = mDirection;
            float a1 = d1.y;
            float b1 = -d1.x;
            float g1 = d1.x * p1.y - d1.y * p1.x;

            Vector2 d3 = line2d.mDirection;
            float a2 = d3.y;
            float b2 = -d3.x;
            float g2 = d3.x * p3.y - d3.y * p3.x;

            // if both segments are parallel, early out
            if (d1.CrossProduct(d3) == 0.0f)
                return false;
            float intersectx = (b2 * g1 - b1 * g2) / (b1 * a2 - b2 * a1);
            float intersecty = (a2 * g1 - a1 * g2) / (a1 * b2 - a2 * b1);

            intersection = new Vector2(intersectx, intersecty);
            return true;
        }
    }
    //-----------------------------------------------------------------------
    /// Represents a 2D segment
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Segment2D
    [StructLayout(LayoutKind.Sequential)]
    public class Segment2D
    {
        public Vector2 mA = new Vector2();
        public Vector2 mB = new Vector2();

        public Segment2D() {
        }

        /// Contructor with arguments
        public Segment2D(Vector2 a, Vector2 b) {
            //
            //ORIGINAL LINE: mA = a;
            mA = (a);
            //
            //ORIGINAL LINE: mB = b;
            mB = (b);
        }

        //    *
        //	 * Computes the interesction between current segment and another segment
        //	 * @param other the other segment
        //	 * @param intersection the point of intersection if outputed there if it exists
        //	 * @return true if segments intersect, false otherwise
        //	 
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool findIntersect(const Segment2D& STLAllocator<U, AllocPolicy>, Vector2& intersection) const
        public bool findIntersect(Segment2D segment2d, ref Vector2 intersection) {
            Vector2 p1 = mA;
            Vector2 p2 = mB;
            Vector2 p3 = segment2d.mA;
            Vector2 p4 = segment2d.mB;


            Vector2 d1 = p2 - p1;
            float a1 = d1.y;
            float b1 = -d1.x;
            float g1 = d1.x * p1.y - d1.y * p1.x;

            Vector2 d3 = p4 - p3;
            float a2 = d3.y;
            float b2 = -d3.x;
            float g2 = d3.x * p3.y - d3.y * p3.x;

            // if both segments are parallel, early out
            if (d1.CrossProduct(d3) == 0.0f)
                return false;

            //Vector2 GlobalMembersProceduralGeometryHelpers.intersect = new Vector2();
            float intersectx = (b2 * g1 - b1 * g2) / (b1 * a2 - b2 * a1);
            float intersecty = (a2 * g1 - a1 * g2) / (a1 * b2 - a2 * b1);

            Vector2 intersect = new Vector2(intersectx, intersecty);

            if ((intersect - p1).DotProduct(intersect - p2) < 0f && (intersect - p3).DotProduct(intersect - p4) < 0f) {
                intersection = intersect;
                return true;
            }
            return false;
        }

        /// Tells whether this segments intersects the other segment
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool intersects(const Segment2D& STLAllocator<U, AllocPolicy>) const
        public bool intersects(Segment2D segment2d) {
            // Early out if segments have nothing in common
            Vector2 min1 = Utils.min(mA, mB);
            Vector2 max1 = Utils.max(mA, mB);
            Vector2 min2 = Utils.min(segment2d.mA, segment2d.mB);
            Vector2 max2 = Utils.max(segment2d.mA, segment2d.mB);
            if (max1.x < min2.x || max1.y < min2.y || max2.x < min1.x || max2.y < min2.y)
                return false;
            Vector2 t = new Vector2();
            return findIntersect(segment2d, ref t);
        }
    }
    //-----------------------------------------------------------------------
    // Compares 2 Vector2, with some tolerance
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Vector2Comparator
    public class Vector2Comparator:IComparer<Vector2>
    {
        //
        //ORIGINAL LINE: bool operator ()(const Ogre::Vector2& one, const Ogre::Vector2& two) const
        //C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
        /// <summary>
        /// 是否相等????
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool Operator(Vector2 one, Vector2 two) {
            if ((one - two).SquaredLength < 1e-6)
                return false;
            if (Math.Abs(one.x - two.x) > 1e-3)
                return one.x < two.x;
            return one.y < two.y;
        }

        #region IComparer<Vector2> 成员

        public int Compare(Vector2 x, Vector2 y) {
            bool cp=Operator(x,y);
            return cp ? 1 : -1;
        }

        #endregion
    }
    //-----------------------------------------------------------------------
    // Compares 2 Vector3, with some tolerance
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Vector3Comparator
    public class Vector3Comparator : IComparer<Vector3>
    {
        //
        //ORIGINAL LINE: bool operator ()(const Ogre::Vector3& one, const Ogre::Vector3& two) const
        //C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
        //bool operator()(const Ogre::Vector3& one, const Ogre::Vector3& two) const
        /// <summary>
        /// 是否相等？？？
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool Operator(Vector3 one, Vector3 two) {
            if ((one - two).SquaredLength < 1e-6)
                return false;
            if (Math.Abs(one.x - two.x) > 1e-3)
                return one.x < two.x;
            if (Math.Abs(one.y - two.y) > 1e-3)
                return one.y < two.y;
            return one.z < two.z;
        }

     

        #region IComparer<Vector3> 成员

        public int Compare(Vector3 x, Vector3 y) {
            bool cp = Operator(x, y);
            return cp ? 1 : -1;
        }

        #endregion
    }
    //-----------------------------------------------------------------------
    /// Represents a 3D segment
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Segment3D
    [StructLayout(LayoutKind.Sequential)]
    public class Segment3D
    {
        public Vector3 mA = new Vector3();
        public Vector3 mB = new Vector3();
        public Segment3D() {
        }

        /// Contructor with arguments
        public Segment3D(Vector3 a, Vector3 b) {
            //
            //ORIGINAL LINE: mA = a;
            mA = (a);
            //
            //ORIGINAL LINE: mB = b;
            mB = (b);
        }

        //
        //ORIGINAL LINE: bool epsilonEquivalent(const Segment3D& STLAllocator<U, AllocPolicy>) const
        public bool epsilonEquivalent(Segment3D segment3D) {
            return (((mA - segment3D.mA).SquaredLength < 1e-8 && (mB - segment3D.mB).SquaredLength < 1e-8) ||
                ((mA - segment3D.mB).SquaredLength < 1e-8 && (mB - segment3D.mA).SquaredLength < 1e-8));
        }

        //
        //ORIGINAL LINE: Segment3D orderedCopy() const
        public Segment3D orderedCopy() {
            if (Vector3Comparator.Operator(mB, mA))
                return new Segment3D(mB, mA);
            return this;
        }

    }
    //-----------------------------------------------------------------------
    /// Represents a 2D triangle
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Triangle2D
    [StructLayout(LayoutKind.Sequential)]
    public class Triangle2D
    {
        public Vector2[] mPoints = new Vector2[3];

        public Triangle2D(Vector2 a, Vector2 b, Vector2 c) {
            mPoints[0] = a;
            mPoints[1] = b;
            mPoints[2] = c;
        }
    }
    //-----------------------------------------------------------------------
    /// Represents a 3D triangle
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport Triangle3D
    [StructLayout(LayoutKind.Sequential)]
    public class Triangle3D
    {
        public Vector3[] mPoints = new Vector3[3];

        public Triangle3D(Vector3 a, Vector3 b, Vector3 c) {
            mPoints[0] = a;
            mPoints[1] = b;
            mPoints[2] = c;
        }

        //
        //ORIGINAL LINE: bool findIntersect(const Triangle3D& STLAllocator<U, AllocPolicy>, Segment3D& intersection) const
        public bool findIntersect(Triangle3D triangle3d, ref Segment3D intersection) {
            // Compute plane equation of first triangle
            Vector3 e1 = mPoints[1] - mPoints[0];
            Vector3 e2 = mPoints[2] - mPoints[0];
            Vector3 n1 = e1.CrossProduct(e2);
            float d1 = -n1.DotProduct(mPoints[0]);

            // Put second triangle in first plane equation to compute distances to the plane
            float[] du = new float[3];
            for (short i = 0; i < 3; i++) {
                du[i] = n1.DotProduct(triangle3d.mPoints[i]) + d1;
                if (Math.Abs(du[i]) < 1e-6)
                    du[i] = 0.0f;
            }

            float du0du1 = du[0] * du[1];
            float du0du2 = du[0] * du[2];

            if (du0du1 > 0.0f && du0du2 > 0.0f) // same sign on all of them + not equal 0 ?
                return false; // no intersection occurs

            // Compute plane equation of first triangle
            e1 = triangle3d.mPoints[1] - triangle3d.mPoints[0];
            e2 = triangle3d.mPoints[2] - triangle3d.mPoints[0];
            Vector3 n2 = e1.CrossProduct(e2);
            float d2 = -n2.DotProduct(triangle3d.mPoints[0]);

            // Put first triangle in second plane equation to compute distances to the plane
            float[] dv = new float[3];
            for (short i = 0; i < 3; i++) {
                dv[i] = n2.DotProduct(mPoints[i]) + d2;
                if (Math.Abs(dv[i]) < 1e-6)
                    dv[i] = 0.0f;
            }

            float dv0dv1 = dv[0] * dv[1];
            float dv0dv2 = dv[0] * dv[2];

            if (dv0dv1 > 0.0f && dv0dv2 > 0.0f) // same sign on all of them + not equal 0 ?
                return false; // no intersection occurs

            //Compute the direction of intersection line
            Vector3 d = n1.CrossProduct(n2);

            // We don't do coplanar triangles
            if (d.SquaredLength < 1e-6)
                return false;

            // Project triangle points onto the intersection line

            // compute and index to the largest component of D
            float max = Math.Abs(d[0]);
            int index = 0;
            float b = Math.Abs(d[1]);
            float c = Math.Abs(d[2]);
            if (b > max) {
                max = b;
                index = 1;
            }
            if (c > max) {
                max = c;
                index = 2;
            }
            // this is the simplified projection onto L
            float vp0 = mPoints[0][index];
            float vp1 = mPoints[1][index];
            float vp2 = mPoints[2][index];

            float up0 = triangle3d.mPoints[0][index];
            float up1 = triangle3d.mPoints[1][index];
            float up2 = triangle3d.mPoints[2][index];

            float[] isect1 = new float[2];
            float[] isect2 = new float[2];
            // compute interval for triangle 1
            GlobalMembersProceduralGeometryHelpers.computeIntervals(vp0, vp1, vp2, dv[0], dv[1], dv[2], dv0dv1, dv0dv2, ref isect1[0], ref isect1[1]);

            // compute interval for triangle 2
            GlobalMembersProceduralGeometryHelpers.computeIntervals(up0, up1, up2, du[0], du[1], du[2], du0du1, du0du2, ref isect2[0], ref isect2[1]);

            if (isect1[0] > isect1[1])
                std_array_swap<float>(isect1, 0, 1);
            if (isect2[0] > isect2[1])
                std_array_swap<float>(isect2, 0, 1);

            if (isect1[1] < isect2[0] || isect2[1] < isect1[0])
                return false;

            // Deproject segment onto line
            float r1 = System.Math.Max(isect1[0], isect2[0]);
            float r2 = System.Math.Min(isect1[1], isect2[1]);

            Plane pl1 = new Plane(n1.x, n1.y, n1.z, d1);
            Plane pl2 = new Plane(n2.x, n2.y, n2.z, d2);
            Line interLine = new Line();
            pl1.intersect(pl2, ref interLine);
            Vector3 p = interLine.mPoint;
            //d.Normalise();
            d = d.NormalisedCopy;
            Vector3 v1 = p + (r1 - p[index]) / d[index] * d;
            Vector3 v2 = p + (r2 - p[index]) / d[index] * d;
            intersection.mA = v1;
            intersection.mB = v2;


            return true;
        }

        private void std_array_swap<T>(T[] array, int p, int p_3) {
            T t = array[p];
            array[p] = array[p_3];
            array[p_3] = t;
        }


    }
    //-----------------------------------------------------------------------
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: struct _ProceduralExport IntVector2
    [StructLayout(LayoutKind.Sequential)]
    public class IntVector2
    {
        public int x = 0;
        public int y = 0;
    }
}


