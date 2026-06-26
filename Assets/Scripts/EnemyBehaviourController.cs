using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviourController : MonoBehaviour
{
	[SerializeField] LayerMask sightBlockingLayers;
	[SerializeField] LayerMask playerLayers;
	[FormerlySerializedAs("targets")] [SerializeField] Transform[] waypoints;

	[SerializeField] EnemyMovementController movementController;


	// Update is called once per frame
	void Update()
	{

		foreach (var target in PlayerTargetHolder.GetTargets()){
			if (CheckPlayerVisibility(target)){
				movementController.AimAt(target);
				return;
			}
		}
		Patrol();
	}

	bool CheckPlayerVisibility(Vector3 targetPoint)
	{

		Vector3 targetDelta = targetPoint - transform.position;
		if (!Physics.Raycast(transform.position, targetDelta, out RaycastHit hit, targetDelta.magnitude,
			    playerLayers.value | sightBlockingLayers.value)) return false;
		Debug.DrawLine(transform.position, hit.point);
		return (playerLayers & (1 << hit.collider.gameObject.layer)) != 0;
	}

	int curTargetIndex = 0;

	void Patrol()
	{
		Vector3 targetPos = waypoints[curTargetIndex].position;
		if ((targetPos - transform.position).sqrMagnitude < 0.1f){
			curTargetIndex++;
			if (curTargetIndex >= waypoints.Length){
				curTargetIndex = 0;
			}

			targetPos = waypoints[curTargetIndex].position;
		}
		movementController.PatrolTo(targetPos);
	}
}