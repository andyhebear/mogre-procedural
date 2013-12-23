using System;
using System.Collections.Generic;
using System.Text;

using Mogre;
using Math=Mogre.Math;

namespace Mogre_Procedural
{
/// This class creates a visualisation of the normals of a TriangleBuffer
//C++ TO C# CONVERTER WARNING: The original type declaration contained unconverted modifiers:
//ORIGINAL LINE: class _ProceduralExport ShowNormalsGenerator
public class ShowNormalsGenerator
{
	public enum VisualStyle: int
	{
		VS_LINE,
		VS_ARROW
	}

	private VisualStyle mVisualStyle;

	private TriangleBuffer mTriangleBuffer;

	private float mSize = 0f;
	public ShowNormalsGenerator()
	{
		mTriangleBuffer = null;
		mSize = 1.0f;
		mVisualStyle = VisualStyle.VS_LINE;
	}

	/// Sets the input Triangle Buffer
	public ShowNormalsGenerator setTriangleBuffer(TriangleBuffer triangleBuffer)
	{
		mTriangleBuffer = triangleBuffer;
		return this;
	}

	/// Sets the size of the normals representation (default = 1.0)
	public ShowNormalsGenerator setSize(float size)
	{
		mSize = size;
		return this;
	}


	/// Sets the visual style, line or arrow (default = line)
	public ShowNormalsGenerator setVisualStyle(VisualStyle visualStyle)
	{
		mVisualStyle = visualStyle;
		return this;
	}


	/// Builds the normals representation as a manual object
	/// \exception Ogre::InvalidStateException The input triangle buffer must not be null
	/// \exception Ogre::InvalidStateException Scene Manager is not set in OGRE root object
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: ManualObject* buildManualObject() const
	public ManualObject buildManualObject()
	{
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
		ManualObject manual = sceneMgr.CreateManualObject();
		manual.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_LIST);
		const List<TriangleBuffer.Vertex> vertices = mTriangleBuffer.getVertices();
		for (List<TriangleBuffer.Vertex>.Enumerator it = vertices.GetEnumerator(); it.MoveNext(); ++it)
		{
			manual.position(it.mPosition);
			manual.position(it.mPosition + it.mNormal * mSize);
	
			if (mVisualStyle == VisualStyle.VS_ARROW)
			{
				Vector3 axis2 = it.mNormal.perpendicular();
				Vector3 axis3 = it.mNormal.crossProduct(axis2);
	
				manual.position(it.mPosition + it.mNormal * mSize);
				manual.position(it.mPosition + (.8f * it.mNormal + .1f * axis2) * mSize);
	
				manual.position(it.mPosition + it.mNormal * mSize);
				manual.position(it.mPosition + .8f * (it.mNormal - .1f * axis2) * mSize);
	
				manual.position(it.mPosition + it.mNormal * mSize);
				manual.position(it.mPosition + .8f * (it.mNormal + .1f * axis3)* mSize);
	
				manual.position(it.mPosition + it.mNormal * mSize);
				manual.position(it.mPosition + .8f * (it.mNormal - .1f * axis3)* mSize);
			}
		}
		manual.End();
	
		return manual;
	}

	/// Builds the normals representation as a mesh
	public MeshPtr buildMesh(string name)
	{
		return buildMesh(name, "General");
	}
	public MeshPtr buildMesh()
	{
		return buildMesh("", "General");
	}
//C++ TO C# CONVERTER WARNING: 'const' methods are not available in C#:
//ORIGINAL LINE: MeshPtr buildMesh(const string& name = "", const string& group = "General") const
//C++ TO C# CONVERTER NOTE: Overloaded method(s) are created above to convert the following method having default parameters:
	public MeshPtr buildMesh(string name, string group)
	{
		SceneManager sceneMgr = Root.Singleton.GetSceneManagerIterator().Current;
		ManualObject mo = buildManualObject();
		MeshPtr mesh = mo.ConvertToMesh(name, group);
	
		sceneMgr.DestroyManualObject(mo);
	
		return mesh;
	}
}
}


