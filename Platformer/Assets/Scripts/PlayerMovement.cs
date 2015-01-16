using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float speed = 10f;
	private Animator m_Animator;

	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hSpeed = Input.GetAxis ("Horizontal");
		m_Animator.SetFloat ("Speed", Mathf.Abs (hSpeed));

		if (hSpeed > 0) {
			transform.localScale = new Vector3 (.75f, .75f, .75f);
		} 
		else if (hSpeed < 0) {
			transform.localScale = new Vector3(-.75f,.75f,.75f);		
		}
		this.rigidbody2D.velocity = new Vector2 (speed* hSpeed, this.rigidbody2D.velocity.y);
	}
}
