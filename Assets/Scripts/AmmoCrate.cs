using UnityEngine;
using System.Collections;

public class AmmoCrate : MonoBehaviour
{
    public int ammunition;
    public Weapon.AmmoType type;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerAttrs>().ammunition[(int)type] += ammunition;
            Destroy(gameObject);
        }
    }
}
