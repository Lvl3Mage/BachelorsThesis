using System.Collections;
using Project.Sounds;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
	[SerializeField] Transform barrelEnd;

	[SerializeField] Rigidbody bulletPrefab;
	[SerializeField] float cooldownTime = 2;
	[SerializeField] float bulletVelocity = 20;
	[SerializeField] GameSound shotSound;

	public void Shoot()
	{
		if (onCooldown) return;
		Rigidbody bullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
		bullet.linearVelocity = bullet.transform.forward*bulletVelocity;
		AudioManager.Play(shotSound, () => this? transform.position : Vector3.zero);
		StartCoroutine(Cooldown());
	}

	bool onCooldown = false;

	public bool IsOnCooldown()
	{
		return onCooldown;
	}

	IEnumerator Cooldown()
	{
		onCooldown = true;
		yield return new WaitForSeconds(cooldownTime);
		onCooldown = false;
	}

	public (Vector3 from, Vector3 direction) GetCurrentAim()
	{
		return (barrelEnd.position, barrelEnd.forward);
	}
}
