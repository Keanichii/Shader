using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    
    //Determines if this GameObject should receive damage or not
    [SerializeField]
    private bool damageable = true;

    //Help prevent the same melee attack dealing damage multiple times
    [SerializeField]
    private float invulnerabilityTime = .2f;

    //Allows the player to be forced up when performing a downward strike above the enemy
    public bool giveUpwardForce = true;

    //Shock Wave Prefab Plug
    public GameObject shockWave;

    [SerializeField]
    private GameObject sparkObject;
    private ParticleSystem spark;

    //Bool that manages if the enemy can receive more damage
    private bool invulnerable;

    [SerializeField]
    private int currentHealth;

    [SerializeField]
    private int healthAmount = 100;

    //Coroutine that runs to allow the enemy to receive damage again
    private Coroutine turnOffInvulnerableCoroutine;

    private SwapMaterial swapMaterial;

    private void Start()
    {
        //Sets the enemy to the max amount of health when the scene loads
        currentHealth = healthAmount;
        swapMaterial = GetComponent<SwapMaterial>();
        spark = sparkObject.GetComponent<ParticleSystem>();
    }

    public void Damage(int amount)
    {
        //First checks to see if the player is currently in an invulnerable state; if not it runs the following logic.
        if (damageable && !invulnerable && currentHealth > 0)
        {
            //turn on bool so enemy is invulnerable 
            invulnerable = true;
            currentHealth -= amount;
            Instantiate(shockWave, transform.position, Quaternion.identity);
            spark.Play();
            swapMaterial.SwapMaterials(1);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                gameObject.SetActive(false);
            }
            else
            {
                turnOffInvulnerableCoroutine = StartCoroutine(TurnOffInvulnerable());
            }
        }
    }

    //Coroutine that runs to allow the enemy to receive damage again
    private IEnumerator TurnOffInvulnerable()
    {
        //Wait in the amount of invulnerabilityTime, which by default is .2 seconds
        yield return new WaitForSeconds(invulnerabilityTime);
        //Turn off the hit bool so the enemy can receive damage again
        invulnerable = false;
        swapMaterial.SwapMaterials(0);
    }
}
