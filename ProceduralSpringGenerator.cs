using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
//-----------------------------------------------------------------------
//*
// * \ingroup pathgrp
// * Produces a helix path
// * \image html spline_helix.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport HelixPath
public class HelixPath
{
	private float mHeight = 0f;
	private float mRadius = 0f;
	private uint mNumSegPath;
	private float mNumRound = 0f;

	/// Default constructor
	public HelixPath()
	{
		mHeight = 1.0f;
		mRadius = 1.0f;
		mNumRound = 5.0f;
		mNumSegPath = 8;
	}

	/// Sets the height of the helix (default=1.0)
	/// \exception Ogre::InvalidParametersException Height must be larger than 0!
	public HelixPath setHeight(float height)
	{
		if (height <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Height must be larger than 0!", "Procedural::HelixPath::setHeight(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("height count must more than 0");
            ;
		mHeight = height;
		return this;
	}

	/// Sets the radius of the helix (default = 1.0)
	/// \exception Ogre::InvalidParametersException Radius must be larger than 0!
	public HelixPath setRadius(float radius)
	{
		if (radius <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::HelixPath::setRadius(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("radius count must more than 0");
            ;
		mRadius = radius;
		return this;
	}

	/// Sets the number of rounds (default = 5.0)
	/// \exception Ogre::InvalidParametersException You have to rotate more then 0 times!
	public HelixPath setNumRound(float numRound)
	{
		if (numRound <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "You have to rotate more then 0 times!", "Procedural::HelixPath::setNumRound(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("numRound count must more than 0");
            ;
		mNumRound = numRound;
		return this;
	}

	/// Sets number of segments along the path per turn
	/// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
	public HelixPath setNumSegPath(uint numSeg)
	{
		if (numSeg == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::HelixPath::setNumSegPath(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSeg count must more than 0");
            ;
		mNumSegPath = numSeg;
		return this;
	}

//    *
//	 * Builds a shape from control points
//	 
	//-----------------------------------------------------------------------
	public Path realizePath()
	{
		Path helix = new Path();
		float angleStep = Math.TWO_PI / (float)(mNumSegPath);
		float heightStep = mHeight / (float)(mNumSegPath);
	
		for (int i =0; i<mNumRound *mNumSegPath; i++)
		{
			helix.addPoint(mRadius * Math.Cos(angleStep * i), heightStep * i, mRadius * Math.Sin(angleStep * i));
		}
	
		return helix;
	}
}

//-----------------------------------------------------------------------
//*
// * \ingroup objgengrp
// * Generates a spring mesh centered on the origin.
// * \image html primitive_spring.png
// 
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport SpringGenerator : public MeshGenerator<SpringGenerator>
public class SpringGenerator : MeshGenerator<SpringGenerator>
{
	private float mHeight = 0f;
	private float mRadiusHelix = 0f;
	private float mRadiusCircle = 0f;
	private int mNumSegPath;
	private int mNumSegCircle;
	private float mNumRound = 0f;

	/// Contructor with arguments
	public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound, int numSegPath) : this(height, radiusHelix, radiusCircle, numRound, numSegPath, 8)
	{
	}
	public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound) : this(height, radiusHelix, radiusCircle, numRound, 10, 8)
	{
	}
	public SpringGenerator(float height, float radiusHelix, float radiusCircle) : this(height, radiusHelix, radiusCircle, 5.0f, 10, 8)
	{
	}
	public SpringGenerator(float height, float radiusHelix) : this(height, radiusHelix, 0.2f, 5.0f, 10, 8)
	{
	}
	public SpringGenerator(float height) : this(height, 1.0f, 0.2f, 5.0f, 10, 8)
	{
	}
	public SpringGenerator() : this(1.0f, 1.0f, 0.2f, 5.0f, 10, 8)
	{
	}
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
//ORIGINAL LINE: SpringGenerator(Ogre::float height=1.0f, Ogre::float radiusHelix=1.0f, Ogre::float radiusCircle=0.2f, Ogre::float numRound=5.0, int numSegPath=10, int numSegCircle=8) : mHeight(height), mRadiusHelix(radiusHelix), mRadiusCircle(radiusCircle), mNumRound(numRound), mNumSegPath(numSegPath), mNumSegCircle(numSegCircle)
	public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound, int numSegPath, int numSegCircle)
	{
		mHeight = height;
		mRadiusHelix = radiusHelix;
		mRadiusCircle = radiusCircle;
		mNumRound = numRound;
		mNumSegPath = numSegPath;
		mNumSegCircle = numSegCircle;
	}

	/// Sets the height of the spring (default=1)
	/// \exception Ogre::InvalidParametersException Height must be larger than 0!
	public SpringGenerator setHeight(float height)
	{
		if (height <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Height must be larger than 0!", "Procedural::SpringGenerator::setHeight(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("height count must more than 0");
            ;
		mHeight = height;
		return this;
	}

	/// Sets helix radius (default=1)
	/// \exception Ogre::InvalidParametersException Radius must be larger than 0!
	public SpringGenerator setRadiusHelix(float radiusHelix)
	{
		if (radiusHelix <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::SpringGenerator::setRadiusHelix(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("radiusHelix count must more than 0");
            ;
		mRadiusHelix = radiusHelix;
		return this;
	}

	/// Sets radius for extruding circle (default=0.1)
	/// \exception Ogre::InvalidParametersException Radius must be larger than 0!
	public SpringGenerator setRadiusCircle(float radiusCircle)
	{
		if (radiusCircle <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::SpringGenerator::setRadiusCircle(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("radiusCirCle count must more than 0");
            ;
		mRadiusCircle = radiusCircle;
		return this;
	}

	/// Sets the number of segments along the height of the spring (default=1)
	/// \exception Ogre::InvalidParametersException You have to rotate more then 0 times!
	public SpringGenerator setNumRound (float numRound)
	{
		if (numRound <= 0.0f)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "You have to rotate more then 0 times!", "Procedural::SpringGenerator::setNumRound(Ogre::Real)", __FILE__, __LINE__);
			throw new Exception("numRound count must more than 0");
            ;
		mNumRound = numRound;
		return this;
	}

	/// Sets the number of segments along helix path (default=10)
	/// \exception Ogre::InvalidParametersException Minimum of numSegPath is 1
	public SpringGenerator setNumSegPath(int numSegPath)
	{
		if (numSegPath == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::SpringGenerator::setNumSegPath(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSegPath count must more than 0");
            ;
		mNumSegPath = numSegPath;
		return this;
	}

	/// Sets the number of segments for extruding circle (default=8)
	/// \exception Ogre::InvalidParametersException Minimum of numSegCircle is 1
	public SpringGenerator setNumSegCircle(int numSegCircle)
	{
		if (numSegCircle == 0)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
			//throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::SpringGenerator::setNumSegCircle(unsigned int)", __FILE__, __LINE__);
			throw new Exception("numSegCircle count must more than 0");
            ;
		mNumSegCircle = numSegCircle;
		return this;
	}

//    *
//	 * Builds the mesh into the given TriangleBuffer
//	 * @param buffer The TriangleBuffer on where to append the mesh.
//	 

	//-----------------------------------------------------------------------
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
	public void addToTriangleBuffer(ref TriangleBuffer buffer)
	{
		Path p = new HelixPath().setHeight(mHeight).setNumRound(mNumRound).setNumSegPath(mNumSegPath).setRadius(mRadiusHelix).realizePath();
	
		Shape s = new CircleShape().setRadius(mRadiusCircle).setNumSeg(mNumSegCircle).realizeShape();
	
		Extruder().setExtrusionPath(p).setShapeToExtrude(s).addToTriangleBuffer(ref buffer);
	}
}

}

