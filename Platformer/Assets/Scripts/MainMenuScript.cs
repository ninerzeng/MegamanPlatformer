using UnityEngine;
using System.Collections;

public class MainMenuScript: MonoBehaviour {
	private bool main = true;
	public GameObject level_select;
	void Awake()
	{
		level_select = GameObject.FindGameObjectWithTag ("Level Selector");
	}
	void Update()
	{

		if(Input.GetAxis("Submit") > 0 && main == true)
		{
			main = false;
			Vector3 curLoc = this.transform.position;
			curLoc.x = 17;
			this.transform.position = curLoc;
			
		}
		Vector3 selector = level_select.transform.position;
		if( Input.GetKeyDown (KeyCode.RightArrow) && main == false && selector.x < 20.18)
		{
			// move fram right
			selector.x += 3.33f;
		}
		if( Input.GetKeyDown (KeyCode.LeftArrow) && main == false && selector.x  > 13.53)
		{
			// move frame left
			selector.x -= 3.33f;
		}
		if( Input.GetKeyDown (KeyCode.UpArrow) && main == false && selector.y < 2.3)
		{
			// move fram right
			selector.y += 2.58f;
		}
		if( Input.GetKeyDown (KeyCode.DownArrow) && main == false && selector.y  > -2.2)
		{
			// move frame left
			selector.y -= 2.58f;
		}
		// 1 - needleman
		// 2 - custom
		level_select.transform.position = selector;
		if (selector.x > 20 && selector.y > 2.9 && Input.GetKeyUp(KeyCode.Return))
			// Application.loadlevel(1);
			print("loading level 1");
	}

}
