using UnityEngine;
using System.Collections;
//using System;
//using System.Runtime.InteropServices;
/*
public class Portal : MonoBehaviour {
	public GameObject[] PortalList;
	public GameObject PortalTarget;
	public GameObject boomParticle;
	//trial globalObj;
	int i = 0, current;
	// Use this for initialization
	void Start () {
	
		GameObject g = GameObject.Find ("Global Object");
		globalObj = g.GetComponent<trial> ();
		if (globalObj == null){
			Debug.Log("ERROR: globalObj IS NULL\n");
		}
		
		PortalList = GameObject.FindGameObjectsWithTag ("portal");
		for (int j = 0; j<PortalList.Length; j++) {
			if(PortalList[j].gameObject == gameObject)
				current = j;
		}
		do {
						i = Random.Range (0, PortalList.Length);
				} while(i==current);
		PortalTarget = PortalList [i];

		Debug.Log ("target portal is:" + PortalTarget);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		if (globalObj == null){
			Debug.Log("ERROR: globalObj IS NULL\n");
		}
		else{
			if (collider.tag == "Player" && globalObj.transferred ==0){
				globalObj.transferred = 1;
				collider.gameObject.transform.position = PortalTarget.transform.position + Vector3.up*5;  // collider.gameObject.transform.position+Vector3.up*5
				GameObject BoomInstance = Instantiate (boomParticle, PortalTarget.transform.position+Vector3.up*16, Quaternion.Euler(-90,0,0)) as GameObject;
				GameObject BoomInstance2 = Instantiate (boomParticle, transform.position+Vector3.up*16, Quaternion.Euler(-90,0,0))as GameObject;
			}
		}
	}
}
*/