using UnityEngine;
using System.Collections;

public class Bowling_ball : MonoBehaviour {
	private PE_Obj peo;
	public float damage = 10;
	private float start_time = 0;
	private float bullet_used = 0;
	// Update is called once per frame
	void Start () {
		start_time = Time.time;
		peo = GetComponent<PE_Obj>();
	}
	void Update () {
		if ((Time.time - start_time) > 3f) {
			//print ("flying too long");
			//delete bullet
			int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
			if (ObjIndex != -1) {
				print ("delete this ball");
				PhysEngine.objs.RemoveAt (ObjIndex);	
				Destroy (this.gameObject);
			}
		}
	}
	
	void OnTriggerEnter(Collider other) {
		print ("i hit something");
		//print (other.name);
		//delete bullet regardless of what is hit
		int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
		if (ObjIndex != -1) {
			print ("DESTROY BALL");
			PhysEngine.objs.RemoveAt (ObjIndex);	
			Destroy (this.gameObject);
		}
		
		//bullet_used = 1;
	}
	
}
