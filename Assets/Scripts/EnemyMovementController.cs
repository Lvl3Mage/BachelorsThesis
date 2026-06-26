using System;
using Lvl3Mage.InterpolationToolkit;
using Lvl3Mage.MathToolkit;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
	[SerializeField] Transform player;
	[SerializeField] Transform[] targets;
	[SerializeField] Transform barrelBone, leftBone, rightBone;
	[SerializeField] float maxRotationSpeed = 2;
	[SerializeField] float rotationSlowdownRange = 30f;
	int curIndex = 0;
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	[SerializeField] float maxMovementSpeed = 4;
	[SerializeField] float slowdownRange = 2;
	[SerializeField] float barrelRotationSpeed = 10, thrusterRotationAmplitude = 25;

	void Update()
	{
		Vector3 targetPos = targets[curIndex].position;
		if ((targetPos - transform.position).sqrMagnitude < 0.1f){
			curIndex++;
			if (curIndex >= targets.Length){
				curIndex = 0;
			}

			targetPos = targets[curIndex].position;
		}

		AimAt(targetPos);


		ControlBones();
	}

	public void PatrolTo(Vector3 target)
	{
		Vector3 targetDelta = target - transform.position;

		Vector3 movementDelta = new Vector3(targetDelta.x, 0, targetDelta.z);
		Vector3 movementDir = movementDelta.normalized;

		float targetVelocity = Mathf.Clamp(RangeTools.TransformRange(movementDelta.magnitude,
			new Vector2(0, slowdownRange),
			new Vector2(0, maxMovementSpeed)), 0, maxMovementSpeed);
		float alignedVelocity = Vector3.Dot(transform.forward, movementDir * targetVelocity);
		rb.linearVelocity = Decay.To(rb.linearVelocity, transform.forward * alignedVelocity, 3, Time.deltaTime);

		float angle = Vector2.SignedAngle(new Vector2(movementDir.x, movementDir.z),
			new Vector2(transform.forward.x, transform.forward.z));
		float angleSign = Mathf.Sign(angle);
		float targetAngularVelocity =
			Mathf.Clamp(RangeTools.TransformRange(Mathf.Abs(angle), 0f, rotationSlowdownRange, 0f, maxRotationSpeed),
				0f, maxRotationSpeed) * angleSign;
		// Debug.Log(angle);
		rb.angularVelocity = Decay.To(rb.angularVelocity, Vector3.up * targetAngularVelocity, 5, Time.deltaTime);
	}

	public void AimAt(Vector3 target)
	{
		Vector3 targetDelta = target - transform.position;
		Vector3 movementDelta = new Vector3(targetDelta.x, 0, targetDelta.z);
		Vector3 movementDir = movementDelta.normalized;

		float angle = Vector2.SignedAngle(new Vector2(movementDir.x, movementDir.z),
			new Vector2(transform.forward.x, transform.forward.z));
		float angleSign = Mathf.Sign(angle);
		float targetAngularVelocity =
			Mathf.Clamp(RangeTools.TransformRange(Mathf.Abs(angle), 0f, rotationSlowdownRange, 0f, maxRotationSpeed),
				0f, maxRotationSpeed) * angleSign;
		rb.angularVelocity = Decay.To(rb.angularVelocity, Vector3.up * targetAngularVelocity, 5, Time.deltaTime);

		Vector3 barrelTargetDelta = target - barrelBone.position;
		Vector3 horizontalBarrelDelta = new Vector3(barrelTargetDelta.x, 0, barrelTargetDelta.z);
		targetBarrelRotation = Mathf.Atan2(Vector3.Dot(transform.up, barrelTargetDelta),Vector3.Dot(horizontalBarrelDelta.normalized, barrelTargetDelta));
	}

	float targetBarrelRotation = 0;


	void ControlBones()
	{
		float leftVel = Vector3.Dot(rb.GetPointVelocity(leftBone.position), transform.forward);
		float rightVel = Vector3.Dot(rb.GetPointVelocity(rightBone.position), transform.forward);

		leftBone.localRotation = Quaternion.AngleAxis(-leftVel * thrusterRotationAmplitude, Vector3.right);
		rightBone.localRotation = Quaternion.AngleAxis(-rightVel * thrusterRotationAmplitude, Vector3.right);


		barrelBone.localRotation = Decay.To(barrelBone.localRotation,
			Quaternion.AngleAxis(targetBarrelRotation * Mathf.Rad2Deg, Vector3.right), barrelRotationSpeed, Time.deltaTime,
			Quaternion.Slerp);
	}
}