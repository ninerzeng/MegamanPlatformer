using UnityEngine;
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
		if (other.tag == "Player") 
		{
			print ("Bang!");
			PhysEngine.DestroyObject(this.gameObject);
		}
	}
	void OnTriggerStay (Collider other)
	{
		OnTriggerEnter (other);
	}
}
