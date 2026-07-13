using System;
using System.Collections;
using Lvl3Mage.InterpolationToolkit;
using Project.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructiblePlayer : MonoBehaviour, IDamageable
{
	[SerializeField] DestructibleObject destructibleObject;
	[SerializeField] GameObject[] hideOnDestroyed;
	[SerializeField] float effectSpeed = 2;
	[SerializeField] AnimationCurve effectCurve;

	void Awake()
	{
		// if (deathEffect.GetFloat("_Progress") > 0){
		//
		// 	StartCoroutine(UndoDeathEffect());
		// }
		destructibleObject.OnDamaged += () => {
			damageOffset += 0.02f;
		};
		destructibleObject.OnDestroyed += () => {
			foreach (GameObject obj in hideOnDestroyed){
				obj.SetActive(false);
			}

			StartCoroutine(PlayDeathEffect());
		};
	}

	float damageOffset = 0;

	void Update()
	{
		float targetProgress =  1.0f - destructibleObject.GetHealth() / destructibleObject.GetMaxHealth();

		float effectProgress = deathEffect.GetFloat("_Progress");
		effectProgress = Mathf.Clamp01(Decay.To(effectProgress, effectCurve.Evaluate(targetProgress), effectSpeed, Time.deltaTime) + damageOffset * (1-targetProgress));
		deathEffect.SetFloat("_Progress",effectProgress);
		damageOffset = Decay.To(damageOffset, 0, 5, Time.deltaTime);
	}


	[SerializeField] Material deathEffect;
	[SerializeField] float deathEffectLength = 1f;
	IEnumerator PlayDeathEffect()
	{
		yield return new WaitForSeconds(0.2f);
		deathEffect.SetFloat("_Progress", 1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	IEnumerator UndoDeathEffect()
	{
		float time = 0;
		while (time < deathEffectLength){
			time += Time.deltaTime;
			float t = time / deathEffectLength;

			deathEffect.SetFloat("_Progress", 1-t);
			yield return null;
		}
		deathEffect.SetFloat("_Progress", 0);
	}

	public void DealDamage(float damage)
	{
		destructibleObject.DealDamage(damage);
	}

	public bool IsDestroyed()
	{
		return destructibleObject.IsDestroyed();
	}
}
