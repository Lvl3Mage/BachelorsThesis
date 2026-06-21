using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{

	[SerializeField] bool isEntrance;
	[SerializeField] bool openDoors = true;
	[SerializeField] Transform defaultPlayerSpawn;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] Animator animator;

	static bool initialized;
	static Vector3 relativePlayerPosition;
	static Quaternion relativePlayerRotation;
	static readonly int Open = Animator.StringToHash("Open");


	void Awake()
	{

		if (!isEntrance) return;
		if (!initialized){
			Debug.Log("Initializing");
			relativePlayerPosition = transform.InverseTransformPoint(defaultPlayerSpawn.position);
			relativePlayerRotation = Quaternion.Inverse(transform.rotation) * defaultPlayerSpawn.rotation;
			initialized = true;
		}

		Debug.Log(relativePlayerPosition);

		var player = GameObject.FindGameObjectWithTag("Player");
		if (!player){
			player = Instantiate(playerPrefab);
		}
		player.transform.position = transform.TransformPoint(relativePlayerPosition);
		player.transform.rotation = transform.rotation * relativePlayerRotation;
		if (openDoors){
			animator.SetBool(Open, true);
		}
	}
	public void LoadScene(int sceneBuildIndex)
	{
		StartCoroutine(SwitchScene(sceneBuildIndex));
		//todo add lock so can only happen once
	}
	IEnumerator SwitchScene(int sceneBuildIndex)
	{

		var player = GameObject.FindGameObjectWithTag("Player").transform;

		if (openDoors){
			animator.SetBool(Open, false);
			yield return new WaitForSeconds(1);//todo jank
		}
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
		while (!asyncLoad.isDone){
			relativePlayerPosition = transform.InverseTransformPoint(player.position);
			relativePlayerRotation = Quaternion.Inverse(transform.rotation) * player.rotation;
			yield return null;
		}

	}
}
