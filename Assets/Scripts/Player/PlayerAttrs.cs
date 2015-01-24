using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Slider healthSlider;
    public AudioClip deathSound;

    Animator anim;
    AudioSource playerAudio;
    bool isDead;


    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }


    void Update()
    {
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        healthSlider.value = currentHealth;

        playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

        anim.SetTrigger("Die");

        playerAudio.clip = deathSound;
        playerAudio.Play();
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

}
