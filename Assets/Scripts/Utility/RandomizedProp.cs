using UnityEngine;

public class RandomizedProp : MonoBehaviour
{
	[SerializeField] Mesh[] meshSelection;
	[ContextMenu("Randomize Mesh")]
    public void SelectRandomMesh()
    {
	    Mesh randomMesh = meshSelection[Random.Range(0, meshSelection.Length)];
	    if (TryGetComponent(out MeshCollider col)){
		    col.sharedMesh = randomMesh;
	    }
	    GetComponent<MeshFilter>().mesh = randomMesh;
    }
}
