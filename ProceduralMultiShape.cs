using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
public static class GlobalMembersProceduralMultiShape
{
	//-----------------------------------------------------------------------

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool MultiShape::isPointInside(const Vector2& point) const
	public static bool MultiShape.isPointInside(Vector2 point)
	{
		// Draw a horizontal lines that goes through "point"
		// Using the closest intersection, find whether the point is actually inside
		int closestSegmentIndex = -1;
		float closestSegmentDistance = std.numeric_limits<float>.max();
		Vector2 closestSegmentIntersection = new Vector2();
		 Shape closestSegmentShape = 0;

		for (int k = 0; k < mShapes.size(); k++)
		{
			const Shape shape = mShapes[k];
			for (int i = 0; i < shape.getSegCount(); i++)
			{
				Vector2 A = shape.getPoint(i);
				Vector2 B = shape.getPoint(i + 1);
				if (A.y != B.y && (A.y - point.y)*(B.y - point.y) <= 0.)
				{
					Vector2 intersect = new Vector2(A.x + (point.y - A.y)*(B.x - A.x) / (B.y - A.y), point.y);
					float dist = Math.Abs(point.x - intersect.x);
					if (dist < closestSegmentDistance)
					{
						closestSegmentIndex = i;
						closestSegmentDistance = dist;
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: closestSegmentIntersection = intersect;
						closestSegmentIntersection.CopyFrom(intersect);
						closestSegmentShape = shape;
					}
				}
			}
		}
		if (closestSegmentIndex != -1)
		{
			int edgePoint = -1;
			if (closestSegmentIntersection.squaredDistance(closestSegmentShape.getPoint(closestSegmentIndex)) < 1e-8)
				edgePoint = closestSegmentIndex;
			else if (closestSegmentIntersection.squaredDistance(closestSegmentShape.getPoint(closestSegmentIndex + 1)) < 1e-8)
				edgePoint = closestSegmentIndex + 1;
			if (edgePoint>-1)
			{
				Mogre.Radian alpha1 = Utils.angleBetween(point - closestSegmentShape.getPoint(edgePoint), closestSegmentShape.getDirectionAfter(edgePoint));
				Mogre.Radian alpha2 = Utils.angleBetween(point - closestSegmentShape.getPoint(edgePoint), -closestSegmentShape.getDirectionBefore(edgePoint));
				if (alpha1 < alpha2)
					closestSegmentIndex = edgePoint;
				else
					closestSegmentIndex = edgePoint - 1;
			}
			return (closestSegmentShape.getNormalAfter(closestSegmentIndex).x * (point.x - closestSegmentIntersection.x) < 0);
		}
		// We're in the case where the point is on the "float outside" of the multishape
		// So, if the float outside == user defined outside, then the point is "user-defined outside"
		return !isOutsideRealOutside();
	}
}

//C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
//class Shape;

//*
// * \ingroup shapegrp
// * Holds a bunch of shapes.
// * There are a number of assumptions that are made and are not checked
// * against : the shapes must not cross each other
// *
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport MultiShape
public class MultiShape
{
	private List<Shape> mShapes = new List<Shape>();

	/// Default constructor
	public MultiShape()
	{
	}

	/// Constructor from a single shape
	public MultiShape(Shape shape)
	{
		mShapes.Add(shape);
	}

	/// Constructor from a variable number of shapes
	/// @param count the number of shapes to add
	/// @param ... pointer to the shapes to add
	//-----------------------------------------------------------------------
	
	public MultiShape(int count, params object[] ParamArray)
	{
//		va_list shapes;
		int ParamCount = -1;
//		va_start(shapes, count);
		for (int i = 0; i < count; i++)
		{
			ParamCount++;
			mShapes.Add(*ParamArray[ParamCount]);
		}
	
//		va_end(shapes);
	}

	/// Adds a shape to the list of shapes
	public MultiShape addShape(Shape shape)
	{
		mShapes.Add(shape);
		return this;
	}

	/// Clears all the content
	public void clear()
	{
		mShapes.Clear();
	}

	/// Returns the i-th shape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Shape& getShape(uint i) const
	public Shape getShape(uint i)
	{
		return mShapes[i];
	}

	/// Returns the i-th shape
	public Shape getShape(uint i)
	{
		return mShapes[i];
	}

	/// Builds an aggregated list of all points contained in all shapes
	//-----------------------------------------------------------------------
	
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: List<Vector2> getPoints() const
	public List<Vector2> getPoints()
	{
		List<Vector2> result = new List<Vector2>();
		for (int i = 0; i < mShapes.Count; i++)
		{
			List<Vector2> points = mShapes[i].getPoints();
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
			result.insert(result.end(), points.GetEnumerator(), points.end());
		}
		return result;
	}

	/// Returns the number of shapes in that MultiShape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getShapeCount() const
	public uint getShapeCount()
	{
		return mShapes.Count;
	}

	/// Append every shape of an other multishape to the current multiShape
	public void addMultiShape(MultiShape STLAllocator<U, AllocPolicy>)
	{
		for (List<Shape>.Enumerator it = STLAllocator<U, AllocPolicy>.mShapes.GetEnumerator(); it!=STLAllocator<U, AllocPolicy>.mShapes.end(); ++it)
		{
			mShapes.Add(it.Current);
		}
	}

