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

#define PROCEDURAL_SHAPE_INCLUDED

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;

    public enum Side : int
    {
        SIDE_LEFT,
        SIDE_RIGHT
    }

    //* @} 

    //*
    // * \ingroup shapegrp
    // * Describes a succession of interconnected 2D points.
    // * It can be closed or not, and there's always an outside and an inside
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Shape
    public class Shape
    {
        public class IntersectionInShape
        {
            public uint[] index = new uint[2];
            public bool[] onVertex = new bool[2];
            public Vector2 position = new Vector2();
            public IntersectionInShape(uint i, uint j, Vector2 intersect) {
                position = intersect;//new Vector2(intersect);
                index[0] = i;
                index[1] = j;
                onVertex[0] = false;
                onVertex[1] = false;
            }
        }
        public enum BooleanOperationType : int
        {
            BOT_UNION,
            BOT_INTERSECTION,
            BOT_DIFFERENCE
        }
        private List<Vector2> mPoints = new List<Vector2>();
        private bool mClosed;
        private Side mOutSide;

        /// Default constructor
        public Shape() {
            mClosed = false;
            mOutSide = Side.SIDE_RIGHT;
        }

        /// Adds a point to the shape
        public Shape addPoint(Vector2 pt) {
            mPoints.Add(pt);
            return this;
        }

        /// Adds a point to the shape
        public Shape addPoint(float x, float y) {
            mPoints.Add(new Vector2(x, y));
            return this;
        }

        /// Inserts a point to the shape
        /// @param index the index before the inserted point
        /// @param x new point's x coordinate
        /// @param y new point's y coordinate
        public Shape insertPoint(int index, float x, float y) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            //mPoints.Insert(mPoints.GetEnumerator()+index, Vector2(x, y));
            mPoints.Insert(index, new Vector2(x, y));
            return this;
        }

        /// Inserts a point to the shape
        /// @param index the index before the inserted point
        /// @param pt new point's position
        public Shape insertPoint(int index, Vector2 pt) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            //mPoints.insert(mPoints.GetEnumerator()+index, pt);
            mPoints.Insert(index, pt);
            return this;
        }

        /// Adds a point to the shape, relative to the last point added
        public Shape addPointRel(Vector2 pt) {
            if (mPoints.Count == 0) {
                mPoints.Add(pt);
            }
            else {
                Vector2 end = mPoints[mPoints.Count - 1];
                mPoints.Add(pt + end);
            }
            return this;
        }

        /// Adds a point to the shape, relative to the last point added
        public Shape addPointRel(float x, float y) {
            if (mPoints.Count == 0)
                mPoints.Add(new Vector2(x, y));
            else {
                Vector2 end = mPoints[mPoints.Count - 1];
                mPoints.Add(new Vector2(x, y) + end);
            }
            return this;
        }
        public Shape addPointRel(List<Vector2> pointList) {
            if (mPoints.Count == 0) {
                mPoints.AddRange(pointList.ToArray());
            }
            else {
                Vector2 refVector = mPoints[mPoints.Count - 1];
                foreach (var it in pointList) {
                    addPoint(it + refVector);
                }
            }
            return this;
        }
        /// Appends another shape at the end of this one
        public Shape appendShape(Shape other) {
            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
            //mPoints.insert(mPoints.end(), STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
            mPoints.AddRange(other.mPoints.ToArray());
            return this;
        }

        /// Appends another shape at the end of this one, relative to the last point of this shape
        public Shape appendShapeRel(Shape other) {
            return addPointRel(other.mPoints);
            if (mPoints.Count == 0)
                appendShape(other);
            else {
                Vector2 refVector = mPoints[mPoints.Count - 1];// *(mPoints.end()-1);
                List<Vector2> pointList = other.mPoints;//new List<Vector2>(STLAllocator<U, AllocPolicy>.mPoints.GetEnumerator(), STLAllocator<U, AllocPolicy>.mPoints.end());
                //            for (List<Vector2>.Enumerator it = pointList.GetEnumerator(); it.MoveNext(); ++it)
                //                it.Current +=refVector;
                ////C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
                //            mPoints.insert(mPoints.end(), pointList.GetEnumerator(), pointList.end());
                foreach (var it in pointList) {
                    addPoint(it + refVector);
                }
            }
            return this;
        }

        /// Extracts a part of the shape as a new shape
        /// @param first first index to be in the new shape
        /// @param last last index to be in the new shape
        public Shape extractSubShape(uint first, uint last) {
            Shape s = new Shape();
            for (int i = (int)first; i <= last; i++)
                s.addPoint(mPoints[i]);
            s.setOutSide(mOutSide);
            if (mClosed)
                s.close();
            return s;
        }

        /// Reverses direction of the shape
        /// The outside is preserved
        public Shape reverse() {
            //std.reverse(mPoints.GetEnumerator(), mPoints.end());
            mPoints.Reverse();
            switchSide();
            return this;
        }
        ///<summary>
        ///Clears the content of the shape
        ///</summary>
        public Shape reset() {
            mPoints.Clear();
            return this;
        }

        /// Converts the shape to a path, with Y=0
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: Path convertToPath() const
        public Path convertToPath() {
            Path p = new Path();
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mPoints) {
                p.addPoint(it.x, 0, it.y);
            }
            if (mClosed)
                p.close();

            return p;
        }

        /// Outputs a track, with Key=X and Value=Y
        //-----------------------------------------------------------------------
        public Track convertToTrack() {
            return convertToTrack(Track.AddressingMode.AM_RELATIVE_LINEIC);
        }
        //
        //ORIGINAL LINE: Track convertToTrack(Track::AddressingMode addressingMode =Track::AM_RELATIVE_LINEIC) const
       //
        public Track convertToTrack(Track.AddressingMode addressingMode) {
            Track t = new Track(addressingMode);
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mPoints) {
                t.addKeyFrame(it.x, it.y);
            }
            return t;
        }

        /// Gets a copy of raw vector data of this shape
        //
        //ORIGINAL LINE: inline List<Ogre::Vector2> getPoints() const
        public Vector2[] getPoints() {
            return mPoints.ToArray();
        }

        /// Gets raw vector data of this shape as a non-const reference
        //public List<Vector2> getPointsReference()
        //{
        //    return mPoints;
        //}

        /// Gets raw vector data of this shape as a non-const reference
        //
        //ORIGINAL LINE: inline const List<Ogre::Vector2>& getPointsReference() const
        public List<Vector2> getPointsReference() {
            return mPoints;
        }

        //    *
        //	 * Bounds-safe method to get a point : it will allow you to go beyond the bounds
        //	 
        //
        //ORIGINAL LINE: inline const Ogre::Vector2& getPoint(int i) const
        public Vector2 getPoint(int i) {
            return mPoints[getBoundedIndex(i)];
        }

        //
        //ORIGINAL LINE: inline uint getBoundedIndex(int i) const
        public int getBoundedIndex(int i) {
            if (mClosed)
                return Utils.modulo(i, mPoints.Count);
            return Utils.cap(i, 0, mPoints.Count - 1);
        }

        /// Gets number of points in current point list
        //
        //ORIGINAL LINE: inline const List<Ogre::Vector2>::size_type getPointCount() const
        public int getPointCount() {
            return mPoints.Count;
        }

        //    *
        //	 * Makes the shape a closed shape, ie it will automatically connect
        //	 * the last point to the first point.
        //	 
        public Shape close() {
            mClosed = true;
            return this;
        }

        //    *
        //	 * Sets which side (left or right) is on the outside of the shape.
        //	 * It is used for such things as normal generation
        //	 * Default is right, which corresponds to placing points anti-clockwise.
        //	 
        public Shape setOutSide(Side side) {
            mOutSide = side;
            return this;
        }

        /// Gets which side is out
        //
        //ORIGINAL LINE: inline Side getOutSide() const
        public Side getOutSide() {
            return mOutSide;
        }

        /// Switches the inside and the outside
        public Shape switchSide() {
            mOutSide = (mOutSide == Side.SIDE_LEFT) ? Side.SIDE_RIGHT : Side.SIDE_LEFT;
            return this;
        }

        /// Gets the number of segments in that shape
        //
        //ORIGINAL LINE: inline int getSegCount() const
        public int getSegCount() {
            return (mPoints.Count - 1) + (mClosed ? 1 : 0);
        }

        /// Gets whether the shape is closed or not
        //
        //ORIGINAL LINE: inline bool isClosed() const
        public bool isClosed() {
            return mClosed;
        }

        //    *
        //	 * Returns local direction after the current point
        //	 
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getDirectionAfter(uint i) const
        public Vector2 getDirectionAfter(uint index) {
            int i = (int)index;
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
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getDirectionBefore(uint i) const
        public Vector2 getDirectionBefore(uint index) {
            int i = (int)index;
            // If the path isn't closed, we get a different calculation at the end, because
            // the tangent shall not be null
            if (!mClosed && i == 1)
                return (mPoints[1] - mPoints[0]).NormalisedCopy;
            else
                return (getPoint(i) - getPoint(i - 1)).NormalisedCopy;
        }

        /// Gets the average between before direction and after direction
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getAvgDirection(uint i) const
        public Vector2 getAvgDirection(uint i) {
            return (getDirectionAfter(i) + getDirectionBefore(i)).NormalisedCopy;
        }

        /// Gets the shape normal just after that point
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getNormalAfter(uint i) const
        public Vector2 getNormalAfter(uint i) {
            if (mOutSide == Side.SIDE_RIGHT)
                return -getDirectionAfter(i).Perpendicular;
            return getDirectionAfter(i).Perpendicular;
        }

        /// Gets the shape normal just before that point
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getNormalBefore(uint i) const
        public Vector2 getNormalBefore(uint i) {
            if (mOutSide == Side.SIDE_RIGHT)
                return -getDirectionBefore(i).Perpendicular;
            return getDirectionBefore(i).Perpendicular;
        }

        /// Gets the "normal" of that point ie an average between before and after normals
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getAvgNormal(uint i) const
        public Vector2 getAvgNormal(uint i) {
            if (mOutSide == Side.SIDE_RIGHT)
                return -getAvgDirection(i).Perpendicular;
            return getAvgDirection(i).Perpendicular;
        }

        //    *
        //	 * Outputs a mesh representing the shape.
        //	 * Mostly for debugging purposes
        //	 
        //-----------------------------------------------------------------------
        public MeshPtr realizeMesh() {
            return realizeMesh("");
        }
        //
        //ORIGINAL LINE: MeshPtr realizeMesh(const string& name ="") const
       //
        public MeshPtr realizeMesh(string name) {
            if (string.IsNullOrEmpty(name)) {
                name = Guid.NewGuid().ToString("N");
            }
            Mogre.SceneManager smgr = Root.Singleton.GetSceneManagerIterator().Current;
            ManualObject manual = smgr.CreateManualObject(name);
            manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_STRIP);

            _appendToManualObject(manual);

            manual.End();
            MeshPtr mesh = null;//new MeshPtr();
            if (name == "")
                mesh = manual.ConvertToMesh(Utils.getName("Procedural_Shape_"));
            else
                mesh = manual.ConvertToMesh(name);
            smgr.DestroyManualObject(manual);
            return mesh;
        }

        //    *
        //	 * Appends the shape vertices to a manual object being edited
        //	 
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: void _appendToManualObject(ManualObject* manual) const
        public void _appendToManualObject(ManualObject manual) {
            //for (List<Vector2>.Enumerator itPos = mPoints.GetEnumerator(); itPos.MoveNext(); itPos++)
            foreach (var itPos in mPoints) {
                manual.Position(new Vector3(itPos.x, itPos.y, 0.0f));
            }
            if (mClosed) {
                //manual.Position(new Vector3(mPoints.GetEnumerator().x, mPoints.GetEnumerator().y, 0.0f));
                manual.Position(new Vector3(mPoints[0].x, mPoints[0].y, 0f));
            }
        }

        //    *
        //	 * Tells whether a point is inside a shape or not
        //	 * @param point The point to check
        //	 * @return true if the point is inside this shape, false otherwise
        //	 
        //
        //ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& point) const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	bool isPointInside(Ogre::Vector2 point);
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool Shape::isPointInside(const Vector2& point) const
        public bool isPointInside(Vector2 point) {
            // Draw a horizontal lines that goes through "point"
            // Using the closest intersection, find whethe the point is actually inside
            int closestSegmentIndex = -1;
            float closestSegmentDistance = float.MaxValue;//std.numeric_limits<Real>.max();
            Vector2 closestSegmentIntersection = new Vector2(0f, 0f);

            for (int i = 0; i < getSegCount(); i++) {
                Vector2 A = getPoint(i);
                Vector2 B = getPoint(i + 1);
                if (A.y != B.y && (A.y - point.y) * (B.y - point.y) <= 0.0f) {
                    Vector2 intersect = new Vector2(A.x + (point.y - A.y) * (B.x - A.x) / (B.y - A.y), point.y);
                    float dist = Math.Abs(point.x - intersect.x);
                    if (dist < closestSegmentDistance) {
                        closestSegmentIndex = i;
                        closestSegmentDistance = dist;
                        //
                        //ORIGINAL LINE: closestSegmentIntersection = intersect;
                        closestSegmentIntersection = (intersect);
                    }
                }
            }
            if (closestSegmentIndex != -1) {
                if (getNormalAfter((uint)closestSegmentIndex).x * (point.x - closestSegmentIntersection.x) < 0f)
                    return true;
                else
                    return false;
            }
            if (findRealOutSide() == mOutSide)
                return false;
            else
                return true;
        }
        /// <summary>
        /// one.Key–°”⁄two.Key
        /// </summary>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static bool _sortAngles(KeyValuePair<Radian, byte> one, KeyValuePair<Radian, byte> two) // waiting for lambda functions!
        {
            return one.Key < two.Key;
        }
        //    *
        //	 * Computes the intersection between this shape and another one.
        //	 * Both shapes must be closed.
        //	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleanintersection.png "Intersection of the two shapes"</td></tr></table>
        //	 * @param other The shape against which the intersection is computed
        //	 * @return The intersection of two shapes, as a new shape
        //	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
        //	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
        //	 
        //-----------------------------------------------------------------------


        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: MultiShape booleanIntersect(const Shape& STLAllocator<U, AllocPolicy>) const
        public MultiShape booleanIntersect(Shape other) {
            return _booleanOperation(other, BooleanOperationType.BOT_INTERSECTION);
        }

        //    *
        //	 * Computes the union between this shape and another one.
        //	 * Both shapes must be closed.
        //	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleanunion.png "Union of the two shapes"</td></tr></table>
        //	 * @param other The shape against which the union is computed
        //	 * @return The union of two shapes, as a new shape
        //	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
        //	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
        //	 
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: MultiShape booleanUnion(const Shape& STLAllocator<U, AllocPolicy>) const
        public MultiShape booleanUnion(Shape other) {
            return _booleanOperation(other, BooleanOperationType.BOT_UNION);
        }

        //    *
        //	 * Computes the difference between this shape and another one.
        //	 * Both shapes must be closed.
        //	 * <table border="0" width="100%"><tr><td>\image html shape_booleansetup.png "Start shapes"</td><td>\image html shape_booleandifference.png "Difference of the two shapes"</td></tr></table>
        //	 * @param other The shape against which the diffenrence is computed
        //	 * @return The difference of two shapes, as a new shape
        //	 * @exception Ogre::InvalidStateException Current shapes must be closed and has to contain at least 2 points!
        //	 * @exception Ogre::InvalidParametersException Other shapes must be closed and has to contain at least 2 points!
        //	 
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: MultiShape booleanDifference(const Shape& STLAllocator<U, AllocPolicy>) const
        public MultiShape booleanDifference(Shape other) {
            return _booleanOperation(other, BooleanOperationType.BOT_DIFFERENCE);
        }

        //    *
        //	 * On a closed shape, find if the outside is located on the right
        //	 * or on the left. If the outside can easily be guessed in your context,
        //	 * you'd rather use setOutside(), which doesn't need any computation.
        //	 

        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: Side findRealOutSide() const
        public Side findRealOutSide() {
            float x = mPoints[0].x;
            uint index = 0;
            for (ushort i = 1; i < mPoints.Count; i++) {
                if (x < mPoints[i].x) {
                    x = mPoints[i].x;
                    index = i;
                }
            }
            Radian alpha1 = Utils.angleTo(new Vector2(0f, 1f)/*Vector2.UNIT_Y*/, getDirectionAfter(index));
            Radian alpha2 = Utils.angleTo(new Vector2(0f, 1f)/*Vector2.UNIT_Y*/, -getDirectionBefore(index));
            if (alpha1 < alpha2)
                return Side.SIDE_RIGHT;
            else
                return Side.SIDE_LEFT;
        }

        //    *
        //	 * Determines whether the outside as defined by user equals "real" outside
        //	
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool isOutsideRealOutside() const
        public bool isOutsideRealOutside() {
            return findRealOutSide() == mOutSide;
        }

        /// Creates a shape with the keys of this shape and extra keys coming from a track
        /// @param track the track to merge keys with
        /// @return a new Shape coming from the merge between original shape and the track
        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: Shape mergeKeysWithTrack(const Track& track) const
        public Shape mergeKeysWithTrack(Track track) {
            if (!track.isInsertPoint() || track.getAddressingMode() == Track.AddressingMode.AM_POINT)
                return this;
            float totalLength = getTotalLength();

            float lineicPos = 0;
            float shapeLineicPos = 0;
            Shape outputShape = new Shape();
            if (mClosed)
                outputShape.close();
            outputShape.addPoint(getPoint(0));
            for (int i = 1; i < mPoints.Count; ) {
                float nextLineicPos = shapeLineicPos + (mPoints[i] - mPoints[i - 1]).Length;

                //std.map<Real,Real>.Enumerator it = track._getKeyValueAfter(lineicPos, lineicPos/totalLength, i-1);
                KeyValuePair<float, float> it = track._getKeyValueAfter(lineicPos, lineicPos / totalLength, ((uint)i - 1));
                float nextTrackPos = it.Key;
                if (track.getAddressingMode() == Track.AddressingMode.AM_RELATIVE_LINEIC)
                    nextTrackPos *= totalLength;

                // Adds the closest point to the curve, being either from the shape or the track
                if (nextLineicPos <= nextTrackPos || lineicPos >= nextTrackPos) {
                    outputShape.addPoint(mPoints[i]);
                    i++;
                    lineicPos = nextLineicPos;
                    shapeLineicPos = nextLineicPos;
                }
                else {
                    outputShape.addPoint(getPosition((uint)i - 1, (nextTrackPos - shapeLineicPos) / (nextLineicPos - shapeLineicPos)));
                    lineicPos = nextTrackPos;
                }
            }
            return outputShape;
        }

        //    *
        //	 * Applies the given translation to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param translation the translation vector
        //	 
        public Shape translate(Vector2 translation) {
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            //	it.Current+=translation;
            for (int i = 0; i < mPoints.Count; i++) {
                mPoints[i] += translation;
            }
            return this;
        }

        //    *
        //	 * Applies the given translation to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param translationX X component of the translation vector
        //	 * @param translationY Y component of the translation vector
        //	 
        public Shape translate(float translationX, float translationY) {
            return translate(new Vector2(translationX, translationY));
        }

        //    *
        //	 * Applies the given rotation to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param angle angle of rotation
        //	 
        public Shape rotate(Radian angle) {
            float c = Math.Cos(angle.ValueRadians);
            float s = Math.Sin(angle.ValueRadians);
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            for (int i = 0; i < mPoints.Count; i++) {
                Vector2 it = mPoints[i];
                float x = it.x;
                float y = it.y;
                float it_x = c * x - s * y;
                float it_y = s * x + c * y;
                it = new Vector2(it_x, it_y);
                mPoints[i] = it;
            }
            return this;
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param amount amount of scale
        //	 
        public Shape scale(float amount) {
            return scale(amount, amount);
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param scaleX amount of scale in the X direction
        //	 * @param scaleY amount of scale in the Y direction
        //	 
        public Shape scale(float scaleX, float scaleY) {
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            for (int i = 0; i < mPoints.Count; i++) {
                Vector2 it = mPoints[i];
                float it_x = it.x * scaleX;
                float it_y = it.y * scaleY;
                it = new Vector2(it_x, it_y);
                mPoints[i] = it;
            }
            return this;
        }

        //    *
        //	 * Applies the given scale to all the points already defined.
        //	 * Has strictly no effect on the points defined after that
        //	 * @param amount of scale
        //	 
        public Shape scale(Vector2 amount) {
            return scale(amount.x, amount.y);
        }

        //    *
        //	 * Reflect all points in this shape against a zero-origined line with a given normal
        //	 * @param normal the normal
        //	 
        public Shape reflect(Vector2 normal) {
            //for (List<Vector2>.Enumerator it = mPoints.GetEnumerator(); it.MoveNext(); ++it)
            for (int i = 0; i < mPoints.Count; i++) {
                mPoints[i] = mPoints[i].Reflect(normal);
                //it.Current = it.reflect(normal);
            }
            return this;
        }

        //    *
        //	 * Create a symetric copy at the origin point.
        //	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
        //	 
        public Shape mirror() {
            return mirror(false);
        }
       //
        //ORIGINAL LINE: Shape& mirror(bool flip = false)
        public Shape mirror(bool flip) {
            return mirrorAroundPoint(new Vector2(0f, 0f), flip);
        }

        //    *
        //	 * Create a symetric copy at a given point.
        //	 * @param x x coordinate of point where to mirror
        //	 * @param y y coordinate of point where to mirror
        //	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
        //	 
        public Shape mirror(float x, float y) {
            return mirror(x, y, false);
        }
       //
        //ORIGINAL LINE: Shape& mirror(Ogre::float x, Ogre::float y, bool flip = false)
        public Shape mirror(float x, float y, bool flip) {
            return mirrorAroundPoint(new Vector2(x, y), flip);
        }

        //    *
        //	 * Create a symetric copy at a given point.
        //	 * @param point Point where to mirror
        //	 * @parm flip \c true if function should start mirroring with the last point in list (default \c false)
        //	 
        public Shape mirrorAroundPoint(Vector2 point) {
            return mirrorAroundPoint(point, false);
        }
       //
        //ORIGINAL LINE: Shape& mirrorAroundPoint(Ogre::Vector2 point, bool flip = false)
        public Shape mirrorAroundPoint(Vector2 point, bool flip) {
            int l = (int)mPoints.Count;
            if (flip)
                for (int i = l - 1; i >= 0; i--) {
                    Vector2 pos = mPoints[i] - point;
                    mPoints.Add(-1.0f * pos + point);
                }
            else
                for (int i = 0; i < l; i++) {
                    Vector2 pos = mPoints[i] - point;
                    mPoints.Add(-1.0f * pos + point);
                }
            return this;
        }

        //    *
        //	 * Create a symetric copy at a given axis.
        //	 * @param axis Axis where to mirror
        //	 * @param flip \c true if function should start mirroring with the first point in list (default \c false)
        //	 
        public Shape mirrorAroundAxis(Vector2 axis) {
            return mirrorAroundAxis(axis, false);
        }
       //
        //ORIGINAL LINE: Shape& mirrorAroundAxis(const Ogre::Vector2& axis, bool flip = false)
        public Shape mirrorAroundAxis(Vector2 axis, bool flip) {
            int l = (int)mPoints.Count;
            Vector2 normal = axis.Perpendicular.NormalisedCopy;
            if (flip)
                for (int i = 0; i < l; i++) {
                    Vector2 pos = mPoints[i];
                    pos = pos.Reflect(normal);
                    if (pos != mPoints[i])
                        mPoints.Add(pos);
                }
            else
                for (int i = l - 1; i >= 0; i--) {
                    Vector2 pos = mPoints[i];
                    pos = pos.Reflect(normal);
                    if (pos != mPoints[i])
                        mPoints.Add(pos);
                }
            return this;
        }

        /// Returns the total lineic length of that shape
        //
        //ORIGINAL LINE: Ogre::float getTotalLength() const
        public float getTotalLength() {
            float length = 0;
            for (int i = 0; i < mPoints.Count - 1; i++)
                length += (mPoints[i + 1] - mPoints[i]).Length;
            if (mClosed)
                length += (mPoints[mPoints.Count - 1] - mPoints[0]).Length;
            return length;
        }

        /// Gets a position on the shape with index of the point and a percentage of position on the segment
        /// @param i index of the segment
        /// @param coord a number between 0 and 1 meaning the percentage of position on the segment
        /// @exception Ogre::InvalidParametersException i is out of bounds
        /// @exception Ogre::InvalidParametersException coord must be comprised between 0 and 1
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getPosition(uint i, Ogre::float coord) const
        public Vector2 getPosition(uint i, float coord) {
            if (!mClosed || i >= mPoints.Count)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Out of Bounds", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Out of Bounds");
            ;
            if (coord < 0.0f || coord > 1.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Coord must be comprised between 0 and 1", "Procedural::Path::getPosition(unsigned int, Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Coord must be comprised between 0 and 1");
            ;
            Vector2 A = getPoint((int)i);
            Vector2 B = getPoint((int)i + 1);
            return A + coord * (B - A);
        }

        /// Gets a position on the shape from lineic coordinate
        /// @param coord lineic coordinate
        /// @exception Ogre::InvalidStateException The shape must at least contain 2 points
        //
        //ORIGINAL LINE: inline Ogre::Vector2 getPosition(Ogre::float coord) const
        public Vector2 getPosition(float coord) {
            if (mPoints.Count < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The shape must at least contain 2 points", "Procedural::Shape::getPosition(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("shape must at least contain 2 points");
            ;
            int i = 0;
            while (true) {
                float nextLen = (getPoint(i + 1) - getPoint(i)).Length;
                if (coord > nextLen)
                    coord -= nextLen;
                else
                    return getPosition((uint)i, coord);
                if (!mClosed && i >= mPoints.Count - 2)
                    return mPoints[mPoints.Count - 1];
                i++;
            }
        }

        /// Computes the radius of a bounding circle centered on the origin
        //
        //ORIGINAL LINE: Ogre::float findBoundingRadius() const
        public float findBoundingRadius() {
            float sqRadius = 0.0f;
            for (int i = 0; i < mPoints.Count; i++)
                sqRadius = System.Math.Max(sqRadius, mPoints[i].SquaredLength);
            return Math.Sqrt(sqRadius);
        }

        //    *
        //	 * Applies a "thickness" to a shape, ie a bit like the extruder, but in 2D
        //	 * <table border="0" width="100%"><tr><td>\image html shape_thick1.png "Start shape (before thicken)"</td><td>\image html shape_thick2.png "Result (after thicken)"</td></tr></table>
        //	 
        //-----------------------------------------------------------------------
        public MultiShape thicken(float amount) {
            if (!mClosed) {
                Shape s = new Shape();
                s.setOutSide(mOutSide);
                for (int i = 0; i < mPoints.Count; i++)
                    s.addPoint(mPoints[i] + amount * getAvgNormal((uint)i));
                for (int i = mPoints.Count - 1; i >= 0; i--)
                    s.addPoint(mPoints[i] - amount * getAvgNormal((uint)i));
                s.close();
                return new MultiShape().addShape(s);
            }
            else {
                MultiShape ms = new MultiShape();
                Shape s1 = new Shape();
                for (int i = 0; i < mPoints.Count; i++)
                    s1.addPoint(mPoints[i] + amount * getAvgNormal((uint)i));
                s1.close();
                s1.setOutSide(mOutSide);
                ms.addShape(s1);
                Shape s2 = new Shape();
                for (int i = 0; i < mPoints.Count; i++)
                    s2.addPoint(mPoints[i] - amount * getAvgNormal((uint)i));
                s2.close();
                s2.setOutSide(mOutSide == Side.SIDE_LEFT ? Side.SIDE_RIGHT : Side.SIDE_LEFT);
                ms.addShape(s2);
                return ms;
            }
        }




        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: MultiShape _booleanOperation(const Shape& STLAllocator<U, AllocPolicy>, BooleanOperationType opType) const
        private MultiShape _booleanOperation(Shape other, BooleanOperationType opType) {
            if (!mClosed || mPoints.Count < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Current shapes must be closed and has to contain at least 2 points!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
                throw new Exception("shape must at least contain 2 points");
            ;
            if (!other.mClosed || other.mPoints.Count < 2)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Other shapes must be closed and has to contain at least 2 points!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
                throw new Exception("Other shapes must be closed and has to contain at least 2 points!");
            ;

            // Compute the intersection between the 2 shapes
            List<IntersectionInShape> intersections = new List<IntersectionInShape>();
            _findAllIntersections(other, ref intersections);

            // Build the resulting shape
            if (intersections.Count == 0) {
                if (isPointInside(other.getPoint(0))) {
                    // Shape B is completely inside shape A
                    if (opType == BooleanOperationType.BOT_UNION) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(this);
                        return ms;
                    }
                    else if (opType == BooleanOperationType.BOT_INTERSECTION) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(other);
                        return ms;
                    }
                    else if (opType == BooleanOperationType.BOT_DIFFERENCE) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(this);
                        ms.addShape(other);
                        ms.getShape(1).switchSide();
                        return ms;
                    }

                }
                else if (other.isPointInside(getPoint(0))) {
                    // Shape A is completely inside shape B
                    if (opType == BooleanOperationType.BOT_UNION) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(other);
                        return ms;
                    }
                    else if (opType == BooleanOperationType.BOT_INTERSECTION) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(this);
                        return ms;
                    }
                    else if (opType == BooleanOperationType.BOT_DIFFERENCE) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(this);
                        ms.addShape(other);
                        ms.getShape(0).switchSide();
                        return ms;
                    }
                }
                else {
                    if (opType == BooleanOperationType.BOT_UNION) {
                        MultiShape ms = new MultiShape();
                        ms.addShape(this);
                        ms.addShape(other);
                        return ms;
                    }
                    else if (opType == BooleanOperationType.BOT_INTERSECTION)
                        return new MultiShape(); //empty result
                    else if (opType == BooleanOperationType.BOT_DIFFERENCE)
                        return new MultiShape(); //empty result
                }
            }
            MultiShape outputMultiShape = new MultiShape();

            Shape[] inputShapes = new Shape[2];
            inputShapes[0] = this;
            inputShapes[1] = other;

            while (intersections.Count != 0) {
                Shape outputShape = new Shape();
                byte shapeSelector = 0; // 0 : first shape, 1 : second shape

                Vector2 currentPosition = intersections[0].position;//intersections.GetEnumerator().position;
                IntersectionInShape firstIntersection = intersections[0];//*intersections.GetEnumerator();
                uint currentSegment = firstIntersection.index[shapeSelector];
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
                //intersections.erase(intersections.GetEnumerator());//“∆≥˝
                intersections.Remove(firstIntersection);
                outputShape.addPoint(currentPosition);

                sbyte isIncreasing = 0; // +1 if increasing, -1 if decreasing, 0 if undefined

                if (!_findWhereToGo(inputShapes, opType, firstIntersection, ref shapeSelector, ref isIncreasing, ref currentSegment)) {
                    // That intersection is located on a place where the resulting shape won't go => discard
                    continue;
                }

                while (true) {
                    // find the closest intersection on the same segment, in the correct direction
                    //List<IntersectionInShape>.Enumerator found_next_intersection = intersections.end();
                    IntersectionInShape found_next_intersection = intersections[intersections.Count - 1];
                    float distanceToNextIntersection = float.MaxValue;// std.numeric_limits<Real>.max();

                    uint nextPoint = currentSegment + (uint)(isIncreasing == 1 ? 1 : 0);
                    bool nextPointIsOnIntersection = false;

                    //for (List<IntersectionInShape>.Enumerator it = intersections.GetEnumerator(); it.MoveNext(); ++it)
                    for (int i = 0; i < intersections.Count; i++) {
                        IntersectionInShape it = intersections[i];
                        if (currentSegment == it.index[shapeSelector]) {
                            if (((it.position - currentPosition).DotProduct(it.position - inputShapes[shapeSelector].getPoint((int)nextPoint)) < 0) || (it.onVertex[shapeSelector] && nextPoint == it.index[shapeSelector])) {
                                // found an intersection between the current one and the next segment point
                                float d = (it.position - currentPosition).Length;
                                if (d < distanceToNextIntersection) {
                                    // check if we have the nearest intersection
                                    found_next_intersection = it;
                                    distanceToNextIntersection = d;
                                }
                            }
                        }
                        if (nextPoint == it.index[shapeSelector] && it.onVertex[shapeSelector])
                            nextPointIsOnIntersection = true;
                    }

                    // stop condition
                    if (currentSegment == firstIntersection.index[shapeSelector]) {
                        // we found ourselves on the same segment as the first intersection and no other
                        if ((firstIntersection.position - currentPosition).DotProduct(firstIntersection.position - inputShapes[shapeSelector].getPoint((int)nextPoint)) < 0f) {
                            float d = (firstIntersection.position - currentPosition).Length;
                            if (d > 0.0f && d < distanceToNextIntersection) {
                                outputShape.close();
                                break;
                            }
                        }
                    }

                    // We actually found the next intersection => change direction and add current intersection to the list
                    //if (found_next_intersection.MoveNext())
                    if (intersections.Count > 1) {
                        //IntersectionInShape currentIntersection = found_next_intersection.Current;
                        //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
                        //intersections.erase(found_next_intersection);
                        intersections.Remove(found_next_intersection);
                        IntersectionInShape currentIntersection = intersections[intersections.Count - 1];
                        outputShape.addPoint(currentIntersection.position);
                        bool result = _findWhereToGo(inputShapes, opType, currentIntersection, ref shapeSelector, ref isIncreasing, ref currentSegment);
                        if (result == null) {
                            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                            //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                            //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "We should not be here!", "Procedural::Shape::_booleanOperation(const Procedural::Shape&, Procedural::BooleanOperationType)", __FILE__, __LINE__);
                            throw new Exception("We should not be here!");
                            ;
                        }
                    }
                    else {
                        // no intersection found for the moment => just continue on the current segment
                        if (!nextPointIsOnIntersection) {
                            if (isIncreasing == 1)
                                currentPosition = inputShapes[shapeSelector].getPoint((int)currentSegment + 1);
                            else
                                currentPosition = inputShapes[shapeSelector].getPoint((int)currentSegment);

                            outputShape.addPoint(currentPosition);
                        }
                        currentSegment = (uint)Utils.modulo((int)currentSegment + isIncreasing, inputShapes[shapeSelector].getSegCount());
                    }
                }

                outputMultiShape.addShape(outputShape);
            }
            return outputMultiShape;
        }



        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool _isLookingForOutside(BooleanOperationType opType, sbyte shapeSelector) const
        private bool _isLookingForOutside(BooleanOperationType opType, sbyte shapeSelector) {
            switch (opType) {
                case BooleanOperationType.BOT_UNION:
                    return true;
                case BooleanOperationType.BOT_INTERSECTION:
                    return false;
                case BooleanOperationType.BOT_DIFFERENCE:
                    if (shapeSelector == 0)
                        return true;
                    return false;
                default:
                    return true;
            }
        }

        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: sbyte _isIncreasing(float d, BooleanOperationType opType, sbyte shapeSelector) const
        private sbyte _isIncreasing(float d, BooleanOperationType opType, sbyte shapeSelector) {
            if (d < 0f && opType == BooleanOperationType.BOT_UNION)
                return -1;
            if (d > 0f && opType == BooleanOperationType.BOT_INTERSECTION)
                return -1;
            if (opType == BooleanOperationType.BOT_DIFFERENCE) {
                if ((d < 0f && shapeSelector == 0) || (d > 0f && shapeSelector == 1))
                    return -1;
            }
            return 1;
        }

        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: bool _findWhereToGo(const Shape* inputShapes[], BooleanOperationType opType, IntersectionInShape intersection, byte& shapeSelector, sbyte& isIncreasing, uint& currentSegment) const
        private bool _findWhereToGo(Shape[] inputShapes, BooleanOperationType opType, IntersectionInShape intersection, ref byte shapeSelector, ref sbyte isIncreasing, ref uint currentSegment) {
            if (intersection.onVertex[0] || intersection.onVertex[1]) {
                // determine 4 directions with normal info
                // if 2 normals "face each other" then you have the couple of outside directions
                Vector2[] directions = new Vector2[4];
                //string sides = new string(new char[4]);
                byte[] sides = new byte[4];
                byte incomingDirection;

                // fill-in the incoming arrays
                if (isIncreasing == 0) {
                    incomingDirection = 255;
                }
                else {
                    incomingDirection = (byte)(shapeSelector + (isIncreasing == 1 ? 2 : 0));
                }
                for (byte i = 0; i < 2; i++)
                    if (intersection.onVertex[i]) {
                        directions[i] = inputShapes[i].getDirectionBefore(intersection.index[i]);
                        directions[2 + i] = -inputShapes[i].getDirectionAfter(intersection.index[i]);
                    }
                    else {
                        directions[2 + i] = -inputShapes[i].getDirectionAfter(intersection.index[i]);
                        directions[i] = -directions[2 + i];
                    }
                for (byte i = 0; i < 4; i++) {
                    sides[i] = (byte)((i / 2 == 0 ? -1 : 1) * (inputShapes[i % 2].mOutSide == Side.SIDE_RIGHT ? -1 : 1));
                }

                bool[] isOutside = new bool[4];
                //std.pair<Radian, byte>[] sortedDirections = new std.pair[4];
                KeyValuePair<Radian, byte>[] sortedDirections = new KeyValuePair<Radian, byte>[4];
                // sort by angle
                for (byte i = 0; i < 4; i++) {
                    if (i == 0) {
                        //sortedDirections[i].first = 0;
                        sortedDirections[i] = new KeyValuePair<Radian, byte>(0, i);
                    }
                    else {
                        Radian first = sides[0] * Utils.angleTo(directions[0], directions[i]);
                        sortedDirections[i] = new KeyValuePair<Radian, byte>(first, i);
                    }
                    //sortedDirections[i].second=i;
                }

                //std.sort(sortedDirections, sortedDirections+4, GlobalMembersProceduralShape._sortAngles);
                //ToDo:sortedDirections≈≈–Ú
                List<KeyValuePair<Radian, byte>> sort_sortedDirections = new List<KeyValuePair<Radian, byte>>();
                sort_sortedDirections.AddRange(sortedDirections);
                sort_sortedDirections.Sort((X, Y) => {
                    return _sortAngles(X, Y) ? -1 : 1;
                });
                sortedDirections = sort_sortedDirections.ToArray();
                //find which segments are outside
                if (sides[0] != sides[sortedDirections[1].Value]) {
                    isOutside[0] = isOutside[sortedDirections[1].Value] = true;
                    isOutside[sortedDirections[2].Value] = isOutside[sortedDirections[3].Value] = false;
                }
                else {
                    isOutside[sortedDirections[1].Value] = isOutside[sortedDirections[2].Value] = true;
                    isOutside[sortedDirections[3].Value] = isOutside[sortedDirections[0].Value] = false;
                }

                //find first eligible segment that is not the current segment
                for (ushort i = 0; i < 4; i++)
                    if ((isOutside[i] == _isLookingForOutside(opType, (sbyte)(i % 2))) && (i != incomingDirection)) {
                        shapeSelector = (byte)(i % 2);
                        isIncreasing = (sbyte)(i / 2 == 0 ? 1 : -1);
                        currentSegment = intersection.index[shapeSelector];
                        return true;
                    }
                // if we reach here, it means that no segment is eligible! (it should only happen with difference opereation
                return false;
            }
            else {
                // determine which way to go
                int nextShapeSelector = (shapeSelector + 1) % 2;

                float d = inputShapes[nextShapeSelector].getDirectionAfter(intersection.index[nextShapeSelector]).DotProduct(inputShapes[shapeSelector].getNormalAfter(currentSegment));
                isIncreasing = _isIncreasing(d, opType, (sbyte)nextShapeSelector);

                shapeSelector = (byte)nextShapeSelector;

                currentSegment = intersection.index[shapeSelector];
                return true;
            }
        }

        //-----------------------------------------------------------------------
        //
        //ORIGINAL LINE: void _findAllIntersections(const Shape& STLAllocator<U, AllocPolicy>, List<IntersectionInShape>& intersections) const
        private void _findAllIntersections(Shape other, ref List<IntersectionInShape> intersections) {
            for (ushort i = 0; i < getSegCount(); i++) {
                Segment2D seg1 = new Segment2D(getPoint(i), getPoint(i + 1));

                for (ushort j = 0; j < other.getSegCount(); j++) {
                    Segment2D seg2 = new Segment2D(other.getPoint(j), other.getPoint(j + 1));

                    Vector2 intersect = new Vector2();
                    if (seg1.findIntersect(seg2, ref intersect)) {
                        IntersectionInShape inter = new IntersectionInShape(i, j, intersect);
                        // check if intersection is "borderline" : too near to a vertex
                        if ((seg1.mA - intersect).SquaredLength < 1e-8) {
                            inter.onVertex[0] = true;
                        }
                        if ((seg1.mB - intersect).SquaredLength < 1e-8) {
                            inter.onVertex[0] = true;
                            inter.index[0]++;
                        }
                        if ((seg2.mA - intersect).SquaredLength < 1e-8) {
                            inter.onVertex[1] = true;
                        }
                        if ((seg2.mB - intersect).SquaredLength < 1e-8) {
                            inter.onVertex[1] = true;
                            inter.index[1]++;
                        }

                        intersections.Add(inter);
                    }
                }
            }
        }

    }

}