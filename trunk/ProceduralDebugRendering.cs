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
    /// This class creates a visualisation of the normals of a TriangleBuffer
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport ShowNormalsGenerator
    public class ShowNormalsGenerator
    {
        public enum VisualStyle : int
        {
            VS_LINE,
            VS_ARROW
        }

        private VisualStyle mVisualStyle;

        private TriangleBuffer mTriangleBuffer;

        private float mSize = 0f;
        public ShowNormalsGenerator() {
            mTriangleBuffer = null;
            mSize = 1.0f;
            mVisualStyle = VisualStyle.VS_LINE;
        }

        /// Sets the input Triangle Buffer
        public ShowNormalsGenerator setTriangleBuffer(TriangleBuffer triangleBuffer) {
            mTriangleBuffer = triangleBuffer;
            return this;
        }

        /// Sets the size of the normals representation (default = 1.0)
        public ShowNormalsGenerator setSize(float size) {
            mSize = size;
            return this;
        }


        /// Sets the visual style, line or arrow (default = line)
        public ShowNormalsGenerator setVisualStyle(VisualStyle visualStyle) {
            mVisualStyle = visualStyle;
            return this;
        }


        /// Builds the normals representation as a manual object
        /// \exception Ogre::InvalidStateException The input triangle buffer must not be null
        /// \exception Ogre::InvalidStateException Scene Manager is not set in OGRE root object
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: ManualObject* buildManualObject() const
        public ManualObject buildManualObject() {
            if (mTriangleBuffer == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "The input triangle buffer must not be null", "Procedural::ShowNormalsGenerator::buildManualObject()", __FILE__, __LINE__);
                throw new Exception("The input triangle buffer must not be null !");
            ;
            SceneManager sceneMgr = Root.Singleton.GetSceneManagerIterator().Current;
            if (sceneMgr == null)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALID_STATE>(), "Scene Manager must be set in Root", "Procedural::ShowNormalsGenerator::buildManualObject()", __FILE__, __LINE__);
                throw new Exception("scenemager must not be null !");
            ;
            ManualObject manual = sceneMgr.CreateManualObject("debug_procedural_" + Guid.NewGuid().ToString("N"));
            manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_LIST);
            List<TriangleBuffer.Vertex> vertices = mTriangleBuffer.getVertices();
            //for (List<TriangleBuffer.Vertex>.Enumerator it = vertices.GetEnumerator(); it.MoveNext(); ++it)
            foreach (var it in vertices) {
                manual.Position(it.mPosition);
                manual.Position(it.mPosition + it.mNormal * mSize);

                if (mVisualStyle == VisualStyle.VS_ARROW) {
                    Vector3 axis2 = it.mNormal.Perpendicular;
                    Vector3 axis3 = it.mNormal.CrossProduct(axis2);

                    manual.Position(it.mPosition + it.mNormal * mSize);
                    manual.Position(it.mPosition + (.8f * it.mNormal + .1f * axis2) * mSize);

                    manual.Position(it.mPosition + it.mNormal * mSize);
                    manual.Position(it.mPosition + .8f * (it.mNormal - .1f * axis2) * mSize);

                    manual.Position(it.mPosition + it.mNormal * mSize);
                    manual.Position(it.mPosition + .8f * (it.mNormal + .1f * axis3) * mSize);

                    manual.Position(it.mPosition + it.mNormal * mSize);
                    manual.Position(it.mPosition + .8f * (it.mNormal - .1f * axis3) * mSize);
                }
            }
            manual.End();

            return manual;
        }

        /// Builds the normals representation as a mesh
        public MeshPtr buildMesh(string name) {
            return buildMesh(name, "General");
        }
        public MeshPtr buildMesh() {
            return buildMesh("", "General");
        }
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: MeshPtr buildMesh(const string& name = "", const string& group = "General") const
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        public MeshPtr buildMesh(string name, string group) {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(name));
            if (string.IsNullOrEmpty(name)) {
                name = Utils.getName("debugrender_procedural");
            }
            SceneManager sceneMgr = Root.Singleton.GetSceneManagerIterator().Current;
            ManualObject mo = buildManualObject();
            MeshPtr mesh = mo.ConvertToMesh(name, group);

            sceneMgr.DestroyManualObject(mo);

            return mesh;
        }
    }
}


