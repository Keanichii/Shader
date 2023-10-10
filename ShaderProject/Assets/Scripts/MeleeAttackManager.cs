using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    //this script takes care of animation and input

    //downward and horizontal force
    public float defaultForce = 300;
    //upwards force
    public float upwardsForce = 600;
    
    public float movementTime = .1f;
    //Input detection
    public bool meleeAttack = false;


    //The animator on the meleePrefab
    private Animator meleeAnimator;

    
    //The Animator component on the player
    private Animator anim;
    //The Character script on the player; this script on my project manages the grounded state, so if you have a different script for that reference that script
    private PlayerMovement player;

    
    private void Start()
    {
        //The Animator component on the player
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerMovement>();
        //The animator on the meleePrefab
        meleeAnimator = GetComponentInChildren<MeleeWeapon>().gameObject.GetComponent<Animator>();
    }
    


    public void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        //Checks to see if Backspace key is pressed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //Sets the meleeAttack bool to true

            meleeAttack = true;
        }
        else
        {
            //Turns off the meleeAttack bool
            meleeAttack = false;
        }
        //Checks to see if meleeAttack is true and pressing up
        if (meleeAttack && Input.GetAxis("Vertical") > 0)
        {
            //Turns on the animation for the player to perform an upward melee attack
            anim.SetTrigger("UpwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack upwards
            meleeAnimator.SetTrigger("UpwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and pressing down while also not grounded
        if (meleeAttack && Input.GetAxis("Vertical") < 0 && !player.isGrounded)
        {
            //Turns on the animation for the player to perform a downward melee attack
            anim.SetTrigger("DownwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack downwards
            meleeAnimator.SetTrigger("DownwardMeleeSwipe");
        }
        //Checks to see if meleeAttack is true and not pressing any direction
        if ((meleeAttack && Input.GetAxis("Vertical") == 0)
            //OR if melee attack is true and pressing down while grounded
            || meleeAttack && (Input.GetAxis("Vertical") < 0 && player.isGrounded))
        {
            //Turns on the animation for the player to perform a forward melee attack
            anim.SetTrigger("ForwardMelee");
            //Turns on the animation on the melee weapon to show the swipe area for the melee attack forwards
            meleeAnimator.SetTrigger("ForwardMeleeSwipe");
        }
    }
}
