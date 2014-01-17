
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

//#ifndef PROCEDURAL_LATHE_INCLUDED
#define PROCEDURAL_LATHE_INCLUDED

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    //*
    // * Builds a mesh by rotating a shape 360 degrees around Y-axis.
    // * The shape is assumed to be defined in the X>=0 half-plane
    // * <table border="0" width="100%"><tr><td>\image html lathe_generic.png "Generic lathe (360 degree)"</td><td>\image html lathe_anglerange.png "Lathe with a specific angle"</td></tr></table>
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Lathe : public MeshGenerator<Lathe>
    public class Lathe : MeshGenerator<Lathe>
    {
        private Shape mShapeToExtrude;
        private MultiShape mMultiShapeToExtrude;
        private uint mNumSeg;
        private Radian mAngleBegin = new Radian();
        private Radian mAngleEnd = new Radian();
        private bool mClosed;
        private bool mCapped;

        //-----------------------------------------------------------------------
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void _latheCapImpl(TriangleBuffer& buffer) const
        private void _latheCapImpl(ref TriangleBuffer buffer) {
            std_vector<int> indexBuffer = new std_vector<int>();
            std_vector<Vector2> pointList = new std_vector<Vector2>();

            buffer.rebaseOffset();

            Triangulator t = new Triangulator();
            Shape shapeCopy = new Shape();
            MultiShape multishapeCopy = new MultiShape();

            if (mShapeToExtrude != null) {
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
                //ORIGINAL LINE: shapeCopy = *mShapeToExtrude;
                shapeCopy = (mShapeToExtrude);
                shapeCopy.close();
                t.setShapeToTriangulate(shapeCopy);
            }
            else {
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
                //ORIGINAL LINE: multishapeCopy = *mMultiShapeToExtrude;
                multishapeCopy = (mMultiShapeToExtrude);
                multishapeCopy.close();
                t.setMultiShapeToTriangulate(mMultiShapeToExtrude);
            }
            t.triangulate(indexBuffer, pointList);
            buffer.estimateIndexCount(2 * (uint)indexBuffer.Count);
            buffer.estimateVertexCount(2 * (uint)pointList.Count);

            //begin cap
            buffer.rebaseOffset();
            Quaternion q = new Quaternion();
            q.FromAngleAxis(mAngleBegin, Vector3.UNIT_Y);
            for (int j = 0; j < pointList.size(); j++) {
                Vector2 vp2 = pointList[j];
                Vector3 vp = new Vector3(vp2.x, vp2.y, 0);
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: Vector3 normal = Vector3::UNIT_Z;
                Vector3 normal = (Vector3.UNIT_Z);

                addPoint(ref buffer, q * vp, q * normal, vp2);
            }

            for (int i = 0; i < indexBuffer.size() / 3; i++) {
                buffer.index(indexBuffer[i * 3]);
                buffer.index(indexBuffer[i * 3 + 1]);
                buffer.index(indexBuffer[i * 3 + 2]);
            }
            //end cap
            buffer.rebaseOffset();
            q.FromAngleAxis(mAngleEnd, Vector3.UNIT_Y);
            for (int j = 0; j < pointList.size(); j++) {
                Vector2 vp2 = pointList[j];
                Vector3 vp = new Vector3(vp2.x, vp2.y, 0);
                Vector3 normal = -Vector3.UNIT_Z;

                addPoint(ref buffer, q * vp, q * normal, vp2);
            }

            for (int i = 0; i < indexBuffer.size() / 3; i++) {
                buffer.index(indexBuffer[i * 3]);
                buffer.index(indexBuffer[i * 3 + 2]);
                buffer.index(indexBuffer[i * 3 + 1]);
            }
        }
        //-----------------------------------------------------------------------
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void _latheBodyImpl(TriangleBuffer& buffer, const Shape* shapeToExtrude) const
        private void _latheBodyImpl(ref TriangleBuffer buffer, Shape shapeToExtrude) {
            if (shapeToExtrude == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Shape must not be null!", "Procedural::Lathe::_latheBodyImpl(Procedural::TriangleBuffer&, const Procedural::Shape*)", __FILE__, __LINE__);
                throw new Exception("Shape must not be null!");
            ;
            int numSegShape = shapeToExtrude.getSegCount();
            if (numSegShape < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Shape must contain at least two points", "Procedural::Lathe::_latheBodyImpl(Procedural::TriangleBuffer&, const Procedural::Shape*)", __FILE__, __LINE__);
                throw new Exception("Shape must contain at least two points");
            ;
            int offset = 0;

            //int numSeg = mClosed?mNumSeg+1:mNumSeg;
            int numSeg = (int)mNumSeg + 1;
            buffer.rebaseOffset();
            buffer.estimateIndexCount((uint)(numSeg * numSegShape * 6));
            buffer.estimateVertexCount((uint)((numSegShape + 1) * (numSeg + 1)));

            Radian angleEnd = new Radian(mAngleEnd);
            if (mAngleBegin > mAngleEnd)
                angleEnd += (Radian)Math.TWO_PI;

            for (int i = 0; i < numSeg; i++) {
                Radian angle = new Radian();
                if (mClosed)
                    angle = i / (float)mNumSeg * Math.TWO_PI;
                else
                    angle = mAngleBegin + i / (float)mNumSeg * (angleEnd - mAngleBegin);
                Quaternion q = new Quaternion();
                q.FromAngleAxis(angle, Vector3.UNIT_Y);

                for (int j = 0; j <= numSegShape; j++) {
                    Vector2 v0 = shapeToExtrude.getPoint(j);
                    Vector3 vp = new Vector3(v0.x, v0.y, 0);
                    Vector2 vp2direction = shapeToExtrude.getAvgDirection((uint)j);
                    Vector2 vp2normal = vp2direction.Perpendicular;
                    Vector3 normal = new Vector3(vp2normal.x, vp2normal.y, 0);
                    normal.Normalise();
                    if (shapeToExtrude.getOutSide() == Side.SIDE_RIGHT)
                        normal = -normal;

                    addPoint(ref buffer, q * vp, q * normal, new Vector2(i / (float)mNumSeg, j / (float)numSegShape));

                    if (j < numSegShape && i < numSeg - 1) {
                        if (shapeToExtrude.getOutSide() == Side.SIDE_RIGHT) {
                            buffer.triangle(offset + numSegShape + 2, offset, offset + numSegShape + 1);
                            buffer.triangle(offset + numSegShape + 2, offset + 1, offset);
                        }
                        else {
                            buffer.triangle(offset + numSegShape + 2, offset + numSegShape + 1, offset);
                            buffer.triangle(offset + numSegShape + 2, offset, offset + 1);
                        }
                    }
                    offset++;
                }
            }
        }

        /// Contructor with arguments
        public Lathe(Shape shapeToExtrude)
            : this(shapeToExtrude, 16) {
        }
        public Lathe()
            : this(null, 16) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Lathe(Shape* shapeToExtrude = 0, uint numSeg = 16) : mShapeToExtrude(shapeToExtrude), mMultiShapeToExtrude(0), mNumSeg(numSeg), mAngleBegin(0), mAngleEnd((Ogre::Radian)Ogre::Math::TWO_PI), mClosed(true), mCapped(true)
        public Lathe(Shape shapeToExtrude, uint numSeg) {
            mShapeToExtrude = shapeToExtrude;
            mMultiShapeToExtrude = null;
            mNumSeg = numSeg;
            mAngleBegin = 0f;
            mAngleEnd = (Radian)Math.TWO_PI;
            mClosed = true;
            mCapped = true;
        }

        //    *
        //	Sets the number of segments when rotating around the axis (default=16)
        //	\exception Ogre::InvalidParametersException Minimum of numSeg is 1
        //	
        public Lathe setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::Lathe::setNumSeg(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numSeg must be larger than 0!");
            ;
            mNumSeg = numSeg;
            return this;
        }

        /// Sets the angle to begin lathe with (default=0)
        /// Automatically makes the lathe not closed
        public Lathe setAngleBegin(Radian angleBegin) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mAngleBegin = angleBegin;
            mAngleBegin = (angleBegin);
            mClosed = false;
            return this;
        }

        /// Sets the angle to end lathe with (default=2PI)
        /// Automatically makes the lathe not closed
        public Lathe setAngleEnd(Radian angleEnd) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mAngleEnd = angleEnd;
            mAngleEnd = (angleEnd);
            mClosed = false;
            return this;
        }

        /// Sets whether the lathe is closed or not
        public Lathe setClosed(bool closed) {
            mClosed = closed;
            return this;
        }

        /// Sets whether the lathe is capped or not (default=true)
        /// Only makes sense if the lathe is not closed.
        public Lathe setCapped(bool capped) {
            mCapped = capped;
            return this;
        }

        //    * Sets the shape to extrude
        //	  * If a multishape is already defined, auto-disables it
        //	  * The shape is assumed to be defined in the X>=0 half-plane
        //	  
        public Lathe setShapeToExtrude(Shape shapeToExtrude) {
            mShapeToExtrude = shapeToExtrude;
            mMultiShapeToExtrude = null;
            return this;
        }

        //    * Sets the multiShape to extrude
        //	  * If a shape is already defined, auto-disables it
        //	  * The shapes in this multi-shape are assumed to be defined in the X>=0 half-plane
        //	  
        public Lathe setMultiShapeToExtrude(MultiShape multiShapeToExtrude) {
            mMultiShapeToExtrude = multiShapeToExtrude;
            mShapeToExtrude = null;
            return this;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 * @exception Ogre::InvalidStateException Either shape or multishape must be defined!
        //	 * @exception Ogre::InvalidStateException Required parameter is zero!
        //	 
        //-----------------------------------------------------------------------
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer) {
            if (mShapeToExtrude == null && mMultiShapeToExtrude == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Either shape or multishape must be defined!", "Procedural::Lathe::addToTriangleBuffer(Procedural::TriangleBuffer)", __FILE__, __LINE__);
                throw new Exception("Either shape or multishape must be defined!");
            ;

            // Triangulate the begin and end caps
            if (!mClosed && mCapped)
                _latheCapImpl(ref buffer);

            // Extrudes the body
            if (mShapeToExtrude != null)
                _latheBodyImpl(ref buffer, mShapeToExtrude);
            else
                for (uint i = 0; i < mMultiShapeToExtrude.getShapeCount(); i++)
                    _latheBodyImpl(ref buffer, mMultiShapeToExtrude.getShape(i));


        }
    }
}

