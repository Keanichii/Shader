using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    //How much damage the melee attack does
    [SerializeField]

    private int damageAmount = 20;
    private PlayerMovement player;
    private Rigidbody2D rb;
    private MeleeAttackManager meleeAttackManager;


    private Vector2 direction;
    private bool collided;
    private bool downwardStrike;
    private bool trigger;

    private void Start()
    {
        player = GetComponentInParent<PlayerMovement>();

        rb = GetComponentInParent<Rigidbody2D>();
        meleeAttackManager = GetComponentInParent<MeleeAttackManager>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            trigger = true;
        }
        //Move player in opposite direction
        HandleMovement();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Checks to see if the GameObject the MeleeWeapon is colliding with has an EnemyHealth script
        if (collision.GetComponent<Enemies>())
        {
                Debug.Log("In range");
                //Method that checks to see what force can be applied to the player when melee attacking
                HandleCollision(collision.GetComponent<Enemies>());
        }
        
    }

    private void HandleCollision(Enemies objHealth)
    {
        
            Debug.Log("Is attacking");
            //Checks to see if the GameObject allows for upward force and if the strike is downward as well as grounded
            if (objHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !player.isGrounded)
            {
                //Sets the direction variable to up
                direction = Vector2.up;
                //Sets downwardStrike to true
                downwardStrike = true;
                //Sets collided to true
                collided = true;
            }
            if (Input.GetAxis("Vertical") > 0 && !player.isGrounded)
            {
                //Sets the direction variable to up
                direction = Vector2.down;
                //Sets collided to true
                collided = true;
            }
            //Checks to see if the melee attack is a standard melee attack
            if ((Input.GetAxis("Vertical") <= 0 && player.isGrounded) || Input.GetAxis("Vertical") == 0)
            {
                //Checks to see if the player is facing left
                if (player.IsFacingRight)
                {
                    //Sets the direction variable to right
                    direction = Vector2.left;
                }
                else
                {
                    //Sets the direction variable to right left
                    direction = Vector2.right;
                }

                collided = true;
            }


            //Deals damage in the amount of damageAmount
            objHealth.Damage(damageAmount);


            //Coroutine that turns off all the bools related to melee attack collision and direction
            StartCoroutine(NoLongerColliding());
        
    
    }

    //Method that makes sure there should be movement from a melee attack and applies force in the appropriate direction based on the amount of force from the melee attack manager script
    private void HandleMovement()
    {
        //Checks to see if the GameObject should allow the player to move when melee attack colides
        if (collided)
        {
                //If the attack was in a downward direction
                if (downwardStrike)
                {
                    //Propels the player upwards by the amount of upwardsForce in the meleeAttackManager script
                    rb.AddForce(direction * meleeAttackManager.upwardsForce);
                }
                else
                {
                    //Propels the player backwards by the amount of horizontalForce in the meleeAttackManager script
                    rb.AddForce(direction * meleeAttackManager.defaultForce);
                }
        }
        
    }

    #region Coroutine to stop attack

    //Coroutine that turns off all the bools that allow movement from the HandleMovement method
    private IEnumerator NoLongerColliding()
    {
        //Waits in the amount of time setup by the meleeAttackManager script; this is by default .1 seconds
        yield return new WaitForSeconds(meleeAttackManager.movementTime);
        //Turns off the collided bool
        collided = false;
        //Turns off the downwardStrike bool
        downwardStrike = false;
        trigger = false;
    }

    #endregion
}

