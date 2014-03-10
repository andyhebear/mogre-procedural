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

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
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
        public SphereGenerator(float radius, uint numRings)
            : this(radius, numRings, 16) {
        }
        public SphereGenerator(float radius)
            : this(radius, 16, 16) {
        }
        public SphereGenerator()
            : this(1.0f, 16, 16) {
        }
       //
        //ORIGINAL LINE: SphereGenerator(Ogre::float radius = 1.0f, uint numRings = 16, uint numSegments = 16) : mRadius(radius),mNumRings(numRings), mNumSegments(numSegments)
        public SphereGenerator(float radius, uint numRings, uint numSegments) {
            mRadius = radius;
            mNumRings = numRings;
            mNumSegments = numSegments;
        }

        //    *
        //	Sets the radius of the sphere (default=1)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public SphereGenerator setRadius(float radius) {
            if (radius <= 0.0f)
              OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::SphereGenerator::setRadius(Ogre::Real)");
            ;
            mRadius = radius;
            return this;
        }

        //    *
        //	Sets the number of rings (default=16)
        //	\exception Ogre::InvalidParametersException Minimum of numRings is 1
        //	
        public SphereGenerator setNumRings(uint numRings) {
            if (numRings == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 rings", "Procedural::SphereGenerator::setNumRings(unsigned int)");
            ;
            mNumRings = numRings;
            return this;
        }

        //    *
        //	Sets the number of segments (default=16)
        //	\exception Ogre::InvalidParametersException Minimum of numSegments is 1
        //	
        public SphereGenerator setNumSegments(uint numSegments) {
            if (numSegments == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::SphereGenerator::setNumSegments(unsigned int)");
            ;
            mNumSegments = numSegments;
            return this;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override  void addToTriangleBuffer(ref TriangleBuffer buffer) {
            buffer.rebaseOffset();
            buffer.estimateVertexCount((mNumRings + 1) * (mNumSegments + 1));
            buffer.estimateIndexCount(mNumRings * (mNumSegments + 1) * 6);

            float fDeltaRingAngle = (Math.PI / mNumRings);
            float fDeltaSegAngle = (Math.TWO_PI / mNumSegments);
            int offset = 0;

            // Generate the group of rings for the sphere
            for (uint ring = 0; ring <= mNumRings; ring++) {
                float r0 = mRadius * sinf(ring * fDeltaRingAngle);
                float y0 = mRadius * cosf(ring * fDeltaRingAngle);

                // Generate the group of segments for the current ring
                for (uint seg = 0; seg <= mNumSegments; seg++) {
                    float x0 = r0 * sinf(seg * fDeltaSegAngle);
                    float z0 = r0 * cosf(seg * fDeltaSegAngle);

                    // Add one vertex to the strip which makes up the sphere
                    addPoint(ref buffer, new Vector3(x0, y0, z0), new Vector3(x0, y0, z0).NormalisedCopy, new Vector2((float)seg / (float)mNumSegments, (float)ring / (float)mNumRings));

                    if (ring != mNumRings) {
                        if (seg != mNumSegments) {
                            // each vertex (except the last) has six indices pointing to it
                            if (ring != mNumRings - 1)
                                buffer.triangle(offset + (int)mNumSegments + 2, offset, offset + (int)mNumSegments + 1);
                            if (ring != 0)
                                buffer.triangle(offset + (int)mNumSegments + 2, offset + 1, offset);
                        }
                        offset++;
                    }
                } // end for seg
            } // end for ring
        }

    }
}

