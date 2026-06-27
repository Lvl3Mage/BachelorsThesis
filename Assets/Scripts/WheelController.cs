using System;
using Lvl3Mage.InterpolationToolkit;
using UnityEngine;

public class WheelController : MonoBehaviour
{
	[SerializeField] Collider collider;
	[SerializeField] float gripRadius;
	[SerializeField] float gripOffset;
	[SerializeField] float friction = 2;

	[SerializeField]float angularVelocity = 0;
	// [SerializeField] Transform target;
	// Vector3 prevpos;

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(Vector3.right, angularVelocity * Time.deltaTime);
		angularVelocity = Decay.To(angularVelocity, 0, friction, Time.deltaTime);
		//
		// SetVelocityAt(target.position, (target.position - prevpos )/ Time.deltaTime);
		// prevpos = target.position;
	}

	Vector3 GetWsDirectionAt(Vector3 wsPosition)
	{
		Vector3 localPos = transform.InverseTransformPoint(wsPosition);
		localPos.x = 0;
		Vector3 dir = -Vector3.Cross(localPos, Vector3.right).normalized;
		return transform.TransformDirection(dir);
	}

	public Vector3 GetVelocityAt(Vector3 wsPosition)
	{
		Vector3 dir = GetWsDirectionAt(wsPosition);
		Vector3 localPos = transform.InverseTransformPoint(wsPosition);
		localPos.x = 0;
		float radius = localPos.magnitude;
		Debug.DrawLine(wsPosition, wsPosition+dir * (radius * Mathf.Deg2Rad * angularVelocity));
		return dir * (radius * Mathf.Deg2Rad * angularVelocity);
	}

	public void SetVelocityAt(Vector3 wsPosition, Vector3 wsTargetVelocity)
	{
		float linearVel = Vector3.Dot(GetWsDirectionAt(wsPosition), wsTargetVelocity);

		Vector3 localPos = transform.InverseTransformPoint(wsPosition);
		localPos.x = 0;
		float radius = localPos.magnitude;

		angularVelocity = linearVel / (radius * Mathf.Deg2Rad);
	}

	public HandPose GetGripPoseAt(Vector3 wsPosition)
	{
		Vector3 localPos = transform.InverseTransformPoint(wsPosition);

		localPos.x = 0;
		Vector3 gripDir = localPos.normalized * (gripRadius+gripOffset);
		return new HandPose{
			WsPosition = transform.TransformPoint(gripDir),
			WsRotation = Quaternion.LookRotation(-transform.TransformDirection(gripDir), GetWsDirectionAt(wsPosition))
		};
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawSphere(transform.position, gripRadius);
		Gizmos.color = new Color(0, 0, 1, 0.5f);
		Gizmos.DrawSphere(transform.position, gripRadius+gripOffset);
	}
}