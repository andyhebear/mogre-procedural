using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Generates a cone mesh along Y-axis
// * \image html primitive_cone.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport ConeGenerator : public MeshGenerator<ConeGenerator>
public class ConeGenerator : MeshGenerator<ConeGenerator>
{
	private uint mNumSegBase;
	private uint mNumSegHeight;
	private float mRadius = 0f;
	private float mHeight = 0f;
	/// Contructor with arguments
	public ConeGenerator(float radius, float height, uint numSegBase) : this(radius, height, numSegBase, 1)
	{
	}
	public ConeGenerator(float radius, float height) : this(radius, height, 16, 1)
	{
	}
	public ConeGenerator(float radius) : this(radius, 1.0f, 16, 1)
	{
	}
	public ConeGenerator() : this(1.0f, 1.0f, 16, 1)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: ConeGenerator(Ogre::float radius = 1.0f, Ogre::float height = 1.0f, uint numSegBase = 16, uint numSegHeight = 1) : mNumSegBase(numSegBase), mNumSegHeight(numSegHeight), mRadius(radius), mHeight(height)
	public ConeGenerator(float radius, float height, uint numSegBase, uint numSegHeight)
	{
		mNumSegBase = numSegBase;
		mNumSegHeight = numSegHeight;
		mRadius = radius;
		mHeight = height;
	}

//    *
//	 * Builds the mesh into the given TriangleBuffer
//	 * @param buffer The TriangleBuffer on where to append the mesh.
//	 
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
	public void addToTriangleBuffer(ref TriangleBuffer buffer)
	{
		buffer.rebaseOffset();
		buffer.estimateVertexCount((mNumSegHeight+1)*(mNumSegBase+1)+mNumSegBase+2);
		buffer.estimateIndexCount(mNumSegHeight *mNumSegBase *6+3 *mNumSegBase);
	
		float deltaAngle = (Math.TWO_PI / mNumSegBase);
		float deltaHeight = mHeight/(float)mNumSegHeight;
		int offset = 0;
	
		Vector3 refNormal = new Vector3(mRadius, mHeight, 0.0f);
		Quaternion q = new Quaternion();
	
		for (uint i = 0; i <=mNumSegHeight; i++)
		{
			float r0 = mRadius * (1 - i / (float)mNumSegHeight);
			for (uint j = 0; j<=mNumSegBase; j++)
			{
				float x0 = r0* cosf(j *deltaAngle);
				float z0 = r0 * sinf(j *deltaAngle);
	
				q.FromAngleAxis(new Radian(-deltaAngle *j), Vector3.UNIT_Y);
	
				addPoint(buffer, new Vector3(x0, i *deltaHeight, z0), q *refNormal, new Vector2(j/(float)mNumSegBase, i/(float)mNumSegHeight));
	
				if (i != mNumSegHeight&& j != mNumSegBase)
				{
					buffer.index(offset + mNumSegBase + 2);
					buffer.index(offset);
					buffer.index(offset + mNumSegBase+1);
					buffer.index(offset + mNumSegBase ++2);
					buffer.index(offset + 1);
					buffer.index(offset);
				}
	
				offset ++;
			}
		}
	
		//low cap
		int centerIndex = offset;
		addPoint(buffer, Vector3.ZERO, Vector3.NEGATIVE_UNIT_Y, Vector2.UNIT_Y);
		offset++;
		for (uint j =0; j<=mNumSegBase; j++)
		{
			float x0 = mRadius * cosf(j *deltaAngle);
			float z0 = mRadius * sinf(j *deltaAngle);
	
			addPoint(buffer, new Vector3(x0, 0.0f, z0), Vector3.NEGATIVE_UNIT_Y, new Vector2(j/(float)mNumSegBase, 0.0));
	
			if (j!=mNumSegBase)
			{
				buffer.index(centerIndex);
				buffer.index(offset);
				buffer.index(offset+1);
			}
			offset++;
		}
	}

//    *
//	Sets the number of segments on the side of the base (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numSegBase is 1
//	
	public ConeGenerator setNumSegBase(uint numSegBase)
	{
		if (numSegBase == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::ConeGenerator::setNumSegBase(unsigned int)", __FILE__, __LINE__);
			 throw new Exception("numSegBase must be larger than 0!");
            ;
		mNumSegBase = numSegBase;
		return this;
	}

//    *
//	Sets the number of segments on the height (default=1)
//	\exception Ogre::InvalidParametersException Minimum of numSegHeight is 1
//	
	public ConeGenerator setNumSegHeight(uint numSegHeight)
	{
		if (numSegHeight == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::ConeGenerator::setNumSegHeight(unsigned int)", __FILE__, __LINE__);
			 throw new Exception("numSegHeight must be larger than 0!");
            ;
		mNumSegHeight = numSegHeight;
		return this;
	}

//    *
//	Sets the base radius (default=1)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public ConeGenerator setRadius(float radius)
	{
		if (radius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::ConeGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
			 throw new Exception("radius must be larger than 0!");
            ;
		mRadius = radius;
		return this;
	}

//    *
//	Sets the height of the cone (default=1)
//	\exception Ogre::InvalidParametersException Height must be larger than 0!
//	
	public ConeGenerator setHeight(float height)
	{
		if (height <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Height must be larger than 0!", "Procedural::ConeGenerator::setHeight(Ogre::Real)", __FILE__, __LINE__);
			 throw new Exception("height must be larger than 0!");
            ;
		mHeight = height;
		return this;
	}


}
}


