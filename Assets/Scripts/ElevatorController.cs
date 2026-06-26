using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lvl3Mage.CoroutineToolkit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorController : MonoBehaviour
{

	[SerializeField] bool isEntrance;
	[SerializeField] bool openDoors = true;
	[SerializeField] bool openPanel = true;
	[SerializeField] Transform defaultPlayerSpawn;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] TogglableObject[] doors;
	[SerializeField] TogglableObject[] panelObjects;

	static bool initialized;
	static Vector3 relativePlayerPosition;
	static Quaternion relativePlayerRotation;

	void Awake()
	{

		if (openDoors){
			StartCoroutine(AnimateDoors(false));
		}

		if (openPanel){
			StartCoroutine(AnimatePanel(true));

		}
		if (!isEntrance) return;
		if (!initialized){
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
	}
	public void LoadScene(int sceneBuildIndex)
	{
		StartCoroutine(SwitchScene(sceneBuildIndex));
		//todo add lock so can only happen once
	}
	IEnumerator SwitchScene(int sceneBuildIndex)
	{

		var player = GameObject.FindGameObjectWithTag("Player").transform;
		List<Coroutine> coroutines = new();
		if (openPanel){
			coroutines.Add(StartCoroutine(AnimatePanel(false)));

		}
		if (openDoors){
			coroutines.Add(StartCoroutine(AnimateDoors(true)));
		}

		yield return CoroutineUtility.WaitForAll(coroutines.ToArray());
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneBuildIndex);
		while (!asyncLoad.isDone){
			relativePlayerPosition = transform.InverseTransformPoint(player.position);
			relativePlayerRotation = Quaternion.Inverse(transform.rotation) * player.rotation;
			yield return null;
		}

	}

	IEnumerator AnimateDoors(bool close)
	{
		yield return CoroutineUtility.WaitForAll(doors.Select(door => door.SetEnabled(close)).ToArray());

	}
	IEnumerator AnimatePanel(bool open)
	{
		yield return CoroutineUtility.WaitForAll(panelObjects.Select(o => o.SetEnabled(open)).ToArray());

	}
}
