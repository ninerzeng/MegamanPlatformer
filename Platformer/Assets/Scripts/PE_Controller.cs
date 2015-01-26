using UnityEngine;
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

	private float slide_start_time = -1f;
	public bool sliding = false;

	private Animator animator;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	// Note that we use Update for input but FixedUpdate for physics. This is because Unity input is handled based on Update
	void Update () {
		vel = peo.vel; // Pull velocity from the PE_Obj
		grounded = (peo.ground != null);
		animator.SetBool("grounded",grounded);
		// Horizontal movement
//		if ((slide_start_time < 0f) || ((Time.time - slide_start_time) > 0.5f && slide_start_time >= 0f)) 
//		{
//			sliding = false;
//		}
		if (slide_start_time < 0f) {
			sliding = false;
			} 
		else if (Time.time - slide_start_time > 0.5f && sliding==true) {
			peo.vel = peo.vel/1.5f;
			sliding = false;
			print ("end slide");
			}
		if (sliding == false) {
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
				vel.y = ladderSpeed;
				SendMessage("onClimb",SendMessageOptions.DontRequireReceiver);
				animator.SetBool("climbing", true);
				animator.SetBool("climbIdle", false);
				
			}
			//this is down
			else if(Input.GetAxis("Vertical") < 0)
			{
				vel.y = -ladderSpeed;
				peo.grav = PE_GravType.none;
				
//				this.transform.Translate(0,-10 * Time.deltaTime,0);
				animator.SetBool("climbing", true);
				animator.SetBool("climbIdle", false);
				
				SendMessage("onClimb",SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				//if you are not moving then just go idle on the ladder
				
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
		animator.SetFloat ("speed", Mathf.Abs (vel.x));
		//left right facing
		if (vel.x > 0) 
		{
			transform.localScale = new Vector3 (1f, 1f, 1f);
		} else if (vel.x < 0) 
		{
			transform.localScale = new Vector3 (-1f, 1f, 1f);		
		}
	}
	void UpdateJump()
	{
		// Jumping with A (which is x or .)
		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Period)) 
		{
			// Jump if you're grounded
			if (grounded) 
			{
				vel.y = jumpVel;
				peo.ground = null; // Jumping will set ground = null
			}
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
				peo.vel = 1.5f * flat_speed;
				//change collider box
				//change animation

			}
		}
	}
}
