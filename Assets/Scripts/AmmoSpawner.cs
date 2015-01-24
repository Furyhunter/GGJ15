using UnityEngine;
using System.Collections;

public class AmmoSpawner : MonoBehaviour
{
    public GameObject[] ammoCrates;
    public Transform[] spawnPoints;

	void Start()
	{
        Spawn();
	}
	
	void Update()
	{
	
	}

    void Spawn()
    {
        for (int k = 0; k < spawnPoints.Length; k++)
            Instantiate(ammoCrates[(int)(Random.value * ammoCrates.Length)],
                spawnPoints[k].position, spawnPoints[k].rotation);
    }
}
