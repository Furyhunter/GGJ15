using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    GameObject lastHit;
    //EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;


    void Awake()
    {
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            lastHit = other.gameObject;
            playerInRange = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
            playerInRange = false;
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange/* && enemyHealth.currentHealth > 0*/)
        {
            Attack();
        }

        //if (playerHealth.currentHealth <= 0)
        {
        //    anim.SetTrigger("PlayerDead");
        }
    }


    void Attack()
    {
        if (lastHit == null)
            return;
        timer = 0f;

        PlayerAttrs health = lastHit.GetComponent<PlayerAttrs>();
        if (health.getCurrentHealth() > 0)
        {
           health.TakeDamage(attackDamage);
        }
    }
}
