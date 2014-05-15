using UnityEngine;
using System.Collections;


public class PlayVideo : MonoBehaviour {


	public MovieTexture movieTex;
	public bool shouldLoop = true;
	
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = movieTex;
		movieTex.Play();
		movieTex.loop = shouldLoop;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
