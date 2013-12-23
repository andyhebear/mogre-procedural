using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Builds a plane mesh
// * \image html primitive_plane.png
// * \note Note that X and Y values in that generator are not global X and Y,
// * but are computed to be : X = normal x global X and Y = normal x X
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport PlaneGenerator : public MeshGenerator<PlaneGenerator>
public class PlaneGenerator : MeshGenerator<PlaneGenerator>
{
	private uint mNumSegX;
	private uint mNumSegY;
	private Vector3 mNormal = new Vector3();
	private float mSizeX = 0f;
	private float mSizeY = 0f;

	public PlaneGenerator()
	{
		mNumSegX = 1;
		mNumSegY = 1;
		mNormal = Vector3.UNIT_Y;
		mSizeX = 1;
		mSizeY = 1;
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
		buffer.estimateVertexCount((mNumSegX+1)*(mNumSegY+1));
		buffer.estimateIndexCount(mNumSegX *mNumSegY *6);
		int offset = 0;
	
		Vector3 vX = mNormal.perpendicular();
		Vector3 vY = mNormal.crossProduct(vX);
		Vector3 delta1 = mSizeX / (float)mNumSegX * vX;
		Vector3 delta2 = mSizeY / (float)mNumSegY * vY;
		// build one corner of the square
		Vector3 orig = -0.5f *mSizeX *vX - 0.5f *mSizeY *vY;
	
		for (ushort i1 = 0; i1<=mNumSegX; i1++)
			for (ushort i2 = 0; i2<=mNumSegY; i2++)
			{
				addPoint(buffer, orig+i1 *delta1+i2 *delta2, mNormal, new Vector2(i1/(float)mNumSegX, i2/(float)mNumSegY));
			}
	
		bool reverse = false;
		if (delta1.crossProduct(delta2).dotProduct(mNormal)>0)
			reverse = true;
		for (ushort n1 = 0; n1<mNumSegX; n1++)
		{
			for (ushort n2 = 0; n2<mNumSegY; n2++)
			{
				if (reverse)
				{
					buffer.index(offset+0);
					buffer.index(offset+(mNumSegY+1));
					buffer.index(offset+1);
					buffer.index(offset+1);
					buffer.index(offset+(mNumSegY+1));
					buffer.index(offset+(mNumSegY+1)+1);
				}
				else
				{
					buffer.index(offset+0);
					buffer.index(offset+1);
					buffer.index(offset+(mNumSegY+1));
					buffer.index(offset+1);
					buffer.index(offset+(mNumSegY+1)+1);
					buffer.index(offset+(mNumSegY+1));
				}
				offset++;
			}
			offset++;
		}
	}

//    *
//	Sets the number of segements along local X axis
//	\exception Ogre::InvalidParametersException Minimum of numSegX is 1
//	
	public PlaneGenerator setNumSegX(uint numSegX)
	{
		if (numSegX == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::PlaneGenerator::setNumSegX(unsigned int)", __FILE__, __LINE__);
			throw new Exception("There must be more than 0 numSegX");
            ;
		mNumSegX = numSegX;
		return this;
	}

//    *
//	Sets the number of segments along local Y axis
//	\exception Ogre::InvalidParametersException Minimum of numSegY is 1
//	
	public PlaneGenerator setNumSegY(uint numSegY)
	{
		if (numSegY == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::PlaneGenerator::setNumSegY(unsigned int)", __FILE__, __LINE__);
			throw new Exception("There must be more than 0 numSegY");
            ;
		mNumSegY = numSegY;
		return this;
	}

//    *
//	Sets the normal of the plane
//	\exception Ogre::InvalidParametersException Normal must not be null
//	
	public PlaneGenerator setNormal(Vector3 normal)
	{
		if (mNormal.isZeroLength())
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Normal must not be null", "Procedural::PlaneGenerator::setNormal(unsigned int)", __FILE__, __LINE__);
			throw new Exception("normal must not zero");
            ;
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: mNormal = normal;
		mNormal.CopyFrom(normal);
		return this;
	}

//    *
//	Sets the size of the plane along local X axis
//	\exception Ogre::InvalidParametersException X size must be larger than 0!
//	
	public PlaneGenerator setSizeX(float sizeX)
	{
		if (sizeX <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "X size must be larger than 0!", "Procedural::BoxGenerator::setSizeX(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("sizex must be more than 0 ");
            ;
		mSizeX = sizeX;
		return this;
	}

//    *
//	Sets the size of the plane along local Y axis
//	\exception Ogre::InvalidParametersException Y size must be larger than 0!
//	
	public PlaneGenerator setSizeY(float sizeY)
	{
		if (sizeY <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Y size must be larger than 0!", "Procedural::BoxGenerator::setSizeY(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("sizeY must be more than 0 ");
            ;
		mSizeY = sizeY;
		return this;
	}

	//* Sets the size (default=1,1) 
	public PlaneGenerator setSize(Vector2 size)
	{
		setSizeX(size.x);
		setSizeY(size.y);
		return this;
	}
}
}

