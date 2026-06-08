using System;
using UnityEngine;

public class HandBehaviourManager : MonoBehaviour
{
	IHandBehaviour[] behaviours;
	[SerializeField] HandVisualizer handVisualizer;
	void Start()
	{
		behaviours = GetComponentsInChildren<IHandBehaviour>();
	}

	IHandBehaviour enabledBehaviour = null;
	void Update()
	{
		int maxPriority =  -1;
		IHandBehaviour maxPriorityBehaviour = null;
		if (enabledBehaviour != null && enabledBehaviour.CanRun()){
			maxPriority = enabledBehaviour.Priority;
			maxPriorityBehaviour = enabledBehaviour;
		}
		foreach (IHandBehaviour behaviour in behaviours){
			if (!behaviour.CanRun()) continue;
			int behaviourPriority = behaviour.Priority;
			if (behaviourPriority <= maxPriority) continue;
			maxPriority = behaviourPriority;
			maxPriorityBehaviour = behaviour;
		}

		TrySwitchBehaviour(maxPriorityBehaviour);
		enabledBehaviour?.Tick();
		HandPose? pose = enabledBehaviour?.GetPose();
		if (pose.HasValue){
			handVisualizer.SetPose(pose.Value);

		}
	}

	void TrySwitchBehaviour(IHandBehaviour newBehaviour)
	{
		if (newBehaviour == enabledBehaviour) return;
		enabledBehaviour?.Exit();
		enabledBehaviour = newBehaviour;
		enabledBehaviour?.Enter();
	}
}

public interface IHandBehaviour
{
	bool CanRun();
	int Priority { get; }

	void Enter();
	void Exit();
	void Tick();

	HandPose GetPose();
}