using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {
	public int maxHealth = 15;
	private int currentHealth;

	void Awake()
	{
		currentHealth = maxHealth;
	}
	void Update()
	{
		if(currentHealth <= 0 )
			Destroy(this.gameObject);
	}
	void OnTriggerEnter(Collider other){
		if (other.tag == "Bullet"){
			Bullet bullet = other.GetComponent<Bullet>();
			print ("hit");	
			currentHealth -= (int) bullet.damage;
		}
	}
}

