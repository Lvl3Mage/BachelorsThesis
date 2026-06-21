using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(HandInput))]
public class InteractionBehaviour : MonoBehaviour, IHandBehaviour
{
	HandInput hand;
	[SerializeField] LayerMask interactableLayers;
	[SerializeField] float interactionRadius;
	[SerializeField] Transform interactionPoint;
	[SerializeField] int interactionPriority;

	void Awake()
	{
		hand = GetComponent<HandInput>();
	}

	void Update()
	{
		List<InteractableButton> buttons = Physics
			.OverlapSphere(interactionPoint.position, interactionRadius, interactableLayers)
			.Select(col => col.GetComponent<InteractableButton>()).Where(btn => btn).ToList();

		closestButton = null;
		float minDistance = Mathf.Infinity;
		foreach (InteractableButton interactableButton in buttons){
			float distance = (interactableButton.transform.position - transform.position).magnitude;
			if (!(distance < minDistance)) continue;
			closestButton = interactableButton;
			minDistance = distance;
		}

	}

	InteractableButton closestButton;
	InteractableButton hoveredButton;

	public bool CanRun()
	{
		return closestButton;
	}

	public int Priority => interactionPriority;

	public void Enter()
	{
		hoveredButton = closestButton;
		hoveredButton?.StartHover();
	}

	public void Exit()
	{
		if (!hoveredButton){
			hoveredButton = null;
			return;
		}
		hoveredButton?.EndHover();
		hoveredButton = null;
	}

	bool inputTriggered = false;
	public void Tick()
	{
		if (closestButton != hoveredButton){
			hoveredButton?.EndHover();
			hoveredButton = closestButton;
			hoveredButton?.StartHover();
		}

		if (!hoveredButton){
			return;
		}

		if (hand.GetInputTrigger() > 0.5f){
			if (!inputTriggered){
				hoveredButton.OnPress();
			}
			inputTriggered = true;
		}
		else{
			inputTriggered = false;
		}
	}

	public HandPose GetPose()
	{
		if (hoveredButton){
			HandPose pose = hoveredButton.GetPose();
			return new HandPose(){
				WsPosition = Vector3.Lerp(pose.WsPosition, transform.position, 0.3f),
				WsRotation = Quaternion.Slerp(pose.WsRotation, transform.rotation, 0.3f)
			};
		}

		return new HandPose{
			WsPosition = transform.position,
			WsRotation = transform.rotation
		};
	}
}