using UnityEngine;
using System.Collections;

public class TestCamtoWorldPlacement : MonoBehaviour {

	[Range(0f, 1f)]
	public float x, y;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 10f));
	}
}
