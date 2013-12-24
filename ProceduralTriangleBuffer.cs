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
    //* This is ogre-procedural's temporary mesh buffer.
    // * It stores all the info needed to build an Ogre Mesh, yet is intented to be more flexible, since
    // * there is no link towards hardware.
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport TriangleBuffer
    public class TriangleBuffer
    {
        public class Vertex
        {
            public Vector3 mPosition = new Vector3();
            public Vector3 mNormal = new Vector3();
            public Vector2 mUV = new Vector2();
        }

        protected List<int> mIndices = new List<int>();

        protected List<Vertex> mVertices = new List<Vertex>();
        //std::vector<Vertex>::iterator mCurrentVertex;
        protected int globalOffset;
        protected int mEstimatedVertexCount;
        protected int mEstimatedIndexCount;
        protected Vertex mCurrentVertex;


        public TriangleBuffer() {
            globalOffset = 0;
            mEstimatedVertexCount = 0;
            mEstimatedIndexCount = 0;
            mCurrentVertex = null;
        }
        public void append(TriangleBuffer other) {
            rebaseOffset();
            foreach (var it in other.mIndices) {
                mIndices.Add(globalOffset + it);
            }
            foreach (var it in other.mVertices) {
                mVertices.Add(it);
            }
        }

        /// Gets a modifiable reference to vertices
        public List<Vertex> getVertices() {
            return mVertices;
        }

        /// Gets a non-modifiable reference to vertices
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const List<Vertex>& getVertices() const
        public Vertex[] _getVertices() {
            return mVertices.ToArray();
        }

        /// Gets a modifiable reference to vertices
        public List<int> getIndices() {
            return mIndices;
        }

        /// Gets a non-modifiable reference to indices
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: const List<int>& getIndices() const
        public int[] _getIndices() {
            return mIndices.ToArray();
        }

        //    *
        //	 * Rebase index offset : call that function before you add a new mesh to the triangle buffer
        //	 
        public void rebaseOffset() {
            globalOffset = mVertices.Count;
        }


        //    *
        //	 * Builds an Ogre Mesh from this buffer.
        //	 
        public MeshPtr transformToMesh(string name) {
            return transformToMesh(name, "General");
        }

        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: Ogre::MeshPtr transformToMesh(const string& name, const Ogre::String& group = "General") const
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        public MeshPtr transformToMesh(string name, string group) {
            Mogre.SceneManager sceneMgr = Root.Singleton.GetSceneManagerIterator().Current;
            Mogre.ManualObject manual = sceneMgr.CreateManualObject(name);
            manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

            foreach (var it in mVertices) {
                manual.Position(it.mPosition);
                manual.TextureCoord(it.mUV);
                manual.Normal(it.mNormal);
            }
            foreach (var it in mIndices) {
                manual.Index((ushort)it);
            }
            manual.End();
            Mogre.MeshPtr mesh = manual.ConvertToMesh(name, group);

            sceneMgr.DestroyManualObject(manual);

            return mesh;
        }

        //* Adds a new vertex to the buffer 
        public TriangleBuffer vertex(Vertex v) {
            mVertices.Add(v);
            mCurrentVertex = mVertices[mVertices.Count - 1];
            return this;
        }

        //* Adds a new vertex to the buffer 
        public TriangleBuffer vertex(Vector3 position, Vector3 normal, Vector2 uv) {
            Vertex v = new Vertex();
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: v.mPosition = position;
            v.mPosition = (position);
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: v.mNormal = normal;
            v.mNormal = (normal);
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: v.mUV = uv;
            v.mUV = (uv);
            mVertices.Add(v);
            mCurrentVertex = mVertices[mVertices.Count - 1];
            return this;
        }

        //* Adds a new vertex to the buffer 
        public TriangleBuffer position(Vector3 pos) {
            Vertex v = new Vertex();
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: v.mPosition = pos;
            v.mPosition = (pos);
            mVertices.Add(v);
            mCurrentVertex = mVertices[mVertices.Count - 1];
            return this;
        }

        //* Adds a new vertex to the buffer 
        public TriangleBuffer position(float x, float y, float z) {
            Vertex v = new Vertex();
            v.mPosition = new Vector3(x, y, z);
            mVertices.Add(v);
            mCurrentVertex = mVertices[mVertices.Count - 1];
            return this;
        }

        //* Sets the normal of the current vertex 
        public TriangleBuffer normal(Vector3 normal) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mCurrentVertex->mNormal = normal;
            mCurrentVertex.mNormal = (normal);
            return this;
        }

        //* Sets the texture coordinates of the current vertex 
        public TriangleBuffer textureCoord(float u, float v) {
            mCurrentVertex.mUV = new Vector2(u, v);
            return this;
        }

        //* Sets the texture coordinates of the current vertex 
        public TriangleBuffer textureCoord(Vector2 vec) {
            //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy assignment (rather than a reference assignment) - this should be verified and a 'CopyFrom' method should be created if it does not yet exist:
            //ORIGINAL LINE: mCurrentVertex->mUV = vec;
            mCurrentVertex.mUV = (vec);
            return this;
        }

        //    *
        //	 * Adds an index to the index buffer.
        //	 * Index is relative to the latest rebaseOffset().
        //	 
        public TriangleBuffer index(int i) {
            mIndices.Add(globalOffset + i);
            return this;
        }

        //    *
        //	 * Adds a triangle to the index buffer.
        //	 * Index is relative to the latest rebaseOffset().
        //	 
        public TriangleBuffer triangle(int i1, int i2, int i3) {
            mIndices.Add(globalOffset + i1);
            mIndices.Add(globalOffset + i2);
            mIndices.Add(globalOffset + i3);
            return this;
        }

        /// Applies a matrix to transform all vertices inside the triangle buffer
        public TriangleBuffer applyTransform(Matrix4 matrix) {
            foreach (var it in mVertices) {
                it.mPosition = matrix * it.mPosition;
                it.mNormal = matrix * it.mNormal;
                it.mNormal = it.mNormal.NormalisedCopy;
            }
            return this;
        }

        /// Applies the translation immediately to all the points contained in that triangle buffer
        /// @param amount translation vector
        public TriangleBuffer translate(Vector3 amount) {
            foreach (var it in mVertices) {
                it.mPosition += amount;
            }
            return this;
        }

        /// Applies the translation immediately to all the points contained in that triangle buffer
        public TriangleBuffer translate(float x, float y, float z) {
            return translate(new Vector3(x, y, z));
        }

        /// Applies the rotation immediately to all the points contained in that triangle buffer
        /// @param quat the rotation quaternion to apply
        public TriangleBuffer rotate(Quaternion quat) {
            foreach (var it in mVertices) {
                it.mPosition = quat * it.mPosition;
                it.mNormal = quat * it.mNormal;
                it.mNormal = it.mNormal.NormalisedCopy;
            }
            return this;
        }

        /// Applies an immediate scale operation to that triangle buffer
        /// @param scale Scale vector
        public TriangleBuffer scale(Vector3 scale) {
            foreach (var it in mVertices) {
                it.mPosition = scale * it.mPosition;
            }
            return this;
        }

        /// Applies an immediate scale operation to that triangle buffer
        /// @param x X scale component
        /// @param y Y scale component
        /// @param z Z scale component
        public TriangleBuffer scale(float x, float y, float z) {
            return scale(new Vector3(x, y, z));
        }

        /// Applies normal inversion on the triangle buffer
        public TriangleBuffer invertNormals() {
            foreach (var it in mVertices) {
                it.mNormal = -it.mNormal;
            }
            for (int i = 0; i < mIndices.Count; ++i) {
                if (i % 3 == 1) {
                    //std::swap(mIndices[i], mIndices[i-1]);
                    list_swap<int>(mIndices, i, i - 1);
                }
            }
            return this;
        }

        private void list_swap<T>(List<T> list, int index1, int index2) {
            T t = list[index1];
            list[index1] = list[index2];
            list[index2] = t;
        }

        //    *
        //	 * Gives an estimation of the number of vertices need for this triangle buffer.
        //	 * If this function is called several times, it means an extra vertices count, not an absolute measure.
        //	 
        public void estimateVertexCount(uint vertexCount) {
            mEstimatedVertexCount += (int)vertexCount;
            mVertices.Capacity = mEstimatedVertexCount;
        }

        //    *
        //	 * Gives an estimation of the number of indices needed for this triangle buffer.
        //	 * If this function is called several times, it means an extra indices count, not an absolute measure.
        //	 
        public void estimateIndexCount(uint indexCount) {
            mEstimatedIndexCount += (int)indexCount;
            mIndices.Capacity = mEstimatedIndexCount;
        }
    }


    //void TriangleBuffer::importEntity(Entity* entity)
    //	{
    //		bool added_shared = false;
    //	int current_offset = 0;
    //	int shared_offset = 0;
    //	int next_offset = 0;
    //	int index_offset = 0;
    //	int vertex_count = 0;
    //	int index_count = 0;
    //
    //	Ogre::MeshPtr mesh = entity->getMesh();
    //
    //
    //	bool useSoftwareBlendingVertices = entity->hasSkeleton();
    //
    //	if (useSoftwareBlendingVertices)
    //	  entity->_updateAnimation();
    //
    //    // Calculate how many vertices and indices we're going to need
    //	for (unsigned short i = 0; i < mesh->getNumSubMeshes(); ++i)
    //	{
    //		Ogre::SubMesh* submesh = mesh->getSubMesh( i );
    //
    //        // We only need to add the shared vertices once
    //		if(submesh->useSharedVertices)
    //		{
    //			if( !added_shared )
    //			{
    //				vertex_count += mesh->sharedVertexData->vertexCount;
    //				added_shared = true;
    //			}
    //		}
    //		else
    //		{
    //			vertex_count += submesh->vertexData->vertexCount;
    //		}
    //
    //        // Add the indices
    //		index_count += submesh->indexData->indexCount;
    //	}
    //
    //
    //    // Allocate space for the vertices and indices
    //	estimateVertexCount(vertex_count);
    //	estimateIndexCount(index_count);
    //
    //	added_shared = false;
    //
    //    // Run through the submeshes again, adding the data into the arrays
    //	for ( unsigned short i = 0; i < mesh->getNumSubMeshes(); ++i)
    //	{
    //		Ogre::SubMesh* submesh = mesh->getSubMesh(i);
    //
    //        //----------------------------------------------------------------
    //        // GET VERTEXDATA
    //        //----------------------------------------------------------------
    //		Ogre::VertexData* vertex_data;
    //
    //        //When there is animation:
    //		if(useSoftwareBlendingVertices)
    //			vertex_data = submesh->useSharedVertices ? entity->_getSkelAnimVertexData() : entity->getSubEntity(i)->_getSkelAnimVertexData();
    //		else
    //			vertex_data = submesh->useSharedVertices ? mesh->sharedVertexData : submesh->vertexData;
    //
    //
    //		if((!submesh->useSharedVertices)||(submesh->useSharedVertices && !added_shared))
    //		{
    //			if(submesh->useSharedVertices)
    //			{
    //				added_shared = true;
    //				shared_offset = current_offset;
    //			}
    //
    //			const Ogre::VertexElement* posElem =
    //				vertex_data->vertexDeclaration->findElementBySemantic(Ogre::VES_POSITION);
    //
    //			Ogre::HardwareVertexBufferSharedPtr vbuf =
    //				vertex_data->vertexBufferBinding->getBuffer(posElem->getSource());
    //
    //			unsigned char* vertex =
    //				static_cast<unsigned char*>(vbuf->lock(Ogre::HardwareBuffer::HBL_READ_ONLY));
    //
    //            // There is _no_ baseVertexPointerToElement() which takes an Ogre::float or a double
    //            //  as second argument. So make it float, to avoid trouble when Ogre::float will
    //            //  be comiled/typedefed as double:
    //            //      Ogre::Real* pReal;
    //			float* pReal;
    //
    //			for( int j = 0; j < vertex_data->vertexCount; ++j, vertex += vbuf->getVertexSize())
    //			{
    //				posElem->baseVertexPointerToElement(vertex, &pReal);
    //
    //				Ogre::Vector3 pt(pReal[0], pReal[1], pReal[2]);
    //
    //				vertices[current_offset + j] = (orient * (pt * scale)) + position;
    //			}
    //
    //			vbuf->unlock();
    //			next_offset += vertex_data->vertexCount;
    //		}
    //
    //
    //		Ogre::IndexData* index_data = submesh->indexData;
    //		int numTris = index_data->indexCount / 3;
    //		Ogre::HardwareIndexBufferSharedPtr ibuf = index_data->indexBuffer;
    //
    //		bool use32bitindexes = (ibuf->getType() == Ogre::HardwareIndexBuffer::IT_32BIT);
    //
    //		void* hwBuf = ibuf->lock(Ogre::HardwareBuffer::HBL_READ_ONLY);
    //
    //		int offset = (submesh->useSharedVertices)? shared_offset : current_offset;
    //		int index_start = index_data->indexStart;
    //		int last_index = numTris*3 + index_start;
    //
    //		if (use32bitindexes) {
    //			Ogre::uint32* hwBuf32 = static_cast<Ogre::uint32*>(hwBuf);
    //			for (int k = index_start; k < last_index; ++k)
    //			{
    //				indices[index_offset++] = hwBuf32[k] + static_cast<Ogre::uint32>( offset );
    //			}
    //		} else {
    //			Ogre::uint16* hwBuf16 = static_cast<Ogre::uint16*>(hwBuf);
    //			for (int k = index_start; k < last_index; ++k)
    //			{
    //				indices[ index_offset++ ] = static_cast<Ogre::uint32>( hwBuf16[k] ) +
    //					static_cast<Ogre::uint32>( offset );
    //			}
    //		}
    //
    //		ibuf->unlock();
    //		current_offset = next_offset;
    //	}
    //	}
}
