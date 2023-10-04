using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    Rigidbody2D rb;

    Vector2 moveInput;
    public float moveSpeed = 2;
    public float acceleration = 0.11f;
    public float decceleration = -0.11f;
    public float velPower = 3;
    public bool isFacingRight;
    public float frictionAmount;

    public float lastOnGroundTime;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = false;

    }

    private void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");

        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);

        //Check Ground
        lastOnGroundTime -= Time.deltaTime;

        /*if (Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer))
            lastOnGroundTime = 0.1f;*/


    }





    void FixedUpdate()
    {

        //direction
        float targetSpeed = moveInput.x * moveSpeed;

        //delta speed
        float delSpeed = targetSpeed - rb.velocity.x;

        //return acceleration or decceleration value
        float accelRate = (Mathf.Abs(targetSpeed) > 0) ? acceleration : decceleration;

        //multiply delta speed by accelRate to control applied force
        //raise that to a power to make it more responsive
        //multiply that by sign of delta speed to preserve direction
        float movement = Mathf.Pow(Mathf.Abs(delSpeed) * accelRate, velPower) * Mathf.Sign(delSpeed);

        //applies force to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force); 

        //check if player isn't accelerating
        if (Mathf.Abs(moveInput.x) < 0.01f)
        {
            //use the smaller value between current velocity and friction amount
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            
            //sets to movement direction
            amount *= Mathf.Sign(rb.velocity.x);

            //applies an equal but opposite force against movement direction
            rb.AddForce(-amount * Vector2.right, ForceMode2D.Impulse);
        }

    }

    public void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        isFacingRight = !isFacingRight;
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != isFacingRight)
            Turn();
    }

}
