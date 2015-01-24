using UnityEngine;
using System.Collections;



public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    private float fire_delay = 0.0f;

    public Weapon CurrentWeapon;

    void Update ()
    
    {
        if (fire_delay > 0)
        {
            fire_delay -= Time.deltaTime;
        }
        if (Input.GetKey ("space") && fire_delay <= 0) // Fix for controller
        {
            create_shots(CurrentWeapon.Ammunition, CurrentWeapon.ProjCount, CurrentWeapon.ProjSpeed,
                         CurrentWeapon.ProjSpread, CurrentWeapon.RefireDelay);
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
            GameObject new_bullet = (GameObject) Instantiate(ammunition, transform.position, Quaternion.identity);
            new_bullet.gameObject.GetComponent<BulletBehaviour>().owner = gameObject;
            new_bullet.GetComponent<Rigidbody>().velocity = proj_head * proj_speed;
        }
        fire_delay = refire_delay;
    }
}
