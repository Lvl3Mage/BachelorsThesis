using Project;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviourController : MonoBehaviour
{
	[SerializeField] LayerMask sightBlockingLayers;
	[SerializeField] LayerMask playerLayers;
	[FormerlySerializedAs("targets")] [SerializeField] Transform[] waypoints;

	[SerializeField] EnemyMovementController movementController;
	[SerializeField] EnemyGun gun;
	[SerializeField] [Range(0f, 90f)] float shotAlignmentThreshold = 5f;

	// Update is called once per frame
	void Update()
	{

		foreach (var target in PlayerTargetHolder.GetTargets()){//todo stagger this
			if (CheckPlayerVisibility(target)){
				TargetAt(target);
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
		return playerLayers.ContainsLayer(hit.collider.gameObject.layer);
	}

	int curTargetIndex = 0;

	void TargetAt(Vector3 target)
	{
		movementController.AimAt(target);
		(Vector3 from, Vector3 direction) aim = gun.GetCurrentAim();
		Vector3 targetAim = target - aim.from;
		float alignmentDegrees = Vector3.Angle(targetAim, aim.direction);
		Debug.Log(alignmentDegrees);
		if (alignmentDegrees > shotAlignmentThreshold){
			return;
		}
		if (gun.IsOnCooldown()){
			return;
		}
		gun.Shoot();

	}

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