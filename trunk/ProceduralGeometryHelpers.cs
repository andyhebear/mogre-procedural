using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
public static class GlobalMembersProceduralGeometryHelpers
{
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Vector3 Line::shortestPathToPoint(const Vector3& point) const
	public static Vector3 Line.shortestPathToPoint(Vector3 point)
	{
		Vector3 projection = (point-mPoint).dotProduct(mDirection) * mDirection;
		Vector3 vec = -projection+point-mPoint;
		return vec;
	}
	//-----------------------------------------------------------------------

	public static void isect(float VV0, float VV1, float VV2, float D0, float D1, float D2, ref float isect0, ref float isect1)
	{
		isect0 =VV0+(VV1-VV0)*D0/(D0-D1);
		isect1 =VV0+(VV2-VV0)*D0/(D0-D2);
	}


	public static void computeIntervals(float VV0, float VV1, float VV2, float D0, float D1, float D2, float D0D1, float D0D2, ref float isect0, ref float isect1)
	{
		if (D0D1>0.0f)
		{
			// here we know that D0D2<=0.0 
			// that is D0, D1 are on the same side, D2 on the other or on the plane 
			isect(VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1);
		}
		else if (D0D2>0.0f)
		{
			// here we know that d0d1<=0.0 
			isect(VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1);
		}
		else if (D1 *D2>0.0f || D0!=0.0f)
		{
			// here we know that d0d1<=0.0 or that D0!=0.0 
			isect(VV0, VV1, VV2, D0, D1, D2, ref isect0, ref isect1);
		}
		else if (D1!=0.0f)
		{
			isect(VV1, VV0, VV2, D1, D0, D2, ref isect0, ref isect1);
		}
		else if (D2!=0.0f)
		{
			isect(VV2, VV0, VV1, D2, D0, D1, ref isect0, ref isect1);
		}
		else
		{
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
public class Circle
{
	private Vector2 mCenter = new Vector2();
	private float mRadius = 0f;


	public Circle()
	{
		mCenter = Vector2.ZERO;
		mRadius = 1;
	}

	/// Contructor with arguments
	public Circle(Vector2 center, float radius)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mCenter = center;
		mCenter.CopyFrom(center);
		mRadius = radius;
	}

	/// Contructor with arguments
	//-----------------------------------------------------------------------
	public Circle(Vector2 p1, Vector2 p2, Vector2 p3)
	{
		Vector2 c1 = 0.5f*(p1+p2);
		Vector2 d1 = (p2-p1).perpendicular();
		float a1 = d1.y;
		float b1 = -d1.x;
		float g1 = d1.x *c1.y-d1.y *c1.x;
	
		Vector2 c3 = 0.5f*(p2+p3);
		Vector2 d3 = (p3-p2).perpendicular();
		float a2 = d3.y;
		float b2 = -d3.x;
		float g2 = d3.x *c3.y-d3.y *c3.x;
	
		Vector2 GlobalMembersProceduralGeometryHelpers.intersect = new Vector2();
		float intersectx = (b2 *g1-b1 *g2)/(b1 *a2-b2 *a1);
		float intersecty = (a2 *g1-a1 *g2)/(a1 *b2-a2 *b1);
	
		GlobalMembersProceduralGeometryHelpers.intersect = new Vector2(intersectx, intersecty);
	
		mCenter = GlobalMembersProceduralGeometryHelpers.intersect;
		mRadius = (GlobalMembersProceduralGeometryHelpers.intersect-p1).length();
	}

	/// Tells whether that point is inside the circle or not
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& p) const
	public bool isPointInside(Vector2 p)
	{
		return (p-mCenter).Length < mRadius;
	}
}
//-----------------------------------------------------------------------
/// Extends the Ogre::Plane class to be able to compute the intersection between 2 planes
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Plane : public Ogre::Plane
public class Plane 
{
	public Plane() : base()
	{
	}

	/// Contructor with arguments
	public Plane(Vector3 normal, Vector3 pos) : base(normal, pos)
	{
	}

