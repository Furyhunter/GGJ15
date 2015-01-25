using UnityEngine;
using System.Collections;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weapons;
    public Transform spawnPoint;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
            Instantiate(weapons[(int)(Random.value * weapons.Length)],
                spawnPoint.position, spawnPoint.rotation);
    }
}
