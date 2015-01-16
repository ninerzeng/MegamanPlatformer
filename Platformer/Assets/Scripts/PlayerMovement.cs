using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 10f;
	public float JumpSpeed = 10f ;
	public LayerMask GroundLayers;

	private Animator m_Animator;
	private Transform m_GroundCheck;
	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
		m_GroundCheck = transform.FindChild ("GroundCheck");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hSpeed = Input.GetAxis ("Horizontal");
		m_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));
		bool isGrounded = Physics2D.OverlapPoint (m_GroundCheck.position, GroundLayers);
		
		if (Input.GetButton ("Jump")) 
		{
			print (m_GroundCheck.position);
			print("Wow");
			if(isGrounded){
				print("Jumping!");
				rigidbody2D.AddForce (Vector2.up * JumpSpeed, ForceMode2D.Impulse);
				isGrounded = false;
			}			
		}
		m_Animator.SetBool("isGrounded", isGrounded);
		
		if (hSpeed > 0) {
			transform.localScale = new Vector3 (.75f, .75f, .75f);
		} 
		else if (hSpeed < 0) {
			transform.localScale = new Vector3(-.75f,.75f,.75f);		
		}
		this.rigidbody2D.velocity = new Vector2 (speed* hSpeed, this.rigidbody2D.velocity.y);
	}
}
