using UnityEngine;
using System.Collections;

public class NewSpring : MonoBehaviour 
{
	private Player lastPlayerToTouch;
	

	Vector3 start_pos;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void FixedUpdate() {
//		Vector3 displacement = gameObject.transform.position - start_pos;
//		float k = 9.80f;
//		Vector3 force = -k * displacement - gameObject.rigidbody.velocity;
//		//Debug.Log ("force  : " + force);
//		gameObject.rigidbody.AddForce (force);
//	}

	void OnTriggerEnter(Collider collider)
	{
		Debug.Log("collided with " + collider.gameObject.name);
		
		if(collider.gameObject.layer == LayerMask.NameToLayer("Leg Trigger"))
		{
			Debug.Log("finding player script");
		
		    LegTrigger legTrigger = collider.GetComponent<LegTrigger>();
		    
			Player collidedPlayer = null;
		    if(legTrigger != null)
				collidedPlayer = legTrigger.thisPlayer;
	
			if(null != collidedPlayer)
			{
				Debug.Log("calling spring jump");
				collidedPlayer.SpringJump();
				audio.Play();
			
			}
		}
		
//		if(collider.gameObject.Getc
//		if(collider.
//		if (collider.tag == "Player")
//		{
//			spas.Play ();
//			gameObject.transform.position -= Vector3.up * 2;
//		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("collided with " + collision.gameObject.name);
		OnTriggerEnter(collision.collider);	
	
	}
	
	
	
	
	void OnTriggerExit(Collider other)
	{
	
	}
	
	public void SpringJump()
	{
		
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
