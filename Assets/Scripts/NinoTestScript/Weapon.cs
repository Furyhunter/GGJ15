using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public enum AmmoType {
        rifle = 0,
        shotgun = 1, 
        rocket = 2, 
        pistol = 3
    };

	public GameObject Ammunition; 
	public int ProjCount;
	public int ProjSpeed;
	public int ProjSpread; 
	public float RefireDelay;
    public float PickupDelay;
    public AudioClip shot;

	public AmmoType Ammo;

    void Start()
    {
        gameObject.GetComponent<AudioSource>().clip = shot;
    }

    void Update()
    {
        if (PickupDelay > 0)
            PickupDelay -= Time.deltaTime;
    }

}
