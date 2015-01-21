using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public float damage = 5;
	private float start_time = 0;
	// Update is called once per frame
	void Start () {
		start_time = Time.time;
		}
	void Update () {
		if ((Time.time - start_time) > 0.5f) {
						print ("flying too long");
						//delete bullet
						int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
						if (ObjIndex != -1) {
								print ("DESTROY");
								PhysEngine.objs.RemoveAt (ObjIndex);	
								Destroy (this.gameObject);
						}
				}
	}

	void OnTriggerEnter(Collider other) {
		print ("i hit something");
			if (other.gameObject.tag == "Enemy") {
				//subtract health from it
				
				}
		//delete bullet regardless of what is hit
		int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
		if (ObjIndex != -1) {
						print ("DESTROY");
						PhysEngine.objs.RemoveAt (ObjIndex);	
						Destroy (this.gameObject);
				}
		}
}
