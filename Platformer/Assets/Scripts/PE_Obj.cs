using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public float 		stunStrength=2; 
	public float 		stunDelay = .45f;
	public bool 		stunned = false;
	public bool 		deleted = false;
	public bool			still = false;
	public PE_Collider	coll = PE_Collider.sphere;
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector3		acc = Vector3.zero;
	
	public Vector3		vel = Vector3.zero;
	public Vector3		vel0 = Vector3.zero;
	public Vector3		velRel = Vector3.zero;
	
	public Vector3		_pos0 = Vector3.zero;
	public Vector3		_pos1 = Vector3.zero;

	public bool			isClimbing = false;
	public bool 		climbable = false;
	public PE_Dir		dir = PE_Dir.still;
	
	public PE_Obj		ground = null; // Stores whether this is on the ground

	private bool collide_left = false;
	
	public Vector3		pos0 {
		get { return( _pos0); }
		set {
			float d = (value - _pos0).magnitude;
			if (d > 1 && gameObject.name == "PC") {
				Debug.Log ("Big change in pos0!");
			}
			_pos0 = value;
		}
	}
	
	public Vector3		pos1 {
		get { return( _pos1); }
		set {
			float d = (value - _pos1).magnitude;
			if (d > 1 && gameObject.name == "PC") {
				Debug.Log ("Big change in pos1!");
			}
			_pos1 = value;
		}
	}

	void Start() {
		if (PhysEngine.objs.IndexOf(this) == -1) {
			_pos1 = _pos0 = transform.position;
			PhysEngine.objs.Add(this);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	
	void OnTriggerEnter(Collider other) {
		if (this.tag == "Player" && isClimbing || other.tag == "Player" && other.GetComponent<PE_Obj>().isClimbing) {
//			print("you shall pass");
			return;		
		}
		if(this.tag == "Player" && other.gameObject.tag == "Ladder")
		{
			climbable = true;
			print ("found a ladder");
			if(Mathf.Abs(Input.GetAxis("Vertical")) > 0)
			{
				isClimbing = true;
				grav = PE_GravType.none;
				print ("updown input detected");
				float ladderX = other.transform.position.x;
				print (other.transform.position);
				Vector3 playerLoc = this.transform.position;
				print (playerLoc);
				playerLoc.x = ladderX;
				print (playerLoc);
				this.transform.position = playerLoc;
			}
			
			return;
			
		}
		//No collision if enemy
		if ((other.tag == "Enemy" || other.tag == "Cannonball") && this.tag == "Player") {
			print ("found a ball");
			PlayerHealth m_PlayerHealth = this.GetComponent<PlayerHealth> ();
			if (m_PlayerHealth.playerHealth <=0 || stunned == true)
				return;
			
			if (stunned == false) {
				PE_Obj nowPE_Obj = this.GetComponent<PE_Obj> ();
				if(m_PlayerHealth.invulnerable ==false)
					m_PlayerHealth.playerHealth-=10;
				if (m_PlayerHealth.playerHealth <=0) {
					Vector3 zeroVector = new Vector3 (0f, 0f, 0f);
					nowPE_Obj.vel = zeroVector;
					Animator animator = this.GetComponent<Animator> ();
					print ("You died!");
					animator.SetBool("death", true);
					PE_Controller m_pController = this.GetComponent<PE_Controller>();
					m_pController.enabled = false; 
					return;
				} else {
					Vector3 stunVel = nowPE_Obj.vel;
					Vector3 directionOfStun = this.transform.position - other.transform.position; 
					directionOfStun.Normalize ();
					//				print (directionOfStun);
					//stun normally if player is on same level or above the 
					if (directionOfStun.x >= 0) {
						stunVel.x = stunStrength;
						//					stunVel.y = stunStrength;
					} else {
						//					stunVel.y = stunStrength;
						stunVel.x = -1 * stunStrength;
						
					}
					
					PE_Controller myController = this.GetComponent<PE_Controller> ();
					
					myController.enabled = false;
					nowPE_Obj.vel = stunVel;
					StartCoroutine (bounceDelay (myController));
					
				}
				
				
				return;
			}
		}
		
		if (this.tag == "Enemy" && other.gameObject.tag == "Player") {
			return;		
		}
		// Ignore collisions of still objects
		if (still) return;
		
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		ResolveCollisionWith(otherPEO);
	}
	
	void OnTriggerStay(Collider other) {
		OnTriggerEnter(other);
	}
	
	void OnTriggerExit(Collider other) {
		if(this.tag == "Player" && other.gameObject.tag == "Ladder")
		{
			print ("exitLadder");
			isClimbing = false;
			climbable = false;
			grav = PE_GravType.constant;
		}
		
		// Ignore collisions of still objects
		if (still) return;
		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;
		
		// This sets ground to null if we fall off of the current ground
		// Jumping will also set ground to null
		if (ground == otherPEO) {
			ground = null;
		}
	}

	void OnDrawGizmos() {
		if (vel.magnitude != 0) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(a0,0.2f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(a0,a1);
			Gizmos.DrawWireSphere(a1,0.2f);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(a1,posFinal);
			Gizmos.DrawWireSphere(posFinal,0.2f);
		}
	}
	public Vector3 a0, a1, b, delta, pU, posFinal; // a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object

	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still
		Vector3 posFinal = pos1; // Sets a defaut value for posFinal
		
		switch (this.coll) {
		case PE_Collider.sphere:
			
			switch (that.coll) {
			case PE_Collider.sphere:
				// Sphere / Sphere collision
				float thisR, thatR, rad;
				// Note, this doesn't work with non-uniform or negative scales!
				thisR = Mathf.Max( this.transform.lossyScale.x, this.transform.lossyScale.y, this.transform.lossyScale.z ) / 2;
				thatR = Mathf.Max( that.transform.lossyScale.x, that.transform.lossyScale.y, that.transform.lossyScale.z ) / 2;
				rad = thisR + thatR;
				
				Vector3 delta = pos1 - that.transform.position;
				delta.Normalize();
				delta *= rad;
				
				posFinal = that.transform.position + delta;
				break;
			}
			
			break;
			
		case PE_Collider.aabb:
			
			switch (that.coll) {
			case PE_Collider.aabb:
				// AABB / AABB collision
				// Axis-Aligned Bounding Box
				// With AABB collisions, we're usually concerned with corners and deciding which corner to consider when making comparisons.
				// I believe that this corner should be determined by looking at the velocity of the moving body (this one)
				
				//Vector3 a0, a1, b; // a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object
				a0 = a1 = b = Vector3.zero;	 // Sets a default value to keep the compiler from complaining
			    delta = pos1 - pos0;
				
				if (dir == PE_Dir.down) {
					// If a0 was above b and a1 is below b resolve to be on top
					a1 = pos1;
					a1.y -= transform.lossyScale.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.y += that.transform.lossyScale.y/2f;
					if ( PhysEngine.GEQ( a0.y, b.y ) && b.y > a1.y) {
						posFinal.y += Mathf.Abs( a1.y - b.y );
						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break; // Exit this switch statement: switch (that.coll)
				}
				
				if (dir == PE_Dir.up) {
					// If a0 was below b and a1 is above b resolve to be below
					a1 = pos1;
					a1.y += transform.lossyScale.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.y -= that.transform.lossyScale.y/2f;
					if ( PhysEngine.LEQ( a0.y, b.y ) && b.y < a1.y) {
						posFinal.y -= Mathf.Abs( a1.y - b.y );
						// Handle vel
						vel.y = 0;
						
						break; // Exit this switch statement: switch (that.coll)
					}
				}
				
				if (dir == PE_Dir.upRight) { // Bottom, Left is the comparison corner
					a1 = pos1;
					a1.x += (transform.lossyScale.x/2f);
					a1.y += transform.lossyScale.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that.transform.lossyScale.x/2f;
					b.y -= that.transform.localScale.y/2f;
					//print ("resolve upright");
				}
				
				if (dir == PE_Dir.upLeft) { // Bottom, Right is the comparison corner
					a1 = pos1;
					if(this.gameObject.tag == "Player"){
					a1.x -= (transform.lossyScale.x/2f +0.9f);
					}
					else{
						a1.x -= (transform.lossyScale.x/2f);
					}
					a1.y += transform.lossyScale.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that.transform.lossyScale.x/2f;
					b.y -= that.transform.localScale.y/2f;
					//print ("resolve upleft");
				}
				
				if (dir == PE_Dir.downLeft) { // Top, Right is the comparison corner
					a1 = pos1;
					if(this.gameObject.tag == "Player"){
						a1.x -= (transform.lossyScale.x/2f +0.9f);
					}
					else{
					a1.x -= (transform.lossyScale.x/2f);
					}
					a1.y -= transform.lossyScale.y/2f;
					//a0 = a1;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that.transform.lossyScale.x/2f;
					b.y += that.transform.localScale.y/2f;
					//print ("resolve downleft");
				}
				
				if (dir == PE_Dir.downRight) { // Top, Left is the comparison corner
					a1 = pos1;
					a1.x += (transform.lossyScale.x/2f);
					a1.y -= transform.lossyScale.y/2f;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that.transform.lossyScale.x/2f;
					b.y += that.transform.localScale.y/2f;
					//print ("resolve downright");
				}
				
				// In the x dimension, find how far along the line segment between a0 and a1 we need to go to encounter b
				float u = (b.x - a0.x) / (a1.x - a0.x);
				
				// Determine this point using linear interpolation (see the appendix of the book)
				pU = (1-u)*a0 + u*a1;

				// Find distance we would have to offset in x or y
				float offsetX = Mathf.Abs(a1.x - b.x);
				float offsetY = Mathf.Abs(a1.y - b.y);
				bool usedX = false;
				
				// Use pU.y vs. b.y to tell which side of PE_Obj "that" PE_Obj "this" should be on
				switch (dir) {
				case PE_Dir.upRight:
					if (pU.y > b.y) { // hit the left side
						posFinal.x -= Mathf.Abs(a1.x - b.x);
						
						// Handle vel
						vel.x = 0;
						usedX = true;
					} else { // hit the bottom
						posFinal.y -= Mathf.Abs(a1.y - b.y);
						
						// Handle vel
						vel.y = 0;
						
					}
					break;
					
				case PE_Dir.downRight:
					if (pU.y < b.y) { // hit the left side
						posFinal.x -= Mathf.Abs(a1.x - b.x);
						
						// Handle vel
						vel.x = 0;
						usedX = true;
						
					} else { // hit the top
						posFinal.y += Mathf.Abs(a1.y - b.y);
						
						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break;
					
				case PE_Dir.upLeft:
					if (pU.y > b.y) { // hit the right side
						posFinal.x += Mathf.Abs(a1.x - b.x);
						
						// Handle vel
						vel.x = 0;
						usedX = true;
					} else { // hit the bottom
						posFinal.y -= Mathf.Abs(a1.y - b.y);
						
						// Handle vel
						vel.y = 0;
						
					}
					break;
					
				case PE_Dir.downLeft:
					if (pU.y < b.y) { // hit the right side
						posFinal.x += Mathf.Abs(a1.x - b.x);
						
						// Handle vel
						vel.x = 0;
						usedX = true;
					} else { // hit the top
						posFinal.y += Mathf.Abs(b.y - a1.y);
						
						// Handle vel
						vel.y = 0;
						
						if (ground == null) ground = that;
					}
					break;
				}
				
				break;
			}
			
			break;
		}
		
		transform.position = pos1 = posFinal;
	}

	IEnumerator bounceDelay(PE_Controller myController) {
		stunned = true;
		Animator animator = this.GetComponent<Animator>();
		
		animator.SetBool("stunned", true);
		yield return new WaitForSeconds(stunDelay);
		myController.enabled = true;
		//to prevent stunlocking
		yield return new WaitForSeconds (.5f);
		animator.SetBool("stunned", false);
		
		
		stunned = false;
	}
}
