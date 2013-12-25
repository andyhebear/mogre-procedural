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
namespace Mogre_Procedural{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mogre;
    /// <summary>
    /// mesh数据 内部根据参数不做处理或者自动销毁
    /// 当autodispose_meshPtr=true,MeshPtr参数在初始化完毕之后就销毁。这里需要注意
    /// 当autodispose_meshPtr=false, 传进的MeshPtr参数需外面管理销毁以防止.net自动回收该指针造成崩溃
    /// </summary>  
    public class OgreMeshData : IDisposable {
        // SGD ：2013/6/20 9:31:35
        // 说明：建立meshdata缓存?

        public string meshName;
        public Mogre.RenderOperation.OperationTypes  opType;
        private Vector3[] _vertices;
        private uint[] _indices;
        private Vector2[] _textureCroodnitas;
        private Vector3 _scale = Vector3.UNIT_SCALE;
        private Quaternion _quaoffset = Quaternion.IDENTITY;
        private Vector3 _posoffset = Vector3.ZERO;
        /// <summary>
        /// key submesh, keyvaluepair(start,end)
        /// </summary>
        private Dictionary<uint, KeyValuePair<uint, uint>> _index_Submesh;

        private bool _readTextureCoordinate = false;
        //private MeshPtr _meshPtr;


        public Vector3[] Vertices {
            get {
                return this._vertices;
            }
        }

        public uint[] Indices {
            get {
                return this._indices;
            }
        }

        public int TriangleCount {
            get {
                return this._indices.Length / 3;
            }
        }

        public Vector2[] _TextureCoordnitas {
            get { return _textureCroodnitas; }
        }
        /// <summary>
        /// 通过朝向与坐标变换
        /// </summary>
        /// <param name="originalVextex"></param>
        /// <returns></returns>
        public static Vector3 GetVextexTrans(Vector3 originalVextex, Quaternion euler, Vector3 posOffset) {
            //(orientation * (pt * scale)) + position
            return GetVextexTrans(originalVextex, euler, Vector3.UNIT_SCALE, posOffset);
        }
        public static Vector3 GetVextexTrans(Vector3 originalVextex, Quaternion euler, Vector3 scale, Vector3 posOffset) {
            //(orientation * (pt * scale)) + position
            Vector3 renderpos = (euler * (originalVextex * scale)) + posOffset;
            return renderpos;
        }
        //
        public float[] _getPoints() {
            //get {
            // extract the points out of the vertices
            float[] points = new float[this.Vertices.Length * 3];
            int i = 0;

            foreach (Vector3 vertex in this.Vertices) {
                points[i + 0] = vertex.x;
                points[i + 1] = vertex.y;
                points[i + 2] = vertex.z;
                i += 3;
            }

            return points;
            //}
        }
        /// <summary>
        /// 根据索引数组中的位置信息取得属于第几个submesh
        /// </summary>
        /// <param name="Indices_Index">索引数组中的位置</param>
        /// <returns></returns>
        public uint getSubMesh(uint Indices_Index) {
            uint sub = 0;
            foreach (var v in this._index_Submesh) {
                if (Indices_Index >= v.Value.Key && Indices_Index <= v.Value.Value) {
                    return v.Key;
                    break;
                }
                sub++;
            }
            return sub;
        }

        #region OgreMeshData
        public OgreMeshData(MeshPtr meshPtr, bool autodispose_meshPtr) {
            Initiliase(meshPtr, autodispose_meshPtr,Vector3.UNIT_SCALE);
        }

        public OgreMeshData(MeshPtr meshPtr,bool autodispose_meshPtr, float unitScale) {
            Initiliase(meshPtr,autodispose_meshPtr, Vector3.UNIT_SCALE * unitScale);
        }

        public OgreMeshData(MeshPtr meshPtr, bool autodispose_meshPtr, Vector3 scale) {
            Initiliase(meshPtr,autodispose_meshPtr, scale);
        }
        public OgreMeshData(MeshPtr meshPtr, bool autodispose_meshPtr, Vector3 scale, bool readtexturecoord) {
            Initiliase(meshPtr,autodispose_meshPtr, scale, this._quaoffset,this._posoffset, readtexturecoord);
        }
        public OgreMeshData(MeshPtr meshPtr, bool autodispose_meshPtr, Vector3 scale, Quaternion qua, Vector3 pos, bool readtexturecoord) {
            Initiliase(meshPtr,autodispose_meshPtr, scale, qua,pos, readtexturecoord);
        }

