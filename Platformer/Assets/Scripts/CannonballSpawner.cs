	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CannonballSpawner : MonoBehaviour {

	public float health = 10;
	public GameObject player;
//	public GameObject HealthPackPrefab;
	public GameObject cannonball;
	public BezierPath bezier;
	private float lastShotTime = 0;
	public float timeBetwenShots=3;
	public Animator animator;
	//maxYdistance needs to also be set in FireBall cs object
	public float maxXdistance = 4; 
	// Use this for initialization=
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		
		animator = this.GetComponent<Animator> ();
	}
	//If time since last shot 
	void FixedUpdate () {
		if (Time.time - lastShotTime > timeBetwenShots) {
				lastShotTime = Time.time;
				if (Mathf.Abs (player.transform.position.x - this.transform.position.x) >= maxXdistance)
				{
					animator.SetBool ("out_of_range", true);
					print("out of range");
				
				}
				else{
					Instantiate(cannonball, this.transform.position, Quaternion.identity);	
					animator.SetTrigger("shooting");
					return;
			}
		}
			
	}
	void OnTriggerEnter(Collider other){
	}
}
