using System;
using Lvl3Mage.InterpolationToolkit;
using Lvl3Mage.MathToolkit;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
	[SerializeField] Transform barrelBone, leftBone, rightBone;
	[SerializeField] float maxRotationSpeed = 2;
	[SerializeField] float rotationSlowdownRange = 30f;
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	[SerializeField] float maxMovementSpeed = 4;
	[SerializeField] float slowdownRange = 2;
	[SerializeField] float barrelRotationSpeed = 10, thrusterRotationAmplitude = 25;

	void Update()
	{


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
		rb.angularVelocity = Decay.To(rb.angularVelocity, Vector3.up * targetAngularVelocity, 5, Time.deltaTime);
	}

	void FixedUpdate()
	{
		rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);

	}

	void LateUpdate()
	{
		transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);//kinda ugly but works to prevent unwanted rotations in xz
	}

	public void AimAt(Vector3 target)
	{
		Vector3 targetDelta = target - transform.position;
		Vector3 movementDelta = new(targetDelta.x, 0, targetDelta.z);
		Vector3 movementDir = movementDelta.normalized;

		float angle = Vector2.SignedAngle(new Vector2(movementDir.x, movementDir.z),
			new Vector2(transform.forward.x, transform.forward.z));
		float angleSign = Mathf.Sign(angle);
		float targetAngularVelocity =
			Mathf.Clamp(RangeTools.TransformRange(Mathf.Abs(angle), 0f, rotationSlowdownRange, 0f, maxRotationSpeed),
				0f, maxRotationSpeed) * angleSign;
		rb.angularVelocity = Decay.To(rb.angularVelocity, Vector3.up * targetAngularVelocity, 5, Time.deltaTime);
		rb.linearVelocity =Decay.To(rb.linearVelocity,Vector3.zero, 3, Time.deltaTime);
		rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0);
		Vector3 barrelTargetDelta = target - barrelBone.position;
		Vector3 horizontalBarrelDelta = new(barrelTargetDelta.x, 0, barrelTargetDelta.z);
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