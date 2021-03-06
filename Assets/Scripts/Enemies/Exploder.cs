﻿using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour
{
    Transform player;               // Reference to the player's position.
    PlayerAttrs playerHealth;      // Reference to the player's health.
    EnemyAttrs enemyHealth;        // Reference to this enemy's health.
    NavMeshAgent nav;               // Reference to the nav mesh agent.


    void Awake ()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerAttrs> ();
        enemyHealth = GetComponent <EnemyAttrs> ();
        nav = GetComponent <NavMeshAgent> ();
    }


    void Update ()
    {
        // If the enemy and the player have health left...
        if(enemyHealth.getCurrentHealth() > 0 && playerHealth.getCurrentHealth() > 0)
        {
            // ... set the destination of the nav mesh agent to the player.
            nav.SetDestination (player.position);
        }
        // Otherwise...
        else
        {
            // ... disable the nav mesh agent.
            nav.enabled = false;
        }
    } 
	void Start()
	{
	
	}
}
