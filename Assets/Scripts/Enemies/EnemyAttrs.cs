using UnityEngine;
using System.Collections;

public class EnemyAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    public float sinkSpeed = 2.5f;
    public AudioClip deathSound;

    Animator anim;
    AudioSource enemyAudio;
    CapsuleCollider collider;

    ParticleSystem hitParticles;

    bool isDead;
    bool isSinking;

    void Awake()
    {
        anim = GetComponent<Animator>();
        enemyAudio = GetComponent<AudioSource>();
        hitParticles = GetComponentInChildren<ParticleSystem>();
        collider = GetComponent<CapsuleCollider>();
        
        currentHealth = maxHealth;
    }

	void Update()
	{
        if (isSinking)
        {
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
	}

    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        if (isDead)
            return;

        enemyAudio.Play();

        currentHealth -= amount;

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();

        if (currentHealth <= 0)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        collider.isTrigger = true;

        anim.SetTrigger("Dead");

        enemyAudio.clip = deathSound;
        enemyAudio.Play();
    }


    public void StartSinking()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isSinking = true;
        //ScoreManager.score += scoreValue;
        Destroy(gameObject, 2f);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }
}
