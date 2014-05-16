using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
	public Vector2 speed;
	
	public Texture[] backgrounds;
	
	private int currentBg;
	
	public enum Backgrounds
	{
		One,
		Two, 
		Three
	}
	
	void Start()
	{
		currentBg = 0;
		
	}
	

	void Update()
	{
		renderer.material.mainTextureOffset += speed * Time.deltaTime;
//		renderer.material.mainTextureOffset += speed * Random.value * Mathf.Sin(Time.time) * 0.005f; 
	}
	
	public void SetTexture(Backgrounds bg)
	{
		currentBg = (int)bg;
		renderer.material.mainTexture =  backgrounds[currentBg];	
	}
	
	public void SetRandomTexture()
	{
		currentBg = Random.Range(0, backgrounds.Length);
		renderer.material.mainTexture =  backgrounds[currentBg];	
	}
	
	public void CycleTexture()
	{
		currentBg = (currentBg + 1) % backgrounds.Length;
		renderer.material.mainTexture =  backgrounds[Random.Range(0, backgrounds.Length)];	
	}
}
