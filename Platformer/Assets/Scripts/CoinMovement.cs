﻿using UnityEngine;
using System.Collections;

public class CoinMovement : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other) 
	{

		if (other.tag == "Player") {
			PlayerHealth hp = other.GetComponent<PlayerHealth>();
			hp.playerHealth +=30;
			int ObjIndex = PhysEngine.objs.IndexOf (this.GetComponent<PE_Obj>() as PE_Obj);
			if (ObjIndex != -1) {
				print ("DESTROY");
				PhysEngine.objs.RemoveAt(ObjIndex);	
				Destroy (this.gameObject);
	      }
		}
	}
}
