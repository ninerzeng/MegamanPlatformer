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
	
	void Update() {
		//for this example, the bar display is linked to the current time,
		//however you would set this value based on your desired display
		//eg, the loading progress, the player's health, or whatever.
//		barDisplay = Time.time*0.05f;
		//        barDisplay = MyControlScript.staticHealth;
		barDisplay = playerHealth * .01f;
		if (playerHealth <= 0) {
			PE_Controller controller = this.GetComponent<PE_Controller>();	
			controller.enabled = false; 
//			GameManagerScript.GameOverMenu(2);


		}
	}
	void OnTriggerEnter(Collider other)
	{
//		print ("found enemy");
		if (other.tag == "Enemy") {
						playerHealth -= 10;
				}
			//			PE_Obj nowPE_Obj = this.GetComponent<PE_Obj>();
//			Vector3 stunVel = nowPE_Obj.vel;
//			Vector3 directionOfStun = this.transform.position - other.transform.position ; 
//			directionOfStun.Normalize();
//			print (directionOfStun);
//			//stun normally if player is on same level or above the 
//			if (directionOfStun.x >= 0)
//			{
//				stunVel.x = stunStrength;
//				stunVel.y=stunStrength;
//			}
//			else
//			{
//				stunVel.y = stunStrength;
//				stunVel.x = -1 *stunStrength;
//				
//			}
//			PE_Controller myController = this.GetComponent<PE_Controller>();
//			myController.enabled = false;
//			print (stunVel);
//			nowPE_Obj.vel = stunVel;
//			StartCoroutine(bounceDelay(myController));
//
//		}
	}
//	IEnumerator bounceDelay(PE_Controller myController) {
//			yield return new WaitForSeconds(stunDelay);
//			myController.enabled = true;
//			
//		}
}