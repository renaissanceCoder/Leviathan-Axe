using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewAxeCollisions : MonoBehaviour {

	public Rigidbody rb;
	public Axe axe;
	
	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision)
	{
		axe.CollisionOccured();
		rb.useGravity = false;
		rb.isKinematic = true;
		AddConstraints();
	}

	public void AddConstraints() 
	{
		rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
	}

	public void RemoveConstraints()
	{
		rb.constraints = RigidbodyConstraints.None;	
	}
	
}
