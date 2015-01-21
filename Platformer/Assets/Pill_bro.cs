using UnityEngine;
using System.Collections;

public class Pill_bro : MonoBehaviour {
	private PE_Obj peo;
	public float health = 10;
	public GameObject player;
	public GameObject HealthPackPrefab;
	// Use this for initialization
	void Start () {
		peo = GetComponent<PE_Obj>();
	}
	
	// Update is called once per frame
	void Update () {
		//if player is to the left, move left
		if (player.transform.position.x < this.transform.position.x) {
			peo.vel.x = -2f;
			}
		//if player is to the right, move right
		else if(player.transform.position.x > this.transform.position.x) {
			peo.vel.x = 2f;
			}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Bullet") {
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
