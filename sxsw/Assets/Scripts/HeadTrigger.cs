using UnityEngine;
using System.Collections;


public class HeadTrigger : MonoBehaviour {

	private Player thisPlayer;

	// Use this for initialization
	void Start () 
	{
		thisPlayer = transform.parent.GetComponent<Player>();
	}
	
	
	
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.gameObject.name);
		//		Destroy(other.gameObject);
		
//		HOTween.To(gameObject.transform, 1, "position", new Vector3(10,20,30));
		if(other.gameObject.layer == LayerMask.NameToLayer("Leg Trigger"))
		   thisPlayer.Dead();
		
	}
}
