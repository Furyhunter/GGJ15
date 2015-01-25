using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameHUD : MonoBehaviour
{
    public GameObject Player;
    
    public Slider healthSlider;

    public Text AmmoOfCurrentGun;

    public Text[] Ammunition= new Text[3];

    //private Color[] Colors = {Color.red, Color.blue, Color.green, Color.yellow};

    public int MaxHealth
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().maxHealth;
        }
    }

    public int Health{
        get
        {
            return Player.GetComponent<PlayerAttrs>().getCurrentHealth();
        }
    }

    /*public Color PlayerColor
    {
        get
        {
            //return Player.GetComponent<PlayerAttrs>().Color
        }
    }*/

    public bool HasWeapon
    {
        get
        {
            return Player.GetComponent<Shoot>().CurrentWeapon != null;
        }
    }

    public Weapon gun
    {
        get
        {
            return Player.GetComponent<Shoot>().CurrentWeapon;
        }
    }

    public int currentGunAmmo
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().ammunition[(int)Player.GetComponent<Shoot>().CurrentWeapon.Ammo];
            //return (int)Player.GetComponent<Shoot>().CurrentWeapon.;
        }
    }

    public int RifleAmmo
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().ammunition[0]; //0 is the rifle
        }
    }

    public int ShotgunAmmo
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().ammunition[1]; //1 is the shotgun
        }
    }

    public int RocketAmmo
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().ammunition[2]; //2 is the rocket
        }
    }

    public int PistolAmmo
    {
        get
        {
            return Player.GetComponent<PlayerAttrs>().ammunition[3]; //3 is the Pistol
        }
    }

	void Start()
	{
	}
	
	void Update()
    {
        if (Player == null)
        {
            Debug.Log("Player is dead, disabling hud object", this);
            enabled = false;
            return;
        }
        if (Player.GetComponent<PlayerAttrs>() == null)
        {
            Debug.Log("Playerattrs missing, disabling hud object", this);
            enabled = false;
            return;
        }
        healthSlider.value = ((float)Health/(float)MaxHealth);
        AmmoOfCurrentGun.text = "" + (HasWeapon ? currentGunAmmo : 0);
        Ammunition[0].text = "" + RifleAmmo; //RifleAmmo is 0
        Ammunition[1].text = "" + ShotgunAmmo; //Shotgun is 1
        Ammunition[2].text = "" + RocketAmmo; //Rocket is 2
	}
}
