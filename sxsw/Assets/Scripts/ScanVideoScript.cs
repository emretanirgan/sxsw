using UnityEngine;
using System.Collections;

public class ScanVideoScript : MonoBehaviour {

	public MovieTexture movieTex;

	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = movieTex;

		movieTex.loop = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void playScanAnim() {
		movieTex.Play();

	}
}
