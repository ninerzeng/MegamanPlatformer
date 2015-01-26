using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fireball : MonoBehaviour {
	public BezierPath bezier;
	public GameObject player;
	public float bulletDelta;
	private List<Vector3> segmentPoints;
	public float timeOfLastShot;
	private int i = 0;
	public float maxXdistance = 6;
	public float maxYheight = 3;
	// Use this for initialization
	void Start () {
		bezier = GetComponent<BezierPath> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		GetPath ();

	}	

	void FixedUpdate () {
		float currentTime = Time.time;

		if (i == segmentPoints.Count) {
			Destroy (this.gameObject);
			return;
		}	
		if( currentTime - timeOfLastShot > bulletDelta)
		{
			timeOfLastShot = currentTime;
			this.transform.position = segmentPoints[i];
			i+=1;
		}
//		foreach(Vector3 v in segmentPoints)
//		{
//
//			StartCoroutine(delayDeltaTime (v));
//			Debug.DrawLine(v_prev, v, Color.red);
//			v_prev = v;
//		}
	}
	void GetPath()
	{
		
		int scalar;
		// Bezier path written by default such that player is to the right of the enemy
		// If the player is the left, then we multiply x coordinates by -1.
		if (player.transform.position.x < this.transform.position.x) {
			scalar = 1;		
		} else {
			scalar = -1;	
		}
		
		List<Vector3> controlPoints = new List<Vector3>();
		Vector3 p0,p1,p2,p3;
		p0 = p1 = p2 = p3 = transform.position;
		
		//			p0.x -=5; 
		p0.y -=0;
		
		//			p1.x -=5;
		p1.y += maxYheight;

		float x = 0;
		if (Mathf.Abs(player.transform.position.x - this.transform.position.x)< maxXdistance)
		    x = Mathf.Abs (this.transform.position.x - player.transform.position.x);
		else
		    x = maxXdistance;
		print (x);
		p2.x -=.75f*x * scalar;
		p2.y +=5;
		
		p3.x -= x*scalar;
		p3.y -=0f;
		controlPoints.Add(p0);
		controlPoints.Add(p1);
		controlPoints.Add(p2);
		
		controlPoints.Add(p3);
		
		bezier.SetControlPoints(controlPoints);
		
		segmentPoints = bezier.GetDrawingPoints2();
		bezier.Interpolate (segmentPoints, .5f);
		segmentPoints = bezier.GetControlPoints ();
	}
}
