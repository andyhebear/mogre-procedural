using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// \brief Projects all TriangleBufferVertices on a sphere
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport SpherifyModifier
public class SpherifyModifier
{
	private TriangleBuffer mInputTriangleBuffer;
	private Vector3 mCenter = new Vector3();
	private float mRadius = 0f;


	public SpherifyModifier()
	{
		mInputTriangleBuffer = null;
		mCenter = Vector3.ZERO;
		mRadius = 1;
	}

	/// \exception Ogre::InvalidParametersException Input triangle buffer must not be null
	public SpherifyModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		if (inputTriangleBuffer == null)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Input triangle buffer must not be null", "Procedural::SpherifyModifier::setInputTriangleBuffer(Procedural::TriangleBuffer*)", __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must not be null!");
            ;
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	public SpherifyModifier setRadius(float radius)
	{
		if (mRadius <= 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be positive", "Procedural::SpherifyModifier::setInputTriangleBuffer(Procedural::TriangleBuffer*)", __FILE__, __LINE__);
			 throw new Exception("Radius must be larger than 0!");
            ;
		mRadius = radius;
		return this;
	}

	public SpherifyModifier setCenter(Vector3 center)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mCenter = center;
		mCenter.CopyFrom(center);
		return this;
	}

	/// \exception Ogre::InvalidStateException Input triangle buffer must be set
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", "Procedural::SpherifyModifier::modify()", __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
	
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			float l = (it.mPosition - mCenter).length();
			if (l > 1e-6)
			{
				it.mNormal = (it.mPosition - mCenter) / l;
				it.mPosition = mCenter + mRadius * it.mNormal;
			}
		}
	}
}

//--------------------------------------------------------------
//*
//WIP
//
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport CalculateNormalsModifier
public class CalculateNormalsModifier
{
	public CalculateNormalsModifier()
	{
		mComputeMode = NormalComputeMode.NCM_VERTEX;
		mInputTriangleBuffer = null;
		mMustWeldUnweldFirst = true;
	}

	public enum NormalComputeMode: int
	{
		NCM_VERTEX,
		NCM_TRIANGLE
	}

	public NormalComputeMode mComputeMode;
	public TriangleBuffer mInputTriangleBuffer;
	public bool mMustWeldUnweldFirst;

	public CalculateNormalsModifier setComputeMode(NormalComputeMode computeMode)
	{
		mComputeMode = computeMode;
		return this;
	}

	public CalculateNormalsModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

//    *
//	 * Tells if the mesh must be first weld (NCM_VERTEX mode) or unweld (NCM_TRIANGLE) before computing normals.
//	 * Has a performance impact if enabled.
//	 * Default : true.
//	 
	public CalculateNormalsModifier setMustWeldUnweldFirst(bool mustWeldUnweldFirst)
	{
		mMustWeldUnweldFirst = mustWeldUnweldFirst;
		return this;
	}

	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
	
		if (mComputeMode == NormalComputeMode.NCM_TRIANGLE)
		{
			if (mMustWeldUnweldFirst)
				UnweldVerticesModifier().setInputTriangleBuffer(mInputTriangleBuffer).modify();
	
			const List<int> indices = mInputTriangleBuffer.getIndices();
			List<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
			for (int i = 0; i<indices.Count; i+=3)
			{
				Vector3 v1 = vertices[indices[i]].mPosition;
				Vector3 v2 = vertices[indices[i+1]].mPosition;
				Vector3 v3 = vertices[indices[i+2]].mPosition;
				Vector3 n = (v2-v1).crossProduct(v3-v1).normalisedCopy();
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vertices[indices[i]].mNormal = n;
				vertices[indices[i]].mNormal.CopyFrom(n);
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vertices[indices[i+1]].mNormal = n;
				vertices[indices[i+1]].mNormal.CopyFrom(n);
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vertices[indices[i+2]].mNormal = n;
				vertices[indices[i+2]].mNormal.CopyFrom(n);
			}
		}
		else
		{
			if (mMustWeldUnweldFirst)
				WeldVerticesModifier().setInputTriangleBuffer(mInputTriangleBuffer).modify();
			const List<int> indices = mInputTriangleBuffer.getIndices();
			List<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
			List<List<Vector3> > tmpNormals = new List<List<Vector3> >();
			tmpNormals.resize(vertices.Count);
			for (int i = 0; i<indices.Count; i+=3)
			{
				Vector3 v1 = vertices[indices[i]].mPosition;
				Vector3 v2 = vertices[indices[i+1]].mPosition;
				Vector3 v3 = vertices[indices[i+2]].mPosition;
				Vector3 n = (v2-v1).crossProduct(v3-v1);
				tmpNormals[indices[i]].push_back(n);
				tmpNormals[indices[i+1]].push_back(n);
				tmpNormals[indices[i+2]].push_back(n);
			}
			for (int i = 0; i<vertices.Count; i++)
			{
				Vector3 n = new Vector3(Vector3.ZERO);
				for (int j = 0; j<tmpNormals[i].size(); j++)
					n += tmpNormals[i][j];
				vertices[i].mNormal = n.normalisedCopy();
			}
		}
	}
}
//--------------------------------------------------------------
//*
// * Welds together the vertices which are 'close enough' one to each other
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport WeldVerticesModifier
public class WeldVerticesModifier
{
	public WeldVerticesModifier()
	{
		mInputTriangleBuffer = null;
		mTolerance = 1e-3f;
	}

