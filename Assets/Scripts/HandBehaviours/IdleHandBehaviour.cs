using UnityEngine;

public class IdleHandBehaviour : MonoBehaviour, IHandBehaviour
{
	[SerializeField] int priority = 0;
	public bool CanRun() => true;

	public int Priority => priority;
	public void Enter() { }

	public void Exit() { }

	public void Tick() { }

	public HandPose GetPose() => new(){
		WsPosition = transform.position,
		WsRotation = transform.rotation
	};
}