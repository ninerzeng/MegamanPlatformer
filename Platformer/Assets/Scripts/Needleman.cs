using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Needleman : MonoBehaviour {
	// GameObject player;
    public float speed = 2;
    public bool floatingMode = false;
    public bool jumperMode = false;
    // public float wayPoints[] = [2,3,-2,-3];
    // int[] numbers = new int[5] {1, 2, 3, 4, 5};
 
    public float[] wayPoints = new float [4]{2, 3, -2, -3};
    private int indexOfWaypoint = 0;
    private float timeAtWaypoint = 0;
    private Vector3 nextWaypoint;
    public float delayBetweenPoints;

	private BezierPath bezier;
	public GameObject player;
	public float bulletDelta;
	private List<Vector3> segmentPoints;
	public float timeOfLastShot;
	private int i = 0;
	public float maxYheight = 3;
	private int i_jump = 0;

	public Animator animator; //option for tracking range from player and playing an animation
	// Use this for initialization
    
    void Awake()
    {
		// player = GameObject.FindGameObjectWithTag ("Player");
		nextWaypoint = transform.position;
		bezier = GetComponent<BezierPath> ();
		nextWaypoint.y += wayPoints[0];
		player = GameObject.FindGameObjectWithTag ("Player");
		GetPath();
		print ("first waypoint is " +  nextWaypoint);

    }
    void Update() {

    	// Vector3 target = player.transform.position;
    	// bool firststep = false;
    	// First we make an incremental jump upward
		// target.y += 1.2f;
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, step);	
       	if( Time.time - timeAtWaypoint > 2f && this.transform.position == nextWaypoint && indexOfWaypoint != 4)
       	{
       		timeAtWaypoint = Time.time;
       		indexOfWaypoint ++;
       		if (indexOfWaypoint != 4)
	       		nextWaypoint.y += wayPoints[indexOfWaypoint];
       		// print ("next waypoint is " + nextWaypoint );
       		print ("waypoint index" + indexOfWaypoint);
       	}
       	else if (indexOfWaypoint == 4)
       	{
			float currentTime = Time.time;

			if (i == segmentPoints.Count) {
				nextWaypoint = transform.position;
				nextWaypoint.y += wayPoints[0];
				if(i_jump >2)
				{
					indexOfWaypoint = 0;
					i_jump = 0;
				}
				print ("finished curve ");
				i=0; //reset index counter for segment path
				i_jump ++;
				GetPath();
				return;
			}	
			if( currentTime - timeOfLastShot > bulletDelta)
			{
				timeOfLastShot = currentTime;
				this.transform.position = segmentPoints[i];
				i+=1;
			}
       	}
       	//after up down motion, we code in the curves for jump


    }
	void JumpMove()
	{
	 	
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
		// x = Mathf.Abs (this.transform.position.x - player.transform.position.x);
		x = 3; // length of jump
		p2.x -=.75f * x * scalar;
		p2.y +=5;
		
		p3.x -= x*scalar;
		p3.y -=0f;
		controlPoints.Clear();
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