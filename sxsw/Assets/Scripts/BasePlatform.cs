﻿using UnityEngine;
using System.Collections;

 public class BasePlatform : MonoBehaviour {

	
	public virtual void OnTriggerEnter(Collider other) {
		
	}
	
	
	public virtual void OnCollisionEnter(Collision collision) {
		OnTriggerEnter(collision.collider);		
	}
}
