using System.Collections.Generic;
using UnityEngine;

public class UniqueInstance : MonoBehaviour
{
	[SerializeField] string instanceKey;

	static HashSet<string> instanceKeys = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

	    if (instanceKeys.Contains(instanceKey)){
		    Destroy(gameObject);
	    }

	    instanceKeys.Add(instanceKey);
	    DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
