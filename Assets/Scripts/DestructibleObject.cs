using System;
using Project.Sounds;
using UnityEngine;
using UnityEngine.Serialization;

public class DestructibleObject : MonoBehaviour, IDamageable
{
	[SerializeField] GameObject destructionEffect;

	float health;
	[FormerlySerializedAs("health")] [SerializeField] float maxHealth;

	[SerializeField] GameSound destructionSound;
	[SerializeField] GameSound damageSound;
	[SerializeField] bool destroyOnKill;

	void Awake()
	{
		health = maxHealth;
	}

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
	    OnDamaged?.Invoke();
	    if (damageSound){
			AudioManager.Play(damageSound, ()=> this?.transform.position);
	    }
    }

    void ObjectDestroyed()
    {
	    OnDestroyed?.Invoke();
	    if (destructionEffect){
			Instantiate(destructionEffect, transform.position, transform.rotation);
	    }
	    if (destructionSound){
			AudioManager.Play(destructionSound, ()=> this?.transform.position);

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
    public event Action OnDamaged;
    public float GetHealth() => health;
    public float GetMaxHealth() => maxHealth;
}