	public Plane(float a, float b, float c, float d) : base(a,b,c,d)
	{
	}

//    *
//	 * Checks whether 2 planes intersect and compute intersecting line if it is the case.
//	 * @param other the other plane with which to check for intersection
//	 * @param outputLine the intersecting line, if planes actually intersect
//	 * @returns true if planes intersect, false otherwise
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool intersect(const Plane& STLAllocator<U, AllocPolicy>, Line& outputLine) const
	public bool intersect(Plane STLAllocator<U, AllocPolicy>, ref Line outputLine)
	{
		//TODO : handle the case where the plane is perpendicular to T
		Vector3 point1 = new Vector3(Vector3.ZERO);
		Vector3 direction = normal.crossProduct(STLAllocator<U, AllocPolicy>.normal);
		if (direction.squaredLength() < 1e-08)
			return false;
	
		float cp = normal.x *STLAllocator<U, AllocPolicy>.normal.y-STLAllocator<U, AllocPolicy>.normal.x *normal.y;
		if (cp!=0)
		{
			float denom = 1.0f/cp;
			point1.x = (normal.y *STLAllocator<U, AllocPolicy>.d-STLAllocator<U, AllocPolicy>.normal.y *d)*denom;
			point1.y = (STLAllocator<U, AllocPolicy>.normal.x *d-normal.x *STLAllocator<U, AllocPolicy>.d)*denom;
			point1.z = 0;
		}
		else if ((cp = normal.y *STLAllocator<U, AllocPolicy>.normal.z-STLAllocator<U, AllocPolicy>.normal.y *normal.z)!=0)
		{
			//special case #1
			float denom = 1.0f/cp;
			point1.x = 0;
			point1.y = (normal.z *STLAllocator<U, AllocPolicy>.d-STLAllocator<U, AllocPolicy>.normal.z *d)*denom;
			point1.z = (STLAllocator<U, AllocPolicy>.normal.y *d-normal.y *STLAllocator<U, AllocPolicy>.d)*denom;
		}
		else if ((cp = normal.x *STLAllocator<U, AllocPolicy>.normal.z-STLAllocator<U, AllocPolicy>.normal.x *normal.z)!=0)
		{
			//special case #2
			float denom = 1.0f/cp;
			point1.x = (normal.z *STLAllocator<U, AllocPolicy>.d-STLAllocator<U, AllocPolicy>.normal.z *d)*denom;
			point1.y = 0;
			point1.z = (STLAllocator<U, AllocPolicy>.normal.x *d-normal.x *STLAllocator<U, AllocPolicy>.d)*denom;
		}
	
		outputLine = new Line(point1, direction);
	
		return true;
	}
}
//-----------------------------------------------------------------------
/// Represents a line in 3D
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Line
public class Line
{
	public Vector3 mPoint = new Vector3();
	public Vector3 mDirection = new Vector3();

	public Line()
	{
	}

	/// Contructor with arguments
	/// @param point a point on the line
	/// @param direction a normalized vector representing the direction of that line
	public Line(Vector3 point, Vector3 direction)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPoint = point;
		mPoint.CopyFrom(point);
		mDirection = direction.normalisedCopy();
	}

	/// Builds the line between 2 points
	public void setFrom2Points(Vector3 a, Vector3 b)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPoint = a;
		mPoint.CopyFrom(a);
		mDirection = (b-a).normalisedCopy();
	}

	/// Finds the shortest vector between that line and a point
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::Vector3 shortestPathToPoint(const Ogre::Vector3& point) const;
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	Ogre::Vector3 shortestPathToPoint(Ogre::Vector3 point);
}
//-----------------------------------------------------------------------
/// Represents a line in 2D
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Line2D
public class Line2D
{
	private Vector2 mPoint = new Vector2();
	private Vector2 mDirection = new Vector2();

	public Line2D()
	{
	}

	/// Contructor with arguments
	/// @param point a point on the line
	/// @param direction a normalized vector representing the direction of that line
	public Line2D(Vector2 point, Vector2 direction)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPoint = point;
		mPoint.CopyFrom(point);
		mDirection = direction.normalisedCopy();
	}

	/// Builds the line between 2 points
	public void setFrom2Points(Vector2 a, Vector2 b)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPoint = a;
		mPoint.CopyFrom(a);
		mDirection = (b-a).normalisedCopy();
	}

