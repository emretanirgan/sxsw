using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public enum Players
{
	P1,
	P2
}

public class Player : MonoBehaviour {
	
	public Players thisPlayer = Players.P1;
	
	public float Gravity = 21f;	 //downward force
	public float TerminalVelocity = 20f;	//max downward speed
	public float JumpSpeed = 6f;
	public float springJumpSpeed = 300f;
	public float MoveSpeed = 10f;
	public float rotateSpeed = 5f;
	public float dodgeTime = 1.5f;
	public float dodgeSpeed = 50f;
	
	public AudioSource dodgeSound, footstepSound, deathMeow, popUpMeow, jumpOnTop; 
	
	public Vector3 MoveVector {get; set;}
	private Vector3 lastNonZeroMoveVector;
	public float VerticalVelocity {get; set;}
	
	public CharacterController CharController;
	
	private Animator anim;
	private bool isDodging = false;
	private Vector3 dodgeVector = Vector3.zero;
	private bool skipGravity = false;
	
	private string horizontalAxis = "Horizontal";
	private string dodgeButton = "Dodge";
	private string jumpButton = "Jump";
	
	private string joystickHorizontalAxis = "Joystick Horizontal";
	private string joystickDodgeButton = "Joystick Dodge";
	private string joystickJumpButton = "Joystick Jump";
	
	private bool isDead = false;
	private float startZ;
	
	// Use this for initialization
	void Awake () {
		CharController = gameObject.GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		
		if(thisPlayer == Players.P1)
		{
			horizontalAxis = "Horizontal";
			dodgeButton = "Dodge";
			jumpButton = "Jump";
			
			joystickHorizontalAxis = "Joystick Horizontal";
			joystickDodgeButton = "Joystick Dodge";
		    joystickJumpButton = "Joystick Jump";
		}
		else
		{
		    horizontalAxis = "Horizontal 2";
			dodgeButton = "Dodge 2";
			jumpButton = "Jump 2";
			
			joystickHorizontalAxis = "Joystick Horizontal 2";
			joystickDodgeButton = "Joystick Dodge 2";
			joystickJumpButton = "Joystick Jump 2";
			
//			this.enabled = false;		
		}
		
		lastNonZeroMoveVector = (thisPlayer == Players.P1) ?  Vector3.right : Vector3.left;
		startZ = transform.position.z;
		
	}
	
	// Update is called once per frame
	void Update () {
		// Popping back up while stationary changes z pos for some reason. Hack Fix !
		if(Mathf.RoundToInt(transform.position.z) != Mathf.Round(startZ))
		{
//			Debug.Log("resetting z");
//			MoveVector = Vector3.right;
			transform.position = new Vector3(transform.position.x, transform.position.y, startZ);
		}
	
		if(CharController.isGrounded)
		{
			anim.SetBool("Jumping", false);
		}
		
		checkMovement();
		HandleActionInput();
		processMovement();
	}
	
	void checkMovement(){
		if(isDodging)
			return;
			
		
	
		//move l/r
		var deadZone = 0.1f;
		VerticalVelocity = MoveVector.y;
		MoveVector = Vector3.zero;
//		if(Input.GetAxis(“Horizontal”) > deadZone || Input.GetAxis(“Horizontal”) < -deadZone){
//		if(Input.GetAxis(horizontalAxis) > deadZone || Input.GetAxis(horizontalAxis) < -deadZone)
		
		float horInp = Mathf.Abs(Input.GetAxis(joystickHorizontalAxis)) > deadZone ? Input.GetAxis(joystickHorizontalAxis) : Input.GetAxis(horizontalAxis);
		if(Mathf.Abs(horInp) > deadZone) 
		{
//			Debug.Log("horINp = " + horInp);
			MoveVector += new Vector3(horInp,0,0);
			
			if(Input.GetButtonDown(dodgeButton) || Input.GetButtonDown(joystickDodgeButton))
			{
				StartCoroutine(Dodge());
			}
		}
		
		
		

		//jump
		
	}
	
	void HandleActionInput(){
		
		if(Input.GetButton(jumpButton) || Input.GetButton(joystickJumpButton)){
			jump();
		}
		
		
	}
	
