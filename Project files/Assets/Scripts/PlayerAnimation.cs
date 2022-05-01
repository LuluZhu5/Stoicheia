using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private PlayerController controller;
    private Rigidbody2D rb;

    int speedID;
    int groundID;
    int crouchID;
    int verticalID;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        speedID = Animator.StringToHash("speed");
        groundID = Animator.StringToHash("isGrounded");
        crouchID = Animator.StringToHash("isCrouching");
        verticalID = Animator.StringToHash("verticalInput");
    }

    private void Update()
    {
        anim.SetFloat(speedID, Mathf.Abs(rb.velocity.x));
        anim.SetBool(groundID, controller.isGrounded);
        anim.SetBool(crouchID, controller.isCrouching);
        anim.SetFloat(verticalID, -Input.GetAxis("Vertical"));
    }
}
