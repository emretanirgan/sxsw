using UnityEngine;
using System.Collections;


public class HeadTrigger : MonoBehaviour {

	public Player thisPlayer;

	// Use this for initialization
	void Start () 
	{
//		thisPlayer = transform.parent.GetComponent<Player>();
	}
	
	void OnCollisionEnter(Collision collision) {
		OnTriggerEnter(collision.collider);
	}
	
	void OnTriggerEnter(Collider other) {
//		Debug.Log(other.gameObject.name);
		//		Destroy(other.gameObject);
		
//		HOTween.To(gameObject.transform, 1, "position", new Vector3(10,20,30));
		if(other.gameObject.layer == LayerMask.NameToLayer("Leg Trigger"))
		{
		   thisPlayer.Dead();
			other.SendMessageUpwards("JumpedOnTop", SendMessageOptions.RequireReceiver);
		}
		
	}
}
