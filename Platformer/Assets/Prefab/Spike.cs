﻿using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {
	private PE_Obj peo;
	public float damage = 5;
	private float start_time = 0;
	private float bullet_used = 0;
	public float range = .5f;
	// Update is called once per frame
	void Start () {
		start_time = Time.time;
		peo = GetComponent<PE_Obj>();
	}
	void Update () {
		if ((Time.time - start_time) > range) {
			//print ("flying too long");
			//delete bullet
			int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj> () as PE_Obj);
			if (ObjIndex != -1) {
				//print ("DESTROY");
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
			print ("DESTROY SPIKE");
			PhysEngine.objs.RemoveAt (ObjIndex);	
			Destroy (this.gameObject);
		}
		
		//bullet_used = 1;
	}
	
}
