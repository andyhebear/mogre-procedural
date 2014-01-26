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
//#ifndef PROCEDURAL_TRACK_INCLUDED
#define PROCEDURAL_TRACK_INCLUDED
// write with new std ... ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    //*
    // * Represents a curve by interpolating between a list of key/values.
    // * It always refers to a "principal" curve (a path, atm), so the keys to either its point index or lineic position.
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Track
    public class Track
    {
        /// Defines addressing mode for the track
        /// ABSOLUTE_LINEIC : use the distance from the start of the principal curve
        /// RELATIVE_LINEIC : use the relative distance from the start of the principal curve, considering the total length of main curve is 1.
        /// POINT : right on the principal curve's key
        public enum AddressingMode : int
        {
            AM_ABSOLUTE_LINEIC,
            AM_RELATIVE_LINEIC,
            AM_POINT
        }
        /// Adressing mode of the track (see the enum definition for more details)
        protected AddressingMode mAddressingMode;

        /// Tells whether we should add new points to principal curve if a key is defined here but not on principal curve
        protected bool mInsertPoint;

        /// Key frames
        protected std_map<float, float> mKeyFrames = new std_map<float, float>();
        /// Default constructor.
        /// Point insertion default to true, and addressing to relative lineic
        public Track(AddressingMode addressingMode)
            : this(addressingMode, true) {

        }
        public Track()
            : this(AddressingMode.AM_RELATIVE_LINEIC, true) {
        }
       //
        //ORIGINAL LINE: Track(AddressingMode addressingMode=AM_RELATIVE_LINEIC, bool insertPoint=true) : mAddressingMode(addressingMode), mInsertPoint(insertPoint)
        public Track(AddressingMode addressingMode, bool insertPoint) {
            mAddressingMode = addressingMode;
            mInsertPoint = insertPoint;
        }

        /// Gets addressing mode of the curve
        //
        //ORIGINAL LINE: AddressingMode getAddressingMode() const
        public AddressingMode getAddressingMode() {
            return mAddressingMode;
        }

        /// Inserts a new Key/Value couple anywhere on the track (it is auto-sorted anyway)
        public Track addKeyFrame(float pos, float @value) {
            if (mKeyFrames.ContainsKey(pos)) {
                mKeyFrames[pos] = @value;
            }
            else {
                mKeyFrames.Add(pos, value);
            }
            return this;
        }

        /// @copydoc Track::mInsertPoint
        //
        //ORIGINAL LINE: inline bool isInsertPoint() const
        public bool isInsertPoint() {
            return mInsertPoint;
        }

        /// Gets the value on the current point, taking into account the addressing mode
        //
        //ORIGINAL LINE: float getValue(float absPos, float relPos, uint index) const
        public float getValue(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return getValue(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return getValue(relPos);
            return getValue((float)index);
        }

        /// Gets the value on the current point
        //
        //ORIGINAL LINE: float getValue(float pos) const
        public float getValue(float pos) {
            std_pair<float, float> itAfter = _getKeyValueAfter(pos);
            std_pair<float, float> itBefore = _getKeyValueBefore(pos);

            if (itAfter == itBefore)
                return itBefore.second;
            //if (itAfter==mKeyFrames.begin())
            if (itAfter == mKeyFrames.get(0))
                return itAfter.second;

            Real x1 = itBefore.first;
            Real y1 = itBefore.second;
            Real x2 = itAfter.first;
            Real y2 = itAfter.second;
            return (pos - x1) / (x2 - x1) * (y2 - y1) + y1;
        }

        /// Get the key value couple before current point, taking into account addressing mode.
        /// If current point is below minimum key, issues minimum key
        //
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float absPos, float relPos, uint index) const
        public std_pair<float, float> _getKeyValueBefore(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return _getKeyValueBefore(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return _getKeyValueBefore(relPos);
            return _getKeyValueBefore((float)index);
        }

        /// Get the key value couple before current point.
        /// If current point is below minimum key, issues minimum key/value
        //
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float pos) const
        //public std.map<Real, Real>.Enumerator _getKeyValueBefore(float pos)
        public std_pair<float, float> _getKeyValueBefore(float pos) {
            std_pair<float, float> it = mKeyFrames.upper_bound(pos);
            int index = mKeyFrames.find(pos);
            //if (it==mKeyFrames.begin())
            if (index == 0)
                return it;
            else {
                return mKeyFrames.lower_bound(pos);
                //return --it;
            }
            //std::map<Real, Real>::const_iterator it = mKeyFrames.upper_bound(pos);
            //if (it==mKeyFrames.begin())
            //    return it;
            //else
            //    return --it;
        }

        /// Get the key value couple after current point, taking into account addressing mode.
        /// If current point is above maximum key, issues maximum key/value
        //
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index) const
        //public std.map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index)
        public std_pair<float, float> _getKeyValueAfter(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return _getKeyValueAfter(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return _getKeyValueAfter(relPos);
            return _getKeyValueAfter((float)index);
        }

        /// Get the key value couple after current point.
        /// If current point is above maximum key, issues maximum key/value
        //
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float pos) const
        //public std.map<Real, Real>.Enumerator _getKeyValueAfter(float pos)
        public std_pair<float, float> _getKeyValueAfter(float pos) {
            std_pair<float, float> it = mKeyFrames.upper_bound(pos);
            int index = mKeyFrames.find(pos);
            //if (it==mKeyFrames.end())
            if (index == mKeyFrames.Count - 1) {
                return mKeyFrames.lower_bound(pos); //return --it;
            }
            else
                return it;
        }

        /// Gets the first value in the track
        //
        //ORIGINAL LINE: Ogre::float getFirstValue() const
        public float getFirstValue() {
            return mKeyFrames.get(0).second;

            //return mKeyFrames.begin().second;
        }

        /// Gets the last value in the track
        //
        //ORIGINAL LINE: Ogre::float getLastValue() const
        public float getLastValue() {
            if (mKeyFrames.Count < 1) return 0f;
            return mKeyFrames.get((uint)(mKeyFrames.Count - 1)).second;

            //return (--mKeyFrames.end()).second;
        }
    }
    //---------------------------------------------------


}
