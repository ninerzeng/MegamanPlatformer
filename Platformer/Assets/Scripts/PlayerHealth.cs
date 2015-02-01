using UnityEngine;
using System.Collections;

// Handles player health and GUI display for health
public class PlayerHealth : MonoBehaviour {
	public float barDisplay; //current progress
	public Vector2 pos = new Vector2(20,40);
	public Vector2 size = new Vector2(20,60);
	public Texture2D emptyTex;
	public Texture2D fullTex;
	public float playerHealth = 100;
	public bool invulnerable = false;
	public bool dead = false;
//	private PE_Obj m_PE_Obj;
	private Animator animator;
	void Start()
	{
//		m_PE_Obj = this.GetComponent<PE_Obj> ();
		animator = this.GetComponent<Animator> ();
	}
	void OnGUI() {
		//draw the background:
		GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
		GUI.Box(new Rect(0,0, size.x, size.y), emptyTex);
		
		//draw the filled-in part:
		GUI.BeginGroup(new Rect(0,0, size.x, size.y * barDisplay));
		GUI.Box(new Rect(0,0, size.x, size.y), fullTex);
		GUI.EndGroup();
		GUI.EndGroup();

//		barDisplay = playerHealth * .01f;
	}
	void OnTriggerEnter(Collider other)
	{
		print("you got hit");

		if(other.tag == "Enemy Bullet")
			playerHealth -= 5;

	}
	void Update() {
		barDisplay = playerHealth * .01f;

		}
}