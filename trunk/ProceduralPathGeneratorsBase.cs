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
    // * \ingroup pathgrp
    // * Base class for Path generators
    // 
    public class BaseSpline3<T>
    {
        /// The number of segments between 2 control points
        protected uint mNumSeg;
        /// Tells if the spline is closed or not
        protected bool mClosed;
        public BaseSpline3() {
            mNumSeg = 4;
            mClosed = false;
        }

        /// Sets the number of segments between 2 control points
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public BaseSpline3<T> setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BaseSpline3::setNumSeg(unsigned int)", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 segments Procedural::BaseSpline3::setNumSeg(unsigned int)");
            ;
            mNumSeg = numSeg;
            //return (T)this;
            return this;
        }

        /// Closes the spline
        public BaseSpline3<T> close() {
            mClosed = true;
            //return (T)this;
            return this;
        }
    }
}
