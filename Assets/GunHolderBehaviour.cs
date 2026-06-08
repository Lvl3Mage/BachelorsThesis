using System;
using UnityEngine;

[RequireComponent(typeof(HandManager))]
public class GunHolderBehaviour : MonoBehaviour, IHandBehaviour
{
	GunSlot slot;
	[SerializeField] float pickupRange;
	[SerializeField] Transform gunLocation;
	[SerializeField] int priority = 5;
	HandManager hand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
	    hand = GetComponent<HandManager>();
    }

    void Start()
    {
	    slot = GameObject.FindGameObjectWithTag("GunSlot").GetComponent<GunSlot>();
    }
    Gun equippedGun;
    public bool CanRun()
    {
	    if (equippedGun){
		    return hand.GetInputGrip();
	    }
	    return (gunLocation.position - slot.GetSlotPosition()).magnitude < pickupRange && !slot.IsEmpty() && hand.GetInputGrip();
    }

    public int Priority => priority;

    public void Enter()
    {
	    equippedGun = slot.RetrieveGun();
	    equippedGun.Equip(gunLocation);
    }

    public void Exit()
    {
	    equippedGun?.Discard();
	    equippedGun = null;
    }

    public void Tick() { }

    public HandPose GetPose()
    {
	    return new HandPose{
		    WsPosition = transform.position,
		    WsRotation = transform.rotation
	    };
    }
}
