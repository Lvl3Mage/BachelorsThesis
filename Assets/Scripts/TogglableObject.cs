using System.Collections;
using Project.Sounds;
using UnityEngine;

public class TogglableObject : MonoBehaviour
{
	[SerializeField] Transform enabledState, disabledState;
	[SerializeField] float animationDuration;
	[SerializeField] AnimationCurve curve;
	[SerializeField] GameSound enabledSound;
	[SerializeField] GameSound disabledSound;


	public IEnumerator SetEnabled(bool enable)
	{
		if (enable && enabledSound){
			AudioManager.Play(enabledSound, () => this ? transform.position : Vector3.zero);
		}
		if (!enable && disabledSound){
			AudioManager.Play(disabledSound, () => this ? transform.position : Vector3.zero);
		}
		float time = 0;
		Vector3 start = enable ? disabledState.position : enabledState.position;
		Vector3 end =  !enable ? disabledState.position : enabledState.position;
		while (time < animationDuration){
			yield return null;
			time += Time.deltaTime;
			float t = time / animationDuration;
			t = curve.Evaluate(t);
			transform.position = Vector3.Lerp(start, end, t);


		}
		transform.position = end;

	}
}
