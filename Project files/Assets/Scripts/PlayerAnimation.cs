using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerController controller;
    private Rigidbody2D rb;

    int xVelocityID;
    int groundID;
    int crouchID;
    int jumpID;
    int yInputID;
    int yVelocityID;
    int hitGroundID;

    public float hitGroundSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        xVelocityID = Animator.StringToHash("horizontalVelocity");
        yVelocityID = Animator.StringToHash("verticalVelocity");
        groundID = Animator.StringToHash("isGrounded");
        crouchID = Animator.StringToHash("isCrouching");
        jumpID = Animator.StringToHash("isJumping");
        yInputID = Animator.StringToHash("verticalInput");
        hitGroundID = Animator.StringToHash("isHitGround");
    }

    private void Update()
    {
        anim.SetBool(crouchID, controller.isCrouching);
        anim.SetBool(jumpID, controller.isJumping);
        anim.SetFloat(yInputID, controller.isCeiling ? 1 : -Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        IsHitGround();

        anim.SetBool(groundID, controller.isGrounded);
        anim.SetFloat(xVelocityID, Mathf.Abs(rb.velocity.x));
        anim.SetFloat(yVelocityID, rb.velocity.y);
    }

    private void IsHitGround()
    {
        if (-rb.velocity.y > hitGroundSpeed)
        {
            anim.SetBool(hitGroundID, true);
        }
    }

    void HitGround()
    {
        CinemachineShake.Instance.ShakeCamera();
        anim.SetBool(hitGroundID, false);
    }
}
