using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
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

    // Start is called before the first frame update
    private void Awake()
    {
        stoiRigidbody = GetComponent<Rigidbody2D>();
        stoiCollider = GetComponent<CapsuleCollider2D>();
        stoiFeetCollider = GetComponent <BoxCollider2D>();
        stoiAnimator = GetComponent<Animator>();
        jumpPressed = false;
        lookTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            jumpPressed = true;
        }

        Jump();

        SwitchAnim();

        
    }

    private void FixedUpdate()
    {
        Debug.Log(lookTime);

        isGrounded = stoiFeetCollider.IsTouchingLayers(ground);

        Movement();

        if (isGrounded && Mathf.Abs(stoiRigidbody.velocity.x) < Mathf.Epsilon)
        {
            float verticalScale = Input.GetAxisRaw("Vertical");
            if (verticalScale == 0)
            {
                lookTime = 0;
                stoiAnimator.SetInteger("Look", 0);
            }
            else
            {
                if (lookTime < lookBufferTime)
                {
                    lookTime += Time.deltaTime;
                }
                else
                {
                    stoiAnimator.SetInteger("Look", verticalScale > 0 ? 1 : -1);
                }
            }
        }
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
        
    }
}
