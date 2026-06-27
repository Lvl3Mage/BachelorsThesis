using System;
using Project.Sounds;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDamageable
{
	[SerializeField] GameObject destructionEffect;

	[SerializeField] float health;

	[SerializeField] GameSound destructionSound;
	[SerializeField] GameSound damageSound;
	[SerializeField] bool destroyOnKill;

    public void DealDamage(float damage)
    {
	    if (IsDestroyed()){
		    return;
	    }
	    health -= damage;
	    if (health > 0){
		    ObjectDamaged();
		    return;
	    }
	    ObjectDestroyed();
	    health = 0;

    }

    void ObjectDamaged()
    {
	    if (damageSound){
			AudioManager.Play(damageSound, ()=> this ? transform.position : Vector3.zero);
	    }
    }

    void ObjectDestroyed()
    {
	    OnDestroyed?.Invoke();
	    if (destructionEffect){
			Instantiate(destructionEffect, transform.position, transform.rotation);
	    }
	    if (destructionSound){
			AudioManager.Play(destructionSound, ()=> this ? transform.position : Vector3.zero);

	    }
	    if (destroyOnKill){
			Destroy(gameObject);

	    }
    }

    public bool IsDestroyed()
    {
	    return health <= 0;
    }

    public event Action OnDestroyed;
}
