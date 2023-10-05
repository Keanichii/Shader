using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    #region VARIABLES 
    Rigidbody2D rb;

    [Header("Movement")]
    Vector2 moveInput;
    public float moveSpeed = 2;
    public float acceleration;
    public float decceleration;
    public float velPower = 3;
    public float frictionAmount;
    public float slideSpeed;
    public float slideAccel;

    [Header("Ground & Wall Checks")]
    [SerializeField] Transform groundCheckPoint;
    [Space(10)]
    [SerializeField] Transform frontWallCheckPoint;
    [SerializeField] Transform backWallCheckPoint;

    [Header("Jump")]
    public float jumpForce;
    [Space(10)]

    float lastOnWallTimeRight;
    float lastOnWallTimeLeft;
    public float lastTimeOnWall;
    public float lastOnGroundTime;
    public float lastJumpTime;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    public float jumpHangTime;
    [Space(10)]

    bool isFacingRight;
    bool isJumping = false;
    [SerializeField] bool isJumpCut;
    bool isFalling;
    bool isWallJumping;

    [Header("Gravity")]
    float gravityScale = 1;
    public float gravityMultiplier;
    public float gravityJumpCutMultiplier;
    public float gravityHangMultiplier;
    public float maxFallSpeed;

    [Header("Layers & Tags")]
    [SerializeField] LayerMask groundLayer;



    #endregion



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetGravityScale(rb.gravityScale);
        isFacingRight = false;

    }

    private void Update()
    {

        //Coyote Timer
        lastOnGroundTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;

        #region CHECKS
        //Ground Check
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 0.1f, groundLayer))
        {
            lastOnGroundTime = jumpCoyoteTime;
            Debug.Log("on ground");
        }
        if (((Physics2D.Raycast(frontWallCheckPoint.position, Vector2.right, 0.1f, groundLayer) && isFacingRight)
                    || (Physics2D.Raycast(backWallCheckPoint.position, Vector2.left, 0.1f, groundLayer) && !isFacingRight)) && !isWallJumping)
            lastOnWallTimeRight = jumpCoyoteTime;

        //Right Wall Check
        if (((Physics2D.Raycast(frontWallCheckPoint.position, Vector2.right, 0.1f, groundLayer) && !isFacingRight)
            || (Physics2D.Raycast(backWallCheckPoint.position, Vector2.left, 0.1f, groundLayer) && isFacingRight)) && !isWallJumping)
            lastOnWallTimeLeft = jumpCoyoteTime;

        //Two checks needed for both left and right walls since whenever the play turns the wall checkPoints swap sides
        lastTimeOnWall = Mathf.Max(lastOnWallTimeLeft, lastOnWallTimeRight);



        //Jump Checks
        if (isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
            isFalling = true;
        }
        if (lastOnGroundTime > 0 && !isJumping && !isWallJumping)
        {
            isJumpCut = false;
        }
        #endregion


        //Jumps
        if (CanJump() && lastJumpTime > 0)
        {
            isJumping = true;
            isJumpCut = false;
            Jump();
        }




        #region INPUTS
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJump();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUp();
        }

        //Input Handler
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        //Check Direction to Face
        if (moveInput.x != 0)
            CheckDirectionToFace(moveInput.x > 0);
        #endregion


        #region GRAVITY

        if (rb.velocity.y < 0f)
        {
            SetGravityScale(gravityScale * gravityMultiplier);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        } 
        else if (isJumpCut)
        {
            SetGravityScale(rb.gravityScale * gravityJumpCutMultiplier);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else if (isJumping && Mathf.Abs(rb.velocity.y) < jumpHangTime)
        {
            SetGravityScale(gravityScale * gravityHangMultiplier);
        }
        else
        {
            SetGravityScale(gravityScale);
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
        if (CanJumpCut())
        isJumpCut = true;
    }

    //Wall Jump
    public void WallJump()
    {

    }


    #endregion

    public void SetGravityScale(float scale)
    {
        rb.gravityScale = scale;
    }

    //Checking Methods
    private bool CanJump()
    {
        return lastOnGroundTime > 0 && !isJumping;
    }

    private bool CanJumpCut()
    {
        return rb.velocity.y > 0 && isJumping;
    }

    private bool CanWallJump()
    {
        return isWallJumping && rb.velocity.y > 0;
    }


    private void Slide()
    {
        float speedDif = slideSpeed - rb.velocity.y;
        float movement = speedDif * slideAccel;

        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        rb.AddForce(movement * Vector2.up);
    }



    public bool CanSlide()
    {
        if (lastTimeOnWall > 0 && !isJumping && !isWallJumping && lastOnGroundTime <= 0)
            return true;
        else
            return false;
    }






    #region GIZMOS
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position - new Vector3(0, 0.1f, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(backWallCheckPoint.position, backWallCheckPoint.position - new Vector3(0.1f, 0, 0));
        Gizmos.DrawLine(frontWallCheckPoint.position, frontWallCheckPoint.position + new Vector3(0.1f, 0, 0));
    }


    #endregion


}
