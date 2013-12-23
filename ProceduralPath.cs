using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
//\defgroup pathgrp Path
//Elements for path generation.
//@{
//@}
//

//*
// * \ingroup pathgrp
// * Succession of points in 3D space.
// * Can be closed or not.
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Path
public class Path
{
	private List<Vector3> mPoints = new List<Vector3>();
	private bool mClosed;
	/// Default constructor
	public Path()
	{
		mClosed = false;
	}

	//* Adds a point to the path, as a Vector3 
	public Path addPoint(Vector3 pt)
	{
		mPoints.Add(pt);
		return this;
	}

	//* Adds a point to the path, using its 3 coordinates 
	public Path addPoint(float x, float y, float z)
	{
		mPoints.Add(new Vector3(x, y, z));
		return this;
	}

	/// Inserts a point to the path
	/// @param index the index before the inserted point
	/// @param x new point's x coordinate
	/// @param y new point's y coordinate
	/// @param z new point's z coordinate
	public Path insertPoint(int index, float x, float y, float z)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.GetEnumerator()+index,new Vector3(x, y, z));
		return this;
	}

	/// Inserts a point to the path
	/// @param index the index before the inserted point
	/// @param pt new point's position
	public Path insertPoint(int index, Vector3 pt)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.GetEnumerator()+index, pt);
		return this;
	}

	/// Appends another path at the end of this one
	public Path appendPath(Path STLAllocator<U, AllocPolicy>)
	{
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
		mPoints.insert(mPoints.end(), STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
		return this;
	}

	/// Appends another path at the end of this one, relative to the last point of this path
	public Path appendPathRel(Path STLAllocator<U, AllocPolicy>)
	{
		if (mPoints.Count == 0)
			appendPath(STLAllocator<U, AllocPolicy>);
		else
		{
			Vector3 refVector = *(mPoints.end()-1);
			List<Vector3> pointList = new List<Vector3>(STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
			for (List<Vector3>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
				it.Current += refVector;
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
			mPoints.insert(mPoints.end(), pointList.GetEnumerator(), pointList.end());
		}
		return this;
	}

	//* Clears the content of the Path 
	public Path reset()
	{
		mPoints.Clear();
		return this;
	}

//    *
//	Define the path as being closed. Almost the same as adding a last point on the first point position
//	\exception Ogre::InvalidStateException Cannot close an empty path
//	
	public Path close()
	{
		if (mPoints.Count == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Cannot close an empty path", "Procedural::Path::close()", __FILE__, __LINE__);
			;
		mClosed = true;
		return this;
	}

	//* Tells if the path is closed or not 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool isClosed() const
	public bool isClosed()
	{
		return mClosed;
	}

	//* Gets the list of points as a vector of Vector3 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const List<Ogre::Vector3>& getPoints() const
	public List<Vector3> getPoints()
	{
		return mPoints;
	}

	/// Gets raw vector data of this path as a non-const reference
	public List<Vector3> getPointsReference()
	{
		return mPoints;
	}

//    * Safely gets a given point.
//	 * Takes into account whether the path is closed or not.
//	 * @param i the index of the point.
//	 *          if it is <0 or >maxPoint, cycle through the list of points
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: const Ogre::Vector3& getPoint(int i) const
	public Vector3 getPoint(int i)
	{
		if (mClosed)
			return mPoints[Utils.modulo(i, mPoints.Count)];
		return mPoints[Utils.cap(i, 0, mPoints.Count-1)];
	}

//    * Gets the number of segments in the path
//	 * Takes into accound whether path is closed or not
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: int getSegCount() const
	public int getSegCount()
	{
		return (mPoints.Count-1) + (mClosed?1:0);
	}

//    *
//	 * Returns local direction after the current point
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::Vector3 getDirectionAfter(uint i) const
	public Vector3 getDirectionAfter(uint i)
	{
		// If the path isn't closed, we get a different calculation at the end, because
		// the tangent shall not be null
		if (!mClosed && i == mPoints.Count - 1 && i > 0)
			return (mPoints[i] - mPoints[i-1]).normalisedCopy();
		else
			return (getPoint(i+1) - getPoint(i)).normalisedCopy();
	}

//    *
//	 * Returns local direction after the current point
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::Vector3 getDirectionBefore(uint i) const
	public Vector3 getDirectionBefore(uint i)
	{
		// If the path isn't closed, we get a different calculation at the end, because
		// the tangent shall not be null
		if (!mClosed && i == 1)
			return (mPoints[1] - mPoints[0]).normalisedCopy();
		else
			return (getPoint(i) - getPoint(i-1)).normalisedCopy();
	}

//    *
//	 * Returns the local direction at the current point.
//	 * @param i index of the point
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::Vector3 getAvgDirection(uint i) const
	public Vector3 getAvgDirection(uint i)
	{
		return (getDirectionAfter(i) + getDirectionBefore(i)).normalisedCopy();
	}

	/// Returns the total lineic length of that shape
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float getTotalLength() const
	public float getTotalLength()
	{
		float length = 0;
		for (uint i = 0; i < mPoints.Count - 1; ++i)
			length += (mPoints[i + 1] - mPoints[i]).length();
		if (mClosed)
			length += (mPoints[mPoints.Count - 1]-*mPoints.GetEnumerator()).length();
		return length;
	}


//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::float getLengthAtPoint(int index) const
	public float getLengthAtPoint(int index)
	{
		float length = 0;
		for (uint i = 0; i < index; ++i)
			length += (mPoints[i + 1] - mPoints[i]).length();
		//if (mClosed)
		//length += (mPoints.back()-*mPoints.begin()).length();
		return length;
	
	}

	/// Gets a position on the shape with index of the point and a percentage of position on the segment
	/// @param i index of the segment
	/// @param coord a number between 0 and 1 meaning the percentage of position on the segment
	/// @exception Ogre::InvalidParametersException i is out of bounds
	/// @exception Ogre::InvalidParametersException coord must be comprised between 0 and 1
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline Ogre::Vector3 getPosition(uint i, Ogre::float coord) const
	public Vector3 getPosition(uint i, float coord)
	{
		if (i >= mPoints.Count)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Out of Bounds", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
			;
		if (coord < 0.0f || coord > 1.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Coord must be comprised between 0 and 1", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
			;
		Vector3 A = getPoint(i);
		Vector3 B = getPoint(i+1);
		return A + coord*(B-A);
	}

	/// Gets a position on the shape from lineic coordinate
	/// @param coord lineic coordinate
	/// @exception Ogre::InvalidStateException The path must at least contain 2 points
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::Vector3 getPosition(Ogre::float coord) const
	public Vector3 getPosition(float coord)
	{
		if (mPoints.Count < 2)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The path must at least contain 2 points", "Procedural::Path::getPosition(Ogre::Real)", __FILE__, __LINE__);
			;
		uint i = 0;
		while (true)
		{
			float nextLen = (getPoint(i + 1) - getPoint(i)).length();
			if (coord > nextLen)
				coord -= nextLen;
			else
				return getPosition(i, coord / nextLen);
			if (!mClosed && i >= mPoints.Count - 2)
				return mPoints[mPoints.Count - 1];
			i++;
		}
	}

//    *
//	 * Outputs a mesh representing the path.
//	 * Mostly for debugging purposes
//	 
	public MeshPtr realizeMesh()
	{
		return realizeMesh("");
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Ogre::MeshPtr realizeMesh(const string& name = "") const
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
	public MeshPtr realizeMesh(string name)
	{
		Mogre.SceneManager smgr = Root.getSingleton().getSceneManagerIterator().begin().second;
		Mogre.ManualObject manual = smgr.createManualObject();
		manual.begin("BaseWhiteNoLighting", RenderOperation.OperationType.OT_LINE_STRIP);
	
		for (List<Vector3>.Enumerator itPos = mPoints.GetEnumerator(); itPos.MoveNext(); itPos++)
			manual.position(itPos.Current);
		if (mClosed)
			manual.position(*(mPoints.GetEnumerator()));
		manual.end();
	
		Mogre.MeshPtr mesh = new MeshPtr();
		if (name == "")
			mesh = manual.convertToMesh(Utils.getName());
		else
			mesh = manual.convertToMesh(name);
	
		return mesh;
	}

	/// Creates a path with the keys of this path and extra keys coming from a track
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Path mergeKeysWithTrack(const Track& track) const
	public Path mergeKeysWithTrack(Track track)
	{
		if (!track.isInsertPoint() || track.getAddressingMode() == Track.AM_POINT)
			return this;
		float totalLength = getTotalLength();
	
		float lineicPos = 0;
		float pathLineicPos = 0;
		Path outputPath = new Path();
		outputPath.addPoint(getPoint(0));
		for (uint i = 1; i < mPoints.Count;)
		{
			float nextLineicPos = pathLineicPos + (mPoints[i] - mPoints[i - 1]).length();
	
			std.map<Real, Real>.Enumerator it = track._getKeyValueAfter(lineicPos, lineicPos / totalLength, i - 1);
	
			float nextTrackPos = it.first;
			if (track.getAddressingMode() == Track.AM_RELATIVE_LINEIC)
				nextTrackPos *= totalLength;
	
			// Adds the closest point to the curve, being either from the path or the track
			if (nextLineicPos <= nextTrackPos || lineicPos >= nextTrackPos)
			{
				outputPath.addPoint(mPoints[i]);
				i++;
				lineicPos = nextLineicPos;
				pathLineicPos = nextLineicPos;
			}
			else
			{
				outputPath.addPoint(getPosition(i - 1, (nextTrackPos - pathLineicPos) / (nextLineicPos - pathLineicPos)));
				lineicPos = nextTrackPos;
			}
		}
		return outputPath;
	}

//    *
//	* Applies the given translation to all the points already defined.
//	* Has strictly no effect on the points defined after that
//	* @param translation the translation vector
//	
	public Path translate(Vector3 translation)
	{
		for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
			it.Current+=translation;
		return this;
	}

//    *
//	 * Applies the given translation to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param translationX X component of the translation vector
//	 * @param translationY Y component of the translation vector
//	 * @param translationZ Z component of the translation vector
//	 
	public Path translate(float translationX, float translationY, float translationZ)
	{
		return translate(Vector3(translationX, translationY, translationZ));
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param amount amount of scale
//	 
	public Path scale(float amount)
	{
		return scale(amount, amount, amount);
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param scaleX amount of scale in the X direction
//	 * @param scaleY amount of scale in the Y direction
//	 * @param scaleZ amount of scale in the Z direction
//	 
	public Path scale(float scaleX, float scaleY, float scaleZ)
	{
		for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			it.x *= scaleX;
			it.y *= scaleY;
			it.z *= scaleZ;
		}
		return this;
	}

//    *
//	 * Applies the given scale to all the points already defined.
//	 * Has strictly no effect on the points defined after that
//	 * @param amount of scale
//	 
	public Path scale(Vector3 amount)
	{
		return scale(amount.x, amount.y, amount.z);
	}

//    *
//	 * Reflect all points in this path against a zero-origined plane with a given normal
//	 * @param normal the normal
//	 
	public Path reflect(Vector3 normal)
	{
		for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			it.Current = it.reflect(normal);
		}
		return this;
	}

	/// Extracts a part of the shape as a new path
	/// @param first first index to be in the new path
	/// @param last last index to be in the new path
	public Path extractSubPath(uint first, uint last)
	{
		Path p = new Path();
		for (uint i =first; i<last; i++)
			p.addPoint(mPoints[i]);
		if (mClosed)
			p.close();
		return p;
	}

	/// Reverses direction of the path
	public Path reverse()
	{
		std.reverse(mPoints.GetEnumerator(), mPoints.end());
		return this;
	}

	public void buildFromSegmentSoup(List<Segment3D> segList, ref List<Path> @out)
	{
		std.multimap<Vector3, Vector3, Vector3Comparator> segs = new std.multimap<Vector3, Vector3, Vector3Comparator>();
		for (List<Segment3D>.Enumerator it = segList.GetEnumerator(); it.MoveNext(); ++it)
		{
			segs.insert(std.pair<Vector3, Vector3 > (it.mA, it.mB));
			segs.insert(std.pair<Vector3, Vector3 > (it.mB, it.mA));
		}
		while (!segs.empty())
		{
			Vector3 headFirst = segs.begin().first;
			Vector3 headSecond = segs.begin().second;
			Path p = new Path();
			p.addPoint(headFirst).addPoint(headSecond);
			std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator firstSeg = segs.begin();
			std.pair<std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator, std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator> correspondants2 = segs.equal_range(headSecond);
			for (std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator it = correspondants2.first; it != correspondants2.second;)
			{
				std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator removeIt = it++;
				if ((removeIt.second - firstSeg.first).squaredLength() < 1e-8)
					segs.erase(removeIt);
			}
			segs.erase(firstSeg);
			bool foundSomething = true;
			while (!segs.empty() && foundSomething)
			{
				foundSomething = false;
				std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator next = segs.find(headSecond);
				if (next != segs.end())
				{
					foundSomething = true;
					headSecond = next.second;
					p.addPoint(headSecond);
					std.pair<std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator, std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator> correspondants = segs.equal_range(headSecond);
					for (std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator it = correspondants.first; it != correspondants.second;)
					{
						std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator removeIt = it++;
						if ((removeIt.second - next.first).squaredLength() < 1e-8)
							segs.erase(removeIt);
					}
					segs.erase(next);
				}
				std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator previous = segs.find(headFirst);
				if (previous != segs.end())
				{
					foundSomething = true;
					p.insertPoint(0, previous.second);
					headFirst = previous.second;
					std.pair<std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator, std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator> correspondants = segs.equal_range(headFirst);
					for (std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator it = correspondants.first; it != correspondants.second;)
					{
						std.multimap<Vector3, Vector3, Vector3Comparator>.Enumerator removeIt = it++;
						if ((removeIt.second - previous.first).squaredLength() < 1e-8)
							segs.erase(removeIt);
					}
					segs.erase(previous);
				}
			}
			if (p.getPoint(0).squaredDistance(p.getPoint(p.getSegCount() + 1)) < 1e-6)
			{
				p.getPointsReference().pop_back();
				p.close();
			}
			@out.push_back(p);
		}
	}

	/// Converts the path to a shape, with Y=0

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Shape convertToShape() const
	public Shape convertToShape()
	{
		Shape s = new Shape();
		for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
		{
			s.addPoint(it.x, it.y);
		}
		if (mClosed)
			s.close();
	
		return s;
	}

}

//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport MultiPath
public class MultiPath
{
	public class PathCoordinate
	{
		public uint pathIndex;
		public uint pointIndex;
		public PathCoordinate(uint _pathIndex, uint _pointIndex)
		{
			pathIndex = _pathIndex;
			pointIndex = _pointIndex;
		}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: bool operator < (const PathCoordinate& STLAllocator<U, AllocPolicy>) const
		public static bool operator < (PathCoordinate STLAllocator<U, AllocPolicy>)
		{
			if (pathIndex != STLAllocator<U, AllocPolicy>.pathIndex)
				return pathIndex < STLAllocator<U, AllocPolicy>.pathIndex;
			return pointIndex<STLAllocator<U, AllocPolicy>.pointIndex;
		}
	}
	//#define PathIntersection_AlternateDefinition1
	private List<Path> mPaths = new List<Path>();
	private std.map<PathCoordinate, List<PathCoordinate>> mIntersectionsMap = new std.map<PathCoordinate, List<PathCoordinate>>();
	private List<List<PathCoordinate>> mIntersections = new List<List<PathCoordinate>>();

	public void clear()
	{
		mPaths.Clear();
	}

	public MultiPath addPath(Path path)
	{
		mPaths.Add(path);
		return this;
	}

	public MultiPath addMultiPath(MultiPath multiPath)
	{
		for (List<Path>.Enumerator it = multiPath.mPaths.GetEnumerator(); it.MoveNext(); ++it)
			mPaths.Add(it.Current);
		return this;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: uint getPathCount() const
	public uint getPathCount()
	{
		return mPaths.Count;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: Path getPath(uint i) const
	public Path getPath(uint i)
	{
		return mPaths[i];
	}

	//-----------------------------------------------------------------------
	public void _calcIntersections()
	{
		mIntersectionsMap.clear();
		mIntersections.Clear();
#if PathIntersection_AlternateDefinition1
		std.map<Vector3, List<PathCoordinate>, Vector3Comparator> pointSet = new std.map<Vector3, List<PathCoordinate>, Vector3Comparator>();
#elif PathIntersection_AlternateDefinition2
		std.map<Vector3, List<PathCoordinate>, Vector3Comparator> pointSet = new std.map<Vector3, List<PathCoordinate>, Vector3Comparator>();
#endif
		for (List<Path>.Enumerator it = mPaths.GetEnumerator(); it.MoveNext(); ++it)
		{
			for (List<Vector3>.Enumerator it2 = it.getPoints().begin(); it2 != it.getPoints().end(); ++it2)
			{
				PathCoordinate pc = new PathCoordinate(it-mPaths.GetEnumerator(), it2-it.getPoints().begin());
				if (pointSet.find(it2.Current)==pointSet.end())
				{
#if PathIntersection_AlternateDefinition1
					List<PathCoordinate> pi = new List<PathCoordinate>();
#elif PathIntersection_AlternateDefinition2
					List<PathCoordinate> pi = new List<PathCoordinate>();
#endif
					pi.Add(pc);
					pointSet[it2.Current] = pi;
				}
				else
					pointSet[it2.Current].push_back(pc);
			}
		}
#if PathIntersection_AlternateDefinition1
		for (std.map<Vector3, List<PathCoordinate>, Vector3Comparator>.Enumerator it = pointSet.begin(); it.MoveNext(); ++it)
#elif PathIntersection_AlternateDefinition2
		for (std.map<Vector3, List<PathCoordinate>, Vector3Comparator>.Enumerator it = pointSet.begin(); it.MoveNext(); ++it)
#endif
			if (it.second.size()>1)
			{
#if PathIntersection_AlternateDefinition1
				for (List<PathCoordinate>.Enumerator it2 = it.second.begin(); it2.MoveNext(); ++it2)
#elif PathIntersection_AlternateDefinition2
				for (List<PathCoordinate>.Enumerator it2 = it.second.begin(); it2.MoveNext(); ++it2)
#endif
					mIntersectionsMap[it2.Current] = it.second;
				mIntersections.Add(it.second);
			}
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline const std::map<PathCoordinate, List<PathCoordinate>>& getIntersectionsMap() const
	public std.map<PathCoordinate, List<PathCoordinate>> getIntersectionsMap()
	{
		return mIntersectionsMap;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: inline const List<List<PathCoordinate>>& getIntersections() const
	public List<List<PathCoordinate>> getIntersections()
	{
		return mIntersections;
	}

//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: List<std::pair<uint, uint> > getNoIntersectionParts(uint pathIndex) const
	public List<std.pair<uint, uint> > getNoIntersectionParts(uint pathIndex)
	{
		Path path = mPaths[pathIndex];
		List<std.pair<uint, uint> > result = new List<std.pair<uint, uint> >();
		List<int> intersections = new List<int>();
		for (std.map<PathCoordinate, List<PathCoordinate>>.Enumerator it = mIntersectionsMap.begin(); it != mIntersectionsMap.end(); ++it)
			if (it.first.pathIndex == pathIndex)
				intersections.Add(it.first.pointIndex);
		std.sort(intersections.GetEnumerator(), intersections.end());
		int begin = 0;
		for (List<int>.Enumerator it = intersections.GetEnumerator(); it.MoveNext(); ++it)
		{
			if (it.Current-1>begin)
				result.Add(std.pair<uint, uint>(begin, it.Current-1));
			begin = it.Current+1;
		}
		if (path.getSegCount() > begin)
			result.Add(std.pair<uint, uint>(begin, path.getSegCount()));
		return result;
	}
}

}


