using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


public class CreateMesh : MonoBehaviour {

	public Material blockMaterial;
	Mesh m;
	// Use this for initialization
	void Start () {
		/*
		Vector2 v1 = new Vector2(0,0);
		Vector2 v2 = new Vector2(10,0);
		Vector2 v3 = new Vector2(10,10);
		Vector2 v4 = new Vector2(0,10);*/

		/*
		float[] xcoords = new float[4];
		xcoords[0] = 0;
		xcoords[1] = 10;
		xcoords[2] = 10;
		xcoords[3] = 0;

		float[] ycoords = new float[4];
		xcoords[0] = 0;
		xcoords[1] = 0;
		xcoords[2] = 10;
		xcoords[3] = 10;*/

		/*
		Vector2 [] verts = new Vector2[4];
		verts[0] = v1;
		verts[1] = v2;
		verts[2] = v3;
		verts[3] = v4;
		m = extrudeMeshOld (verts);

		GameObject go = new GameObject();



		// Set up game object with mesh;
		go.AddComponent(typeof(MeshRenderer));
		go.GetComponent<MeshRenderer>().material = blockMaterial;
		MeshFilter filter = go.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = m;
		go.AddComponent (typeof(MeshCollider));
		go.GetComponent<MeshCollider>().sharedMesh = m;
		go.layer = LayerMask.NameToLayer ("Collisions");*/
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public Mesh extrudeMesh(float [] polyX, float [] polyY, int size, Vector3 center)
		
	{

		Vector2 [] poly = new Vector2[size];
		for(int i=0; i<size; i++){
			Vector2 v = new Vector2(polyX[i]-center.x, polyY[i]-center.y);
			poly[i] = v;
		}
		
		// convert polygon to triangles
		
		Triangulator triangulator = new Triangulator(poly);
		
		int[] tris = triangulator.Triangulate();
		
		Mesh m = new Mesh();
		
		Vector3[] vertices = new Vector3[poly.Length*2];
		
		
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			vertices[i].x = polyX[i]-center.x;
			
			vertices[i].y = polyY[i]-center.y;
			
			vertices[i].z = -10; // front vertex
			
			vertices[i+poly.Length].x = polyX[i]-center.x;
			
			vertices[i+poly.Length].y = polyY[i]-center.y;
			
			vertices[i+poly.Length].z = 10;  // back vertex     
			
		}
		
		int[] triangles = new int[tris.Length*2+poly.Length*6];
		
		int count_tris = 0;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[i] = tris[i];
			
			triangles[i+1] = tris[i+1];
			
			triangles[i+2] = tris[i+2];
			
		} // front vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[count_tris+i] = tris[i+2]+poly.Length;
			
			triangles[count_tris+i+1] = tris[i+1]+poly.Length;
			