//    *
//	 * Computes the interesction between current segment and another segment
//	 * @param other the other segment
//	 * @param intersection the point of intersection if outputed there if it exists
//	 * @return true if segments intersect, false otherwise
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool findIntersect(const Line2D& STLAllocator<U, AllocPolicy>, Vector2& intersection) const
	public bool findIntersect(Line2D STLAllocator<U, AllocPolicy>, ref Vector2 intersection)
	{
		const Vector2 p1 = mPoint;
		//const Vector2& p2 = mPoint+mDirection;
		const Vector2 p3 = STLAllocator<U, AllocPolicy>.mPoint;
		//const Vector2& p4 = other.mPoint+other.mDirection;
	
		Vector2 d1 = mDirection;
		float a1 = d1.y;
		float b1 = -d1.x;
		float g1 = d1.x *p1.y-d1.y *p1.x;
	
		Vector2 d3 = STLAllocator<U, AllocPolicy>.mDirection;
		float a2 = d3.y;
		float b2 = -d3.x;
		float g2 = d3.x *p3.y-d3.y *p3.x;
	
		// if both segments are parallel, early out
		if (d1.crossProduct(d3) == 0.)
			return false;
		float intersectx = (b2 *g1-b1 *g2)/(b1 *a2-b2 *a1);
		float intersecty = (a2 *g1-a1 *g2)/(a1 *b2-a2 *b1);
	
		intersection = new Vector2(intersectx, intersecty);
		return true;
	}
}
//-----------------------------------------------------------------------
/// Represents a 2D segment
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Segment2D
public class Segment2D
{
	public Vector2 mA = new Vector2();
	public Vector2 mB = new Vector2();

	public Segment2D()
	{
	}

	/// Contructor with arguments
	public Segment2D(Vector2 a, Vector2 b)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mA = a;
		mA.CopyFrom(a);
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mB = b;
		mB.CopyFrom(b);
	}

//    *
//	 * Computes the interesction between current segment and another segment
//	 * @param other the other segment
//	 * @param intersection the point of intersection if outputed there if it exists
//	 * @return true if segments intersect, false otherwise
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool findIntersect(const Segment2D& STLAllocator<U, AllocPolicy>, Vector2& intersection) const
	public bool findIntersect(Segment2D STLAllocator<U, AllocPolicy>, ref Vector2 intersection)
	{
		const Vector2 p1 = mA;
		const Vector2 p2 = mB;
		const Vector2 p3 = STLAllocator<U, AllocPolicy>.mA;
		const Vector2 p4 = STLAllocator<U, AllocPolicy>.mB;
	
	
		Vector2 d1 = p2-p1;
		float a1 = d1.y;
		float b1 = -d1.x;
		float g1 = d1.x *p1.y-d1.y *p1.x;
	
		Vector2 d3 = p4-p3;
		float a2 = d3.y;
		float b2 = -d3.x;
		float g2 = d3.x *p3.y-d3.y *p3.x;
	
		// if both segments are parallel, early out
		if (d1.crossProduct(d3) == 0.)
			return false;
	
		Vector2 GlobalMembersProceduralGeometryHelpers.intersect = new Vector2();
		float intersectx = (b2 *g1-b1 *g2)/(b1 *a2-b2 *a1);
		float intersecty = (a2 *g1-a1 *g2)/(a1 *b2-a2 *b1);
	
		GlobalMembersProceduralGeometryHelpers.intersect = new Vector2(intersectx, intersecty);
	
		if ((GlobalMembersProceduralGeometryHelpers.intersect-p1).dotProduct(GlobalMembersProceduralGeometryHelpers.intersect-p2)<0 && (GlobalMembersProceduralGeometryHelpers.intersect-p3).dotProduct(GlobalMembersProceduralGeometryHelpers.intersect-p4)<0)
		{
			intersection = GlobalMembersProceduralGeometryHelpers.intersect;
			return true;
		}
		return false;
	}

	/// Tells whether this segments intersects the other segment
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool intersects(const Segment2D& STLAllocator<U, AllocPolicy>) const
	public bool intersects(Segment2D STLAllocator<U, AllocPolicy>)
	{
		// Early out if segments have nothing in common
		Vector2 min1 = Utils.min(mA, mB);
		Vector2 max1 = Utils.max(mA, mB);
		Vector2 min2 = Utils.min(STLAllocator<U, AllocPolicy>.mA, STLAllocator<U, AllocPolicy>.mB);
		Vector2 max2 = Utils.max(STLAllocator<U, AllocPolicy>.mA, STLAllocator<U, AllocPolicy>.mB);
		if (max1.x<min2.x || max1.y<min2.y || max2.x<min1.x || max2.y<min2.y)
			return false;
		Vector2 t = new Vector2();
		return findIntersect(STLAllocator<U, AllocPolicy>, t);
	}
}
//-----------------------------------------------------------------------
// Compares 2 Vector2, with some tolerance
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Vector2Comparator
public class Vector2Comparator
{
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ()(const Ogre::Vector2& one, const Ogre::Vector2& two) const
//C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
	public static bool operator ==(Vector2 one, Vector2 two)
	{
		if ((one - two).squaredLength() < 1e-6)
			return false;
		if (Math.Abs(one.x - two.x) > 1e-3)
			return one.x < two.x;
		return one.y < two.y;
	}
}
//-----------------------------------------------------------------------
// Compares 2 Vector3, with some tolerance
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Vector3Comparator
public class Vector3Comparator
{
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ()(const Ogre::Vector3& one, const Ogre::Vector3& two) const
//C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
	public static bool operator ==(Vector3 one, Vector3 two)
	{
		if ((one-two).squaredLength()<1e-6)
			return false;
		if (Math.Abs(one.x - two.x)>1e-3)
			return one.x<two.x;
		if (Math.Abs(one.y - two.y)>1e-3)
			return one.y<two.y;
		return one.z<two.z;
	}
}
//-----------------------------------------------------------------------
/// Represents a 3D segment
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Segment3D
public class Segment3D
{
	public Vector3 mA = new Vector3();
	public Vector3 mB = new Vector3();
	public Segment3D()
	{
	}

