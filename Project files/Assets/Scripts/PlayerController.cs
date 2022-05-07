using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    //private PlayerMovement() { }
    //public static PlayerMovement instance { get { return Nested.instance} }

    //class Nested
    //{ 
    //    static Nested() { }
    //    internal static readonly PlayerMovement instance = new PlayerMovement();
    //}
    
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [Header("Environment")]
    public float GroundCheckDistance;
    public float CeilingCheckDistance;
    public LayerMask groundLayer;

    [Header("Movement")]
    [Range(0, 30)] public float speed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpTime;
    public float doubleJumpForce;
    public float doubleJumpTime;
    public float crouchJumpForce;
    public float crouchJumpTime;
    private float jumpTimeCounter;

    public int jumpMaxCount = 2;
    private int jumpCount;

    [Header("Jump Optimization")]
    [Range(1, 20)] public float highFallMutiplier;
    [Range(1, 20)] public float lowFallMutiplier;

    [Header("Look")]
    public float lookBufferTime;
    //private float lookTime;

    [Header("Crouch")]
    public float crouchSpeedDivisor;

    [Header("State")]
    public bool isGrounded;
    public bool isCeiling;
    public bool isJumping;
    public bool isLooking;
    public bool isCrouching;

    //Button Detectors
    private float horizontalInput;
    private float verticalInput;
    private bool jumpPressed;
    private bool jumpHeld;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
}

    // Update is called once per frame
    private void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");

        verticalInput = Input.GetAxisRaw("Vertical");

        Jump();

        DoubleJump();

        JumpAdjustment();

        Crouch();

        //SwitchAnim();

        //Look();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        
        horizontalInput = Input.GetAxis("Horizontal");

        Movement();
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftGroundCheck = Raycast(new Vector2(-coll.size.x / 2, 0f), Vector2.down, GroundCheckDistance, groundLayer);
        RaycastHit2D rightGroundCheck = Raycast(new Vector2(coll.size.x / 2, 0f), Vector2.down, GroundCheckDistance, groundLayer);
        isGrounded = leftGroundCheck || rightGroundCheck;

        RaycastHit2D leftCeilingCheck = Raycast(new Vector2(-coll.size.x / 2, coll.size.y), Vector2.up, CeilingCheckDistance, groundLayer);
        RaycastHit2D rightCeilingCheck = Raycast(new Vector2(coll.size.x / 2, coll.size.y), Vector2.up, CeilingCheckDistance, groundLayer);
        isCeiling = leftCeilingCheck || rightCeilingCheck;
    }

    void Movement()
    {
        if (isCrouching)
        {
            horizontalInput /= crouchSpeedDivisor;
        }

        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput > 0 ? 1 : -1, 1, 1);
        }
    }

    void Jump ()
    {
        float force = isCrouching ? crouchJumpForce : jumpForce;

        if (isGrounded)
        {
            jumpCount = jumpMaxCount - 1;
        }

        if (isGrounded && jumpPressed && jumpCount > 0)
        {
            isJumping = true;
            jumpTimeCounter = isCrouching ? crouchJumpTime : jumpTime;
            rb.velocity = Vector2.up * force;
            --jumpCount;
            jumpPressed = false;
        }

        if (jumpHeld && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * force;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (!jumpHeld)
        {
            isJumping = false;
        }
    }

    void DoubleJump()
    {
        if (!isGrounded && jumpPressed && !isJumping && !isCrouching && jumpCount > 0)
        {
            isJumping = true;
            jumpTimeCounter = doubleJumpTime;
            rb.velocity = Vector2.up * doubleJumpForce;
            --jumpCount;
            jumpPressed = false;
        }
    }

    void JumpAdjustment()
    {
        if (rb.velocity.y < -0.1f)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (highFallMutiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0.1f && !jumpHeld)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowFallMutiplier - 1) * Time.deltaTime;
        }
    }

    void Crouch()
    {
        if (isGrounded && verticalInput < 0)
        {
            isCrouching = true;
        }
        else if (isGrounded && verticalInput == 0)
        {
            isCrouching = isCeiling;
        }
    }

    //void Look()
    //{
    //    if (verticalInput != 0 && isGrounded && Mathf.Abs(rb.velocity.x) < Mathf.Epsilon)
    //    {
    //        if (lookTime < lookBufferTime)
    //        {
    //            lookTime += Time.deltaTime;
    //        }
    //        else if (!isLooking)
    //        {
    //            if (verticalInput > 0)
    //            {
    //                anim.SetInteger("Look", 1);
    //            }
    //            else
    //            {
    //                anim.SetInteger("Look", -1);
    //            }
    //            isLooking = true;
    //        }
    //    }
    //    else
    //    {
    //        lookTime = 0;
    //        anim.SetInteger("Look", 0);
    //        isLooking = false;
    //    }
    //}

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask Layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, Layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
