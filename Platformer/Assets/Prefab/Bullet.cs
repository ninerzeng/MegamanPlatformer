using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	private float start_time = 0;
	// Update is called once per frame
	void Start () {
		start_time = Time.time;
		}
	void Update () {
		if ((Time.time - start_time) > 2f) {
			print ("flying too long");
			//delete bullet
				}
	}

	void OnTriggerEnter(Collider other) {
		print ("i hit something");
			if (other.gameObject.tag == "Enemy") {
				//subtract health from it
				}
			//delete bullet regardless of what is hit
		}
}
