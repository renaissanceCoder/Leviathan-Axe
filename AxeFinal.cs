using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeFinal : MonoBehaviour {

	// Public Variables
	// We need a reference for the axe gameobject of course
	public GameObject axe;
	// We need another reference for the axe's rb
	public Rigidbody axeRB;
	// We need a reference for the temporary holder as well
	public GameObject axeTempHolder;

	// Now we need to setup a few variables to handle the axe's movement
	public float axeFlightSpeed = 10f;
	public float axeThrowPower = 10f;
	public float axeRotationSpeed = 10f;

	// We need a reference for the axe collisions script
	public AxeCollisions axeCollisions;

	// Enum for axeState I am using an enum here but this functionality could also be achieved with several boolean variables
	// I just think that this is the cleaner way to go about controlling the axe's various states
	public enum AxeState {Static, Thrown, Travelling, Returning}
	public AxeState axeState;

	// Now we need a couple of private variables that we will use to handle the axe's returning movement and speed
	private float startTime;
	private float journeyLength;
	private Vector3 startPos;
    private Vector3 endPos;
	
	// In our Start function, we need to setup the Rigidbody, we are also going to set isKinematic to true which means that physics will not have an impact on the axe,
	// and we are going to set usegravity to false as well.
	void Start () 
	{
		axeRB = axe.GetComponent<Rigidbody>();
		axeRB.isKinematic = true;
		axeRB.useGravity = false;
	}

	// Now we need to change update to fixedupdate and start filling out our script.
	void FixedUpdate () 
	{
		// Ok the first thing that we want to setup is a check to see if we left click which will be used to throw the axe for now we are just 
		// going to change the state of the axe to thrown
		if (Input.GetMouseButtonDown(0)) {						
			axeState = AxeState.Thrown;			
		}

		// Now let's setup the right mouse click to recall the axe,
		if (Input.GetMouseButtonDown(1)) {
			// First of all we need to get some positions for our lerping function like the starting and ending positions
			startPos = axe.transform.position;
			endPos = axeTempHolder.transform.position;
			// We need to reset our startTime to Time.time
			startTime = Time.time;
			// Our journey length will be a distance check from the start position to the end position
        	journeyLength = Vector3.Distance(startPos, endPos);
        	// Finally we need to change up the axe's state.
			axeState = AxeState.Returning;			
		}

		// Ok, we want to check to see if our axe has been thrown and if so we are going to call a function, which we will setup in a minute
		if (axeState == AxeState.Thrown) {			
			ThrowAxeWithPhysics();
		}

		// Next let's check to see if the axe is travelling or returning and if so, we want to make the axe spin.
		if (axeState == AxeState.Travelling || axeState == AxeState.Returning) {
			axe.transform.Rotate(6.0f*axeRotationSpeed*Time.deltaTime,0,0);
		}	

		// Ok and finally, if the axe is supposed to return then we need to call another function. 
		if (axeState == AxeState.Returning) {
			RecallAxe();
		}	
	}

	// Throw Axe with Physics called when the axe is actually thrown
	void ThrowAxeWithPhysics()
	{	
		// make sure that the axe does not have a parent, if we don't do this then the axe will still move around 
		// while the player looks around
		axe.transform.parent = null;
		// lets change up the state of the axe
		axeState = AxeState.Travelling;
		// WE need to change the values of iskinematic and usegravity
		axeRB.isKinematic = false;
		axeRB.useGravity = true;
		// finally let's throw the axe using physics.
		axeRB.AddForce(axe.transform.forward * axeThrowPower);
	}	

	// Used when we want the axe to actually be recalled
	void RecallAxe()
	{
		// We want to lerp the axe back to the players hand so we need the distance covered and the fracjourney
		float distCovered = (Time.time - startTime) * axeFlightSpeed;
        float fracJourney = distCovered / journeyLength;
	 	axe.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

	 	// If we have completed our journey, then we want to call our reset function
		if (fracJourney >= 1.0f)
		{
			RecalledAxe();
		}
	}

	// Occurs when the axe has been recalled.
	void RecalledAxe()
	{
		// We need to change the current state of our axe back to static
		axeState = AxeState.Static;
		// We want to remove the position constraints from the rigidbody
		axeCollisions.RemoveConstraints();
		// We want to complete a hard reset of the position and rotation of the axe back to the initial settings
		axe.transform.position = axeTempHolder.transform.position;
		axe.transform.rotation = axeTempHolder.transform.rotation;
		// Change iskinematic and usegravity
		axeRB.isKinematic = true;
		axeRB.useGravity = false;
		// change up the parent of the axe so that it is back under the main camera.
		axe.transform.parent = this.transform;
	}

	// Occurs when the axe has entered into a collision.
	public void CollisionOccured() 
	{
		axeState = AxeState.Static;
	}
}
