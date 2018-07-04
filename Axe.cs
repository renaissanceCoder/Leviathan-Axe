using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour {

	public GameObject axe;
	public Rigidbody axeRb;
	public GameObject axeTempHolder;

	public float axeFlightSpeed = 10f;
	public float axeThrowPower = 10f;
	public float axeRotationSpeed = 10f;

	public NewAxeCollisions axeCollsions;

	public enum AxeState {Static, Thrown, Travelling, Returning}
	public AxeState axeState;

	private float startTime;
	private float journeyLength;
	private Vector3 startPos;
	private Vector3 endPos;

	// Use this for initialization
	void Start () {
		axeRb = axe.GetComponent<Rigidbody>();
		axeRb.isKinematic = true;
		axeRb.useGravity = false;
	}
		
	void FixedUpdate () {
		if (Input.GetMouseButtonDown(0)) {
			axeState = AxeState.Thrown;
		}

		if (Input.GetMouseButtonDown(1)) {
			startPos = axe.transform.position;
			endPos = axeTempHolder.transform.position;
			startTime = Time.time;
			journeyLength = Vector3.Distance(startPos, endPos);
			axeState = AxeState.Returning;
		}

		if (axeState == AxeState.Thrown) {
			ThrownAxeWithPhysics();
		}

		if (axeState == AxeState.Travelling || axeState == AxeState.Returning) {
			axe.transform.Rotate(6.0f * axeRotationSpeed * Time.deltaTime, 0, 0);
		}

		if (axeState == AxeState.Returning) {
			RecallAxe();
		}
	}

	void ThrownAxeWithPhysics()
	{
		axe.transform.parent = null;
		axeState = AxeState.Travelling;
		axeRb.isKinematic = false;
		axeRb.useGravity = true;
		axeRb.AddForce(axe.transform.forward * axeThrowPower);
	}

	void RecallAxe()
	{
		float distCovered = (Time.time - startTime) * axeFlightSpeed;
		float fracJourney = distCovered / journeyLength;
		axe.transform.position = Vector3.Lerp(startPos, endPos, fracJourney);

		if (fracJourney >= 1.0f) {
			RecalledAxe();
		}
	}

	void RecalledAxe()
	{
		axeState = AxeState.Static;
		axeCollsions.RemoveConstraints();
		axe.transform.position = axeTempHolder.transform.position;
		axe.transform.rotation = axeTempHolder.transform.rotation;
		axeRb.isKinematic = true;
		axeRb.useGravity = false;
		axe.transform.parent = this.transform;
	}

	public void CollisionOccured()
	{
		axeState = AxeState.Static;
	}

























}
