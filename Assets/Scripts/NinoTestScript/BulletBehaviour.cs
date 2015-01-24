﻿using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour
{
    public float BulletLife = 4.0f;
    public bool DestroyOnContact = true;
    public float ExplosionRadius = 0.0f;
    public int Damage = 4;
    public GameObject owner = null;

    void Update()
    {
        if (BulletLife < 0)
            GameObject.Destroy(gameObject, 0.0f);
        BulletLife -= Time.deltaTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject != owner)
        {
            //do bullet things
            if (DestroyOnContact)
            {
                GameObject.Destroy(gameObject, 0.0f);
            }
        }
    }
}