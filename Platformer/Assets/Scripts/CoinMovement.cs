using UnityEngine;
using System.Collections;

public class CoinMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other) 
	{
		print ("Bang!");
		if (other.tag == "Player") 
		{
			Destroy(this.gameObject);
		}
	}
	void OnTriggerStay2D (Collider2D other)
	{
		OnTriggerEnter2D (other);
	}
}
