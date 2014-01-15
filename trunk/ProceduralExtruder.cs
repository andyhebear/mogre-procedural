using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
public static class GlobalMembersProceduralExtruder
{
	//-----------------------------------------------------------------------
	public static void _extrudeShape(ref TriangleBuffer buffer, Shape shape, Vector3 position, Quaternion orientationLeft, Quaternion orientationRight, float scale, float scaleCorrectionLeft, float scaleCorrectionRight, float totalShapeLength, float uTexCoord, bool joinToTheNextSection, Track shapeTextureTrack)
	{
		float lineicShapePos = 0.0f;
		int numSegShape = shape.getSegCount();
		// Insert new points
		for (uint j =0; j <= numSegShape; ++j)
		{
			Vector2 vp2 = shape.getPoint(j);
			Quaternion orientation = (vp2.x>0) ? orientationRight : orientationLeft;
			Vector2 vp2normal = shape.getAvgNormal(j);
			Vector3 vp = new Vector3();
			if (vp2.x>0)
				vp = new Vector3(scaleCorrectionRight * vp2.x, vp2.y, 0);
			else
				vp = new Vector3(scaleCorrectionLeft * vp2.x, vp2.y, 0);
			Vector3 normal = new Vector3(vp2normal.x, vp2normal.y, 0);
			buffer.rebaseOffset();
			Vector3 newPoint = position+orientation*(scale *vp);
			if (j>0)
				lineicShapePos += (vp2 - shape.getPoint(j-1)).length;
			float vTexCoord = 0f;
			if (shapeTextureTrack != null)
				vTexCoord = shapeTextureTrack.getValue(lineicShapePos, lineicShapePos / totalShapeLength, j);
			else
				vTexCoord = lineicShapePos / totalShapeLength;

			buffer.vertex(newPoint, orientation *normal, new Vector2(uTexCoord, vTexCoord));

			if (j <numSegShape && joinToTheNextSection)
			{
				if (shape.getOutSide() == Side.SIDE_LEFT)
				{
					buffer.triangle(numSegShape + 1, numSegShape + 2, 0);
					buffer.triangle(0, numSegShape + 2, 1);
				}
				else
				{
					buffer.triangle(numSegShape + 2, numSegShape + 1, 0);
					buffer.triangle(numSegShape + 2, 0, 1);
				}
			}
		}
	}
	//-----------------------------------------------------------------------
	public static void _extrudeBodyImpl(ref TriangleBuffer buffer, Shape shapeToExtrude, Path pathToExtrude, int pathBeginIndex, int pathEndIndex, Track shapeTextureTrack, Track rotationTrack, Track scaleTrack, Track pathTextureTrack)
	{
		if (pathToExtrude == null || shapeToExtrude == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Shape and Path must not be null!", "Procedural::Extruder::_extrudeBodyImpl(Procedural::TriangleBuffer&, const Procedural::Shape*)", __FILE__, __LINE__);
			 throw new Exception("Shape and Path must not be null!");
            ;

		int numSegPath = (pathEndIndex - pathBeginIndex);
		int numSegShape = shapeToExtrude.getSegCount();

		if (numSegPath == 0 || numSegShape == 0)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Shape and path must contain at least two points", "Procedural::Extruder::_extrudeBodyImpl(Procedural::TriangleBuffer&, const Procedural::Shape*)", __FILE__, __LINE__);
			 throw new Exception("Shape and path must contain at least two points");
            ;

		float totalPathLength = pathToExtrude.getTotalLength();
		float totalShapeLength = shapeToExtrude.getTotalLength();

		// Merge shape and path with tracks
		float lineicPos = pathToExtrude.getLengthAtPoint(pathBeginIndex);
		Path path = pathToExtrude;
		numSegPath = (pathEndIndex - pathBeginIndex);
		numSegShape = shapeToExtrude.getSegCount();

		// Estimate vertex and index count
		buffer.rebaseOffset();
		buffer.estimateIndexCount(numSegShape *numSegPath *6);
		buffer.estimateVertexCount((numSegShape+1)*(numSegPath+1));

		Vector3 oldup = new Vector3();
		for (int i = pathBeginIndex; i <= pathEndIndex; ++i)
		{
			Vector3 v0 = path.getPoint(i);
			Vector3 direction = path.getAvgDirection(i);

			Quaternion q = Utils._computeQuaternion(direction);

			Radian angle = Utils.angleBetween ((q *Vector3.UNIT_Y),(oldup));
			if (i>pathBeginIndex && angle>(Radian)Math.HALF_PI/2.0f)
			{
				q = Utils._computeQuaternion(direction, oldup);
			}
			oldup = q * Vector3.UNIT_Y;

			float scale =1.0f;

			if (i>pathBeginIndex)
				lineicPos += (v0-path.getPoint(i-1)).Length;

			// Get the values of angle and scale
			if (rotationTrack != null)
			{
				float angle_2 = 0f;
				angle_2 = rotationTrack.getValue(lineicPos, lineicPos / totalPathLength, (uint)i);

				q = q *new Quaternion((Radian)angle_2, Vector3.UNIT_Z);
			}
			if (scaleTrack != null)
			{
				scale = scaleTrack.getValue(lineicPos, lineicPos / totalPathLength, (uint)i);
			}
			float uTexCoord = 0f;
			if (pathTextureTrack != null)
				uTexCoord = pathTextureTrack.getValue(lineicPos, lineicPos / totalPathLength, (uint)i);
			else
				uTexCoord = lineicPos / totalPathLength;

			_extrudeShape(ref buffer, shapeToExtrude, v0, q, q, scale, 1.0f, 1.0f, totalShapeLength, uTexCoord, i<pathEndIndex, shapeTextureTrack);
		}
	}
	//-----------------------------------------------------------------------
	public static void _extrudeCapImpl(ref TriangleBuffer buffer, MultiShape multiShapeToExtrude, MultiPath extrusionMultiPath, Extruder.TrackMap scaleTracks, Extruder.TrackMap rotationTracks)
	{
		List<int> indexBuffer = new List<int>();
		List<Vector2> pointList = new List<Vector2>();

		Triangulator t = new Triangulator();
		t.setMultiShapeToTriangulate(multiShapeToExtrude);
		t.triangulate(ref indexBuffer, ref pointList);

		for (uint i =0; i<extrusionMultiPath.getPathCount(); ++i)
		{
			Path extrusionPath = extrusionMultiPath.getPath(i);
			 Track scaleTrack = null;
			 Track rotationTrack = null;
			if (scaleTracks.find(i) != scaleTracks.end())
				scaleTrack = scaleTracks.find(i).second;
			if (rotationTracks.find(i) != rotationTracks.end())
				rotationTrack = rotationTracks.find(i).second;

			//begin cap
			if (extrusionMultiPath.getIntersectionsMap().find(MultiPath.PathCoordinate(i, 0)) == extrusionMultiPath.getIntersectionsMap().end())
			{
				buffer.rebaseOffset();
				buffer.estimateIndexCount(indexBuffer.Count);
				buffer.estimateVertexCount(pointList.Count);

				Quaternion qBegin = Utils._computeQuaternion(extrusionPath.getDirectionAfter(0));
				if (rotationTrack != null)
				{
					float angle = rotationTrack.getFirstValue();
					qBegin = qBegin *new Quaternion((Radian)angle, Vector3.UNIT_Z);
				}
				float scaleBegin =1.;
				if (scaleTrack != null)
					scaleBegin = scaleTrack.getFirstValue();

				for (int j =0; j<pointList.Count; j++)
				{
					Vector2 vp2 = pointList[j];
					Vector3 vp = new Vector3(vp2.x, vp2.y, 0);
					Vector3 normal = -Vector3.UNIT_Z;

					Vector3 newPoint = extrusionPath.getPoint(0)+qBegin*(scaleBegin *vp);
					buffer.vertex(newPoint, qBegin *normal, vp2);
				}
				for (int i =0; i<indexBuffer.Count/3; i++)
				{
					buffer.index(indexBuffer[i *3]);
					buffer.index(indexBuffer[i *3+2]);
					buffer.index(indexBuffer[i *3+1]);
				}
			}

			//end cap
			if (extrusionMultiPath.getIntersectionsMap().find(MultiPath.PathCoordinate(i, extrusionPath.getSegCount())) == extrusionMultiPath.getIntersectionsMap().end())
			{
				buffer.rebaseOffset();
				buffer.estimateIndexCount(indexBuffer.Count);
				buffer.estimateVertexCount(pointList.Count);

				Quaternion qEnd = Utils._computeQuaternion(extrusionPath.getDirectionBefore(extrusionPath.getSegCount()));
				if (rotationTrack != null)
				{
					float angle = rotationTrack.getLastValue();
					qEnd = qEnd *new Quaternion((Radian)angle, Vector3.UNIT_Z);
				}
				float scaleEnd =1.0f;
				if (scaleTrack != null)
					scaleEnd = scaleTrack.getLastValue();

				for (int j =0; j<pointList.Count; j++)
				{
					Vector2 vp2 = pointList[j];
					Vector3 vp = new Vector3(vp2.x, vp2.y, 0);
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Vector3 normal = Vector3::UNIT_Z;
					Vector3 normal =(Vector3.UNIT_Z);

					Vector3 newPoint = extrusionPath.getPoint(extrusionPath.getSegCount())+qEnd*(scaleEnd *vp);
					buffer.vertex(newPoint, qEnd *normal, vp2);
				}
				for (int ii =0; ii<indexBuffer.Count/3; ii++)
				{
					buffer.index(indexBuffer[ii *3]);
					buffer.index(indexBuffer[ii *3+1]);
					buffer.index(indexBuffer[ii *3+2]);
				}
			}
		}
	}
	//-----------------------------------------------------------------------
	public static void _extrudeIntersectionImpl(ref TriangleBuffer buffer, MultiPath.PathIntersection intersection, MultiPath multiPath, Shape shape, Track shapeTextureTrack)
	{
		Vector3 intersectionLocation = multiPath.getPath(intersection[0].pathIndex).getPoint(intersection[0].pointIndex);
		Quaternion firstOrientation = Utils._computeQuaternion(multiPath.getPath(intersection[0].pathIndex).getDirectionBefore(intersection[0].pointIndex));
		Vector3 refX = firstOrientation * Vector3.UNIT_X;
		Vector3 refZ = firstOrientation * Vector3.UNIT_Z;

		List<Vector2> v2s = new List<Vector2>();
		List<MultiPath.PathCoordinate> coords = new List<MultiPath.PathCoordinate>();
		List<float> direction = new List<float>();

		for (int i = 0; i < intersection.size(); ++i)
		{
			 Path path = multiPath.getPath(intersection[i].pathIndex);
			int pointIndex = intersection[i].pointIndex;
			if (pointIndex>0 || path.isClosed())
			{
				Vector3 vb = path.getDirectionBefore(pointIndex);
				Vector2 vb2 = new Vector2(vb.DotProduct(refX), vb.DotProduct(refZ));
				v2s.Add(vb2);
				coords.Add(intersection[i]);
				direction.Add(1);
			}
			if (pointIndex<path.getSegCount() || path.isClosed())
			{
				Vector3 va = -path.getDirectionAfter(pointIndex);
				Vector2 va2 = new Vector2(va.DotProduct(refX), va.DotProduct(refZ));
				v2s.Add(va2);
				coords.Add(intersection[i]);
				direction.Add(-1);
			}
		}

		std.map<Radian, int> angles = new std.map<Radian, int>();
		for (int i =1; i<v2s.Count; ++i)
			angles[Utils.angleTo(v2s[0], v2s[i])] =i;

		List<int> orderedIndices = new List<int>();
		orderedIndices.Add(0);
		for (std.map<Radian, int>.Enumerator it = angles.begin(); it!=angles.end(); ++it)
			orderedIndices.Add(it.second);
		for (int i = 0; i < orderedIndices.Count; ++i)
		{
			int idx = orderedIndices[i];
			int idxBefore = orderedIndices[Utils.modulo(i-1, orderedIndices.Count)];
			int idxAfter = orderedIndices[Utils.modulo(i+1, orderedIndices.Count)];
			Radian angleBefore = (Utils.angleBetween(v2s[idx], v2s[idxBefore]) - (Radian)Math.PI)/2;
			Radian angleAfter = ((Radian)Math.PI - Utils.angleBetween(v2s[idx], v2s[idxAfter]))/2;

			int pointIndex = coords[idx].pointIndex - direction[idx];
			const Path path = multiPath.getPath(coords[idx].pathIndex);

			Quaternion qStd = Utils._computeQuaternion(path.getAvgDirection(pointIndex) * direction[idx]);
			float lineicPos = 0f;
			float uTexCoord = path.getLengthAtPoint(pointIndex) / path.getTotalLength();

			// Shape making the joint with "standard extrusion"
			_extrudeShape(ref buffer, shape, path.getPoint(pointIndex), qStd, qStd, 1.0, 1.0, 1.0, shape.getTotalLength(), uTexCoord, true, shapeTextureTrack);

			// Modified shape at the intersection
			Quaternion q = new Quaternion();
			if (direction[idx]>0)
				q = Utils._computeQuaternion(path.getDirectionBefore(coords[idx].pointIndex));
			else
				q = Utils._computeQuaternion(-path.getDirectionAfter(coords[idx].pointIndex));
			Quaternion qLeft = q * new Quaternion(angleBefore, Vector3.UNIT_Y);
			Quaternion qRight = q * new Quaternion(angleAfter, Vector3.UNIT_Y);
			float scaleLeft = 1.0f/Math.Abs(Math.Cos(angleBefore));
			float scaleRight = 1.0f/Math.Abs(Math.Cos(angleAfter));

			uTexCoord = path.getLengthAtPoint(coords[idx].pointIndex) / path.getTotalLength();
			_extrudeShape(ref buffer, shape, path.getPoint(coords[idx].pointIndex), qLeft, qRight, 1.0, scaleLeft, scaleRight, shape.getTotalLength(), uTexCoord, false, shapeTextureTrack);
		}
	}
}

//*
// * Extrudes a 2D shape along a path to build an extruded mesh.
// * Can be used to build things such as pipelines, roads...
// * <table border="0" width="100%"><tr><td>\image html extruder_generic.png "Generic extrusion"</td>
// * <td>\image html extruder_rotationtrack.png "Extrusion with rotation track"</td></tr>
// * <tr><td>\image html extruder_scaletrack.png "Extrusion with scale track"</td>
// * <td>\image html extruder_texturetrack.png "Extrusion with texture track"</td></tr>
// * <tr><td>\image html extruder_multishape.png "Multishape extrusion"</td><td>&nbsp;</td></tr></table>
// * \note Concerning UV texCoords, U is along the path and V along the shape.
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport Extruder : public MeshGenerator<Extruder>
    /// <summary>
/// 挤出机 挤压机 压出机 
    /// </summary>
    public class Extruder : MeshGenerator<Extruder>
{
	private MultiShape mMultiShapeToExtrude = new MultiShape();
	private MultiPath mMultiExtrusionPath = new MultiPath();
	private bool mCapped;