        #endregion

        #region read
        private void Initiliase(MeshPtr meshPtr, bool autodispose_meshPtr, Vector3 scale) {
            Initiliase(meshPtr,autodispose_meshPtr, scale, Quaternion.IDENTITY,Vector3.ZERO,false);
        }
        private void Initiliase(MeshPtr meshPtr, bool autodispose_meshPtr, Vector3 scale, Quaternion qua, Vector3 pos, bool readtextcroodnitas) {
            this._scale = scale;
            //this._meshPtr = meshPtr;
            this.meshName = meshPtr.Name;
            this.opType = meshPtr.GetSubMesh(0).operationType;
            this._readTextureCoordinate = readtextcroodnitas;
            this._quaoffset = qua;
            this._posoffset = pos;
            PrepareBuffers(meshPtr);
            ReadData(meshPtr);
            if (autodispose_meshPtr) {
                meshPtr.Dispose();
                meshPtr = null;
            }
        }

        private void PrepareBuffers(MeshPtr _meshPtr) {
            uint indexCount = 0;
            uint vertexCount = 0;

            // Add any shared vertices
            if (_meshPtr.sharedVertexData != null)
                vertexCount = _meshPtr.sharedVertexData.vertexCount;

            // Calculate the number of vertices and indices in the sub meshes
            for (ushort i = 0; i < _meshPtr.NumSubMeshes; i++) {
                SubMesh subMesh = _meshPtr.GetSubMesh(i);

                // we have already counted the vertices that are shared
                if (subMesh.useSharedVertices == false)
                    vertexCount += subMesh.vertexData.vertexCount;

                indexCount += subMesh.indexData.indexCount;
            }

            // Allocate space for the vertices and indices
            _vertices = new Vector3[vertexCount];
            _indices = new uint[indexCount];
            _index_Submesh = new Dictionary<uint, KeyValuePair<uint, uint>>();
            if (_readTextureCoordinate) {
                _textureCroodnitas = new Vector2[vertexCount];
            }
        }


        private void ReadData(MeshPtr _meshPtr) {
            int indexOffset = 0;
            uint vertexOffset = 0;

            // read the index and vertex data from the mesh
            for (ushort i = 0; i < _meshPtr.NumSubMeshes; i++) {
                SubMesh subMesh = _meshPtr.GetSubMesh(i);
                //index
                int indexOffset2 = ReadIndexData(indexOffset, vertexOffset, subMesh.indexData);
                _index_Submesh.Add(i, new KeyValuePair<uint, uint>((uint)indexOffset, (uint)indexOffset2));
                indexOffset = indexOffset2;
                //vertex
                if (subMesh.useSharedVertices == false) {
                    vertexOffset = ReadVertexData(vertexOffset, subMesh.vertexData);
                }
            }

            // add the shared vertex data
            if (_meshPtr.sharedVertexData != null) {
                vertexOffset = ReadVertexData(vertexOffset, _meshPtr.sharedVertexData);
            }
        }


