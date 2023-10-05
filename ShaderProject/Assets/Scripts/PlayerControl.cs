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
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundLayer;

    public float jumpForce;
    [Range(0f, 1f)] public float jumpCutMultiplier;
    public float lastJumpTime;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    bool isJumping = false;
    [SerializeField]bool jumpInputReleased;

    float gravityScale = 1;
    public float gravityMultiplier;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = false;

    }

    private void Update()
    {
        //Input Handler
        moveInput.x = Input.GetAxis("Horizontal");
        
        //Check Direction to Face
        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);

        //Coyote Timer
        lastOnGroundTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime; 


        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.1f, groundLayer))
        {
            lastOnGroundTime = jumpCoyoteTime;
            isJumping = false;
            Debug.Log("on ground");
        }

        if (lastOnGroundTime > 0 && lastJumpTime > 0 && !isJumping)
        {
            Jump();
        }
            

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUp();
        }
        

        if (rb.velocity.y < 0f)
        {
            rb.gravityScale = gravityScale * gravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }


    }





    void FixedUpdate()
    {
        Run();
         
        //friction
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


    public void Run()
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


    public void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        lastJumpTime = 0;
        lastOnGroundTime = 0;
        isJumping = true;
        jumpInputReleased = false;
    }

    public void OnJump()
    {
        lastJumpTime = jumpBufferTime;
        
    }

    public void OnJumpUp()
    {
        
        rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        

        jumpInputReleased = true;
        lastJumpTime = 0;
    }


}