	/// Contructor with arguments
	public Segment3D(Vector3 a, Vector3 b)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mA = a;
		mA.CopyFrom(a);
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mB = b;
		mB.CopyFrom(b);
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool epsilonEquivalent(const Segment3D& STLAllocator<U, AllocPolicy>) const
	public bool epsilonEquivalent(Segment3D STLAllocator<U, AllocPolicy>)
	{
		return ((mA.squaredDistance(STLAllocator<U, AllocPolicy>.mA) < 1e-8 && mB.squaredDistance(STLAllocator<U, AllocPolicy>.mB) < 1e-8) || (mA.squaredDistance(STLAllocator<U, AllocPolicy>.mB) < 1e-8 && mB.squaredDistance(STLAllocator<U, AllocPolicy>.mA) < 1e-8));
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Segment3D orderedCopy() const
	public Segment3D orderedCopy()
	{
		if (new Vector3Comparator()(mB, mA))
			return new Segment3D(mB, mA);
		return this;
	}

}
//-----------------------------------------------------------------------
/// Represents a 2D triangle
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Triangle2D
public class Triangle2D
{
	public Vector2[] mPoints = new Vector2[3];

	public Triangle2D(Vector2 a, Vector2 b, Vector2 c)
	{
		mPoints[0] =a;
		mPoints[1] =b;
		mPoints[2] =c;
	}
}
//-----------------------------------------------------------------------
/// Represents a 3D triangle
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport Triangle3D
public class Triangle3D
{
	public Vector3[] mPoints = new Vector3[3];

