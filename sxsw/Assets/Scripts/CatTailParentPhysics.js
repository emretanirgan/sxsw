#pragma strict

private var thisParent : Transform;
private var thisRigidbody : Rigidbody;

private var parentPosLastFrame : Vector3 = Vector3.zero;

function Awake () {
	thisParent = transform.parent;
	thisRigidbody = transform.GetComponent.< Rigidbody > ();
}

function Update () {
	thisRigidbody.AddForce ( ( parentPosLastFrame - thisParent.position ) * 100 );
	parentPosLastFrame = thisParent.position;
}