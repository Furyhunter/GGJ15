using UnityEngine;
using System.Collections;

public class EnemyAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public AudioClip deathSound;

    Animator anim;
    AudioSource enemyAudio;
    //CapsuleCollider collider;

    bool isDead;
    bool isSinking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        //collider = GetComponent<CapsuleCollider>();

        currentHealth = maxHealth;
    }

	void Update()
	{

	}

    public void TakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        //collider.isTrigger = true;
        Destroy(gameObject);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }
}