	public Triangle3D(Vector3 a, Vector3 b, Vector3 c)
	{
		mPoints[0] =a;
		mPoints[1] =b;
		mPoints[2] =c;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool findIntersect(const Triangle3D& STLAllocator<U, AllocPolicy>, Segment3D& intersection) const
	public bool findIntersect(Triangle3D STLAllocator<U, AllocPolicy>, ref Segment3D intersection)
	{
		// Compute plane equation of first triangle
		Vector3 e1 = mPoints[1]-mPoints[0];
		Vector3 e2 = mPoints[2]-mPoints[0];
		Vector3 n1 = e1.crossProduct(e2);
		float d1 = - n1.dotProduct(mPoints[0]);
	
		// Put second triangle in first plane equation to compute distances to the plane
		Real[] du = new Real[3];
		for (short i =0; i<3; i++)
		{
			du[i] = n1.dotProduct(STLAllocator<U, AllocPolicy>.mPoints[i]) + d1;
			if (Math.Abs(du[i])<1e-6)
				du[i] =0.0;
		}
	
		float du0du1 =du[0]*du[1];
		float du0du2 =du[0]*du[2];
	
		if (du0du1>0.0f && du0du2>0.0f) // same sign on all of them + not equal 0 ?
			return false; // no intersection occurs
	
		// Compute plane equation of first triangle
		e1 = STLAllocator<U, AllocPolicy>.mPoints[1]-STLAllocator<U, AllocPolicy>.mPoints[0];
		e2 = STLAllocator<U, AllocPolicy>.mPoints[2]-STLAllocator<U, AllocPolicy>.mPoints[0];
		Vector3 n2 = e1.crossProduct(e2);
		float d2 = - n2.dotProduct(STLAllocator<U, AllocPolicy>.mPoints[0]);
	
		// Put first triangle in second plane equation to compute distances to the plane
		Real[] dv = new Real[3];
		for (short i =0; i<3; i++)
		{
			dv[i] = n2.dotProduct(mPoints[i]) + d2;
			if (Math.Abs(dv[i])<1e-6)
				dv[i] =0.0;
		}
	
		float dv0dv1 =dv[0]*dv[1];
		float dv0dv2 =dv[0]*dv[2];
	
		if (dv0dv1>0.0f && dv0dv2>0.0f) // same sign on all of them + not equal 0 ?
			return false; // no intersection occurs
	
		//Compute the direction of intersection line
		Vector3 d = n1.crossProduct(n2);
	
		// We don't do coplanar triangles
		if (d.squaredLength()<1e-6)
			return false;
	
		// Project triangle points onto the intersection line
	
		// compute and index to the largest component of D
		float max =Math.Abs(d[0]);
		int index =0;
		float b =Math.Abs(d[1]);
		float c =Math.Abs(d[2]);
		if (b>max)
			max =b,index =1;
		if (c>max)
			max =c,index =2;
	
		// this is the simplified projection onto L
		float vp0 =mPoints[0][index];
		float vp1 =mPoints[1][index];
		float vp2 =mPoints[2][index];
	
		float up0 =STLAllocator<U, AllocPolicy>.mPoints[0][index];
		float up1 =STLAllocator<U, AllocPolicy>.mPoints[1][index];
		float up2 =STLAllocator<U, AllocPolicy>.mPoints[2][index];
	
		Real[] isect1 = new Real[2];
		Real[] isect2 = new Real[2];
		// compute interval for triangle 1
		GlobalMembersProceduralGeometryHelpers.computeIntervals(vp0, vp1, vp2, dv[0], dv[1], dv[2], dv0dv1, dv0dv2, ref isect1[0], ref isect1[1]);
	
		// compute interval for triangle 2
		GlobalMembersProceduralGeometryHelpers.computeIntervals(up0, up1, up2, du[0], du[1], du[2], du0du1, du0du2, ref isect2[0], ref isect2[1]);
	
		if (isect1[0]>isect1[1])
			std.swap(isect1[0],isect1[1]);
		if (isect2[0]>isect2[1])
			std.swap(isect2[0],isect2[1]);
	
		if (isect1[1]<isect2[0] || isect2[1]<isect1[0])
			return false;
	
		// Deproject segment onto line
		float r1 = Math.Max(isect1[0], isect2[0]);
		float r2 = Math.Min(isect1[1], isect2[1]);
	
		Plane pl1 = new Plane(n1.x, n1.y, n1.z, d1);
		Plane pl2 = new Plane(n2.x, n2.y, n2.z, d2);
		Line interLine = new Line();
		pl1.intersect(pl2, ref interLine);
		Vector3 p =interLine.mPoint;
		d.normalise();
		Vector3 v1 = p+(r1-p[index]) /d[index] *d;
		Vector3 v2 = p+(r2-p[index]) /d[index] *d;
		intersection.mA = v1;
		intersection.mB = v2;
	
	
		return true;
	}
}
//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: struct _ProceduralExport IntVector2
public class IntVector2
{
	public int x = new int();
	public int y = new int();
}
}


