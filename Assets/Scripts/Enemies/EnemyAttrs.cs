using UnityEngine;
using System.Collections;

public class EnemyAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
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
        enemyAudio.clip = deathSound;
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
        enemyAudio.Play();
        //collider.isTrigger = true;
        Destroy(gameObject, 0.5f);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }
}
