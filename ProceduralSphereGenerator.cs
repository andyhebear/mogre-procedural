using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Builds an UV sphere mesh
// * \image html primitive_sphere.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport SphereGenerator : public MeshGenerator<SphereGenerator>
public class SphereGenerator : MeshGenerator<SphereGenerator>
{
	private float mRadius = 0f;
	private uint mNumRings;
	private uint mNumSegments;

	/// Constructor with arguments
	public SphereGenerator(float radius, uint numRings) : this(radius, numRings, 16)
	{
	}
	public SphereGenerator(float radius) : this(radius, 16, 16)
	{
	}
	public SphereGenerator() : this(1.0f, 16, 16)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: SphereGenerator(Ogre::float radius = 1.0f, uint numRings = 16, uint numSegments = 16) : mRadius(radius),mNumRings(numRings), mNumSegments(numSegments)
	public SphereGenerator(float radius, uint numRings, uint numSegments)

	{
		mRadius = radius;
		mNumRings = numRings;
		mNumSegments = numSegments;
	}

//    *
//	Sets the radius of the sphere (default=1)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public SphereGenerator setRadius(float radius)
	{
		if (radius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::SphereGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("radius count must more than 0");
            ;
		mRadius = radius;
		return this;
	}

//    *
//	Sets the number of rings (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numRings is 1
//	
	public SphereGenerator setNumRings(uint numRings)
	{
		if (numRings == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 rings", "Procedural::SphereGenerator::setNumRings(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numRings count must more than 0");
            ;
		mNumRings = numRings;
		return this;
	}

//    *
//	Sets the number of segments (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numSegments is 1
//	
	public SphereGenerator setNumSegments(uint numSegments)
	{
		if (numSegments == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::SphereGenerator::setNumSegments(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSegments count must more than 0");
            ;
		mNumSegments = numSegments;
		return this;
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
		buffer.estimateVertexCount((mNumRings+1)*(mNumSegments+1));
		buffer.estimateIndexCount(mNumRings*(mNumSegments+1)*6);
	
		float fDeltaRingAngle = (Math.PI / mNumRings);
		float fDeltaSegAngle = (Math.TWO_PI / mNumSegments);
		int offset = 0;
	
		// Generate the group of rings for the sphere
		for (uint ring = 0; ring <= mNumRings; ring++)
		{
			float r0 = mRadius * sinf (ring * fDeltaRingAngle);
			float y0 = mRadius * cosf (ring * fDeltaRingAngle);
	
			// Generate the group of segments for the current ring
			for (uint seg = 0; seg <= mNumSegments; seg++)
			{
				float x0 = r0 * sinf(seg * fDeltaSegAngle);
				float z0 = r0 * cosf(seg * fDeltaSegAngle);
	
				// Add one vertex to the strip which makes up the sphere
				addPoint(buffer, new Vector3(x0, y0, z0), new Vector3(x0, y0, z0).normalisedCopy(), new Vector2((float) seg / (float) mNumSegments, (float) ring / (float) mNumRings));
	
				if (ring != mNumRings)
				{
					if (seg != mNumSegments)
					{
						// each vertex (except the last) has six indices pointing to it
						if (ring != mNumRings-1)
							buffer.triangle(offset + mNumSegments + 2, offset, offset + mNumSegments + 1);
						if (ring != 0)
							buffer.triangle(offset + mNumSegments + 2, offset + 1, offset);
					}
					offset ++;
				}
			} // end for seg
		} // end for ring
	}

}
}

