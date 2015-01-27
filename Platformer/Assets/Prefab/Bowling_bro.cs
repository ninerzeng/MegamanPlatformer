using UnityEngine;
using System.Collections;

public class Bowling_bro : MonoBehaviour {
	public GameObject megaman;
	public float health = 15f;
	public GameObject bowling_ball;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//shoot left
		if (megaman.transform.position.x < this.transform.position.x) {

		}
		//shoot right
		else if (megaman.transform.position.x > this.transform.position.x) {

		}
	}

	void shoot_right(){
		GameObject ball_right = Instantiate(bowling_ball) as GameObject;
		ball_right.transform.position = this.transform.position;
		PE_Obj peo_right = ball_right.GetComponent<PE_Obj>();
		peo_right.vel.x = 10f;
		peo_right.vel.y = 0f;
	}

	void shoot_left(){
		GameObject ball_left = Instantiate(bowling_ball) as GameObject;
		ball_left.transform.position = this.transform.position;
		PE_Obj peo_left = ball_left.GetComponent<PE_Obj>();
		peo_left.vel.x = -10f;
		peo_left.vel.y = 0f;
	}
}
