                                                                                                                                                                                                                                                                                                                                                using UnityEngine;
using System.Collections;

public class Global : MonoBehaviour {
	public int transferred;
	public int slipping;
	public float playerVelocity;
	// Use this for initialization
	void Start () {
		transferred = 0;
		slipping = 0;
		playerVelocity = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}