using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	public GameObject megaman;
	public GameObject left_bullet_prefab;
	public GameObject right_bullet_prefab;
	public float fire_rate = 0;
	public float damage = 10;
	//what you don't want the weapon to hit. Player, background, etc
	public LayerMask not_to_hit;
	private bool slide = false;
	float time_to_fire;
	//this is a child under the weapon, array where you fire bullets from
	Transform firepoint; 
	private PE_Controller MyScript;
	//true is right, false is left
	private bool last_dir = true;
	private PE_Obj peo;

	void Start(){
		MyScript = GetComponent<PE_Controller> ();
		peo = megaman.GetComponent<PE_Obj> ();
//		slide = MyScript.sliding;
	}

	void Shoot(){
		//Vector2 fire_point_position = new Vector2 (firepoint.position.x, firepoint.position.y);
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A) || last_dir == false) {
			print ("bullet fired left");
			GameObject bullet = Instantiate(left_bullet_prefab) as GameObject;
			PE_Obj peo_left = bullet.GetComponent<PE_Obj>();
			bullet.transform.position = this.transform.position;
			peo_left.vel.x = -20f;
			//peo_left.vel.y = peo.vel.y;
			//peo_left.vel.y = 0f;
		} 
		else {
			print ("bullet shot right");
			GameObject bullet = Instantiate(right_bullet_prefab) as GameObject;
			PE_Obj peo_right = bullet.GetComponent<PE_Obj>();
			bullet.transform.position = this.transform.position;
			peo_right.vel.x = 20f;
			//peo_right.vel.y = peo.vel.y;
			//peo_right.vel.y = 0f;
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
		if ((Input.GetKeyDown (KeyCode.Z) || Input.GetKeyDown (KeyCode.Comma)) && MyScript.sliding==false) {
			Shoot();
		}
		if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A)) {
			last_dir = false;
				}
		else if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D)) {
			last_dir = true;
				}
	}
}
