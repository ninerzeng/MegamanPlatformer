﻿using UnityEngine;
using System.Collections;

public class PE_Controller : MonoBehaviour {
	private PE_Obj peo;

	public Vector3	vel;
	public bool		grounded = false;
	public float	hSpeed = 10;
	public float	acceleration = 10;
	public float	jumpVel = 10f;
	public float	airSteeringAmt = 0.5f;
	public float	ladderSpeed = 1;
	public float	airMomentumX = 1f; // 0 for no momentum (i.e. 100% drag), 1 for total momentum
	public float	groundMomentumX = 0.1f;

	public Vector2	maxSpeed = new Vector2( 10, 30 ); // Different x & y to limit maximum falling velocity
	private Vector3 collider_size;
	private Vector3 collider_center;
	private float slide_start_time = -1f;
	public bool sliding = false;
	BoxCollider box;
	//false is left, true is right
	//public bool last_direction = true;

	private Animator animator;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		animator = GetComponent<Animator> ();
		box = GetComponent<BoxCollider>();
		collider_size = box.size;
		collider_center = box.center;
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		animator.SetBool("grounded",grounded);
		animator.SetBool ("sliding", sliding);

		if (Input.GetKeyUp ("left") || Input.GetKeyUp("right"))
			animator.SetBool ("running", false);
		else if(Mathf.Abs (Input.GetAxis("Horizontal" )) > 0) 
		{
			animator.SetBool ("running",true);
		}
			
		// Horizontal movement

//		if ((slide_start_time < 0f) || ((Time.time - slide_start_time) > 0.5f && slide_start_time >= 0f)) 
//		{
//			sliding = false;
//		}
		if (sliding == true) {
			if(vel.x>0){
				vel.x = 20f;
			}
			else if(vel.x<0){
				vel.x = -20f;
			}

		}
		if (slide_start_time < 0f) {
			sliding = false;
			} 
		else if (Time.time - slide_start_time > 0.35f && sliding==true) {
			if(vel.x>0){
				vel.x = 10f;
			}
			else if(vel.x<0){
				vel.x = -10f;
			}

			sliding = false;
			box.size = collider_size;
			box.center = collider_center;
			print ("end slide");
			}
		if (sliding == false) {
			if(peo.isClimbing == false)
				UpdateRun ();
			UpdateJump ();
			UpdateClimb ();
			UpdateSlide ();
		}
		peo.vel = vel;
	}
	void UpdateClimb()
	{
		if(peo.isClimbing )
		{
			//this is up
			if(Input.GetAxis("Vertical") > 0)
			{
				//turn off gravity
				//move up the ladder at a slow pace
				//did that for testing
//				this.transform.Translate(0,20 * Time.deltaTime,0);
				peo.grav = PE_GravType.none;
				Vector3 playerLoc = this.transform.position;
				vel.y = ladderSpeed;
				this.transform.position = playerLoc;
				SendMessage("onClimb",SendMessageOptions.DontRequireReceiver);
				animator.SetBool("grounded", true);
				animator.SetFloat("speed", 0f);
				animator.SetBool("climbing", true);
				animator.SetBool("climbIdle", false);
				
			}
			//this is down
			else if(Input.GetAxis("Vertical") < 0)
			{
				peo.grav = PE_GravType.none;
				Vector3 playerLoc = this.transform.position;
//				playerLoc.y = -ladderSpeed*Time.deltaTime;
				vel.y = -ladderSpeed;
				this.transform.position = playerLoc;
				
//				this.transform.Translate(0,-10 * Time.deltaTime,0);
				animator.SetBool("climbing", true);
				animator.SetBool("grounded", true);
				animator.SetFloat("speed", 0f);
				animator.SetBool("climbIdle", false);
				
				SendMessage("onClimb",SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				//if you are not moving then just go idle on the ladder
				vel = new Vector3(0f,0f,0f);
				animator.SetBool("grounded", true);
				animator.SetBool("climbIdle", true);
				animator.SetBool("climbing", false);
				
				SendMessage("onClimbIdle",SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
				animator.SetBool("climbing", false);
				animator.SetBool("climbIdle", false);	
		}
	}
	void UpdateRun()
	{
		float vX = Input.GetAxis("Horizontal"); // Returns a number [-1..1]
		vel.x = vX * hSpeed;
		//animator actions
//		animator.SetFloat ("speed", Mathf.Abs (vel.x));
		
		//left right facing
		if (vel.x > 0) 
		{
			transform.localScale = new Vector3 (1f, 1f, 1f);
			//last_direction = true;
		} else if (vel.x < 0) 
		{
			transform.localScale = new Vector3 (-1f, 1f, 1f);	
			//last_direction = false;
		}
	}
	void UpdateJump()
	{
		// Jumping with A (which is x or .)
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) 
		{
			
			// Jump if you're grounded
			if (grounded || peo.isClimbing) 
			{
				vel.y = jumpVel;
				peo.ground = null; // Jumping will set ground = null
			}
			peo.isClimbing = false;
		}
	}

	void UpdateSlide()
	{
		//Slide with S or Down
		slide_start_time = -1f;

		if (Input.GetKeyDown (KeyCode.S) || Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			if(grounded)
			{
				print ("start slide");
				sliding = true; 
				slide_start_time = Time.time; 
				//last for 0.5 seconds
				//increase speed momentarily
				Vector3 flat_speed = peo.vel;
				//peo.vel.x = 20f;
				//change collider box
				Vector3 new_collider_size;
				new_collider_size.x = 0.805f;
				new_collider_size.y = 0.65f;
				new_collider_size.z = 1f;
				Vector3 new_collider_center;
				new_collider_center.x = box.center.x;
				new_collider_center.y = -0.3f;
				new_collider_center.z = box.center.z;
				box.size = new_collider_size;
				box.center = new_collider_center;
				//change animation

			}
		}
	}
}
