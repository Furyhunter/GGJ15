using UnityEngine;
using System.Collections;

public class InitialWeaponSpawner : MonoBehaviour
{
    public GameObject[] ammoCrates;
    public GameObject[] guns;
    public Transform[] spawnPoints;

	void Start()
	{
        Spawn();
	}

    void Spawn()
    {
        for (int k = 0; k < spawnPoints.Length; k+=2)
        {
            int i = (int)(Random.value * ammoCrates.Length);
            Instantiate(guns[i],
                spawnPoints[k].position, spawnPoints[k].rotation);
            Instantiate(ammoCrates[i],
                spawnPoints[k + 1].position, spawnPoints[k + 1].rotation);
        }
            
    }

}
