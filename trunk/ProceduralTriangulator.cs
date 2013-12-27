using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//struct Triangle2D;

//#define PointList_AlternateDefinition1

//*
// * Implements a Delaunay Triangulation algorithm.
// * It works on Shapes to build Triangle Buffers
// * \image html shape_triangulation.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Triangulator : public MeshGenerator<Triangulator>
public class Triangulator : MeshGenerator<Triangulator>
{
//C++ TO C# CONVERTER TODO TASK: The implementation of the following type could not be found.
//	struct Triangle;
//C++ TO C# CONVERTER TODO TASK: The implementation of the following type could not be found.
//	struct DelaunaySegment;

	//#define DelaunayTriangleBuffer_AlternateDefinition1
	//-----------------------------------------------------------------------
	private class DelaunaySegment
	{
		public int i1;
		public int i2;
		public DelaunaySegment(int _i1, int _i2)
		{
			i1 = _i1;
			i2 = _i2;
		}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator <(const DelaunaySegment& STLAllocator<U, AllocPolicy>) const
		public static bool operator <(DelaunaySegment This,DelaunaySegment Other)
		{
			if (This.i1!=Other.i1)
				return This.i1<Other.i1;
			else
				return This.i2<Other.i2;
		}
		public DelaunaySegment inverse()
		{
			return new DelaunaySegment(i2, i1);
		}
	}

