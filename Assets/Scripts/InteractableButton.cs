using UnityEngine;
using UnityEngine.Events;

public class InteractableButton : MonoBehaviour
{
	[SerializeField] Animator animator;
	[SerializeField] UnityEvent onPressed;
	[SerializeField] Transform interactionPose;
	public void StartHover()
	{
		// animator.SetBool("Hover", true);
	}

	public void EndHover()
	{
		// animator.SetBool("Hover", false);

	}

	public void OnPress()
	{

		animator.SetTrigger("Press");
		onPressed?.Invoke();
	}

	public HandPose GetPose()
	{
		return new HandPose{
			WsPosition = interactionPose.position,
			WsRotation = interactionPose.rotation,
		};
	}
}