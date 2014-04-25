using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {
	public GameObject[] PortalList;
	public GameObject PortalTarget;
	Global globalObj;
	int i = 0, current;
	// Use this for initialization
	void Start () {
	
		GameObject g = GameObject.Find ("Global Object");
		globalObj = g.GetComponent<Global> ();

		PortalList = GameObject.FindGameObjectsWithTag ("portal");
		for (int j = 0; j<PortalList.Length; j++) {
			if(PortalList[j].gameObject == gameObject)
				current = j;
		}
		do {
						i = Random.Range (0, PortalList.Length);
				} while(i==current);
		PortalTarget = PortalList [i];

		Debug.Log (PortalTarget);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Player" && globalObj.transferred ==0){
			globalObj.transferred = 1;
			collider.gameObject.transform.position = PortalTarget.transform.position;
			collider.gameObject.transform.position += Vector3.up*3;
			}
		}
}
