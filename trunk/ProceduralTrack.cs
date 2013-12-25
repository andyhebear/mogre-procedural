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
        protected Dictionary<float, float> mKeyFrames = new Dictionary<float, float>();
        /// Default constructor.
        /// Point insertion default to true, and addressing to relative lineic
        public Track(AddressingMode addressingMode)
            : this(addressingMode, true) {

        }
        public Track()
            : this(AddressingMode.AM_RELATIVE_LINEIC, true) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: Track(AddressingMode addressingMode=AM_RELATIVE_LINEIC, bool insertPoint=true) : mAddressingMode(addressingMode), mInsertPoint(insertPoint)
        public Track(AddressingMode addressingMode, bool insertPoint) {
            mAddressingMode = addressingMode;
            mInsertPoint = insertPoint;
        }

        /// Gets addressing mode of the curve
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
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
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline bool isInsertPoint() const
        public bool isInsertPoint() {
            return mInsertPoint;
        }

        /// Gets the value on the current point, taking into account the addressing mode
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float getValue(float absPos, float relPos, uint index) const
        public float getValue(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return getValue(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return getValue(relPos);
            return getValue((float)index);
        }

        /// Gets the value on the current point
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: float getValue(float pos) const
        public float getValue(float pos) {
            KeyValuePair<float, float> itAfter = _getKeyValueAfter(pos);
            KeyValuePair<float, float> itBefore = _getKeyValueBefore(pos);

            //if (itAfter ==itBefore)
            //    return itBefore.second;
            //if (itAfter ==mKeyFrames.begin())
            //    return itAfter.second;
            if (itAfter.Key == itBefore.Key) {// && itAfter.Value == itBefore.Value) { 
                return itBefore.Value;
            }
            Dictionary<float, float>.Enumerator it = mKeyFrames.GetEnumerator();
            if (itAfter.Key == it.Current.Key) {
                return itAfter.Value;
            }
            float x1 = itBefore.Key;
            float y1 = itBefore.Value;
            float x2 = itAfter.Key;
            float y2 = itAfter.Value;
            return (pos - x1) / (x2 - x1) * (y2 - y1) + y1;
        }

        /// Get the key value couple before current point, taking into account addressing mode.
        /// If current point is below minimum key, issues minimum key
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float absPos, float relPos, uint index) const
        public std.map<Real, Real>.Enumerator _getKeyValueBefore(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return _getKeyValueBefore(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return _getKeyValueBefore(relPos);
            return _getKeyValueBefore((float)index);
        }

        /// Get the key value couple before current point.
        /// If current point is below minimum key, issues minimum key/value
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueBefore(float pos) const
        //public std.map<Real, Real>.Enumerator _getKeyValueBefore(float pos)
        public KeyValuePair<float, float> _getKeyValueBefore(float pos) {
            if (!mKeyFrames.ContainsKey(pos)) {
                throw new Exception("the mKeyFrames not contains pos");
            }
            Dictionary<float, float>.Enumerator it = mKeyFrames.GetEnumerator();
            int count = mKeyFrames.Count;
            int index = 0;
            KeyValuePair<float, float> before = new KeyValuePair<float, float>();
            while (index < count) {
                before = it.Current;
                if (it.MoveNext()) {
                    KeyValuePair<float, float> cur = it.Current;
                    if (cur.Key == pos) {
                        return before;
                    }
                }
                index++;
            }
            return before;

            KeyValuePair<float, float> kv = new KeyValuePair<float, float>();
            index = 0;
            count = mKeyFrames.Count;
            foreach (var v in mKeyFrames) {
                if (pos == 0) {
                    kv = new KeyValuePair<float, float>(v.Key, v.Value);
                    break;
                }
                else if (pos >= count) {
                    if (index == count - 1) {
                        kv = new KeyValuePair<float, float>(v.Key, v.Value);
                    }
                    continue;
                }
                else if (pos == index + 1) {
                    kv = new KeyValuePair<float, float>(v.Key, v.Value);
                    break;
                }
                index++;
            }
            return kv;
            //std.map<Real, Real>.Enumerator it = mKeyFrames.upper_bound(pos);
            //if (it ==mKeyFrames.begin())
            //    return it;
            //else
            //    return --it;
        }

        /// Get the key value couple after current point, taking into account addressing mode.
        /// If current point is above maximum key, issues maximum key/value
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index) const
        //public std.map<Real, Real>.Enumerator _getKeyValueAfter(float absPos, float relPos, uint index)
        public KeyValuePair<float, float> _getKeyValueAfter(float absPos, float relPos, uint index) {
            if (mAddressingMode == AddressingMode.AM_ABSOLUTE_LINEIC)
                return _getKeyValueAfter(absPos);
            if (mAddressingMode == AddressingMode.AM_RELATIVE_LINEIC)
                return _getKeyValueAfter(relPos);
            return _getKeyValueAfter((float)index);
        }

        /// Get the key value couple after current point.
        /// If current point is above maximum key, issues maximum key/value
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: std::map<Real, Real>.Enumerator _getKeyValueAfter(float pos) const
        //public std.map<Real, Real>.Enumerator _getKeyValueAfter(float pos)
        public KeyValuePair<float, float> _getKeyValueAfter(float pos) {
            if (!mKeyFrames.ContainsKey(pos)) {
                throw new Exception("the mKeyFrames not contains pos");
            }
            KeyValuePair<float, float> after = new KeyValuePair<float, float>();
            bool findcurrent = false;
            foreach (var v in mKeyFrames) {
                if (findcurrent) {
                    after = new KeyValuePair<float, float>(v.Key, v.Value);
                    break;
                }
                else {
                    after = new KeyValuePair<float, float>(v.Key, v.Value);
                }
                if (v.Key == pos) {
                    findcurrent = true;
                }
            }
            return after;

            KeyValuePair<float, float> kv = new KeyValuePair<float, float>();
            int index = 0;
            int count = mKeyFrames.Count;
            foreach (var v in mKeyFrames) {
                if (pos < 0) {
                    //第一个
                    kv = new KeyValuePair<float, float>(v.Key, v.Value);
                    break;
                }
                else if (pos == 0) {
                    //第一个
                    if (index == 1) {
                        kv = new KeyValuePair<float, float>(v.Key, v.Value);
                        break;
                    }
                    continue;
                }
                else if (pos >= count - 1) {
                    //最后一个
                    if (index == count - 1) {
                        kv = new KeyValuePair<float, float>(v.Key, v.Value);
                        break;
                    }
                    continue;
                }
                else {
                    if (pos == index - 1) {
                        kv = new KeyValuePair<float, float>(v.Key, v.Value);
                    }
                }
                index++;
            }
            //std.map<Real, Real>.Enumerator it = mKeyFrames.upper_bound(pos);
            //if (it ==mKeyFrames.end())
            //    return --it;
            //else
            //    return it;
            return kv;
        }

        /// Gets the first value in the track
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::float getFirstValue() const
        public float getFirstValue() {
            foreach (var v in mKeyFrames.Values) {
                return v;
            }
            return 0f;
            //return mKeyFrames.begin().second;
        }

        /// Gets the last value in the track
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::float getLastValue() const
        public float getLastValue() {
            int count = mKeyFrames.Count;
            int i = 0;
            foreach (var v in mKeyFrames.Values) {
                i++;
                if (i == count) {
                    return v;
                }
            }
            return 0f;
            //return (--mKeyFrames.end()).second;
        }
    }
    //---------------------------------------------------


}