	//-----------------------------------------------------------------------
	private class Triangle
	{
		public readonly List<Vector2> pl;
		public int[] i = new int[3];
		public Triangle(List<Vector2> pl)
		{
			this.pl = pl;
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 p(int k) const
		public Vector2 p(int k)
		{
			return ( pl)[i[k]];
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator ==(const Triangle& STLAllocator<U, AllocPolicy>) const
		public static bool operator ==(Triangle This,Triangle Other)
		{
			return This.i[0] ==Other.i[0] && This.i[1] ==Other.i[1] && This.i[2] ==Other.i[2];
		}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getMidPoint() const
		public Vector2 getMidPoint()
		{
			return 1.0f/3.0f * (p(0)+p(1)+p(2));
		}

		//-----------------------------------------------------------------------
		public void setVertices(int i0, int i1, int i2)
		{
			i[0] = i0;
			i[1] = i1;
			i[2] = i2;
		}

		//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int findSegNumber(int i0, int i1) const
		public int findSegNumber(int i0, int i1)
		{
			if ((i0 ==i[0] && i1 ==i[1])||(i0 ==i[1] && i1 ==i[0]))
				return 2;
			if ((i0 ==i[1] && i1 ==i[2])||(i0 ==i[2] && i1 ==i[1]))
				return 0;
			if ((i0 ==i[2] && i1 ==i[0])||(i0 ==i[0] && i1 ==i[2]))
				return 1;
		//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
		//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "We should not be here!", "Procedural::Triangulator::Triangle::findSegNumber(int i0, int i1)", __FILE__, __LINE__);
			throw new Exception("We should not be here!");
            ;
			return -1;
		}

//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//		bool isPointInside(Ogre::Vector2 point);

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool containsSegment(int i0, int i1) const
		public bool containsSegment(int i0, int i1)
		{
			return ((i0 ==i[0] || i0 ==i[1] || i0 ==i[2])&&(i1 ==i[0] || i1 ==i[1] || i1 ==i[2]));
		}

		public enum InsideType: int
		{
			IT_INSIDE,
			IT_OUTSIDE,
			IT_BORDERLINEOUTSIDE
		}

//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//		InsideType isPointInsideCircumcircle(Ogre::Vector2 point);

		public void makeDirectIfNeeded()
		{
			if ((p(1)-p(0)).CrossProduct(p(2)-p(0))<0f)
			{
				std_swap<int>(i,0, 1);
			}
		}

private void std_swap<T>(T[]array,int index1,int index2)
{
 	T temp=array[index1];
    array[index1]=array[index2];
    array[index2]=temp;
}

		public bool isDegenerate()
		{
			if (Math.Abs((p(1)-p(0)).CrossProduct(p(2)-p(0)))<1e-4)
				return true;
			return false;
		}

		public string debugDescription()
		{
			return "("+StringConverter_toString(i[0])+"," +StringConverter_toString(i[1])+","+StringConverter_toString(i[2])+") <" + "("+StringConverter_toString(p(0))+"," +StringConverter_toString(p(1))+","+StringConverter_toString(p(2))+">";
		}

private string StringConverter_toString(Vector2 vector2)
{
 	return string.Format("['{0}','{1}']",vector2.x,vector2.y);
}

private string StringConverter_toString(int p)
{
 	return p.ToString();
}
	}
	//-----------------------------------------------------------------------
	private class TouchSuperTriangle
	{
		public int i0;
		public int i1;
		public int i2;
		public TouchSuperTriangle(int i, int j, int k)
		{
			i0 = i;
			i1 = j;
			i2 = k;
		}
//C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
		public  bool Operator(Triangulator.Triangle tri)
		{
			for (int k =0; k<3; k++)
				if (tri.i[k] ==i0 || tri.i[k] ==i1 ||tri.i[k] ==i2)
					return true;
			return false;
		}
	}

	private readonly Shape mShapeToTriangulate;
	private readonly MultiShape mMultiShapeToTriangulate;
	private Triangle2D mManualSuperTriangle;
	private List<Segment2D> mSegmentListToTriangulate;
	private bool mRemoveOutside;

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void delaunay(List<Ogre::Vector2>& pointList, LinkedList<Triangle>& tbuffer) const
	private void delaunay(ref List<Vector2> pointList, ref LinkedList<Triangle> tbuffer)
#elif DelaunayTriangleBuffer_AlternateDefinition2
	void Triangulator.delaunay(ref List<Vector2>& pointList, ref LinkedList<Triangle>& tbuffer) const
#endif
#elif PointList_AlternateDefinition2
#if DelaunayTriangleBuffer_AlternateDefinition1
	void Triangulator.delaunay(ref List<Vector2>& pointList, ref LinkedList<Triangle>& tbuffer) const
#elif DelaunayTriangleBuffer_AlternateDefinition2
	void Triangulator.delaunay(ref List<Vector2>& pointList, ref LinkedList<Triangle>& tbuffer) const
#endif
#endif
	{
		// Compute super triangle or insert manual super triangle
		if (mManualSuperTriangle == null)
		{
			float maxTriangleSize = 0.0f;
#if PointList_AlternateDefinition1
			for (List<Vector2>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
#elif PointList_AlternateDefinition2
			for (List<Vector2>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
#endif
			{
				maxTriangleSize = System.Math.Max(maxTriangleSize, Math.Abs(it.x));
				maxTriangleSize = System.Math.Max(maxTriangleSize, Math.Abs(it.y));
			}
			pointList.Add(new Vector2(-3f *maxTriangleSize, -3f *maxTriangleSize));
			pointList.Add(new Vector2(3f *maxTriangleSize, -3f *maxTriangleSize));
			pointList.Add(new Vector2(0.0f, 3f *maxTriangleSize));
	
			int maxTriangleIndex =pointList.Count-3;
			Triangle superTriangle = new Triangle(pointList);
			superTriangle.i[0] = maxTriangleIndex;
			superTriangle.i[1] = maxTriangleIndex+1;
			superTriangle.i[2] = maxTriangleIndex+2;
			tbuffer.AddLast(superTriangle);
		}
	
		// Point insertion loop
		for (ushort i =0; i<pointList.Count-3; i++)
		{
			//Utils::log("insert point " + StringConverter::toString(i));
			LinkedList<LinkedList<Triangle>.Enumerator> borderlineTriangles = new LinkedList<LinkedList<Triangle>.Enumerator>();
			// Insert 1 point, find all triangles for which the point is in circumcircle
			Vector2 p = pointList[i];
			std.set<DelaunaySegment> segments = new std.set<DelaunaySegment>();
#if DelaunayTriangleBuffer_AlternateDefinition1
			for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#elif DelaunayTriangleBuffer_AlternateDefinition2
			for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#endif
			{
				Triangle.InsideType isInside = it.isPointInsideCircumcircle(p);
				if (isInside == Triangle.IT_INSIDE)
				{
					if (!it.isDegenerate())
					{
						//Utils::log("tri insie" + it->debugDescription());
						for (int k =0; k<3; k++)
						{
							DelaunaySegment[] d1 = new DelaunaySegment[k](it.i, it.i[(k+1)%3]);
							if (segments.find(d1)!=segments.end())
								segments.erase(d1);
							else if (segments.find(d1.inverse())!=segments.end())
								segments.erase(d1.inverse());
							else
								segments.insert(d1);
						}
					}
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
					it =tbuffer.erase(it);
				}
				else if (isInside == Triangle.IT_BORDERLINEOUTSIDE)
				{
					//Utils::log("tri borer " + it->debugDescription());
					borderlineTriangles.AddLast(it);
				}
				else
				{
				}
			}
	
			// Robustification of the standard algorithm : if one triangle's circumcircle was borderline against the new point,
			// test whether that triangle is intersected by new segments or not (normal situation : it should not)
			// If intersected, the triangle is considered having the new point in its circumc
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: std::set<DelaunaySegment> copySegment = segments;
			std.set<DelaunaySegment> copySegment = new std.set(segments);
			for (LinkedList<LinkedList<Triangle>.Enumerator>.Enumerator itpTri = borderlineTriangles.GetEnumerator(); itpTri.MoveNext(); itpTri++)
			{
#if DelaunayTriangleBuffer_AlternateDefinition1
				LinkedList<Triangle>.Enumerator itTri = itpTri.Current;
#elif DelaunayTriangleBuffer_AlternateDefinition2
				LinkedList<Triangle>.Enumerator itTri = itpTri.Current;
#endif
				bool triRemoved = false;
				for (std.set<DelaunaySegment>.Enumerator it = copySegment.begin(); it.MoveNext() && !triRemoved; ++it)
				{
					bool isTriangleIntersected = false;
					for (int k =0; k<2; k++)
					{
						int i1 = (k ==0)?it.i1:it.i2;
						int i2 = i;
						for (int l =0; l<3; l++)
						{
							//Early out if 2 points are in fact the same
							if (itTri.i[l] ==i1 || itTri.i[l] ==i2 || itTri.i[(l+1)%3] ==i1 || itTri.i[(l+1)%3] ==i2)
								continue;
							Segment2D seg2 = new Segment2D(itTri.p(l), itTri.p((l+1)%3));
							Segment2D[] seg1 = new Segment2D[i1](pointList, pointList[i2]);
							if (seg1.intersects(seg2))
							{
								isTriangleIntersected = true;
								break;
							}
						}
	
					}
					if (isTriangleIntersected)
					{
						if (!itTri.isDegenerate())
						{
							//Utils::log("tri inside" + itTri->debugDescription());
							for (int m =0; m<3; m++)
							{
								DelaunaySegment[] d1 = new DelaunaySegment[m](itTri.i, itTri.i[(m+1)%3]);
								if (segments.find(d1)!=segments.end())
									segments.erase(d1);
								else if (segments.find(d1.inverse())!=segments.end())
									segments.erase(d1.inverse());
								else
									segments.insert(d1);
							}
						}
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
						tbuffer.erase(itTri);
						triRemoved =true;
					}
				}
			}
	
			// Find all the non-interior edges
			for (std.set<DelaunaySegment>.Enumerator it = segments.begin(); it.MoveNext(); ++it)
			{
				Triangle dt = new Triangle(pointList);
				dt.setVertices(it.i1, it.i2, i);
				dt.makeDirectIfNeeded();
				//Utils::log("Add tri " + dt.debugDescription());
				tbuffer.AddLast(dt);
	
			}
		}
	
		// NB : Don't remove super triangle here, because all outer triangles are already removed in the addconstraints method.
		//      Uncomment that code if delaunay triangulation ever has to be unconstrained...
	//    TouchSuperTriangle touchSuperTriangle(maxTriangleIndex, maxTriangleIndex+1,maxTriangleIndex+2);
	//	tbuffer.remove_if(touchSuperTriangle);
	//	pointList.pop_back();
	//	pointList.pop_back();
	//	pointList.pop_back();
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void _addConstraints(LinkedList<Triangle>& tbuffer, const List<Ogre::Vector2>& pl, const List<int>& segmentListIndices) const
	private void _addConstraints(ref LinkedList<Triangle> tbuffer, List<Vector2> pl, List<int> segmentListIndices)
#elif DelaunayTriangleBuffer_AlternateDefinition2
	void Triangulator._addConstraints(ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pl, const List<int>& segmentListIndices) const
#endif
#elif PointList_AlternateDefinition2
#if DelaunayTriangleBuffer_AlternateDefinition1
	void Triangulator._addConstraints(ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pl, const List<int>& segmentListIndices) const
#elif DelaunayTriangleBuffer_AlternateDefinition2
	void Triangulator._addConstraints(ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pl, const List<int>& segmentListIndices) const
#endif
#endif
	{
		List<DelaunaySegment> segList = new List<DelaunaySegment>();
		//Utils::log("a co");
		//for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end();it++)
		//	Utils::log(it->debugDescription());
	
		// First, list all the segments that are not already in one of the delaunay triangles
		for (List<int>.Enumerator it2 = segmentListIndices.GetEnumerator(); it2.MoveNext(); it2++)
		{
			int i1 = it2.Current;
			int i2 = it2.Current;
			bool isAlreadyIn = false;
#if DelaunayTriangleBuffer_AlternateDefinition1
			for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext(); ++it)
#elif DelaunayTriangleBuffer_AlternateDefinition2
			for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext(); ++it)
#endif
			{
				if (it.containsSegment(i1,i2))
				{
					isAlreadyIn = true;
					break;
				}
			}
			// only do something for segments not already in DT
			if (!isAlreadyIn)
				segList.Add(new DelaunaySegment(i1, i2));
		}
	
		// Re-Triangulate according to the new segments
		for (List<DelaunaySegment>.Enumerator itSeg =segList.GetEnumerator(); itSeg.MoveNext(); itSeg++)
		{
			//Utils::log("itseg " + StringConverter::toString(itSeg->i1) + "," + StringConverter::toString(itSeg->i2) + " " + StringConverter::toString(pl[itSeg->i1]) + "," + StringConverter::toString(pl[itSeg->i2]));
			// Remove all triangles intersecting the segment and keep a list of outside edges
			std.set<DelaunaySegment> segments = new std.set<DelaunaySegment>();
			Segment2D[] seg1 = new Segment2D[itSeg.i1](pl, pl[itSeg.i2]);
#if DelaunayTriangleBuffer_AlternateDefinition1
			for (LinkedList<Triangle>.Enumerator itTri = tbuffer.GetEnumerator(); itTri.MoveNext();)
#elif DelaunayTriangleBuffer_AlternateDefinition2
			for (LinkedList<Triangle>.Enumerator itTri = tbuffer.GetEnumerator(); itTri.MoveNext();)
#endif
			{
				bool isTriangleIntersected = false;
				bool isDegenerate = false;
				int degenIndex;
				for (int i =0; i<3; i++)
				{
					//Early out if 2 points are in fact the same
					if (itTri.i[i] ==itSeg.i1 || itTri.i[i] ==itSeg.i2 || itTri.i[(i+1)%3] ==itSeg.i1 || itTri.i[(i+1)%3] ==itSeg.i2)
					{
						if (itTri.isDegenerate())
						{
							if (itTri.i[i] ==itSeg.i1 || itTri.i[(i+1)%3] ==itSeg.i1)
								degenIndex = itSeg.i1;
							else if (itTri.i[i] ==itSeg.i2 || itTri.i[(i+1)%3] ==itSeg.i2)
								degenIndex = itSeg.i2;
							isTriangleIntersected = true;
							isDegenerate = true;
						}
						else
							continue;
					}
					Segment2D seg2 = new Segment2D(itTri.p(i), itTri.p((i+1)%3));
					if (seg1.intersects(seg2))
					{
						isTriangleIntersected = true;
						break;
					}
				}
				if (isTriangleIntersected)
				{
					//if (isDegenerate)
					//Utils::log("degen " + itTri->debugDescription());
					for (int k =0; k<3; k++)
					{
						DelaunaySegment[] d1 = new DelaunaySegment[k](itTri.i, itTri.i[(k+1)%3]);
						if (segments.find(d1)!=segments.end())
							segments.erase(d1);
						else if (segments.find(d1.inverse())!=segments.end())
							segments.erase(d1.inverse());
						else
							segments.insert(d1);
					}
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
					itTri =tbuffer.erase(itTri);
				}
				else
			}
	
			// Divide the list of points (coming from remaining segments) in 2 groups : "above" and "below"
			List<int> pointsAbove = new List<int>();
			List<int> pointsBelow = new List<int>();
			int pt = itSeg.i1;
			bool isAbove = true;
			while (segments.size()>0)
			{
				//find next point
				for (std.set<DelaunaySegment>.Enumerator it = segments.begin(); it.MoveNext(); ++it)
				{
					if (it.i1 ==pt || it.i2 ==pt)
					{
						//Utils::log("next " + StringConverter::toString(pt));
	
						if (it.i1 ==pt)
							pt = it.i2;
						else
							pt = it.i1;
						segments.erase(it);
						if (pt ==itSeg.i2)
							isAbove =false;
						else if (pt!=itSeg.i1)
						{
							if (isAbove)
								pointsAbove.Add(pt);
							else
								pointsBelow.Add(pt);
						}
						break;
					}
				}
			}
	
			// Recursively triangulate both polygons
			_recursiveTriangulatePolygon(itSeg.Current, pointsAbove, ref tbuffer, pl);
			_recursiveTriangulatePolygon(itSeg.inverse(), pointsBelow, ref tbuffer, pl);
		}
		// Clean up segments outside of multishape
		if (mRemoveOutside)
		{
			if (mMultiShapeToTriangulate != null && mMultiShapeToTriangulate.isClosed())
			{
#if DelaunayTriangleBuffer_AlternateDefinition1
				for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#elif DelaunayTriangleBuffer_AlternateDefinition2
				for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#endif
				{
					bool isTriangleOut = !mMultiShapeToTriangulate.isPointInside(it.getMidPoint());
	
					if (isTriangleOut)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
						it = tbuffer.erase(it);
					else
				}
			}
			else if (mShapeToTriangulate != null && mShapeToTriangulate.isClosed())
			{
#if DelaunayTriangleBuffer_AlternateDefinition1
				for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#elif DelaunayTriangleBuffer_AlternateDefinition2
				for (LinkedList<Triangle>.Enumerator it = tbuffer.GetEnumerator(); it.MoveNext();)
#endif
				{
					bool isTriangleOut = !mShapeToTriangulate.isPointInside(it.getMidPoint());
	
					if (isTriangleOut)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL list 'erase' method in C#:
						it = tbuffer.erase(it);
					else
				}
			}
		}
	}
	void _recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, List<int> inputPoints, ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pl) const;


	/// Default ctor
	Triangulator() : mShapeToTriangulate(0), mMultiShapeToTriangulate(0), mManualSuperTriangle(0), mRemoveOutside(true), mSegmentListToTriangulate(0)
	{
	}

	/// Sets shape to triangulate
	Triangulator& setShapeToTriangulate(const Shape* shape)
	{
		mShapeToTriangulate = shape;
		mMultiShapeToTriangulate = 0;
		return this;
	}

	/// Sets multi shape to triangulate
	Triangulator& setMultiShapeToTriangulate(const MultiShape* multiShape)
	{
		mMultiShapeToTriangulate = multiShape;
		return this;
	}

	/// Sets segment list to triangulate
	Triangulator& setSegmentListToTriangulate(ref List<Segment2D>* segList)
	{
		mSegmentListToTriangulate = segList;
		return this;
	}

	/// Sets manual super triangle (instead of letting Triangulator guessing it)
	Triangulator& setManualSuperTriangle(Triangle2D* tri)
	{
		mManualSuperTriangle = tri;
		return this;
	}

	/// Sets if the outside of shape must be removed
	Triangulator& setRemoveOutside(bool removeOutside)
	{
		mRemoveOutside = removeOutside;
		return this;
	}

//    *
//	 * Executes the Constrained Delaunay Triangulation algorithm
//	 * @param output A vector of index where is outputed the resulting triangle indexes
//	 * @param outputVertices A vector of vertices where is outputed the resulting triangle vertices
//	 * @exception Ogre::InvalidStateException Either shape or multishape or segment list must be defined
//	 
	void triangulate(ref List<int>& output, ref List<Vector2>& outputVertices) const;

//    *
//	 * Builds the mesh into the given TriangleBuffer
//	 * @param buffer The TriangleBuffer on where to append the mesh.
//	 
	void addToTriangleBuffer(ref TriangleBuffer& buffer) const;
}

//}




//namespace GlobalMembersProceduralTriangulator.Procedural
//{
//-----------------------------------------------------------------------
bool Triangulator.Triangle.isPointInside(const Vector2& point)
{
	// Compute vectors
	Vector2 v0 = p(2) - p(0);
	Vector2 v1 = p(1) - p(0);
	Vector2 v2 = point - p(0);

	// Compute dot products
	float dot00 = v0.squaredLength();
	float dot01 = v0.dotProduct(v1);
	float dot02 = v0.dotProduct(v2);
	float dot11 = v1.squaredLength();
	float dot12 = v1.dotProduct(v2);

	// Compute barycentric coordinates
	float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
	float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
	float v = (dot00 * dot12 - dot01 * dot02) * invDenom;

	// Check if point is in triangle
	return (u >= 0) && (v >= 0) && (u + v - 1 <= 0);
}
//-----------------------------------------------------------------------
Triangulator.Triangle.InsideType Triangulator.Triangle.isPointInsideCircumcircle(const Vector2& pt)
{
	Vector2 v0 =p(0);
	Vector2 v1 =p(1);
	Vector2 v2 =p(2);
	Matrix4 m = new Matrix4(v0.x, v0.y, v0.squaredLength(), 1., v1.x, v1.y, v1.squaredLength(), 1., v2.x, v2.y, v2.squaredLength(), 1., pt.x, pt.y, pt.squaredLength(), 1.);
	float det = m.determinant();
	if (det>=0)
		return IT_INSIDE;
	if (det>-1e-3)
		return IT_BORDERLINEOUTSIDE;
	return IT_OUTSIDE;
}
//-----------------------------------------------------------------------
// Triangulation by insertion
#if PointList_AlternateDefinition1
#if DelaunayTriangleBuffer_AlternateDefinition1
//-----------------------------------------------------------------------
#if PointList_AlternateDefinition1
#if DelaunayTriangleBuffer_AlternateDefinition1
//-----------------------------------------------------------------------
#if PointList_AlternateDefinition1
#if DelaunayTriangleBuffer_AlternateDefinition1
void Triangulator._recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, List<int> inputPoints, ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pointList) const
#elif DelaunayTriangleBuffer_AlternateDefinition2
void Triangulator._recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, List<int> inputPoints, ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pointList) const
#endif
#elif PointList_AlternateDefinition2
#if DelaunayTriangleBuffer_AlternateDefinition1
void Triangulator._recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, List<int> inputPoints, ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pointList) const
#elif DelaunayTriangleBuffer_AlternateDefinition2
void Triangulator._recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, List<int> inputPoints, ref LinkedList<Triangle>& tbuffer, const List<Vector2>& pointList) const
#endif
#endif
{
	if (inputPoints.size() == 0)
		return;
	if (inputPoints.size() ==1)
	{
		Triangle t = new Triangle(pointList);
		t.setVertices(cuttingSeg.i1, cuttingSeg.i2, *inputPoints.begin());
		t.makeDirectIfNeeded();
		tbuffer.AddLast(t);
		return;
	}
	// Find a point which, when associated with seg.i1 and seg.i2, builds a Delaunay triangle
	List<int>.Enumerator currentPoint = inputPoints.begin();
	bool found = false;
	while (!found)
	{
		bool isDelaunay = true;
		Circle[] c = new Circle[currentPoint.Current](pointList, pointList[cuttingSeg.i1], pointList[cuttingSeg.i2]);
		for (List<int>.Enumerator it = inputPoints.begin(); it.MoveNext(); ++it)
		{
			if (c.isPointInside(pointList[it.Current]) && (it.Current != currentPoint.Current))
			{
				isDelaunay = false;
				currentPoint = it;
				break;
			}
		}
		if (isDelaunay)
			found = true;
	}

	// Insert current triangle
	Triangle t = new Triangle(pointList);
	t.setVertices(currentPoint.Current, cuttingSeg.i1, cuttingSeg.i2);
	t.makeDirectIfNeeded();
	tbuffer.AddLast(t);

	// Recurse
	DelaunaySegment newCut1 = new DelaunaySegment(cuttingSeg.i1, currentPoint.Current);
	DelaunaySegment newCut2 = new DelaunaySegment(cuttingSeg.i2, currentPoint.Current);
	List<int> set1 = new List<int>(inputPoints.begin(), currentPoint);
	List<int> set2 = new List<int>(currentPoint+1, inputPoints.end());

	if (!set1.Count == 0)
		_recursiveTriangulatePolygon(newCut1, set1, ref tbuffer, pointList);
	if (!set2.Count == 0)
		_recursiveTriangulatePolygon(newCut2, set2, ref tbuffer, pointList);
}
//-----------------------------------------------------------------------
#if PointList_AlternateDefinition1
void Triangulator.triangulate(ref List<int>& output, ref List<Vector2>& outputVertices) const
#elif PointList_AlternateDefinition2
void Triangulator.triangulate(ref List<int>& output, ref List<Vector2>& outputVertices) const
#endif
{
	if (mShapeToTriangulate == null && mMultiShapeToTriangulate == null && mSegmentListToTriangulate == null)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
		throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Either shape or multishape or segment list must be defined!", "Procedural::Triangulator::triangulate(std::vector<int>&, PointList&)", __FILE__, __LINE__);
		;

	Mogre.Timer mTimer = new Timer();
	mTimer.reset();
#if DelaunayTriangleBuffer_AlternateDefinition1
	LinkedList<Triangle> dtb = new LinkedList<Triangle>();
#elif DelaunayTriangleBuffer_AlternateDefinition2
	LinkedList<Triangle> dtb = new LinkedList<Triangle>();
#endif
	// Do the Delaunay triangulation
	List<int> segmentListIndices = new List<int>();

	if (mShapeToTriangulate != null)
	{
		outputVertices = mShapeToTriangulate.getPoints();
		for (uint i =0; i<mShapeToTriangulate.getSegCount(); ++i)
		{
			segmentListIndices.Add(i);
			segmentListIndices.Add(mShapeToTriangulate.getBoundedIndex(i+1));
		}
	}
	else if (mMultiShapeToTriangulate != null)
	{
		outputVertices = mMultiShapeToTriangulate.getPoints();
		uint index =0;
		for (uint i =0; i<mMultiShapeToTriangulate.getShapeCount(); ++i)
		{
			const Shape shape = mMultiShapeToTriangulate.getShape(i);
			for (uint j =0; j<shape.getSegCount(); j++)
			{
				segmentListIndices.Add(index+j);
				segmentListIndices.Add(index+shape.getBoundedIndex(j+1));
			}
			index+=shape.getSegCount();
		}
	}
	else if (mSegmentListToTriangulate != null)
	{
		std.map<Vector2, int, Vector2Comparator> backMap = new std.map<Vector2, int, Vector2Comparator>();

		for (List<Segment2D>.Enumerator it = mSegmentListToTriangulate.GetEnumerator(); it.MoveNext(); it++)
		{
			if (it.mA.squaredDistance(it.mB)<1e-6)
				continue;

			std.map<Vector2, int, Vector2Comparator>.Enumerator it2 = backMap.find(it.mA);
			if (it2.MoveNext())
			{
				segmentListIndices.Add(it2.second);
			}
			else
			{
				backMap[it.mA] = outputVertices.size();
				segmentListIndices.Add(outputVertices.size());
				outputVertices.push_back(it.mA);
			}

			it2 = backMap.find(it.mB);
			if (it2.MoveNext())
			{
				segmentListIndices.Add(it2.second);
			}
			else
			{
				backMap[it.mB] = outputVertices.size();
				segmentListIndices.Add(outputVertices.size());
				outputVertices.push_back(it.mB);
			}
		}

		if (mManualSuperTriangle != null)
		{
			Triangle superTriangle = new Triangle(outputVertices);
			for (int i =0; i<3; i++)
			{
				std.map<Vector2, int, Vector2Comparator>.Enumerator it = backMap.find(mManualSuperTriangle.mPoints[i]);
				if (it.MoveNext())
				{
					//segmentListIndices.push_back(it->second);
					superTriangle.i[i] = it.second;
				}
				else
				{
					backMap[mManualSuperTriangle.mPoints[i]] = outputVertices.size();
					//segmentListIndices.push_back(outputVertices.size());
					superTriangle.i[i] = outputVertices.size();
					outputVertices.push_back(mManualSuperTriangle.mPoints[i]);
				}
			}

			dtb.AddLast(superTriangle);
		}
	}
	//Utils::log("Triangulator preparation : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
	delaunay(ref outputVertices, ref dtb);
	//Utils::log("Triangulator delaunay : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
	// Add contraints
	_addConstraints(ref dtb, outputVertices, segmentListIndices);
	//Utils::log("Triangulator constraints : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
	//Outputs index buffer
#if DelaunayTriangleBuffer_AlternateDefinition1
	for (LinkedList<Triangle>.Enumerator it = dtb.GetEnumerator(); it.MoveNext(); ++it)
#elif DelaunayTriangleBuffer_AlternateDefinition2
	for (LinkedList<Triangle>.Enumerator it = dtb.GetEnumerator(); it.MoveNext(); ++it)
#endif
		if (!it.isDegenerate())
		{
			output.push_back(it.i[0]);
			output.push_back(it.i[1]);
			output.push_back(it.i[2]);
		}

	// Remove super triangle
	if (mRemoveOutside)
	{
		outputVertices.pop_back();
		outputVertices.pop_back();
		outputVertices.pop_back();
	}
	//Utils::log("Triangulator output : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
}
//-----------------------------------------------------------------------
void Triangulator.addToTriangleBuffer(ref TriangleBuffer& buffer) const
{
#if PointList_AlternateDefinition1
	List<Vector2> pointList = new List<Vector2>();
#elif PointList_AlternateDefinition2
	List<Vector2> pointList = new List<Vector2>();
#endif
	List<int> indexBuffer = new List<int>();
	triangulate(ref indexBuffer, ref pointList);
	for (int j =0; j<pointList.Count; j++)
	{
		Vector2 vp2 = pointList[j];
		Vector3 vp = new Vector3(vp2.x, vp2.y, 0);
		Vector3 normal = -Vector3.UNIT_Z;

		addPoint(buffer, vp, normal, new Vector2(vp2.x, vp2.y));
	}

	for (int i =0; i<indexBuffer.Count/3; i++)
	{
		buffer.index(indexBuffer[i *3]);
		buffer.index(indexBuffer[i *3+2]);
		buffer.index(indexBuffer[i *3+1]);
	}

}
}
