using UnityEngine;
using System.Collections;
//using System;
//using System.Runtime.InteropServices;

public class Slip : MonoBehaviour {
	
	//public AudioSource spas;
	//public AudioClip spclip;
	bool moveLeft = false;
	bool moveRight = false;

	bool onSlipery = false;

	Vector3 startPos;
	//trial globalObj;
	// Use this for initialization
	void Start () {
		//spas = (AudioSource)gameObject.AddComponent<AudioSource> ();
		//spas.clip = spclip;
		startPos = gameObject.transform.position;
		/*GameObject g = GameObject.Find ("Global Object");
		globalObj = g.GetComponent<trial> ();
		if (globalObj == null){
			Debug.Log("ERROR: globalObj IS NULL\n");
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void FixedUpdate() {
		Vector3 displacement = gameObject.transform.position - startPos;
		float k = 9.80f;
		Vector3 force = -k * displacement - gameObject.rigidbody.velocity;
		gameObject.rigidbody.AddForce (force);
	/*	if (moveLeft) {
			gameObject.transform.position -= new Vector3(1.0f,0.0f,0.0f);
			moveLeft = false;
				}
		if (moveRight) {
			gameObject.transform.position += new Vector3(1.0f,0.0f,0.0f);
			moveRight = false;
				}*/

		if (globalObj == null){
			Debug.Log("ERROR: globalObj IS NULL\n");
		}
		else{
			if (globalObj.slipping == 1) {
				if(Input.GetAxisRaw("Horizontal") == -1)
				gameObject.transform.position -= Vector3.left * 0.1f;
				if(Input.GetAxisRaw("Horizontal") ==1)
			 	gameObject.transform.position -= Vector3.right * 0.1f;

			}
		}
	}
	
	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Player")
		{
			if (globalObj == null){
				Debug.Log("ENTER ERROR: globalObj IS NULL\n");
			}
			else
				globalObj.slipping = 1;
			onSlipery = true;

		
			//gameObject.transform.position -= Vector3.left * 2;
		/*	Debug.Log("**********");
			Debug.Log(collider.gameObject.tag);
			Debug.Log("**********");*/
			Vector3 curPos = collider.gameObject.transform.position;
		//	Debug.Log(curPos);
		//	Debug.Log("**********");
			//collider.gameObject.transform.position += Vector3.up * 2;
			curPos[1] = startPos[1];
			//curPos += Vector3.up*3;
			//collider.gameObject.transform.position = curPos;
		/*	if((curPos - startPos)[0] < 0){ //left of platform
				gameObject.transform.position -= Vector3.left * 2;
			}
			else{
				gameObject.transform.position += Vector3.left * 2;
			}*/
			//Destroy(collider.gameObject);
			//gameObject.transform.position -= Vector3.up * 2;
		}
	}

	void OnCollisionEnter(UnityEngine.Collision collision){

	
	}
		//	{
		//		if (collision.collider.tag == "Player")
		//		{
		//			spas.Play ();
		//			gameObject.transform.position -= Vector3.up * 2;
		//		}
		//	}

	void OnTriggerExit(Collider collider){
		if (collider.tag == "Player") {
			if (globalObj == null){
				Debug.Log("EXIT ERROR: globalObj IS NULL\n");
			}
			else
				globalObj.slipping = 0;

			onSlipery = false;
		}
	}
	//	void OnCollisionEnter(UnityEngine.Collision collision)
	//	{
	//		if (collision.collider.tag == "Player")
	//		{
	//			spas.Play ();
	//			gameObject.transform.position -= Vector3.up * 2;
	//		}
	//	}
	//	void OnCollisionEnter(Collision collision){
	//		Debug.Log (collision.collider.name);
	//		if (collision.collider.tag == "Player")
	//						Debug.Log ("abc");
	//			gameObject.transform.position -= Vector3.up * 2;
	//	}
}
