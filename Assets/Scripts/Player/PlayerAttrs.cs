using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public Slider healthSlider;
    public AudioClip deathSound;

    public int[] ammunition = {0, 0, 0, 0};

    Animator anim;
    AudioSource playerAudio;
    bool isDead;


    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        //healthSlider.value = currentHealth;

        //playerAudio.Play();

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        //anim.SetTrigger("Die");

        //playerAudio.clip = deathSound;
        //playerAudio.Play();

        Destroy(gameObject);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

}
