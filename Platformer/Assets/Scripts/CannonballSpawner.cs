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
	// Use this for initialization=

	//If time since last shot 
	void FixedUpdate () {
		if (Time.time - lastShotTime > timeBetwenShots) {
				lastShotTime = Time.time;
				Instantiate(cannonball, this.transform.position, Quaternion.identity);				
		}
			
	}
	void OnTriggerEnter(Collider other){
	}
}
