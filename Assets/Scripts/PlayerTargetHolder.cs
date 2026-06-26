using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTargetHolder : MonoBehaviour
{
	static PlayerTargetHolder instance;

	void Awake()
	{
		if (instance){
			Debug.LogError("Another player target holder exists!");
			return;
		}
		instance = this;
	}

	[SerializeField] Transform[] targets;
	public static IEnumerable<Vector3> GetTargets() => instance.targets.Select(t => t.position);
}
