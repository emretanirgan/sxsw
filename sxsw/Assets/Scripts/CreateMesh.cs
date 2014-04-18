using UnityEngine;
using System.Collections;

public class CreateMesh : MonoBehaviour {
	
	Mesh m;
	// Use this for initialization
	void Start () {
		Vector2 v1 = new Vector2(0,0);
		Vector2 v2 = new Vector2(10,0);
		Vector2 v3 = new Vector2(10,10);
		Vector2 v4 = new Vector2(0,10);
		
		Vector2 [] verts = new Vector2[4];
		verts[0] = v1;
		verts[1] = v2;
		verts[2] = v3;
		verts[3] = v4;
		m = extrudeMesh (verts);
		
		// Set up game object with mesh;
		gameObject.AddComponent(typeof(MeshRenderer));
		MeshFilter filter = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;
		filter.mesh = m;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	Mesh extrudeMesh(Vector2 [] poly)
		
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
			
			triangles[count_tris] = i;
			
			triangles[count_tris+1] = i + poly.Length;
			
			triangles[count_tris+2] = n;
			
			triangles[count_tris+3] = n;
			
			triangles[count_tris+4] = n + poly.Length;
			
			triangles[count_tris+5] = i + poly.Length;
			
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
