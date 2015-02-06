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

    public Material[] colors = new Material[4];

    public int maxHealth = 100;
    public int currentHealth;
    
    public Slider healthSlider;
    public AudioClip deathSound;
    public AudioClip hitSound;

    public int[] ammunition = {0, 0, 0, 0};

    Animator anim;
    AudioSource playerAudio;
    bool isDead;

    public PlayerColor color;
    public InputDevice controller;

    public Transform AttachPoint;
    public GameObject BloodParticlePrefab;

    Projector circle;

    void Awake()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerAudio.clip = hitSound;
        currentHealth = maxHealth;
    }

    void Start()
    {
        circle = gameObject.GetComponentInChildren<Projector>();
        circle.material = colors[(int)color];
        controller = InputManager.Devices[(int)color];
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        //healthSlider.value = currentHealth;

        playerAudio.Play();

        // Instantiate blood
        if (BloodParticlePrefab != null)
        {
            var blood = (GameObject)Instantiate(BloodParticlePrefab);
            blood.transform.position = transform.position + new Vector3(0, 1f, 0);
        }

        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;

        //anim.SetTrigger("Die");

        playerAudio.clip = deathSound;
        playerAudio.Play();

        controller.Vibrate(0, 0);

        Destroy(gameObject,0.5f);
    }

    public int getCurrentHealth()
    {
        return currentHealth;
    }

}
