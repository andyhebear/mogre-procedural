using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
public static class GlobalMembersProceduralShape
{
	//-----------------------------------------------------------------------
	public static bool _sortAngles(std.pair<Radian, byte> one, std.pair<Radian, byte> two) // waiting for lambda functions!
	{
		return one.first<two.first;
	}
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool Shape::isPointInside(const Vector2& point) const
	public static bool Shape.isPointInside(Vector2 point)
	{
		// Draw a horizontal lines that goes through "point"
		// Using the closest intersection, find whethe the point is actually inside
		int closestSegmentIndex =-1;
		float closestSegmentDistance = std.numeric_limits<Real>.max();
		Vector2 closestSegmentIntersection = new Vector2(Vector2.ZERO);

		for (int i =0; i<getSegCount(); i++)
		{
			Vector2 A = getPoint(i);
			Vector2 B = getPoint(i+1);
			if (A.y!=B.y && (A.y-point.y)*(B.y-point.y)<=0.)
			{
				Vector2 intersect = new Vector2(A.x+(point.y-A.y)*(B.x-A.x)/(B.y-A.y), point.y);
				float dist = Math.Abs(point.x-intersect.x);
				if (dist<closestSegmentDistance)
				{
					closestSegmentIndex = i;
					closestSegmentDistance = dist;
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: closestSegmentIntersection = intersect;
					closestSegmentIntersection.CopyFrom(intersect);
				}
			}
		}
		if (closestSegmentIndex!=-1)
		{
			if (getNormalAfter(closestSegmentIndex).x * (point.x-closestSegmentIntersection.x)<0)
				return true;
			else
				return false;
		}
		if (findRealOutSide() == mOutSide)
			return false;
		else
			return true;
	}
}

public enum Side: int
{
	SIDE_LEFT,
	SIDE_RIGHT
}

//* @} 

//*
// * \ingroup shapegrp
// * Describes a succession of interconnected 2D points.
// * It can be closed or not, and there's always an outside and an inside
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Shape
public class Shape
{
	private List<Vector2> mPoints = new List<Vector2>();
	private bool mClosed;
	private Side mOutSide;

	/// Default constructor
	public Shape()
	{
		mClosed = false;
		mOutSide = SIDE_RIGHT;
	}

	/// Adds a point to the shape
	public Shape addPoint(Vector2 pt)
	{
		mPoints.Add(pt);
		return this;
	}

	/// Adds a point to the shape
	public Shape addPoint(float x, float y)
	{
		mPoints.Add(new Vector2(x, y));
		return this;
	}

