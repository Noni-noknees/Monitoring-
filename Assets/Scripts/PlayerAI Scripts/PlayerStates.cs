using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.Universal;

public class PlayerStates : MonoBehaviour
{
    public Transform player;
    public float health = 50f;
    public float maxHealth = 50f;
    public float hpPerSecond = 1f; //enhanced state health regeneration
    public float power = 3f;
    public float starveTime = 3 * 60f; // Time limit to starvation state if player hasn't collected any keys 
    public float enhanceDuration = 5 * 60f; // Time limit of enhanced state
    private float enhanceTime;
    private int collectedKeys = 0;
    private int numCollectedTreasures = 0;
    private float timeSinceLastKey = 0f; // Time since player has collected a key
    private PlayerState currentState;

    public enum PlayerState { Healthy, Starved, Enhanced };
    public bool Sprint = false;
    public bool DoubleJump = false;

    public float speed = 3.0f;
    public float jumpHeight = 1.0f;
    public float gravity = -9.81f;

    //private Volume postProcessingVolume;
   // private Vignette vignette;

    void Start()
    {
        currentState = PlayerState.Healthy;

        /*postProcessingVolume = FindObjectOfType<Volume>();
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out vignette);
        }*/
    }

    void Update()
    {
        // Track time since the last key 
        if (currentState != PlayerState.Starved)
        {
            timeSinceLastKey += Time.deltaTime;
        }

        switch (currentState)
        {
            case PlayerState.Healthy:
                Healthy();
                if (timeSinceLastKey >= starveTime)
                {
                    TransitionToStarved();
                }
                else if (collectedBoosters() >= 3)
                {
                    TransitionToEnhanced();
                }
                break;

            case PlayerState.Starved:
                Starved();
                if (collectedKeys >= 2)
                {
                    TransitionToHealthy();
                }
                break;

            case PlayerState.Enhanced:
                Enhanced();
                if (enhanceTime <= 0)
                {
                    TransitionToHealthy();
                }
                break;
        }
    }

    private void Healthy()
    {
        Sprint = true;
        //SetVignetteEffect(false); //no blur
        Debug.Log("Healthy state is running.");
    }

    private void Starved()
    {
        Sprint = false;
        DoubleJump = false;

        health -= Time.deltaTime * 2f;
        power -= Time.deltaTime * 1f;
        health = Mathf.Clamp(health, 0, maxHealth);
        Debug.Log("Starved state is running.");

        if (power <= 0)
        {
            power = 0;
        }

        if (health <= 0)
        {
            health = 0;
            Debug.Log("Game Over");
        }

   //     SetVignetteEffect(true); //set blur
    }

    private void Enhanced()
    {
        DoubleJump = true;
        Sprint = true;

        enhanceTime -= Time.deltaTime;
        health += hpPerSecond * Time.deltaTime;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health > maxHealth)
        {
            health = maxHealth;
        }
       // SetVignetteEffect(false); //no blur
        Debug.Log("Enhanced state is running.");
    }

    private void TransitionToStarved()
    {
        currentState = PlayerState.Starved;
        Debug.Log("Player has entered Starved state.");
    }

    private void TransitionToHealthy()
    {
        currentState = PlayerState.Healthy;
        timeSinceLastKey = 0f;
        Debug.Log("Player has entered Healthy state.");
    }

    private void TransitionToEnhanced()
    {
        currentState = PlayerState.Enhanced;
        enhanceTime = enhanceDuration;
        Debug.Log("Player has entered Enhanced state.");
    }

 /*   private void SetVignetteEffect(bool isActive)
    {
        if (vignette != null)
        {
            vignette.active = isActive;
        }
    }*/

    public void CollectKey()
    {
        collectedKeys++;
        timeSinceLastKey = 0f;
        Debug.Log("Key collected!");
    }

    public int collectedBoosters()
    {
        numCollectedTreasures++;
        return numCollectedTreasures;
    }
}

