using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
	public Vector2 speed;
	
	public Texture[] backgrounds;
	
	public enum Backgrounds
	{
		One,
		Two, 
		Three
	}

	void Update()
	{
		renderer.material.mainTextureOffset += speed * Time.deltaTime;
//		renderer.material.mainTextureOffset += speed * Random.value * Mathf.Sin(Time.time) * 0.005f; 
	}
	
	public void SetTexture(Backgrounds bg)
	{
		renderer.material.mainTexture =  backgrounds[(int)bg];	
	}
	
	public void SetRandomTexture()
	{
		renderer.material.mainTexture =  backgrounds[Random.Range(0, backgrounds.Length)];	
	}
}
