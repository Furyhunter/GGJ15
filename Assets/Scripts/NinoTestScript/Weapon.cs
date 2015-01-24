using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public enum AmmoType {rifle , shotgun , rocket , pistol};

	public GameObject Ammunition; 
	public int ProjCount;
	public int ProjSpeed;
	public int ProjSpread; 
	public float RefireDelay;

	public AmmoType Ammo;
}
