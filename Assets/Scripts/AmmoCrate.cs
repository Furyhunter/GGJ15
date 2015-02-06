using UnityEngine;
using System.Collections;
using System;

public class AmmoCrate : MonoBehaviour
{
    public int ammunition;
    public Weapon.AmmoType type;
    public GameObject soundObject;

    public void Start()
    {
        double mult = 1 + 0.15 * (GlobalState.NumberOfPlayers - 1);
        ammunition = (int)(Math.Round(ammunition * mult));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerAttrs>().ammunition[(int)type] += ammunition;
            Instantiate(soundObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