	/// Outputs the Multi Shape to a Mesh, mostly for visualisation or debugging purposes
	//-----------------------------------------------------------------------
	
	public MeshPtr realizeMesh()
	{
		return realizeMesh("");
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: MeshPtr realizeMesh(const string& name ="")
	public MeshPtr realizeMesh(string name)
	{
		Mogre.SceneManager smgr = Root.getSingleton().getSceneManagerIterator().begin().second;
		ManualObject manual = smgr.createManualObject(name);
	
		for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
		{
			manual.begin("BaseWhiteNoLighting", RenderOperation.OperationType.OT_LINE_STRIP);
			it._appendToManualObject(manual);
			manual.end();
		}
	
		MeshPtr mesh = new MeshPtr();
		if (name == "")
			mesh = manual.convertToMesh(Utils.getName());
		else
			mesh = manual.convertToMesh(name);
		smgr.destroyManualObject(manual);
		return mesh;
	}

	/// Tells whether a point is located inside that multishape
	/// It assumes that all of the shapes in that multishape are closed,
	/// and that they don't contradict each other,
	/// ie a point cannot be outside and inside at the same time
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& point) const;
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	bool isPointInside(Ogre::Vector2 point);

//    *
//	 * Tells whether multishape is "closed" or not.
//	 * MultiShape is considered to be closed if and only if all shapes are closed
//	 
	//-----------------------------------------------------------------------
	
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isClosed() const
	public bool isClosed()
	{
		for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
		{
			if (!it.isClosed())
				return false;
		}
		return true;
	}

//    *
//	 * Closes all shapes included in this multiShape
//	 
	//-----------------------------------------------------------------------
	
	public void close()
	{
		for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
		{
			it.close();
		}
	}

//    *
//	 * Determines whether the outside as defined by user equals "real" outside
//	 

	//-----------------------------------------------------------------------
	
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isOutsideRealOutside() const
	public bool isOutsideRealOutside()
	{
		float x = std.numeric_limits<float>.min();
		int index = 0;
		int shapeIndex = 0;
		for (int j = 0; j < mShapes.Count; j++)
		{
			const Shape s = mShapes[j];
			const List<Vector2> points = s.getPointsReference();
			for (int i = 0; i < points.Count; i++)
			{
				if (x < points[i].x)
				{
					x = points[i].x;
					index = i;
					shapeIndex = j;
				}
			}
		}
		Radian alpha1 = Utils.angleTo(Vector2.UNIT_Y, mShapes[shapeIndex].getDirectionAfter(index));
		Radian alpha2 = Utils.angleTo(Vector2.UNIT_Y, -mShapes[shapeIndex].getDirectionBefore(index));
		Side shapeSide;
		if (alpha1 < alpha2)
			shapeSide = SIDE_RIGHT;
		else
			shapeSide = SIDE_LEFT;
		return shapeSide == mShapes[shapeIndex].getOutSide();
	}

	//-----------------------------------------------------------------------
	public void buildFromSegmentSoup(List<Segment2D> segList)
	{
		std.multimap<Vector2, Vector2, Vector2Comparator> segs = new std.multimap<Vector2, Vector2, Vector2Comparator>();
		for (List<Segment2D>.Enumerator it = segList.GetEnumerator(); it.MoveNext(); ++it)
		{
			segs.insert(std.pair<Vector2, Vector2 > (it.mA, it.mB));
			segs.insert(std.pair<Vector2, Vector2 > (it.mB, it.mA));
		}
		while (!segs.empty())
		{
			Vector2 headFirst = segs.begin().first;
			Vector2 headSecond = segs.begin().second;
			Shape s = new Shape();
			s.addPoint(headFirst).addPoint(headSecond);
			std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator firstSeg = segs.begin();
			std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants2 = segs.equal_range(headSecond);
			for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants2.first; it != correspondants2.second;)
			{
				std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
				if ((removeIt.second - firstSeg.first).squaredLength() < 1e-8)
					segs.erase(removeIt);
			}
			segs.erase(firstSeg);
			bool foundSomething = true;
			while (!segs.empty() && foundSomething)
			{
				foundSomething = false;
				std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator next = segs.find(headSecond);
				if (next != segs.end())
				{
					foundSomething = true;
					headSecond = next.second;
					s.addPoint(headSecond);
					std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants = segs.equal_range(headSecond);
					for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants.first; it != correspondants.second;)
					{
						std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
						if ((removeIt.second - next.first).squaredLength() < 1e-8)
							segs.erase(removeIt);
					}
					segs.erase(next);
				}
				std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator previous = segs.find(headFirst);
				if (previous != segs.end())
				{
					foundSomething = true;
					s.insertPoint(0, previous.second);
					headFirst = previous.second;
					std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants = segs.equal_range(headFirst);
					for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants.first; it != correspondants.second;)
					{
						std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
						if ((removeIt.second - previous.first).squaredLength() < 1e-8)
							segs.erase(removeIt);
					}
					segs.erase(previous);
				}
			}
			if (s.getPoint(0).squaredDistance(s.getPoint(s.getSegCount() + 1)) < 1e-6)
			{
				s.getPointsReference().pop_back();
				s.close();
			}
			addShape(s);
		}
	}

}
}
