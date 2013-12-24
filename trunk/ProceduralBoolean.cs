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

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer)
	{
		 List<TriangleBuffer.Vertex> vec1 = mMesh1.getVertices();
		 List<int> ind1 = mMesh1.getIndices();
		 List<TriangleBuffer.Vertex> vec2 = mMesh2.getVertices();
		 List<int> ind2 = mMesh2.getIndices();
		Segment3D intersectionResult = new Segment3D();
	
		List<Intersect> intersectionList = new List<Intersect>();
	
		// Find all intersections between mMesh1 and mMesh2
		int idx1 = 0;
		for (List<int>.Enumerator it = ind1.GetEnumerator(); it.MoveNext(); idx1++)
		{
			Triangle3D[] t1 = new Triangle3D[it.Current++](vec1.mPosition, vec1[it.Current++].mPosition, vec1[it.Current++].mPosition);
	
			int idx2 = 0;
			for (List<int>.Enumerator it2 = ind2.GetEnumerator(); it2.MoveNext(); idx2++)
			{
				Triangle3D[] t2 = new Triangle3D[it2.Current++](vec2.mPosition, vec2[it2.Current++].mPosition, vec2[it2.Current++].mPosition);
	
				if (t1.findIntersect(t2, ref intersectionResult))
				{
					Intersect GlobalMembersProceduralBoolean.intersect = new Intersect(intersectionResult, idx1, idx2);
					intersectionList.Add(GlobalMembersProceduralBoolean.intersect);
				}
			}
		}
		// Remove all intersection segments too small to be relevant
		for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext();)
			if ((it.mSeg.mB - it.mSeg.mA).squaredLength() < 1e-8)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
				it = intersectionList.erase(it);
			else
	
		// Retriangulate
		TriangleBuffer newMesh1 = new TriangleBuffer();
		TriangleBuffer newMesh2 = new TriangleBuffer();
		GlobalMembersProceduralBoolean._retriangulate(ref newMesh1, mMesh1, intersectionList, true);
		GlobalMembersProceduralBoolean._retriangulate(ref newMesh2, mMesh2, intersectionList, false);
	
		//buffer.append(newMesh1);
		//buffer.append(newMesh2);
		//return;
	
		// Trace contours
		List<Path> contours = new List<Path>();
		List<Segment3D> segmentSoup = new List<Segment3D>();
		for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext(); ++it)
			segmentSoup.Add(it.mSeg);
		Path().buildFromSegmentSoup(segmentSoup, ref contours);
	
		// Build a lookup from segment to triangle
		std.multimap<Segment3D, int, Seg3Comparator> triLookup1 = new std.multimap<Segment3D, int, Seg3Comparator>();
		std.multimap<Segment3D, int, Seg3Comparator> triLookup2 = new std.multimap<Segment3D, int, Seg3Comparator>();
		GlobalMembersProceduralBoolean._buildTriLookup(ref triLookup1, newMesh1);
		GlobalMembersProceduralBoolean._buildTriLookup(ref triLookup2, newMesh2);
	
		std.set<Segment3D, Seg3Comparator> limits = new std.set<Segment3D, Seg3Comparator>();
		for (List<Segment3D>.Enumerator it = segmentSoup.GetEnumerator(); it.MoveNext(); ++it)
			limits.insert(it.orderedCopy());
		// Build resulting mesh
		for (List<Path>.Enumerator it = contours.GetEnumerator(); it.MoveNext(); ++it)
		{
			// Find 2 seed triangles for each contour
			Segment3D firstSeg = new Segment3D(it.getPoint(0), it.getPoint(1));
	
			std.pair<std.multimap<Segment3D, int, Seg3Comparator>.Enumerator, std.multimap<Segment3D, int, Seg3Comparator>.Enumerator> it2mesh1 = triLookup1.equal_range(firstSeg.orderedCopy());
			std.pair<std.multimap<Segment3D, int, Seg3Comparator>.Enumerator, std.multimap<Segment3D, int, Seg3Comparator>.Enumerator> it2mesh2 = triLookup2.equal_range(firstSeg.orderedCopy());
			int mesh1seed1;
			int mesh1seed2;
			int mesh2seed1;
			int mesh2seed2;
	
			if (it2mesh1.first != triLookup1.end() && it2mesh2.first != triLookup2.end())
			{
				// check which of seed1 and seed2 must be included (it can be 0, 1 or both)
				mesh1seed1 = it2mesh1.first.second;
				mesh1seed2 = (--it2mesh1.second).second;
				mesh2seed1 = it2mesh2.first.second;
				mesh2seed2 = (--it2mesh2.second).second;
				if (mesh1seed1 == mesh1seed2)
					mesh1seed2 = -1;
				if (mesh2seed1 == mesh2seed2)
					mesh2seed2 = -1;
	
				Vector3 vMesh1 = new Vector3();
				Vector3 nMesh1 = new Vector3();
				Vector3 vMesh2 = new Vector3();
				Vector3 nMesh2 = new Vector3();
				for (int i =0; i<3; i++)
				{
					const Vector3 pos = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mPosition;
					if (pos.squaredDistance(firstSeg.mA)>1e-6 && pos.squaredDistance(firstSeg.mB)>1e-6)
					{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vMesh1 = pos;
						vMesh1.CopyFrom(pos);
						nMesh1 = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mNormal;
						break;
					}
				}
	
				for (int i =0; i<3; i++)
				{
					const Vector3 pos = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mPosition;
					if (pos.squaredDistance(firstSeg.mA)>1e-6 && pos.squaredDistance(firstSeg.mB)>1e-6)
					{
//C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
//ORIGINAL LINE: vMesh2 = pos;
						vMesh2.CopyFrom(pos);
						nMesh2 = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mNormal;
						break;
					}
				}
	
				bool M2S1InsideM1 = (nMesh1.dotProduct(vMesh2-firstSeg.mA) < 0);
				bool M1S1InsideM2 = (nMesh2.dotProduct(vMesh1-firstSeg.mA) < 0);
	
				GlobalMembersProceduralBoolean._removeFromTriLookup(mesh1seed1, ref triLookup1);
				GlobalMembersProceduralBoolean._removeFromTriLookup(mesh2seed1, ref triLookup2);
				GlobalMembersProceduralBoolean._removeFromTriLookup(mesh1seed2, ref triLookup1);
				GlobalMembersProceduralBoolean._removeFromTriLookup(mesh2seed2, ref triLookup2);
	
				// Recursively add all neighbours of these triangles
				// Stop when a contour is touched
				switch (mBooleanOperation)
				{
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
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, false);
					else
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, false);
					break;
				case BooleanOperation.BT_DIFFERENCE:
					if (M1S1InsideM2)
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
					else
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
					if (M2S1InsideM1)
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, true);
					else
						GlobalMembersProceduralBoolean._recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, true);
					break;
				}
			}
		}
	}
    }
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

        public static void _removeFromTriLookup(int k, ref std.multimap<Segment3D, int, Seg3Comparator> lookup) {
            for (std.multimap<Segment3D, int, Seg3Comparator>.Enumerator it2 = lookup.begin(); it2 != lookup.end(); ) {
                std.multimap<Segment3D, int, Seg3Comparator>.Enumerator removeIt = it2++;
                if (removeIt.second == k)
                    lookup.erase(removeIt);
            }
        }
        //-----------------------------------------------------------------------

        public static void _recursiveAddNeighbour(ref TriangleBuffer result, TriangleBuffer source, int triNumber, ref std.multimap<Segment3D, int, Seg3Comparator> lookup, std.set<Segment3D, Seg3Comparator> limits, bool inverted) {
            if (triNumber == -1)
                return;
            Utils.log("tri " + StringConverter.ToString(triNumber));
            List<int> ind = source.getIndices();
            List<TriangleBuffer.Vertex> vec = source.getVertices();
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

            std.multimap<Segment3D, int, Seg3Comparator>.Enumerator it;

            int nextTriangle1 = -1;
            int nextTriangle2 = -1;
            int nextTriangle3 = -1;
            it = lookup.find(new Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 1]].mPosition).orderedCopy());
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            //	Utils::log("Cross limit1");

            if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end()) {
                nextTriangle1 = it.second;
                _removeFromTriLookup(nextTriangle1, ref lookup);
            }
            it = lookup.find(new Segment3D(vec[ind[triNumber * 3 + 1]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            //Utils::log("Cross limit2");

            if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end()) {
                nextTriangle2 = it.second;
                _removeFromTriLookup(nextTriangle2, ref lookup);
            }
            it = lookup.find(new Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
            //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
            //	Utils::log("Cross limit3");
            if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end()) {
                nextTriangle3 = it.second;
                _removeFromTriLookup(nextTriangle3, ref lookup);
            }
            //Utils::log("add " + StringConverter::toString(nextTriangle1) + " ," + StringConverter::toString(nextTriangle2) + " ,"+StringConverter::toString(nextTriangle3) );

            _recursiveAddNeighbour(ref result, source, nextTriangle1, ref lookup, limits, inverted);
            _recursiveAddNeighbour(ref result, source, nextTriangle2, ref lookup, limits, inverted);
            _recursiveAddNeighbour(ref result, source, nextTriangle3, ref lookup, limits, inverted);
        }
        //-----------------------------------------------------------------------

        public static void _retriangulate(ref TriangleBuffer newMesh, TriangleBuffer inputMesh, List<Intersect> intersectionList, bool first)
	{
		const List<TriangleBuffer.Vertex> vec = inputMesh.getVertices();
		const List<int> ind = inputMesh.getIndices();
		// Triangulate
		//  Group intersections by triangle indice
		std.map<int, List<Segment3D> > meshIntersects = new std.map<int, List<Segment3D> >();
		for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext(); ++it)
		{
			std.map<int, List<Segment3D> >.Enumerator it2;
			if (first)
				it2 = meshIntersects.find(it.mTri1);
			else
				it2 = meshIntersects.find(it.mTri2);
			if (it2 != meshIntersects.end())
				it2.second.push_back(it.mSeg);
			else
			{
				List<Segment3D> vec = new List<Segment3D>();
				vec.Add(it.mSeg);
				if (first)
					meshIntersects[it.mTri1] = vec;
				else
					meshIntersects[it.mTri2] = vec;
			}
		}
		// Build a new TriangleBuffer holding non-intersected triangles and retriangulated-intersected triangles
		for (List<TriangleBuffer.Vertex>.Enumerator it = vec.GetEnumerator(); it.MoveNext(); ++it)
			newMesh.vertex(it.Current);
		for (int i = 0; i < (int)ind.Count / 3; i++)
			if (meshIntersects.find(i) == meshIntersects.end())
				newMesh.triangle(ind[i * 3], ind[i * 3 + 1], ind[i * 3 + 2]);
		int numNonIntersected1 = newMesh.getIndices().size();
		for (std.map<int, List<Segment3D> >.Enumerator it = meshIntersects.begin(); it.MoveNext(); ++it)
		{
			List<Segment3D> segments = it.second;
			int triIndex = it.first;
			Vector3 v1 = vec[ind[triIndex * 3]].mPosition;
			Vector3 v2 = vec[ind[triIndex * 3+1]].mPosition;
			Vector3 v3 = vec[ind[triIndex * 3+2]].mPosition;
			Vector3 triNormal = ((v2-v1).crossProduct(v3-v1)).normalisedCopy();
			Vector3 xAxis = triNormal.perpendicular();
			Vector3 yAxis = triNormal.crossProduct(xAxis);
			Vector3 planeOrigin = vec[ind[triIndex * 3]].mPosition;

			// Project intersection segments onto triangle plane
			List<Segment2D> segments2 = new List<Segment2D>();

			for (List<Segment3D>.Enumerator it2 = segments.GetEnumerator(); it2.MoveNext(); it2++)
				segments2.Add(projectOnAxis(it2.Current, planeOrigin, xAxis, yAxis));
			for (List<Segment2D>.Enumerator it2 = segments2.GetEnumerator(); it2.MoveNext();)
				if ((it2.mA - it2.mB).squaredLength() < 1e-5)
//C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
					it2 = segments2.erase(it2);
				else

			// Triangulate
			Triangulator t = new Triangulator();
			Triangle2D[[]] tri = new Triangle2D[ind[triIndex * 3]](projectOnAxis(vec.mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 1]].mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 2]].mPosition, planeOrigin, xAxis, yAxis));
			List<Mogre.Vector2> outPointList = new List<Mogre.Vector2>();
			List<int> outIndice = new List<int>();
			t.setManualSuperTriangle(tri).setRemoveOutside(false).setSegmentListToTriangulate(ref segments2).triangulate(ref outIndice, ref outPointList);

			// Deproject and add to triangleBuffer
			newMesh.rebaseOffset();
			for (List<int>.Enumerator it = outIndice.GetEnumerator(); it.MoveNext(); ++it)
				newMesh.index(it.Current);
			float x1 = tri.mPoints[0].x;
			float y1 = tri.mPoints[0].y;
			Vector2 uv1 = vec[ind[triIndex * 3]].mUV;
			float x2 = tri.mPoints[1].x;
			float y2 = tri.mPoints[1].y;
			Vector2 uv2 = vec[ind[triIndex * 3 + 1]].mUV;
			float x3 = tri.mPoints[2].x;
			float y3 = tri.mPoints[2].y;
			Vector2 uv3 = vec[ind[triIndex * 3 + 2]].mUV;
			float DET = x1 * y2 - x2 * y1 + x2 * y3 - x3 * y2 + x3 * y1 - x1 *y3;
			Vector2 A = ((y2 - y3) * uv1 + (y3 - y1) * uv2 + (y1 - y2) * uv3) / DET;
			Vector2 B = ((x3 - x2) * uv1 + (x1 - x3) * uv2 + (x2 - x1) * uv3) / DET;
			Vector2 C = ((x2 * y3 - x3 * y2) * uv1 + (x3 * y1 - x1 * y3) * uv2 + (x1 * y2 - x2 * y1) * uv3) / DET;

			for (List<Vector2>.Enumerator it = outPointList.GetEnumerator(); it.MoveNext(); ++it)
			{
				Vector2 uv = A * it.x + B * it.y + C;
				newMesh.position(deprojectOnAxis(it.Current, planeOrigin, xAxis, yAxis));
				newMesh.normal(triNormal);
				newMesh.textureCoord(uv);
			}
		}
	}
        //-----------------------------------------------------------------------

        public static void _buildTriLookup(ref std.multimap<Segment3D, int, Seg3Comparator> lookup, TriangleBuffer newMesh) {
            const List<TriangleBuffer.Vertex> nvec = newMesh.getVertices();
            const List<int> nind = newMesh.getIndices();
            for (int i = 0; i < (int)nind.Count / 3; i++) {
                lookup.insert(std.pair<Segment3D, int>(new Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 1]].mPosition).orderedCopy(), i));
                lookup.insert(std.pair<Segment3D, int>(new Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
                lookup.insert(std.pair<Segment3D, int>(new Segment3D(nvec[nind[i * 3 + 1]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
            }
        }
    }
    public class Intersect
    {
        public Segment3D mSeg = new Segment3D();
        public int mTri1;
        public int mTri2;

        public Intersect(Segment3D seg, int tri1, int tri2) {
            mSeg = new Segment3D(seg);
            mTri1 = tri1;
            mTri2 = tri2;
        }
    }
    //-----------------------------------------------------------------------

    public class Seg3Comparator
    {

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: bool operator ()(const Segment3D& one, const Segment3D& two) const
        //C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
        public static bool operator ==(Segment3D one, Segment3D two) {
            if (one.epsilonEquivalent(two))
                return false;

            if (one.mA.squaredDistance(two.mA) > 1e-6)
                return new Vector3Comparator()(one.mA, two.mA);
            return new Vector3Comparator()(one.mB, two.mB);
        }
    }
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



    //    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //    //ORIGINAL LINE: class _ProceduralExport Boolean : public MeshGenerator<Boolean>
    //    public class Boolean : MeshGenerator<Boolean>
    //    {
    //        public enum BooleanOperation : int
    //        {
    //            BT_UNION,
    //            BT_INTERSECTION,
    //            BT_DIFFERENCE
    //        }
    //        private BooleanOperation mBooleanOperation;
    //        private TriangleBuffer mMesh1;
    //        private TriangleBuffer mMesh2;

    //        public Boolean() {
    //            mMesh1 = 0;
    //            mMesh2 = 0;
    //            mBooleanOperation = BooleanOperation.BT_UNION;
    //        }

    //        public Boolean setMesh1(ref TriangleBuffer tb) {
    //            mMesh1 = tb;
    //            return this;
    //        }

    //        public Boolean setMesh2(ref TriangleBuffer tb) {
    //            mMesh2 = tb;
    //            return this;
    //        }

    //        public Boolean setBooleanOperation(BooleanOperation op) {
    //            mBooleanOperation = op;
    //            return this;
    //        }

    //        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    //        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const;
    //        //C++ TO C# CONVERTER TODO TASK: The implementation of the following method could not be found:
    //        //	void addToTriangleBuffer(ref TriangleBuffer buffer);


    //        //-----------------------------------------------------------------------

    //    public static Vector2 projectOnAxis(Vector3 input, Vector3 origin, Vector3 axis1, Vector3 axis2)
    //    { 

    //        return new Vector2((input - origin).DotProduct(axis1), (input - origin).DotProduct(axis2));
    //    }
    //    //-----------------------------------------------------------------------

    //    public static Vector3 deprojectOnAxis(Vector2 input, Vector3 origin, Vector3 axis1, Vector3 axis2)
    //    {
    //        return origin + input.x * axis1 + input.y* axis2;
    //    }
    //    //-----------------------------------------------------------------------

    //    public static Segment2D projectOnAxis(Segment3D input, Vector3 origin, Vector3 axis1, Vector3 axis2)
    //    {
    //        return new Segment2D(projectOnAxis(input.mA, origin, axis1, axis2), projectOnAxis(input.mB, origin, axis1, axis2));
    //    }
    //    //-----------------------------------------------------------------------

    //    public static void _removeFromTriLookup(int k, ref std.multimap<Segment3D, int, Seg3Comparator> lookup)
    //    {
    //        for (std.multimap<Segment3D, int, Seg3Comparator>.Enumerator it2 = lookup.begin(); it2 != lookup.end();)
    //        {
    //            std.multimap<Segment3D, int, Seg3Comparator>.Enumerator removeIt = it2++;
    //            if (removeIt.second == k)
    //                lookup.erase(removeIt);
    //        }
    //    }
    //    //-----------------------------------------------------------------------

    //    public static void _recursiveAddNeighbour(ref TriangleBuffer result, TriangleBuffer source, int triNumber, ref std.multimap<Segment3D, int, Seg3Comparator> lookup, std.set<Segment3D, Seg3Comparator> limits, bool inverted)
    //    {
    //        if (triNumber == -1)
    //            return;
    //        Utils.log("tri " + StringConverter.toString(triNumber));
    //        const List<int> ind = source.getIndices();
    //        const List<TriangleBuffer.Vertex> vec = source.getVertices();
    //        result.rebaseOffset();
    //        if (inverted)
    //        {
    //            result.triangle(0, 2, 1);
    //            TriangleBuffer.Vertex v = vec[ind[triNumber * 3]];
    //            v.mNormal = -v.mNormal;
    //            result.vertex(v);
    //            v = vec[ind[triNumber * 3+1]];
    //            v.mNormal = -v.mNormal;
    //            result.vertex(v);
    //            v = vec[ind[triNumber * 3+2]];
    //            v.mNormal = -v.mNormal;
    //            result.vertex(v);
    //        }
    //        else
    //        {
    //            result.triangle(0, 1, 2);
    //            result.vertex(vec[ind[triNumber * 3]]);
    //            result.vertex(vec[ind[triNumber * 3 + 1]]);
    //            result.vertex(vec[ind[triNumber * 3 + 2]]);
    //        }

    //        //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3]].mPosition));
    //        //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3+1]].mPosition));
    //        //Utils::log("vertex " + StringConverter::toString(vec[ind[triNumber*3+2]].mPosition));

    //        std.multimap<Segment3D, int, Seg3Comparator>.Enumerator it;

    //        int nextTriangle1 = -1;
    //        int nextTriangle2 = -1;
    //        int nextTriangle3 = -1;
    //        it = lookup.find(Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 1]].mPosition).orderedCopy());
    //        //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
    //        //	Utils::log("Cross limit1");

    //        if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end())
    //        {
    //            nextTriangle1 = it.second;
    //            _removeFromTriLookup(nextTriangle1, ref lookup);
    //        }
    //        it = lookup.find(Segment3D(vec[ind[triNumber * 3 + 1]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
    //        //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
    //        //Utils::log("Cross limit2");

    //        if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end())
    //        {
    //            nextTriangle2 = it.second;
    //            _removeFromTriLookup(nextTriangle2, ref lookup);
    //        }
    //        it = lookup.find(Segment3D(vec[ind[triNumber * 3]].mPosition, vec[ind[triNumber * 3 + 2]].mPosition).orderedCopy());
    //        //if (it != lookup.end() && limits.find(it->first.orderedCopy()) != limits.end())
    //        //	Utils::log("Cross limit3");
    //        if (it != lookup.end() && limits.find(it.first.orderedCopy()) == limits.end())
    //        {
    //            nextTriangle3 = it.second;
    //            _removeFromTriLookup(nextTriangle3, ref lookup);
    //        }
    //        //Utils::log("add " + StringConverter::toString(nextTriangle1) + " ," + StringConverter::toString(nextTriangle2) + " ,"+StringConverter::toString(nextTriangle3) );

    //        _recursiveAddNeighbour(ref result, source, nextTriangle1, ref lookup, limits, inverted);
    //        _recursiveAddNeighbour(ref result, source, nextTriangle2, ref lookup, limits, inverted);
    //        _recursiveAddNeighbour(ref result, source, nextTriangle3, ref lookup, limits, inverted);
    //    }
    //    //-----------------------------------------------------------------------

    //    public static void _retriangulate(ref TriangleBuffer newMesh, TriangleBuffer inputMesh, List<Intersect> intersectionList, bool first)
    //    {
    //        const List<TriangleBuffer.Vertex> vec = inputMesh.getVertices();
    //        const List<int> ind = inputMesh.getIndices();
    //        // Triangulate
    //        //  Group intersections by triangle indice
    //        std.map<int, List<Segment3D> > meshIntersects = new std.map<int, List<Segment3D> >();
    //        for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext(); ++it)
    //        {
    //            std.map<int, List<Segment3D> >.Enumerator it2;
    //            if (first)
    //                it2 = meshIntersects.find(it.mTri1);
    //            else
    //                it2 = meshIntersects.find(it.mTri2);
    //            if (it2 != meshIntersects.end())
    //                it2.second.push_back(it.mSeg);
    //            else
    //            {
    //                List<Segment3D> vec = new List<Segment3D>();
    //                vec.Add(it.mSeg);
    //                if (first)
    //                    meshIntersects[it.mTri1] = vec;
    //                else
    //                    meshIntersects[it.mTri2] = vec;
    //            }
    //        }
    //        // Build a new TriangleBuffer holding non-intersected triangles and retriangulated-intersected triangles
    //        for (List<TriangleBuffer.Vertex>.Enumerator it = vec.GetEnumerator(); it.MoveNext(); ++it)
    //            newMesh.vertex(it.Current);
    //        for (int i = 0; i < (int)ind.Count / 3; i++)
    //            if (meshIntersects.find(i) == meshIntersects.end())
    //                newMesh.triangle(ind[i * 3], ind[i * 3 + 1], ind[i * 3 + 2]);
    //        int numNonIntersected1 = newMesh.getIndices().size();
    //        for (std.map<int, List<Segment3D> >.Enumerator it = meshIntersects.begin(); it.MoveNext(); ++it)
    //        {
    //            List<Segment3D> segments = it.second;
    //            int triIndex = it.first;
    //            Vector3 v1 = vec[ind[triIndex * 3]].mPosition;
    //            Vector3 v2 = vec[ind[triIndex * 3+1]].mPosition;
    //            Vector3 v3 = vec[ind[triIndex * 3+2]].mPosition;
    //            Vector3 triNormal = ((v2-v1).crossProduct(v3-v1)).normalisedCopy();
    //            Vector3 xAxis = triNormal.perpendicular();
    //            Vector3 yAxis = triNormal.crossProduct(xAxis);
    //            Vector3 planeOrigin = vec[ind[triIndex * 3]].mPosition;

    //            // Project intersection segments onto triangle plane
    //            List<Segment2D> segments2 = new List<Segment2D>();

    //            for (List<Segment3D>.Enumerator it2 = segments.GetEnumerator(); it2.MoveNext(); it2++)
    //                segments2.Add(projectOnAxis(it2.Current, planeOrigin, xAxis, yAxis));
    //            for (List<Segment2D>.Enumerator it2 = segments2.GetEnumerator(); it2.MoveNext();)
    //                if ((it2.mA - it2.mB).squaredLength() < 1e-5)
    ////C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
    //                    it2 = segments2.erase(it2);
    //                else

    //            // Triangulate
    //            Triangulator t = new Triangulator();
    //            Triangle2D[[]] tri = new Triangle2D[ind[triIndex * 3]](projectOnAxis(vec.mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 1]].mPosition, planeOrigin, xAxis, yAxis), projectOnAxis(vec[ind[triIndex * 3 + 2]].mPosition, planeOrigin, xAxis, yAxis));
    //            PointList outPointList = new PointList();
    //            List<int> outIndice = new List<int>();
    //            t.setManualSuperTriangle(tri).setRemoveOutside(false).setSegmentListToTriangulate(segments2).triangulate(outIndice, outPointList);

    //            // Deproject and add to triangleBuffer
    //            newMesh.rebaseOffset();
    //            for (List<int>.Enumerator it = outIndice.GetEnumerator(); it.MoveNext(); ++it)
    //                newMesh.index(it.Current);
    //            float x1 = tri.mPoints[0].x;
    //            float y1 = tri.mPoints[0].y;
    //            Vector2 uv1 = vec[ind[triIndex * 3]].mUV;
    //            float x2 = tri.mPoints[1].x;
    //            float y2 = tri.mPoints[1].y;
    //            Vector2 uv2 = vec[ind[triIndex * 3 + 1]].mUV;
    //            float x3 = tri.mPoints[2].x;
    //            float y3 = tri.mPoints[2].y;
    //            Vector2 uv3 = vec[ind[triIndex * 3 + 2]].mUV;
    //            float DET = x1 * y2 - x2 * y1 + x2 * y3 - x3 * y2 + x3 * y1 - x1 *y3;
    //            Vector2 A = ((y2 - y3) * uv1 + (y3 - y1) * uv2 + (y1 - y2) * uv3) / DET;
    //            Vector2 B = ((x3 - x2) * uv1 + (x1 - x3) * uv2 + (x2 - x1) * uv3) / DET;
    //            Vector2 C = ((x2 * y3 - x3 * y2) * uv1 + (x3 * y1 - x1 * y3) * uv2 + (x1 * y2 - x2 * y1) * uv3) / DET;

    //            for (List<Vector2>.Enumerator it = outPointList.begin(); it.MoveNext(); ++it)
    //            {
    //                Vector2 uv = A * it.x + B * it.y + C;
    //                newMesh.position(deprojectOnAxis(it.Current, planeOrigin, xAxis, yAxis));
    //                newMesh.normal(triNormal);
    //                newMesh.textureCoord(uv);
    //            }
    //        }
    //    }
    //    //-----------------------------------------------------------------------

    //    public static void _buildTriLookup(ref std.multimap<Segment3D, int, Seg3Comparator> lookup, TriangleBuffer newMesh)
    //    {
    //        const List<TriangleBuffer.Vertex> nvec = newMesh.getVertices();
    //        const List<int> nind = newMesh.getIndices();
    //        for (int i = 0; i < (int)nind.Count / 3; i++)
    //        {
    //            lookup.insert(std.pair<Segment3D, int>(Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 1]].mPosition).orderedCopy(), i));
    //            lookup.insert(std.pair<Segment3D, int>(Segment3D(nvec[nind[i * 3]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
    //            lookup.insert(std.pair<Segment3D, int>(Segment3D(nvec[nind[i * 3 + 1]].mPosition, nvec[nind[i * 3 + 2]].mPosition).orderedCopy(), i));
    //        }
    //    }
    //    //-----------------------------------------------------------------------

    ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    ////ORIGINAL LINE: void Boolean::addToTriangleBuffer(TriangleBuffer& buffer) const
    //    public static void addToTriangleBuffer(ref TriangleBuffer buffer)
    //    {
    //         List<TriangleBuffer.Vertex> vec1 = mMesh1.getVertices();
    //         List<int> ind1 = mMesh1.getIndices();
    //         List<TriangleBuffer.Vertex> vec2 = mMesh2.getVertices();
    //         List<int> ind2 = mMesh2.getIndices();
    //        Segment3D intersectionResult = new Segment3D();

    //        List<Intersect> intersectionList = new List<Intersect>();

    //        // Find all intersections between mMesh1 and mMesh2
    //        int idx1 = 0;
    //        for (List<int>.Enumerator it = ind1.GetEnumerator(); it.MoveNext(); idx1++)
    //        {
    //            Triangle3D[] t1 = new Triangle3D[it.Current++](vec1.mPosition, vec1[it.Current++].mPosition, vec1[it.Current++].mPosition);

    //            int idx2 = 0;
    //            for (List<int>.Enumerator it2 = ind2.GetEnumerator(); it2.MoveNext(); idx2++)
    //            {
    //                Triangle3D[] t2 = new Triangle3D[it2.Current++](vec2.mPosition, vec2[it2.Current++].mPosition, vec2[it2.Current++].mPosition);

    //                if (t1.findIntersect(t2, intersectionResult))
    //                {
    //                    Intersect intersect = new Intersect(intersectionResult, idx1, idx2);
    //                    intersectionList.Add(intersect);
    //                }
    //            }
    //        }
    //        // Remove all intersection segments too small to be relevant
    //        for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext();)
    //            if ((it.mSeg.mB - it.mSeg.mA).squaredLength() < 1e-8)
    ////C++ TO C# CONVERTER TODO TASK: There is no direct equivalent to the STL vector 'erase' method in C#:
    //                it = intersectionList.erase(it);//移除
    //            else

    //        // Retriangulate
    //        TriangleBuffer newMesh1 = new TriangleBuffer();
    //        TriangleBuffer newMesh2 = new TriangleBuffer();
    //        _retriangulate(ref newMesh1, *mMesh1, intersectionList, true);
    //        _retriangulate(ref newMesh2, *mMesh2, intersectionList, false);

    //        //buffer.append(newMesh1);
    //        //buffer.append(newMesh2);
    //        //return;

    //        // Trace contours
    //        List<Path> contours = new List<Path>();
    //        List<Segment3D> segmentSoup = new List<Segment3D>();
    //        for (List<Intersect>.Enumerator it = intersectionList.GetEnumerator(); it.MoveNext(); ++it)
    //            segmentSoup.Add(it.mSeg);
    //        Path().buildFromSegmentSoup(segmentSoup, contours);

    //        // Build a lookup from segment to triangle
    //        std.multimap<Segment3D, int, Seg3Comparator> triLookup1 = new std.multimap<Segment3D, int, Seg3Comparator>();
    //        std.multimap<Segment3D, int, Seg3Comparator> triLookup2 = new std.multimap<Segment3D, int, Seg3Comparator>();
    //        _buildTriLookup(ref triLookup1, newMesh1);
    //        _buildTriLookup(ref triLookup2, newMesh2);

    //        std.set<Segment3D, Seg3Comparator> limits = new std.set<Segment3D, Seg3Comparator>();
    //        for (List<Segment3D>.Enumerator it = segmentSoup.GetEnumerator(); it.MoveNext(); ++it)
    //            limits.insert(it.orderedCopy());
    //        // Build resulting mesh
    //        for (List<Path>.Enumerator it = contours.GetEnumerator(); it.MoveNext(); ++it)
    //        {
    //            // Find 2 seed triangles for each contour
    //            Segment3D firstSeg = new Segment3D(it.getPoint(0), it.getPoint(1));

    //            std.pair<std.multimap<Segment3D, int, Seg3Comparator>.Enumerator, std.multimap<Segment3D, int, Seg3Comparator>.Enumerator> it2mesh1 = triLookup1.equal_range(firstSeg.orderedCopy());
    //            std.pair<std.multimap<Segment3D, int, Seg3Comparator>.Enumerator, std.multimap<Segment3D, int, Seg3Comparator>.Enumerator> it2mesh2 = triLookup2.equal_range(firstSeg.orderedCopy());
    //            int mesh1seed1;
    //            int mesh1seed2;
    //            int mesh2seed1;
    //            int mesh2seed2;

    //            if (it2mesh1.first != triLookup1.end() && it2mesh2.first != triLookup2.end())
    //            {
    //                // check which of seed1 and seed2 must be included (it can be 0, 1 or both)
    //                mesh1seed1 = it2mesh1.first.second;
    //                mesh1seed2 = (--it2mesh1.second).second;
    //                mesh2seed1 = it2mesh2.first.second;
    //                mesh2seed2 = (--it2mesh2.second).second;
    //                if (mesh1seed1 == mesh1seed2)
    //                    mesh1seed2 = -1;
    //                if (mesh2seed1 == mesh2seed2)
    //                    mesh2seed2 = -1;

    //                Vector3 vMesh1 = new Vector3();
    //                Vector3 nMesh1 = new Vector3();
    //                Vector3 vMesh2 = new Vector3();
    //                Vector3 nMesh2 = new Vector3();
    //                for (int i =0; i<3; i++)
    //                {
    //                    const Vector3 pos = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mPosition;
    //                    if (pos.squaredDistance(firstSeg.mA)>1e-6 && pos.squaredDistance(firstSeg.mB)>1e-6)
    //                    {
    //                        vMesh1 = pos;
    //                        nMesh1 = newMesh1.getVertices()[newMesh1.getIndices()[mesh1seed1 * 3 + i]].mNormal;
    //                        break;
    //                    }
    //                }

    //                for (int i =0; i<3; i++)
    //                {
    //                    const Vector3 pos = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mPosition;
    //                    if (pos.squaredDistance(firstSeg.mA)>1e-6 && pos.squaredDistance(firstSeg.mB)>1e-6)
    //                    {
    //                        vMesh2 = pos;
    //                        nMesh2 = newMesh2.getVertices()[newMesh2.getIndices()[mesh2seed1 * 3 + i]].mNormal;
    //                        break;
    //                    }
    //                }

    //                bool M2S1InsideM1 = (nMesh1.dotProduct(vMesh2-firstSeg.mA) < 0);
    //                bool M1S1InsideM2 = (nMesh2.dotProduct(vMesh1-firstSeg.mA) < 0);

    //                _removeFromTriLookup(mesh1seed1, ref triLookup1);
    //                _removeFromTriLookup(mesh2seed1, ref triLookup2);
    //                _removeFromTriLookup(mesh1seed2, ref triLookup1);
    //                _removeFromTriLookup(mesh2seed2, ref triLookup2);

    //                // Recursively add all neighbours of these triangles
    //                // Stop when a contour is touched
    //                switch (mBooleanOperation)
    //                {
    //                case BT_UNION:
    //                    if (M1S1InsideM2)
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
    //                    if (M2S1InsideM1)
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, false);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, false);
    //                    break;
    //                case BT_INTERSECTION:
    //                    if (M1S1InsideM2)
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
    //                    if (M2S1InsideM1)
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, false);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, false);
    //                    break;
    //                case BT_DIFFERENCE:
    //                    if (M1S1InsideM2)
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed2, ref triLookup1, limits, false);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh1, mesh1seed1, ref triLookup1, limits, false);
    //                    if (M2S1InsideM1)
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed1, ref triLookup2, limits, true);
    //                    else
    //                        _recursiveAddNeighbour(ref buffer, newMesh2, mesh2seed2, ref triLookup2, limits, true);
    //                    break;
    //                }
    //            }
    //        }
    //    }
    //    }

    //public class Intersect
    //{
    //    public Segment3D mSeg = new Segment3D();
    //    public int mTri1;
    //    public int mTri2;

    //    public Intersect(Segment3D seg, int tri1, int tri2)
    //    {
    //        mSeg = seg;
    //        mTri1 = tri1;
    //        mTri2 = tri2;
    //    }
    //}
    ////-----------------------------------------------------------------------

    //public class Seg3Comparator
    //{

    ////C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
    ////ORIGINAL LINE: bool operator ()(const Segment3D& one, const Segment3D& two) const
    ////C++ TO C# CONVERTER TODO TASK: The () operator cannot be overloaded in C#:
    //    public static bool operator ==(Segment3D one, Segment3D two)
    //    {
    //        if (one.epsilonEquivalent(two))
    //            return false;

    //        if (one.mA.squaredDistance(two.mA) > 1e-6)
    //            return Vector3Comparator()(one.mA, two.mA);
    //        return Vector3Comparator()(one.mB, two.mB);
    //    }
    //}


}
