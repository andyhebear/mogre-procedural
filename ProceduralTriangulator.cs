

//#ifndef PROCEDURAL_TRIANGULATOR_INCLUDED
#define PROCEDURAL_TRIANGULATOR_INCLUDED

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

//use new std wrapper...ok

namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;

    using Mogre_Procedural.std;
    using PointList = Mogre_Procedural.std.std_vector<Mogre.Vector2>;
    using DelaunayTriangleBuffer = Mogre_Procedural.std.std_list<Mogre_Procedural.Triangulator.Triangle>;
    //C++ TO C# CONVERTER NOTE: C# has no need of forward class declarations:
    //struct Triangle2D;

    //#define PointList_AlternateDefinition1

    //*
    // * Implements a Delaunay Triangulation algorithm.
    // * It works on Shapes to build Triangle Buffers
    // * \image html shape_triangulation.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Triangulator : public MeshGenerator<Triangulator>
    public class Triangulator : MeshGenerator<Triangulator>
    {
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following type could not be found.
        //	struct Triangle;
        //C++ TO C# CONVERTER TODO TASK: The implementation of the following type could not be found.
        //	struct DelaunaySegment;

        //#define DelaunayTriangleBuffer_AlternateDefinition1
        //-----------------------------------------------------------------------
        private class DelaunaySegment
        {
            public int i1;
            public int i2;
            public DelaunaySegment(int _i1, int _i2) {
                i1 = _i1;
                i2 = _i2;
            }
            //
            //ORIGINAL LINE: bool operator <(const DelaunaySegment& STLAllocator<U, AllocPolicy>) const
            public static bool operator <(DelaunaySegment This, DelaunaySegment Other) {
                if (This.i1 != Other.i1)
                    return This.i1 < Other.i1;
                else
                    return This.i2 < Other.i2;
            }
            public static bool operator >(DelaunaySegment This, DelaunaySegment Other) {
                if (This.i1 != Other.i1)
                    return This.i1 > Other.i1;
                else
                    return This.i2 > Other.i2;
            }
            public DelaunaySegment inverse() {
                return new DelaunaySegment(i2, i1);
            }
        }

        //-----------------------------------------------------------------------
        internal class Triangle
        {
            public readonly PointList pl;
            public int[] i = new int[3];
            public Triangle(PointList pl) {
                this.pl = pl;
            }

            //
            //ORIGINAL LINE: inline Ogre::Vector2 p(int k) const
            public Vector2 p(int k) {
                return (pl)[i[k]];
            }

            //
            //ORIGINAL LINE: bool operator ==(const Triangle& STLAllocator<U, AllocPolicy>) const
            public static bool operator ==(Triangle This, Triangle Other) {
                return This.i[0] == Other.i[0] && This.i[1] == Other.i[1] && This.i[2] == Other.i[2];
            }
            public static bool operator !=(Triangle This, Triangle Other) {
                return This.i[0] != Other.i[0] || This.i[1] != Other.i[1] || This.i[2] != Other.i[2];
            }
            //
            //ORIGINAL LINE: inline Ogre::Vector2 getMidPoint() const
            public Vector2 getMidPoint() {
                return 1.0f / 3.0f * (p(0) + p(1) + p(2));
            }

            //-----------------------------------------------------------------------
            public void setVertices(int i0, int i1, int i2) {
                i[0] = i0;
                i[1] = i1;
                i[2] = i2;
            }

            //-----------------------------------------------------------------------
            //
            //ORIGINAL LINE: int findSegNumber(int i0, int i1) const
            public int findSegNumber(int i0, int i1) {
                if ((i0 == i[0] && i1 == i[1]) || (i0 == i[1] && i1 == i[0]))
                    return 2;
                if ((i0 == i[1] && i1 == i[2]) || (i0 == i[2] && i1 == i[1]))
                    return 0;
                if ((i0 == i[2] && i1 == i[0]) || (i0 == i[0] && i1 == i[2]))
                    return 1;
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INTERNAL_ERROR>(), "We should not be here!", "Procedural::Triangulator::Triangle::findSegNumber(int i0, int i1)", __FILE__, __LINE__);
                throw new Exception("We should not be here!");
                ;
                return -1;
            }

            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //		bool isPointInside(Ogre::Vector2 point);
            public bool isPointInside(Vector2 point) {
                // Compute vectors
                Vector2 v0 = p(2) - p(0);
                Vector2 v1 = p(1) - p(0);
                Vector2 v2 = point - p(0);

                // Compute dot products
                Real dot00 = v0.SquaredLength;
                Real dot01 = v0.DotProduct(v1);
                Real dot02 = v0.DotProduct(v2);
                Real dot11 = v1.SquaredLength;
                Real dot12 = v1.DotProduct(v2);

                // Compute barycentric coordinates
                Real invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
                Real u = (dot11 * dot02 - dot01 * dot12) * invDenom;
                Real v = (dot00 * dot12 - dot01 * dot02) * invDenom;

                // Check if point is in triangle
                return (u >= 0) && (v >= 0) && (u + v - 1 <= 0);
            }
            //
            //ORIGINAL LINE: bool containsSegment(int i0, int i1) const
            public bool containsSegment(int i0, int i1) {
                return ((i0 == i[0] || i0 == i[1] || i0 == i[2]) && (i1 == i[0] || i1 == i[1] || i1 == i[2]));
            }

            public enum InsideType : int
            {
                IT_INSIDE,
                IT_OUTSIDE,
                IT_BORDERLINEOUTSIDE
            }

            //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
            //		InsideType isPointInsideCircumcircle(Ogre::Vector2 point);
            public InsideType isPointInsideCircumcircle(Vector2 pt) {
                Vector2 v0 = p(0);
                Vector2 v1 = p(1);
                Vector2 v2 = p(2);
                Matrix4 m = new Matrix4(v0.x, v0.y, v0.SquaredLength, 1.0f,
                           v1.x, v1.y, v1.SquaredLength, 1.0f,
                           v2.x, v2.y, v2.SquaredLength, 1.0f,
                           pt.x, pt.y, pt.SquaredLength, 1.0f);
                Real det = m.Determinant;
                if (det >= 0)
                    return InsideType.IT_INSIDE;
                if (det > -1e-3)
                    return InsideType.IT_BORDERLINEOUTSIDE;
                return InsideType.IT_OUTSIDE;
            }
            public void makeDirectIfNeeded() {
                if ((p(1) - p(0)).CrossProduct(p(2) - p(0)) < 0f) {
                    std_swap<int>(i, 0, 1);
                }
            }

            private void std_swap<T>(T[] array, int index1, int index2) {
                T temp = array[index1];
                array[index1] = array[index2];
                array[index2] = temp;
            }

            public bool isDegenerate() {
                if (Math.Abs((p(1) - p(0)).CrossProduct(p(2) - p(0))) < 1e-4)
                    return true;
                return false;
            }

            public string debugDescription() {
                return "(" + StringConverter_toString(i[0]) + "," + StringConverter_toString(i[1]) + "," + StringConverter_toString(i[2]) + ") <" + "(" + StringConverter_toString(p(0)) + "," + StringConverter_toString(p(1)) + "," + StringConverter_toString(p(2)) + ">";
            }

            private string StringConverter_toString(Vector2 vector2) {
                return string.Format("['{0}','{1}']", vector2.x, vector2.y);
            }

            private string StringConverter_toString(int p) {
                return p.ToString();
            }
        }
        //-----------------------------------------------------------------------
        private class TouchSuperTriangle
        {
            public int i0;
            public int i1;
            public int i2;
            public TouchSuperTriangle(int i, int j, int k) {
                i0 = i;
                i1 = j;
                i2 = k;
            }
            //C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
            public bool Operator(Triangulator.Triangle tri) {
                for (int k = 0; k < 3; k++)
                    if (tri.i[k] == i0 || tri.i[k] == i1 || tri.i[k] == i2)
                        return true;
                return false;
            }


        }



        private Shape mShapeToTriangulate;
        private MultiShape mMultiShapeToTriangulate;
        private Triangle2D mManualSuperTriangle;
        private std_vector<Segment2D> mSegmentListToTriangulate;
        private bool mRemoveOutside;

        //
        //ORIGINAL LINE: void delaunay(List<Ogre::Vector2>& pointList, LinkedList<Triangle>& tbuffer) const
        void delaunay(PointList pointList, ref DelaunayTriangleBuffer tbuffer) {
            // Compute super triangle or insert manual super triangle
            if (mManualSuperTriangle != null) {
                float maxTriangleSize = 0.0f;
                //for (PointList::iterator it = pointList.begin(); it!=pointList.end(); ++it)
                foreach (Vector2 it in pointList) {
                    maxTriangleSize = max(maxTriangleSize, Math.Abs(it.x));
                    maxTriangleSize = max(maxTriangleSize, Math.Abs(it.y));
                }
                pointList.push_back(new Vector2(-3f * maxTriangleSize, -3f * maxTriangleSize));
                pointList.push_back(new Vector2(3f * maxTriangleSize, -3f * maxTriangleSize));
                pointList.push_back(new Vector2(0.0f, 3 * maxTriangleSize));

                int maxTriangleIndex = pointList.size() - 3;
                Triangle superTriangle = new Triangle(pointList);
                superTriangle.i[0] = maxTriangleIndex;
                superTriangle.i[1] = maxTriangleIndex + 1;
                superTriangle.i[2] = maxTriangleIndex + 2;
                tbuffer.push_back(superTriangle);
            }

            // Point insertion loop
            for (int i = 0; i < pointList.size() - 3; i++) {
                //Utils::log("insert point " + StringConverter::toString(i));
                //std::list<std::list<Triangle>::iterator> borderlineTriangles;
                std_list<Triangle> borderlineTriangles = new std_list<Triangle>();
                // Insert 1 point, find all triangles for which the point is in circumcircle
                Vector2 p = pointList[i];
                //std::set<DelaunaySegment> segments;
                std_set<DelaunaySegment> segments = new std_set<DelaunaySegment>();
                IEnumerator<Triangle> et = tbuffer.GetEnumerator();
                //for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end();)
                List<Triangle> need_erase = new List<Triangle>();
                while (et.MoveNext()) {
                    Triangle it = et.Current;
                    Triangle.InsideType isInside = it.isPointInsideCircumcircle(p);
                    if (isInside == Triangle.InsideType.IT_INSIDE) {
                        if (!it.isDegenerate()) {
                            //Utils::log("tri insie" + it->debugDescription());
                            for (int k = 0; k < 3; k++) {
                                DelaunaySegment d1 = new DelaunaySegment(it.i[k], it.i[(k + 1) % 3]);
                                if (segments.find(d1) != segments.end())
                                    segments.erase(d1);
                                else if (segments.find(d1.inverse()) != segments.end())
                                    segments.erase(d1.inverse());
                                else
                                    segments.insert(d1);
                            }
                        }
                        //it=tbuffer.erase(it);
                        need_erase.Add(it);
                    }
                    else if (isInside == Triangle.InsideType.IT_BORDERLINEOUTSIDE) {
                        //Utils::log("tri borer " + it->debugDescription());
                        borderlineTriangles.push_back(it);
                        //++it;
                    }
                    else {
                        //++it;
                    }
                }
                //do delete
                foreach (var v in need_erase) {
                    tbuffer.Remove(v);
                }

                // Robustification of the standard algorithm : if one triangle's circumcircle was borderline against the new point,
                // test whether that triangle is intersected by new segments or not (normal situation : it should not)
                // If intersected, the triangle is considered having the new point in its circumc
                std_set<DelaunaySegment> copySegment = segments;
                IEnumerator<Triangle> be = borderlineTriangles.GetEnumerator();
                //for (std::list<std::list<Triangle>::iterator>::iterator itpTri = borderlineTriangles.begin(); itpTri!=borderlineTriangles.end(); itpTri++ )
                while (be.MoveNext()) {
                    Triangle itpTri = be.Current;
                    //DelaunayTriangleBuffer::iterator itTri = *itpTri;
                    Triangle itTri = itpTri;
                    bool triRemoved = false;
                    //for (std::set<DelaunaySegment>::iterator it = copySegment.begin(); it!=copySegment.end() && !triRemoved; ++it)
                    IEnumerator<DelaunaySegment> cse = copySegment.GetEnumerator();
                    while (cse.MoveNext() && !triRemoved) {
                        DelaunaySegment it = cse.Current;
                        bool isTriangleIntersected = false;
                        for (int k = 0; k < 2; k++) {
                            int i1 = (k == 0) ? it.i1 : it.i2;
                            int i2 = i;
                            for (int l = 0; l < 3; l++) {
                                //Early out if 2 points are in fact the same
                                if (itTri.i[l] == i1 || itTri.i[l] == i2 || itTri.i[(l + 1) % 3] == i1 || itTri.i[(l + 1) % 3] == i2)
                                    continue;
                                Segment2D seg2 = new Segment2D(itTri.p(l), itTri.p((l + 1) % 3));
                                Segment2D seg1 = new Segment2D(pointList[i1], pointList[i2]);
                                if (seg1.intersects(seg2)) {
                                    isTriangleIntersected = true;
                                    break;
                                }
                            }

                        }
                        if (isTriangleIntersected) {
                            if (!itTri.isDegenerate()) {
                                //Utils::log("tri inside" + itTri->debugDescription());
                                for (int m = 0; m < 3; m++) {
                                    DelaunaySegment d1 = new DelaunaySegment(itTri.i[m], itTri.i[(m + 1) % 3]);
                                    if (segments.find(d1) != segments.end())
                                        segments.erase(d1);
                                    else if (segments.find(d1.inverse()) != segments.end())
                                        segments.erase(d1.inverse());
                                    else
                                        segments.insert(d1);
                                }
                            }
                            //tbuffer.erase(itTri);
                            need_erase.Clear();
                            need_erase.Add(itTri);
                            triRemoved = true;
                        }
                    }
                }
                //do delete
                foreach (var v in need_erase) {
                    tbuffer.Remove(v);
                }
                // Find all the non-interior edges
                IEnumerator<DelaunaySegment> seg_ie = segments.GetEnumerator();
                //for (std::set<DelaunaySegment>::iterator it = segments.begin(); it!=segments.end(); ++it)
                while (seg_ie.MoveNext()) {
                    DelaunaySegment it = seg_ie.Current;
                    //Triangle dt(&pointList);
                    Triangle dt = new Triangle(pointList);
                    dt.setVertices(it.i1, it.i2, i);
                    dt.makeDirectIfNeeded();
                    //Utils::log("Add tri " + dt.debugDescription());
                    tbuffer.push_back(dt);

                }
            }

            // NB : Don't remove super triangle here, because all outer triangles are already removed in the addconstraints method.
            //      Uncomment that code if delaunay triangulation ever has to be unconstrained...
            /*TouchSuperTriangle touchSuperTriangle(maxTriangleIndex, maxTriangleIndex+1,maxTriangleIndex+2);
            tbuffer.remove_if(touchSuperTriangle);
            pointList.pop_back();
            pointList.pop_back();
            pointList.pop_back();*/
        }

        void _addConstraints(ref DelaunayTriangleBuffer tbuffer, PointList pl, std_vector<int> segmentListIndices) {
            std_vector<DelaunaySegment> segList = new std_vector<DelaunaySegment>();

            //Utils::log("a co");
            //for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end();it++)
            //	Utils::log(it->debugDescription());

            // First, list all the segments that are not already in one of the delaunay triangles
            //for (std::vector<int>::const_iterator it2 = segmentListIndices.begin(); it2 != segmentListIndices.end(); it2++)
            for (int i = 0; i < segmentListIndices.Count; i++) {
                //int i1 = *it2;
                int i1 = segmentListIndices[i];
                //it2++;
                i++;
                //int i2 = *it2;
                int i2 = segmentListIndices[i];

                bool isAlreadyIn = false;
                //for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end(); ++it)
                foreach (var it in tbuffer) {
                    if (it.containsSegment(i1, i2)) {
                        isAlreadyIn = true;
                        break;
                    }
                }
                // only do something for segments not already in DT
                if (!isAlreadyIn)
                    segList.push_back(new DelaunaySegment(i1, i2));
            }

            // Re-Triangulate according to the new segments
            //for (std::vector<DelaunaySegment>::iterator itSeg=segList.begin(); itSeg!=segList.end(); itSeg++)
            for (int ii = segList.Count - 1; ii >= 0; ii--) {
                DelaunaySegment itSeg = segList[ii];
                //Utils::log("itseg " + StringConverter::toString(itSeg->i1) + "," + StringConverter::toString(itSeg->i2) + " " + StringConverter::toString(pl[itSeg->i1]) + "," + StringConverter::toString(pl[itSeg->i2]));
                // Remove all triangles intersecting the segment and keep a list of outside edges
                std_set<DelaunaySegment> segments = new std_set<DelaunaySegment>();
                Segment2D seg1 = new Segment2D(pl[itSeg.i1], pl[itSeg.i2]);
                //for (DelaunayTriangleBuffer::iterator itTri = tbuffer.begin(); itTri!=tbuffer.end(); )
                for (int jj = tbuffer.Count - 1; jj >= 0; jj--) {
                    Triangle itTri = tbuffer.getElement(jj).Value;
                    bool isTriangleIntersected = false;
                    bool isDegenerate = false;
                    int degenIndex;
                    for (int i = 0; i < 3; i++) {
                        //Early out if 2 points are in fact the same
                        if (itTri.i[i] == itSeg.i1 || itTri.i[i] == itSeg.i2 || itTri.i[(i + 1) % 3] == itSeg.i1 || itTri.i[(i + 1) % 3] == itSeg.i2) {
                            if (itTri.isDegenerate()) {
                                if (itTri.i[i] == itSeg.i1 || itTri.i[(i + 1) % 3] == itSeg.i1)
                                    degenIndex = itSeg.i1;
                                else if (itTri.i[i] == itSeg.i2 || itTri.i[(i + 1) % 3] == itSeg.i2)
                                    degenIndex = itSeg.i2;
                                isTriangleIntersected = true;
                                isDegenerate = true;
                            }
                            else
                                continue;
                        }
                        Segment2D seg2 = new Segment2D(itTri.p(i), itTri.p((i + 1) % 3));
                        if (seg1.intersects(seg2)) {
                            isTriangleIntersected = true;
                            break;
                        }
                    }
                    if (isTriangleIntersected) {
                        //if (isDegenerate)
                        //Utils::log("degen " + itTri->debugDescription());
                        for (int k = 0; k < 3; k++) {
                            DelaunaySegment d1 = new DelaunaySegment(itTri.i[k], itTri.i[(k + 1) % 3]);
                            if (segments.find(d1) != segments.end())
                                segments.erase(d1);
                            else if (segments.find(d1.inverse()) != segments.end())
                                segments.erase(d1.inverse());
                            else
                                segments.insert(d1);
                        }
                        //itTri=tbuffer.erase(itTri);
                        tbuffer.erase(jj);
                    }
                    //else
                    //	itTri++;
                }

                // Divide the list of points (coming from remaining segments) in 2 groups : "above" and "below"
                std_vector<int> pointsAbove = new std_vector<int>();
                std_vector<int> pointsBelow = new std_vector<int>();
                int pt = itSeg.i1;
                bool isAbove = true;
                while (segments.size() > 0) {
                    //find next point
                    //for (std::set<DelaunaySegment>::iterator it = segments.begin(); it!=segments.end(); ++it)
                    DelaunaySegment[] segments_all = segments.get_allocator();
                    for (int i = 0; i < segments_all.Length; ++i) {
                        DelaunaySegment it = segments_all[i];//segments.find(i,true);
                        if (it.i1 == pt || it.i2 == pt) {
                            //Utils::log("next " + StringConverter::toString(pt));

                            if (it.i1 == pt)
                                pt = it.i2;
                            else
                                pt = it.i1;
                            segments.erase(it);
                            if (pt == itSeg.i2)
                                isAbove = false;
                            else if (pt != itSeg.i1) {
                                if (isAbove)
                                    pointsAbove.push_back(pt);
                                else
                                    pointsBelow.push_back(pt);
                            }
                            break;
                        }
                    }
                }

                // Recursively triangulate both polygons
                _recursiveTriangulatePolygon(itSeg, pointsAbove, tbuffer, pl);
                _recursiveTriangulatePolygon(itSeg.inverse(), pointsBelow, tbuffer, pl);
            }
            // Clean up segments outside of multishape
            if (mRemoveOutside) {
                if (mMultiShapeToTriangulate != null && mMultiShapeToTriangulate.isClosed()) {
                    //for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end();)
                    for (int i = tbuffer.Count - 1; i >= 0; i--) {
                        Triangle it = tbuffer.getElement(i).Value;
                        bool isTriangleOut = !mMultiShapeToTriangulate.isPointInside(it.getMidPoint());

                        if (isTriangleOut) {
                            //it = tbuffer.erase(it);
                            tbuffer.erase(i);
                        }
                        //else
                        //	++it;
                    }
                }
                else if (mShapeToTriangulate != null && mShapeToTriangulate.isClosed()) {
                    //for (DelaunayTriangleBuffer::iterator it = tbuffer.begin(); it!=tbuffer.end();)
                    for (int i = tbuffer.Count - 1; i >= 0; i--) {
                        Triangle it = tbuffer.getElement(i).Value;
                        bool isTriangleOut = !mShapeToTriangulate.isPointInside(it.getMidPoint());

                        if (isTriangleOut) {
                            //it = tbuffer.erase(it);
                            tbuffer.erase(i);
                        }
                        //else
                        //	++it;
                    }
                }
            }
        }

        //void Triangulator::_recursiveTriangulatePolygon(const DelaunaySegment& cuttingSeg, std::vector<int> inputPoints, DelaunayTriangleBuffer& tbuffer, const PointList&  pointList) const
        void _recursiveTriangulatePolygon(DelaunaySegment cuttingSeg, std_vector<int> inputPoints, DelaunayTriangleBuffer tbuffer, PointList pointList) {
            if (inputPoints.size() == 0)
                return;
            if (inputPoints.size() == 1) {
                Triangle t2 = new Triangle(pointList);
                //t.setVertices(cuttingSeg.i1, cuttingSeg.i2, inputPoints.begin());
                t2.setVertices(cuttingSeg.i1, cuttingSeg.i2, inputPoints.front());
                t2.makeDirectIfNeeded();
                tbuffer.push_back(t2);
                return;
            }
            // Find a point which, when associated with seg.i1 and seg.i2, builds a Delaunay triangle
            //std::vector<int>::iterator currentPoint = inputPoints.begin();
            int currentPoint_pos = inputPoints.begin();
            int currentPoint = inputPoints.front();
            bool found = false;
            while (!found) {
                bool isDelaunay = true;
                //Circle c=new Circle(pointList[*currentPoint], pointList[cuttingSeg.i1], pointList[cuttingSeg.i2]);
                Circle c = new Circle(pointList[currentPoint], pointList[cuttingSeg.i1], pointList[cuttingSeg.i2]);
                //for (std::vector<int>::iterator it = inputPoints.begin(); it!=inputPoints.end(); ++it)
                int idx = -1;
                foreach (var it in inputPoints) {
                    idx++;
                    //if (c.isPointInside(pointList[*it]) && (*it != *currentPoint))
                    if (c.isPointInside(pointList[it]) && (it != currentPoint)) {
                        isDelaunay = false;
                        currentPoint = it;
                        currentPoint_pos = idx;
                        break;
                    }
                }
                if (isDelaunay)
                    found = true;
            }

            // Insert current triangle
            Triangle t = new Triangle(pointList);
            //t.setVertices(*currentPoint, cuttingSeg.i1, cuttingSeg.i2);
            t.setVertices(currentPoint, cuttingSeg.i1, cuttingSeg.i2);
            t.makeDirectIfNeeded();
            tbuffer.push_back(t);

            // Recurse
            //DelaunaySegment newCut1=new DelaunaySegment(cuttingSeg.i1, *currentPoint);
            //DelaunaySegment newCut2=new DelaunaySegment(cuttingSeg.i2, *currentPoint);
            DelaunaySegment newCut1 = new DelaunaySegment(cuttingSeg.i1, currentPoint);
            DelaunaySegment newCut2 = new DelaunaySegment(cuttingSeg.i2, currentPoint);
            //std_vector<int> set1=new std_vector<int>(inputPoints.begin(), currentPoint);
            //std_vector<int> set2=new std_vector<int>(currentPoint+1, inputPoints.end());
            std_vector<int> set1 = new std_vector<int>();
            set1.assign(inputPoints.ToArray(), inputPoints.begin(), currentPoint_pos);
            std_vector<int> set2 = new std_vector<int>();
            set2.assign(inputPoints.ToArray(), currentPoint_pos + 1, inputPoints.end());

            if (!set1.empty())
                _recursiveTriangulatePolygon(newCut1, set1, tbuffer, pointList);
            if (!set2.empty())
                _recursiveTriangulatePolygon(newCut2, set2, tbuffer, pointList);
        }




        /// Default ctor
        //Triangulator() : mShapeToTriangulate(0), mMultiShapeToTriangulate(0), mManualSuperTriangle(0), mRemoveOutside(true), mSegmentListToTriangulate(0)
        //{
        //}
        public Triangulator() {
            mShapeToTriangulate = null;
            mMultiShapeToTriangulate = null;
            mManualSuperTriangle = null;
            mRemoveOutside = true;
            mSegmentListToTriangulate = null;
        }
        public Triangulator(Shape ss, MultiShape multis, Triangle2D t2d, bool removeOutSide, std_vector<Segment2D> listToTriangulate) {
            mShapeToTriangulate = ss;
            mMultiShapeToTriangulate = multis;
            mManualSuperTriangle = t2d;
            mRemoveOutside = removeOutSide;
            mSegmentListToTriangulate = listToTriangulate;
        }
        /// Sets shape to triangulate
        public Triangulator setShapeToTriangulate(Shape shape) {
            mShapeToTriangulate = shape;
            mMultiShapeToTriangulate = null;
            return this;
        }

        /// Sets multi shape to triangulate
        public Triangulator setMultiShapeToTriangulate(MultiShape multiShape) {
            mMultiShapeToTriangulate = multiShape;
            return this;
        }

        /// Sets segment list to triangulate
        public Triangulator setSegmentListToTriangulate(ref std_vector<Segment2D> segList) {
            mSegmentListToTriangulate = segList;
            return this;
        }

        /// Sets manual super triangle (instead of letting Triangulator guessing it)
        public Triangulator setManualSuperTriangle(Triangle2D tri) {
            mManualSuperTriangle = tri;
            return this;
        }

        /// Sets if the outside of shape must be removed
        public Triangulator setRemoveOutside(bool removeOutside) {
            mRemoveOutside = removeOutside;
            return this;
        }

        //    *
        //	 * Executes the Constrained Delaunay Triangulation algorithm
        //	 * @param output A vector of index where is outputed the resulting triangle indexes
        //	 * @param outputVertices A vector of vertices where is outputed the resulting triangle vertices
        //	 * @exception Ogre::InvalidStateException Either shape or multishape or segment list must be defined
        //	 
        //void triangulate(ref List<int>& output, ref List<Vector2>& outputVertices) const;
        public void triangulate(std_vector<int> output, PointList outputVertices) {
            if (mShapeToTriangulate == null && mMultiShapeToTriangulate == null && mSegmentListToTriangulate == null) {
                throw new NullReferenceException("Ogre::Exception::ERR_INVALID_STATE," + "Either shape or multishape or segment list must be defined!" + ",  Procedural::Triangulator::triangulate(std::vector<int>&, PointList&)");
            }
            //Ogre::Timer mTimer;
            //Mogre.Timer mTimer = new Timer();
            //mTimer.Reset();
            DelaunayTriangleBuffer dtb = new std_list<Triangle>();
            // Do the Delaunay triangulation
            std_vector<int> segmentListIndices = new std_vector<int>();

            if (mShapeToTriangulate != null) {
                outputVertices = new std_vector<Vector2>(mShapeToTriangulate.getPoints());
                for (int i = 0; i < mShapeToTriangulate.getSegCount(); ++i) {
                    segmentListIndices.push_back(i);
                    segmentListIndices.push_back(mShapeToTriangulate.getBoundedIndex(i + 1));
                }
            }
            else if (mMultiShapeToTriangulate != null) {
                outputVertices = new std_vector<Vector2>(mMultiShapeToTriangulate.getPoints());
                int index = 0;
                for (int i = 0; i < mMultiShapeToTriangulate.getShapeCount(); ++i) {
                    Shape shape = mMultiShapeToTriangulate.getShape((uint)i);
                    for (int j = 0; j < shape.getSegCount(); j++) {
                        segmentListIndices.push_back(index + j);
                        segmentListIndices.push_back(index + shape.getBoundedIndex(j + 1));
                    }
                    index += shape.getSegCount();
                }
            }
            else if (mSegmentListToTriangulate != null) {
                //std_map<Vector2, int, Vector2Comparator> backMap;
                std_map<Vector2, int> backMap = new std_map<Vector2, int>(new Vector2Comparator());
                //for (std::vector<Segment2D>::iterator it = mSegmentListToTriangulate->begin(); it!= mSegmentListToTriangulate->end(); it++)
                foreach (var it in mSegmentListToTriangulate) {
                    if ((it.mA - it.mB).SquaredLength < 1e-6)
                        continue;

                    //std::map<Vector2, int, Vector2Comparator>::iterator it2 = backMap.find(it->mA);
                    int it2_pos = backMap.find(it.mA);
                    //if (it2 != backMap.end())
                    if (it2_pos != -1) {
                        //segmentListIndices.push_back(it2->second);
                        segmentListIndices.push_back(backMap[it.mA]);
                    }
                    else {
                        //backMap[it->mA] = outputVertices.size();
                        backMap.insert(it.mA, outputVertices.size());
                        segmentListIndices.push_back(outputVertices.size());
                        outputVertices.push_back(it.mA);
                    }

                    //it2 = backMap.find(it.mB);
                    it2_pos = backMap.find(it.mB);
                    //if (it2 != backMap.end())
                    if (it2_pos != -1) {
                        //segmentListIndices.push_back(it2.second);
                        segmentListIndices.push_back(backMap[it.mB]);
                    }
                    else {
                        //backMap[it->mB] = outputVertices.size();
                        backMap.insert(it.mB, outputVertices.size());
                        segmentListIndices.push_back(outputVertices.size());
                        outputVertices.push_back(it.mB);
                    }
                }

                if (mManualSuperTriangle != null) {
                    Triangle superTriangle = new Triangle(outputVertices);
                    for (int i = 0; i < 3; i++) {
                        //std::map<Vector2, int, Vector2Comparator>::iterator it = backMap.find(mManualSuperTriangle->mPoints[i]);
                        int it_pos = backMap.find(mManualSuperTriangle.mPoints[i]);
                        //if (it != backMap.end())
                        if (it_pos != -1) {
                            //segmentListIndices.push_back(it->second);
                            //superTriangle.i[i] = it->second;
                            superTriangle.i[i] = backMap[mManualSuperTriangle.mPoints[i]];
                        }
                        else {
                            //backMap[mManualSuperTriangle->mPoints[i]] = outputVertices.size();
                            backMap.insert(mManualSuperTriangle.mPoints[i], outputVertices.size());
                            //segmentListIndices.push_back(outputVertices.size());
                            superTriangle.i[i] = outputVertices.size();
                            outputVertices.push_back(mManualSuperTriangle.mPoints[i]);
                        }
                    }

                    dtb.push_back(superTriangle);
                }
            }
            //Utils::log("Triangulator preparation : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
            delaunay(outputVertices, ref dtb);
            //Utils::log("Triangulator delaunay : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
            // Add contraints
            _addConstraints(ref dtb, outputVertices, segmentListIndices);
            //Utils::log("Triangulator constraints : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
            //Outputs index buffer
            //for (DelaunayTriangleBuffer::iterator it = dtb.begin(); it!=dtb.end(); ++it)
            foreach (var it in dtb) {
                if (!it.isDegenerate()) {
                    output.push_back(it.i[0]);
                    output.push_back(it.i[1]);
                    output.push_back(it.i[2]);
                }
            }
            // Remove super triangle
            if (mRemoveOutside) {
                outputVertices.pop_back();
                outputVertices.pop_back();
                outputVertices.pop_back();
            }
            //Utils::log("Triangulator output : " + StringConverter::toString(mTimer.getMicroseconds() / 1000.0f) + " ms");
        }
        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //void addToTriangleBuffer(ref TriangleBuffer& buffer) const;
        //-----------------------------------------------------------------------
        //void Triangulator::addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            PointList pointList = new std_vector<Vector2>();
            std_vector<int> indexBuffer = new std_vector<int>();
            triangulate(indexBuffer, pointList);
            for (int j = 0; j < pointList.size(); j++) {
                Vector2 vp2 = pointList[j];
                Vector3 vp = new Vector3(vp2.x, vp2.y, 0f);
                Vector3 normal = -Vector3.UNIT_Z;

                addPoint(ref buffer, vp, normal, new Vector2(vp2.x, vp2.y));
            }

            for (int i = 0; i < indexBuffer.size() / 3; i++) {
                buffer.index(indexBuffer[i * 3]);
                buffer.index(indexBuffer[i * 3 + 2]);
                buffer.index(indexBuffer[i * 3 + 1]);
            }

        }
    }

}




