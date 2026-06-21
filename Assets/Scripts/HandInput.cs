using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;

public class HandInput : MonoBehaviour
{
    [SerializeField] XRNode handNode;

    void Awake()
    {
    }

    InputDevice GetHand()
    {
        return InputDevices.GetDeviceAtXRNode(handNode);
    }

    // Update is called once per frame
    void Update()
    {
        GetHand().TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 handPosition);
        GetHand().TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion handRotation);
        transform.localPosition = handPosition;
        transform.localRotation = handRotation;

    }

    public Vector3 GetVelocity()
    {

        GetHand().TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 vel);
        return vel;
    }

    public float GetInputTrigger()
    {
	    GetHand().TryGetFeatureValue(CommonUsages.trigger, out float trigger);
	    return trigger;
    }
    public bool GetInputGrip()
    {
	    GetHand().TryGetFeatureValue(CommonUsages.gripButton, out bool gripButton);
	    return gripButton;
    }

    public bool GetInputPrimary()
    {
        GetHand().TryGetFeatureValue(CommonUsages.primaryButton, out bool trigger);
        return trigger;
    }
    public bool GetInputSecondary()
    {
        GetHand().TryGetFeatureValue(CommonUsages.secondaryButton, out bool trigger);
        return trigger;
    }
}
