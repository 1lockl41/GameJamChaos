using System;
using UnityEngine;


[RequireComponent(typeof(Platformer2DUserControl))]
public class PlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    public Rigidbody2D m_Rigidbody2D;
    public bool m_FacingRight = true;  // For determining which way the player is currently facing.

    public int m_jumpLimit = 2;
    public int m_numberOfJumps = 0;
    public float m_timeBetweenJump = 0.25f;
    public float m_timeSinceLastJump = 0.0f;

    public float m_dodgeForce = 30.0f;

    Platformer2DUserControl m_controller;

    float m_gravityScaleDefault;

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_gravityScaleDefault = m_Rigidbody2D.gravityScale;

        m_controller = GetComponent<Platformer2DUserControl>();
    }


    private void FixedUpdate()
    {
        m_timeSinceLastJump += Time.deltaTime;

        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        // Set the vertical animation
        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }


    public void Move(float move, bool crouch, bool jump, float jumpPower)
    {
        if(jump && m_timeSinceLastJump < m_timeBetweenJump)
        {
            jump = false;
        }

        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool("Crouch"))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if ((m_Grounded || m_AirControl) && !m_controller.m_isDodging && !m_controller.m_isShielding && m_controller.m_canMove)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move*m_CrouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            m_Rigidbody2D.velocity = new Vector2(move * (m_MaxSpeed / m_controller.speedScale) , m_Rigidbody2D.velocity.y);
  

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
                // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // Jumping while in the air, always use max jump power
        if(!m_Grounded && jump && (m_numberOfJumps < m_jumpLimit))
        {
            //Debug.Log("air jump");
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_controller.m_jumpPowerMax), ForceMode2D.Impulse);
            m_timeSinceLastJump = 0.0f;
            m_controller.ResetJumpPower();
            m_numberOfJumps++;
        }

        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            // Add a vertical force to the player.
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
            m_Rigidbody2D.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
            m_timeSinceLastJump = 0.0f;
            m_controller.ResetJumpPower();
            m_numberOfJumps++;
        }

        if(m_Grounded && !jump && m_Anim.GetBool("Ground"))
        {
            if(m_Rigidbody2D.velocity.y < 0.5f)
            {
                if (!m_controller.m_isChargingJump)
                {
                    m_controller.ResetJumpPower();
                }
                m_numberOfJumps = 0;
            }
        }
    }

    public void StopMovement()
    {
        m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
        m_Anim.SetFloat("Speed", 0);          
    }

    public void StopMovementHorizontal()
    {
        m_Rigidbody2D.velocity = new Vector2(0.0f, m_Rigidbody2D.velocity.y);
        m_Anim.SetFloat("Speed", 0);
    }

    public void StopMovementVertical()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
        m_Anim.SetFloat("Speed", 0);
    }


    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void DodgeLeft()
    {
        m_Rigidbody2D.AddForce(new Vector2(-m_dodgeForce, 0f), ForceMode2D.Impulse);
    }

    public void DodgeRight()
    {
        m_Rigidbody2D.AddForce(new Vector2(m_dodgeForce, 0f), ForceMode2D.Impulse);
    }

    public void StopGravityScale()
    {
        m_Rigidbody2D.gravityScale = 0.0f;
    }

    public void HalfGravityScale()
    {
        m_Rigidbody2D.gravityScale = 0.5f;
    }

    public void ResetGravityScale()
    {
        m_Rigidbody2D.gravityScale = m_gravityScaleDefault;
    }

    public void AddDownForce()
    {
        m_Rigidbody2D.AddForce(new Vector2(0.0f, -12.0f));
    }

    public void AddUpForce()
    {

        m_Rigidbody2D.AddForce(new Vector2(0.0f, 12.0f));
    }

    public bool CheckIfFalling()
    {
        if(m_Rigidbody2D.velocity.y < -0.1f)
        {
            return true;
        }
        return false;
    }

    public void PushAwayFromPoint(Vector2 pushPos, float forceMagnitude)
    {
        Vector2 direction = new Vector2(transform.position.x, transform.position.y) - pushPos;

        StopMovement();
        m_Rigidbody2D.AddForceAtPosition(direction.normalized * forceMagnitude, pushPos, ForceMode2D.Impulse);
    }

    public void UnfreezeRotation()
    {
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.identity;
        transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1.0f);
        //this.gameObject.transform.eulerAngles = Vector3.zero;
        m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void AddPunchUpForce(Vector2 pushPos, float force)
    {
        m_Rigidbody2D.AddForce(new Vector2(0.0f, force), ForceMode2D.Impulse);
    }

    public void SetGravityScale(float modifier)
    {
        m_Rigidbody2D.gravityScale = m_gravityScaleDefault * modifier;
    }
}

