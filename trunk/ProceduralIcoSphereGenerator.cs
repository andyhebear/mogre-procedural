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
    //*
    // * \ingroup objgengrp
    // * Builds an icosphere mesh, ie a sphere built with equally sized triangles
    // * \image html primitive_icosphere.png
    // 
    //C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
    //ORIGINAL LINE: class _ProceduralExport IcoSphereGenerator : public MeshGenerator<IcoSphereGenerator>
    public class IcoSphereGenerator : MeshGenerator<IcoSphereGenerator>
    {
        private float mRadius = 0f;
        private uint mNumIterations;

        /// Contructor with arguments
        public IcoSphereGenerator(float radius)
            : this(radius, 2) {
        }
        public IcoSphereGenerator()
            : this(1.0f, 2) {
        }
        //C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
        //ORIGINAL LINE: IcoSphereGenerator(Ogre::float radius = 1.0f, uint numIterations = 2) : mRadius(radius), mNumIterations(numIterations)
        public IcoSphereGenerator(float radius, uint numIterations) {
            mRadius = radius;
            mNumIterations = numIterations;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public void addToTriangleBuffer(ref TriangleBuffer buffer) {
            List<Vector3> vertices = new List<Vector3>();
            int offset = 0;

            /// Step 1 : Generate icosahedron
            float phi = 0.5f * (1.0f + Math.Sqrt(5.0f));
            float invnorm = 1f / Math.Sqrt(phi * phi + 1);

            vertices.Add(invnorm * new Vector3(-1, phi, 0)); //0
            vertices.Add(invnorm * new Vector3(1, phi, 0)); //1
            vertices.Add(invnorm * new Vector3(0, 1, -phi)); //2
            vertices.Add(invnorm * new Vector3(0, 1, phi)); //3
            vertices.Add(invnorm * new Vector3(-phi, 0, -1)); //4
            vertices.Add(invnorm * new Vector3(-phi, 0, 1)); //5
            vertices.Add(invnorm * new Vector3(phi, 0, -1)); //6
            vertices.Add(invnorm * new Vector3(phi, 0, 1)); //7
            vertices.Add(invnorm * new Vector3(0, -1, -phi)); //8
            vertices.Add(invnorm * new Vector3(0, -1, phi)); //9
            vertices.Add(invnorm * new Vector3(-1, -phi, 0)); //10
            vertices.Add(invnorm * new Vector3(1, -phi, 0)); //11

            int[] firstFaces = { 0, 1, 2, 0, 3, 1, 0, 4, 5, 1, 7, 6, 1, 6, 2, 1, 3, 7, 0, 2, 4, 0, 5, 3, 2, 6, 8, 2, 8, 4, 3, 5, 9, 3, 9, 7, 11, 6, 7, 10, 5, 4, 10, 4, 8, 10, 9, 5, 11, 8, 6, 11, 7, 9, 10, 8, 11, 10, 11, 9 };

            //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
            //ORIGINAL LINE: std::vector<int> faces(firstFaces, firstFaces + sizeof(firstFaces)/sizeof(*firstFaces));
            // 定义一个容纳100个int型数据的容器,初值赋为0
            //vector<int> vecMyHouse(100,0);
            List<int> faces = new List<int>(firstFaces);//(firstFaces, firstFaces + firstFaces.Length);

            int size = 60;

            /// Step 2 : tessellate
            for (ushort iteration = 0; iteration < mNumIterations; iteration++) {
                size *= 4;
                List<int> newFaces = new List<int>();
                newFaces.Clear();
                //newFaces.resize(size);
                for (int i = 0; i < size / 12; i++) {
                    int i1 = faces[i * 3];
                    int i2 = faces[i * 3 + 1];
                    int i3 = faces[i * 3 + 2];
                    int i12 = vertices.Count;
                    int i23 = i12 + 1;
                    int i13 = i12 + 2;
                    Vector3 v1 = vertices[i1];
                    Vector3 v2 = vertices[i2];
                    Vector3 v3 = vertices[i3];
                    //make 1 vertice at the center of each edge and project it onto the sphere
                    vertices.Add((v1 + v2).NormalisedCopy);
                    vertices.Add((v2 + v3).NormalisedCopy);
                    vertices.Add((v1 + v3).NormalisedCopy);
                    //now recreate indices
                    newFaces.Add(i1);
                    newFaces.Add(i12);
                    newFaces.Add(i13);
                    newFaces.Add(i2);
                    newFaces.Add(i23);
                    newFaces.Add(i12);
                    newFaces.Add(i3);
                    newFaces.Add(i13);
                    newFaces.Add(i23);
                    newFaces.Add(i12);
                    newFaces.Add(i23);
                    newFaces.Add(i13);
                }
                //faces.swap(newFaces);
                faces = newFaces;
            }

            /// Step 3 : generate texcoords
            List<Vector2> texCoords = new List<Vector2>();
            for (ushort i = 0; i < vertices.Count; i++) {
                Vector3 vec = vertices[i];
                float u = 0f;
                float v = 0f;
                float r0 = sqrtf(vec.x * vec.x + vec.z * vec.z);
                float alpha = 0f;
                alpha = atan2f(vec.z, vec.x);
                u = alpha / Math.TWO_PI + .5f;
                v = atan2f(vec.y, r0) / Math.PI + .5f;
                texCoords.Add(new Vector2(u, v));
            }

            /// Step 4 : fix texcoords
            // find vertices to split
            List<int> indexToSplit = new List<int>();

            for (int i = 0; i < faces.Count / 3; i++) {
                Vector2 t0 = texCoords[faces[i * 3 + 0]];
                Vector2 t1 = texCoords[faces[i * 3 + 1]];
                Vector2 t2 = texCoords[faces[i * 3 + 2]];
                if (Math.Abs(t2.x - t0.x) > 0.5) {
                    if (t0.x < 0.5)
                        indexToSplit.Add(faces[i * 3]);
                    else
                        indexToSplit.Add(faces[i * 3 + 2]);
                }
                if (Math.Abs(t1.x - t0.x) > 0.5) {
                    if (t0.x < 0.5)
                        indexToSplit.Add(faces[i * 3]);
                    else
                        indexToSplit.Add(faces[i * 3 + 1]);
                }
                if (Math.Abs(t2.x - t1.x) > 0.5) {
                    if (t1.x < 0.5)
                        indexToSplit.Add(faces[i * 3 + 1]);
                    else
                        indexToSplit.Add(faces[i * 3 + 2]);
                }
            }

            //split vertices
            for (ushort i = 0; i < indexToSplit.Count; i++) {
                int index = indexToSplit[i];
                //duplicate vertex
                Vector3 v = vertices[index];
                Vector2 t = texCoords[index] + Vector2.UNIT_X;
                vertices.Add(v);
                texCoords.Add(t);
                int newIndex = vertices.Count - 1;
                //reassign indices
                for (ushort j = 0; j < faces.Count; j++) {
                    if (faces[j] == index) {
                        int index1 = faces[(j + 1) % 3 + (j / 3) * 3];
                        int index2 = faces[(j + 2) % 3 + (j / 3) * 3];
                        if ((texCoords[index1].x > 0.5) || (texCoords[index2].x > 0.5)) {
                            faces[j] = newIndex;
                        }
                    }
                }
            }

            /// Step 5 : realize
            buffer.rebaseOffset();
            buffer.estimateVertexCount((uint)vertices.Count);
            buffer.estimateIndexCount((uint)size);

            for (ushort i = 0; i < vertices.Count; i++) {
                addPoint(ref buffer, mRadius * vertices[i], vertices[i], new Vector2(texCoords[i].x, texCoords[i].y));
            }
            for (ushort i = 0; i < size; i++) {
                buffer.index(offset + faces[i]);
            }
            offset += vertices.Count;
        }



        //    *
        //	Sets the radius of the sphere (default=1)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public IcoSphereGenerator setRadius(float radius) {
            if (radius <= 0.0f)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "Radius must be larger than 0!", "Procedural::IcoSphereGenerator::setRadius(Ogre::Real)", __FILE__, __LINE__);
                throw new Exception("Radius must be larger than 0!");
            ;
            mRadius = radius;
            return this;
        }

        //    * Sets the number of iterations needed to build the sphere mesh.
        //		First iteration corresponds to a 20 face sphere.
        //		Each iteration has 3 more faces than the previous.
        //		(default=2)
        //	\exception Ogre::InvalidParametersException Minimum of numIterations is 1
        //	
        public IcoSphereGenerator setNumIterations(uint numIterations) {
            if (numIterations == 0)
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __LINE__ macro:
                //C++ TO C# CONVERTER TODO TASK: There is no direct equivalent in C# to the C++ __FILE__ macro:
                //throw ExceptionFactory.create(Mogre.ExceptionCodeType<Mogre.Exception.ExceptionCodes.ERR_INVALIDPARAMS>(), "There must be more than 0 iterations", "Procedural::IcoSphereGenerator::setNumRings(unsigned int)", __FILE__, __LINE__);
                throw new Exception("numIterations must be larger than 0!");
            ;
            mNumIterations = numIterations;
            return this;
        }

    }
}

