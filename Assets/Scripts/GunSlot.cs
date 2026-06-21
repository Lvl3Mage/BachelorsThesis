using UnityEngine;

public class GunSlot : MonoBehaviour
{
	[SerializeField] Gun gunPrefab;

	[SerializeField] Transform spawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
	    RespawnGun();
    }

    Gun spawnedGun = null;
    void RespawnGun()
    {
	    spawnedGun = Instantiate(gunPrefab, spawnPoint);
	    spawnedGun.OnDiscard += RespawnGun;
    }

    public Gun RetrieveGun()
    {
	    spawnedGun.transform.parent = null;
	    Gun retrievedGun = spawnedGun;
	    spawnedGun = null;
	    return retrievedGun;
    }

    public bool IsEmpty()
    {
	    return !spawnedGun;
    }

    public Vector3 GetSlotPosition()
    {
	    return spawnPoint.position;
    }
}
