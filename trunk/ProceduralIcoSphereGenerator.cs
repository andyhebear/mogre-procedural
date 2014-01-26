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
//#ifndef PROCEDURAL_ICOSPHERE_GENERATOR_INCLUDED
#define PROCEDURAL_ICOSPHERE_GENERATOR_INCLUDED
//write with new std... ok
namespace Mogre_Procedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Mogre;
    using Math = Mogre.Math;
    using Mogre_Procedural.std;
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
       //
        //ORIGINAL LINE: IcoSphereGenerator(Ogre::float radius = 1.0f, uint numIterations = 2) : mRadius(radius), mNumIterations(numIterations)
        public IcoSphereGenerator(float radius, uint numIterations) {
            mRadius = radius;
            mNumIterations = numIterations;
        }

        //    *
        //	 * Builds the mesh into the given TriangleBuffer
        //	 * @param buffer The TriangleBuffer on where to append the mesh.
        //	 
        //
        //ORIGINAL LINE: void addToTriangleBuffer(TriangleBuffer& buffer) const
        public override void addToTriangleBuffer(ref TriangleBuffer buffer) {
            std_vector<Vector3> vertices = new  std_vector<Vector3>();
            int offset = 0;

            /// Step 1 : Generate icosahedron
            float phi = 0.5f * (1.0f + Math.Sqrt(5.0f));
            float invnorm = 1f / Math.Sqrt(phi * phi + 1f);

            vertices.push_back(invnorm * new Vector3(-1f, phi, 0f)); //0
            vertices.push_back(invnorm * new Vector3(1f, phi, 0f)); //1
            vertices.push_back(invnorm * new Vector3(0f, 1f, -phi)); //2
            vertices.push_back(invnorm * new Vector3(0f, 1f, phi)); //3
            vertices.push_back(invnorm * new Vector3(-phi, 0f, -1f)); //4
            vertices.push_back(invnorm * new Vector3(-phi, 0f, 1f)); //5
            vertices.push_back(invnorm * new Vector3(phi, 0f, -1f)); //6
            vertices.push_back(invnorm * new Vector3(phi, 0f, 1f)); //7
            vertices.push_back(invnorm * new Vector3(0f, -1f, -phi)); //8
            vertices.push_back(invnorm * new Vector3(0f, -1f, phi)); //9
            vertices.push_back(invnorm * new Vector3(-1f, -phi, 0f)); //10
            vertices.push_back(invnorm * new Vector3(1f, -phi, 0f)); //11

            int[] firstFaces = { 0, 1, 2,
                                   0, 3, 1,
                                   0, 4, 5, 
                                   1, 7, 6, 
                                   1, 6, 2, 
                                   1, 3, 7,
                                   0, 2, 4, 
                                   0, 5, 3, 
                                   2, 6, 8,
                                   2, 8, 4, 
                                   3, 5, 9, 
                                   3, 9, 7,
                                   11, 6, 7, 
                                   10, 5, 4, 
                                   10, 4, 8, 
                                   10, 9, 5,
                                   11, 8, 6,
                                   11, 7, 9, 
                                   10, 8, 11, 
                                   10, 11, 9 
                               };

            //C++ TO C# CONVERTER WARNING: This 'sizeof' ratio was replaced with a direct reference to the array length:
            //ORIGINAL LINE: std::vector<int> faces(firstFaces, firstFaces + sizeof(firstFaces)/sizeof(*firstFaces));
            // 定义一个容纳100个int型数据的容器,初值赋为0
            //vector<int> vecMyHouse(100,0);
            std_vector<int> faces = new  std_vector<int>(firstFaces);//(firstFaces, firstFaces + firstFaces.Length);

            int size = 60;

            /// Step 2 : tessellate
            for (ushort iteration = 0; iteration < mNumIterations; iteration++) {
                size *= 4;
                std_vector<int> newFaces = new std_vector<int>();
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
                    vertices.push_back((v1 + v2).NormalisedCopy);
                    vertices.push_back((v2 + v3).NormalisedCopy);
                    vertices.push_back((v1 + v3).NormalisedCopy);
                    //now recreate indices
                    newFaces.push_back(i1);
                    newFaces.push_back(i12);
                    newFaces.push_back(i13);
                    newFaces.push_back(i2);
                    newFaces.push_back(i23);
                    newFaces.push_back(i12);
                    newFaces.push_back(i3);
                    newFaces.push_back(i13);
                    newFaces.push_back(i23);
                    newFaces.push_back(i12);
                    newFaces.push_back(i23);
                    newFaces.push_back(i13);
                }
                //faces.swap(newFaces);
                faces = newFaces;
            }

            /// Step 3 : generate texcoords
            std_vector<Vector2> texCoords = new std_vector<Vector2>();
            for (ushort i = 0; i < vertices.size(); i++) {
                Vector3 vec = vertices[i];
                float u = 0f;
                float v = 0f;
                float r0 = sqrtf(vec.x * vec.x + vec.z * vec.z);
                float alpha = 0f;
                alpha = atan2f(vec.z, vec.x);
                u = alpha / Math.TWO_PI + .5f;
                v = atan2f(vec.y, r0) / Math.PI + .5f;
                texCoords.push_back(new Vector2(u, v));
            }

            /// Step 4 : fix texcoords
            // find vertices to split
            std_vector<int> indexToSplit = new  std_vector<int>();

            for (int i = 0; i < faces.size() / 3; i++) {
                Vector2 t0 = texCoords[faces[i * 3 + 0]];
                Vector2 t1 = texCoords[faces[i * 3 + 1]];
                Vector2 t2 = texCoords[faces[i * 3 + 2]];
                if (Math.Abs(t2.x - t0.x) > 0.5) {
                    if (t0.x < 0.5)
                        indexToSplit.push_back(faces[i * 3]);
                    else
                        indexToSplit.push_back(faces[i * 3 + 2]);
                }
                if (Math.Abs(t1.x - t0.x) > 0.5) {
                    if (t0.x < 0.5)
                        indexToSplit.push_back(faces[i * 3]);
                    else
                        indexToSplit.push_back(faces[i * 3 + 1]);
                }
                if (Math.Abs(t2.x - t1.x) > 0.5) {
                    if (t1.x < 0.5)
                        indexToSplit.push_back(faces[i * 3 + 1]);
                    else
                        indexToSplit.push_back(faces[i * 3 + 2]);
                }
            }

            //split vertices
            for (ushort i = 0; i < indexToSplit.size(); i++) {
                int index = indexToSplit[i];
                //duplicate vertex
                Vector3 v = vertices[index];
                Vector2 t = texCoords[index] + Vector2.UNIT_X;
                vertices.push_back(v);
                texCoords.push_back(t);
                int newIndex = vertices.size() - 1;
                //reassign indices
                for (ushort j = 0; j < faces.size(); j++) {
                    if (faces[j] == index) {
                        int index1 = faces[(j + 1) % 3 + (j / 3) * 3];
                        int index2 = faces[(j + 2) % 3 + (j / 3) * 3];
                        if ((texCoords[index1].x > 0.5f) || (texCoords[index2].x > 0.5f)) {
                            faces[j] = newIndex;
                        }
                    }
                }
            }

            /// Step 5 : realize
            buffer.rebaseOffset();
            buffer.estimateVertexCount((uint)vertices.size());
            buffer.estimateIndexCount((uint)size);

            for (ushort i = 0; i < vertices.size(); i++) {
                addPoint(ref buffer, mRadius * vertices[i], vertices[i], new Vector2(texCoords[i].x, texCoords[i].y));
            }
            for (ushort i = 0; i < size; i++) {
                buffer.index(offset + faces[i]);
            }
            offset += vertices.size();
        }



        //    *
        //	Sets the radius of the sphere (default=1)
        //	\exception Ogre::InvalidParametersException Radius must be larger than 0!
        //	
        public IcoSphereGenerator setRadius(float radius) {
            if (radius <= 0.0f)
             OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "Radius must be larger than 0!", "Procedural::IcoSphereGenerator::setRadius(Ogre::Real)");            ;
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
             OGRE_EXCEPT("Ogre::Exception::ERR_INVALIDPARAMS", "There must be more than 0 iterations", "Procedural::IcoSphereGenerator::setNumRings(unsigned int)");
            ;
            mNumIterations = numIterations;
            return this;
        }

    }
}

