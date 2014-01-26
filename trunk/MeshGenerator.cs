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
//#ifndef PROCEDURAL_MESH_GENERATOR_INCLUDED
#define PROCEDURAL_MESH_GENERATOR_INCLUDED
//write with new std .... ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
    //C++ TO C# CONVERTER TODO TASK: The original C++ template specifier was replaced with a C# generic specifier, which may not produce the same behavior:
    //ORIGINAL LINE: template <typename T>
    //*
    //\defgroup objgengrp Object generators
    //Elements for procedural mesh generation of various objects.
    //@{
    //@}
    //

    //*
    //\ingroup objgengrp
    //Superclass of everything that builds meshes
    // 
    public abstract class MeshGenerator<T>
    {
        /// A pointer to the default scene manager
        //Ogre::SceneManager* mSceneMgr;

        /// U tile for texture coords generation
        protected float mUTile = 0f;

        /// V tile for texture coords generation
        protected float mVTile = 0f;

        /// Whether to produces normals or not
        protected bool mEnableNormals;

        /// The number of texture coordinate sets to include
        protected byte mNumTexCoordSet;

        /// Rectangle in which the texture coordinates will be placed
        protected Vector2 mUVOrigin = new Vector2();

        /// If set to true, the UV coordinates coming from the mesh generator will be switched.
        /// It can be used, for example, if your texture doesn't fit the mesh generator's assumptions about UV.
        /// If UV were to fit in a given rectangle, they still fit in it after the switch.
        protected bool mSwitchUV;

        /// Orientation to apply the mesh
        protected Quaternion mOrientation = new Quaternion();

        /// Scale to apply the mesh
        protected Vector3 mScale = new Vector3();

        /// Position to apply to the mesh
        protected Vector3 mPosition = new Vector3();

        // Whether a transform has been defined or not
        protected bool mTransform;

        /// Default constructor
        /// \exception Ogre::InvalidStateException Scene Manager is not set in OGRE root object
        public MeshGenerator() {
            mUTile = 1.0f;
            mVTile = 1.0f;
            mEnableNormals = true;
            mNumTexCoordSet = 1;
            mUVOrigin = new Vector2(0f, 0f);
            mSwitchUV = false;
            mOrientation = Quaternion.IDENTITY;
            mScale = new Vector3(1f, 1f, 1f);
            mPosition = new Vector3(0f, 0f, 0f);
            mTransform = false;
        }

        //    *
        //	 * Builds a mesh.
        //	 * @param name of the mesh for the MeshManager
        //	 * @param group ressource group in which the mesh will be created
        //	 
        public MeshPtr realizeMesh(string name) {
            return realizeMesh(name, "General");
        }
        public MeshPtr realizeMesh() {
            return realizeMesh("", "General");
        }
       //
        //ORIGINAL LINE: Ogre::MeshPtr realizeMesh(const string& name = "", const Ogre::String& group = "General")
        public MeshPtr realizeMesh(string name, string group) {
            TriangleBuffer tbuffer = new TriangleBuffer();
            addToTriangleBuffer(ref tbuffer);
            MeshPtr mesh = MeshManager.Singleton.CreateManual(name, group); //new  MeshPtr();
            if (name == "")
                mesh = tbuffer.transformToMesh(Utils.getName("mesh_procedural_"), group);
            else
                mesh = tbuffer.transformToMesh(name, group);
            return mesh;
        }

        //    *
        //	 * Outputs a triangleBuffer
        //	 
        //
        //ORIGINAL LINE: TriangleBuffer buildTriangleBuffer() const
        public TriangleBuffer buildTriangleBuffer() {
            TriangleBuffer tbuffer = new TriangleBuffer();
            addToTriangleBuffer(ref tbuffer);
            return tbuffer;
        }

        //    *
        //	 * Overloaded by each generator to implement the specifics
        //	 
        public abstract void addToTriangleBuffer(ref TriangleBuffer buffer);

        //    *
        //	 * Sets U Tile, ie the number by which u texture coordinates are multiplied (default=1)
        //	 
        public MeshGenerator<T> setUTile(float uTile) {
            mUTile = uTile;
            //return( this);
            return this;
        }
        protected float cosf(float a) {
            return Math.Cos(a);
        }
        protected float sinf(float a) {
            return Math.Sin(a);
        }
        protected float atan2f(float p, float p_2) {
            return Math.ATan2(p, p_2).ValueRadians;
        }

        protected float sqrtf(float p) {
            return Math.Sqrt(p);
        }
        protected Real max(Real p, Real p_2) {
            return System.Math.Max(p, p_2);
        }
        protected static void OGRE_EXCEPT(string p, string p_2, string p_3) {
            throw new Exception(p + "_" + p_2 + "_" + p_3);
        }
        //    *
        //	 * Sets V Tile, ie the number by which v texture coordinates are multiplied (default=1)
        //	 
        public MeshGenerator<T> setVTile(float vTile) {
            mVTile = vTile;
            //return (T)( this);
            return this;
        }

        //    *
        //	 * Sets the texture rectangle
        //	 
        public MeshGenerator<T> setTextureRectangle(RealRect textureRectangle) {
            mUVOrigin = new Vector2(textureRectangle.top, textureRectangle.left);
            mUTile = textureRectangle.right - textureRectangle.left;
            mVTile = textureRectangle.bottom - textureRectangle.top;
            //return  (T)this;
            return this;
        }

        //    *
        //	 * Sets whether normals are enabled or not (default=true)
        //	 
        public MeshGenerator<T> setEnableNormals(bool enableNormals) {
            mEnableNormals = enableNormals;
            //return (T)( this);
            return this;
        }

        //    *
        //	 * Sets the number of texture coordintate sets (default=1)
        //	 
        public MeshGenerator<T> setNumTexCoordSet(byte numTexCoordSet) {
            mNumTexCoordSet = numTexCoordSet;
            //return (T)( this);
            return this;
        }

        /// Sets whether to switch U and V texture coordinates
        public MeshGenerator<T> setSwitchUV(bool switchUV) {
            mSwitchUV = switchUV;
            //return (T)( this);
            return this;
        }

        /// Sets an orientation to give when building the mesh
        public MeshGenerator<T> setOrientation(Quaternion orientation) {
            mOrientation = orientation;
            mTransform = true;
            //return (T)( this);
            return this;
        }

        /// Sets a translation baked into the resulting mesh
        public MeshGenerator<T> setPosition(Vector3 position) {
            mPosition = position;
            mTransform = true;
            //return (T)( this);
            return this;
        }

        /// Sets a translation baked into the resulting mesh
        public MeshGenerator<T> setPosition(float x, float y, float z) {
            mPosition = new Vector3(x, y, z);
            mTransform = true;
            //return (T)( this);
            return this;
        }


        /// Sets a scale baked into the resulting mesh
        public MeshGenerator<T> setScale(Vector3 scale) {
            mScale = scale;
            mTransform = true;
            //return (T)( this);
            return this;
        }

        /// Sets a uniform scale baked into the resulting mesh
        public MeshGenerator<T> setScale(float scale) {
            mScale = new Vector3(scale);
            mTransform = true;
            //return (T)( this);
            return this;
        }

        /// Sets a scale baked into the resulting mesh
        public MeshGenerator<T> setScale(float x, float y, float z) {
            mScale = new Vector3(x, y, z);
            mTransform = true;
            //return (T)( this);
            return this;
        }

        /// Resets all transforms (orientation, position and scale) that would have been applied to the mesh to their default values
        public MeshGenerator<T> resetTransforms() {
            mTransform = false;
            mPosition = Vector3.ZERO;
            mOrientation = Quaternion.IDENTITY;
            mScale = new Vector3(1f);
            //return (T)( this);
            return this;
        }

        /// Adds a new point to a triangle buffer, using the format defined for that MeshGenerator
        /// @param buffer the triangle buffer to update
        /// @param position the position of the new point
        /// @param normal the normal of the new point
        /// @param uv the uv texcoord of the new point
        //
        //ORIGINAL LINE: inline void addPoint(TriangleBuffer& buffer, const Ogre::Vector3& position, const Ogre::Vector3& normal, const Ogre::Vector2& uv) const
        protected void addPoint(ref TriangleBuffer buffer, Vector3 position, Vector3 normal, Vector2 uv) {
            if (mTransform)
                buffer.position(mPosition + mOrientation * (mScale * position));
            else
                buffer.position(position);
            if (mEnableNormals) {
                if (mTransform)
                    buffer.normal(mOrientation * normal);
                else
                    buffer.normal(normal);
            }
            if (mSwitchUV)
                for (byte i = 0; i < mNumTexCoordSet; i++)
                    buffer.textureCoord(mUVOrigin.x + uv.y * mUTile, mUVOrigin.y + uv.x * mVTile);
            else
                for (byte i = 0; i < mNumTexCoordSet; i++)
                    buffer.textureCoord(mUVOrigin.x + uv.x * mUTile, mUVOrigin.y + uv.y * mVTile);
        }

    }
    //
}
