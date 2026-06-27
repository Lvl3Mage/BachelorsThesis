using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecenter : MonoBehaviour
{
	[SerializeField] HandInput handLeft, handRight;

	[SerializeField] Transform camera;

	[SerializeField] Transform playerRig;

	// Start is called before the first frame update
	void Start()
	{

		playerRig.position = transform.TransformPoint(recenterOffset);
		playerRig.localRotation = recenterRotation;
	}

	float recenterTimer = 0;

	// Update is called once per frame
	void Update()
	{
		if (IsTriggeringRecenter()){
			recenterTimer += Time.deltaTime;
		}
		else{
			recenterTimer = 0;
		}

		if (recenterTimer >= 1){
			recenterTimer = 0;

			Vector3 cameraPosition = transform.InverseTransformPoint(camera.position);
			Vector3 newOffset = transform.InverseTransformPoint(playerRig.position) - cameraPosition;
			newOffset.y +=1.36144f;
			recenterOffset = newOffset;
			playerRig.position = transform.TransformPoint(recenterOffset);
			float cameraLookDirection = camera.eulerAngles.y;
			float rigLookDirection = playerRig.eulerAngles.y;
			recenterRotation = Quaternion.AngleAxis(rigLookDirection - cameraLookDirection, Vector3.up);

			playerRig.localRotation = recenterRotation;

		}
	}

	static Vector3 recenterOffset = Vector3.zero;
	static Quaternion recenterRotation = Quaternion.identity;

	bool IsTriggeringRecenter()
	{
		return handLeft.GetInputPrimary() && handRight.GetInputPrimary();
	}
}