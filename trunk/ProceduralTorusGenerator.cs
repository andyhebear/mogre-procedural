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
    // * Builds a torus mesh whose axis is Y
    // * \image html primitive_torus.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TorusGenerator : public MeshGenerator<TorusGenerator>
    /// <summary>
    /// 圆环 圆环体 环面 
    /// </summary>
    public class TorusGenerator : MeshGenerator<TorusGenerator>
    {
        private uint mNumSegSection;
        private uint mNumSegCircle;
        private float mRadius = 0f;
        private float mSectionRadius = 0f;
        /// Constructor with arguments
        public TorusGenerator(float radius, float sectionRadius, uint numSegSection)
            : this(radius, sectionRadius, numSegSection, 16) {
        }
        public TorusGenerator(float radius, float sectionRadius)
            : this(radius, sectionRadius, 16, 16) {
        }
        public TorusGenerator(float radius)
            : this(radius, .2f, 16, 16) {
        }
        public TorusGenerator()
            : this(1.0f, 0.2f, 16, 16) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: TorusGenerator(Ogre::float radius=1.0f, Ogre::float sectionRadius=.2f, uint numSegSection=16, uint numSegCircle=16) : mNumSegSection(numSegSection), mNumSegCircle(numSegCircle), mRadius(radius), mSectionRadius(sectionRadius)
        public TorusGenerator(float radius, float sectionRadius, uint numSegSection, uint numSegCircle) {
            mNumSegSection = numSegSection;
            mNumSegCircle = numSegCircle;
            mRadius = radius;
            mSectionRadius = sectionRadius;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer) {
            buffer.rebaseOffset();
            buffer.estimateVertexCount((mNumSegCircle + 1) * (mNumSegSection + 1));
            buffer.estimateIndexCount((mNumSegCircle) * (mNumSegSection + 1) * 6);

            float deltaSection = (Math.TWO_PI / mNumSegSection);
            float deltaCircle = (Math.TWO_PI / mNumSegCircle);
            int offset = 0;

            for (uint i = 0; i <= mNumSegCircle; i++)
                for (uint j = 0; j <= mNumSegSection; j++) {
                    Vector3 c0 = new Vector3(mRadius, 0.0f, 0.0f);
                    Vector3 v0 = new Vector3(mRadius + mSectionRadius * cosf(j * deltaSection), mSectionRadius * sinf(j * deltaSection), 0.0f);
                    Quaternion q = new Quaternion();
                    q.FromAngleAxis(new Radian(i * deltaCircle), Vector3.UNIT_Y);
                    Vector3 v = q * v0;
                    Vector3 c = q * c0;
                    addPoint(ref buffer, v, (v - c).NormalisedCopy, new Vector2(i / (float)mNumSegCircle, j / (float)mNumSegSection));

                    if (i != mNumSegCircle) {
                        buffer.index(offset + (int)mNumSegSection + 1);
                        buffer.index(offset);
                        buffer.index(offset + (int)mNumSegSection);
                        buffer.index(offset + (int)mNumSegSection + 1);
                        buffer.index(offset + 1);
                        buffer.index(offset);
                    }
                    offset++;
                }
        }

        //    *
        //	Sets the number of segments on the section circle
        //	\exception Ogre::InvalidParametersException Minimum of numSegSection is 1
        //	
        public TorusGenerator setNumSegSection(uint numSegSection) {
            if (mNumSegSection == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::TorusGenerator::setNumSegSection(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSegSection must more than 0");
            ;
            mNumSegSection = numSegSection;
            return this;
        }

        //    *
        //	Sets the number of segments along the guiding circle
        //	\exception Ogre::InvalidParametersException Minimum of numSegCircle is 1
        //	
        public TorusGenerator setNumSegCircle(uint numSegCircle) {
            if (numSegCircle == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::TorusGenerator::setNumSegCircle(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSegCircle must more than 0");
            ;
            mNumSegCircle = numSegCircle;
            return this;
        }

        //    *
        //	Sets the radius of the guiding circle
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public TorusGenerator setRadius(float radius) {
            if (radius <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::TorusGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("radius must more than 0");
            ;
            mRadius = radius;
            return this;
        }

        //    *
        //	Sets the radius of the section circle
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public TorusGenerator setSectionRadius(float sectionRadius) {
            if (sectionRadius <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::TorusGenerator::setSectionRadius(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("sectionRadius must more than 0");
            ;
            mSectionRadius = sectionRadius;
            return this;
        }

    }
}

