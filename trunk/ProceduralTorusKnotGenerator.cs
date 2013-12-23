using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//*
// * \ingroup objgengrp
// * Builds a torus knot mesh
// * \image html primitive_torusknot.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport TorusKnotGenerator : public MeshGenerator<TorusKnotGenerator>
public class TorusKnotGenerator : MeshGenerator<TorusKnotGenerator>
{
	private uint mNumSegSection;
	private uint mNumSegCircle;
	private float mRadius = 0f;
	private float mSectionRadius = 0f;
	private int mP;
	private int mQ;
	/// Constructor with arguments
	public TorusKnotGenerator(float radius, float sectionRadius, int p, int q, uint numSegSection) : this(radius, sectionRadius, p, q, numSegSection, 16)
	{
	}
	public TorusKnotGenerator(float radius, float sectionRadius, int p, int q) : this(radius, sectionRadius, p, q, 8, 16)
	{
	}
	public TorusKnotGenerator(float radius, float sectionRadius, int p) : this(radius, sectionRadius, p, 3, 8, 16)
	{
	}
	public TorusKnotGenerator(float radius, float sectionRadius) : this(radius, sectionRadius, 2, 3, 8, 16)
	{
	}
	public TorusKnotGenerator(float radius) : this(radius, 0.2f, 2, 3, 8, 16)
	{
	}
	public TorusKnotGenerator() : this(1.0f, 0.2f, 2, 3, 8, 16)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: TorusKnotGenerator(Ogre::float radius=1.0f, Ogre::float sectionRadius=.2f, int p=2, int q=3, uint numSegSection=8, uint numSegCircle=16) : mNumSegSection(numSegSection), mNumSegCircle(numSegCircle), mRadius(radius), mSectionRadius(sectionRadius), mP(p), mQ(q)
	public TorusKnotGenerator(float radius, float sectionRadius, int p, int q, uint numSegSection, uint numSegCircle)
	{
		mNumSegSection = numSegSection;
		mNumSegCircle = numSegCircle;
		mRadius = radius;
		mSectionRadius = sectionRadius;
		mP = p;
		mQ = q;
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
		buffer.estimateVertexCount((mNumSegCircle *mP+1)*(mNumSegSection+1));
		buffer.estimateIndexCount((mNumSegCircle *mP)*(mNumSegSection+1)*6);
	
		int offset = 0;
	
		for (uint i = 0; i <= mNumSegCircle * mP; i++)
		{
			float phi = Math.TWO_PI * i/(float)mNumSegCircle;
			float x0 = mRadius*(2 + Math.Cos(mQ *phi/(float)mP)) * Math.Cos(phi) / 3.0f;
			float y0 = mRadius *Math.Sin(mQ *phi/(float)mP) / 3.0f;
			float z0 = mRadius*(2 + Math.Cos(mQ *phi/(float)mP)) * Math.Sin(phi) / 3.0f;
	
			float phi1 = Math.TWO_PI * (i+1)/(float)mNumSegCircle;
			float x1 = mRadius*(2 + Math.Cos(mQ *phi1/(float)mP)) * Math.Cos(phi1) / 3.0f;
			float y1 = mRadius *Math.Sin(mQ *phi1/mP) / 3.0f;
			float z1 = mRadius*(2 + Math.Cos(mQ *phi1/(float)mP)) * Math.Sin(phi1) / 3.0f;
	
			Vector3 v0 = new Vector3(x0, y0, z0);
			Vector3 v1 = new Vector3(x1, y1, z1);
			Vector3 direction = new Vector3((v1-v0).normalisedCopy());
	
			Quaternion q = Utils._computeQuaternion(direction);
	
			for (uint j =0; j<=mNumSegSection; j++)
			{
				float alpha = Math.TWO_PI *j/mNumSegSection;
				Vector3 vp = mSectionRadius*(q * new Vector3(Math.Cos(alpha), Math.Sin(alpha), 0));
	
				addPoint(buffer, v0+vp, vp.normalisedCopy(), new Vector2(i/(float)mNumSegCircle, j/(float)mNumSegSection));
	
				if (i != mNumSegCircle * mP)
				{
					buffer.index(offset + mNumSegSection + 1);
					buffer.index(offset + mNumSegSection);
					buffer.index(offset);
					buffer.index(offset + mNumSegSection + 1);
					buffer.index(offset);
					buffer.index(offset + 1);
				}
				offset ++;
			}
		}
	}

//    *
//	Sets the number of segments along the section (default=8)
//	\exception Ogre::InvalidParametersException Minimum of numSegCircle is 1
//	
	public TorusKnotGenerator setNumSegSection(uint numSegSection)
	{
		if (numSegSection == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::TorusKnotGenerator::setNumSegSection(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSegSection must more than 0");
            ;
		mNumSegSection = numSegSection;
		return this;
	}

//    *
//	Sets the number of segments along the circle (default=16)
//	\exception Ogre::InvalidParametersException Minimum of numSegCircle is 1
//	
	public TorusKnotGenerator setNumSegCircle(uint numSegCircle)
	{
		if (numSegCircle == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::TorusKnotGenerator::setNumSegCircle(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSegCircle must more than 0");
            ;
		mNumSegCircle = numSegCircle;
		return this;
	}

//    *
//	Sets the main radius of the knot (default=1)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public TorusKnotGenerator setRadius(float radius)
	{
		if (radius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::TorusKnotGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("radius must more than 0");
            ;
		mRadius = radius;
		return this;
	}

//    *
//	Sets the section radius (default=0.2)
//	\exception Ogre::InvalidParametersException Radius must be larger than 0!
//	
	public TorusKnotGenerator setSectionRadius(float sectionRadius)
	{
		if (sectionRadius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::TorusKnotGenerator::setSectionRadius(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("sectionRadius must more than 0");
            ;
		mSectionRadius = sectionRadius;
		return this;
	}

//    *
//	Sets the p parameter of the knot (default=2)
//	\exception Ogre::InvalidParametersException Parameter p must be larger than 0!
//	
	public TorusKnotGenerator setP(int p)
	{
		if (p <= 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Parameter p must be larger than 0!", "Procedural::TorusKnotGenerator::setP(int)", __FILE__, __LINE__);
			throw new Exception("parameter p must more than 0");
            ;
		mP = p;
		return this;
	}

//    *
//	Sets the q parameter of the knot (default=3)
//	\exception Ogre::InvalidParametersException Parameter q must be larger than 0!
//	
	public TorusKnotGenerator setQ(int q)
	{
		if (q <= 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Parameter q must be larger than 0!", "Procedural::TorusKnotGenerator::setQ(int)", __FILE__, __LINE__);
			throw new Exception("parameter q must more than 0");
            ;
		mQ = q;
		return this;
	}

}
}
