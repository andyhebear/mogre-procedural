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
    // * Builds a rounded box.
    // * You can choose the size of the rounded borders to get a sharper or smoother look.
    // * \image html primitive_roundedbox.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport RoundedBoxGenerator : public MeshGenerator<RoundedBoxGenerator>
    public class RoundedBoxGenerator : MeshGenerator<RoundedBoxGenerator>
    {
        private float mSizeX = 0f;
        private float mSizeY = 0f;
        private float mSizeZ = 0f;
        private ushort mNumSegX;
        private ushort mNumSegY;
        private ushort mNumSegZ;
        private float mChamferSize = 0f;
        private ushort mChamferNumSeg;

        public RoundedBoxGenerator() {
            mSizeX = 1.0f;
            mSizeY = 1.0f;
            mSizeZ = 1.0f;
            mNumSegX = 1;
            mNumSegY = 1;
            mNumSegZ = 1;
            mChamferSize = 0.1f;
            mChamferNumSeg = 8;
        }

        //    *
        //	Sets the size of the box along X axis
        //	\exception Ogre::InvalidParametersException X size must be larger than 0!
        //	
        public RoundedBoxGenerator setSizeX(float sizeX) {
            if (sizeX <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "X size must be larger than 0!", "Procedural::RoundedBoxGenerator::setSizeX(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("sizeX must be more than 0 ");
            ;
            mSizeX = sizeX;
            return this;
        }

        //    *
        //	Sets the size of the box along Y axis
        //	\exception Ogre::InvalidParametersException Y size must be larger than 0!
        //	
        public RoundedBoxGenerator setSizeY(float sizeY) {
            if (sizeY <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "X size must be larger than 0!", "Procedural::RoundedBoxGenerator::setSizeY(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("sizeY must be more than 0 ");
            ;
            mSizeY = sizeY;
            return this;
        }

        //    *
        //	Sets the size of the box along Z axis
        //	\exception Ogre::InvalidParametersException Z size must be larger than 0!
        //	
        public RoundedBoxGenerator setSizeZ(float sizeZ) {
            if (sizeZ <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Z size must be larger than 0!", "Procedural::RoundedBoxGenerator::setSizeZ(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("sizeZ must be more than 0 ");
            ;
            mSizeZ = sizeZ;
            return this;
        }

        //* Sets the size (default=1,1,1) 
        public RoundedBoxGenerator setSize(Vector3 size) {
            setSizeX(size.x);
            setSizeY(size.y);
            setSizeZ(size.z);
            return this;
        }

        //    *
        //	Sets the number of segments along X axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegX is 1
        //	
        public RoundedBoxGenerator setNumSegX(ushort numSegX) {
            if (numSegX == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::RoundedBoxGenerator::setNumSegX(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSegY must be more than 0 ");
            ;
            mNumSegX = numSegX;
            return this;
        }

        //    *
        //	Sets the number of segments along Y axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegY is 1
        //	
        public RoundedBoxGenerator setNumSegY(ushort numSegY) {
            if (numSegY == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::RoundedBoxGenerator::setNumSegY(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSegY must be more than 0 ");
            ;
            mNumSegY = numSegY;
            return this;
        }

        //    *
        //	Sets the number of segments along Z axis (default=1)
        //	\exception Ogre::InvalidParametersException Minimum of numSegZ is 1
        //	
        public RoundedBoxGenerator setNumSegZ(ushort numSegZ) {
            if (numSegZ == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::RoundedBoxGenerator::setNumSegZ(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSegZ must be more than 0 ");
            ;
            mNumSegZ = numSegZ;
            return this;
        }

        //    *
        //	Sets the size of the chamfer, ie the radius of the rounded part
        //	\exception Ogre::InvalidParametersException chamferSize must be larger than 0!
        //	
        public RoundedBoxGenerator setChamferSize(float chamferSize) {
            if (chamferSize <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Chamfer size must be larger than 0!", "Procedural::RoundedBoxGenerator::setChamferSize(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("chamferSize must be more than 0 ");
            ;
            mChamferSize = chamferSize;
            return this;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            //int offset = 0;
            // Generate the pseudo-box shape
            PlaneGenerator pg = new PlaneGenerator();
            pg.setUTile(mUTile).setVTile(mVTile);
            if (mTransform != null) {
                pg.setScale(mScale);
                pg.setOrientation(mOrientation);
            }

            pg.setNumSegX(mNumSegY).setNumSegY(mNumSegX).setSizeX(mSizeY).setSizeY(mSizeX).setNormal(Vector3.NEGATIVE_UNIT_Z).setPosition((.5f * mSizeZ + mChamferSize) * (mOrientation * Vector3.NEGATIVE_UNIT_Z)).addToTriangleBuffer(ref buffer);
            buffer.rebaseOffset();
            pg.setNumSegX(mNumSegY).setNumSegY(mNumSegX).setSizeX(mSizeY).setSizeY(mSizeX).setNormal(Vector3.UNIT_Z).setPosition((.5f * mSizeZ + mChamferSize) * (mOrientation * Vector3.UNIT_Z)).addToTriangleBuffer(ref buffer);
            buffer.rebaseOffset();
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegX).setSizeX(mSizeZ).setSizeY(mSizeX).setNormal(Vector3.NEGATIVE_UNIT_Y).setPosition((.5f * mSizeY + mChamferSize) * (mOrientation * Vector3.NEGATIVE_UNIT_Y)).addToTriangleBuffer(ref buffer);
            buffer.rebaseOffset();
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegX).setSizeX(mSizeZ).setSizeY(mSizeX).setNormal(Vector3.UNIT_Y).setPosition((.5f * mSizeY + mChamferSize) * (mOrientation * Vector3.UNIT_Y)).addToTriangleBuffer(ref buffer);
            buffer.rebaseOffset();
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegY).setSizeX(mSizeZ).setSizeY(mSizeY).setNormal(Vector3.NEGATIVE_UNIT_X).setPosition((.5f * mSizeX + mChamferSize) * (mOrientation * Vector3.NEGATIVE_UNIT_X)).addToTriangleBuffer(ref buffer);
            buffer.rebaseOffset();
            pg.setNumSegX(mNumSegZ).setNumSegY(mNumSegY).setSizeX(mSizeZ).setSizeY(mSizeY).setNormal(Vector3.UNIT_X).setPosition((.5f * mSizeX + mChamferSize) * (mOrientation * Vector3.UNIT_X)).addToTriangleBuffer(ref buffer);

            // Generate the corners
            _addCorner(ref buffer, true, true, true);
            _addCorner(ref buffer, true, true, false);
            _addCorner(ref buffer, true, false, true);
            _addCorner(ref buffer, true, false, false);
            _addCorner(ref buffer, false, true, true);
            _addCorner(ref buffer, false, true, false);
            _addCorner(ref buffer, false, false, true);
            _addCorner(ref buffer, false, false, false);

            // Generate the edges
            _addEdge(ref buffer, -1, -1, 0);
            _addEdge(ref buffer, -1, 1, 0);
            _addEdge(ref buffer, 1, -1, 0);
            _addEdge(ref buffer, 1, 1, 0);
            _addEdge(ref buffer, -1, 0, -1);
            _addEdge(ref buffer, -1, 0, 1);
            _addEdge(ref buffer, 1, 0, -1);
            _addEdge(ref buffer, 1, 0, 1);
            _addEdge(ref buffer, 0, -1, -1);
            _addEdge(ref buffer, 0, -1, 1);
            _addEdge(ref buffer, 0, 1, -1);
            _addEdge(ref buffer, 0, 1, 1);
        }


        /// Internal. Builds an "edge" of the rounded box, ie a quarter cylinder

        //*
        // * xPos,yPos,zPos : 1 => positive
        //					-1 => negative
        //					0 => undefined
        // 
        //
        //ORIGINAL LINE: void _addEdge(TriangleBuffer& buffer, short xPos, short yPos, short zPos) const
        private void _addEdge(ref TriangleBuffer buffer, short xPos, short yPos, short zPos) {
            int offset = 0;

            Vector3 centerPosition = 0.5f * xPos * mSizeX * Vector3.UNIT_X + .5f * yPos * mSizeY * Vector3.UNIT_Y + .5f * zPos * mSizeZ * Vector3.UNIT_Z;
            Vector3 vy0 = (1.0f - Math.Abs(xPos)) * Vector3.UNIT_X + (1.0f - Math.Abs(yPos)) * Vector3.UNIT_Y + (1.0f - Math.Abs(zPos)) * Vector3.UNIT_Z; //extrusion direction

            Vector3 vx0 = Utils.vectorAntiPermute(vy0);
            Vector3 vz0 = Utils.vectorPermute(vy0);
            if (vx0.DotProduct(centerPosition) < 0.0)
                vx0 = -vx0;
            if (vz0.DotProduct(centerPosition) < 0.0)
                vz0 = -vz0;
            if (vx0.CrossProduct(vy0).DotProduct(vz0) < 0.0)
                vy0 = -vy0;

            float height = (1 - Math.Abs(xPos)) * mSizeX + (1 - Math.Abs(yPos)) * mSizeY + (1 - Math.Abs(zPos)) * mSizeZ;
            Vector3 offsetPosition = centerPosition - .5f * height * vy0;
            int numSegHeight = 1;

            if (xPos == 0)
                numSegHeight = mNumSegX;
            else if (yPos == 0)
                numSegHeight = mNumSegY;
            else if (zPos == 0)
                numSegHeight = mNumSegZ;

            float deltaAngle = (Math.HALF_PI / mChamferNumSeg);
            float deltaHeight = height / (float)numSegHeight;


            buffer.rebaseOffset();
            buffer.estimateIndexCount((uint)(6 * numSegHeight * mChamferNumSeg));
            buffer.estimateVertexCount((uint)((numSegHeight + 1) * (mChamferNumSeg + 1)));

            for (ushort i = 0; i <= numSegHeight; i++){
                for (ushort j = 0; j <= mChamferNumSeg; j++) {
                    float x0 = mChamferSize * cosf(j * deltaAngle);
                    float z0 = mChamferSize * sinf(j * deltaAngle);
                    //addPoint(ref buffer, new Vector3(x0 * vx0 + i * deltaHeight * vy0 + z0 * vz0 + offsetPosition), (x0 * vx0 + z0 * vz0).NormalisedCopy, new Vector2(j / (float)mChamferNumSeg, i / (float)numSegHeight));
                

                    //addPoint(buffer, Vector3(x0 * vx0 + i * deltaHeight * vy0 + z0 * vz0 + offsetPosition),
                    // (x0 * vx0 + z0 * vz0).normalisedCopy(),
                    // Vector2(j / (Real)mChamferNumSeg, i / (Real)numSegHeight));
                    
                    addPoint(ref buffer,
                        (x0 * vx0 + i * deltaHeight * vy0 + z0 * vz0 + offsetPosition),
                        (x0 * vx0 + z0 * vz0).NormalisedCopy,
                        new Vector2(j / (float)mChamferNumSeg, i / (float)numSegHeight)
                        );
                    if (i != numSegHeight && j != mChamferNumSeg) {
                        buffer.index(offset + mChamferNumSeg + 2);
                        buffer.index(offset);
                        buffer.index(offset + mChamferNumSeg + 1);
                        buffer.index(offset + mChamferNumSeg + 2);
                        buffer.index(offset + 1);
                        buffer.index(offset);
                    }
                    offset++;
                }
        }
        }

        /// Internal. Builds a "corner" of the rounded box, ie a 1/8th of a sphere
        //
        //ORIGINAL LINE: void _addCorner(TriangleBuffer& buffer, bool isXPositive, bool isYPositive, bool isZPositive) const
        private void _addCorner(ref TriangleBuffer buffer, bool isXPositive, bool isYPositive, bool isZPositive) {
            buffer.rebaseOffset();
            buffer.estimateVertexCount((uint)((mChamferNumSeg + 1) * (mChamferNumSeg + 1)));
            buffer.estimateIndexCount((uint)(mChamferNumSeg * mChamferNumSeg * 6));
            int offset = 0;

            Vector3 offsetPosition = new Vector3((isXPositive ? 1 : -1) * .5f * mSizeX, (isYPositive ? 1 : -1) * .5f * mSizeY, (isZPositive ? 1 : -1) * .5f * mSizeZ);
            float deltaRingAngle = (Math.HALF_PI / mChamferNumSeg);
            float deltaSegAngle = (Math.HALF_PI / mChamferNumSeg);
            float offsetRingAngle = isYPositive ? 0 : Math.HALF_PI;
            float offsetSegAngle = 0f;
            if (isXPositive && isZPositive)
                offsetSegAngle = 0;
            if ((!isXPositive) && isZPositive)
                offsetSegAngle = 1.5f * Math.PI;
            if (isXPositive && (!isZPositive))
                offsetSegAngle = Math.HALF_PI;
            if ((!isXPositive) && (!isZPositive))
                offsetSegAngle = Math.PI;

            // Generate the group of rings for the sphere
            for (ushort ring = 0; ring <= mChamferNumSeg; ring++) {
                float r0 = mChamferSize * sinf(ring * deltaRingAngle + offsetRingAngle);
                float y0 = mChamferSize * cosf(ring * deltaRingAngle + offsetRingAngle);

                // Generate the group of segments for the current ring
                for (ushort seg = 0; seg <= mChamferNumSeg; seg++) {
                    float x0 = r0 * sinf(seg * deltaSegAngle + offsetSegAngle);
                    float z0 = r0 * cosf(seg * deltaSegAngle + offsetSegAngle);

                    // Add one vertex to the strip which makes up the sphere
                    addPoint(buffer, new Vector3(x0 + offsetPosition.x, y0 + offsetPosition.y, z0 + offsetPosition.z), new Vector3(x0, y0, z0).NormalisedCopy, new Vector2((float)seg / (float)mChamferNumSeg, (float)ring / (float)mChamferNumSeg));

                    if ((ring != mChamferNumSeg) && (seg != mChamferNumSeg)) {
                        // each vertex (except the last) has six indices pointing to it
                        buffer.index(offset + mChamferNumSeg + 2);
                        buffer.index(offset);
                        buffer.index(offset + mChamferNumSeg + 1);
                        buffer.index(offset + mChamferNumSeg + 2);
                        buffer.index(offset + 1);
                        buffer.index(offset);
                    }

                    offset++;
                } // end for seg
            } // end for ring
        }

    }


}
