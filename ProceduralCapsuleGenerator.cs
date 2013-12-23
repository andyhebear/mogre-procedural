using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Generates a capsule mesh, i.e. a sphere-terminated cylinder
// * \image html primitive_capsule.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport CapsuleGenerator : public MeshGenerator<CapsuleGenerator>
public class CapsuleGenerator : MeshGenerator<CapsuleGenerator>
{
	///Radius of the spheric part
	private float mRadius = 0f;

	///Total height
	private float mHeight = 0f;

	private uint mNumRings;
	private uint mNumSegments;
	private uint mNumSegHeight;

	/// Default constructor
	public CapsuleGenerator()
	{
		mRadius = 1.0f;
		mHeight = 1.0f;
		mNumRings = 8;
		mNumSegments = 16;
		mNumSegHeight = 1;
	}

	/// Constructor with arguments
	public CapsuleGenerator(float radius, float height, uint numRings, uint numSegments, uint numSegHeight)
	{
		mRadius = radius;
		mHeight = height;
		mNumRings = numRings;
		mNumSegments = numSegments;
		mNumSegHeight = numSegHeight;
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
		buffer.estimateVertexCount((2 *mNumRings+2)*(mNumSegments+1) + (mNumSegHeight-1)*(mNumSegments+1));
		buffer.estimateIndexCount((2 *mNumRings+1)*(mNumSegments+1)*6 + (mNumSegHeight-1)*(mNumSegments+1)*6);
	
		float fDeltaRingAngle = (Math.HALF_PI / mNumRings);
		float fDeltaSegAngle = (Math.TWO_PI / mNumSegments);
	
		float sphereRatio = mRadius / (2 * mRadius + mHeight);
		float cylinderRatio = mHeight / (2 * mRadius + mHeight);
		int offset = 0;
		// Top half sphere
	
		// Generate the group of rings for the sphere
		for (uint ring = 0; ring <= mNumRings; ring++)
		{
			float r0 = mRadius * sinf (ring * fDeltaRingAngle);
			float y0 = mRadius * cosf (ring * fDeltaRingAngle);
	
			// Generate the group of segments for the current ring
			for (uint seg = 0; seg <= mNumSegments; seg++)
			{
				float x0 = r0 * cosf(seg * fDeltaSegAngle);
				float z0 = r0 * sinf(seg * fDeltaSegAngle);
	
				// Add one vertex to the strip which makes up the sphere
				addPoint(buffer, new Vector3(x0, 0.5f *mHeight + y0, z0), new Vector3(x0, y0, z0).normalisedCopy(), new Vector2((float) seg / (float) mNumSegments, (float) ring / (float) mNumRings * sphereRatio));
	
				// each vertex (except the last) has six indices pointing to it
				buffer.index(offset + mNumSegments + 1);
				buffer.index(offset + mNumSegments);
				buffer.index(offset);
				buffer.index(offset + mNumSegments + 1);
				buffer.index(offset);
				buffer.index(offset + 1);
	
				offset ++;
			} // end for seg
		} // end for ring
	
		// Cylinder part
		float deltaAngle = (Math.TWO_PI / mNumSegments);
		float deltamHeight = mHeight/(float)mNumSegHeight;
	
		for (ushort i = 1; i < mNumSegHeight; i++)
			for (ushort j = 0; j<=mNumSegments; j++)
			{
				float x0 = mRadius * cosf(j *deltaAngle);
				float z0 = mRadius * sinf(j *deltaAngle);
	
				addPoint(buffer, new Vector3(x0, 0.5f *mHeight-i *deltamHeight, z0), new Vector3(x0, 0, z0).normalisedCopy(), new Vector2(j/(float)mNumSegments, i/(float)mNumSegHeight * cylinderRatio + sphereRatio));
	
				buffer.index(offset + mNumSegments + 1);
				buffer.index(offset + mNumSegments);
				buffer.index(offset);
				buffer.index(offset + mNumSegments + 1);
				buffer.index(offset);
				buffer.index(offset + 1);
	
				offset ++;
			}
	
		// Bottom half sphere
	
		// Generate the group of rings for the sphere
		for (uint ring = 0; ring <= mNumRings; ring++)
		{
			float r0 = mRadius * sinf (Math.HALF_PI + ring * fDeltaRingAngle);
			float y0 = mRadius * cosf (Math.HALF_PI + ring * fDeltaRingAngle);
	
			// Generate the group of segments for the current ring
			for (uint seg = 0; seg <= mNumSegments; seg++)
			{
				float x0 = r0 * cosf(seg * fDeltaSegAngle);
				float z0 = r0 * sinf(seg * fDeltaSegAngle);
	
				// Add one vertex to the strip which makes up the sphere
				addPoint(buffer, new Vector3(x0, -0.5f *mHeight + y0, z0), new Vector3(x0, y0, z0).normalisedCopy(), new Vector2((float) seg / (float) mNumSegments, (float) ring / (float) mNumRings *sphereRatio + cylinderRatio + sphereRatio));
	
				if (ring != mNumRings)
				{
					// each vertex (except the last) has six indices pointing to it
					buffer.index(offset + mNumSegments + 1);
					buffer.index(offset + mNumSegments);
					buffer.index(offset);
					buffer.index(offset + mNumSegments + 1);
					buffer.index(offset);
					buffer.index(offset + 1);
				}
				offset ++;
			} // end for seg
		} // end for ring
	}

//    *
//	Sets the radius of the cylinder part (default=1)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public CapsuleGenerator setRadius(float radius)
	{
        if (radius <= 0.0f)
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
            //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::CapsuleGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
            throw new Exception("Radius must be larger than 0!");
            ;
		mRadius = radius;
		return this;
	}

//    *
//	Sets the number of segments of the sphere part (default=8)
//	\exception Ogre::InvalidParametersException Minimum of numRings is 1
//	
	public CapsuleGenerator setNumRings(uint numRings)
	{
		if (numRings == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 rings", "Procedural::CapsuleGenerator::setNumRings(unsigned int)", __FILE__, __LINE__);
            throw new Exception("numRings must be larger than 0!");
            ;
		mNumRings = numRings;
		return this;
	}

//    *
//	Sets the number of segments when rotating around the cylinder (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numSegments is 1
//	
	public CapsuleGenerator setNumSegments(uint numSegments)
	{
		if (numSegments == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::CapsuleGenerator::setNumSegments(unsigned int)", __FILE__, __LINE__);
            throw new Exception("numSegments must be larger than 0!");
            ;
		mNumSegments = numSegments;
		return this;
	}

//    *
//	Sets the number of segments along the axis of the cylinder (default=1)
//	\exception Ogre::InvalidParametersException Minimum of numSeg is 1
//	
	public CapsuleGenerator setNumSegHeight(uint numSegHeight)
	{
		if (numSegHeight == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::CapsuleGenerator::setNumSegHeight(unsigned int)", __FILE__, __LINE__);
            throw new Exception("numSegHeight must be larger than 0!");
            ;
		mNumSegHeight = numSegHeight;
		return this;
	}

//    *
//	Sets the height of the cylinder part of the capsule (default=1)
//	\exception Ogre::InvalidParametersException Height must be larger than 0!
//	
	public CapsuleGenerator setHeight(float height)
	{
		if (height <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Height must be larger than 0!", "Procedural::CapsuleGenerator::setHeight(Ogre::Real)", __FILE__, __LINE__);
            throw new Exception("height must be larger than 0!");
            ;
		mHeight = height;
		return this;
	}


}
}


