using System.Linq;
using Lvl3Mage.InterpolationToolkit;
using UnityEngine;

[RequireComponent(typeof(HandManager))]
public class WheelGrabBehaviour : MonoBehaviour, IHandBehaviour
{
	[SerializeField] float gripRange = 0.4f;

	[SerializeField] float maxStrength = 10;

	[SerializeField] Transform player;
	WheelController[] wheels;
	[SerializeField] int priority = 5;
	HandManager hand;

	void Start()
	{
		hand = GetComponent<HandManager>();
		wheels = GameObject.FindGameObjectsWithTag("Wheel").Select(o => o.GetComponent<WheelController>()).ToArray();
	}

	WheelController grabbedWheel = null;
	Vector3 previousGripPose;

	bool IsGripPoseValid(HandPose gripPose)
	{
		return (gripPose.WsPosition - transform.position).magnitude <= gripRange;
	}

	WheelController GetValidWheel() =>
		(from wheel in wheels
			let gripPose = wheel.GetGripPoseAt(transform.position)
			where IsGripPoseValid(gripPose)
			select wheel).FirstOrDefault();

	void SetPreviousPose(HandPose pose)
	{
		previousGripPose = player.InverseTransformPoint(pose.WsPosition);
	}

	void ApplyGrip(WheelController wheel, Vector3 position, Vector3 gripVel, float gripStrength)
	{
		Vector3 wheelVel = wheel.GetVelocityAt(position);
		Vector3 targetVel = Decay.To(wheelVel, gripVel, gripStrength, Time.deltaTime);
		wheel.SetVelocityAt(position, targetVel);
	}

	public bool CanRun()
	{
		if (hand.GetInputTrigger() <= 0.2f) return false;
		return GetValidWheel();
	}

	public int Priority => priority;

	public void Enter()
	{
		WheelController validWheel = GetValidWheel();
		Debug.Assert(validWheel);
		grabbedWheel = validWheel;
		SetPreviousPose(grabbedWheel.GetGripPoseAt(transform.position));
	}

	public void Exit()
	{
		grabbedWheel = null;
	}

	public void Tick()
	{
		var gripPose = grabbedWheel.GetGripPoseAt(transform.position);
		Vector3 delta = player.InverseTransformPoint(gripPose.WsPosition) - previousGripPose;
		Vector3 targetVel = player.TransformDirection(delta / Time.deltaTime);
		previousGripPose = player.InverseTransformPoint(gripPose.WsPosition);
		ApplyGrip(grabbedWheel, gripPose.WsPosition, targetVel, hand.GetInputTrigger() * maxStrength);
	}

	public HandPose GetPose()
	{
		return grabbedWheel.GetGripPoseAt(transform.position);
	}
}