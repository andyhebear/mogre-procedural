
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

//#ifndef PROCEDURAL_MULTISHAPE_INCLUDED
#define PROCEDURAL_MULTISHAPE_INCLUDED
//use new std overwrite....ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //class Shape;

    //*
    // * \ingroup shapegrp
    // * Holds a bunch of shapes.
    // * There are a number of assumptions that are made and are not checked
    // * against : the shapes must not cross each other
    // *
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport MultiShape
    public class MultiShape
    {
        private std_vector<Shape> mShapes = new std_vector<Shape>();

        /// Default constructor
        public MultiShape() {
        }

        /// Constructor from a single shape
        public MultiShape(Shape shape) {
            mShapes.push_back(shape);
        }

        /// Constructor from a variable number of shapes
        /// @param count the number of shapes to add
        /// @param ... pointer to the shapes to add
        //-----------------------------------------------------------------------

        public MultiShape(int count, Shape[] ParamArray) {
            //		va_list shapes;
            int ParamCount = -1;
            //		va_start(shapes, count);
            for (int i = 0; i < count; i++) {
                ParamCount++;
                //mShapes.Add(*ParamArray[ParamCount]);
                mShapes.push_back(ParamArray[ParamCount]);
            }

            //		va_end(shapes);
        }

        /// Adds a shape to the list of shapes
        public MultiShape addShape(Shape shape) {
            mShapes.push_back(shape);
            return this;
        }

        /// Clears all the content
        public void clear() {
            mShapes.clear();
        }

        /// Returns the i-th shape
        //
        //ORIGINAL LINE: const Shape& getShape(uint i) const
        public Shape getShape(uint i) {
            return mShapes[(int)i];
        }

        /// Builds an aggregated list of all points contained in all shapes
        //-----------------------------------------------------------------------

        //
        //ORIGINAL LINE: List<Vector2> getPoints() const
        public std_vector<Vector2> getPoints() {
            std_vector<Vector2> result = new std_vector<Vector2>();
            for (int i = 0; i < mShapes.Count; i++) {
                Vector2[] points = mShapes[i].getPoints();
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'insert' method in C#:
                //result.insert(result.end(), points.GetEnumerator(), points.end());
                result.AddRange(points);
            }
            return result;
        }

        /// Returns the number of shapes in that MultiShape
        //
        //ORIGINAL LINE: uint getShapeCount() const
        public int getShapeCount() {
            return mShapes.size();
        }

        /// Append every shape of an other multishape to the current multiShape
        public void addMultiShape(MultiShape other) {
            //for (List<Shape>.Enumerator it = STLAllocator<U, AllocPolicy>.mShapes.GetEnumerator(); it!=STLAllocator<U, AllocPolicy>.mShapes.end(); ++it)
            foreach (var it in other.mShapes) {
                mShapes.push_back(it);
            }
        }

        /// Outputs the Multi Shape to a Mesh, mostly for visualisation or debugging purposes
        //-----------------------------------------------------------------------

        public MeshPtr realizeMesh() {
            return realizeMesh("");
        }
       //
        //ORIGINAL LINE: MeshPtr realizeMesh(const string& name ="")
        public MeshPtr realizeMesh(string name) {
            Mogre.SceneManager smgr = Root.Singleton.GetSceneManagerIterator().Current;
            ManualObject manual = smgr.CreateManualObject(name);

            //for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mShapes) {
                manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_STRIP);
                it._appendToManualObject(manual);
                manual.End();
            }

            MeshPtr mesh = null;// MeshManager.Singleton.CreateManual(Guid.NewGuid().ToString("N"), "General"); //new MeshPtr();
            if (name == "")
                mesh = manual.ConvertToMesh(Utils.getName("mutishape_procedural_"));
            else
                mesh = manual.ConvertToMesh(name);
            smgr.DestroyManualObject(manual);
            return mesh;
        }

        /// Tells whether a point is located inside that multishape
        /// It assumes that all of the shapes in that multishape are closed,
        /// and that they don't contradict each other,
        /// ie a point cannot be outside and inside at the same time
        //
        //ORIGINAL LINE: bool isPointInside(const Ogre::Vector2& point) const;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
        //	bool isPointInside(Ogre::Vector2 point);
        public bool isPointInside(Vector2 point) {
            // Draw a horizontal lines that goes through "point"
            // Using the closest intersection, find whether the point is actually inside
            int closestSegmentIndex = -1;
            float closestSegmentDistance = float.MaxValue; //std.numeric_limits<float>.max();
            Vector2 closestSegmentIntersection = new Vector2();
            Shape closestSegmentShape = null;

            for (int k = 0; k < mShapes.size(); k++) {
                Shape shape = mShapes[k];
                for (int i = 0; i < shape.getSegCount(); i++) {
                    Vector2 A = shape.getPoint(i);
                    Vector2 B = shape.getPoint(i + 1);
                    if (A.y != B.y && (A.y - point.y) * (B.y - point.y) <= 0.0f) {
                        Vector2 intersect = new Vector2(A.x + (point.y - A.y) * (B.x - A.x) / (B.y - A.y), point.y);
                        float dist = Math.Abs(point.x - intersect.x);
                        if (dist < closestSegmentDistance) {
                            closestSegmentIndex = i;
                            closestSegmentDistance = dist;
                            //
                            //ORIGINAL LINE: closestSegmentIntersection = intersect;
                            closestSegmentIntersection = (intersect);
                            closestSegmentShape = shape;
                        }
                    }
                }
            }
            if (closestSegmentIndex != -1) {
                int edgePoint = -1;
                if ((closestSegmentIntersection - closestSegmentShape.getPoint(closestSegmentIndex)).SquaredLength < 1e-8)
                    edgePoint = closestSegmentIndex;
                else if ((closestSegmentIntersection - closestSegmentShape.getPoint(closestSegmentIndex + 1)).SquaredLength < 1e-8)
                    edgePoint = closestSegmentIndex + 1;
                if (edgePoint > -1) {
                    Radian alpha1 = Utils.angleBetween(point - closestSegmentShape.getPoint(edgePoint), closestSegmentShape.getDirectionAfter((uint)edgePoint));
                    Radian alpha2 = Utils.angleBetween(point - closestSegmentShape.getPoint(edgePoint), -closestSegmentShape.getDirectionBefore((uint)edgePoint));
                    if (alpha1 < alpha2)
                        closestSegmentIndex = edgePoint;
                    else
                        closestSegmentIndex = edgePoint - 1;
                }
                return (closestSegmentShape.getNormalAfter((uint)closestSegmentIndex).x * (point.x - closestSegmentIntersection.x) < 0f);
            }
            // We're in the case where the point is on the "float outside" of the multishape
            // So, if the float outside == user defined outside, then the point is "user-defined outside"
            return !isOutsideRealOutside();
        }
        //    *
        //	 * Tells whether multishape is "closed" or not.
        //	 * MultiShape is considered to be closed if and only if all shapes are closed
        //	 
        //-----------------------------------------------------------------------

        //
        //ORIGINAL LINE: bool isClosed() const
        public bool isClosed() {
            //for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mShapes) {
                if (!it.isClosed())
                    return false;
            }
            return true;
        }

        //    *
        //	 * Closes all shapes included in this multiShape
        //	 
        //-----------------------------------------------------------------------

        public void close() {
            //for (List<Shape>.Enumerator it = mShapes.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in mShapes) {
                it.close();
            }
        }

        //    *
        //	 * Determines whether the outside as defined by user equals "real" outside
        //	 

        //-----------------------------------------------------------------------

        //
        //ORIGINAL LINE: bool isOutsideRealOutside() const
        public bool isOutsideRealOutside() {
            float x = float.MinValue;//std.numeric_limits<float>.min();
            int index = 0;
            int shapeIndex = 0;
            for (int j = 0; j < mShapes.size(); j++) {
                Shape s = mShapes[j];
                std_vector<Vector2> points = s.getPointsReference();
                for (int i = 0; i < points.Count; i++) {
                    if (x < points[i].x) {
                        x = points[i].x;
                        index = i;
                        shapeIndex = j;
                    }
                }
            }
            Radian alpha1 = Utils.angleTo(Vector2.UNIT_Y, mShapes[shapeIndex].getDirectionAfter((uint)index));
            Radian alpha2 = Utils.angleTo(Vector2.UNIT_Y, -mShapes[shapeIndex].getDirectionBefore((uint)index));
            Side shapeSide;
            if (alpha1 < alpha2)
                shapeSide = Side.SIDE_RIGHT;
            else
                shapeSide = Side.SIDE_LEFT;
            return shapeSide == mShapes[shapeIndex].getOutSide();
        }

        //-----------------------------------------------------------------------
        public void buildFromSegmentSoup(std_vector<Segment2D> segList) {
            //std.multimap<Vector2, Vector2, Vector2Comparator> segs = new std.multimap<Vector2, Vector2, Vector2Comparator>();
            std_multimap<Vector2, Vector2> segs = new std_multimap<Vector2, Vector2>();
            // for (List<Segment2D>.Enumerator it = segList.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in segList) {
                //segs.insert(std.pair<Vector2, Vector2 > (it.mA, it.mB));
                //segs.insert(std.pair<Vector2, Vector2 > (it.mB, it.mA));
                segs.insert(it.mA, it.mB);
                segs.insert(it.mB, it.mA);
            }
            while (!segs.empty()) {
                Vector2 headFirst = segs.get(0).first;//segs.begin().first;
                Vector2 headSecond = segs.get(0).second[0]; //segs.begin().second;
                Shape s = new Shape();
                s.addPoint(headFirst).addPoint(headSecond);
                //std_multimap<Vector2, Vector2, Vector2Comparator>.Enumerator firstSeg = segs.begin();
                int firstSeg_pos = segs.begin();
                std_pair<Vector2, List<Vector2>> firstSeg = segs.get(0);
                // std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants2 = segs.equal_range(headSecond);
                std_pair<std_pair<Vector2, List<Vector2>>, std_pair<Vector2, List<Vector2>>> correspondants2 = segs.equal_range(headSecond);
                //for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants2.first; it != correspondants2.second;)
                //{
                //    std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
                //    if ((removeIt.second - firstSeg.first).squaredLength() < 1e-8)
                //        segs.erase(removeIt);
                //}
                for (int i = 1; i < correspondants2.first.second.Count; i++) {
                    Vector2 removeIt = correspondants2.first.second[i];
                    if ((removeIt - firstSeg.first).SquaredLength < 1e-8) {
                        segs.erase(removeIt);
                    }
                }
                segs.erase(firstSeg.first);
                bool foundSomething = true;
                while (!segs.empty() && foundSomething) {
                    foundSomething = false;
                    //std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator next = segs.find(headSecond);
                    int next_pos = segs.find(headSecond);
                    // if (next != segs.end())
                    if (next_pos != -1) {
                        std_pair<Vector2, List<Vector2>> next = segs.get((uint)next_pos);
                        foundSomething = true;
                        //headSecond = next.second;
                        headSecond = next.second[0];
                        s.addPoint(headSecond);
                        //std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants = segs.equal_range(headSecond);
                        std_pair<std_pair<Vector2, List<Vector2>>, std_pair<Vector2, List<Vector2>>> correspondants = segs.equal_range(headSecond);
                        //for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants.first; it != correspondants.second;)
                        //{
                        //    std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
                        //    if ((removeIt.second - next.first).squaredLength() < 1e-8)
                        //        segs.erase(removeIt);
                        //}
                        for (int j = 1; j < correspondants.first.second.Count; j++) {
                            Vector2 removeIt = correspondants2.first.second[j];
                            if ((removeIt - next.first).SquaredLength < 1e-8) {
                                segs.erase(removeIt);
                            }
                        }
                        //segs.erase(next);
                        segs.erase(next.first);
                    }
                    //std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator previous = segs.find(headFirst);
                    int previous_pos = segs.find(headFirst);
                    //if (previous != segs.end()) {
                    if (previous_pos != -1) {
                        foundSomething = true;
                        std_pair<Vector2, List<Vector2>> previous = segs.get((uint)previous_pos);
                        // s.insertPoint(0, previous.second);
                        s.insertPoint(0, previous.second[0]);
                        //headFirst = previous.second;
                        headFirst = previous.second[0];
                        //std.pair<std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator, std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator> correspondants = segs.equal_range(headFirst);
                        std_pair<std_pair<Vector2, List<Vector2>>, std_pair<Vector2, List<Vector2>>> correspondants = segs.equal_range(headFirst);
                        //for (std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator it = correspondants.first; it != correspondants.second; ) {
                        for (int j = 1; j < correspondants.first.second.Count; j++) {
                            //std.multimap<Vector2, Vector2, Vector2Comparator>.Enumerator removeIt = ++it;
                            //if ((removeIt.second - previous.first).squaredLength() < 1e-8)
                            //    segs.erase(removeIt);
                            Vector2 removeIt = correspondants.first.second[j];
                            if ((removeIt - previous.first).SquaredLength < 1e-8) {
                                segs.erase(removeIt);
                            }
                        }
                        //segs.erase(previous);
                        segs.erase(previous.first);
                    }
                }
                if ((s.getPoint(0) - s.getPoint(s.getSegCount() + 1)).SquaredLength < 1e-6) {
                    s.getPointsReference().pop_back();
                    s.close();
                }
                addShape(s);
            }
        }

    }
}