	private readonly std.map<uint, Track> mRotationTracks = new std.map<uint, Track>();
	private readonly std.map<uint, Track> mScaleTracks = new std.map<uint, Track>();
	private readonly std.map<uint, Track> mShapeTextureTracks = new std.map<uint, Track>();
	private readonly std.map<uint, Track> mPathTextureTracks = new std.map<uint, Track>();

	/// Default constructor
	public Extruder()
	{
		mCapped = true;
	}

//    *
//	 * Builds the mesh into the given TriangleBuffer
//	 * @param buffer The TriangleBuffer on where to append the mesh.
//	 * @exception Ogre::InvalidStateException Either shape or multishape must be defined!
//	 * @exception Ogre::InvalidStateException Required parameter is zero!
//	 
	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
	public void addToTriangleBuffer(ref TriangleBuffer buffer)
	{
        if (mMultiShapeToExtrude.getShapeCount() == 0)
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
            //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "At least one shape must be defined!", "Procedural::Extruder::addToTriangleBuffer(Procedural::TriangleBuffer)", __FILE__, __LINE__);
            throw new Exception("At least one shape must be defined!");
            ;
	
		// Triangulate the begin and end caps
		if (mCapped && mMultiShapeToExtrude.isClosed())
		{
			GlobalMembersProceduralExtruder._extrudeCapImpl(ref buffer, mMultiShapeToExtrude, mMultiExtrusionPath, mScaleTracks, mRotationTracks);
		}
	
		// Extrude the paths contained in multiExtrusionPath
		for (uint j =0; j<mMultiExtrusionPath.getPathCount(); ++j)
		{
			Path extrusionPath = mMultiExtrusionPath.getPath(j);
			 Track rotationTrack = null;
			if (mRotationTracks.find(j) != mRotationTracks.end())
			{
				rotationTrack = mRotationTracks.find(j).second;
				extrusionPath = extrusionPath.mergeKeysWithTrack(*mRotationTracks.find(j).second);
			}
			 Track scaleTrack = null;
			if (mScaleTracks.find(j) != mScaleTracks.end())
			{
				rotationTrack = mScaleTracks.find(j).second;
				extrusionPath = extrusionPath.mergeKeysWithTrack(*mScaleTracks.find(j).second);
			}
			 Track pathTextureTrack = null;
			if (mPathTextureTracks.find(j) != mPathTextureTracks.end())
			{
				pathTextureTrack = mPathTextureTracks.find(j).second;
				extrusionPath = extrusionPath.mergeKeysWithTrack(*mPathTextureTracks.find(j).second);
			}
	
			List<std.pair<uint, uint> > segs = mMultiExtrusionPath.getNoIntersectionParts(j);
	
			for (List<std.pair<uint, uint> >.Enumerator it = segs.GetEnumerator(); it.MoveNext(); ++it)
			{
				for (uint i =0; i<mMultiShapeToExtrude.getShapeCount(); i++)
				{
					Shape shapeToExtrude = mMultiShapeToExtrude.getShape(i);
					const Track shapeTextureTrack = 0;
					if (mShapeTextureTracks.find(i) != mShapeTextureTracks.end())
					{
						shapeTextureTrack = mShapeTextureTracks.find(i).second;
						shapeToExtrude.mergeKeysWithTrack(shapeTextureTrack);
					}
					GlobalMembersProceduralExtruder._extrudeBodyImpl(ref buffer, shapeToExtrude, extrusionPath, it.first, it.second, shapeTextureTrack, rotationTrack, scaleTrack, pathTextureTrack);
				}
			}
	
			// Make the intersections
			const List<MultiPath.PathIntersection> intersections = mMultiExtrusionPath.getIntersections();
			for (List<MultiPath.PathIntersection>.Enumerator it = intersections.GetEnumerator(); it.MoveNext(); ++it)
			{
				for (uint i =0; i<mMultiShapeToExtrude.getShapeCount(); i++)
				{
					const Track shapeTextureTrack = 0;
					if (mShapeTextureTracks.find(i) != mShapeTextureTracks.end())
						shapeTextureTrack = mShapeTextureTracks.find(i).second;
					GlobalMembersProceduralExtruder._extrudeIntersectionImpl(ref buffer, it.Current, mMultiExtrusionPath, mMultiShapeToExtrude.getShape(i), shapeTextureTrack);
				}
			}
		}
	}

