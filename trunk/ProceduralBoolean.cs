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
// write with new std ... ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    //-----------------------------------------------------------------------
    //typedef std::multimap<Segment3D, int, Seg3Comparator> TriLookup;
    using TriLookup = std.std_multimap<Segment3D, int>;

    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport Boolean : public MeshGenerator<Boolean>
    public class Boolean : MeshGenerator<Boolean>
    {
        public enum BooleanOperation : int
        {
            BT_UNION,
            BT_INTERSECTION,
            BT_DIFFERENCE
        }
        private BooleanOperation mBooleanOperation;
        private TriangleBuffer mMesh1;
        private TriangleBuffer mMesh2;

        public Boolean() {
            mMesh1 = null;
            mMesh2 = null;
            mBooleanOperation = BooleanOperation.BT_UNION;
        }

        public Boolean setMesh1(TriangleBuffer tb) {
            mMesh1 = tb;
            return this;
        }

        public Boolean setMesh2(TriangleBuffer tb) {
            mMesh2 = tb;
            return this;
        }

        public Boolean setBooleanOperation(BooleanOperation op) {
            mBooleanOperation = op;
            return this;
        }

        //-----------------------------------------------------------------------

        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            std_vector<TriangleBuffer.Vertex> vec1 = mMesh1.getVertices();
            std_vector<int> ind1 = mMesh1.getIndices();
            std_vector<TriangleBuffer.Vertex> vec2 = mMesh2.getVertices();
            std_vector<int> ind2 = mMesh2.getIndices();
            Segment3D intersectionResult = new Segment3D();

            std_vector<Intersect> intersectionList = new std_vector<Intersect>();

            // Find all intersections between mMesh1 and mMesh2
            int idx1 = 0;
            //for (std::vector<int>::const_iterator it = ind1.begin(); it != ind1.end(); idx1++)
            for (int i = 0; i < ind1.Count; i += 3, idx1++) {
                int it = ind1[i];
                //Triangle3D t1(vec1[*it++].mPosition, vec1[*it++].mPosition, vec1[*it++].mPosition);
                Triangle3D t1 = new Triangle3D(vec1[it].mPosition, vec1[it + 1].mPosition, vec1[it + 2].mPosition);
                int idx2 = 0;
                //for (std::vector<int>::const_iterator it2 = ind2.begin(); it2 != ind2.end(); idx2++)
                for (int j = 0; j < ind2.Count; j += 3, idx2++) {
                    int it2 = ind2[j];
                    //Triangle3D t2(vec2[*it2++].mPosition, vec2[*it2++].mPosition, vec2[*it2++].mPosition);
                    Triangle3D t2 = new Triangle3D(vec2[it2].mPosition, vec2[it2 + 1].mPosition, vec2[it2 + 2].mPosition);
                    if (t1.findIntersect(t2, ref intersectionResult)) {
                        Intersect intersect = new Intersect(intersectionResult, idx1, idx2);
                        intersectionList.push_back(intersect);
                    }
                }

            }
            // Remove all intersection segments too small to be relevant
            //for (std::vector<Intersect>::iterator it = intersectionList.begin(); it != intersectionList.end();)
            //    if ((it.mSeg.mB - it.mSeg.mA).squaredLength() < 1e-8)
            //        it = intersectionList.erase(it);
            //    else
            //        ++it;
            for (int i = intersectionList.Count - 1; i >= 0; i--) {
                Intersect it = intersectionList[i];
                if ((it.mSeg.mB - it.mSeg.mA).SquaredLength < 1e-8)
                    intersectionList.erase((uint)i);
            }


            // Retriangulate
            TriangleBuffer newMesh1 = new TriangleBuffer();
            TriangleBuffer newMesh2 = new TriangleBuffer();
            GlobalMembersProceduralBoolean._retriangulate(ref newMesh1, mMesh1, intersectionList, true);
            GlobalMembersProceduralBoolean._retriangulate(ref newMesh2, mMesh2, intersectionList, false);

            //buffer.append(newMesh1);
            //buffer.append(newMesh2);
            //return;

            // Trace contours
            std_vector<Path> contours = new std_vector<Path>();
            std_vector<Segment3D> segmentSoup = new std_vector<Segment3D>();
            //for (std::vector<Intersect>::iterator it = intersectionList.begin(); it != intersectionList.end(); ++it)
            foreach (var it in intersectionList)
                segmentSoup.push_back(it.mSeg);
            new Path().buildFromSegmentSoup(segmentSoup, ref contours);

            // Build a lookup from segment to triangle
            TriLookup triLookup1 = new std_multimap<Segment3D, int>(new Seg3Comparator()), triLookup2 = new std_multimap<Segment3D, int>(new Seg3Comparator());
            GlobalMembersProceduralBoolean._buildTriLookup(ref triLookup1, newMesh1);
            GlobalMembersProceduralBoolean._buildTriLookup(ref triLookup2, newMesh2);

            std_set<Segment3D> limits = new std_set<Segment3D>(new Seg3Comparator());
            //for (std::vector<Segment3D>::iterator it = segmentSoup.begin(); it != segmentSoup.end(); ++it)
            foreach (var it in segmentSoup)
                limits.insert(it.orderedCopy());
            // Build resulting mesh
            //for (std::vector<Path>::iterator it = contours.begin(); it != contours.end(); ++it)
            foreach (var it in contours) {
                // Find 2 seed triangles for each contour
                Segment3D firstSeg = new Segment3D(it.getPoint(0), it.getPoint(1));
                //std_pair<TriLookup::iterator, TriLookup::iterator> it2mesh1 = triLookup1.equal_range(firstSeg.orderedCopy());
                //std_pair<TriLookup::iterator, TriLookup::iterator> it2mesh2 = triLookup2.equal_range(firstSeg.orderedCopy());
                std_pair<std_pair<Segment3D, List<int>>, std_pair<Segment3D, List<int>>> it2mesh1 = triLookup1.equal_range(firstSeg.orderedCopy());
                std_pair<std_pair<Segment3D, List<int>>, std_pair<Segment3D, List<int>>> it2mesh2 = triLookup2.equal_range(firstSeg.orderedCopy());
                int mesh1seed1 = 0, mesh1seed2 = 0, mesh2seed1 = 0, mesh2seed2 = 0;

                //if (it2mesh1.first != triLookup1.end() && it2mesh2.first != triLookup2.end())
                if (it2mesh1.first != null && it2mesh2.first != null) {
                    // check which of seed1 and seed2 must be included (it can be 0, 1 or both)
                    //mesh1seed1 = it2mesh1.first.second;
                    //mesh1seed2 = (--it2mesh1.second).second;
                    //mesh2seed1 = it2mesh2.first.second;
                    //mesh2seed2 = (--it2mesh2.second).second;
                    mesh1seed1 = it2mesh1.first.second[0];
                    mesh1seed2 = it2mesh1.first.second[it2mesh1.first.second.Count - 1]; //(--it2mesh1.second).second[0];
                    mesh2seed1 = it2mesh2.first.second[0];
                    mesh2seed2 = it2mesh2.first.second[it2mesh2.first.second.Count - 1];//(--it2mesh2.second).second[0];

                    if (mesh1seed1 == mesh1seed2)
                        mesh1seed2 = -1;
                    if (mesh2seed1 == mesh2seed2)
                        mesh2seed2 = -1;

                    Vector3 vMesh1, nMesh1, vMesh2, nMesh2;
                    for (int i = 0; i < 3; i++) {
                        Vector3 pos = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mPosition;
                        if ((pos - firstSeg.mA).SquaredLength > 1e-6 && (pos - firstSeg.mB).SquaredLength > 1e-6) {
                            vMesh1 = pos;
                            nMesh1 = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mNormal;
                            break;
                        }
                    }

                    for (int i = 0; i < 3; i++) {
                        Vector3 pos = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mPosition;
                        if ((pos - firstSeg.mA).SquaredLength > 1e-6 && (pos - firstSeg.mB).SquaredLength > 1e-6) {
                            vMesh2 = pos;
                            nMesh2 = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mNormal;
                            break;
                        }
                    }

                    bool M2S1InsideM1 = (nMesh1.DotProduct(vMesh2 - firstSeg.mA) < 0f);
                    bool M1S1InsideM2 = (nMesh2.DotProduct(vMesh1 - firstSeg.mA) < 0f);

                    GlobalMembersProceduralBoolean._removeFromTriLookup(mesh1seed1, ref triLookup1);
                    GlobalMembersProceduralBoolean._removeFromTriLookup(mesh2seed1, ref triLookup2);
                    GlobalMembersProceduralBoolean._removeFromTriLookup(mesh1seed2, ref triLookup1);
                    GlobalMembersProceduralBoolean._removeFromTriLookup(mesh2seed2, ref triLookup2);

                    // Recursively add all neighbours of these triangles
                    // Stop when a contour is touched
                    switch (mBooleanOperation) {
                        case BooleanOperation.BT_UNION:
                            if (M1S1InsideM2)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
                            if (M2S1InsideM1)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, false);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, false);
                            break;
                        case BooleanOperation.BT_INTERSECTION:
                            if (M1S1InsideM2)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
                            if (M2S1InsideM1)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref  triLookup2, limits, false);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, false);
                            break;
                        case BooleanOperation.BT_DIFFERENCE:
                            if (M1S1InsideM2)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref  triLookup1, limits, false);
                            if (M2S1InsideM1)
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref  triLookup2, limits, true);
                            else
                                GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, true);
                            break;
                    }
                }
            }
        }
    }
    //-----------------------------------------------------------------------

    public class Intersect
    {
        public Segment3D mSeg = new Segment3D();
        public int mTri1;
        public int mTri2;

        public Intersect(Segment3D seg, int tri1, int tri2) {
            //mSeg = new Segment3D(seg);           
            mSeg.mA = seg.mA;
            mSeg.mB = seg.mB;
            mTri1 = tri1;
            mTri2 = tri2;
        }
    }
    //-----------------------------------------------------------------------
    public class Seg3Comparator : IComparer<Segment3D>
    {

        //
        //ORIGINAL LINE: bool operator ()(const Segment3D& one, const Segment3D& two) const
        //C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
        public static bool Operator(Segment3D one, Segment3D two) {
            if (one.epsilonEquivalent(two))
                return false;

            if ((one.mA - two.mA).SquaredLength > 1e-6)
                return Vector3Comparator.Operator(one.mA, two.mA);
            return Vector3Comparator.Operator(one.mB, two.mB);
        }

        #region IComparer<Segment3D> 成员

        public int Compare(Segment3D x, Segment3D y) {
            bool ok = Operator(x, y);
            return ok ? 1 : -1;
        }

        #endregion
    }
    //-----------------------------------------------------------------------

    public static class GlobalMembersProceduralBoolean
    {
        //-----------------------------------------------------------------------

        public static Vector2 projectOnAxis(Vector3 input, Vector3 origin, Vector3 axis1, Vector3 axis2) {
            return new Vector2((input - origin).DotProduct(axis1), (input - origin).DotProduct(axis2));
        }
        //-----------------------------------------------------------------------

        public static Vector3 deprojectOnAxis(Vector2 input, Vector3 origin, Vector3 axis1, Vector3 axis2) {
            return origin + input.x * axis1 + input.y * axis2;
        }
        //-----------------------------------------------------------------------

        public static Segment2D projectOnAxis(Segment3D input, Vector3 origin, Vector3 axis1, Vector3 axis2) {
            return new Segment2D(projectOnAxis(input.mA, origin, axis1, axis2), projectOnAxis(input.mB, origin, axis1, axis2));
        }
        //-----------------------------------------------------------------------

        public static void _removeFromTriLookup(int k, ref TriLookup lookup) {
            //for (std.multimap<Segment3D, int, Seg3Comparator>.Enumerator it2 = lookup.begin(); it2 != lookup.end(); )
            int count = lookup.Count;
            for (int i = count - 1; i >= 0; i--) {
                //std.multimap<Segment3D, int, Seg3Comparator>.Enumerator removeIt = it2++;
                std_pair<Segment3D, List<int>> removeIt = lookup.get((uint)i);
                foreach (var v in removeIt.second) {
                    if (v == k) {
                        //lookup.Remove(removeIt);
                        lookup.Remove(removeIt.first);
                        break;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------

        public static void _recursiveAddNeighbour(ref TriangleBuffer result, TriangleBuffer source, int triNumber, ref TriLookup lookup, std_set<Segment3D> limits, bool inverted) {
            if (triNumber == -1)
                return;
            Utils.log("tri " + (triNumber.ToString()));
            std_vector<int> ind = source.getIndices();
            std_vector<TriangleBuffer.Vertex> vec = source.getVertices();
            result.rebaseOffset();
            if (inverted) {
                result.triangle(0, 2, 1);
                TriangleBuffer.Vertex v = vec[ind[triNumber * 3]];
                v.mNormal = -v.mNormal;
                result.vertex(v);
                v = vec[ind[triNumber * 3 + 1]];
                v.mNormal = -v.mNormal;
                result.vertex(v);
                v = vec[ind[triNumber * 3 + 2]];
                v.mNormal = -v.mNormal;
                result.vertex(v);
            }
            else {
                result.triangle(0, 1, 2);
                result.vertex(vec[ind[triNumber * 3]]);
                result.vertex(vec[ind[triNumber * 3 + 1]]);
                result.vertex(vec[ind[triNumber * 3 + 2]]);
            }

            //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3]].mPosition));
            //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3+1]].mPosition));
            //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3+2]].mPosition));

            std_pair<Segment3D, List<int>> it = null;

            int nextTriangle1 = -1;
            int nextTriangle2 = -1;
            int nextTriangle3 = -1;
            int it_find = lookup.find(new Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 1]].mPosition).orderedCopy());
            ////if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            ////	Utils::log("Cross limit1");
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) == limits.end()) {
            //    nextTriangle1 = it->second;
            //    _removeFromTriLookup(nextTriangle1, lookup);
            //} 
            if (it_find != -1) {
                it = lookup.get((uint)it_find);
                if (limits.find(it.first.orderedCopy()) == -1) {
                    nextTriangle1 = it.second[0];
                    GlobalMembersProceduralBoolean._removeFromTriLookup(nextTriangle1, ref lookup);
                }
            }
            //	it = lookup.find(Segment3D(vec[ind[triNumber * 3 + 1]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
            it_find = lookup.find(new Segment3D(vec[ind[triNumber * 3 + 1]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());

            ////if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            ////Utils::log("Cross limit2");
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) == limits.end()) {
            //    nextTriangle2 = it->second;
            //    _removeFromTriLookup(nextTriangle2, lookup);
            //}
            if (it_find != -1) {
                it = lookup.get((uint)it_find);
                if (limits.find(it.first.orderedCopy()) == -1) {
                    nextTriangle2 = it.second[0];
                    GlobalMembersProceduralBoolean._removeFromTriLookup(nextTriangle2, ref lookup);
                }
            }
            //it = lookup.find(Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
            ////if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            ////	Utils::log("Cross limit3");
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) == limits.end()) {
            //    nextTriangle3 = it->second;
            //    _removeFromTriLookup(nextTriangle3, lookup);
            //}
            it_find = lookup.find(new Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
            if (it_find != -1) {
                it = lookup.get((uint)it_find);
                if (limits.find(it.first.orderedCopy()) == -1) {
                    nextTriangle3 = it.second[0];
                    GlobalMembersProceduralBoolean._removeFromTriLookup(nextTriangle3, ref lookup);
                }
            }

            //Utils::log("add " + StringConverter::toString(nextTriangle1) + " ," + StringConverter::toString(nextTriangle2) + " ,"+StringConverter::toString(nextTriangle3) );

            _recursiveAddNeighbour(ref result, source, nextTriangle1, ref lookup, limits, inverted);
            _recursiveAddNeighbour(ref result, source, nextTriangle2, ref lookup, limits, inverted);
            _recursiveAddNeighbour(ref result, source, nextTriangle3, ref lookup, limits, inverted);
        }
        //-----------------------------------------------------------------------

        public static void _retriangulate(ref TriangleBuffer newMesh, TriangleBuffer inputMesh, std_vector<Intersect> intersectionList, bool first) {
            std_vector<TriangleBuffer.Vertex> vec = inputMesh.getVertices();
            std_vector<int> ind = inputMesh.getIndices();
            // Triangulate
            //  Group intersections by triangle indice
            std_map<int, std_vector<Segment3D>> meshIntersects = new std_map<int, std_vector<Segment3D>>();
            //for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in intersectionList) {
                int it2_find;
                if (first)
                    it2_find = meshIntersects.find(it.mTri1);
                else
                    it2_find = meshIntersects.find(it.mTri2);
                if (it2_find != -1) {
                    std_pair<int, std_vector<Segment3D>> it2 = meshIntersects.get((uint)it2_find);
                    it2.second.push_back(it.mSeg);
                }
                else {
                    std_vector<Segment3D> vec2 = new std_vector<Segment3D>();
                    vec2.push_back(it.mSeg);
                    if (first)
                        meshIntersects[it.mTri1] = vec2;
                    else
                        meshIntersects[it.mTri2] = vec2;
                }
            }
            // Build a new TriangleBuffer holding non-intersected triangles and retriangulated-intersected triangles
            //for (List<TriangleBuffer.Vertex>.Enumerator it = vec.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in vec) {
                newMesh.vertex(it);
            }
            //for (int i = 0; i < (int)ind.Count / 3; i++)
            //    if (meshIntersects.find(i) == meshIntersects.end())
            //        newMesh.triangle(ind[i * 3], ind[i * 3 + 1], ind[i * 3 + 2]);
            for (int i = 0; i < (int)ind.size() / 3; i++) {
                if (meshIntersects.find(i) == -1) {
                    newMesh.triangle(ind[i * 3], ind[i * 3 + 1], ind[i * 3 + 2]);
                }
            }

            int numNonIntersected1 = newMesh.getIndices().size();
            //for (std.map<int, List<Segment3D> >.Enumerator it = meshIntersects.begin(); it.MoveNext(); ++it)
            foreach (var it in meshIntersects) {
                std_vector<Segment3D> segments = it.Value;
                int triIndex = it.Key;
                Vector3 v1 = vec[ind[triIndex * 3]].mPosition;
                Vector3 v2 = vec[ind[triIndex * 3 + 1]].mPosition;
                Vector3 v3 = vec[ind[triIndex * 3 + 2]].mPosition;
                Vector3 triNormal = ((v2 - v1).CrossProduct(v3 - v1)).NormalisedCopy;
                Vector3 xAxis = triNormal.Perpendicular;
                Vector3 yAxis = triNormal.CrossProduct(xAxis);
                Vector3 planeOrigin = vec[ind[triIndex * 3]].mPosition;

                // Project intersection segments onto triangle plane
                std_vector<Segment2D> segments2 = new std_vector<Segment2D>();

                //for (List<Segment3D>.Enumerator it2 = segments.GetEnumerator(); it2.MoveNext(); it2++)
                //    segments2.Add(projectOnAxis(it2.Current, planeOrigin, xAxis, yAxis));
                foreach (var it2 in segments) {
                    segments2.push_back(projectOnAxis(it2, planeOrigin, xAxis, yAxis));
                }
                //for (List<Segment2D>.Enumerator it2 = segments2.GetEnumerator(); it2.MoveNext();)
                int it2_c = segments2.Count;
                for (int j = it2_c - 1; j >= 0; j--) {
                    Segment2D it2 = segments2[j];
                    if ((it2.mA - it2.mB).SquaredLength < 1e-5)
                        //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
                        //it2 = segments2.erase(it2);
                        segments2.RemoveAt(j);
                    //else
                }
                // Triangulate
                Triangulator t = new Triangulator();
                //Triangle2D[[]] tri = new Triangle2D[ind[triIndex * 3]](projectOnAxis(vec.mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 1]].mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 2]].mPosition, planeOrigin, xAxis, yAxis));
                Triangle2D tri = new Triangle2D(projectOnAxis(vec[ind[triIndex * 3]].mPosition, planeOrigin, xAxis, yAxis),
                           projectOnAxis(vec[ind[triIndex * 3 + 1]].mPosition, planeOrigin, xAxis, yAxis),
                           projectOnAxis(vec[ind[triIndex * 3 + 2]].mPosition, planeOrigin, xAxis, yAxis));
                std_vector<Vector2> outPointList = new std_vector<Vector2>();//PointList outPointList;
                std_vector<int> outIndice = new std_vector<int>();
                t.setManualSuperTriangle(tri).setRemoveOutside(false).setSegmentListToTriangulate(ref segments2).triangulate(outIndice, outPointList);

                // Deproject and add to triangleBuffer
                newMesh.rebaseOffset();
                //for (List<int>.Enumerator it = outIndice.GetEnumerator(); it.MoveNext(); ++it)
                //    newMesh.index(it.Current);
                foreach (var oindex in outIndice) {
                    newMesh.index(oindex);
                }
                float x1 = tri.mPoints[0].x;
                float y1 = tri.mPoints[0].y;
                Vector2 uv1 = vec[ind[triIndex * 3]].mUV;
                float x2 = tri.mPoints[1].x;
                float y2 = tri.mPoints[1].y;
                Vector2 uv2 = vec[ind[triIndex * 3 + 1]].mUV;
                float x3 = tri.mPoints[2].x;
                float y3 = tri.mPoints[2].y;
                Vector2 uv3 = vec[ind[triIndex * 3 + 2]].mUV;
                float DET = x1 * y2 - x2 * y1 + x2 * y3 - x3 * y2 + x3 * y1 - x1 * y3;
                Vector2 A = ((y2 - y3) * uv1 + (y3 - y1) * uv2 + (y1 - y2) * uv3) / DET;
                Vector2 B = ((x3 - x2) * uv1 + (x1 - x3) * uv2 + (x2 - x1) * uv3) / DET;
                Vector2 C = ((x2 * y3 - x3 * y2) * uv1 + (x3 * y1 - x1 * y3) * uv2 + (x1 * y2 - x2 * y1) * uv3) / DET;

                //for (List<Vector2>.Enumerator it = outPointList.GetEnumerator(); it.MoveNext(); ++it)
                foreach (var it2 in outPointList) {
                    Vector2 uv = A * it2.x + B * it2.y + C;
                    newMesh.position(deprojectOnAxis(it2, planeOrigin, xAxis, yAxis));
                    newMesh.normal(triNormal);
                    newMesh.textureCoord(uv);
                }
            }
        }
        //-----------------------------------------------------------------------

        public static void _buildTriLookup(ref TriLookup lookup, TriangleBuffer newMesh) {
            std_vector<TriangleBuffer.Vertex> nvec = newMesh.getVertices();
            std_vector<int> nind = newMesh.getIndices();
            for (int i = 0; i < (int)nind.Count / 3; i++) {
                lookup.insert(new Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 1]].mPosition).orderedCopy(), i);
                lookup.insert(new KeyValuePair<Segment3D, int>(new Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
                lookup.insert(new KeyValuePair<Segment3D, int>(new Segment3D(nvec[nind[i * 3 + 1]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
            }
        }
    }
    //-----------------------------------------------------------------------


    internal sealed class DefineConstantsProceduralBoolean
    {
        public const int OGRE_PROFILING = 0;
#if OGRE_ASSERT_MODE_AlternateDefinition1
	public const int OGRE_ASSERT_MODE = 0;
#elif OGRE_ASSERT_MODE_AlternateDefinition2
	public const int OGRE_ASSERT_MODE = 1;
#endif
        public const int OGRE_PRETEND_TEXTURE_UNITS = 0;
        public const int OGRE_DOUBLE_PRECISION = 0;
        public const int OGRE_MAX_TEXTURE_COORD_SETS = 6;
        public const int OGRE_MAX_TEXTURE_LAYERS = 16;
        public const int OGRE_MAX_SIMULTANEOUS_LIGHTS = 8;
        public const int OGRE_MAX_BLEND_WEIGHTS = 4;
        public const int OGRE_MEMORY_ALLOCATOR_STD = 1;
        public const int OGRE_MEMORY_ALLOCATOR_NED = 2;
        public const int OGRE_MEMORY_ALLOCATOR_USER = 3;
        public const int OGRE_MEMORY_ALLOCATOR_NEDPOOLING = 4;
        public const int OGRE_CONTAINERS_USE_CUSTOM_MEMORY_ALLOCATOR = 1;
        public const int OGRE_STRING_USE_CUSTOM_MEMORY_ALLOCATOR = 0;
        public const int OGRE_MEMORY_TRACKER_DEBUG_MODE = 0;
        public const int OGRE_MEMORY_TRACKER_RELEASE_MODE = 0;
        public const int OGRE_MAX_MULTIPLE_RENDER_TARGETS = 8;
        public const int OGRE_THREAD_SUPPORT = 0;
        public const int OGRE_THREAD_PROVIDER = 0;
        public const int OGRE_NO_FREEIMAGE = 0;
        public const int OGRE_NO_DEVIL = 1;
        public const int OGRE_NO_DDS_CODEC = 0;
        public const int OGRE_NO_ZIP_ARCHIVE = 0;
        public const int OGRE_USE_NEW_COMPILERS = 1;
        public const int OGRE_PLATFORM_WIN32 = 1;
        public const int OGRE_PLATFORM_LINUX = 2;
        public const int OGRE_PLATFORM_APPLE = 3;
        public const int OGRE_PLATFORM_SYMBIAN = 4;
        public const int OGRE_PLATFORM_IPHONE = 5;
        public const int OGRE_COMPILER_MSVC = 1;
        public const int OGRE_COMPILER_GNUC = 2;
        public const int OGRE_COMPILER_BORL = 3;
        public const int OGRE_COMPILER_WINSCW = 4;
        public const int OGRE_COMPILER_GCCE = 5;
        public const int OGRE_ENDIAN_LITTLE = 1;
        public const int OGRE_ENDIAN_BIG = 2;
        public const int OGRE_ARCHITECTURE_32 = 1;
        public const int OGRE_ARCHITECTURE_64 = 2;
#if OGRE_DEBUG_MODE_AlternateDefinition1
	public const int OGRE_DEBUG_MODE = 1;
#elif OGRE_DEBUG_MODE_AlternateDefinition2
	public const int OGRE_DEBUG_MODE = 0;
#endif
#if OGRE_UNICODE_SUPPORT_AlternateDefinition1
	public const int OGRE_UNICODE_SUPPORT = 1;
#elif OGRE_UNICODE_SUPPORT_AlternateDefinition2
	public const int OGRE_UNICODE_SUPPORT = 0;
#endif
        public const int CLOCKS_PER_SEC = 1000;
#if OGRE_PLATFORM_LIB_AlternateDefinition1
	public const string OGRE_PLATFORM_LIB = "OgrePlatform.bundle";
#elif OGRE_PLATFORM_LIB_AlternateDefinition2
	public const string OGRE_PLATFORM_LIB = "OgrePlatform.a";
#elif OGRE_PLATFORM_LIB_AlternateDefinition3
	public const string OGRE_PLATFORM_LIB = "libOgrePlatform.so";
#endif
#if OGRE_MEMORY_TRACKER_AlternateDefinition1
	public const int OGRE_MEMORY_TRACKER = 1;
#elif OGRE_MEMORY_TRACKER_AlternateDefinition2
	public const int OGRE_MEMORY_TRACKER = 0;
#endif
        public const int OGRE_VERSION_MAJOR = 1;
        public const int OGRE_VERSION_MINOR = 7;
        public const int OGRE_VERSION_PATCH = 4;
        public const string OGRE_VERSION_SUFFIX = "";
        public const string OGRE_VERSION_NAME = "Cthugha";
        public const int OGRE_THREAD_HARDWARE_CONCURRENCY = 2;
        public const int OGRE_STREAM_TEMP_SIZE = 128;
        public const int OGRE_RENDERABLE_DEFAULT_PRIORITY = 100;
        public const int OGRE_MAX_NUM_BONES = 256;
        public const int OGRE_LOG_THRESHOLD = 4;
#if OGRE_IS_NATIVE_WCHAR_T_AlternateDefinition1
	public const int OGRE_IS_NATIVE_WCHAR_T = 1;
#elif OGRE_IS_NATIVE_WCHAR_T_AlternateDefinition2
	public const int OGRE_IS_NATIVE_WCHAR_T = 0;
#endif
        public const int __RenderSystemCapabilities__ = 1;
        public const int CAPS_CATEGORY_SIZE = 4;
        public const int OGRE_NUM_RENDERTARGET_GROUPS = 10;
        public const int OGRE_DEFAULT_RT_GROUP = 4;
        public const int OGRE_REND_TO_TEX_RT_GROUP = 2;
#if PROCEDURAL_RED_AlternateDefinition1
	public const int PROCEDURAL_RED = 3;
#elif PROCEDURAL_RED_AlternateDefinition2
	public const int PROCEDURAL_RED = 0;
#endif
#if PROCEDURAL_GREEN_AlternateDefinition1
	public const int PROCEDURAL_GREEN = 2;
#elif PROCEDURAL_GREEN_AlternateDefinition2
	public const int PROCEDURAL_GREEN = 1;
#endif
#if PROCEDURAL_BLUE_AlternateDefinition1
	public const int PROCEDURAL_BLUE = 1;
#elif PROCEDURAL_BLUE_AlternateDefinition2
	public const int PROCEDURAL_BLUE = 2;
#endif
#if PROCEDURAL_ALPHA_AlternateDefinition1
	public const int PROCEDURAL_ALPHA = 0;
#elif PROCEDURAL_ALPHA_AlternateDefinition2
	public const int PROCEDURAL_ALPHA = 3;
#endif
    }






}
