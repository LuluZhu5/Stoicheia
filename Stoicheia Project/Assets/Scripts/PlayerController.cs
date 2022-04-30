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
    
    private Rigidbody2D stoiRigidbody;
    private CapsuleCollider2D stoiCollider;
    private BoxCollider2D stoiFeetCollider;
    private Animator stoiAnimator;

    public float speed, jumpForce;
    public LayerMask ground;

    private bool isGrounded;

    private bool jumpPressed;
    private int jumpCount;
    public int maxJumpCount;

    public float lookBufferTime;
    private float lookTime;
    private bool isLooking;

    // Start is called before the first frame update
    private void Start()
    {
        stoiRigidbody = GetComponent<Rigidbody2D>();
        stoiCollider = GetComponent<CapsuleCollider2D>();
        stoiFeetCollider = GetComponent <BoxCollider2D>();
        stoiAnimator = GetComponent<Animator>();
        jumpPressed = false;
        lookTime = 0;
        isLooking = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        Jump();

        SwitchAnim();

        Look();
    }

    private void FixedUpdate()
    {
        isGrounded = stoiFeetCollider.IsTouchingLayers(ground);

        Movement();
    }

    void Movement()
    {
        float horizontalScale = Input.GetAxisRaw("Horizontal");
        stoiRigidbody.velocity = new Vector2(horizontalScale * speed, stoiRigidbody.velocity.y);

        if (horizontalScale != 0)
        {
            transform.localScale = new Vector3(-horizontalScale, 1, 1);
        }
    }

    void Jump ()
    {
        if (isGrounded)
        {
            jumpCount = maxJumpCount - 1;
        }
        if (jumpPressed && isGrounded) // First Jump (from ground)
        {
            stoiRigidbody.velocity = new Vector2(stoiRigidbody.velocity.x, jumpForce);
            --jumpCount;
            jumpPressed = false;
        }
        else if (jumpPressed && jumpCount > 0 && !isGrounded) // Additional Jump (from Air)
        {
            stoiRigidbody.velocity = new Vector2(stoiRigidbody.velocity.x, jumpForce);
            --jumpCount;
            jumpPressed = false;
        }
    }

    void SwitchAnim()
    {
        stoiAnimator.SetFloat("Run", Mathf.Abs(stoiRigidbody.velocity.x));

        if (isGrounded)
        {
            stoiAnimator.SetBool("Fall", false);
            stoiAnimator.SetBool("Jump", false);
        }
        else if (!isGrounded && stoiRigidbody.velocity.y > 0)
        {
            stoiAnimator.SetBool("Jump", true);
        }
        else if (stoiRigidbody.velocity.y < 0)
        {
            stoiAnimator.SetBool("Jump", false);
            stoiAnimator.SetBool("Fall", true);
        }
    }

    void Look()
    {
        float verticalScale = Input.GetAxisRaw("Vertical");
        if (verticalScale != 0 && isGrounded && Mathf.Abs(stoiRigidbody.velocity.x) < Mathf.Epsilon)
        {
            if (lookTime < lookBufferTime)
            {
                lookTime += Time.fixedDeltaTime;
            }
            else if (!isLooking)
            {
                int dir = verticalScale > 0 ? 1 : -1;
                stoiAnimator.SetInteger("Look", dir);
                isLooking = true;
            }
        }
        else
        {
            lookTime = 0;
            stoiAnimator.SetInteger("Look", 0);
            isLooking = false;
        }
    }
}
