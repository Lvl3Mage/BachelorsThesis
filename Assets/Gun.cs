using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	    if (!assignedTarget) return;
	    transform.position = assignedTarget.position;
	    transform.rotation = assignedTarget.rotation;
    }

    Transform assignedTarget;
    public void Equip(Transform target)
    {
	    assignedTarget = target;
    }

    public void Discard()
    {
	    OnDiscard?.Invoke();
	    Destroy(gameObject);
    }

    public event Action OnDiscard;
}
