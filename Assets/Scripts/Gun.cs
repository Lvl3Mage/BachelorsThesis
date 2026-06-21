using System;
using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] Bullet bulletPrefab;

	[SerializeField] int totalAmmo = 5;
	[SerializeField] float cooldownTime = 0.1f;
	[SerializeField] Transform gunPoint;
	[SerializeField] float muzzleVelocity = 20;

	int currentAmmo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	    currentAmmo = totalAmmo;
    }

    bool onCooldown = false;
    IEnumerator BeginCooldown()
    {
	    onCooldown = true;
	    yield return new WaitForSeconds(cooldownTime);
	    onCooldown = false;
    }
    public bool IsOnCooldown() => onCooldown;


    public bool TryShoot()
    {
	    if (currentAmmo == 0) return false;
	    if (IsOnCooldown()) return false;
	    Shoot();
	    return true;
    }

    void Shoot()
    {
	    Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
	    bullet.GetComponent<Rigidbody>().linearVelocity = gunPoint.forward * muzzleVelocity;
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