	//* Sets the shape to extrude. Mutually exclusive with setMultiShapeToExtrude. 
	public Extruder setShapeToExtrude(Shape shapeToExtrude)
	{
		mMultiShapeToExtrude.clear();
		mMultiShapeToExtrude.addShape(shapeToExtrude);
		return this;
	}

	//* Sets the multishape to extrude. Mutually exclusive with setShapeToExtrude. 
	public Extruder setMultiShapeToExtrude(MultiShape multiShapeToExtrude)
	{
		mMultiShapeToExtrude.clear();
		mMultiShapeToExtrude.addMultiShape(multiShapeToExtrude);
		return this;
	}

	//* Sets the extrusion path 
	public Extruder setExtrusionPath(Path extrusionPath)
	{
		mMultiExtrusionPath.clear();
		mMultiExtrusionPath.addPath(extrusionPath);
		mMultiExtrusionPath._calcIntersections();
		return this;
	}

	//* Sets the extrusion multipath 
	public Extruder setExtrusionPath(MultiPath multiExtrusionPath)
	{
		mMultiExtrusionPath.clear();
		mMultiExtrusionPath.addMultiPath(multiExtrusionPath);
		mMultiExtrusionPath._calcIntersections();
		return this;
	}

	//* Sets the rotation track (optional) 
	public Extruder setRotationTrack(Track rotationTrack)
	{
		return setRotationTrack(rotationTrack, 0);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: inline Extruder& setRotationTrack(const Track* rotationTrack, uint index = 0)
	public Extruder setRotationTrack(Track rotationTrack, uint index)
	{
		if (rotationTrack == null && mRotationTracks.find(index) != mRotationTracks.end())
			mRotationTracks.erase(mRotationTracks.find(index));
		if (rotationTrack != null)
			mRotationTracks[index] = rotationTrack;
		return this;
	}

	//* Sets the scale track (optional) 
	public Extruder setScaleTrack(Track scaleTrack)
	{
		return setScaleTrack(scaleTrack, 0);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: inline Extruder& setScaleTrack(const Track* scaleTrack, uint index = 0)
	public Extruder setScaleTrack(Track scaleTrack, uint index)
	{
		if (scaleTrack == null && mScaleTracks.find(index) != mScaleTracks.end())
			mRotationTracks.erase(mScaleTracks.find(index));
		if (scaleTrack != null)
			mScaleTracks[index] = scaleTrack;
		return this;
	}

	/// Sets the track that maps shape points to V texture coords (optional).
	/// Warning : if used with multishape, all shapes will have the same track.
	public Extruder setShapeTextureTrack(Track shapeTextureTrack)
	{
		return setShapeTextureTrack(shapeTextureTrack, 0);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: inline Extruder& setShapeTextureTrack(const Track* shapeTextureTrack, uint index = 0)
	public Extruder setShapeTextureTrack(Track shapeTextureTrack, uint index)
	{
		if (shapeTextureTrack == null && mShapeTextureTracks.find(index) != mShapeTextureTracks.end())
			mShapeTextureTracks.erase(mShapeTextureTracks.find(index));
		if (shapeTextureTrack != null)
			mShapeTextureTracks[index] = shapeTextureTrack;
		return this;
	}

	/// Sets the track that maps path points to V texture coord (optional).
	public Extruder setPathTextureTrack(Track pathTextureTrack)
	{
		return setPathTextureTrack(pathTextureTrack, 0);
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: inline Extruder& setPathTextureTrack(const Track* pathTextureTrack, uint index = 0)
	public Extruder setPathTextureTrack(Track pathTextureTrack, uint index)
	{
		if (pathTextureTrack == null && mPathTextureTracks.find(index) != mPathTextureTracks.end())
			mPathTextureTracks.erase(mPathTextureTracks.find(index));
		if (pathTextureTrack != null)
			mPathTextureTracks[index] = pathTextureTrack;
		return this;
	}

	//* Sets whether caps are added to the extremities or not (not closed paths only) 
	public Extruder setCapped(bool capped)
	{
		mCapped = capped;
		return this;
	}
}
}

