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
        protected Side mOutSide = new Side();
        /// Default constructor
        public BaseSpline2() {
            mNumSeg = 4;
            mClosed = false;
            mOutSide = SIDE_RIGHT;
        }

        /// Sets the out side of the shape
        public void setOutSide(Side outSide) {
            mOutSide = outSide;
            //return (T)this;
        }

        /// Gets the out side of the shape
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline Side getOutSide() const
        public Side getOutSide() {
            return mOutSide;
        }

        /// Sets the number of segments between 2 control points
        /// \exception Ogre::InvalidParametersException Minimum of numSeg is 1
        public void setNumSeg(uint numSeg) {
            if (numSeg == 0)
                //OGRE_EXCEPT(Mogre.Exception.ERR_INVALIDPARAMS, "There must be more than 0 segments", "Procedural::BaseSpline2::setNumSeg(unsigned int)");
                throw new Exception("There must be more than 0 segments   Procedural::BaseSpline2::setNumSeg(unsigned int)");
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
