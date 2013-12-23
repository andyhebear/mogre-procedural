using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math = Mogre.Math;

namespace Mogre_Procedural
{

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
        public void setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 segments", "Procedural::BaseSpline3::setNumSeg(unsigned int)", __FILE__, __LINE__);
                throw new Exception("There must be more than 0 segments Procedural::BaseSpline3::setNumSeg(unsigned int)");
                ;
            mNumSeg = numSeg;
            //return (T)this;
        }

        /// Closes the spline
        public void close() {
            mClosed = true;
            //return (T)this;
        }
    }
}
