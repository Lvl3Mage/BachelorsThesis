using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecenter : MonoBehaviour
{
	[SerializeField] HandManager handLeft, handRight;

	[SerializeField] Transform camera;

	[SerializeField] Transform playerRig;

	// Start is called before the first frame update
	void Start()
	{
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
			playerRig.position = transform.TransformPoint(newOffset);

			float cameraLookDirection = camera.eulerAngles.y;
			float rigLookDirection = playerRig.eulerAngles.y;

			playerRig.localRotation = Quaternion.AngleAxis(rigLookDirection - cameraLookDirection, Vector3.up);

		}
	}

	bool IsTriggeringRecenter()
	{
		return handLeft.GetInputPrimary() && handRight.GetInputPrimary();
	}
}