        //增加读取纹理坐标
        private unsafe uint ReadVertexData(uint vertexOffset, VertexData vertexData) {
            VertexElement posElem = vertexData.vertexDeclaration.FindElementBySemantic(VertexElementSemantic.VES_POSITION);
            HardwareVertexBufferSharedPtr vertexBuffer = vertexData.vertexBufferBinding.GetBuffer(posElem.Source);
            byte* vertexMemory = (byte*)vertexBuffer.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
            float* pElem;
            bool needrecomputeVertex = false;
            if (this._quaoffset != Quaternion.IDENTITY || this._posoffset != Vector3.ZERO) {
                needrecomputeVertex = true;
            }
            //texture          
            if (_readTextureCoordinate) {
                ReadTextureCoordData(vertexOffset, vertexMemory, vertexBuffer.VertexSize, vertexData);
            }

            if (needrecomputeVertex) {
                for (uint i = 0; i < vertexData.vertexCount; i++) {
                    posElem.BaseVertexPointerToElement(vertexMemory, &pElem);
                    Vector3 point = new Vector3(pElem[0], pElem[1], pElem[2]);
                    // vertices[current_offset + j] = (orientation * (pt * scale)) + position;
                    _vertices[vertexOffset] = GetVextexTrans(point, this._quaoffset, this._scale, this._posoffset); //point * this.scale;
                    vertexMemory += vertexBuffer.VertexSize;
                    vertexOffset++;
                }
            }
            else {
                for (uint i = 0; i < vertexData.vertexCount; i++) {
                    posElem.BaseVertexPointerToElement(vertexMemory, &pElem);
                    Vector3 point = new Vector3(pElem[0], pElem[1], pElem[2]);
                    _vertices[vertexOffset] = point * this._scale;
                    vertexMemory += vertexBuffer.VertexSize;
                    vertexOffset++;
                }
            }

            vertexBuffer.Unlock();
            // SGD ：2013/6/8 13:43:51
            // 说明：销毁指针引用 防止.NET回收
            vertexBuffer.Dispose();
            return vertexOffset;
        }
        // SGD ：2013/6/18 15:02:51
        // 说明：顶点纹理坐标
        private unsafe void ReadTextureCoordData(uint vertexOffset, byte* vertexBuffer_Lock, uint vertexBuffer_VertexSize, VertexData vertexData) {
            VertexElement texElem = vertexData.vertexDeclaration.FindElementBySemantic(VertexElementSemantic.VES_TEXTURE_COORDINATES);
            float* tReal;
            if (texElem == null) {
                for (uint i = 0; i < vertexData.vertexCount; i++) {
                    this._textureCroodnitas[vertexOffset + i] = new Vector2(0f, 0f);
                }
            }
            else {
                //byte* vextex = vertexBuffer_Lock;
                //for (uint i = 0; i < vertexData.vertexCount; i++) {
                //    texElem.BaseVertexPointerToElement(vextex, &tReal);
                //    this._textureCroodnitas[vertexOffset + i] = new Vector2(tReal[0], tReal[1]);
                //    vextex += vertexBuffer_VertexSize;
                //}
                //读取纹理坐标修正 2013/12/18
                 VertexData vertex_data=vertexData;
                HardwareVertexBufferSharedPtr vbufPtr_tex = vertex_data.vertexBufferBinding.GetBuffer(texElem.Source);
                byte* vertex_tex = (byte*)vbufPtr_tex.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY);
                for (uint i = 0; i < vertex_data.vertexCount; ++i, /*vertex += vbufPtr.VertexSize,*/ vertex_tex += vbufPtr_tex.VertexSize) {
                    //posElem.BaseVertexPointerToElement(vertex, &pReal);
                    texElem.BaseVertexPointerToElement(vertex_tex, &tReal);
                    this._textureCroodnitas[vertexOffset + i] = new Vector2(tReal[0], tReal[1]);
                    vertex_tex += vertexBuffer_VertexSize;
                    //Vector3 pt = new Vector3(pReal[0], pReal[1], pReal[2]);
                    //vertices[current_offset + j] = (orientation * (pt * scale)) + position;
                    //tex_Cors[current_offset + j] = new Vector2(tReal[0], tReal[1]);
                }
                vbufPtr_tex.Unlock();
                vbufPtr_tex.Dispose();
            }

        }
        private unsafe int ReadIndexData(int indexOffset, uint vertexOffset, IndexData indexData) {

            // get index data
            HardwareIndexBufferSharedPtr indexBuf = indexData.indexBuffer;
            HardwareIndexBuffer.IndexType indexType = indexBuf.Type;
            uint* pLong = (uint*)(indexBuf.Lock(HardwareBuffer.LockOptions.HBL_READ_ONLY));
            ushort* pShort = (ushort*)pLong;

            for (uint i = 0; i < indexData.indexCount; i++) {
                if (indexType == HardwareIndexBuffer.IndexType.IT_32BIT)
                    _indices[indexOffset] = pLong[i] + vertexOffset;
                else
                    _indices[indexOffset] = pShort[i] + vertexOffset;

                indexOffset++;
            }

            indexBuf.Unlock();
            // SGD ：2013/6/8 13:42:42
            // 说明：销毁指针引用  防止.NET回收
            indexBuf.Dispose();
            return indexOffset;
        }
        #endregion

        #region IDisposable 成员
        public bool IsDisposed { get { return disposedValue; } }
        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!this.disposedValue) {
                if (!disposing) {
                    Console.WriteLine("~StaticMeshData()");
                }
                this._indices = null;
                this._vertices = null;
                this._index_Submesh.Clear();
                this._index_Submesh = null;
                this._textureCroodnitas = null;
                // SGD ：2013/6/8 13:46:08
                // 说明：销毁指针引用 防止.NET回收
                //this._meshPtr.Dispose();
                //this._meshPtr = null;
            }
            this.disposedValue = true;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~OgreMeshData() {
            Dispose(false);
        }
        #endregion

    }


}
