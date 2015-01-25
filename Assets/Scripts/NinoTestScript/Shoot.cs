using UnityEngine;
using System.Collections;
using InControl;



public class Shoot : MonoBehaviour
{
    private float fire_delay = 0.0f;

    private PlayerAttrs attrs;
    
    public Weapon CurrentWeapon;

    private InputDevice controller;

    void Start()
    {
        attrs = GetComponent<PlayerAttrs>();
        controller = gameObject.GetComponent<PlayerAttrs>().controller;
    }

    void Update ()
    
    {
        if (fire_delay > 0)
        {
            fire_delay -= Time.deltaTime;
        }
        if (controller.RightTrigger && CurrentWeapon != null && fire_delay <= 0) // Fix for controller
        {
            if (attrs.ammunition[(int)CurrentWeapon.Ammo] <= 0)
            {
                //Do Something
            }
            else
            {
                attrs.ammunition[(int)CurrentWeapon.Ammo]--;
                create_shots(CurrentWeapon.Ammunition, CurrentWeapon.ProjCount, CurrentWeapon.ProjSpeed,
                             CurrentWeapon.ProjSpread, CurrentWeapon.RefireDelay);
            }
        }
    }

    Vector3 get_weapon_spread(int max_degrees_offset)
    {
        float x_head = Mathf.Sin((transform.eulerAngles.y + (Random.value * 2 - 1) * max_degrees_offset) * Mathf.Deg2Rad);
        float z_head = Mathf.Cos((transform.eulerAngles.y + (Random.value * 2 - 1) * max_degrees_offset) * Mathf.Deg2Rad);
        return new Vector3(x_head, 0.0f, z_head);
    }

    void create_shots(GameObject ammunition, int proj_count, int proj_speed, int proj_spread, float refire_delay)
    {
        for (int i = proj_count; i > 0; --i)
        {
            Vector3 proj_head = get_weapon_spread(proj_spread);
            GameObject new_bullet = (GameObject) Instantiate(ammunition, gameObject.GetComponentInChildren<Weapon>().gameObject.transform.position, Quaternion.identity);
            new_bullet.gameObject.GetComponent<BulletBehaviour>().owner = gameObject;
            new_bullet.GetComponent<Rigidbody>().velocity = proj_head * proj_speed;
        }
        fire_delay = refire_delay;
    }
}
