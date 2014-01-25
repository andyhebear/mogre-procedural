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
//#ifndef PROCEDURAL_CAPSULE_GENERATOR_INCLUDED
#define PROCEDURAL_CAPSULE_GENERATOR_INCLUDED
// write with new std ..... ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
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
        public CapsuleGenerator() {
            mRadius = 1.0f;
            mHeight = 1.0f;
            mNumRings = 8;
            mNumSegments = 16;
            mNumSegHeight = 1;
        }

        /// Constructor with arguments
        public CapsuleGenerator(float radius, float height, uint numRings, uint numSegments, uint numSegHeight) {
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
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer) {
            buffer.rebaseOffset();
            buffer.estimateVertexCount((2 * mNumRings + 2) * (mNumSegments + 1) + (mNumSegHeight - 1) * (mNumSegments + 1));
            buffer.estimateIndexCount((2 * mNumRings + 1) * (mNumSegments + 1) * 6 + (mNumSegHeight - 1) * (mNumSegments + 1) * 6);

            float fDeltaRingAngle = (Math.HALF_PI / mNumRings);
            float fDeltaSegAngle = (Math.TWO_PI / mNumSegments);

            float sphereRatio = mRadius / (2 * mRadius + mHeight);
            float cylinderRatio = mHeight / (2 * mRadius + mHeight);
            int offset = 0;
            // Top half sphere

            // Generate the group of rings for the sphere
            for (uint ring = 0; ring <= mNumRings; ring++) {
                float r0 = mRadius * sinf(ring * fDeltaRingAngle);
                float y0 = mRadius * cosf(ring * fDeltaRingAngle);

                // Generate the group of segments for the current ring
                for (uint seg = 0; seg <= mNumSegments; seg++) {
                    float x0 = r0 * cosf(seg * fDeltaSegAngle);
                    float z0 = r0 * sinf(seg * fDeltaSegAngle);

                    // Add one vertex to the strip which makes up the sphere
                    addPoint(ref buffer, new Vector3(x0, 0.5f * mHeight + y0, z0), new Vector3(x0, y0, z0).NormalisedCopy, new Vector2((float)seg / (float)mNumSegments, (float)ring / (float)mNumRings * sphereRatio));

                    // each vertex (except the last) has six indices pointing to it
                    buffer.index(offset + (int)mNumSegments + 1);
                    buffer.index(offset + (int)mNumSegments);
                    buffer.index(offset);
                    buffer.index(offset + (int)mNumSegments + 1);
                    buffer.index(offset);
                    buffer.index(offset + 1);

                    offset++;
                } // end for seg
            } // end for ring

            // Cylinder part
            float deltaAngle = (Math.TWO_PI / mNumSegments);
            float deltamHeight = mHeight / (float)mNumSegHeight;

            for (ushort i = 1; i < mNumSegHeight; i++)
                for (ushort j = 0; j <= mNumSegments; j++) {
                    float x0 = mRadius * cosf(j * deltaAngle);
                    float z0 = mRadius * sinf(j * deltaAngle);

                    addPoint(ref buffer, new Vector3(x0, 0.5f * mHeight - i * deltamHeight, z0), new Vector3(x0, 0, z0).NormalisedCopy, new Vector2(j / (float)mNumSegments, i / (float)mNumSegHeight * cylinderRatio + sphereRatio));

                    buffer.index(offset + (int)mNumSegments + 1);
                    buffer.index(offset + (int)mNumSegments);
                    buffer.index(offset);
                    buffer.index(offset + (int)mNumSegments + 1);
                    buffer.index(offset);
                    buffer.index(offset + 1);

                    offset++;
                }

            // Bottom half sphere

            // Generate the group of rings for the sphere
            for (uint ring = 0; ring <= mNumRings; ring++) {
                float r0 = mRadius * sinf(Math.HALF_PI + ring * fDeltaRingAngle);
                float y0 = mRadius * cosf(Math.HALF_PI + ring * fDeltaRingAngle);

                // Generate the group of segments for the current ring
                for (uint seg = 0; seg <= mNumSegments; seg++) {
                    float x0 = r0 * cosf(seg * fDeltaSegAngle);
                    float z0 = r0 * sinf(seg * fDeltaSegAngle);

                    // Add one vertex to the strip which makes up the sphere
                    addPoint(ref buffer, new Vector3(x0, -0.5f * mHeight + y0, z0), new Vector3(x0, y0, z0).NormalisedCopy, new Vector2((float)seg / (float)mNumSegments, (float)ring / (float)mNumRings * sphereRatio + cylinderRatio + sphereRatio));

                    if (ring != mNumRings) {
                        // each vertex (except the last) has six indices pointing to it
                        buffer.index(offset + (int)mNumSegments + 1);
                        buffer.index(offset + (int)mNumSegments);
                        buffer.index(offset);
                        buffer.index(offset + (int)mNumSegments + 1);
                        buffer.index(offset);
                        buffer.index(offset + 1);
                    }
                    offset++;
                } // end for seg
            } // end for ring
        }

        //    *
        //	Sets the radius of the cylinder part (default=1)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public CapsuleGenerator setRadius(float radius) {
            if (radius <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::CapsuleGenerator::setRadius(Ogre::Real)");
            ;
            mRadius = radius;
            return this;
        }

        //    *
        //	Sets the number of segments of the sphere part (default=8)
        //	\exception Ogre::InvalidParametersException Minimum of numRings is 1
        //	
        public CapsuleGenerator setNumRings(uint numRings) {
            if (numRings == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 rings", "Procedural::CapsuleGenerator::setNumRings(unsigned int)");
            ;
            mNumRings = numRings;
            return this;
        }

        //    *
        //	Sets the number of segments when rotating around the cylinder (default=16)
        //	\exception Ogre::InvalidParametersException Minimum of numSegments is 1
        //	
        public CapsuleGenerator setNumSegments(uint numSegments) {
            if (numSegments == 0)
              	OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::CapsuleGenerator::setNumSegments(unsigned int)");
            ;
            mNumSegments = numSegments;
            return this;
        }

        //    *
        //	Sets the number of segments along the axis of the cylinder (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSeg is 1
        //	
        public CapsuleGenerator setNumSegHeight(uint numSegHeight) {
            if (numSegHeight == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::CapsuleGenerator::setNumSegHeight(unsigned int)");
            ;
            mNumSegHeight = numSegHeight;
            return this;
        }

        //    *
        //	Sets the height of the cylinder part of the capsule (default=1)
        //	\exception Ogre::InvalidParametersException Height must be larger than 0!
        //	
        public CapsuleGenerator setHeight(float height) {
            if (height <= 0.0f)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Height must be larger than 0!", "Procedural::CapsuleGenerator::setHeight(Ogre::Real)");
            ;
            mHeight = height;
            return this;
        }


    }
}


