using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerAttrs : MonoBehaviour
{
    public enum PlayerColor
    {
        RED,
        BLUE,
        GREEN,
        YELLOW
    };

    public int maxHealth = 100;
    public int currentHealth;
    
    public Slider healthSlider;
    public AudioClip deathSound;

    public int[] ammunition = {0, 0, 0, 0};

    Animator anim;
    AudioSource playerAudio;
    bool isDead;

    public PlayerColor color;
    public InputDevice controller;

    public Transform AttachPoint;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        controller = InputManager.Devices[(int) color];
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
