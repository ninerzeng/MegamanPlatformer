using UnityEngine;
using System.Collections;

public class Bowling_bro : MonoBehaviour {
	public GameObject megaman;
	public float health = 15f;
	public GameObject bowling_ball;
	private bool done_shooting = true;
	public Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		megaman = GameObject.FindGameObjectWithTag ("Player");

	}
	
	// Update is called once per frame
	void Update () {
		//shoot left
		if (done_shooting == true) {
			if (megaman.transform.position.x < this.transform.position.x) {
				Invoke ("shoot_left", 3f);
			}
		//shoot right
			else if (megaman.transform.position.x > this.transform.position.x) {
				Invoke ("shoot_right", 3f);
			}
			done_shooting = false;
			animator.SetTrigger("done_shooting");
		}
	}
	private Vector3 temp_loc;
	void shoot_right(){
		// face right
		transform.localScale = new Vector3 (-1f, 1f, 1f);	
		animator.SetTrigger("shooting");

		GameObject ball_right = Instantiate(bowling_ball) as GameObject;
		//ball_right.transform.position = this.transform.position;
		temp_loc = this.transform.position;
		temp_loc.y += 0.2f;
		ball_right.transform.position = temp_loc;
		PE_Obj peo_right = ball_right.GetComponent<PE_Obj>();
		peo_right.vel.x = 10f;
		peo_right.vel.y = 0f;
		done_shooting = true;
	}

	void shoot_left(){
		// face left
		transform.localScale = new Vector3 (1f, 1f, 1f);
		animator.SetTrigger("shooting");

		GameObject ball_left = Instantiate(bowling_ball) as GameObject;
		//ball_left.transform.position = this.transform.position;
		temp_loc = this.transform.position;
		temp_loc.y += 0.2f;
		ball_left.transform.position = temp_loc;
		PE_Obj peo_left = ball_left.GetComponent<PE_Obj>();
		peo_left.vel.x = -10f;
		peo_left.vel.y = 0f;
		done_shooting = true;
	}

	void OnTriggerEnter(Collider other){
		//can only take damage if after shooting. need animation for this!
		if (other.gameObject.tag == "Bullet") {
			//take dmg
			//health = health - other.gameObject.GetComponent<Bullet>().Damage;
			health = health - 5;
			if(health <= 0){
				int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
				if (ObjIndex != -1) {
					print ("DESTROY BOWLING BRO");
					PhysEngine.objs.RemoveAt (ObjIndex);	
					Destroy (this.gameObject);
				}
			}
		} 
		else if (other.gameObject.tag == "Player") {
			//keep up what youre doing buddy
		} 
	}
}