	public TriangleBuffer mInputTriangleBuffer;
	public float mTolerance = 0f;


	/// The triangle buffer to modify
	public WeldVerticesModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	/// The tolerance in position to consider that 2 vertices are the same (default = 1e-3)
	public WeldVerticesModifier setTolerance(float tolerance)
	{
		mTolerance = tolerance;
		return this;
	}

	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
		std.map<Vector3, int, Vector3Comparator> mapExistingVertices = new std.map<Vector3, int, Vector3Comparator>();
		List<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
		List<int> indices = mInputTriangleBuffer.getIndices();
	
		int newSize = vertices.Count;
		for (List<TriangleBuffer.Vertex>.Enumerator it = vertices.GetEnumerator(); it.MoveNext(); ++it)
		{
			int currentIndex = it - vertices.GetEnumerator();
			if (currentIndex>=newSize)
				break;
			if (mapExistingVertices.find(it.mPosition) == mapExistingVertices.end())
				mapExistingVertices[it.mPosition] = currentIndex;
			else
			{
				int existingIndex = mapExistingVertices[it.mPosition];
				--newSize;
				if (currentIndex == newSize)
				{
					for (List<int>.Enumerator it2 = indices.GetEnumerator(); it2.MoveNext(); ++it2)
						if (it2.Current == currentIndex)
							it2.Current = existingIndex;
				}
				else
				{
					int lastIndex = newSize;
					it.Current = vertices[lastIndex];
					for (List<int>.Enumerator it2 = indices.GetEnumerator(); it2.MoveNext(); ++it2)
					{
						if (it2.Current == currentIndex)
							it2.Current = existingIndex;
						else if (it2.Current == lastIndex)
							it2.Current = currentIndex;
					}
				}
			}
		}
		vertices.resize(newSize);
	}
}
//--------------------------------------------------------------
//*
// * \brief Switches the triangle buffer from indexed triangles to (pseudo) triangle list
// * It can be used if you want discontinuities between all your triangles.
//
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport UnweldVerticesModifier
public class UnweldVerticesModifier
{
	public UnweldVerticesModifier()
	{
		mInputTriangleBuffer = null;
	}

	public TriangleBuffer mInputTriangleBuffer;

	public UnweldVerticesModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
		List<TriangleBuffer.Vertex> newVertices = new List<TriangleBuffer.Vertex>();
		const List<TriangleBuffer.Vertex> originVertices = mInputTriangleBuffer.getVertices();
		const List<int> originIndices = mInputTriangleBuffer.getIndices();
		for (int i =0; i<originIndices.Count; i+=3)
		{
			newVertices.Add(originVertices[originIndices[i]]);
			newVertices.Add(originVertices[originIndices[i+1]]);
			newVertices.Add(originVertices[originIndices[i+2]]);
		}
		mInputTriangleBuffer.getVertices().clear();
		mInputTriangleBuffer.getVertices().reserve(newVertices.Count);
		for (List<TriangleBuffer.Vertex>.Enumerator it = newVertices.GetEnumerator(); it.MoveNext(); ++it)
			mInputTriangleBuffer.getVertices().push_back(it.Current);
		mInputTriangleBuffer.getIndices().clear();
		mInputTriangleBuffer.getIndices().reserve(newVertices.Count);
		for (int i =0; i<newVertices.Count; i++)
			mInputTriangleBuffer.getIndices().push_back(i);
	}
}
//--------------------------------------------------------------
//*
// * \brief Recomputes the mesh's UVs based on its projection on a plane
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport PlaneUVModifier
public class PlaneUVModifier
{
	private Vector3 mPlaneNormal = new Vector3();
	private Vector3 mPlaneCenter = new Vector3();
	private Vector2 mPlaneSize = new Vector2();
	private TriangleBuffer mInputTriangleBuffer;

