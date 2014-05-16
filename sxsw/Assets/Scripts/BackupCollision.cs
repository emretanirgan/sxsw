using UnityEngine;
using System.Collections;
//using System;
//using System.Runtime.InteropServices;

public class BackupCollision : MonoBehaviour {
	public int life = 3;
	public Vector3 positionori;
	public AudioSource deathas;
	public AudioClip deathclip;
	public AudioSource fireCatchas;
	public AudioClip fireCatchclip;
	trial globalObj;
	// Use this for initialization
	void Start () {
		GameObject g = GameObject.Find ("Global Object");
		globalObj = g.GetComponent<trial> ();
		if (globalObj == null) {
			Debug.Log("NULL Reference\n");
				}
		deathas = (AudioSource)gameObject.AddComponent<AudioSource> ();
		deathas.clip = deathclip;
		fireCatchas = (AudioSource)gameObject.AddComponent<AudioSource> ();
		fireCatchas.clip = fireCatchclip;
		PlayerPrefs.SetString ("result", "lose");
		positionori = gameObject.transform.position;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log (collider.name);
		if (collider.tag != "portal") {
			if (globalObj == null){
				Debug.Log("ERROR: globalObj IS NULL\n");
			}
			else
				globalObj.transferred = 0;
		}
		if (collider.tag == "fire" || collider.tag == "water" || collider.tag == "gear") 
		{

			if(collider.tag == "fire")
			{
				Debug.Log("fire sound");
				fireCatchas.Play ();
			}
			deathas.Play();
			life --;
			if (life > 0)
					reSet ();
			else 
			{
				Destroy (gameObject);
				Application.LoadLevel (1);
			}
		} 

	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if (hit.collider.tag == "door") 
		{
			PlayerPrefs.SetString ("result", "win");
			Application.LoadLevel (1);			
		}
	}


	void reSet()
	{
		gameObject.transform.position = positionori;
	}
}
