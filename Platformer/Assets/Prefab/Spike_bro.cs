using UnityEngine;
using System.Collections;

public class Spike_bro : MonoBehaviour {
	private PE_Obj peo;
	public GameObject megaman;
	public float health = 30f;
	private bool shooting = false;
	private bool moving = false;
//	public GameObject spike_left;
//	public GameObject spike_left_up;
//	public GameObject spike_up;
//	public GameObject spike_right_up;
//	public GameObject spike_right;
	public GameObject spike;
	private int hit = 0;
	private float hit_time = -1f;
	private float move_time = -1f;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		megaman = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		//start shooting
		if (Mathf.Abs(this.transform.position.x - megaman.transform.position.x) < 5) {
			//fire 5 spikes, at -90, -45, 0, 45, 90 respective to pos Y axis
			if(shooting == false && moving == false){
				shooting = true;
				Shooting ();
			}
			shooting = true;
		}
		//if you hit it, then roll towards the player
		if (hit == 1 && (Time.time - hit_time)>1f) {
			if(moving == false){
				move_time = Time.time;
			}
			moving = true;
			CancelInvoke();
			//change animation
			shooting = false;
			//if player is to the left, move left
			if (megaman.transform.position.x < this.transform.position.x) {
				peo.vel.x = -5f;
			}
			//if player is to the right, move right
			else if(megaman.transform.position.x > this.transform.position.x) {
				peo.vel.x = 5f;
			}
		}
		//after a certain amount of time, start shooting again
		if ((Time.time - move_time) > 4f && moving == true) {
			peo.vel.x = 0f;
			moving = false;
			Shooting ();
			hit = 0;
		}
	}
	void Shooting(){
		if (shooting == true) {
			InvokeRepeating("Shoot", 0.1f, 1f);
		} 
	}

	void Shoot(){
		//fire 5 spikes, at -90, -45, 0, 45, 90 respective to pos Y axis
		GameObject spike_left = Instantiate(spike) as GameObject;
		spike_left.transform.position = this.transform.position;
		PE_Obj peo_left = spike_left.GetComponent<PE_Obj>();
		peo_left.vel.x = -5f;
		peo_left.vel.y = 0f;
		
		GameObject spike_left_up = Instantiate(spike) as GameObject;
		spike_left_up.transform.position = this.transform.position;
		PE_Obj peo_left_up = spike_left_up.GetComponent<PE_Obj>();
		peo_left_up.vel.x = -Mathf.Sqrt(10f);
		peo_left_up.vel.y = Mathf.Sqrt(10f);
		
		GameObject spike_up = Instantiate(spike) as GameObject;
		spike_up.transform.position = this.transform.position;
		Vector3 new_pos = spike_up.transform.position;
		PE_Obj peo_up = spike_up.GetComponent<PE_Obj>();
		peo_up.vel.x = 0f;
		peo_up.vel.y = 5f;
		
		GameObject spike_right_up = Instantiate(spike) as GameObject;
		spike_right_up.transform.position = this.transform.position;
		PE_Obj peo_up_right = spike_right_up.GetComponent<PE_Obj>();
		peo_up_right.vel.x = Mathf.Sqrt(10f);
		peo_up_right.vel.y = Mathf.Sqrt(10f);
		
		GameObject spike_right = Instantiate(spike) as GameObject;
		spike_right.transform.position = this.transform.position;
		PE_Obj peo_right = spike_right.GetComponent<PE_Obj>();
		peo_right.vel.x = 5f;
		peo_right.vel.y = 0f;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Bullet") {
			hit = 1;
			if(hit_time < 0){
				hit_time = Time.time;
			}
			if(moving == false){
			health = health - 5f;
			}
			if(health <= 0){
				int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
				if (ObjIndex != -1) {
					print ("DESTROY spike");
					PhysEngine.objs.RemoveAt (ObjIndex);	
					Destroy (this.gameObject);
					Vector3 spikeBroLoc = this.transform.position;
					spikeBroLoc.y -= .5f;;
				}
			}
		} 
		else if (other.gameObject.tag == "Player") {
			//keep up what youre doing buddy
		} 
		else {
			peo.vel.x = -peo.vel.x;
		}
	}

}