	void processMovement()
	{	
		if(isDodging)
				MoveVector = dodgeVector;
		
		if(MoveVector.x != 0)
		{
			//face correct direction
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveVector , Vector3.up), rotateSpeed * Time.deltaTime);
			lastNonZeroMoveVector = new Vector3(MoveVector.x, 0, 0);
			
		}
		else
		{
			
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastNonZeroMoveVector , Vector3.up), rotateSpeed * Time.deltaTime);
		}
		
		//normalize moveVector if magnitude > 1
		if(MoveVector.magnitude > 1){
			MoveVector = Vector3.Normalize(MoveVector);
			
			
		}
		
		 
		//multiply moveVector by moveSpeed
		
		if(isDodging)
		{
			MoveVector *= dodgeSpeed;
		}
		else
		{
			MoveVector *= MoveSpeed;
		}
		
						
		//reapply vertical velocity to moveVector.y
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
		
		//apply gravity
		if(!isDodging)
			applyGravity();
		else
			Debug.Log("skipping applying gravity");
			
		
		//set speed for animator
		anim.SetFloat("Speed", Mathf.Abs(MoveVector.x));
		
		//move character in world-space
		CharController.Move(MoveVector * Time.deltaTime);
		
		
	}
	
	void applyGravity(){
		if(MoveVector.y > -TerminalVelocity)
		{
//			MoveVector = new Vector3(MoveVector.x, (MoveVector.y – Gravity * Time.deltaTime), MoveVector.z);
//			MoveVector.Set(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - (Gravity * Time.deltaTime), MoveVector.z);
		}
		if(CharController.isGrounded && MoveVector.y < -1){
			MoveVector = new Vector3(MoveVector.x, (-1), MoveVector.z);
		}
	}
	
	public void jump(){
		if(CharController.isGrounded){
			anim.SetBool("Jumping", true);
			VerticalVelocity = JumpSpeed;
		}
	}
	
	
	public void SpringJump()
	{
//		if(CharController.isGrounded){
			anim.SetBool("Jumping", true);
			VerticalVelocity = springJumpSpeed;
//		}	
		
		Debug.Log("in spring jump");
		
		processMovement();
	}
	
	public void JumpedOnTop()
	{
		anim.SetBool("Jumping", true);
		VerticalVelocity = JumpSpeed;
		
		if(!jumpOnTop.isPlaying)
			jumpOnTop.Play();
		
		processMovement();
	
	}
	
//	public void S
	
	void OnTriggerEnter(Collider other) {
		Debug.Log(other.gameObject.name);
//		Destroy(other.gameObject);
	}
	
	public void Dead()
	{
		if(isDead)
			return;		
		
		Toolbox.Instance.PlayerWon(thisPlayer == Players.P1 ? Players.P2 : Players.P1);
				
		
		
		isDead = true;
		
		if(!deathMeow.isPlaying)
			deathMeow.Play();
			
		Shrink();
		StartCoroutine(WaitAndPopBackUp(3f));		
	}
	
	void Shrink()
	{
		HOTween.Kill(gameObject.transform);
		TweenParms parms = new TweenParms().Prop("localScale", new Vector3(2f,0.75f,2f)).Ease(EaseType.EaseInBack);
		HOTween.To(gameObject.transform, 0.2f, parms );
	}
	
	IEnumerator WaitAndPopBackUp(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		PopBackUp();
	}
	
	
	void PopBackUp()
	{
		// Make charcter jump
		jump();
		lastNonZeroMoveVector = (thisPlayer == Players.P1) ?  Vector3.right : Vector3.left;
		processMovement();
		
		if(!popUpMeow.isPlaying)
			popUpMeow.Play();
		
		
		TweenParms parms = new TweenParms().Prop("localScale", new Vector3(2f,2f,2f)).Ease(EaseType.EaseOutBack);
		HOTween.To(gameObject.transform, 1f, parms );
		
		isDead = false;
	
	}
	
	IEnumerator Dodge()
	{
//		Debug.Log("started dodging");
		dodgeSound.Play();
		Vector3 startPos = transform.position;
		
		isDodging = true;
		anim.SetBool("isDodging", true);
		dodgeVector = new Vector3(MoveVector.x, 0f, 0f);
		
//		float moveSpeedCopy = MoveSpeed;
//		MoveSpeed = dodgeSpeed;
		yield return new WaitForSeconds(dodgeTime);
		
		dodgeVector = Vector3.zero;
		isDodging = false;
		anim.SetBool("isDodging", false);
		
//		Debug.Log("ended dodging");
		Debug.Log("Dodge distance = " + (transform.position - startPos).magnitude);
		
	
	}
	
	void PlayFootstepSound()
	{
		if(CharController.isGrounded)
			footstepSound.Play();
	
	}
	
	void EnableTrail()
	{
		gameObject.GetComponent<TrailRenderer>().enabled = true;	
	}
	
	void DisableTrail()
	{
		gameObject.GetComponent<TrailRenderer>().enabled = false;	
	}
}