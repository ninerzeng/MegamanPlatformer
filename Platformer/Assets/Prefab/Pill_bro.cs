using UnityEngine;
using System.Collections;

public class Pill_bro : MonoBehaviour {
	private PE_Obj peo;
	public float health = 10;
	public GameObject player;
	public GameObject HealthPackPrefab;
	public GameObject spike;
	private bool shot = false;
	private bool moving = false;
	private bool done_shooting = false;
	private float time_shot = -1f;
	public Animator animator;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
		player = GameObject.FindGameObjectWithTag ("Player");
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//if player is to the left, move left
		if(done_shooting == true && (Time.time - time_shot > 0.5f)){
			if (player.transform.position.x < this.transform.position.x) {
				peo.vel.x = -2f;
			}
			//if player is to the right, move right
			else if(player.transform.position.x > this.transform.position.x){
				peo.vel.x = 2f;
			}
		}
		//if you're close enough, shoot once at you and then run towards you
		if (Mathf.Abs(this.transform.position.x - player.transform.position.x) < 7f) {
			//fire 5 spikes, at -90, -45, 0, 45, 90 respective to pos Y axis
			if(shot==false){
				shot = true;
				animator.SetTrigger("Wakeup");
				print ("woke up pillbro");
				Invoke("Shoot", 0.5f);
			}
			shot = true;
		}
	}
	
	void Shoot(){


		GameObject spike_left = Instantiate(spike) as GameObject;
		spike_left.transform.position = this.transform.position;
		PE_Obj peo_left = spike_left.GetComponent<PE_Obj>();
		peo_left.vel.x = -10f;
		peo_left.vel.y = 0f;
		
		GameObject spike_left_up = Instantiate(spike) as GameObject;
		spike_left_up.transform.position = this.transform.position;
		PE_Obj peo_left_up = spike_left_up.GetComponent<PE_Obj>();
		peo_left_up.vel.x = -Mathf.Sqrt(20f);
		peo_left_up.vel.y = Mathf.Sqrt(20f);
		
		GameObject spike_left_down = Instantiate(spike) as GameObject;
		spike_left_down.transform.position = this.transform.position;
		Vector3 new_pos = spike_left_down.transform.position;
		PE_Obj peo_left_down = spike_left_down.GetComponent<PE_Obj>();
		peo_left_down.vel.x = -Mathf.Sqrt (20f);
		peo_left_down.vel.y = Mathf.Sqrt (20f);

		done_shooting = true;
		time_shot = Time.time;
	}

	void OnTriggerEnter(Collider other){
		//can only take damage if after shooting. need animation for this!
		if (other.gameObject.tag == "Bullet" && done_shooting==true) {
			//take dmg
			//health = health - other.gameObject.GetComponent<Bullet>().Damage;
			health = health - 5;
			if(health <= 0){
				int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
				if (ObjIndex != -1) {
					print ("DESTROY");
					PhysEngine.objs.RemoveAt (ObjIndex);	
					Destroy (this.gameObject);
					Vector3 pillBroLoc = this.transform.position;
					pillBroLoc.y -= .5f;
					Instantiate(this.HealthPackPrefab, pillBroLoc, Quaternion.identity);
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