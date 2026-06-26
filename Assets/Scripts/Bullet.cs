using System;
using Project;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] LayerMask targetLayers;
	[SerializeField] float damage;
	[SerializeField] GameObject explosionEffect;

	void OnCollisionEnter(Collision collision)
	{
		if (!targetLayers.ContainsLayer(collision.collider.gameObject.layer)){
			Explode();
			return;
		}

		IDamageable damageable = collision.collider.gameObject.GetComponentInParent<IDamageable>();
		if (damageable == null){
			Explode();
			return;
		}
		damageable.DealDamage(damage);
		Explode();

	}

	void Explode()
	{
		Instantiate(explosionEffect, transform.position, transform.rotation);
		Destroy(gameObject);

	}
}
