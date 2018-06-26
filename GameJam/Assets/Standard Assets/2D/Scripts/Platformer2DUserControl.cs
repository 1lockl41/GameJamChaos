using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        bool m_JumpHeld;

        float m_jumpPower;
        public float m_jumpPowerMin = 10.0f;
        public float m_jumpPowerMax = 20.0f;
        public float m_JumpPowerGain = 70.0f;
        public bool m_isChargingJump = false;

        public bool m_isDodging = false;
        float m_dodgeStopTimer = 0.0f;
        float m_dodgeStopDuration = 0.1f;

        float m_timeBetweenDodge = 0.15f;
        float m_betweenDodgeTimer = 0.0f;
        bool m_canDodge = true;

        public bool m_isShielding = false;
        float m_timeBetweenShield = 0.3f;
        float m_betweenShieldTimer = 0.0f;
        bool m_canShield = true;

        bool m_isAxisInUse = false;

        bool m_canTakeDamage = true;
        float m_damageDelay = 0.2f;
        float m_damageDelayTimer = 0.0f;

        bool m_wasHitSmall = false;
        float m_hitSmallDelay = 0.25f;
        float m_hitSmallTimer = 0.0f;

        bool m_wasHitBig = false;
        float m_hitBigDelay = 0.5f;
        float m_hitBigTimer = 0.0f;

        bool m_wasPunchedUp = false;
        float m_minPunchUpTime = 0.5f;
        float m_maxPunchUpTime = 2.0f;
        float m_punchUpTimer = 0.0f;

        public bool m_canMove = true;

        public float currentHealthPercent = 0.0f;

        public GameObject shieldSprite;

        bool canAttack = true;
        bool isLightAttacking = false;
        float attackDurationLight = 0.1f;
        float attackTimerLight = 0.0f;
        public GameObject attackBoxLight;

        bool isHeavyAttacking = false;
        float attackDurationHeavy = 0.2f;
        float attackTimerHeavy = 0.0f;
        public GameObject attackBoxHeavy;

        float attackResetDuration = 0.1f;
        float attackResetTimer = 0.0f;



        public bool dontAllowMovement = false;

        public string horizontalAxisButton;
        public string verticalAxisButton;
        public string jumpButton;
        public string fireButton;
        public string fire2Button;
        public string fire3Button;

        private void Awake()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            m_Character = GetComponent<PlatformerCharacter2D>();
            m_jumpPower = m_jumpPowerMin;
        }

        private void Update()
        {
            if(dontAllowMovement)
            {
                m_canMove = false;
                canAttack = false;
            }

            if (m_wasPunchedUp)
            {
                m_punchUpTimer += Time.deltaTime;
                m_canMove = false;
                m_Character.UnfreezeRotation();

                m_Character.m_Rigidbody2D.AddTorque(CrossPlatformInputManager.GetAxis(horizontalAxisButton) * 1);

                if(m_punchUpTimer > m_minPunchUpTime)
                {
                    m_canShield = true;
                    if (m_Character.m_Grounded || m_isShielding || (m_punchUpTimer > m_maxPunchUpTime))
                    {
                        m_punchUpTimer = 0.0f;
                        m_wasPunchedUp = false;
                        m_wasHitSmall = false;
                        m_wasHitBig = false;
                        m_canMove = true;
                        m_Character.ResetRotation();
                    }
                }
                else
                {
                    m_canShield = false;
                }
            }
            else
            {
                if (m_wasHitSmall)
                {
                    m_canShield = false;
                    m_canMove = false;
                    m_hitSmallTimer += Time.deltaTime;
                    if (m_hitSmallTimer > m_hitSmallDelay)
                    {
                        m_wasHitSmall = false;
                        m_hitSmallTimer = 0.0f;
                        m_canMove = true;
                        m_canShield = true;
                    }
                }

                if (m_wasHitBig)
                {
                    m_canShield = false;
                    m_canMove = false;
                    m_hitBigTimer += Time.deltaTime;
                    if (m_hitBigTimer > m_hitBigDelay)
                    {
                        m_wasHitBig = false;
                        m_hitBigTimer = 0.0f;
                        m_canMove = true;
                        m_canShield = true;
                    }
                }
            }

            if(!m_canTakeDamage)
            {
                m_damageDelayTimer += Time.deltaTime;
                if(m_damageDelayTimer > m_damageDelay)
                {
                    m_damageDelayTimer = 0.0f;
                    m_canTakeDamage = true;
                }
            }

            if (!m_Character.m_Grounded && !m_isShielding)
            {
                if (CrossPlatformInputManager.GetAxisRaw(verticalAxisButton) < 0)
                {
                    m_Character.AddDownForce();
                }
                else if (CrossPlatformInputManager.GetAxisRaw(verticalAxisButton) > 0 && m_Character.CheckIfFalling())
                {
                    m_Character.AddUpForce();
                }
            }

            if (CrossPlatformInputManager.GetButton(jumpButton) && m_Character.m_Grounded)
            {
                m_isChargingJump = true;
            }
            else if(!m_Character.m_Grounded)
            {
                m_isChargingJump = false;
            }  
            
            if(CrossPlatformInputManager.GetButton(fire2Button) && m_canShield)
            {
                if (!m_isDodging)
                {
                    m_Character.HalfGravityScale();
                    m_Character.StopMovementHorizontal();
                }

                if(!m_isShielding)
                {
                    m_isAxisInUse = true;
                    m_Character.StopMovement();
                }

                m_isShielding = true;

                shieldSprite.SetActive(true);
            }
            else if(m_isShielding)
            {
                m_Character.ResetGravityScale();
                m_isShielding = false;
                shieldSprite.SetActive(false);
                m_canShield = false;
            }

            if(!m_canShield)
            {
                m_betweenShieldTimer += Time.deltaTime;
                if(m_betweenShieldTimer > m_timeBetweenShield)
                {
                    m_canShield = true;
                    m_betweenShieldTimer = 0.0f;
                }
            }

            if(m_isShielding && m_canDodge)
            {
                if(CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) != 0)
                {
                    if(!m_isAxisInUse)
                    {
                        if(CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) < 0)
                        {
                            m_Character.DodgeLeft();
                        }
                        else
                        {
                            m_Character.DodgeRight();
                        }

                        m_isDodging = true;
                        m_isAxisInUse = true;
                    }
                }
                if(CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) == 0)
                {
                    m_isAxisInUse = false;
                }

            }

            if(!m_canDodge)
            {
                m_betweenDodgeTimer += Time.deltaTime;
                if(m_betweenDodgeTimer > m_timeBetweenDodge)
                {
                    m_canDodge = true;
                    m_betweenDodgeTimer = 0.0f;
                }
            }

            if (m_isDodging)
            {
                m_Character.StopGravityScale();
                m_dodgeStopTimer += Time.deltaTime;
                if (m_dodgeStopTimer > m_dodgeStopDuration)
                {
                    m_isDodging = false;
                    m_Character.ResetGravityScale();
                    m_dodgeStopTimer = 0.0f;

                    m_canDodge = false;
                    m_betweenDodgeTimer = 0.0f;
                }

                shieldSprite.SetActive(false);
            }


            if ((!m_Jump && (m_Character.m_timeSinceLastJump > m_Character.m_timeBetweenJump) && (m_Character.m_numberOfJumps < m_Character.m_jumpLimit) && !m_isShielding && m_canMove))
            {
                // Read the jump input in Update so button presses aren't missed.
                if(m_isChargingJump)
                {
                    //Debug.Log("jump cur" + m_jumpPower + "max " + m_jumpPowerMax);
                    m_jumpPower += m_JumpPowerGain * Time.deltaTime;
                    if(m_jumpPower > m_jumpPowerMax)
                    {
                        //Debug.Log("jump");
                        m_jumpPower = m_jumpPowerMax;
                        m_Jump = true;
                        m_isChargingJump = false;
                    }

                    if (CrossPlatformInputManager.GetButtonUp(jumpButton))
                    {
                        m_Jump = true;
                        m_isChargingJump = false;
                    }
                }
                else
                {
                    if (CrossPlatformInputManager.GetButtonDown(jumpButton))
                    {
                        m_Jump = true;
                    }
                }               
            }

            if(!canAttack)
            {
                attackResetTimer += Time.deltaTime;
                if(attackResetTimer > attackResetDuration)
                {
                    canAttack = true;
                    attackResetTimer = 0.0f;
                }
            }

            if(CrossPlatformInputManager.GetButtonDown(fireButton))
            {
                if(!isLightAttacking && !isHeavyAttacking && canAttack)
                {
                    isLightAttacking = true;
                    m_Character.StopMovementHorizontal();
                }
            }
            else if (CrossPlatformInputManager.GetButtonDown(fire3Button))
            {
                if (!isLightAttacking && !isHeavyAttacking && canAttack)
                {
                    isHeavyAttacking = true;
                    m_Character.StopMovementHorizontal();
                }
            }

            if (isLightAttacking)
            {
                m_canMove = false;
                attackBoxLight.SetActive(true);
                attackTimerLight += Time.deltaTime;
                if(attackTimerLight > attackDurationLight)
                {
                    isLightAttacking = false;
                    attackTimerLight = 0.0f;
                    attackBoxLight.SetActive(false);
                    canAttack = false;
                    m_canMove = true;
                }
            }
            else if(isHeavyAttacking)
            {
                m_canMove = false;
                attackBoxHeavy.SetActive(true);
                attackTimerHeavy += Time.deltaTime;
                if (attackTimerHeavy > attackDurationHeavy)
                {
                    isHeavyAttacking = false;
                    attackTimerHeavy = 0.0f;
                    attackBoxHeavy.SetActive(false);
                    canAttack = false;
                    m_canMove = true;
                }
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);

            float h = CrossPlatformInputManager.GetAxis(horizontalAxisButton);

            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump, m_jumpPower);
            m_Jump = false;
        }

        public void ResetJumpPower()
        {
            m_jumpPower = m_jumpPowerMin;
        }

        public void TakeDamage(float damage, Vector2 attackPos, bool shouldPunchUp)
        {
            if (m_canTakeDamage)
            {
                m_Character.m_Grounded = false;

                m_canTakeDamage = false;
                currentHealthPercent += damage;

                if (UnityEngine.Random.Range(0.0f, 100.0f) < currentHealthPercent)
                {
                    //Debug.Log("big hit");
                    m_wasHitBig = true;

                    if(shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, 0.8f * damage);
                        m_wasPunchedUp = true;
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, 1.0f * damage);
                    }
                }
                else
                {
                    //Debug.Log("small hit");
                    m_wasHitSmall = true;

                    if (shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, 0.8f * damage);
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, 1.0f * damage);
                    }
                }
            }
        }
    }
}
