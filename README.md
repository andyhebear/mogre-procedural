# mogre-procedural
this is overwrite ogre-procedural c++ project using c#, look ogre-procedural c++ source http://code.google.com/p/ogre-procedural/  Ogre Procedural is a library for creating procedural geometry and textures for Ogre3d based projects.  That includes :      Primitives, such as box, sphere... Extruded shapes along paths or around an axis (useful for roads, rails...) Splines 2D Triangulation Textures (in default branch)   if you has problem,please go to http://www.ogre3d.org/addonforums/viewtopic.php?f=33&amp;t=30133      or http://www.ogre3d.org/addonforums/viewtopic.php?f=8&amp;t=30124 !   then faceback to me. 


Simple Code

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

        putMesh("boxMesh", new Vector3(10, 10, -10)); //

        new CapsuleGenerator().setHeight(2.0f).realizeMesh("capsuleMesh");

        putMesh("capsuleMesh", new Vector3(0, 10, 10)); TorusKnotGenerator tkg = (new TorusKnotGenerator().setRadius(2.0f).setSectionRadius(0.5f).setUTile(3.0f) as TorusKnotGenerator);

        tkg.setNumSegCircle(64).setNumSegSection(16).realizeMesh("torusKnotMesh");

        putMesh("torusKnotMesh", new Vector3(-10, 10, 10)); //

        new IcoSphereGenerator().setRadius(2.0f).setNumIterations(3).setUTile(5.0f).setVTile(5.0f).realizeMesh("icoSphereMesh");

        putMesh("icoSphereMesh", new Vector3(10, 10, 10));

        new RoundedBoxGenerator().setSizeX(1.0f).setSizeY(5.0f).setSizeZ(5.0f).setChamferSize(1.0f).realizeMesh("roundedBoxMesh");

        putMesh("roundedBoxMesh",new Vector3(20,10,10));

        new SpringGenerator().setNumSegCircle(32).setNumSegPath(30).realizeMesh("springMesh");

        putMesh("springMesh",new Vector3(20,10,0)); 

