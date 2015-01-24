using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerAttrs : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    
    public Slider healthSlider;
    public AudioClip deathSound;

    public int[] ammunition = {0, 0, 0, 0};

    Animator anim;
    AudioSource playerAudio;
    bool isDead;

    public InputDevice controller;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        controller = InputManager.ActiveDevice;
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

        anim.SetTrigger("Die");

        playerAudio.clip = deathSound;
        playerAudio.Play();
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

}
