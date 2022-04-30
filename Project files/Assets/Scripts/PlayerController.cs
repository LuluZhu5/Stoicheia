using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    //private PlayerController() { }
    //public static PlayerController instance { get { return Nested.instance} }

    //class Nested
    //{ 
    //    static Nested() { }
    //    internal static readonly PlayerController instance = new PlayerController();
    //}
    
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Environment")]
    public float footOffset;
    public float groundDistance;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float speed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpTime;
    public float doubleJumpForce;
    public float doubleJumpTime;
    private float jumpTimeCounter;

    public int jumpMaxCount = 2;
    private int jumpCount;

    [Header("Look")]
    public float lookBufferTime;
    private float lookTime;

    [Header("State")]
    public bool isGrounded;
    public bool isJumping;
    public bool isLooking;

    //Button Detectors
    private float horizontalInput;
    private float verticalInput;
    private bool jumpPressed;
    private bool jumpHeld;
    private bool jumpReleased;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");
        jumpReleased = Input.GetButtonUp("Jump");

        Jump();

        DoubleJump();

        SwitchAnim();

        Look();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        Movement();
    }

    void PhysicsCheck()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        RaycastHit2D rightCheck = Raycast(new Vector2(footOffset, 0f), Vector2.down, groundDistance, groundLayer);
        isGrounded = leftCheck || rightCheck;
    }

    void Movement()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(horizontalInput, 1, 1);
        }
    }

    void Jump ()
    {
        if (isGrounded)
        {
            jumpCount = jumpMaxCount - 1;
        }

        if (isGrounded && jumpPressed && jumpCount > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
            --jumpCount;
            jumpPressed = false;
        }

        if (jumpHeld && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (jumpReleased)
        {
            isJumping = false;
        }
    }

    void DoubleJump()
    {
        if (!isGrounded && jumpPressed && !isJumping && jumpCount > 0)
        {
            isJumping = true;
            jumpTimeCounter = doubleJumpTime;
            rb.velocity = Vector2.up * doubleJumpForce;
            --jumpCount;
            jumpPressed = false;
        }
    }

    void SwitchAnim()
    {
        anim.SetFloat("Run", Mathf.Abs(rb.velocity.x));

        if (isGrounded)
        {
            anim.SetBool("Fall", false);
            anim.SetBool("Jump", false);
        }
        else if (!isGrounded && rb.velocity.y > 0.1f)
        {
            anim.SetBool("Jump", true);
        }
        else if (rb.velocity.y < -0.1f)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Fall", true);
        }
    }

    void Look()
    {
        if (verticalInput != 0 && isGrounded && Mathf.Abs(rb.velocity.x) < 0.1f)
        {
            if (lookTime < lookBufferTime)
            {
                lookTime += Time.deltaTime;
            }
            else if (!isLooking)
            {
                if (verticalInput > 0)
                {
                    anim.SetInteger("Look", 1);
                }
                else
                {
                    anim.SetInteger("Look", -1);
                }
                isLooking = true;
            }
        }
        else
        {
            lookTime = 0;
            anim.SetInteger("Look", 0);
            isLooking = false;
        }
    }

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask Layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, Layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