			triangles[count_tris+i+2] = tris[i]+poly.Length;
			
		} // back vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			// triangles around the perimeter of the object

			int n = (i+1)%poly.Length;

			/*triangles[count_tris] = i;
			
			triangles[count_tris+1] = i + poly.Length;
			
			triangles[count_tris+2] = n;
			
			triangles[count_tris+3] = n;
			
			triangles[count_tris+4] = i + poly.Length;
			
			triangles[count_tris+5] = n + poly.Length;*/
			
			triangles[count_tris] = i;
			
			triangles[count_tris+1] = n;
			
			triangles[count_tris+2] = i + poly.Length;
			
			triangles[count_tris+3] = i + poly.Length;
			
			triangles[count_tris+4] = n;
			
			triangles[count_tris+5] = n + poly.Length;
			
			count_tris += 6;
			
		}

		Vector3[] norms = new Vector3[m.vertices.Length];
		/*m.normals = new Vector3[m.vertices.Length];
		for(int i=0; i<m.normals.Length; i++){
			m.normals[i] = (m.vertices[i]-center).normalized;
		}*/
		
		m.vertices = vertices;
		
		m.triangles = triangles;

		m.RecalculateNormals();
		
		m.RecalculateBounds();
		
		m.Optimize();
		
		return m;
		
	}

	public Mesh extrudeMeshReverse(float [] polyX, float [] polyY, int size, Vector3 center)
		
	{
		
		Vector2 [] poly = new Vector2[size];
		for(int i=0; i<size; i++){
			Vector2 v = new Vector2(polyX[i]-center.x, polyY[i]-center.y);
			poly[i] = v;
		}
		
		// convert polygon to triangles
		
		Triangulator triangulator = new Triangulator(poly);
		
		int[] tris = triangulator.Triangulate();
		
		Mesh m = new Mesh();
		
		Vector3[] vertices = new Vector3[poly.Length*2];
		
		
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			vertices[i].x = polyX[i]-center.x;
			
			vertices[i].y = polyY[i]-center.y;
			
			vertices[i].z = -10; // front vertex
			
			vertices[i+poly.Length].x = polyX[i]-center.x;
			
			vertices[i+poly.Length].y = polyY[i]-center.y;
			
			vertices[i+poly.Length].z = 10;  // back vertex     
			
		}
		
		int[] triangles = new int[tris.Length*2+poly.Length*6];
		
		int count_tris = 0;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[i] = tris[i];
			
			triangles[i+1] = tris[i+1];
			
			triangles[i+2] = tris[i+2];
			
		} // front vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[count_tris+i] = tris[i+2]+poly.Length;
			
			triangles[count_tris+i+1] = tris[i+1]+poly.Length;
			
			triangles[count_tris+i+2] = tris[i]+poly.Length;
			
		} // back vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			// triangles around the perimeter of the object
			
			int n = (i+1)%poly.Length;
			
			triangles[count_tris] = i;
			
			triangles[count_tris+1] = i + poly.Length;
			
			triangles[count_tris+2] = n;
			
			triangles[count_tris+3] = n;
			
			triangles[count_tris+4] = i + poly.Length;
			
			triangles[count_tris+5] = n + poly.Length;

			count_tris += 6;
			
		}
		
		Vector3[] norms = new Vector3[m.vertices.Length];
		/*m.normals = new Vector3[m.vertices.Length];
		for(int i=0; i<m.normals.Length; i++){
			m.normals[i] = (m.vertices[i]-center).normalized;
		}*/
		
		m.vertices = vertices;
		
		m.triangles = triangles;
		
		m.RecalculateNormals();
		
		m.RecalculateBounds();
		
		m.Optimize();
		
		return m;
		
	}

	Mesh extrudeMeshOld(Vector2 [] poly)
		
	{
		
		// convert polygon to triangles
		
		Triangulator triangulator = new Triangulator(poly);
		
		int[] tris = triangulator.Triangulate();
		
		Mesh m = new Mesh();
		
		Vector3[] vertices = new Vector3[poly.Length*2];
		
		
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			vertices[i].x = poly[i].x;
			
			vertices[i].y = poly[i].y;
			
			vertices[i].z = -10; // front vertex
			
			vertices[i+poly.Length].x = poly[i].x;
			
			vertices[i+poly.Length].y = poly[i].y;
			
			vertices[i+poly.Length].z = 10;  // back vertex     
			
		}
		
		int[] triangles = new int[tris.Length*2+poly.Length*6];
		
		int count_tris = 0;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[i] = tris[i];
			
			triangles[i+1] = tris[i+1];
			
			triangles[i+2] = tris[i+2];
			
		} // front vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<tris.Length;i+=3)
			
		{
			
			triangles[count_tris+i] = tris[i+2]+poly.Length;
			
			triangles[count_tris+i+1] = tris[i+1]+poly.Length;
			
			triangles[count_tris+i+2] = tris[i]+poly.Length;
			
		} // back vertices
		
		count_tris+=tris.Length;
		
		for(int i=0;i<poly.Length;i++)
			
		{
			
			// triangles around the perimeter of the object
			
			int n = (i+1)%poly.Length;
			
			/*triangles[count_tris] = i;
			
			triangles[count_tris+1] = i + poly.Length;
			
			triangles[count_tris+2] = n;
			
			triangles[count_tris+3] = n;
			
			triangles[count_tris+4] = i + poly.Length;
			
			triangles[count_tris+5] = n + poly.Length;*/

			triangles[count_tris] = i;
			
			triangles[count_tris+1] = n;
			
			triangles[count_tris+2] = i + poly.Length;
			
			triangles[count_tris+3] = i + poly.Length;
			
			triangles[count_tris+4] = n;
			
			triangles[count_tris+5] = n + poly.Length;
			
			count_tris += 6;
			
		}
		
		m.vertices = vertices;
		
		m.triangles = triangles;

		m.RecalculateNormals();
		
		m.RecalculateBounds();
		
		m.Optimize();
		
		return m;
		
	}
}
