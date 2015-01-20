using UnityEngine;
using System.Collections;



public class PE_Obj : MonoBehaviour {
	public bool			still = false;
	public PE_Collider	coll = PE_Collider.aabb;
	public PE_GravType	grav = PE_GravType.constant;
	
	public Vector2		acc = Vector2.zero;

	public Vector2		vel = Vector2.zero;
	public Vector2		vel0 = Vector2.zero;

	public Vector2		pos0 = Vector2.zero;
	public Vector2		pos1 = Vector2.zero;

	public PE_Dir		dir = PE_Dir.still;

	public PE_Obj		ground = null; // Stores whether this is on the ground


	void Start() {
		if (PhysEngine.objs.IndexOf(this) == -1) {
			PhysEngine.objs.Add(this);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}


	void OnTriggerEnter2D(Collider2D other) {
		// Ignore collisions of still objects
		if (still) return;

		PE_Obj otherPEO = other.GetComponent<PE_Obj>();
		if (otherPEO == null) return;

		ResolveCollisionWith(otherPEO);
	}

	void OnTriggerStay2D(Collider2D other) {
		OnTriggerEnter2D(other);
	}

	void OnTriggerExit2D(Collider2D other) {
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

	void ResolveCollisionWith(PE_Obj that) {
		// Assumes that "that" is still
		Vector3 posFinal = pos1; // Sets a defaut value for posFinal

		switch (this.coll) {
		case PE_Collider.sphere:

			switch (that.coll) {
			case PE_Collider.sphere:
				print ("should not be here");
//				// Sphere / Sphere collision
//				float thisR, thatR, rad;
//				// Note, this doesn't work with non-uniform or negative scales!
//				thisR = Mathf.Max( this.transform.lossyScale.x, this.transform.lossyScale.y, this.transform.lossyScale.z ) / 2;
//				thatR = Mathf.Max( that.transform.lossyScale.x, that.transform.lossyScale.y, that.transform.lossyScale.z ) / 2;
//				rad = thisR + thatR;
//
//				Vector3 delta = pos1 - that.transform.position;
//				delta.Normalize();
//				delta *= rad;
//
//				posFinal = that.transform.position + delta;
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

				Vector2 a0, a1, b; // a0-moving corner last frame, a1-moving corner now, b-comparison corner on other object
				a0 = a1 = b = Vector2.zero;	 // Sets a default value to keep the compiler from complaining
				Vector2 delta = pos1 - pos0;

				Vector2 this_size = this.gameObject.GetComponent<BoxCollider2D>().size;
				Vector2 that_size = that.gameObject.GetComponent<BoxCollider2D>().size;
				Vector2 this_center = this.gameObject.GetComponent<BoxCollider2D>().center;
				Vector2 that_center = that.gameObject.GetComponent<BoxCollider2D>().center;
				
				float this_scale_Y = this_size.y / 2f;
				float that_scale_Y = that_size.y / 2f;
				float distance_Y = this_scale_Y + that_scale_Y;
				float this_scale_X = this_size.x / 2f;
				float that_scale_X = that_size.x / 2f;
				float distance_X = this_scale_X + that_scale_X;


				if (dir == PE_Dir.down) {
					// Just resolve to be on top
				    a1 = pos1;
					//a1 = this_center;
					a1.y -= this_scale_Y;
					b = that.pos1;
					//b = that_center;
					b.y += that_scale_Y;
					if (b.y > a1.y) {
						posFinal.y += Mathf.Abs( a1.y - b.y );
					}
					// Handle vel
					vel.y = 0;

					if (ground == null) ground = that;
					break; // Exit this switch statement: switch (that.coll)
				}

				if (dir == PE_Dir.up) {
					// Just resolve to be below
					a1 = pos1;
					a1.y += this_scale_Y;
					b = that.pos1;
					b.y -= that_scale_Y;
					if (b.y < a1.y) {
						posFinal.y -= Mathf.Abs( a1.y - b.y );
					}
					// Handle vel
					vel.y = 0;

					break; // Exit this switch statement: switch (that.coll)
				}

				if (dir == PE_Dir.upRight) { // Bottom, Left is the comparison corner
					a1 = pos1;
					a1.x += this_scale_X;
					a1.y += this_scale_Y;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that_scale_X;
					b.y -= that_scale_Y;
				}

				if (dir == PE_Dir.upLeft) { // Bottom, Right is the comparison corner
					a1 = pos1;
					a1.x -= this_scale_X;
					a1.y += this_scale_Y;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that_scale_X;
					b.y -= that_scale_Y;
				}

				if (dir == PE_Dir.downLeft) { // Top, Right is the comparison corner
 					a1 = pos1;
					a1.x -= this_scale_X;
					a1.y -= this_scale_Y;
					a0 = a1 - delta;
					b = that.pos1;
					b.x += that_scale_X;
					b.y += that_scale_Y;
				}

				if (dir == PE_Dir.downRight) { // Top, Left is the comparison corner
					a1 = pos1;
					a1.x += this_scale_X;
					a1.y -= this_scale_Y;
					a0 = a1 - delta;
					b = that.pos1;
					b.x -= that_scale_X;
					b.y += that_scale_Y;
				}

				// In the x dimension, find how far along the line segment between a0 and a1 we need to go to encounter b
				float u = (b.x - a0.x) / (a1.x - a0.x);

				// Determine this point using linear interpolation (see the appendix of the book)
				Vector2 pU = (1-u)*a0 + u*a1;

				// Use pU.y vs. b.y to tell which side of PE_Obj "that" PE_Obj "this" should be on
				switch (dir) {
				case PE_Dir.upRight:
					if (pU.y > b.y) { // hit the left side
						posFinal.x -= Mathf.Abs(a1.x - b.x);

						// Handle vel
						vel.x = 0;

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


}
