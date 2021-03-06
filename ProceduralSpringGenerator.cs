/*
-----------------------------------------------------------------------------
This source file is part of mogre-procedural
For the latest info, see http://code.google.com/p/mogre-procedural/
my blog:http://hi.baidu.com/rainssoft
this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/
   
Copyright (c) 2013-2020 rains soft

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
-----------------------------------------------------------------------------
*/
//write ok

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
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
        public HelixPath() {
            mHeight = 1.0f;
            mRadius = 1.0f;
            mNumRound = 5.0f;
            mNumSegPath = 8;
        }

        /// Sets the height of the helix (default=1.0)
        /// \exception Ogre::InvalidParametersException Height must be larger than 0!
        public HelixPath setHeight(float height) {
            if (height <= 0.0f)
              OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Height must be larger than 0!", "Procedural::HelixPath::setHeight(Ogre::Real)");
            ;
            mHeight = height;
            return this;
        }

        private void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p+"_"+p_2+"_"+p_3);
        }

        /// Sets the radius of the helix (default = 1.0)
        /// \exception Ogre::InvalidParametersException Radius must be larger than 0!
        public HelixPath setRadius(float radius) {
            if (radius <= 0.0f)
             OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::HelixPath::setRadius(Ogre::Real)");
            ;
            mRadius = radius;
            return this;
        }

        /// Sets the number of rounds (default = 5.0)
        /// \exception Ogre::InvalidParametersException You have to rotate more then 0 times!
        public HelixPath setNumRound(float numRound) {
            if (numRound <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "You have to rotate more then 0 times!", "Procedural::HelixPath::setNumRound(Ogre::Real)");
            ;
            mNumRound = numRound;
            return this;
        }

        /// Sets number of segments along the path per turn
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public HelixPath setNumSegPath(uint numSeg) {
            if (numSeg == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::HelixPath::setNumSegPath(unsigned int)");
            ;
            mNumSegPath = numSeg;
            return this;
        }

        //    *
        //	 * Builds a shape from control points
        //	 
        //-----------------------------------------------------------------------
        public Path realizePath() {
            Path helix = new Path();
            float angleStep = Math.TWO_PI / (float)(mNumSegPath);
            float heightStep = mHeight / (float)(mNumSegPath);

            for (int i = 0; i < mNumRound * mNumSegPath; i++) {
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
        public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound, int numSegPath)
            : this(height, radiusHelix, radiusCircle, numRound, numSegPath, 8) {
        }
        public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound)
            : this(height, radiusHelix, radiusCircle, numRound, 10, 8) {
        }
        public SpringGenerator(float height, float radiusHelix, float radiusCircle)
            : this(height, radiusHelix, radiusCircle, 5.0f, 10, 8) {
        }
        public SpringGenerator(float height, float radiusHelix)
            : this(height, radiusHelix, 0.2f, 5.0f, 10, 8) {
        }
        public SpringGenerator(float height)
            : this(height, 1.0f, 0.2f, 5.0f, 10, 8) {
        }
        public SpringGenerator()
            : this(1.0f, 1.0f, 0.2f, 5.0f, 10, 8) {
        }
       //
        //ORIGINAL LINE: SpringGenerator(Ogre::float height=1.0f, Ogre::float radiusHelix=1.0f, Ogre::float radiusCircle=0.2f, Ogre::float numRound=5.0, int numSegPath=10, int numSegCircle=8) : mHeight(height), mRadiusHelix(radiusHelix), mRadiusCircle(radiusCircle), mNumRound(numRound), mNumSegPath(numSegPath), mNumSegCircle(numSegCircle)
        public SpringGenerator(float height, float radiusHelix, float radiusCircle, float numRound, int numSegPath, int numSegCircle) {
            mHeight = height;
            mRadiusHelix = radiusHelix;
            mRadiusCircle = radiusCircle;
            mNumRound = numRound;
            mNumSegPath = numSegPath;
            mNumSegCircle = numSegCircle;
        }

        /// Sets the height of the spring (default=1)
        /// \exception Ogre::InvalidParametersException Height must be larger than 0!
        public SpringGenerator setHeight(float height) {
            if (height <= 0.0f)
              OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Height must be larger than 0!", "Procedural::SpringGenerator::setHeight(Ogre::Real)");
            ;
            mHeight = height;
            return this;
        }

        /// Sets helix radius (default=1)
        /// \exception Ogre::InvalidParametersException Radius must be larger than 0!
        public SpringGenerator setRadiusHelix(float radiusHelix) {
            if (radiusHelix <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::SpringGenerator::setRadiusHelix(Ogre::Real)");
            ;
            mRadiusHelix = radiusHelix;
            return this;
        }

        /// Sets radius for extruding circle (default=0.1)
        /// \exception Ogre::InvalidParametersException Radius must be larger than 0!
        public SpringGenerator setRadiusCircle(float radiusCircle) {
            if (radiusCircle <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::SpringGenerator::setRadiusCircle(Ogre::Real)");
            ;
            mRadiusCircle = radiusCircle;
            return this;
        }

        /// Sets the number of segments along the height of the spring (default=1)
        /// \exception Ogre::InvalidParametersException You have to rotate more then 0 times!
        public SpringGenerator setNumRound(float numRound) {
            if (numRound <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "You have to rotate more then 0 times!", "Procedural::SpringGenerator::setNumRound(Ogre::Real)");
            ;
            mNumRound = numRound;
            return this;
        }

        /// Sets the number of segments along helix path (default=10)
        /// \exception Ogre::InvalidParametersException Minimum of numSegPath is 1
        public SpringGenerator setNumSegPath(int numSegPath) {
            if (numSegPath == 0)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::SpringGenerator::setNumSegPath(unsigned int)");
            ;
            mNumSegPath = numSegPath;
            return this;
        }

        /// Sets the number of segments for extruding circle (default=8)
        /// \exception Ogre::InvalidParametersException Minimum of numSegCircle is 1
        public SpringGenerator setNumSegCircle(int numSegCircle) {
            if (numSegCircle == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::SpringGenerator::setNumSegCircle(unsigned int)");
            ;
            mNumSegCircle = numSegCircle;
            return this;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 

        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            Path p = new HelixPath().setHeight(mHeight).setNumRound(mNumRound).setNumSegPath((uint)mNumSegPath).setRadius(mRadiusHelix).realizePath();

            Shape s = new CircleShape().setRadius(mRadiusCircle).setNumSeg((uint)mNumSegCircle).realizeShape();

            new Extruder().setExtrusionPath(p).setShapeToExtrude(s).addToTriangleBuffer(ref buffer);
        }
    }

}

