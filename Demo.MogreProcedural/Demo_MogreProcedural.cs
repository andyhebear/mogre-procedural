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
namespace Demo.MogreProcedural
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using Mogre_Procedural.Game.BaseApp;
    using Mogre_Procedural;
    using Mogre;

    public class Demo_MogreProcedural : Mogre_Procedural.Game.BaseApp.BaseApplication
    //public class Demo_MogreProcedural:Game.BaseApp.OgreWindow
    {

        public Demo_MogreProcedural() {
        }
        //public Demo_MogreProcedural(Point point, IntPtr handle)
        //    : base(point, handle) {

        //}
        //create base date
        CameraMan _cameraCtl;
        protected override void CreateScene() {
            // Set ambient light
            SceneMgr.AmbientLight = new ColourValue(0.7F, 0.7F, 0.7F);

            // Create a skydome
            //SceneMgr.SetSkyDome(true, "Examples/CloudySky", 5, 8);
            //SceneMgr.SetSkyBox(true, "Examples/StormySkyBox", 200f, true);
            SceneMgr.SetSkyBox(true, "Examples/CloudyNoonSkyBox", 200f, true);
            // Define a floor plane mesh
            Mogre.Plane p;
            p.normal = Vector3.UNIT_Y;
            p.d = 50f;
            MeshManager.Singleton.CreatePlane("FloorPlane", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, p, 1000F, 1000F, 20, 20, true, 1, 10F, 10F, Vector3.UNIT_Z);
            putMeshMat("FloorPlane", "Examples/GrassFloor", Vector3.ZERO, false);
            //
            // Create a light
            Light l = SceneMgr.CreateLight("MainLight");
            l.Type = (Light.LightTypes.LT_POINT);
            //l. = 200;
            l.Direction = (new Vector3(0, -1, 1).NormalisedCopy);
            l.DiffuseColour = (new ColourValue(0.5f, 0.5f, 0.5f));
            l.SpecularColour = (new ColourValue(0.1f, 0.1f, 0.1f));
            // Accept default settings: point light, white diffuse, just set position
            // NB I could attach the light to a SceneNode if I wanted it to move automatically with
            //  other objects, but I don't
            l.Position = new Vector3(20F, 30F, 20F);
            //SceneMgr.RootSceneNode.AttachObject(l);


            //throw new NotImplementedException();
            _cameraCtl = new CameraMan(base.Camera);
            _cameraCtl.FastMove = false;

            // Generates every type of primitive
            new PlaneGenerator().setNumSegX(20).setNumSegY(20).setSizeX(150f).setSizeY(150f).setUTile(5.0f).setVTile(5.0f).realizeMesh("planeMesh");
            putMesh2("planeMesh", new Vector3(0, 0, 0));
            new SphereGenerator().setRadius(2.0f).setUTile(5.0f).setVTile(5.0f).realizeMesh("sphereMesh");
            putMesh("sphereMesh", new Vector3(0, 10, 0));
            new CylinderGenerator().setHeight(3.0f).setRadius(1.0f).setUTile(3.0f).realizeMesh("cylinderMesh");
            putMesh("cylinderMesh", new Vector3(10, 10, 0));
            new TorusGenerator().setRadius(3.0f).setSectionRadius(1.0f).setUTile(10.0f).setVTile(5.0f).realizeMesh("torusMesh");
            putMesh("torusMesh", new Vector3(-10, 10, 0));
            new ConeGenerator().setRadius(2.0f).setHeight(3.0f).setNumSegBase(36).setNumSegHeight(2).setUTile(3.0f).realizeMesh("coneMesh");
            putMesh("coneMesh", new Vector3(0, 10, -10));
            new TubeGenerator().setHeight(3.0f).setUTile(3.0f).realizeMesh("tubeMesh");
            putMesh("tubeMesh", new Vector3(-10, 10, -10));
            new BoxGenerator().setSizeX(2.0f).setSizeY(4.0f).setSizeZ(6.0f).realizeMesh("boxMesh");
            putMesh("boxMesh", new Vector3(10, 10, -10));
            //         
            new CapsuleGenerator().setHeight(2.0f).realizeMesh("capsuleMesh");
            putMesh("capsuleMesh", new Vector3(0, 10, 10));
            TorusKnotGenerator tkg = (new TorusKnotGenerator().setRadius(2.0f).setSectionRadius(0.5f).setUTile(3.0f) as TorusKnotGenerator);
            tkg.setNumSegCircle(64).setNumSegSection(16).realizeMesh("torusKnotMesh");
            putMesh("torusKnotMesh", new Vector3(-10, 10, 10));
            //
            new IcoSphereGenerator().setRadius(2.0f).setNumIterations(3).setUTile(5.0f).setVTile(5.0f).realizeMesh("icoSphereMesh");
            putMesh("icoSphereMesh", new Vector3(10, 10, 10));
            new RoundedBoxGenerator().setSizeX(1.0f).setSizeY(5.0f).setSizeZ(5.0f).setChamferSize(1.0f).realizeMesh("roundedBoxMesh");
            putMesh("roundedBoxMesh", new Vector3(20, 10, 10));
            new SpringGenerator().setNumSegCircle(32).setNumSegPath(30).realizeMesh("springMesh");
            putMesh("springMesh", new Vector3(20, 10, 0));

        }

        protected override void Update(Mogre.FrameEvent evt) {
            //throw new NotImplementedException();
            _cameraCtl.UpdateCamera(evt.timeSinceLastFrame, base.Input);
        }
        //-------------------------------------------------------------------------------------
        void putMesh3(string meshName, Vector3 position) {
            putMeshMat(meshName, "Procedural/Road", position, false);
        }
        //-------------------------------------------------------------------------------------
        void putMesh2(string meshName, Vector3 position) {
            putMeshMat(meshName, "Procedural/Rockwall", position, false);
        }
        //-------------------------------------------------------------------------------------
        void putMesh(string meshName, Vector3 position) {
            putMeshMat(meshName, "Procedural/BeachStones", position, true);
        }

        void putMeshMat(string meshName, string matName, Vector3 position, bool castShadows) {
            Entity ent2 = mSceneMgr.CreateEntity(meshName);
            SceneNode sn = mSceneMgr.RootSceneNode.CreateChildSceneNode();
            sn.AttachObject(ent2);
            sn.Position = (position);
            ent2.SetMaterialName(matName);
            ent2.CastShadows = (castShadows);
        }

    }






}
