using System;
using UnityEngine;

public class HandVisualizer : MonoBehaviour
{
	HandPose pose;
	public void SetPose(HandPose newPose)
	{
		pose = newPose;
	}

	void LateUpdate()
	{

		transform.position = pose.WsPosition;
		transform.rotation = pose.WsRotation;
	}
}

public struct HandPose
{
	public Vector3 WsPosition;
	public Quaternion WsRotation;
}