using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rigidbody;
	[SerializeField] WheelController leftWheel, rightWheel;
	[SerializeField] AnimationCurve angularRemap;
	[SerializeField] float maxAngularVel;

	[SerializeField] Transform floorContactLeft, floorContactRight;

	[SerializeField] float linearBoost = 3f;
    void Awake()
    {
	    rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
	    Vector3 leftVelocity = -leftWheel.GetVelocityAt(floorContactLeft.position);
	    Vector3 rightVelocity = -rightWheel.GetVelocityAt(floorContactRight.position);

	    // Linear velocity of the body
	    Vector3 linearVelocity = (leftVelocity + rightVelocity) * (0.5f * linearBoost);

	    // Wheel separation
	    Vector3 wheelAxis = floorContactRight.position - floorContactLeft.position;
	    float wheelDistance = wheelAxis.magnitude;

	    // Angular velocity about the up axis
	    float leftForward = Vector3.Dot(leftVelocity, transform.forward);
	    float rightForward = Vector3.Dot(rightVelocity, transform.forward);

	    float angularVelocityY = (leftForward-rightForward) / wheelDistance;

	    rigidbody.linearVelocity = new Vector3(linearVelocity.x, rigidbody.linearVelocity.y, linearVelocity.z);

	    float absAngular = Mathf.Abs(angularVelocityY)/maxAngularVel;
	    absAngular = angularRemap.Evaluate(absAngular)*maxAngularVel;
	    absAngular = Mathf.Clamp(absAngular, 0, maxAngularVel);
	    rigidbody.angularVelocity = transform.up * (absAngular * Mathf.Sign(angularVelocityY));

    }
}
