using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : MonoBehaviour
{
    // Inspector Variables
    [SerializeField] float jumpForce = 400f;
    [Range(0, 1)] [SerializeField] float crouchSpeed = 0.36f;
    [Range(0, 0.3f)] [SerializeField] float movementSmoothing = 0.05f;
    [SerializeField] bool allowAirControl = false;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform ceilingCheck;
    [SerializeField] Collider2D crouchDisableCollider;

    // Private Variables
    const float groundedRadius = 0.5f;
    const float ceilingRadius = 0.2f;
    bool isGrounded;
    bool isCrouching = false;
    bool isFacingRight = true;
    Rigidbody2D rb;
    Vector3 velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent onLandEvent = new UnityEvent();
    public BoolEvent onCrouchEvent = new BoolEvent();

    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded)
                {
                    onLandEvent.Invoke();
                }
            }
        }        
    }

    public void Move(float move, bool crouch, bool jump)
    {
        crouch = CheckCrouching(crouch);

        if (isGrounded || allowAirControl)
        {
            CrouchLogic(ref move, crouch);

            VelocityMove(move);

            SpriteDirection(move);
        }

        Jump(jump);
    }

    private void CrouchLogic(ref float move, bool crouch)
    {
        if (crouch)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                onCrouchEvent.Invoke(true);
            }

            move *= crouchSpeed;

            if (crouchDisableCollider != null)
                crouchDisableCollider.enabled = false;
        }
        else
        {
            if (crouchDisableCollider != null)
                crouchDisableCollider.enabled = true;

            if (isCrouching)
            {
                isCrouching = false;
                onCrouchEvent.Invoke(false);
            }
        }
    }

    private void VelocityMove(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
    }

    private void SpriteDirection(float move)
    {
        if (move > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (move < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Jump(bool jump)
    {
        if (isGrounded && jump)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private bool CheckCrouching(bool crouch)
    {
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, groundLayer))
            {
                crouch = true;
            }
        }

        return crouch;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        SpriteUtilities.Flip(transform);
    }
}
