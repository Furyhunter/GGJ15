using UnityEngine;
using System.Collections;
using System;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    public int[] ptValues;
    public int points;

    public Transform[] spawnPoints;
    private int currentSpawnPoint;

    void Start()
    {
        if (ptValues.Length != enemies.Length)
            throw new Exception("Not all enemies have assigned point values");
        currentSpawnPoint = 0;
        Spawn();
    }

    void Spawn()
    {
        int index = enemies.Length - 1;

        while (points > 0)
        {
            while (ptValues[index] > points)
                index--;
            int i = rval(index);
            points -= ptValues[i];
            spawnEnemy(i);
        }
    }

    int rval(int to)
    {
        return (int)(UnityEngine.Random.value * to);
    }

    void spawnEnemy(int i)
    {
        currentSpawnPoint = (currentSpawnPoint + 1) % spawnPoints.Length;
        
        Instantiate(enemies[i], 
            spawnPoints[currentSpawnPoint].position, 
            spawnPoints[currentSpawnPoint].rotation);
    }
}
