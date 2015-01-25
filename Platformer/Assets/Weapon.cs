﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public GameObject left_bullet_prefab;
	public GameObject right_bullet_prefab;
	public float fire_rate = 0;
	public float damage = 10;
	//what you don't want the weapon to hit. Player, background, etc
	public LayerMask not_to_hit;
	
	float time_to_fire;
	//this is a child under the weapon, array where you fire bullets from
	Transform firepoint; 
	
	void Shoot(){
		//Vector2 fire_point_position = new Vector2 (firepoint.position.x, firepoint.position.y);
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			print ("bullet fired left");
			GameObject bullet = Instantiate(left_bullet_prefab) as GameObject;
			bullet.transform.position = this.transform.position;
		} 
		else {
			print ("bullet shot right");
			GameObject bullet = Instantiate (right_bullet_prefab) as GameObject;
			bullet.transform.position = this.transform.position;
				}
		//		Vector2 bullet_speed;
		//		bullet_speed.x = 5;
		//		bullet_speed.y = 0;
		//		bullet.rigidbody2D.velocity = bullet_speed;
	}
	
	// Use this for initialization
	void Awake () {
		//		firepoint = transform.FindChild;
		//		if (firepoint == NULL)
		//			print ("didn't find a weapon");
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) {
			Shoot();
		}
	}
}