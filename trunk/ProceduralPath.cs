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
    //\defgroup pathgrp Path
    //Elements for path generation.
    //@{
    //@}
    //

    //*
    // * \ingroup pathgrp
    // * Succession of points in 3D space.
    // * Can be closed or not.
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Path
    public class Path
    {
        private List<Vector3> mPoints = new List<Vector3>();
        private bool mClosed;
        /// Default constructor
        public Path() {
            mClosed = false;
        }

        //* Adds a point to the path, as a Vector3 
        public Path addPoint(Vector3 pt) {
            mPoints.Add(pt);
            return this;
        }

        //* Adds a point to the path, using its 3 coordinates 
        public Path addPoint(float x, float y, float z) {
            mPoints.Add(new Vector3(x, y, z));
            return this;
        }

        /// Inserts a point to the path
        /// @param index the index before the inserted point
        /// @param x new point's x coordinate
        /// @param y new point's y coordinate
        /// @param z new point's z coordinate
        public Path insertPoint(int index, float x, float y, float z) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            mPoints.Insert(index, new Vector3(x, y, z));
            return this;
        }

        /// Inserts a point to the path
        /// @param index the index before the inserted point
        /// @param pt new point's position
        public Path insertPoint(int index, Vector3 pt) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            mPoints.Insert(index, pt);
            return this;
        }

        /// Appends another path at the end of this one
        public Path appendPath(Path other) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            //mPoints.insert(mPoints.end(), STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
            //mPoints.insert(mPoints.end(), other.mPoints.begin(), other.mPoints.end());
            mPoints.AddRange(other.mPoints.ToArray());
            return this;
        }

        ///<summary>
        /// Appends another path at the end of this one, relative to the last point of this path
        ///</summary>
        public Path appendPathRel(Path other) {
            if (mPoints.Count == 0)
                appendPath(other);
            else {
                Vector3 refVector = mPoints[mPoints.Count - 1]; //*(mPoints.end()-1);
                Vector3[] pointList = (other.mPoints.ToArray());
                //for (List<Vector3>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
                //	it.Current += refVector;
                for (int i = 0; i < pointList.Length; i++) {
                    pointList[i] += refVector;
                }
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
                mPoints.AddRange(pointList);
            }
            return this;
        }

        //* Clears the content of the Path 
        public Path reset() {
            mPoints.Clear();
            return this;
        }

        //    *
        //	Define the path as being closed. Almost the same as adding a last point on the first point position
        //	\exception Ogre::InvalidStateException Cannot close an empty path
        /// <summary>
        /// exception Ogre::InvalidStateException Cannot close an empty path
        /// </summary>
        /// <returns></returns>
        public Path close() {
            if (mPoints.Count == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Cannot close an empty path", "Procedural::Path::close()", __FILE__, __LINE__);
                throw new Exception("Cannot close an empty path");
            ;
            mClosed = true;
            return this;
        }

        //* Tells if the path is closed or not 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool isClosed() const
        public bool isClosed() {
            return mClosed;
        }

        //* Gets the list of points as a vector of Vector3 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const List<Ogre::Vector3>& getPoints() const
        public List<Vector3> _getPoints() {
            //return mPoints; 
            return getPointsReference();
        }
        public Vector3[] getPoints() {
            return mPoints.ToArray();
        }
        /// Gets raw vector data of this path as a non-const reference
        public List<Vector3> getPointsReference() {
            return mPoints;
        }

        //    * Safely gets a given point.
        //	 * Takes into account whether the path is closed or not.
        //	 * @param i the index of the point.
        //	 *          if it is <0 or >maxPoint, cycle through the list of points
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const Ogre::Vector3& getPoint(int i) const
        public Vector3 getPoint(int i) {
            if (mClosed)
                return mPoints[Utils.modulo(i, mPoints.Count)];
            return mPoints[Utils.cap(i, 0, mPoints.Count - 1)];
        }

        //    * Gets the number of segments in the path
        //	 * Takes into accound whether path is closed or not
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: int getSegCount() const
        public int getSegCount() {
            return (mPoints.Count - 1) + (mClosed ? 1 : 0);
        }

        //    *
        //	 * Returns local direction after the current point
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::Vector3 getDirectionAfter(uint i) const
        public Vector3 getDirectionAfter(int i) {
            // If the path isn't closed, we get a different calculation at the end, because
            // the tangent shall not be null
            if (!mClosed && i == mPoints.Count - 1 && i > 0)
                return (mPoints[i] - mPoints[i - 1]).NormalisedCopy;
            else
                return (getPoint(i + 1) - getPoint(i)).NormalisedCopy;
        }

        //    *
        //	 * Returns local direction after the current point
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::Vector3 getDirectionBefore(uint i) const
        public Vector3 getDirectionBefore(int i) {
            // If the path isn't closed, we get a different calculation at the end, because
            // the tangent shall not be null
            if (!mClosed && i == 1)
                return (mPoints[1] - mPoints[0]).NormalisedCopy;
            else
                return (getPoint(i) - getPoint(i - 1)).NormalisedCopy;
        }

        //    *
        //	 * Returns the local direction at the current point.
        //	 * @param i index of the point
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::Vector3 getAvgDirection(uint i) const
        public Vector3 getAvgDirection(int i) {
            return (getDirectionAfter(i) + getDirectionBefore(i)).NormalisedCopy;
        }

        /// Returns the total lineic length of that shape
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::float getTotalLength() const
        public float getTotalLength() {
            float length = 0;
            for (int i = 0; i < mPoints.Count - 1; ++i) {
                length += (mPoints[i + 1] - mPoints[i]).Length;
            }
            if (mClosed) {
                length += (mPoints[mPoints.Count - 1] - mPoints[0]).Length;
            }
            return length;
        }


        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::float getLengthAtPoint(int index) const
        public float getLengthAtPoint(int index) {
            float length = 0;
            for (int i = 0; i < index; ++i)
                length += (mPoints[i + 1] - mPoints[i]).Length;
            //if (mClosed)
            //length += (mPoints.back()-*mPoints.begin()).length();
            return length;

        }

        /// Gets a position on the shape with index of the point and a percentage of position on the segment
        /// @param i index of the segment
        /// @param coord a number between 0 and 1 meaning the percentage of position on the segment
        /// @exception Ogre::InvalidParametersException i is out of bounds
        /// @exception Ogre::InvalidParametersException coord must be comprised between 0 and 1
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline Ogre::Vector3 getPosition(uint i, Ogre::float coord) const
        public Vector3 getPosition(int i, float coord) {
            if (i >= mPoints.Count)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Out of Bounds", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("out of bounds Out of Bounds");
            ;
            if (coord < 0.0f || coord > 1.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Coord must be comprised between 0 and 1", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("coord must in 0 and 1.0");
            ;
            Vector3 A = getPoint(i);
            Vector3 B = getPoint(i + 1);
            return A + coord * (B - A);
        }

        /// Gets a position on the shape from lineic coordinate
        /// @param coord lineic coordinate
        /// @exception Ogre::InvalidStateException The path must at least contain 2 points
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::Vector3 getPosition(Ogre::float coord) const
        public Vector3 getPosition(float coord) {
            if (mPoints.Count < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The path must at least contain 2 points", "Procedural::Path::getPosition(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("The path must at least contain 2 points");
            ;
            int i = 0;
            while (true) {
                float nextLen = (getPoint(i + 1) - getPoint(i)).Length;
                if (coord > nextLen)
                    coord -= nextLen;
                else
                    return getPosition(i, coord / nextLen);
                if (!mClosed && i >= mPoints.Count - 2)
                    return mPoints[mPoints.Count - 1];
                i++;
            }
        }

        //    *
        //	 * Outputs a mesh representing the path.
        //	 * Mostly for debugging purposes
        //	 
        public MeshPtr realizeMesh() {
            return realizeMesh("");
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::MeshPtr realizeMesh(const string& name = "") const
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        public MeshPtr realizeMesh(string name) {
            Mogre.SceneManager smgr = Root.Singleton.GetSceneManagerIterator().Current;
            Mogre.ManualObject manual = smgr.CreateManualObject(name);
            manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_STRIP);

            foreach (var itPos in mPoints) {
                manual.Position(itPos);
            }
            if (mClosed) {
                manual.Position(mPoints[0]);
            }
            manual.End();

            Mogre.MeshPtr mesh = MeshManager.Singleton.CreateManual(name, "General");
            if (name == "")
                mesh = manual.ConvertToMesh(Utils.getName("mesh_procedural_"));
            else
                mesh = manual.ConvertToMesh(name);

            return mesh;
        }

        /// Creates a path with the keys of this path and extra keys coming from a track
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Path mergeKeysWithTrack(const Track& track) const
        public Path mergeKeysWithTrack(Track track) {
            if (!track.isInsertPoint() || track.getAddressingMode() == Track.AddressingMode.AM_POINT)
                return this;
            float totalLength = getTotalLength();

            float lineicPos = 0;
            float pathLineicPos = 0;
            Path outputPath = new Path();
            outputPath.addPoint(getPoint(0));
            for (int i = 1; i < mPoints.Count; ) {
                float nextLineicPos = pathLineicPos + (mPoints[i] - mPoints[i - 1]).Length;

                KeyValuePair<float, float> it = track._getKeyValueAfter(lineicPos, lineicPos / totalLength, (uint)(i - 1));

                float nextTrackPos = it.Key;
                if (track.getAddressingMode() == Track.AddressingMode.AM_RELATIVE_LINEIC)
                    nextTrackPos *= totalLength;

                // Adds the closest point to the curve, being either from the path or the track
                if (nextLineicPos <= nextTrackPos || lineicPos >= nextTrackPos) {
                    outputPath.addPoint(mPoints[i]);
                    i++;
                    lineicPos = nextLineicPos;
                    pathLineicPos = nextLineicPos;
                }
                else {
                    outputPath.addPoint(getPosition(i - 1, (nextTrackPos - pathLineicPos) / (nextLineicPos - pathLineicPos)));
                    lineicPos = nextTrackPos;
                }
            }
            return outputPath;
        }

        //    *
        //	* Applies the given translation to all the points already defined.
        //	* Has strictly no effect on the points defined after that
        //	* @param translation the translation vector
        //	
        public Path translate(Vector3 translation) {
            //for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            //	it.Current+=translation;
            for (int i = 0; i < mPoints.Count - 1; i++) {
                mPoints[i] += translation;
            }
            return this;
        }

        //    *
        //	 * Applies the given translation to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param translationX X component of the translation vector
        //	 * @param translationY Y component of the translation vector
        //	 * @param translationZ Z component of the translation vector
        //	 
        public Path translate(float translationX, float translationY, float translationZ) {
            return translate(new Vector3(translationX, translationY, translationZ));
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param amount amount of scale
        //	 
        public Path scale(float amount) {
            return scale(amount, amount, amount);
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param scaleX amount of scale in the X direction
        //	 * @param scaleY amount of scale in the Y direction
        //	 * @param scaleZ amount of scale in the Z direction
        //	 
        public Path scale(float scaleX, float scaleY, float scaleZ) {
            //for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            for (int i = 0; i < mPoints.Count; i++) {
                float it_x = mPoints[i].x * scaleX;
                float it_y = mPoints[i].y * scaleY;
                float it_z = mPoints[i].z * scaleZ;
                mPoints[i] = new Vector3(it_x, it_y, it_z);
            }
            return this;
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param amount of scale
        //	 
        public Path scale(Vector3 amount) {
            return scale(amount.x, amount.y, amount.z);
        }

        //    *
        //	 * Reflect all points in this path against a zero-origined plane with a given normal
        //	 * @param normal the normal
        //	 
        public Path reflect(Vector3 normal) {//反映；反射，照出；反省
            //for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            for (int i = 0; i < mPoints.Count; i++) {
                mPoints[i] = mPoints[i].Reflect(normal);
                //it.Current = it.reflect(normal);
            }
            return this;
        }

        /// Extracts a part of the shape as a new path
        /// @param first first index to be in the new path
        /// @param last last index to be in the new path
        public Path extractSubPath(int first, int last) {//提取 抽出 抽取 
            Path p = new Path();
            for (int i = first; i < last; i++) {
                p.addPoint(mPoints[i]);
            }
            if (mClosed) {
                p.close();
            }
            return p;
        }

        /// Reverses direction of the path
        public Path reverse() {// 颠倒；倒转
            //std.reverse(mPoints.GetEnumerator(), mPoints.end());
            mPoints.Reverse();
            return this;
        }

        //void Path::buildFromSegmentSoup(const std::vector<Segment3D>& segList, std::vector<Path>& out)
        //{
        //    typedef std::multimap<Vector3, Vector3, Vector3Comparator> Vec3MultiMap;
        //    Vec3MultiMap segs;
        //    for (std::vector<Segment3D>::const_iterator it = segList.begin(); it != segList.end(); ++it)
        //    {
        //        segs.insert(std::pair<Vector3, Vector3 > (it->mA, it->mB));
        //        segs.insert(std::pair<Vector3, Vector3 > (it->mB, it->mA));
        //    }
        //    while (!segs.empty())
        //    {
        //        Ogre::Vector3 headFirst = segs.begin()->first;
        //        Ogre::Vector3 headSecond = segs.begin()->second;
        //        Path p;
        //        p.addPoint(headFirst).addPoint(headSecond);
        //        Vec3MultiMap::iterator firstSeg = segs.begin();
        //        std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants2 = segs.equal_range(headSecond);
        //        for (Vec3MultiMap::iterator it = correspondants2.first; it != correspondants2.second;)
        //        {
        //            Vec3MultiMap::iterator removeIt = it++;
        //            if ((removeIt->second - firstSeg->first).squaredLength() < 1e-8)
        //                segs.erase(removeIt);
        //        }
        //        segs.erase(firstSeg);
        //        bool foundSomething = true;
        //        while (!segs.empty() && foundSomething)
        //        {
        //            foundSomething = false;
        //            Vec3MultiMap::iterator next = segs.find(headSecond);
        //            if (next != segs.end())
        //            {
        //                foundSomething = true;
        //                headSecond = next->second;
        //                p.addPoint(headSecond);
        //                std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headSecond);
        //                for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
        //                {
        //                    Vec3MultiMap::iterator removeIt = it++;
        //                    if ((removeIt->second - next->first).squaredLength() < 1e-8)
        //                        segs.erase(removeIt);
        //                }
        //                segs.erase(next);
        //            }
        //            Vec3MultiMap::iterator previous = segs.find(headFirst);
        //            if (previous != segs.end())
        //            {
        //                foundSomething = true;
        //                p.insertPoint(0, previous->second);
        //                headFirst = previous->second;
        //                std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headFirst);
        //                for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
        //                {
        //                    Vec3MultiMap::iterator removeIt = it++;
        //                    if ((removeIt->second - previous->first).squaredLength() < 1e-8)
        //                        segs.erase(removeIt);
        //                }
        //                segs.erase(previous);
        //            }
        //        }
        //        if (p.getPoint(0).squaredDistance(p.getPoint(p.getSegCount() + 1)) < 1e-6)
        //        {
        //            p.getPointsReference().pop_back();
        //            p.close();
        //        }
        //        out.push_back(p);
        //    }
        //}


        public void buildFromSegmentSoup(List<Segment3D> segList, ref List<Path> @out) {
            //typedef std::multimap<Vector3, Vector3, Vector3Comparator> Vec3MultiMap;
            //Vec3MultiMap segs;     
            List<KeyValuePair<Vector3, Vector3>> segs = new List<KeyValuePair<Vector3, Vector3>>();

            //for (std::vector<Segment3D>::const_iterator it = segList.begin(); it != segList.end(); ++it)
            foreach (var it in segList) {
                //segs.insert(std::pair<Vector3, Vector3 > (it->mA, it->mB));
                //segs.insert(std::pair<Vector3, Vector3 > (it->mB, it->mA));
                segs.Add(new KeyValuePair<Vector3, Vector3>(it.mA, it.mB));//如果值一样？
                segs.Add(new KeyValuePair<Vector3, Vector3>(it.mB, it.mA));
            }
            while (segs.Count != 0) {
                //Ogre::Vector3 headFirst = segs.begin()->first;
                //Ogre::Vector3 headSecond = segs.begin()->second;
                Vector3 headFirst = segs[0].Key;//
                Vector3 headSecond = segs[0].Value;
                Path p = new Path();
                p.addPoint(headFirst).addPoint(headSecond);
                //Vec3MultiMap::iterator firstSeg = segs.begin();
                KeyValuePair<Vector3, Vector3> firstSeg = segs[0];//new KeyValuePair<Vector3,Vector3>(headFirst,headSecond);
                //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants2 = segs.equal_range(headSecond);
                KeyValuePair<List<KeyValuePair<Vector3, Vector3>>, List<KeyValuePair<Vector3, Vector3>>> correspondants2 =
                    new KeyValuePair<List<KeyValuePair<Vector3, Vector3>>, List<KeyValuePair<Vector3, Vector3>>>();
                correspondants2 = list_vector3_equal_range(segs, headSecond);
                //for (Vec3MultiMap::iterator it = correspondants2.first; it != correspondants2.second;)
                foreach (var it in correspondants2.Key) {
                    //Vec3MultiMap::iterator removeIt = it++;
                    KeyValuePair<Vector3, Vector3> removeIt = it;
                    //if ((removeIt->second - firstSeg->first).squaredLength() < 1e-8)
                    //	segs.erase(removeIt);
                    if ((removeIt.Value - firstSeg.Key).SquaredLength < 1e-8) {
                        segs.Remove(removeIt);
                    }
                }
                //segs.erase(firstSeg);
                segs.Remove(firstSeg);
                bool foundSomething = true;
                while (segs.Count != 0 && foundSomething) {
                    foundSomething = false;
                    //Vec3MultiMap::iterator next = segs.find(headSecond);
                    KeyValuePair<Vector3, Vector3> next = segs.Find(Z => {
                        if (Z.Value == headSecond) {
                            return true;
                        }
                        return false;
                    });
                    //if (next != segs.end())
                    if (next.Key != segs[segs.Count - 1].Key &&
                        next.Value != segs[segs.Count - 1].Value) {
                        foundSomething = true;
                        headSecond = next.Value;
                        p.addPoint(headSecond);
                        //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headSecond);
                        KeyValuePair<List<KeyValuePair<Vector3, Vector3>>, List<KeyValuePair<Vector3, Vector3>>> correspondants =
                            list_vector3_equal_range(segs, headSecond);
                        //for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
                        foreach (var it in correspondants.Key) {
                            //Vec3MultiMap::iterator removeIt = it++;
                            KeyValuePair<Vector3, Vector3> removeIt = it;
                            if ((removeIt.Value - next.Key).SquaredLength < 1e-8) {
                                segs.Remove(removeIt);
                            }
                        }
                        segs.Remove(next);
                    }
                    //Vec3MultiMap::iterator previous = segs.find(headFirst);
                    KeyValuePair<Vector3, Vector3> previous = segs.Find(Z => {
                        return Z.Key == headFirst;
                    });
                    //if (previous != segs.end())
                    if (previous.Key != segs[segs.Count - 1].Key &&
                        previous.Value != segs[segs.Count - 1].Value) {
                        foundSomething = true;
                        p.insertPoint(0, previous.Value);
                        headFirst = previous.Value;
                        //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headFirst);
                        KeyValuePair<List<KeyValuePair<Vector3, Vector3>>, List<KeyValuePair<Vector3, Vector3>>> correspondants =
                        list_vector3_equal_range(segs, headSecond);
                        //for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
                        foreach (var it in correspondants.Key) {
                            //Vec3MultiMap::iterator removeIt = it++;
                            KeyValuePair<Vector3, Vector3> removeIt = it;
                            if ((removeIt.Value - previous.Key).SquaredLength < 1e-8)
                                segs.Remove(removeIt);
                        }
                        segs.Remove(previous);
                    }
                }
                //if (p.getPoint(0).squaredDistance(p.getPoint(p.getSegCount() + 1)) < 1e-6) {
                if ((p.getPoint(0) - p.getPoint(p.getSegCount() + 1)).SquaredLength < 1e-6) {
                    //    p.getPointsReference().pop_back();//移除最后一个
                    p.getPointsReference().RemoveAt(p.getPointsReference().Count - 1);
                    p.close();
                }
                @out.Add(p);
            }
        }

        //
        private KeyValuePair<List<KeyValuePair<Vector3, Vector3>>, List<KeyValuePair<Vector3, Vector3>>> list_vector3_equal_range(List<KeyValuePair<Vector3, Vector3>> segs, Vector3 headSecond) {
            //equal_range是C++ STL中的一种二分查找的算法，试图在已排序的[first,last)中寻找value，
            //函数equal_range()返回first和last之间等于val的元素区间. 此函数假定first和last区间内的元素可以使用<操作符或者指定的comp执行比较操作.

            //equal_range()可以被认为是lower_bound和upper_bound的结合, pair中的第一个迭代器由lower_bound返回, 第二个则由upper_bound返回.
            //            语法:
            //  pair equal_range( const key_type &key );
            //equal_range()函数查找multimap中键值等于key的所有元素，返回指示范围的两个迭代器,正序与逆序。
            //例如, 下面的代码使用equal_range()探测一个有序的vector中的可以插入数字8的位置:
            throw new NotImplementedException();
        }
        /// Converts the path to a shape, with Y=0

        //-----------------------------------------------------------------------
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Shape convertToShape() const
        public Shape convertToShape() {
            Shape s = new Shape();
            //for (List<Vector3>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mPoints) {
                s.addPoint(it.x, it.y);
            }
            if (mClosed)
                s.close();

            return s;
        }

    }

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport MultiPath
    public class MultiPath
    {
        /// <summary>
        /// 路径坐标
        /// </summary>
        public class PathCoordinate
        {
            /// <summary>
            /// 路径序号
            /// </summary>
            public uint pathIndex;
            /// <summary>
            /// 顶点序号
            /// </summary>
            public uint pointIndex;
            public PathCoordinate(uint _pathIndex, uint _pointIndex) {
                pathIndex = _pathIndex;
                pointIndex = _pointIndex;
            }
            //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
            //ORIGINAL LINE: bool operator < (const PathCoordinate& STLAllocator<U, AllocPolicy>) const
            public static bool operator <(PathCoordinate th, PathCoordinate other) {
                if (th.pathIndex != other.pathIndex)
                    return th.pathIndex < other.pathIndex;
                return th.pointIndex < other.pointIndex;
            }
        }
        //#define PathIntersection_AlternateDefinition1
        private List<Path> mPaths = new List<Path>();
        private Dictionary<PathCoordinate, List<PathCoordinate>> mIntersectionsMap = new Dictionary<PathCoordinate, List<PathCoordinate>>();
        private List<List<PathCoordinate>> mIntersections = new List<List<PathCoordinate>>();

        public void clear() {
            mPaths.Clear();
        }

        public MultiPath addPath(Path path) {
            mPaths.Add(path);
            return this;
        }

        public MultiPath addMultiPath(MultiPath multiPath) {
            //for (List<Path>.Enumerator it = multiPath.mPaths.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in multiPath.mPaths) {
                mPaths.Add(it);
            }
            return this;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: uint getPathCount() const
        public int getPathCount() {
            return mPaths.Count;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Path getPath(uint i) const
        public Path getPath(int i) {
            return mPaths[i];
        }

        //-----------------------------------------------------------------------
        /// <summary>
        /// 查找交集
        /// </summary>
        public void _calcIntersections() {
            mIntersectionsMap.Clear();
            mIntersections.Clear();
            //std::map<Ogre::Vector3, PathIntersection, Vector3Comparator> pointSet;        
            Dictionary<Vector3, List<PathCoordinate>> pointSet = new Dictionary<Vector3, List<PathCoordinate>>();
            //       for (std::vector<Path>::iterator it = mPaths.begin(); it!= mPaths.end(); ++it)
            uint it_index = 0;
            foreach (var it in mPaths) {

                uint it_point_index = 0;
                //for (std::vector<Ogre::Vector3>::const_iterator it2 = it->getPoints().begin(); it2 != it->getPoints().end(); ++it2)
                foreach (var it2 in it._getPoints()) {

                    //PathCoordinate pc(it-mPaths.begin(), it2-it->getPoints().begin());
                    PathCoordinate pc = new PathCoordinate(it_index, it_point_index);
                    //if (pointSet.find(*it2)==pointSet.end())
                    if (!pointSet.ContainsKey(it2)) {
                        List<PathCoordinate> pi = new List<PathCoordinate>();
                        pi.Add(pc);
                        pointSet[it2] = pi;
                    }
                    else {
                        pointSet[it2].Add(pc);
                    }

                    it_point_index++;
                }
                it_index++;
            }

            //#if PathIntersection_AlternateDefinition1
            //for (Dictionary<Vector3, List<PathCoordinate>>.Enumerator it = pointSet.begin(); it.MoveNext(); ++it)
            foreach (var it_ps in pointSet) {
                //#elif PathIntersection_AlternateDefinition2
                //		for (std.map<Vector3, List<PathCoordinate>, Vector3Comparator>.Enumerator it = pointSet.begin(); it.MoveNext(); ++it)
                //#endif
                if (it_ps.Value.Count > 1) {
                    //#if PathIntersection_AlternateDefinition1
                    //for (List<PathCoordinate>.Enumerator it2 = it.second.begin(); it2.MoveNext(); ++it2)
                    foreach (var it2 in it_ps.Value) {
                        //#elif PathIntersection_AlternateDefinition2
                        //				for (List<PathCoordinate>.Enumerator it2 = it.second.begin(); it2.MoveNext(); ++it2)
                        //#endif
                        mIntersectionsMap[it2] = it_ps.Value;
                    }
                    mIntersections.Add(it_ps.Value);
                }
            }
            //          
        }

        //private V pointSet_find<T, V>(Dictionary<T, V> pointSet, T vector3) {
        //    return pointSet[vector3];
        //}

        //private KeyValuePair<T, V> pointSet_end<T, V>(Dictionary<T, V> pointSet) {
        //    Dictionary<T, V>.Enumerator it_end = pointSet.GetEnumerator();
        //    KeyValuePair<T, V> end = it_end.Current;
        //    while (it_end.MoveNext()) {
        //        end = it_end.Current;
        //    }
        //    return end;
        //}

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline const std::map<PathCoordinate, List<PathCoordinate>>& getIntersectionsMap() const
        public Dictionary<PathCoordinate, List<PathCoordinate>> getIntersectionsMap() {
            return mIntersectionsMap;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: inline const List<List<PathCoordinate>>& getIntersections() const
        public List<List<PathCoordinate>> getIntersections() {
            return mIntersections;
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: List<std::pair<uint, uint> > getNoIntersectionParts(uint pathIndex) const
        /// <summary>
        /// 获取非交集
        /// </summary>
        /// <param name="pathIndex"></param>
        /// <returns></returns>
        public List<KeyValuePair<int, int>> getNoIntersectionParts(int pathIndex) {
            Path path = mPaths[pathIndex];
            List<KeyValuePair<int, int>> result = new List<KeyValuePair<int, int>>();
            List<int> intersections = new List<int>();
            //for (std.map<PathCoordinate, List<PathCoordinate>>.Enumerator it = mIntersectionsMap.begin(); it != mIntersectionsMap.end(); ++it) {
            foreach (var it in mIntersectionsMap) {
                if (it.Key.pathIndex == pathIndex) {
                    intersections.Add((int)it.Key.pointIndex);
                }
            }
            //std.sort(intersections.GetEnumerator(), intersections.end());
            intersections.Sort((x, y) => {
                return x - y;//正序排序(重写int比较器，|x|>|y|返回正数，|x|=|y|返回0，|x|<|y|返回负数)  
            });
            int begin = 0;
            //for (std::vector<int>::iterator it = intersections.begin(); it!= intersections.end(); ++it)
            //{
            //    if (*it-1>begin)
            //        result.push_back(std::pair<unsigned int, unsigned int>(begin, *it-1));
            //    begin = *it+1;
            //}
            //if (path.getSegCount() > begin)
            //    result.push_back(std::pair<unsigned int, unsigned int>(begin, path.getSegCount()));

            //for (List<int>.Enumerator it = intersections.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in intersections) {
                if (it - 1 > begin)
                    result.Add(new KeyValuePair<int, int>(begin, it - 1));
                begin = it + 1;
            }
            if (path.getSegCount() > begin)
                result.Add(new KeyValuePair<int, int>(begin, path.getSegCount()));
            return result;
        }
    }

}


