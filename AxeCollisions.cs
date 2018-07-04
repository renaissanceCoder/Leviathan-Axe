using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeCollisions : MonoBehaviour {

	public Rigidbody rb;
	public AxeFinal axeFinal;

	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision collision)
    {
    	axeFinal.CollisionOccured();
    	rb.useGravity = false;
    	rb.isKinematic = true;      
    	AddConstraints();  
    }

    public void RemoveConstraints()
    {
    	rb.constraints = RigidbodyConstraints.None;
    }

    public void AddConstraints()
    {
    	rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;    	
    }
}
