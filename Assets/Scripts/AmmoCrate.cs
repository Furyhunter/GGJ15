using UnityEngine;
using System.Collections;

public class AmmoCrate : MonoBehaviour
{
    public int ammunition;
    public Weapon.AmmoType type;
    public GameObject soundObject;

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
