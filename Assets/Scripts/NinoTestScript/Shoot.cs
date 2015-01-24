using UnityEngine;
using System.Collections;



public class Shoot : MonoBehaviour
{
    public GameObject bullet;
    public int weapon_id;
    const int proj_speed = 23;
    enum weapons {rifle, shotgun, rocket, fist_gun};
    private float fire_delay = 0.0f;

    void Update ()
    {
        if (fire_delay > 0)
        {
            fire_delay -= Time.deltaTime;
        }
        if (Input.GetKey ("space") && fire_delay <= 0) // Fix for controller
        {

            switch (weapon_id)
            {
            case (int)weapons.rifle:
                for (int proj_count = 1; i > 0; --i)
                {
                    Vector3 proj_head = get_weapon_spread(4);
                    GameObject new_bullet = (GameObject) Instantiate(bullet, transform.position, Quaternion.identity);
                    new_bullet.GetComponent<Rigidbody>().velocity = proj_head * proj_speed;
                }
                fire_delay = 0.085f;
                break;
            default:
                Debug.Log("Not a valid weapon.");
                break;
            }
        }
    }

    Vector3 get_weapon_spread(int max_degrees_offset)
    {
        float x_head = Mathf.Sin((transform.eulerAngles.y + (Random.value * 2 - 1) * max_degrees_offset) * Mathf.Deg2Rad);
        float z_head = Mathf.Cos((transform.eulerAngles.y + (Random.value * 2 - 1) * max_degrees_offset) * Mathf.Deg2Rad);
        return new Vector3(x_head, 0.0f, z_head);
    }
}
