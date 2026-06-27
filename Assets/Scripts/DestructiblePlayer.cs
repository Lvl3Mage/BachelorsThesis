using System;
using System.Collections;
using Project.Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestructiblePlayer : MonoBehaviour, IDamageable
{
	[SerializeField] DestructibleObject destructibleObject;
	[SerializeField] GameObject[] hideOnDestroyed;

	void Awake()
	{
		if (deathEffect.GetFloat("_Progress") > 0){

			StartCoroutine(UndoDeathEffect());
		}
		destructibleObject.OnDestroyed += () => {
			foreach (GameObject obj in hideOnDestroyed){
				obj.SetActive(false);
			}

			StartCoroutine(PlayDeathEffect());
		};
	}


	[SerializeField] Material deathEffect;
	[SerializeField] float deathEffectLength = 1f;
	IEnumerator PlayDeathEffect()
	{
		float time = 0;
		while (time < deathEffectLength){
			time += Time.deltaTime;
			float t = time / deathEffectLength;
			deathEffect.SetFloat("_Progress", t);
			yield return null;
		}
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
