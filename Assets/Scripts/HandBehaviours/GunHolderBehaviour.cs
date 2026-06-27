using System;
using Project.Sounds;
using UnityEngine;

[RequireComponent(typeof(HandInput))]
public class GunHolderBehaviour : MonoBehaviour, IHandBehaviour
{
	GunSlot slot;
	[SerializeField] float pickupRange;
	[SerializeField] Transform gunLocation;
	[SerializeField] int priority = 5;
	[SerializeField] GameSound gripSound;
	[SerializeField] GameSound ungripSound;
	HandInput hand;

	Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
	    player = GameObject.FindGameObjectWithTag("Player").transform;
	    hand = GetComponent<HandInput>();
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
	    equippedGun.Equip(gunLocation,player);
	    AudioManager.Play(gripSound,()=>transform.position);
    }

    public void Exit()
    {
	    AudioManager.Play(ungripSound, ()=>transform.position);
	    equippedGun?.Discard();
	    equippedGun = null;
    }

    bool inputTriggered = false;
    public void Tick()
    {
	    if (hand.GetInputTrigger() > 0.8f){
		    if (!inputTriggered){
				equippedGun?.TryShoot();
		    }
		    inputTriggered = true;
	    }
	    else{
		    inputTriggered = false;
	    }
    }

    public HandPose GetPose()
    {
	    return new HandPose{
		    WsPosition = transform.position,
		    WsRotation = transform.rotation
	    };
    }
}
