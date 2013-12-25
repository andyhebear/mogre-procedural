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
    // * Generates a box mesh centered on the origin.
    // * Default size is 1.0 with 1 quad per face.
    // * \image html primitive_box.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport BoxGenerator : public MeshGenerator<BoxGenerator>
    public class BoxGenerator : MeshGenerator<BoxGenerator>
    {
        private float mSizeX = 0f;
        private float mSizeY = 0f;
        private float mSizeZ = 0f;
        private uint mNumSegX;
        private uint mNumSegY;
        private uint mNumSegZ;
        /// Contructor with arguments
        public BoxGenerator(float sizeX, float sizeY, float sizeZ, uint numSegX, uint numSegY)
            : this(sizeX, sizeY, sizeZ, numSegX, numSegY, 1) {
        }
        public BoxGenerator(float sizeX, float sizeY, float sizeZ, uint numSegX)
            : this(sizeX, sizeY, sizeZ, numSegX, 1, 1) {
        }
        public BoxGenerator(float sizeX, float sizeY, float sizeZ)
            : this(sizeX, sizeY, sizeZ, 1, 1, 1) {
        }
        public BoxGenerator(float sizeX, float sizeY)
            : this(sizeX, sizeY, 1.0f, 1, 1, 1) {
        }
        public BoxGenerator(float sizeX)
            : this(sizeX, 1.0f, 1.0f, 1, 1, 1) {
        }
        public BoxGenerator()
            : this(1.0f, 1.0f, 1.0f, 1, 1, 1) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: BoxGenerator(Ogre::float sizeX=1.0f, Ogre::float sizeY=1.0f, Ogre::float sizeZ=1.0f, uint numSegX=1, uint numSegY=1, uint numSegZ=1) : mSizeX(sizeX), mSizeY(sizeY), mSizeZ(sizeZ), mNumSegX(numSegX), mNumSegY(numSegY), mNumSegZ(numSegZ)
        public BoxGenerator(float sizeX, float sizeY, float sizeZ, uint numSegX, uint numSegY, uint numSegZ) {
            mSizeX = sizeX;
            mSizeY = sizeY;
            mSizeZ = sizeZ;
            mNumSegX = numSegX;
            mNumSegY = numSegY;
            mNumSegZ = numSegZ;
        }

        //    *
        //	Sets size along X axis (default=1)
        //	\exception Ogre::InvalidParametersException X size must be larger than 0!
        //	
        public BoxGenerator setSizeX(float sizeX) {
            if (sizeX <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "X size must be larger than 0!", "Procedural::BoxGenerator::setSizeX(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("X size must be larger than 0!");
            ;
            mSizeX = sizeX;
            return this;
        }

        //    *
        //	Sets size along Y axis (default=1)
        //	\exception Ogre::InvalidParametersException Y size must be larger than 0!
        //	
        public BoxGenerator setSizeY(float sizeY) {
            if (sizeY <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Y size must be larger than 0!", "Procedural::BoxGenerator::setSizeY(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Y size must be larger than 0!");
            ;
            mSizeY = sizeY;
            return this;
        }

        //    *
        //	Sets size along Z axis (default=1)
        //	\exception Ogre::InvalidParametersException Z size must be larger than 0!
        //	
        public BoxGenerator setSizeZ(float sizeZ) {
            if (sizeZ <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Z size must be larger than 0!", "Procedural::BoxGenerator::setSizeZ(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Z size must be larger than 0!");
            ;
            mSizeZ = sizeZ;
            return this;
        }

        //* Sets the size (default=1,1,1) 
        public BoxGenerator setSize(Vector3 size) {
            setSizeX(size.x);
            setSizeY(size.y);
            setSizeZ(size.z);
            return this;
        }

        //    *
        //	Sets the number of segments along X axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegX is 1
        //	
        public BoxGenerator setNumSegX(uint numSegX) {
            if (numSegX == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BoxGenerator::setNumSegX(unsigned int)", __FILE__, __LINE__);
                throw new Exception(" setNumSegX(uint numSegX)There must be more than 0 segments");
            ;
            mNumSegX = numSegX;
            return this;
        }

        //    *
        //	Sets the number of segments along Y axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegY is 1
        //	
        public BoxGenerator setNumSegY(uint numSegY) {
            if (numSegY == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BoxGenerator::setNumSegY(unsigned int)", __FILE__, __LINE__);
                throw new Exception(" setNumSegY(uint numSegY)There must be more than 0 segments");
            ;
            mNumSegY = numSegY;
            return this;
        }

        //    *
        //	Sets the number of segments along Z axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegZ is 1
        //	
        public BoxGenerator setNumSegZ(uint numSegZ) {
            if (numSegZ == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BoxGenerator::setNumSegZ(unsigned int)", __FILE__, __LINE__);
                throw new Exception(" setNumSegZ(uint numSegZ)There must be more than 0 segments");
            ;
            mNumSegZ = numSegZ;
            return this;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer) {
            PlaneGenerator pg = new PlaneGenerator();
            pg.setUTile(mUTile).setVTile(mVTile);
            if (mTransform != null) {
                pg.setScale(mScale);
                pg.setOrientation(mOrientation);
            }
            pg.setNumSegX(mNumSegY).setNumSegY(mNumSegX).setSizeX(mSizeY).setSizeY(mSizeX).setNormal(Vector3.NEGATIVE_UNIT_Z).setPosition(mScale * (mPosition + .5f * mSizeZ * (mOrientation * Vector3.NEGATIVE_UNIT_Z))).addToTriangleBuffer(ref buffer);
            pg.setNumSegX(mNumSegY).setNumSegY(mNumSegX).setSizeX(mSizeY).setSizeY(mSizeX).setNormal(Vector3.UNIT_Z).setPosition(mScale * (mPosition + .5f * mSizeZ * (mOrientation * Vector3.UNIT_Z))).addToTriangleBuffer(ref buffer);
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegX).setSizeX(mSizeZ).setSizeY(mSizeX).setNormal(Vector3.NEGATIVE_UNIT_Y).setPosition(mScale * (mPosition + .5f * mSizeY * (mOrientation * Vector3.NEGATIVE_UNIT_Y))).addToTriangleBuffer(ref buffer);
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegX).setSizeX(mSizeZ).setSizeY(mSizeX).setNormal(Vector3.UNIT_Y).setPosition(mScale * (mPosition + .5f * mSizeY * (mOrientation * Vector3.UNIT_Y))).addToTriangleBuffer(ref buffer);
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegY).setSizeX(mSizeZ).setSizeY(mSizeY).setNormal(Vector3.NEGATIVE_UNIT_X).setPosition(mScale * (mPosition + .5f * mSizeX * (mOrientation * Vector3.NEGATIVE_UNIT_X))).addToTriangleBuffer(ref buffer);
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegY).setSizeX(mSizeZ).setSizeY(mSizeY).setNormal(Vector3.UNIT_X).setPosition(mScale * (mPosition + .5f * mSizeX * (mOrientation * Vector3.UNIT_X))).addToTriangleBuffer(ref buffer);
        }

    }


}