	public PlaneUVModifier()
	{
		mPlaneNormal = Vector3.UNIT_Y;
		mPlaneCenter = Vector3.ZERO;
		mPlaneSize = Vector2.UNIT_SCALE;
		mInputTriangleBuffer = null;
	}

	public PlaneUVModifier setPlaneNormal(Vector3 planeNormal)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPlaneNormal = planeNormal;
		mPlaneNormal.CopyFrom(planeNormal);
		return this;
	}

	public PlaneUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	public PlaneUVModifier setPlaneCenter(Vector3 planeCenter)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPlaneCenter = planeCenter;
		mPlaneCenter.CopyFrom(planeCenter);
		return this;
	}

	public PlaneUVModifier setPlaneSize(Vector2 planeSize)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mPlaneSize = planeSize;
		mPlaneSize.CopyFrom(planeSize);
		return this;
	}

	/// \exception Ogre::InvalidStateException Input triangle buffer must be set
	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
		Vector3 xvec = mPlaneNormal.perpendicular();
		Vector3 yvec = mPlaneNormal.crossProduct(xvec);
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			Vector3 v = it.mPosition - mPlaneCenter;
			it.mUV.x = v.dotProduct(xvec);
			it.mUV.y = v.dotProduct(yvec);
		}
	}
}
//--------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport SphereUVModifier
public class SphereUVModifier
{
	private TriangleBuffer mInputTriangleBuffer;

	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Input triangle buffer must be set!");
            ;
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			Vector3 v = it.mPosition.normalisedCopy();
			Vector2 vxz = new Vector2(v.x, v.z);
			it.mUV.x = Vector2.UNIT_X.angleTo(vxz).valueRadians() / Math.TWO_PI;
			it.mUV.y = (Math.ATan(v.y / vxz.length()).valueRadians() + Math.HALF_PI) / Math.PI;
		}
	}

	public SphereUVModifier()
	{
		mInputTriangleBuffer = null;
	}

	public SphereUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}
}
//--------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport HemisphereUVModifier
public class HemisphereUVModifier
{
	private TriangleBuffer mInputTriangleBuffer;
	private TRect<float> mTextureRectangleTop = new TRect<float>();
	private TRect<float> mTextureRectangleBottom = new TRect<float>();
	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("input triangle buffer must be set!");
            ;
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			Vector3 input = it.mPosition.normalisedCopy();
			Vector3 v = new Vector3();
			Radian r = new Radian();
			if (input.y > 0)
				Vector3.UNIT_Y.getRotationTo(input).ToAngleAxis(ref r, ref v);
			else
				Vector3.NEGATIVE_UNIT_Y.getRotationTo(input).ToAngleAxis(ref r, ref v);
			Vector2 v2 = new Vector2(input.x, input.z);
			v2.normalise();
			Vector2 uv = new Vector2(.5, .5) + .5f * (r / Math.HALF_PI).valueRadians() * v2;
	
			if (input.y > 0)
				it.mUV = Utils.reframe(mTextureRectangleTop, uv);
			else
				it.mUV = Utils.reframe(mTextureRectangleBottom, uv);
		}
	}

	public HemisphereUVModifier()
	{
		mInputTriangleBuffer = null;
		mTextureRectangleTop = new TRect(0, 0, 1, 1);
		mTextureRectangleBottom = new TRect(0, 0, 1, 1);
	}

	public HemisphereUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	public HemisphereUVModifier setTextureRectangleTop(TRect<float> textureRectangleTop)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mTextureRectangleTop = textureRectangleTop;
		mTextureRectangleTop.CopyFrom(textureRectangleTop);
		return this;
	}

	public HemisphereUVModifier setTextureRectangleBottom(TRect<float> textureRectangleBottom)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mTextureRectangleBottom = textureRectangleBottom;
		mTextureRectangleBottom.CopyFrom(textureRectangleBottom);
		return this;
	}

}
//--------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport CylinderUVModifier
public class CylinderUVModifier
{
	private TriangleBuffer mInputTriangleBuffer;
	private float mHeight = 0f;
	private float mRadius = 0f;
	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("input triangle buffer must be set!");
            ;
		if (mHeight <=0)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Height must be strictly positive", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("mHeight must be larger than 0!");
            ;
		if (mRadius <= 0)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Radius must be strictly positive", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("Radius must be larger than 0!");
            ;
	
