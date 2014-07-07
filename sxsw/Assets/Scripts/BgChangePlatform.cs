using UnityEngine;
using System.Collections;

public class BgChangePlatform : BasePlatform {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void OnTriggerEnter (Collider other)
	{
		base.OnTriggerEnter (other);
		
		//Debug.Log("trigger in BgChangePlatform");
		if(other.gameObject.layer == LayerMask.NameToLayer("Leg Trigger"))
		{
		   Toolbox.Instance.bgMusic.PlayNextMusicClip();
			Toolbox.Instance.videoBackground.PlayNextVideo();
		}
	}
}
