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

    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template<class T>
    //-----------------------------------------------------------------------
    //*
    //\ingroup shapegrp
    //Base class for Shape generators
    //
    public class BaseSpline2<T>
    {
        /// The number of segments between 2 control points
        protected uint mNumSeg;
        /// Whether the shape will be closed or not
        protected bool mClosed;
        /// The "out" side of the shape
        protected Side mOutSide = Side.SIDE_LEFT;
        /// Default constructor
        public BaseSpline2() {
            mNumSeg = 4;
            mClosed = false;
            mOutSide = Side.SIDE_RIGHT;
        }

        /// Sets the out side of the shape
        public BaseSpline2<T> setOutSide(Side outSide) {
            mOutSide = outSide;
            return this;
        }

        /// Gets the out side of the shape
        //
        //ORIGINAL LINE: inline Side getOutSide() const
        public Side getOutSide() {
            return mOutSide;
        }

        /// Sets the number of segments between 2 control points
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public BaseSpline2<T> setNumSeg(uint numSeg) {
            if (numSeg == 0)
               OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 segments", "Procedural::BaseSpline2::setNumSeg(unsigned int)");
            mNumSeg = numSeg;
            return this;
        }

        protected static void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p+"_"+p_2+"_"+p_3);
        }

        /// Closes the spline
        public BaseSpline2<T> close() {
            mClosed = true;
            return this;
        }
    }

}
