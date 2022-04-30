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
    private Animator stoiAnimator;
    public CinemachineVirtualCamera mainCamera;

    [Header("Environment")]
    public float footOffset;
    public float bodyOffset;
    public float groundDistance;
    public LayerMask groundLayer;

    [Header("Movement")]
    public float speed;

    [Header("Jump")]
    public float jumpForce;
    public float jumpHoldForce;
    public float jumpHoldDuration;
    private float jumpTime;

    public float secondJumpForce;

    [Header("Look")]
    public float lookBufferTime;
    private float lookTime;

    [Header("State")]
    public bool isGrounded;
    public bool isLooking;
   
    //Button Detectors
    private bool jumpPressed;
    private bool jumpHeld;

    // Start is called before the first frame update
    private void Start()
    {
        stoiRigidbody = GetComponent<Rigidbody2D>();
        stoiAnimator = GetComponent<Animator>();
        jumpPressed = false;
        lookTime = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        jumpPressed = Input.GetButtonDown("Jump");
        jumpHeld = Input.GetButton("Jump");

        Jump();

        SwitchAnim();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();

        Movement();
    }

    void PhysicsCheck()
    {

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
        if (jumpPressed && isGrounded) // First Jump
        {
            jumpTime = Time.time + jumpHoldDuration;
            stoiRigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            jumpPressed = false;
        }
        if (jumpHeld && !isGrounded && Time.time < jumpTime) // Still Jumping
        {
            stoiRigidbody.AddForce(new Vector2(0f, jumpHoldForce), ForceMode2D.Force);
        }
        if (jumpPressed && !isGrounded) // Second jump
        {
            stoiRigidbody.AddForce(new Vector2(0f, secondJumpForce), ForceMode2D.Impulse);
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
                lookTime += Time.deltaTime;
            }
            else if (!isLooking)
            {
                if (verticalScale > 0)
                {
                    stoiAnimator.SetInteger("Look", 1);
                }
                else
                {
                    stoiAnimator.SetInteger("Look", -1);
                }
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

    RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float length, LayerMask Layer)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, length, Layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * length, color);
        return hit;
    }
}
