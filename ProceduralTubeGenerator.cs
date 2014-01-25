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
//write with new std ... ok
namespace Mogre_Procedural
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    //*
    // * \ingroup objgengrp
    // * Builds an Y-axis tube mesh, i.e. an emptied cylinder
    // * \image html primitive_tube.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TubeGenerator : public MeshGenerator<TubeGenerator>
    public class TubeGenerator : MeshGenerator<TubeGenerator>
    {
        private uint mNumSegBase;
        private uint mNumSegHeight;
        private float mOuterRadius = 0f;
        private float mInnerRadius = 0f;
        private float mHeight = 0f;

        /// Constructor with arguments
        public TubeGenerator(float outerRadius, float innerRadius, float height, uint numSegBase)
            : this(outerRadius, innerRadius, height, numSegBase, 1) {
        }
        public TubeGenerator(float outerRadius, float innerRadius, float height)
            : this(outerRadius, innerRadius, height, 16, 1) {
        }
        public TubeGenerator(float outerRadius, float innerRadius)
            : this(outerRadius, innerRadius, 1.0f, 16, 1) {
        }
        public TubeGenerator(float outerRadius)
            : this(outerRadius, 1.0f, 1.0f, 16, 1) {
        }
        public TubeGenerator()
            : this(2.0f, 1.0f, 1.0f, 16, 1) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: TubeGenerator(Ogre::float outerRadius=2.0f, Ogre::float innerRadius=1.0f, Ogre::float height=1.0f, uint numSegBase=16, uint numSegHeight=1) : mNumSegBase(numSegBase), mNumSegHeight(numSegHeight), mOuterRadius(outerRadius), mInnerRadius(innerRadius), mHeight(height)
        public TubeGenerator(float outerRadius, float innerRadius, float height, uint numSegBase, uint numSegHeight) {
            mNumSegBase = numSegBase;
            mNumSegHeight = numSegHeight;
            mOuterRadius = outerRadius;
            mInnerRadius = innerRadius;
            mHeight = height;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            buffer.rebaseOffset();
            buffer.estimateVertexCount((mNumSegHeight + 1) * (mNumSegBase + 1) * 2 + (mNumSegBase + 1) * 4);
            buffer.estimateIndexCount(6 * (mNumSegBase + 1) * mNumSegHeight * 2 + 6 * mNumSegBase * 2);

            float deltaAngle = (Math.TWO_PI / mNumSegBase);
            float deltaHeight = mHeight / (float)mNumSegHeight;
            int offset = 0;

            for (uint i = 0; i <= mNumSegHeight; i++)
                for (uint j = 0; j <= mNumSegBase; j++) {
                    float x0 = mOuterRadius * cosf(j * deltaAngle);
                    float z0 = mOuterRadius * sinf(j * deltaAngle);
                    addPoint(ref buffer, new Vector3(x0, i * deltaHeight, z0), new Vector3(x0, 0, z0).NormalisedCopy, new Vector2(j / (float)mNumSegBase, i / (float)mNumSegHeight));

                    if (i != mNumSegHeight) {
                        buffer.index(offset + (int)mNumSegBase + 1);
                        buffer.index(offset);
                        buffer.index(offset + (int)mNumSegBase);
                        buffer.index(offset + (int)mNumSegBase + 1);
                        buffer.index(offset + 1);
                        buffer.index(offset);
                    }
                    offset++;
                }

            for (uint i = 0; i <= mNumSegHeight; i++)
                for (uint j = 0; j <= mNumSegBase; j++) {
                    float x0 = mInnerRadius * cosf(j * deltaAngle);
                    float z0 = mInnerRadius * sinf(j * deltaAngle);
                    addPoint(ref buffer, new Vector3(x0, i * deltaHeight, z0), -new Vector3(x0, 0, z0).NormalisedCopy, new Vector2(j / (float)mNumSegBase, i / (float)mNumSegHeight));

                    if (i != mNumSegHeight) {
                        buffer.index(offset + (int)mNumSegBase + 1);
                        buffer.index(offset + (int)mNumSegBase);
                        buffer.index(offset);
                        buffer.index(offset + (int)mNumSegBase + 1);
                        buffer.index(offset);
                        buffer.index(offset + 1);
                    }
                    offset++;
                }


            //low cap
            for (uint j = 0; j <= mNumSegBase; j++) {
                float x0 = mInnerRadius * cosf(j * deltaAngle);
                float z0 = mInnerRadius * sinf(j * deltaAngle);

                addPoint(ref buffer, new Vector3(x0, 0.0f, z0), Vector3.NEGATIVE_UNIT_Y, new Vector2(j / (float)mNumSegBase, 1.0f));

                x0 = mOuterRadius * cosf(j * deltaAngle);
                z0 = mOuterRadius * sinf(j * deltaAngle);

                addPoint(ref buffer, new Vector3(x0, 0.0f, z0), Vector3.NEGATIVE_UNIT_Y, new Vector2(j / (float)mNumSegBase, 0.0f));

                if (j != mNumSegBase) {
                    buffer.index(offset);
                    buffer.index(offset + 1);
                    buffer.index(offset + 3);
                    buffer.index(offset + 2);
                    buffer.index(offset);
                    buffer.index(offset + 3);
                }
                offset += 2;
            }


            //high cap
            for (uint j = 0; j <= mNumSegBase; j++) {
                float x0 = mInnerRadius * cosf(j * deltaAngle);
                float z0 = mInnerRadius * sinf(j * deltaAngle);

                addPoint(ref buffer, new Vector3(x0, mHeight, z0), Vector3.UNIT_Y, new Vector2(j / (float)mNumSegBase, 0.0f));

                x0 = mOuterRadius * cosf(j * deltaAngle);
                z0 = mOuterRadius * sinf(j * deltaAngle);

                addPoint(ref buffer, new Vector3(x0, mHeight, z0), Vector3.UNIT_Y, new Vector2(j / (float)mNumSegBase, 1.0f));

                if (j != mNumSegBase) {
                    buffer.index(offset + 1);
                    buffer.index(offset);
                    buffer.index(offset + 3);
                    buffer.index(offset);
                    buffer.index(offset + 2);
                    buffer.index(offset + 3);
                }
                offset += 2;
            }
        }

        //    *
        //	Sets the number of segments when rotating around the tube's axis (default=16)
        //	\exception Ogre::InvalidParametersException Minimum of numSegBase is 1
        //	
        public TubeGenerator setNumSegBase(uint numSegBase) {
            if (numSegBase == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::TubeGenerator::setNumSegBase(unsigned int)"); ;
            mNumSegBase = numSegBase;
            return this;
        }

        //    *
        //	Sets the number of segments along the height of the cylinder (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegHeight is 1
        //	
        public TubeGenerator setNumSegHeight(uint numSegHeight) {
            if (numSegHeight == 0)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::TubeGenerator::setNumSegHeight(unsigned int)");
            ;
            mNumSegHeight = numSegHeight;
            return this;
        }

        //    *
        //	Sets the outer radius of the tube (default=2)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	\exception Ogre::InvalidParametersException Outer radius must be bigger than inner radius
        //	
        public TubeGenerator setOuterRadius(float outerRadius) {
            if (outerRadius <= 0.0f)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::TubeGenerator::setOuterRadius(Ogre::Real)");
            if (outerRadius < mInnerRadius)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Outer radius must be bigger than inner radius!", "Procedural::TubeGenerator::setOuterRadius(Ogre::Real)"); ;
            mOuterRadius = outerRadius;
            return this;
        }

        //    *
        //	Sets the inner radius of the tube (default=1)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	\exception Ogre::InvalidParametersException Outer radius must be bigger than inner radius
        //	
        public TubeGenerator setInnerRadius(float innerRadius) {
            if (innerRadius <= 0.0f)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::TubeGenerator::setInnerRadius(Ogre::Real)");
            if (mOuterRadius < innerRadius)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Outer radius must be bigger than inner radius!", "Procedural::TubeGenerator::setInnerRadius(Ogre::Real)");
            ;
            mInnerRadius = innerRadius;
            return this;
        }

        //    *
        //	Sets the height of the tube (default=1)
        //	\exception Ogre::InvalidParametersException Height must be larger than 0!
        //	
        public TubeGenerator setHeight(float height) {
            if (height <= 0.0f)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Height must be larger than 0!", "Procedural::TubeGenerator::setHeight(Ogre::Real)");
            ;
            mHeight = height;
            return this;
        }
    }
}
