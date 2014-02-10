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
//#ifndef PROCEDURAL_PATH_INCLUDED
#define PROCEDURAL_PATH_INCLUDED
//write with new std ....ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
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
        private std_vector<Vector3> mPoints = new std_vector<Vector3>();
        private bool mClosed;
        /// Default constructor
        public Path() {
            mClosed = false;
        }

        //* Adds a point to the path, as a Vector3 
        public Path addPoint(Vector3 pt) {
            mPoints.push_back(pt);
            return this;
        }

        //* Adds a point to the path, using its 3 coordinates 
        public Path addPoint(float x, float y, float z) {
            mPoints.push_back(new Vector3(x, y, z));
            return this;
        }

        /// Inserts a point to the path
        /// @param index the index before the inserted point
        /// @param x new point's x coordinate
        /// @param y new point's y coordinate
        /// @param z new point's z coordinate
        public Path insertPoint(int index, float x, float y, float z) {
            //
            mPoints.insert(index, new Vector3(x, y, z));
            return this;
        }

        /// Inserts a point to the path
        /// @param index the index before the inserted point
        /// @param pt new point's position
        public Path insertPoint(int index, Vector3 pt) {
            //
            mPoints.insert(index, pt);
            return this;
        }

        /// Appends another path at the end of this one
        public Path appendPath(Path other) {
            //mPoints.insert(mPoints.end(), other.mPoints.begin(), other.mPoints.end());
            mPoints.AddRange(other.mPoints.ToArray());
            return this;
        }

        ///<summary>
        /// Appends another path at the end of this one, relative to the last point of this path
        ///</summary>
        public Path appendPathRel(Path other) {
            if (mPoints.empty())
                appendPath(other);
            else {
                Vector3 refVector = mPoints[mPoints.Count - 1]; //*(mPoints.end()-1);
                Vector3[] pointList = (other.mPoints.ToArray());
                //for (List<Vector3>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
                //	it.Current += refVector;
                for (int i = 0; i < pointList.Length; i++) {
                    pointList[i] += refVector;
                }
                //mPoints.insert(mPoints.end(), pointList.begin(), pointList.end());
                mPoints.AddRange(pointList);
            }
            return this;
        }

        //* Clears the content of the Path 
        public Path reset() {
            mPoints.clear();
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
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALID_STATE", "Cannot close an empty path", "Procedural::Path::close()");
            ;
            mClosed = true;
            return this;
        }

        private void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p + "_" + p_2 + "_" + p_3);
        }

        //* Tells if the path is closed or not 
        //
        //ORIGINAL LINE: bool isClosed() const
        public bool isClosed() {
            return mClosed;
        }

        //* Gets the list of points as a vector of Vector3 
        //
        //ORIGINAL LINE: const List<Ogre::Vector3>& getPoints() const
        public std_vector<Vector3> _getPoints() {
            //return mPoints; 
            return getPointsReference();
        }
        public Vector3[] getPoints() {
            return mPoints.ToArray();
        }
        /// Gets raw vector data of this path as a non-const reference
        public std_vector<Vector3> getPointsReference() {
            return mPoints;
        }

        //    * Safely gets a given point.
        //	 * Takes into account whether the path is closed or not.
        //	 * @param i the index of the point.
        //	 *          if it is <0 or >maxPoint, cycle through the list of points
        //	 
        //
        //ORIGINAL LINE: const Ogre::Vector3& getPoint(int i) const
        public Vector3 getPoint(int i) {
            if (mClosed)
                return mPoints[Utils.modulo(i, mPoints.size())];
            return mPoints[Utils.cap(i, 0, mPoints.size() - 1)];
        }

        //    * Gets the number of segments in the path
        //	 * Takes into accound whether path is closed or not
        //	 
        //
        //ORIGINAL LINE: int getSegCount() const
        public int getSegCount() {
            return (mPoints.size() - 1) + (mClosed ? 1 : 0);
        }

        //    *
        //	 * Returns local direction after the current point
        //	 
        //
        //ORIGINAL LINE: Ogre::Vector3 getDirectionAfter(uint i) const
        public Vector3 getDirectionAfter(int i) {
            // If the path isn't closed, we get a different calculation at the end, because
            // the tangent shall not be null
            if (!mClosed && i == mPoints.size() - 1 && i > 0)
                return (mPoints[i] - mPoints[i - 1]).NormalisedCopy;
            else
                return (getPoint(i + 1) - getPoint(i)).NormalisedCopy;
        }

        //    *
        //	 * Returns local direction after the current point
        //	 
        //
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
        //
        //ORIGINAL LINE: Ogre::Vector3 getAvgDirection(uint i) const
        public Vector3 getAvgDirection(int i) {
            return (getDirectionAfter(i) + getDirectionBefore(i)).NormalisedCopy;
        }

        /// Returns the total lineic length of that shape
        //
        //ORIGINAL LINE: Ogre::float getTotalLength() const
        public float getTotalLength() {
            float length = 0;
            for (int i = 0; i < mPoints.size() - 1; ++i) {
                length += (mPoints[i + 1] - mPoints[i]).Length;
            }
            if (mClosed) {
                length += (mPoints[mPoints.Count - 1] - mPoints[0]).Length;
            }
            return length;
        }


        //
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
        //
        //ORIGINAL LINE: inline Ogre::Vector3 getPosition(uint i, Ogre::float coord) const
        public Vector3 getPosition(int i, float coord) {
            if (i >= mPoints.Count)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Out of Bounds", "Procedural::Path::getPosition(unsigned int, Ogre::Real)");
            ;
            if (coord < 0.0f || coord > 1.0f)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Coord must be comprised between 0 and 1", "Procedural::Path::getPosition(unsigned int, Ogre::Real)");
            ;
            Vector3 A = getPoint(i);
            Vector3 B = getPoint(i + 1);
            return A + coord * (B - A);
        }

        /// Gets a position on the shape from lineic coordinate
        /// @param coord lineic coordinate
        /// @exception Ogre::InvalidStateException The path must at least contain 2 points
        //
        //ORIGINAL LINE: Ogre::Vector3 getPosition(Ogre::float coord) const
        public Vector3 getPosition(float coord) {
            if (mPoints.Count < 2)
                OGRE_EXCEPT("Ogre::Exception::ERR_INVALID_STATE", "The path must at least contain 2 points", "Procedural::Path::getPosition(Ogre::Real)");
            ;
            int i = 0;
            while (true) {
                float nextLen = (getPoint(i + 1) - getPoint(i)).Length;
                if (coord > nextLen)
                    coord -= nextLen;
                else
                    return getPosition(i, coord / nextLen);
                if (!mClosed && i >= mPoints.size() - 2)
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
        //
        //ORIGINAL LINE: Ogre::MeshPtr realizeMesh(const string& name = "") const
        //
        public MeshPtr realizeMesh(string name) {
            SceneManagerEnumerator.SceneManagerIterator item = Root.Singleton.GetSceneManagerIterator();
            item.MoveNext();//
            Mogre.SceneManager smgr = item.Current;
            item.Dispose();
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
        //
        //ORIGINAL LINE: Path mergeKeysWithTrack(const Track& track) const
        public Path mergeKeysWithTrack(Track track) {
            if (!track.isInsertPoint() || track.getAddressingMode() == Track.AddressingMode.AM_POINT)
                return this;
            float totalLength = getTotalLength();

            float lineicPos = 0;
            float pathLineicPos = 0;
            Path outputPath = new Path();
            outputPath.addPoint(getPoint(0));
            for (int i = 1; i < mPoints.size(); ) {
                float nextLineicPos = pathLineicPos + (mPoints[i] - mPoints[i - 1]).Length;

                std_pair<float, float> it = track._getKeyValueAfter(lineicPos, lineicPos / totalLength, (uint)(i - 1));

                float nextTrackPos = it.first;
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

        public void buildFromSegmentSoup(std_vector<Segment3D> segList, ref std_vector<Path> @out)
        {
            //typedef std::multimap<Vector3, Vector3, Vector3Comparator> Vec3MultiMap;
            //Vec3MultiMap segs;
           std_multimap<Vector3,Vector3>segs=new std_multimap<Vector3,Vector3>(new Vector3Comparator());
           // for (std::vector<Segment3D>::const_iterator it = segList.begin(); it != segList.end(); ++it)
           foreach(var it in segList)
           {
                //segs.insert(std::pair<Vector3, Vector3 > (it->mA, it->mB));
                //segs.insert(std::pair<Vector3, Vector3 > (it->mB, it->mA));
               segs.insert(it.mA,it.mB);
               segs.insert(it.mB,it.mA);
            }
            while (!segs.empty())
            {
               Vector3 headFirst = segs.get(0).first;//segs.begin()->first;
                Vector3 headSecond = segs.get(0).second[0];//segs.begin()->second;
                Path p=new Path();
                p.addPoint(headFirst).addPoint(headSecond);
                //Vec3MultiMap::iterator firstSeg = segs.begin();
                int firstSeg_pos=segs.begin();
                Vector3 firstSeg=segs.get(0).first;
                //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants2 = segs.equal_range(headSecond);
               std_pair<std_pair<Vector3,List<Vector3>>,std_pair<Vector3,List<Vector3>>>  correspondants2 = segs.equal_range(headSecond);
                //for (Vec3MultiMap::iterator it = correspondants2.first; it != correspondants2.second;)
                for(int i=correspondants2.first.second.Count-1;i>=0;i--)
                {
                   // Vec3MultiMap::iterator removeIt = it++;
                    Vector3 removeIt=correspondants2.first.second[i];
                    //if ((removeIt->second - firstSeg->first).squaredLength() < 1e-8)
                    if((removeIt-firstSeg).SquaredLength<1e-8)
                        segs.erase(removeIt);
                }
                segs.erase(firstSeg);
                bool foundSomething = true;
                while (!segs.empty() && foundSomething)
                {
                    foundSomething = false;
                    //Vec3MultiMap::iterator next = segs.find(headSecond);
                    int next_pos = segs.find(headSecond);
                    //if (next != segs.end())
                    if(next_pos!=-1)
                    {
                        std_pair<Vector3,List<Vector3>>next=segs.get((uint)next_pos);
                        foundSomething = true;
                        headSecond = next.second[0];
                        p.addPoint(headSecond);
                        //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headSecond);
                        std_pair<std_pair<Vector3,List<Vector3>>,std_pair<Vector3,List<Vector3>>>correspondants = segs.equal_range(headSecond);
                        //for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
                        for (int i = correspondants.first.second.Count - 1; i >= 0;i-- ) {
                            //Vec3MultiMap::iterator removeIt = it++;
                            Vector3 removeIt = correspondants.first.second[i];
                            //if ((removeIt->second - next->first).squaredLength() < 1e-8)
                            if ((removeIt - next.first).SquaredLength < 1e-8)
                                segs.erase(removeIt);
                        }
                        //segs.erase(next);
                        segs.erase(next.first);
                    }
                    //Vec3MultiMap::iterator previous = segs.find(headFirst);
                    int previous_pos=segs.find(headFirst);
                    //if (previous != segs.end())
                    if(previous_pos!=-1)
                    {
                        std_pair<Vector3, List<Vector3>> previous = segs.get((uint)previous_pos);
                        foundSomething = true;
                        //p.insertPoint(0, previous.second);
                        p.insertPoint(0, previous.second[0]);//???
                        headFirst = previous.second[0];
                        //std::pair<Vec3MultiMap::iterator, Vec3MultiMap::iterator> correspondants = segs.equal_range(headFirst);
                        std_pair<std_pair<Vector3,List<Vector3>>,std_pair<Vector3,List<Vector3>>>correspondants = segs.equal_range(headFirst);
                        //for (Vec3MultiMap::iterator it = correspondants.first; it != correspondants.second;)
                        for(int i=correspondants.first.second.Count-1;i>=0;i--)
                        {
                            //Vec3MultiMap::iterator removeIt = it++;
                            Vector3 removeIt=correspondants.first.second[i];
                            //if ((removeIt->second - previous->first).squaredLength() < 1e-8)
                            if((removeIt-previous.first).SquaredLength<1e-8) 
                            segs.erase(removeIt);
                        }
                        //segs.erase(previous);
                        segs.erase(previous.first);
                    }
                }
                if ((p.getPoint(0)-p.getPoint(p.getSegCount() + 1)).SquaredLength < 1e-6)
                {
                    p.getPointsReference().pop_back();
                    p.close();
                }
                @out.push_back(p);
            }
        }



        /// Converts the path to a shape, with Y=0

        //-----------------------------------------------------------------------
        //
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
            //
            //ORIGINAL LINE: bool operator < (const PathCoordinate& STLAllocator<U, AllocPolicy>) const
            public static bool operator <(PathCoordinate th, PathCoordinate other) {
                if (th.pathIndex != other.pathIndex)
                    return th.pathIndex < other.pathIndex;
                return th.pointIndex < other.pointIndex;
            }
            public static bool operator >(PathCoordinate th, PathCoordinate other) {
                if (th.pathIndex != other.pathIndex)
                    return th.pathIndex > other.pathIndex;
                return th.pointIndex > other.pointIndex;
            }
        }
        //#define PathIntersection_AlternateDefinition1
        private std_vector<Path> mPaths = new std_vector<Path>();
        private std_map<PathCoordinate, std_vector<PathCoordinate>> mIntersectionsMap = new std_map<PathCoordinate, std_vector<PathCoordinate>>();
        private std_vector<std_vector<PathCoordinate>> mIntersections = new std_vector<std_vector<PathCoordinate>>();

        public void clear() {
            mPaths.clear();
        }

        public MultiPath addPath(Path path) {
            mPaths.push_back(path);
            return this;
        }

        public MultiPath addMultiPath(MultiPath multiPath) {
            //for (List<Path>.Enumerator it = multiPath.mPaths.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in multiPath.mPaths) {
                mPaths.push_back(it);
            }
            return this;
        }

        //
        //ORIGINAL LINE: uint getPathCount() const
        public int getPathCount() {
            return mPaths.size();
        }

        //
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
            std_map<Vector3, std_vector<PathCoordinate>> pointSet = new std_map<Vector3, std_vector<PathCoordinate>>(new Vector3Comparator());
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
                        std_vector<PathCoordinate> pi = new std_vector<PathCoordinate>();
                        pi.Add(pc);
                        //pointSet[it2] = pi;
                        pointSet.Add(it2, pi);
                    }
                    else {
                        pointSet[it2].push_back(pc);
                    }

                    it_point_index++;
                }
                it_index++;
            }
            //for (std::map<Ogre::Vector3, PathIntersection, Vector3Comparator>::iterator it = pointSet.begin(); it != pointSet.end(); ++it)
            foreach (var it_ps in pointSet) {
                if (it_ps.Value.size() > 1) {
                    foreach (var it2 in it_ps.Value) {
                        //mIntersectionsMap[*it2] = it->second;
                        if (mIntersectionsMap.ContainsKey(it2)) {
                            mIntersectionsMap[it2] = it_ps.Value;
                        }
                        else {
                            mIntersectionsMap.Add(it2, it_ps.Value);
                        }
                    }
                    mIntersections.push_back(it_ps.Value);
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

        //
        //ORIGINAL LINE: inline const std::map<PathCoordinate, List<PathCoordinate>>& getIntersectionsMap() const
        public std_map<PathCoordinate, std_vector<PathCoordinate>> getIntersectionsMap() {
            return mIntersectionsMap;
        }

        //
        //ORIGINAL LINE: inline const List<List<PathCoordinate>>& getIntersections() const
        public std_vector<std_vector<PathCoordinate>> getIntersections() {
            return mIntersections;
        }

        //
        //ORIGINAL LINE: List<std::pair<uint, uint> > getNoIntersectionParts(uint pathIndex) const
        /// <summary>
        /// 获取非交集
        /// </summary>
        /// <param name="pathIndex"></param>
        /// <returns></returns>
        public std_vector<std_pair<uint, uint>> getNoIntersectionParts(uint pathIndex) {
            Path path = mPaths[(int)pathIndex];
            std_vector<std_pair<uint, uint>> result = new std_vector<std_pair<uint, uint>>();
            std_vector<int> intersections = new std_vector<int>();
            //for (std.map<PathCoordinate, List<PathCoordinate>>.Enumerator it = mIntersectionsMap.begin(); it != mIntersectionsMap.end(); ++it) {
            foreach (var it in mIntersectionsMap) {
                if (it.Key.pathIndex == pathIndex) {
                    intersections.push_back((int)it.Key.pointIndex);
                }
            }
            //std.sort(intersections.GetEnumerator(), intersections.end());
            intersections.Sort((x, y) => {
                return x - y;//正序排序(重写int比较器，|x|>|y|返回正数，|x|=|y|返回0，|x|<|y|返回负数)  
            });
            //Array.Sort(intersections.ToArray());

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
                    result.push_back(new std_pair<uint, uint>((uint)begin, (uint)it - 1));
                begin = it + 1;
            }
            if (path.getSegCount() > begin)
                result.push_back(new std_pair<uint, uint>((uint)begin, (uint)path.getSegCount()));
            return result;
        }
    }

}


