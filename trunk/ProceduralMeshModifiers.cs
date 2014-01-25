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
//#ifndef PROCEDURAL_MESH_MODIFIERS_INCLUDED
#define PROCEDURAL_MESH_MODIFIERS_INCLUDED

//wrapper with new std class...ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using TRect = Mogre.RealRect;
    using Mogre_Procedural.std;
    //*
    // \brief Projects all TriangleBufferVertices on a sphere
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport SpherifyModifier
    public class SpherifyModifier
    {
        private TriangleBuffer mInputTriangleBuffer;
        private Vector3 mCenter = new Vector3();
        private float mRadius = 0f;


        public SpherifyModifier() {
            mInputTriangleBuffer = null;
            mCenter = Vector3.ZERO;
            mRadius = 1f;
        }

        /// \exception Ogre::InvalidParametersException Input triangle buffer must not be null
        public SpherifyModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            if (inputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Input triangle buffer must not be null", "Procedural::SpherifyModifier::setInputTriangleBuffer(Procedural::TriangleBuffer*)", __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must not be null!");
            ;
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        public SpherifyModifier setRadius(float radius) {
            if (mRadius <= 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be positive", "Procedural::SpherifyModifier::setInputTriangleBuffer(Procedural::TriangleBuffer*)", __FILE__, __LINE__);
                throw new Exception("Radius must be larger than 0!");
            ;
            mRadius = radius;
            return this;
        }

        public SpherifyModifier setCenter(Vector3 center) {
            //
            //ORIGINAL LINE: mCenter = center;
            mCenter = (center);
            return this;
        }

        /// \exception Ogre::InvalidStateException Input triangle buffer must be set
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", "Procedural::SpherifyModifier::modify()", __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;

            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                float l = (it.mPosition - mCenter).Length;
                if (l > 1e-6) {
                    it.mNormal = (it.mPosition - mCenter) / l;
                    it.mPosition = mCenter + mRadius * it.mNormal;
                }
            }
        }
    }

    //--------------------------------------------------------------
    //*
    //WIP
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport CalculateNormalsModifier
    public class CalculateNormalsModifier
    {
        public CalculateNormalsModifier() {
            mComputeMode = NormalComputeMode.NCM_VERTEX;
            mInputTriangleBuffer = null;
            mMustWeldUnweldFirst = true;
        }

        public enum NormalComputeMode : int
        {
            NCM_VERTEX,
            NCM_TRIANGLE
        }

        public NormalComputeMode mComputeMode;
        public TriangleBuffer mInputTriangleBuffer;
        public bool mMustWeldUnweldFirst;

        public CalculateNormalsModifier setComputeMode(NormalComputeMode computeMode) {
            mComputeMode = computeMode;
            return this;
        }

        public CalculateNormalsModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        //    *
        //	 * Tells if the mesh must be first weld (NCM_VERTEX mode) or unweld (NCM_TRIANGLE) before computing normals.
        //	 * Has a performance impact if enabled.
        //	 * Default : true.
        //	 
        public CalculateNormalsModifier setMustWeldUnweldFirst(bool mustWeldUnweldFirst) {
            mMustWeldUnweldFirst = mustWeldUnweldFirst;
            return this;
        }

        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;

            if (mComputeMode == NormalComputeMode.NCM_TRIANGLE) {
                if (mMustWeldUnweldFirst)
                    new WeldVerticesModifier.UnweldVerticesModifier().setInputTriangleBuffer(mInputTriangleBuffer).modify();

                std_vector<int> indices = mInputTriangleBuffer.getIndices();
                std_vector<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
                for (int i = 0; i < indices.size(); i += 3) {
                    Vector3 v1 = vertices[indices[i]].mPosition;
                    Vector3 v2 = vertices[indices[i + 1]].mPosition;
                    Vector3 v3 = vertices[indices[i + 2]].mPosition;
                    Vector3 n = (v2 - v1).CrossProduct(v3 - v1).NormalisedCopy;
                    //
                    //ORIGINAL LINE: vertices[indices[i]].mNormal = n;
                    vertices[indices[i]].mNormal = (n);
                    //
                    //ORIGINAL LINE: vertices[indices[i+1]].mNormal = n;
                    vertices[indices[i + 1]].mNormal = (n);
                    //
                    //ORIGINAL LINE: vertices[indices[i+2]].mNormal = n;
                    vertices[indices[i + 2]].mNormal = (n);
                }
            }
            else {
                if (mMustWeldUnweldFirst)
                    new WeldVerticesModifier().setInputTriangleBuffer(mInputTriangleBuffer).modify();
                std_vector<int> indices = mInputTriangleBuffer.getIndices();
                std_vector<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
                std_vector<std_vector<Vector3>> tmpNormals = new std_vector<std_vector<Vector3>>();
                tmpNormals.resize(vertices.size());
                for (int i = 0; i < indices.size(); i += 3) {
                    Vector3 v1 = vertices[indices[i]].mPosition;
                    Vector3 v2 = vertices[indices[i + 1]].mPosition;
                    Vector3 v3 = vertices[indices[i + 2]].mPosition;
                    Vector3 n = (v2 - v1).CrossProduct(v3 - v1);
                    tmpNormals[indices[i]].Add(n);
                    tmpNormals[indices[i + 1]].Add(n);
                    tmpNormals[indices[i + 2]].Add(n);
                }
                for (int i = 0; i < vertices.size(); i++) {
                    Vector3 n = (Vector3.ZERO);
                    for (int j = 0; j < tmpNormals[i].Count; j++)
                        n += tmpNormals[i][j];
                    vertices[i].mNormal = n.NormalisedCopy;
                }
            }
        }
    }
    //--------------------------------------------------------------
    //*
    // * Welds together the vertices which are 'close enough' one to each other
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport WeldVerticesModifier
    public class WeldVerticesModifier
    {
        public WeldVerticesModifier() {
            mInputTriangleBuffer = null;
            mTolerance = 1e-3f;
        }

        public TriangleBuffer mInputTriangleBuffer;
        public float mTolerance = 0f;


        /// The triangle buffer to modify
        public WeldVerticesModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        /// The tolerance in position to consider that 2 vertices are the same (default = 1e-3)
        public WeldVerticesModifier setTolerance(float tolerance) {
            mTolerance = tolerance;
            return this;
        }

        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;
            //std.map<Vector3, int, Vector3Comparator> mapExistingVertices = new std.map<Vector3, int, Vector3Comparator>();
            std_map<Vector3, int> mapExistingVertices = new std_map<Vector3, int>(new Vector3Comparator());
            std_vector<TriangleBuffer.Vertex> vertices = mInputTriangleBuffer.getVertices();
            std_vector<int> indices = mInputTriangleBuffer.getIndices();

            int newSize = vertices.size();

            //	for (std::vector<TriangleBuffer::Vertex>::iterator it = vertices.begin(); it!= vertices.end(); ++it)
            for (int i = 0; i < vertices.Count; i++) {
                //size_t currentIndex = it - vertices.begin();
                TriangleBuffer.Vertex it = vertices[i];
                int currentIndex = i;
                if (currentIndex >= newSize)
                    break;
                //if (mapExistingVertices.find(it.mPosition) == mapExistingVertices.end())
                //	mapExistingVertices[it.mPosition] = currentIndex;
                if (mapExistingVertices.find(it.mPosition) == -1) {
                    mapExistingVertices.insert(it.mPosition, currentIndex);
                }
                else {
                    int existingIndex = mapExistingVertices[it.mPosition];
                    --newSize;
                    if (currentIndex == newSize) {
                        //for (std::vector<int>::iterator it2 = indices.begin(); it2 != indices.end(); ++it2)
                        for (int j = 0; j < indices.Count; j++) {
                            int it2 = indices[j];
                            if (it2 == currentIndex) {
                                //*it2 = existingIndex;
                                indices[j] = existingIndex;
                            }
                        }
                    }
                    else {
                        int lastIndex = newSize;
                        //*it = vertices[lastIndex];
                        it = vertices[lastIndex];
                        //for (std::vector<int>::iterator it2 = indices.begin(); it2 != indices.end(); ++it2)
                        for (int j = 0; j < indices.Count; j++) {
                            int it2 = indices[j];
                            //if (*it2 == currentIndex)
                            if (it2 == currentIndex) {
                                //*it2 = existingIndex;
                                indices[j] = existingIndex;
                            }
                            //else if (*it2 == lastIndex)
                            else if (it2 == lastIndex) {
                                //*it2 = currentIndex;
                                indices[j] = currentIndex;
                            }
                        }
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------
    //*
    // * \brief Switches the triangle buffer from indexed triangles to (pseudo) triangle list
    // * It can be used if you want discontinuities between all your triangles.
    //
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport UnweldVerticesModifier
    public class UnweldVerticesModifier
    {
        public UnweldVerticesModifier() {
            mInputTriangleBuffer = null;
        }

        public TriangleBuffer mInputTriangleBuffer;

        public UnweldVerticesModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;
            std_vector<TriangleBuffer.Vertex> newVertices = new std_vector<TriangleBuffer.Vertex>();
            std_vector<TriangleBuffer.Vertex> originVertices = mInputTriangleBuffer.getVertices();
            std_vector<int> originIndices = mInputTriangleBuffer.getIndices();
            for (int i = 0; i < originIndices.size(); i += 3) {
                newVertices.Add(originVertices[originIndices[i]]);
                newVertices.Add(originVertices[originIndices[i + 1]]);
                newVertices.Add(originVertices[originIndices[i + 2]]);
            }
            mInputTriangleBuffer.getVertices().clear();
            mInputTriangleBuffer.getVertices().reserve(newVertices.size());
            //for (List<TriangleBuffer.Vertex>.Enumerator it = newVertices.GetEnumerator(); it.MoveNext(); ++it)
            //	mInputTriangleBuffer.getVertices().push_back(it.Current);
            foreach (var it in newVertices) {
                mInputTriangleBuffer.getVertices().push_back(it);
            }
            mInputTriangleBuffer.getIndices().clear();
            mInputTriangleBuffer.getIndices().reserve(newVertices.size());
            for (int i = 0; i < newVertices.Count; i++)
                mInputTriangleBuffer.getIndices().push_back(i);
        }
    }
    //--------------------------------------------------------------
    //*
    // * \brief Recomputes the mesh's UVs based on its projection on a plane
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport PlaneUVModifier
    public class PlaneUVModifier
    {
        private Vector3 mPlaneNormal = new Vector3();
        private Vector3 mPlaneCenter = new Vector3();
        private Vector2 mPlaneSize = new Vector2();
        private TriangleBuffer mInputTriangleBuffer;

        public PlaneUVModifier() {
            mPlaneNormal = Vector3.UNIT_Y;
            mPlaneCenter = Vector3.ZERO;
            mPlaneSize = Vector2.UNIT_SCALE;
            mInputTriangleBuffer = null;
        }

        public PlaneUVModifier setPlaneNormal(Vector3 planeNormal) {
            //
            //ORIGINAL LINE: mPlaneNormal = planeNormal;
            mPlaneNormal = (planeNormal);
            return this;
        }

        public PlaneUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        public PlaneUVModifier setPlaneCenter(Vector3 planeCenter) {
            //
            //ORIGINAL LINE: mPlaneCenter = planeCenter;
            mPlaneCenter = (planeCenter);
            return this;
        }

        public PlaneUVModifier setPlaneSize(Vector2 planeSize) {
            //
            //ORIGINAL LINE: mPlaneSize = planeSize;
            mPlaneSize = (planeSize);
            return this;
        }

        /// \exception Ogre::InvalidStateException Input triangle buffer must be set
        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;
            Vector3 xvec = mPlaneNormal.Perpendicular;
            Vector3 yvec = mPlaneNormal.CrossProduct(xvec);
            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                Vector3 v = it.mPosition - mPlaneCenter;
                float it_mUV_x = v.DotProduct(xvec);
                float it_mUV_y = v.DotProduct(yvec);
                it.mUV = new Vector2(it_mUV_x, it_mUV_y);
            }
        }
    }
    //--------------------------------------------------------------
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport SphereUVModifier
    public class SphereUVModifier
    {
        private TriangleBuffer mInputTriangleBuffer;

        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Input triangle buffer must be set!");
            ;
            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                Vector3 v = it.mPosition.NormalisedCopy;
                Vector2 vxz = new Vector2(v.x, v.z);
                it.mUV.x = Utils.angleTo(Vector2.UNIT_X, vxz).ValueRadians / Math.TWO_PI;
                it.mUV.y = (Math.ATan(v.y / vxz.Length).ValueRadians + Math.HALF_PI) / Math.PI;
            }
        }

        public SphereUVModifier() {
            mInputTriangleBuffer = null;
        }

        public SphereUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }
    }
    //--------------------------------------------------------------
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport HemisphereUVModifier
    public class HemisphereUVModifier
    {
        private TriangleBuffer mInputTriangleBuffer;
        //private TRect<float> mTextureRectangleTop = new TRect<float>();
        //private TRect<float> mTextureRectangleBottom = new TRect<float>();
        private Mogre.RealRect mTextureRectangleTop = new Mogre.RealRect();
        private Mogre.RealRect mTextureRectangleBottom = new Mogre.RealRect();
        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("input triangle buffer must be set!");
            ;
            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                Vector3 input = it.mPosition.NormalisedCopy;
                Vector3 v = new Vector3();
                Radian r = new Radian();
                if (input.y > 0)
                    Vector3.UNIT_Y.GetRotationTo(input).ToAngleAxis(out r, out v);
                else
                    Vector3.NEGATIVE_UNIT_Y.GetRotationTo(input).ToAngleAxis(out r, out v);
                Vector2 v2 = new Vector2(input.x, input.z);
                v2.Normalise();
                Vector2 uv = new Vector2(0.5f, 0.5f) + 0.5f * (r / Math.HALF_PI).ValueRadians * v2;

                if (input.y > 0)
                    it.mUV = Utils.reframe(mTextureRectangleTop, uv);
                else
                    it.mUV = Utils.reframe(mTextureRectangleBottom, uv);
            }
        }

        public HemisphereUVModifier() {
            mInputTriangleBuffer = null;
            mTextureRectangleTop = new Mogre.RealRect(0, 0, 1, 1);
            mTextureRectangleBottom = new Mogre.RealRect(0, 0, 1, 1);
        }

        public HemisphereUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        public HemisphereUVModifier setTextureRectangleTop(TRect textureRectangleTop) {
            //
            //ORIGINAL LINE: mTextureRectangleTop = textureRectangleTop;
            mTextureRectangleTop = (textureRectangleTop);
            return this;
        }

        public HemisphereUVModifier setTextureRectangleBottom(TRect textureRectangleBottom) {
            //
            //ORIGINAL LINE: mTextureRectangleBottom = textureRectangleBottom;
            mTextureRectangleBottom = (textureRectangleBottom);
            return this;
        }

    }
    //--------------------------------------------------------------
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport CylinderUVModifier
    public class CylinderUVModifier
    {
        private TriangleBuffer mInputTriangleBuffer;
        private float mHeight = 0f;
        private float mRadius = 0f;
        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("input triangle buffer must be set!");
            ;
            if (mHeight <= 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Height must be strictly positive", __FUNC__, __FILE__, __LINE__);
                throw new Exception("mHeight must be larger than 0!");
            ;
            if (mRadius <= 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Radius must be strictly positive", __FUNC__, __FILE__, __LINE__);
                throw new Exception("Radius must be larger than 0!");
            ;

            float angleThreshold = Math.ATan(mHeight / mRadius).ValueRadians;
            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                Vector2 nxz = new Vector2(it.mNormal.x, it.mNormal.z);
                float alpha = (Math.ATan(it.mNormal.y / nxz.Length).ValueRadians + Math.HALF_PI);
                if (Math.Abs(alpha) > angleThreshold) {
                    Vector2 vxz = new Vector2(it.mPosition.x, it.mPosition.z);
                    it.mUV = vxz / mRadius;
                }
                else {
                    Vector2 vxz = new Vector2(it.mPosition.x, it.mPosition.z);
                    it.mUV.x = Utils.angleTo(Vector2.UNIT_X, vxz).ValueRadians / Math.TWO_PI;
                    it.mUV.y = it.mPosition.y / mHeight - 0.5f;
                }
            }
        }

        public CylinderUVModifier() {
            mInputTriangleBuffer = null;
            mRadius = 1.0f;
            mHeight = 1.0f;
        }

        public CylinderUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        public CylinderUVModifier setRadius(float radius) {
            mRadius = radius;
            return this;
        }

        public CylinderUVModifier setHeight(float height) {
            mHeight = height;
            return this;
        }

    }
    //--------------------------------------------------------------
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport BoxUVModifier
    public class BoxUVModifier
    {
        public enum MappingType : int
        {
            MT_FULL,
            MT_CROSS,
            MT_PACKED,
        }
        private TriangleBuffer mInputTriangleBuffer;
        private MappingType mMappingType;
        private Vector3 mBoxSize = new Vector3();
        private Vector3 mBoxCenter = new Vector3();

        //--------------------------------------------------------------
        public void modify() {
            if (mInputTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Input triangle buffer must be set", __FUNC__, __FILE__, __LINE__);
                throw new Exception("input triangle buffer must be set!");
            ;

            Vector3[] directions = new Vector3[] { Vector3.UNIT_X, Vector3.UNIT_Y, Vector3.UNIT_Z, Vector3.NEGATIVE_UNIT_X, Vector3.NEGATIVE_UNIT_Y, Vector3.NEGATIVE_UNIT_Z };

            //for (List<TriangleBuffer.Vertex>.Enumerator it = mInputTriangleBuffer.getVertices().begin(); it != mInputTriangleBuffer.getVertices().end(); ++it)
            foreach (var it in mInputTriangleBuffer.getVertices()) {
                Vector3 v = it.mPosition - mBoxCenter;
                if (v.IsZeroLength)
                    continue;
                //v.normalise();
                v.x /= mBoxSize.x;
                v.y /= mBoxSize.y;
                v.z /= mBoxSize.z;
                //C++ TO C# CONVERTER WARNING: The following line was determined to be a copy constructor call - this should be verified and a copy constructor should be created if it does not yet exist:
                //ORIGINAL LINE: Vector3 n = it->mNormal;
                Vector3 n = (it.mNormal);
                float maxAxis = 0;
                int principalAxis = 0;
                for (byte i = 0; i < 6; i++) {
                    if (directions[i].DotProduct(n) > maxAxis) {
                        maxAxis = directions[i].DotProduct(n);
                        principalAxis = i;
                    }
                }

                Vector3 vX = new Vector3();
                Vector3 vY = new Vector3();
                if (principalAxis % 3 == 1)
                    vY = Vector3.UNIT_X;
                else
                    vY = Vector3.UNIT_Y;
                vX = directions[principalAxis].CrossProduct(vY);

                Vector2 uv = new Vector2(0.5f - vX.DotProduct(v), 0.5f - vY.DotProduct(v));
                if (mMappingType == MappingType.MT_FULL)
                    it.mUV = uv;
                else if (mMappingType == MappingType.MT_CROSS) {
                }
                else if (mMappingType == MappingType.MT_PACKED)
                    it.mUV = new Vector2((uv.x + principalAxis % 3) / 3, (uv.y + principalAxis / 3) / 2);
            }
        }

        public BoxUVModifier() {
            mInputTriangleBuffer = null;
            mMappingType = MappingType.MT_FULL;
            mBoxSize = Vector3.UNIT_SCALE;
            mBoxCenter = Vector3.ZERO;
        }

        public BoxUVModifier setInputTriangleBuffer(TriangleBuffer inputTriangleBuffer) {
            mInputTriangleBuffer = inputTriangleBuffer;
            return this;
        }

        public BoxUVModifier setBoxSize(Vector3 boxSize) {
            //
            //ORIGINAL LINE: mBoxSize = boxSize;
            mBoxSize = (boxSize);
            return this;
        }

        public BoxUVModifier setBoxCenter(Vector3 boxCenter) {
            //
            //ORIGINAL LINE: mBoxCenter = boxCenter;
            mBoxCenter = (boxCenter);
            return this;
        }

        public BoxUVModifier setMappingType(MappingType mappingType) {
            mMappingType = mappingType;
            return this;
        }
    }
}


