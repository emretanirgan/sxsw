using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]
public class PlayerCharacterController : MonoBehaviour {
	
	// Player Handling
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 20.0F;
	
	
	private CharacterController controller;
	
	private Animator animator;
	private GameManager manager;
	
	private Vector3 moveDirection = Vector3.zero;
	
	private PlayerPhysics playerPhysics;
	

	void Start () 
	{
//		Debug.Log(Application.dataPath);
		
		animator = GetComponent<Animator>();
		manager = Camera.main.GetComponent<GameManager>();
		
		controller = GetComponent<CharacterController>();
//		animator.SetLayerWeight(1,1);
	}
	
	void Update()
	{
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		moveDirection = transform.TransformDirection(moveDirection);
		moveDirection *= speed;
		
		if (controller.isGrounded) {
			
			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
			
		}
		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	
	
	}
	
	/*void Update2 () {
	
		

		// If player is touching the ground
		if (playerPhysics.grounded) {
			amountToMove.y = 0;
			
			if (wallHolding) {
				wallHolding = false;
				animator.SetBool("Wall Hold", false);
			}
			
			// Jump logic
			if (jumping) {
				jumping = false;
				animator.SetBool("Jumping",false);
			}
			
			// Slide logic
			if (sliding) {
				if (Mathf.Abs(currentSpeed) < .25f || stopSliding) {
					stopSliding = false;
					sliding = false;
					animator.SetBool("Sliding",false);
					playerPhysics.ResetCollider();
				}
			}
			
			// Slide Input
//			if (Input.GetButtonDown("Slide")) {
//				if (Mathf.Abs(currentSpeed) > initiateSlideThreshold) {
//					sliding = true;
//					animator.SetBool("Sliding",true);
//					targetSpeed = 0;
//					
//					playerPhysics.SetCollider(new Vector3(10.3f,1.5f,3), new Vector3(.35f,.75f,0));
//				}
//			}
		}
		else {
			if (!wallHolding) {
				if (playerPhysics.canWallHold) {
					wallHolding = true;
					animator.SetBool("Wall Hold", true);
				}
			}
		}
		
		// Jump Input
		if (Input.GetButtonDown("Jump")) {
			if (sliding) {
				stopSliding = true;
			}
			else if (playerPhysics.grounded || wallHolding) {
				amountToMove.y = jumpHeight;
				jumping = true;
				animator.SetBool("Jumping",true);
				
				if (wallHolding) {
					wallHolding = false;
					animator.SetBool("Wall Hold", false);
				}
			}
		}
		
		
		// Set animator parameters
		animationSpeed = IncrementTowards(animationSpeed,Mathf.Abs(targetSpeed),acceleration);
		animator.SetFloat("Speed",animationSpeed);
		
		// Input
		moveDirX = Input.GetAxisRaw("Horizontal");
		if (!sliding) {
		//	float speed = (Input.GetButton("Run"))?runSpeed:walkSpeed;
			float speed = runSpeed;
			targetSpeed = moveDirX * speed;
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed,acceleration);
			
			// Face Direction
			if (moveDirX !=0 && !wallHolding) {
				transform.eulerAngles = (moveDirX>0)?Vector3.up * 180:Vector3.zero;
			}
		}
		else {
			currentSpeed = IncrementTowards(currentSpeed, targetSpeed,slideDeceleration);
		}
		
		// Set amount to move
		amountToMove.x = currentSpeed;
		
		if (wallHolding) {
			amountToMove.x = 0;
			if (Input.GetAxisRaw("Vertical") != -1) {
				amountToMove.y = 0;	
			}
		}
		
		if(amountToMove.y >  -maxGravityForce) 
			amountToMove.y -= gravity * Time.deltaTime;
		
//		Debug.Log("amt to move y = " + amountToMove.y);
		playerPhysics.Move(amountToMove * Time.deltaTime, moveDirX);
	
	} */

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Checkpoint") {
			manager.SetCheckpoint(c.transform.position);
		}
		if (c.tag == "Finish") {
			manager.EndLevel();
		}
	}
	
	// Increase n towards target by speed
	private float IncrementTowards(float n, float target, float a) {
		if (n == target) {
			return n;	
		}
		else {
			float dir = Mathf.Sign(target - n); // must n be increased or decreased to get closer to target
			n += a * Time.deltaTime * dir;
			return (dir == Mathf.Sign(target-n))? n: target; // if n has now passed target then return target, otherwise return n
		}
	}
}