		float angleThreshold = Math.ATan(mHeight / mRadius).valueRadians();
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			Vector2 nxz = new Vector2(it.mNormal.x, it.mNormal.z);
			float alpha = (Math.ATan(it.mNormal.y / nxz.length()).valueRadians() + Math.HALF_PI);
			if (Math.Abs(alpha) > angleThreshold)
			{
				Vector2 vxz = new Vector2(it.mPosition.x, it.mPosition.z);
				it.mUV = vxz / mRadius;
			}
			else
			{
				Vector2 vxz = new Vector2(it.mPosition.x, it.mPosition.z);
				it.mUV.x = Vector2.UNIT_X.angleTo(vxz).valueRadians()/Math.TWO_PI;
				it.mUV.y = it.mPosition.y/mHeight - 0.5f;
			}
		}
	}

	public CylinderUVModifier()
	{
		mInputTriangleBuffer = 0;
		mRadius = 1.0f;
		mHeight = 1.0f;
	}

	public CylinderUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	public CylinderUVModifier setRadius(float radius)
	{
		mRadius = radius;
		return this;
	}

	public CylinderUVModifier setHeight(float height)
	{
		mHeight = height;
		return this;
	}

}
//--------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport BoxUVModifier
public class BoxUVModifier
{
	public enum MappingType: int
	{
		MT_FULL,
		MT_CROSS,
		MT_PACKED,
	}
	private TriangleBuffer mInputTriangleBuffer;
	private MappingType mMappingType;
	private Vector3 mBoxSize = new Vector3();
	private Vector3 mBoxCenter = new Vector3();

	//--------------------------------------------------------------
	public void modify()
	{
		if (mInputTriangleBuffer == null)
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
	//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
			 throw new Exception("input triangle buffer must be set!");
            ;
	
		Vector3[] directions = { Vector3.UNIT_X, Vector3.UNIT_Y, Vector3.UNIT_Z,Vector3.NEGATIVE_UNIT_X, Vector3.NEGATIVE_UNIT_Y, Vector3.NEGATIVE_UNIT_Z };
	
		for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
		{
			Vector3 v = it.mPosition - mBoxCenter;
			if (v.isZeroLength())
				continue;
			//v.normalise();
			v.x/=mBoxSize.x;
			v.y/=mBoxSize.y;
			v.z/=mBoxSize.z;
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
//ORIGINAL LINE: Vector3 n = it->mNormal;
			Vector3 n = new Vector3(it.mNormal);
			float maxAxis = 0;
			int principalAxis = 0;
			for (byte i = 0; i < 6; i++)
			{
				if (directions[i].dotProduct(n) > maxAxis)
				{
					maxAxis = directions[i].dotProduct(n);
					principalAxis = i;
				}
			}
	
			Vector3 vX = new Vector3();
			Vector3 vY = new Vector3();
			if (principalAxis%3 == 1)
				vY = Vector3.UNIT_X;
			else
				vY = Vector3.UNIT_Y;
			vX = directions[principalAxis].crossProduct(vY);
	
			Vector2 uv = new Vector2(0.5-vX.dotProduct(v), 0.5-vY.dotProduct(v));
			if (mMappingType == MappingType.MT_FULL)
				it.mUV = uv;
			else if (mMappingType == MappingType.MT_CROSS)
			{
			}
			else if (mMappingType == MappingType.MT_PACKED)
				it.mUV = new Vector2((uv.x + principalAxis%3)/3, (uv.y + principalAxis/3)/2);
		}
	}

	public BoxUVModifier()
	{
		mInputTriangleBuffer = 0;
		mMappingType = MappingType.MT_FULL;
		mBoxSize = Vector3.UNIT_SCALE;
		mBoxCenter = Vector3.ZERO;
	}

	public BoxUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer)
	{
		mInputTriangleBuffer = inputTriangleBuffer;
		return this;
	}

	public BoxUVModifier setBoxSize(Vector3 boxSize)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mBoxSize = boxSize;
		mBoxSize.CopyFrom(boxSize);
		return this;
	}

	public BoxUVModifier setBoxCenter(Vector3 boxCenter)
	{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mBoxCenter = boxCenter;
		mBoxCenter.CopyFrom(boxCenter);
		return this;
	}

	public BoxUVModifier setMappingType(MappingType mappingType)
	{
		mMappingType = mappingType;
		return this;
	}
}
}


