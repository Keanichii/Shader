using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region VARIABLES 
    Rigidbody2D rb;

    Vector2 moveInput;
    public float moveSpeed = 2;
    public float acceleration = 0.11f;
    public float decceleration = -0.11f;
    public float velPower = 3;
    public float frictionAmount;

    [Header("Ground & Wall Checks")]
    [SerializeField] Transform groundCheckPoint;
    [Space(10)]
    [SerializeField] Transform frontWallCheckPoint;
    [SerializeField] Transform backWallCheckPoint;

    [Header("Jump")]
    public float lastOnGroundTime;
    public float jumpForce;
    [Range(0f, 1f)] public float jumpCutMultiplier;
    public float lastJumpTime;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    [Space(10)]

    bool isFacingRight;
    bool isJumping = false;
    [SerializeField] bool isJumpCut;


    [Header("Gravity")]
    float gravityScale = 1;
    public float gravityMultiplier;
    public float maxFallSpeed;

    [Header("Layers & Tags")]
    [SerializeField] LayerMask groundLayer;



    #endregion



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
            isJumping = true;
            isJumpCut = false;
            Jump();
        }
            

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }

        #region GRAVITY

        if (rb.velocity.y < 0f)
        {
            SetGravityScale(rb.gravityScale * gravityMultiplier);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        } 
        else if (isJumpCut)
        {

            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else
        {
            SetGravityScale(rb.gravityScale);
        }
        #endregion

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

    #region TURN
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
    #endregion


    #region JUMP
    public void Jump()
    {
        lastJumpTime = 0;
        lastOnGroundTime = 0;
        
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }

    public void OnJump()
    {
        lastJumpTime = jumpBufferTime;
        
    }

    public void OnJumpUp()
    {
        
        isJumpCut = true;
        lastJumpTime = 0;
    }
    #endregion

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }



}
