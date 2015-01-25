using UnityEngine;
using System.Collections;

public class Spike_bro : MonoBehaviour {
	private PE_Obj peo;
	public GameObject megaman;
	public float health = 30f;
	public GameObject spike_left;
	public GameObject spike_left_up;
	public GameObject spike_up;
	public GameObject spike_right_up;
	public GameObject spike_right;
	private int hit = 0;
	private float hit_time = -1f;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	void Update () {
		//start shooting
		if (Mathf.Abs(this.transform.position.x - megaman.transform.position.x) < 5) {
			//fire 5 spikes, at -90, -45, 0, 45, 90 respective to pos Y axis

		}
		//if you hit it, then roll towards the player
		if (hit == 1 && (Time.time - hit_time)>0.5) {
			//change animation
			//if player is to the left, move left
			if (megaman.transform.position.x < this.transform.position.x) {
				peo.vel.x = -5f;
			}
			//if player is to the right, move right
			else if(megaman.transform.position.x > this.transform.position.x) {
				peo.vel.x = 5f;
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Bullet") {
			hit = 1;
			if(hit_time < 0){
				hit_time = Time.time;
			}
			health = health - 5f;
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