	/// Inserts a point to the shape
	/// @param index the index before the inserted point
	/// @param x new point's x coordinate
	/// @param y new point's y coordinate
	public Shape insertPoint(int index, float x, float y)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.GetEnumerator()+index, Vector2(x, y));
		return this;
	}

	/// Inserts a point to the shape
	/// @param index the index before the inserted point
	/// @param pt new point's position
	public Shape insertPoint(int index, Vector2 pt)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.GetEnumerator()+index, pt);
		return this;
	}

	/// Adds a point to the shape, relative to the last point added
	public Shape addPointRel(Vector2 pt)
	{
		if (mPoints.Count == 0)
			mPoints.Add(pt);
		else
			mPoints.Add(pt + *(mPoints.end()-1));
		return this;
	}

	/// Adds a point to the shape, relative to the last point added
	public Shape addPointRel(float x, float y)
	{
		if (mPoints.Count == 0)
			mPoints.Add(new Vector2(x, y));
		else
			mPoints.Add(new Vector2(x, y) + *(mPoints.end()-1));
		return this;
	}

	/// Appends another shape at the end of this one
	public Shape appendShape(Shape STLAllocator<U, AllocPolicy>)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.end(), STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
		return this;
	}

	/// Appends another shape at the end of this one, relative to the last point of this shape
	public Shape appendShapeRel(Shape STLAllocator<U, AllocPolicy>)
	{
		if (mPoints.Count == 0)
			appendShape(STLAllocator<U, AllocPolicy>);
		else
		{
			Vector2 refVector = *(mPoints.end()-1);
			List<Vector2> pointList = new List<Vector2>(STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
			for (List<Vector2>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
				it.Current +=refVector;
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
			mPoints.insert(mPoints.end(), pointList.GetEnumerator(), pointList.end());
		}
		return this;
	}

	/// Extracts a part of the shape as a new shape
	/// @param first first index to be in the new shape
	/// @param last last index to be in the new shape
	public Shape extractSubShape(uint first, uint last)
	{
		Shape s = new Shape();
		for (uint i =first; i<=last; i++)
			s.addPoint(mPoints[i]);
		s.setOutSide(mOutSide);
		if (mClosed)
			s.close();
		return s;
	}

	/// Reverses direction of the shape
	/// The outside is preserved
	public Shape reverse()
	{
		std.reverse(mPoints.GetEnumerator(), mPoints.end());
		switchSide();
		return this;
	}

	/// Clears the content of the shape
	public Shape reset()
	{
		mPoints.Clear();
		return this;
	}

	/// Converts the shape to a path, with Y=0
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Path convertToPath() const
	public Path convertToPath()
	{
		Path p = new Path();
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			p.addPoint(it.x, 0, it.y);
		}
		if (mClosed)
			p.close();
	
		return p;
	}

	/// Outputs a track, with Key=X and Value=Y
	//-----------------------------------------------------------------------
	public Track convertToTrack()
	{
		return convertToTrack(Track.AM_RELATIVE_LINEIC);
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Track convertToTrack(Track::AddressingMode addressingMode =Track::AM_RELATIVE_LINEIC) const
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
	public Track convertToTrack(Track.AddressingMode addressingMode)
	{
		Track t = new Track(addressingMode);
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			t.addKeyFrame(it.x, it.y);
		}
		return t;
	}

	/// Gets a copy of raw vector data of this shape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline List<Ogre::Vector2> getPoints() const
	public List<Vector2> getPoints()
	{
		return mPoints;
	}

	/// Gets raw vector data of this shape as a non-const reference
	public List<Vector2> getPointsReference()
	{
		return mPoints;
	}

	/// Gets raw vector data of this shape as a non-const reference
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline const List<Ogre::Vector2>& getPointsReference() const
	public List<Vector2> getPointsReference()
	{
		return mPoints;
	}

//    *
//	 * Bounds-safe method to get a point : it will allow you to go beyond the bounds
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline const Ogre::Vector2& getPoint(int i) const
	public Vector2 getPoint(int i)
	{
		return mPoints[getBoundedIndex(i)];
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline uint getBoundedIndex(int i) const
	public uint getBoundedIndex(int i)
	{
		if (mClosed)
			return Utils.modulo(i, mPoints.Count);
		return Utils.cap(i, 0, mPoints.Count-1);
	}

	/// Gets number of points in current point list
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline const List<Ogre::Vector2>::size_type getPointCount() const
	public List<Vector2>.size_type getPointCount()
	{
		return mPoints.Count;
	}

//    *
//	 * Makes the shape a closed shape, ie it will automatically connect
//	 * the last point to the first point.
//	 
	public Shape close()
	{
		mClosed = true;
		return this;
	}

//    *
//	 * Sets which side (left or right) is on the outside of the shape.
//	 * It is used for such things as normal generation
//	 * Default is right, which corresponds to placing points anti-clockwise.
//	 
	public Shape setOutSide(Side side)
	{
		mOutSide = side;
		return this;
	}

	/// Gets which side is out
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Side getOutSide() const
	public Side getOutSide()
	{
		return mOutSide;
	}

	/// Switches the inside and the outside
	public Shape switchSide()
	{
		mOutSide = (mOutSide == SIDE_LEFT)? SIDE_RIGHT: SIDE_LEFT;
		return this;
	}

	/// Gets the number of segments in that shape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline int getSegCount() const
	public int getSegCount()
	{
		return (mPoints.Count-1) + (mClosed?1:0);
	}

	/// Gets whether the shape is closed or not
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline bool isClosed() const
	public bool isClosed()
	{
		return mClosed;
	}

//    *
//	 * Returns local direction after the current point
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getDirectionAfter(uint i) const
	public Vector2 getDirectionAfter(uint i)
	{
		// If the path isn't closed, we get a different calculation at the end, because
		// the tangent shall not be null
		if (! mClosed && i == mPoints.Count - 1 && i > 0)
			return (mPoints[i] - mPoints[i-1]).normalisedCopy();
		else
			return (getPoint(i+1) - getPoint(i)).normalisedCopy();
	}

//    *
//	 * Returns local direction after the current point
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getDirectionBefore(uint i) const
	public Vector2 getDirectionBefore(uint i)
	{
		// If the path isn't closed, we get a different calculation at the end, because
		// the tangent shall not be null
		if (!mClosed && i == 1)
			return (mPoints[1] - mPoints[0]).normalisedCopy();
		else
			return (getPoint(i) - getPoint(i-1)).normalisedCopy();
	}

	/// Gets the average between before direction and after direction
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getAvgDirection(uint i) const
	public Vector2 getAvgDirection(uint i)
	{
		return (getDirectionAfter(i) + getDirectionBefore(i)).normalisedCopy();
	}

	/// Gets the shape normal just after that point
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getNormalAfter(uint i) const
	public Vector2 getNormalAfter(uint i)
	{
		if (mOutSide ==SIDE_RIGHT)
			return -getDirectionAfter(i).perpendicular();
		return getDirectionAfter(i).perpendicular();
	}

	/// Gets the shape normal just before that point
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getNormalBefore(uint i) const
	public Vector2 getNormalBefore(uint i)
	{
		if (mOutSide ==SIDE_RIGHT)
			return -getDirectionBefore(i).perpendicular();
		return getDirectionBefore(i).perpendicular();
	}

	/// Gets the "normal" of that point ie an average between before and after normals
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getAvgNormal(uint i) const
	public Vector2 getAvgNormal(uint i)
	{
		if (mOutSide ==SIDE_RIGHT)
			return -getAvgDirection(i).perpendicular();
		return getAvgDirection(i).perpendicular();
	}

//    *
//	 * Outputs a mesh representing the shape.
//	 * Mostly for debugging purposes
//	 
	//-----------------------------------------------------------------------
	public MeshPtr realizeMesh()
	{
		return realizeMesh("");
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MeshPtr realizeMesh(const string& name ="") const
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
	public MeshPtr realizeMesh(string name)
	{
		Mogre.SceneManager smgr = Root.getSingleton().getSceneManagerIterator().begin().second;
		ManualObject manual = smgr.createManualObject();
		manual.begin("BaseWhiteNoLighting", RenderOperation.OperationType.OT_LINE_STRIP);
	
		_appendToManualObject(manual);
	
		manual.end();
		MeshPtr mesh = new MeshPtr();
		if (name =="")
			mesh = manual.convertToMesh(Utils.getName());
		else
			mesh = manual.convertToMesh(name);
		smgr.destroyManualObject(manual);
		return mesh;
	}

//    *
//	 * Appends the shape vertices to a manual object being edited
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void _appendToManualObject(ManualObject* manual) const
	public void _appendToManualObject(ManualObject manual)
	{
		for (List<Vector2>.Enumerator itPos = mPoints.GetEnumerator(); itPos.MoveNext(); itPos++)
			manual.position(new Vector3(itPos.x, itPos.y, 0.0f));
		if (mClosed)
			manual.position(new Vector3(mPoints.GetEnumerator().x, mPoints.GetEnumerator().y, 0.0f));
	}

//    *
//	 * Tells whether a point is inside a shape or not
//	 * @param point The point to check
//	 * @return true if the point is inside this shape, false otherwise
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& point) const;
//C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
//	bool isPointInside(Ogre::Vector2 point);

//    *
//	 * Computes the intersection between this shape and another one.
//	 * Both shapes must be closed.
//	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleanintersection.png "Intersection of the two shapes"</td></tr></table>
//	 * @param other The shape against which the intersection is computed
//	 * @return The intersection of two shapes, as a new shape
//	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
//	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MultiShape booleanIntersect(const Shape& STLAllocator<U, AllocPolicy>) const
	public MultiShape booleanIntersect(Shape STLAllocator<U, AllocPolicy>)
	{
		return _booleanOperation(STLAllocator<U, AllocPolicy>, BOT_INTERSECTION);
	}

//    *
//	 * Computes the union between this shape and another one.
//	 * Both shapes must be closed.
//	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleanunion.png "Union of the two shapes"</td></tr></table>
//	 * @param other The shape against which the union is computed
//	 * @return The union of two shapes, as a new shape
//	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
//	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MultiShape booleanUnion(const Shape& STLAllocator<U, AllocPolicy>) const
	public MultiShape booleanUnion(Shape STLAllocator<U, AllocPolicy>)
	{
		return _booleanOperation(STLAllocator<U, AllocPolicy>, BOT_UNION);
	}

//    *
//	 * Computes the difference between this shape and another one.
//	 * Both shapes must be closed.
//	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleandifference.png "Difference of the two shapes"</td></tr></table>
//	 * @param other The shape against which the diffenrence is computed
//	 * @return The difference of two shapes, as a new shape
//	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
//	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MultiShape booleanDifference(const Shape& STLAllocator<U, AllocPolicy>) const
	public MultiShape booleanDifference(Shape STLAllocator<U, AllocPolicy>)
	{
		return _booleanOperation(STLAllocator<U, AllocPolicy>, BOT_DIFFERENCE);
	}

//    *
//	 * On a closed shape, find if the outside is located on the right
//	 * or on the left. If the outside can easily be guessed in your context,
//	 * you'd rather use setOutside(), which doesn't need any computation.
//	 

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Side findRealOutSide() const
	public Side findRealOutSide()
	{
		float x = mPoints[0].x;
		int index =0;
		for (ushort i =1; i<mPoints.Count; i++)
		{
			if (x < mPoints[i].x)
			{
				x = mPoints[i].x;
				index = i;
			}
		}
		Radian alpha1 = Utils.angleTo(Vector2.UNIT_Y, getDirectionAfter(index));
		Radian alpha2 = Utils.angleTo(Vector2.UNIT_Y, -getDirectionBefore(index));
		if (alpha1<alpha2)
			return SIDE_RIGHT;
		else
			return SIDE_LEFT;
	}

//    *
//	 * Determines whether the outside as defined by user equals "real" outside
//	
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isOutsideRealOutside() const
	public bool isOutsideRealOutside()
	{
		return findRealOutSide() == mOutSide;
	}

	/// Creates a shape with the keys of this shape and extra keys coming from a track
	/// @param track the track to merge keys with
	/// @return a new Shape coming from the merge between original shape and the track
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Shape mergeKeysWithTrack(const Track& track) const
	public Shape mergeKeysWithTrack(Track track)
	{
		if (!track.isInsertPoint() || track.getAddressingMode() == Track.AM_POINT)
			return this;
		float totalLength =getTotalLength();
	
		float lineicPos = 0;
		float shapeLineicPos = 0;
		Shape outputShape = new Shape();
		if (mClosed)
			outputShape.close();
		outputShape.addPoint(getPoint(0));
		for (uint i = 1; i < mPoints.Count;)
		{
			float nextLineicPos = shapeLineicPos + (mPoints[i] - mPoints[i-1]).length();
	
			std.map<Real,Real>.Enumerator it = track._getKeyValueAfter(lineicPos, lineicPos/totalLength, i-1);
	
			float nextTrackPos = it.first;
			if (track.getAddressingMode() == Track.AM_RELATIVE_LINEIC)
				nextTrackPos *= totalLength;
	
			// Adds the closest point to the curve, being either from the shape or the track
			if (nextLineicPos<=nextTrackPos || lineicPos>=nextTrackPos)
			{
				outputShape.addPoint(mPoints[i]);
				i++;
				lineicPos = nextLineicPos;
				shapeLineicPos = nextLineicPos;
			}
			else
			{
				outputShape.addPoint(getPosition(i-1, (nextTrackPos-shapeLineicPos)/(nextLineicPos-shapeLineicPos)));
				lineicPos = nextTrackPos;
			}
		}
		return outputShape;
	}

//    *
//	 * Applies the given translation to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param translation the translation vector
//	 
	public Shape translate(Vector2 translation)
	{
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
			it.Current+=translation;
		return this;
	}

//    *
//	 * Applies the given translation to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param translationX X component of the translation vector
//	 * @param translationY Y component of the translation vector
//	 
	public Shape translate(float translationX, float translationY)
	{
		return translate(Vector2(translationX, translationY));
	}

//    *
//	 * Applies the given rotation to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param angle angle of rotation
//	 
	public Shape rotate(Mogre.Radian angle)
	{
		float c = Math.Cos(angle.valueRadians());
		float s = Math.Sin(angle.valueRadians());
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			float x = it.x;
			float y = it.y;
			it.x = c * x - s * y;
			it.y = s * x + c * y;
		}
		return this;
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param amount amount of scale
//	 
	public Shape scale(float amount)
	{
		return scale(amount, amount);
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param scaleX amount of scale in the X direction
//	 * @param scaleY amount of scale in the Y direction
//	 
	public Shape scale(float scaleX, float scaleY)
	{
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			it.x *= scaleX;
			it.y *= scaleY;
		}
		return this;
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param amount of scale
//	 
	public Shape scale(Vector2 amount)
	{
		return scale(amount.x, amount.y);
	}

//    *
//	 * Reflect all points in this shape against a zero-origined line with a given normal
//	 * @param normal the normal
//	 
	public Shape reflect(Vector2 normal)
	{
		for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			it.Current = it.reflect(normal);
		}
		return this;
	}

//    *
//	 * Create a symetric copy at the origin point.
//	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
//	 
	public Shape mirror()
	{
		return mirror(false);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: Shape& mirror(bool flip = false)
	public Shape mirror(bool flip)
	{
		return mirrorAroundPoint(Vector2.ZERO, flip);
	}

//    *
//	 * Create a symetric copy at a given point.
//	 * @param x x coordinate of point where to mirror
//	 * @param y y coordinate of point where to mirror
//	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
//	 
	public Shape mirror(float x, float y)
	{
		return mirror(x, y, false);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: Shape& mirror(Ogre::float x, Ogre::float y, bool flip = false)
	public Shape mirror(float x, float y, bool flip)
	{
		return mirrorAroundPoint(Vector2(x, y), flip);
	}

//    *
//	 * Create a symetric copy at a given point.
//	 * @param point Point where to mirror
//	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
//	 
	public Shape mirrorAroundPoint(Vector2 point)
	{
		return mirrorAroundPoint(point, false);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: Shape& mirrorAroundPoint(Ogre::Vector2 point, bool flip = false)
	public Shape mirrorAroundPoint(Vector2 point, bool flip)
	{
		int l = (int)mPoints.Count;
		if (flip)
			for (int i = l - 1; i >= 0; i--)
			{
				Vector2 pos = mPoints[i] - point;
				mPoints.Add(-1.0 * pos + point);
			}
		else
			for (int i = 0; i < l; i++)
			{
				Vector2 pos = mPoints[i] - point;
				mPoints.Add(-1.0 * pos + point);
			}
		return this;
	}

//    *
//	 * Create a symetric copy at a given axis.
//	 * @param axis Axis where to mirror
//	 * @param flip \c true if function should start mirroring with the first point in list (default \c false)
//	 
	public Shape mirrorAroundAxis(Vector2 axis)
	{
		return mirrorAroundAxis(axis, false);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: Shape& mirrorAroundAxis(const Ogre::Vector2& axis, bool flip = false)
	public Shape mirrorAroundAxis(Vector2 axis, bool flip)
	{
		int l = (int)mPoints.Count;
		Vector2 normal = axis.perpendicular().normalisedCopy();
		if (flip)
			for (int i = 0; i < l; i++)
			{
				Vector2 pos = mPoints[i];
				pos = pos.reflect(normal);
				if (pos != mPoints[i])
					mPoints.Add(pos);
			}
		else
			for (int i = l - 1; i >= 0; i--)
			{
				Vector2 pos = mPoints[i];
				pos = pos.reflect(normal);
				if (pos != mPoints[i])
					mPoints.Add(pos);
			}
		return this;
	}

	/// Returns the total lineic length of that shape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float getTotalLength() const
	public float getTotalLength()
	{
		float length = 0;
		for (uint i =0; i<mPoints.Count-1; i++)
			length+=(mPoints[i+1]-mPoints[i]).length();
		if (mClosed)
			length+=(mPoints[mPoints.Count - 1]-*mPoints.GetEnumerator()).length();
		return length;
	}

	/// Gets a position on the shape with index of the point and a percentage of position on the segment
	/// @param i index of the segment
	/// @param coord a number between 0 and 1 meaning the percentage of position on the segment
	/// @exception Ogre::InvalidParametersException i is out of bounds
	/// @exception Ogre::InvalidParametersException coord must be comprised between 0 and 1
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getPosition(uint i, Ogre::float coord) const
	public Vector2 getPosition(uint i, float coord)
	{
		if (!mClosed || i >= mPoints.Count)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Out of Bounds", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
            throw new Exception("Out of Bounds");			
;
		if (coord < 0.0f || coord > 1.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Coord must be comprised between 0 and 1", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
        throw new Exception("Coord must be comprised between 0 and 1");			
;
		Vector2 A = getPoint(i);
		Vector2 B = getPoint(i+1);
		return A + coord*(B-A);
	}

	/// Gets a position on the shape from lineic coordinate
	/// @param coord lineic coordinate
	/// @exception Ogre::InvalidStateException The shape must at least contain 2 points
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector2 getPosition(Ogre::float coord) const
	public Vector2 getPosition(float coord)
	{
		if (mPoints.Count < 2)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The shape must at least contain 2 points", "Procedural::Shape::getPosition(Ogre::Real)", __FILE__, __LINE__);
        throw new Exception("shape must at least contain 2 points");				
;
		uint i =0;
		while (true)
		{
			float nextLen = (getPoint(i+1) - getPoint(i)).length();
			if (coord>nextLen)
				coord-=nextLen;
			else
				return getPosition(i, coord);
			if (!mClosed && i>= mPoints.Count-2)
				return mPoints[mPoints.Count - 1];
			i++;
		}
	}

	/// Computes the radius of a bounding circle centered on the origin
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float findBoundingRadius() const
	public float findBoundingRadius()
	{
		float sqRadius =0.0f;
		for (uint i =0; i<mPoints.Count; i++)
			sqRadius =Math.Max(sqRadius,mPoints[i].squaredLength());
		return Math.Sqrt(sqRadius);
	}

//    *
//	 * Applies a "thickness" to a shape, ie a bit like the extruder, but in 2D
//	 * <table border="0" width="100%"><tr><td>\image html shape_thick1.png "Start shape (before thicken)"</td><td>\image html shape_thick2.png "Result (after thicken)"</td></tr></table>
//	 
	//-----------------------------------------------------------------------
	public MultiShape thicken(float amount)
	{
		if (!mClosed)
		{
			Shape s = new Shape();
			s.setOutSide(mOutSide);
			for (uint i =0; i<mPoints.Count; i++)
				s.addPoint(mPoints[i]+amount *getAvgNormal(i));
			for (int i =mPoints.Count-1; i>=0; i--)
				s.addPoint(mPoints[i]-amount *getAvgNormal(i));
			s.close();
			return new MultiShape().addShape(s);
		}
		else
		{
			MultiShape ms = new MultiShape();
			Shape s1 = new Shape();
			for (uint i =0; i<mPoints.Count; i++)
				s1.addPoint(mPoints[i]+amount *getAvgNormal(i));
			s1.close();
			s1.setOutSide(mOutSide);
			ms.addShape(s1);
			Shape s2 = new Shape();
			for (uint i =0; i<mPoints.Count; i++)
				s2.addPoint(mPoints[i]-amount *getAvgNormal(i));
			s2.close();
			s2.setOutSide(mOutSide ==SIDE_LEFT?SIDE_RIGHT:SIDE_LEFT);
			ms.addShape(s2);
			return ms;
		}
	}


	private enum BooleanOperationType: int
	{
		BOT_UNION,
		BOT_INTERSECTION,
		BOT_DIFFERENCE
	}

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MultiShape _booleanOperation(const Shape& STLAllocator<U, AllocPolicy>, BooleanOperationType opType) const
	private MultiShape _booleanOperation(Shape STLAllocator<U, AllocPolicy>, BooleanOperationType opType)
	{
		if (!mClosed || mPoints.Count < 2)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Current shapes must be closed and has to contain at least 2 points!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
            throw new Exception("shape must at least contain 2 points");				
;
		if (!STLAllocator<U, AllocPolicy>.mClosed || STLAllocator<U, AllocPolicy>.mPoints.Count < 2)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Other shapes must be closed and has to contain at least 2 points!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
            throw new Exception("Other shapes must be closed and has to contain at least 2 points!");				
;
	
		// Compute the intersection between the 2 shapes
		List<IntersectionInShape> intersections = new List<IntersectionInShape>();
		_findAllIntersections(STLAllocator<U, AllocPolicy>, intersections);
	
		// Build the resulting shape
		if (intersections.Count == 0)
		{
			if (isPointInside(STLAllocator<U, AllocPolicy>.getPoint(0)))
			{
				// Shape B is completely inside shape A
				if (opType == BOT_UNION)
					return this;
				else if (opType == BOT_INTERSECTION)
					return STLAllocator<U, AllocPolicy>;
				else if (opType == BOT_DIFFERENCE)
				{
					MultiShape ms = new MultiShape();
					ms.addShape(this);
					ms.addShape(STLAllocator<U, AllocPolicy>);
					ms.getShape(1).switchSide();
					return ms;
				}
	
			}
			else if (STLAllocator<U, AllocPolicy>.isPointInside(getPoint(0)))
			{
				// Shape A is completely inside shape B
				if (opType == BOT_UNION)
					return STLAllocator<U, AllocPolicy>;
				else if (opType == BOT_INTERSECTION)
					return this;
				else if (opType == BOT_DIFFERENCE)
				{
					MultiShape ms = new MultiShape();
					ms.addShape(this);
					ms.addShape(STLAllocator<U, AllocPolicy>);
					ms.getShape(0).switchSide();
					return ms;
				}
			}
			else
			{
				if (opType == BOT_UNION)
				{
					MultiShape ms = new MultiShape();
					ms.addShape(this);
					ms.addShape(STLAllocator<U, AllocPolicy>);
					return ms;
				}
				else if (opType == BOT_INTERSECTION)
					return new Shape(); //empty result
				else if (opType == BOT_DIFFERENCE)
					return new Shape(); //empty result
			}
		}
		MultiShape outputMultiShape = new MultiShape();
	
		Shape[] inputShapes = new Shape[2];
		inputShapes[0] =this;
		inputShapes[1] =&STLAllocator<U, AllocPolicy>;
	
		while (!intersections.Count == 0)
		{
			Shape outputShape = new Shape();
			byte shapeSelector = 0; // 0 : first shape, 1 : second shape
	
			Vector2 currentPosition = intersections.GetEnumerator().position;
			IntersectionInShape firstIntersection = *intersections.GetEnumerator();
			uint currentSegment = firstIntersection.index[shapeSelector];
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
			intersections.erase(intersections.GetEnumerator());
			outputShape.addPoint(currentPosition);
	
			sbyte isIncreasing = 0; // +1 if increasing, -1 if decreasing, 0 if undefined
	
			if (!_findWhereToGo(inputShapes, opType, firstIntersection, ref shapeSelector, ref isIncreasing, ref currentSegment))
			{
				// That intersection is located on a place where the resulting shape won't go => discard
				continue;
			}
	
			while (true)
			{
				// find the closest intersection on the same segment, in the correct direction
				List<IntersectionInShape>.Enumerator found_next_intersection = intersections.end();
				float distanceToNextIntersection = std.numeric_limits<Real>.max();
	
				uint nextPoint = currentSegment+ (isIncreasing ==1?1:0);
				bool nextPointIsOnIntersection = false;
	
				for (List<IntersectionInShape>.Enumerator it = intersections.GetEnumerator(); it.MoveNext(); ++it)
				{
					if (currentSegment == it.index[shapeSelector])
					{
						if (((it.position-currentPosition).dotProduct(it.position-inputShapes[shapeSelector].getPoint(nextPoint)) < 0) || (it.onVertex[shapeSelector] && nextPoint == it.index[shapeSelector]))
						{
							// found an intersection between the current one and the next segment point
							float d = (it.position-currentPosition).length();
							if (d < distanceToNextIntersection)
							{
								// check if we have the nearest intersection
								found_next_intersection = it;
								distanceToNextIntersection = d;
							}
						}
					}
					if (nextPoint == it.index[shapeSelector] && it.onVertex[shapeSelector])
						nextPointIsOnIntersection = true;
				}
	
				// stop condition
				if (currentSegment == firstIntersection.index[shapeSelector])
				{
					// we found ourselves on the same segment as the first intersection and no other
					if ((firstIntersection.position-currentPosition).dotProduct(firstIntersection.position-inputShapes[shapeSelector].getPoint(nextPoint)) < 0)
					{
						float d = (firstIntersection.position-currentPosition).length();
						if (d>0. && d < distanceToNextIntersection)
						{
							outputShape.close();
							break;
						}
					}
				}
	
				// We actually found the next intersection => change direction and add current intersection to the list
				if (found_next_intersection.MoveNext())
				{
					IntersectionInShape currentIntersection = found_next_intersection.Current;
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
					intersections.erase(found_next_intersection);
					outputShape.addPoint(currentIntersection.position);
					bool result = _findWhereToGo(inputShapes, opType, currentIntersection, ref shapeSelector, ref isIncreasing, ref currentSegment);
					if (result == null)
					{
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
						//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "We should not be here!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
                         throw new Exception("We should not be here!");						
;
					}
				}
				else
				{
					// no intersection found for the moment => just continue on the current segment
					if (!nextPointIsOnIntersection)
					{
						if (isIncreasing ==1)
							currentPosition = inputShapes[shapeSelector].getPoint(currentSegment+1);
						else
							currentPosition = inputShapes[shapeSelector].getPoint(currentSegment);
	
						outputShape.addPoint(currentPosition);
					}
					currentSegment =Utils.modulo(currentSegment+isIncreasing, inputShapes[shapeSelector].getSegCount());
				}
			}
	
			outputMultiShape.addShape(outputShape);
		}
		return outputMultiShape;
	}

	private class IntersectionInShape
	{
		public uint[] index = new uint[2];
		public bool[] onVertex = new bool[2];
		public Vector2 position = new Vector2();
		public IntersectionInShape(uint i, uint j, Vector2 intersect)
		{
			position = new Vector3(GlobalMembersProceduralShape.intersect);
			index[0] = i;
			index[1] = j;
			onVertex[0] = false;
			onVertex[1] = false;
		}
	}

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool _isLookingForOutside(BooleanOperationType opType, sbyte shapeSelector) const
	private bool _isLookingForOutside(BooleanOperationType opType, sbyte shapeSelector)
	{
		switch (opType)
		{
		case BOT_UNION:
			return true;
		case BOT_INTERSECTION:
			return false;
		case BOT_DIFFERENCE:
			if (shapeSelector == 0)
				return true;
			return false;
		default :
			return true;
		}
	}

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: sbyte _isIncreasing(float d, BooleanOperationType opType, sbyte shapeSelector) const
	private sbyte _isIncreasing(float d, BooleanOperationType opType, sbyte shapeSelector)
	{
		if (d<0 && opType == BOT_UNION)
			return -1;
		if (d>0 && opType == BOT_INTERSECTION)
			return -1;
		if (opType == BOT_DIFFERENCE)
		{
			if ((d<0 && shapeSelector == 0)||(d>0 && shapeSelector == 1))
				return -1;
		}
		return 1;
	}

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool _findWhereToGo(const Shape* inputShapes[], BooleanOperationType opType, IntersectionInShape intersection, byte& shapeSelector, sbyte& isIncreasing, uint& currentSegment) const
	private bool _findWhereToGo(Shape[] inputShapes, BooleanOperationType opType, IntersectionInShape intersection, ref byte shapeSelector, ref sbyte isIncreasing, ref uint currentSegment)
	{
		if (intersection.onVertex[0] || intersection.onVertex[1])
		{
			// determine 4 directions with normal info
			// if 2 normals "face each other" then you have the couple of outside directions
			Vector2[] directions = new Vector2[4];
			string sides = new string(new char[4]);
			byte incomingDirection;
	
			// fill-in the incoming arrays
			if (isIncreasing ==0)
				incomingDirection =255;
			else
				incomingDirection = shapeSelector + (isIncreasing ==1?2:0);
			for (byte i = 0; i<2; i++)
				if (intersection.onVertex[i])
				{
					directions[i] = inputShapes[i].getDirectionBefore(intersection.index[i]);
					directions[2+i] = - inputShapes[i].getDirectionAfter(intersection.index[i]);
				}
				else
				{
					directions[2+i] = - inputShapes[i].getDirectionAfter(intersection.index[i]);
					directions[i] = - directions[2+i];
				}
			for (byte i =0; i<4; i++)
			{
				sides[i] =(i/2==0?-1:1)*(inputShapes[i%2].mOutSide == SIDE_RIGHT?-1:1);
			}
	
			bool[] isOutside = new bool[4];
			std.pair<Radian, byte>[] sortedDirections = new std.pair[4];
	
			// sort by angle
			for (int i =0; i<4; i++)
			{
				if (i ==0)
					sortedDirections[i].first = 0;
				else
					sortedDirections[i].first = sides[0] * Utils.angleTo(directions[0], directions[i]);
				sortedDirections[i].second=i;
			}
	
			std.sort(sortedDirections, sortedDirections+4, GlobalMembersProceduralShape._sortAngles);
	
			//find which segments are outside
			if (sides[0] != sides[sortedDirections[1].second])
			{
				isOutside[0] =isOutside[sortedDirections[1].second] =true;
				isOutside[sortedDirections[2].second] =isOutside[sortedDirections[3].second] =false;
			}
			else
			{
				isOutside[sortedDirections[1].second] =isOutside[sortedDirections[2].second] =true;
				isOutside[sortedDirections[3].second] =isOutside[sortedDirections[0].second] =false;
			}
	
			//find first eligible segment that is not the current segment
			for (ushort i =0; i<4; i++)
				if ((isOutside[i] == _isLookingForOutside(opType, i%2)) && (i!=incomingDirection))
				{
					shapeSelector = i%2;
					isIncreasing = i/2==0?1:-1;
					currentSegment = intersection.index[shapeSelector];
					return true;
				}
			// if we reach here, it means that no segment is eligible! (it should only happen with difference opereation
			return false;
		}
		else
		{
			// determine which way to go
			int nextShapeSelector = (shapeSelector+1)%2;
	
			float d = inputShapes[nextShapeSelector].getDirectionAfter(intersection.index[nextShapeSelector]).dotProduct(inputShapes[shapeSelector].getNormalAfter(currentSegment));
			isIncreasing = _isIncreasing(d, opType, nextShapeSelector);
	
			shapeSelector = nextShapeSelector;
	
			currentSegment = intersection.index[shapeSelector];
			return true;
		}
	}

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void _findAllIntersections(const Shape& STLAllocator<U, AllocPolicy>, List<IntersectionInShape>& intersections) const
	private void _findAllIntersections(Shape STLAllocator<U, AllocPolicy>, ref List<IntersectionInShape> intersections)
	{
		for (ushort i =0; i<getSegCount(); i++)
		{
			Segment2D seg1 = new Segment2D(getPoint(i), getPoint(i+1));
	
			for (ushort j =0; j<STLAllocator<U, AllocPolicy>.getSegCount(); j++)
			{
				Segment2D seg2 = new Segment2D(STLAllocator<U, AllocPolicy>.getPoint(j), STLAllocator<U, AllocPolicy>.getPoint(j+1));
	
				Vector2 GlobalMembersProceduralShape.intersect = new Vector2();
				if (seg1.findIntersect(seg2, ref GlobalMembersProceduralShape.intersect))
				{
					IntersectionInShape inter = new IntersectionInShape(i, j, GlobalMembersProceduralShape.intersect);
					// check if intersection is "borderline" : too near to a vertex
					if (seg1.mA.squaredDistance(GlobalMembersProceduralShape.intersect)<1e-8)
					{
						inter.onVertex[0] = true;
					}
					if (seg1.mB.squaredDistance(GlobalMembersProceduralShape.intersect)<1e-8)
					{
						inter.onVertex[0] = true;
						inter.index[0]++;
					}
					if (seg2.mA.squaredDistance(GlobalMembersProceduralShape.intersect)<1e-8)
					{
						inter.onVertex[1] = true;
					}
					if (seg2.mB.squaredDistance(GlobalMembersProceduralShape.intersect)<1e-8)
					{
						inter.onVertex[1] = true;
						inter.index[1]++;
					}
	
					intersections.Add(inter);
				}
			}
		}
	}

}

}