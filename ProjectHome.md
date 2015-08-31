this is overwrite  ogre-procedural c++ project using c#, look  ogre-procedural c++ source http://code.google.com/p/ogre-procedural/


Ogre Procedural is a library for creating procedural geometry and textures for Ogre3d based projects.

That includes :

> Primitives, such as box, sphere...
> Extruded shapes along paths or around an axis (useful for roads, rails...)
> Splines
> 2D Triangulation
> Textures (in default branch)
if you has problem,please go to http://www.ogre3d.org/addonforums/viewtopic.php?f=33&t=30133
> or  http://www.ogre3d.org/addonforums/viewtopic.php?f=8&t=30124 !
then faceback to me.


&lt;hr/&gt;

<br>
<b>Simple Code</b>
<table width='360' border='1'>
<tr><td>
<blockquote>// Generates every type of primitive<br>
<br />new PlaneGenerator().setNumSegX(20).setNumSegY(20).setSizeX(150f).setSizeY(150f).setUTile(5.0f).setVTile(5.0f).realizeMesh("planeMesh");<br>
<blockquote><br />putMesh2("planeMesh", new Vector3(0, 0, 0));<br>
<br />new SphereGenerator().setRadius(2.0f).setUTile(5.0f).setVTile(5.0f).realizeMesh("sphereMesh");<br>
<br />putMesh("sphereMesh", new Vector3(0, 10, 0));<br>
<br />new CylinderGenerator().setHeight(3.0f).setRadius(1.0f).setUTile(3.0f).realizeMesh("cylinderMesh");<br>
<br />putMesh("cylinderMesh", new Vector3(10, 10, 0));<br>
<br />new TorusGenerator().setRadius(3.0f).setSectionRadius(1.0f).setUTile(10.0f).setVTile(5.0f).realizeMesh("torusMesh");<br>
<br />putMesh("torusMesh", new Vector3(-10, 10, 0));<br>
<br />new ConeGenerator().setRadius(2.0f).setHeight(3.0f).setNumSegBase(36).setNumSegHeight(2).setUTile(3.0f).realizeMesh("coneMesh");<br>
<br />putMesh("coneMesh", new Vector3(0, 10, -10));<br>
<br />new TubeGenerator().setHeight(3.0f).setUTile(3.0f).realizeMesh("tubeMesh");<br>
<br />putMesh("tubeMesh", new Vector3(-10, 10, -10));<br>
<br />new BoxGenerator().setSizeX(2.0f).setSizeY(4.0f).setSizeZ(6.0f).realizeMesh("boxMesh");<br>
<br />putMesh("boxMesh", new Vector3(10, 10, -10));<br>
//<br>
<br />new CapsuleGenerator().setHeight(2.0f).realizeMesh("capsuleMesh");<br>
<br />putMesh("capsuleMesh", new Vector3(0, 10, 10));<br>
TorusKnotGenerator tkg = (new TorusKnotGenerator().setRadius(2.0f).setSectionRadius(0.5f).setUTile(3.0f) as TorusKnotGenerator);<br>
<br />tkg.setNumSegCircle(64).setNumSegSection(16).realizeMesh("torusKnotMesh");<br>
<br />putMesh("torusKnotMesh", new Vector3(-10, 10, 10));<br>
//<br>
<br />new IcoSphereGenerator().setRadius(2.0f).setNumIterations(3).setUTile(5.0f).setVTile(5.0f).realizeMesh("icoSphereMesh");<br>
<br />putMesh("icoSphereMesh", new Vector3(10, 10, 10));<br>
<br />new RoundedBoxGenerator().setSizeX(1.0f).setSizeY(5.0f).setSizeZ(5.0f).setChamferSize(1.0f).realizeMesh("roundedBoxMesh");<br>
<br />putMesh("roundedBoxMesh",new Vector3(20,10,10));<br>
<br />new SpringGenerator().setNumSegCircle(32).setNumSegPath(30).realizeMesh("springMesh");<br>
<br />putMesh("springMesh",new Vector3(20,10,0));<br>
</td></tr>
</table>
<br>
<br>
<hr/><br>
<br>
<br>
<img src='http://ogre-procedural.googlecode.com/files/Extrusion.jpg' />
<img src='http://ogre-procedural.googlecode.com/files/Primitives.jpg' /></blockquote></blockquote></li></ul>

<img src='http://mogre-procedural.googlecode.com/svn/screenshot04102014_120405437.png' />