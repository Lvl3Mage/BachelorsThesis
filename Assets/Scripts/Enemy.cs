using System;
using Lvl3Mage.InterpolationToolkit;
using Lvl3Mage.MathToolkit;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	Transform player;
	Rigidbody rb;
	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	[SerializeField] Transform target;
	[SerializeField] float maxMovementSpeed = 4;
	[SerializeField] float slowdownRange = 2;
	void Update()
	{
		Vector3 targetDelta = target.position - transform.position;

		Vector3 movementDelta = new Vector3(targetDelta.x, 0, targetDelta.z);
		Vector3 movementDir = movementDelta.normalized;

		float targetVelocity = Mathf.Clamp( RangeTools.TransformRange(movementDelta.magnitude, new Vector2(0, slowdownRange),
			new Vector2(0, maxMovementSpeed)), 0,maxMovementSpeed);
		float alignedVelocity = Vector3.Dot(transform.forward, movementDir * targetVelocity);
		rb.linearVelocity = Decay.To(rb.linearVelocity, transform.forward * alignedVelocity, 10, Time.deltaTime);

		float angle = Vector2.SignedAngle(transform.forward, movementDir);
	}
}
