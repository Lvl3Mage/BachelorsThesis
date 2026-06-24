using System;
using Lvl3Mage.InterpolationToolkit;
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
	HandPose pose;
	[SerializeField] Transform relativeRef;
	public void SetPose(HandPose newPose)
	{
		pose = newPose;
	}

	// Vector3 lastRelativePosition;
	// Quaternion lastRelativeRotation;
	void LateUpdate()
	{
		Vector3 targetPos = relativeRef.InverseTransformPoint(pose.WsPosition);
		Quaternion targetRot = Quaternion.Inverse(relativeRef.rotation) * pose.WsRotation;
		Vector3 currentPos = relativeRef.InverseTransformPoint(transform.position);
		Quaternion currentRot = Quaternion.Inverse(relativeRef.rotation) * transform.rotation;

		currentPos = Decay.To(currentPos, targetPos, 15f, Time.deltaTime);
		currentRot = Decay.To(currentRot, targetRot, 15f, Time.deltaTime, Quaternion.Slerp);
		transform.position = relativeRef.TransformPoint(currentPos);
		transform.rotation = relativeRef.rotation * currentRot;
	}
}

public struct HandPose
{
	public Vector3 WsPosition;
	public Quaternion WsRotation;
}