using System;
using System.Collections;
using Lvl3Mage.InterpolationToolkit;
using Project.Sounds;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gun : MonoBehaviour
{
	[SerializeField] Bullet bulletPrefab;

	[SerializeField] int totalAmmo = 5;
	[SerializeField] float cooldownTime = 0.1f;
	[SerializeField] Transform gunPoint;
	[SerializeField] float muzzleVelocity = 20;
	[SerializeField] float attractionStrength = 18;
	[SerializeField] GameSound shotSound;
	[SerializeField] float linearRecoil = 0.2f;
	[SerializeField] Vector2 recoilYaw = new Vector2(-30f,30f);
	[SerializeField] Vector2 recoilPitch = new Vector2(-60f, -30f);
	[SerializeField] float recoilDecay = 18;
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
	    StartCoroutine(BeginCooldown());
	    return true;
    }

    void Shoot()
    {
	    Bullet bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
	    bullet.GetComponent<Rigidbody>().linearVelocity = gunPoint.forward * muzzleVelocity;
	    recoilOffset -= gunPoint.forward * linearRecoil;
	    recoilRotationOffset *= Quaternion.AngleAxis(Random.Range(recoilPitch.x,recoilPitch.y),Vector3.right) *Quaternion.AngleAxis(Random.Range(recoilYaw.x,recoilYaw.y), Vector3.up);
	    AudioManager.Play(shotSound, ()=>this ? transform.position :  Vector3.zero);
    }

    Vector3 recoilOffset;

    Quaternion recoilRotationOffset;
    // Update is called once per frame
    void Update()
    {
	    if (!assignedTarget) return;
	    transform.position = Decay.To(transform.position, assignedTarget.position + recoilOffset, attractionStrength, Time.deltaTime);
	    transform.rotation = Decay.To(transform.rotation, assignedTarget.rotation *recoilRotationOffset, attractionStrength, Time.deltaTime, Quaternion.Slerp);
	    recoilOffset = Decay.To(recoilOffset, Vector3.zero, recoilDecay, Time.deltaTime);
	    recoilRotationOffset = Decay.To(recoilRotationOffset, Quaternion.identity, recoilDecay, Time.deltaTime, Quaternion.Slerp);
    }

    Transform assignedTarget;
    public void Equip(Transform target, Transform relativeTo)
    {
	    assignedTarget = target;
	    transform.parent = relativeTo;
    }

    public void Discard()
    {
	    OnDiscard?.Invoke();
	    Destroy(gameObject);
    }

    public event Action OnDiscard;
}
