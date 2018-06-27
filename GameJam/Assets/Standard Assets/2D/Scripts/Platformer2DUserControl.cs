using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;

        //Jumping variables
        private bool m_Jump;
        bool m_JumpHeld;
        float m_jumpPower;
        public float m_jumpPowerMin = 8.0f;
        public float m_jumpPowerMax = 18.0f;
        public float m_JumpPowerGain = 100.0f;
        public bool m_isChargingJump = false;

        //Dodging variables
        public bool m_isDodging = false;
        float m_dodgeStopTimer = 0.0f;
        float m_dodgeStopDuration = 0.15f;
        float m_timeBetweenDodge = 0.2f;
        float m_betweenDodgeTimer = 0.0f;
        bool m_canDodge = true;

        //Shielding variables
        public bool m_isShielding = false;
        float m_timeBetweenShield = 0.8f;
        float m_betweenShieldTimer = 0.0f;
        bool m_canShield = true;
        public float m_shieldChargeCurrent = 0.0f;
        float m_shieldChargeMax = 100.0f;
        float m_shieldRechargeRate = 45.0f;
        float m_shieldDrainRate = 125.0f;
        float m_shieldMinChargeForUse = 25.0f;
        public GameObject shieldSprite;

        bool m_isAxisInUse = false;

        bool m_canTakeDamage = true;
        float m_damageDelay = 0.25f;
        float m_damageDelayTimer = 0.0f;

        bool m_wasHitSmall = false;
        float m_hitSmallDelay = 0.25f;
        float m_hitSmallTimer = 0.0f;

        bool m_wasHitBig = false;
        float m_hitBigDelay = 1.0f;
        float m_hitBigTimer = 0.0f;
        float m_minBigHitTime = 0.2f;

        bool m_wasPunchedUp = false;
        float m_minPunchUpTime = 0.25f;
        float m_maxPunchUpTime = 1.5f;
        float m_punchUpTimer = 0.0f;

        public bool m_canMove = true;

        public float currentHealthPercent = 0.0f;

        bool canAttack = true;
        bool canLightAttack = true;
        bool isLightAttacking = false;
        float attackDurationLight = 0.2f;
        float attackTimerLight = 0.0f;
        public GameObject attackBoxLight;

        bool canHeavyAttack = true;
        bool isHeavyAttacking = false;
        float attackDurationHeavy = 0.3f;
        float attackTimerHeavy = 0.0f;
        public GameObject attackBoxHeavy;

        float lightAttackResetDuration = 0.35f;
        float lightAttackResetTimer = 0.0f;
        float heavyAttackResetDuration = 1.0f;
        float heavyAttackResetTimer = 0.0f;

        public string horizontalAxisButton;
        public string verticalAxisButton;
        public string jumpButton;
        public string fireButton;
        public string fire2Button;
        public string fire3Button;

        BoxCollider2D playerCollider;

        public bool isBuffed;
        public float buffAmount;
        bool hasStartedBuff;
        float startScaleX;
        float startScaleY;

        private void Awake()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            m_Character = GetComponent<PlatformerCharacter2D>();
            m_jumpPower = m_jumpPowerMin;
            playerCollider = GetComponent<BoxCollider2D>();
            m_shieldChargeCurrent = m_shieldChargeMax;

            startScaleX = this.transform.localScale.x;
            startScaleY = this.transform.localScale.y;
        }

        private void OnEnable()
        {
            m_shieldChargeCurrent = m_shieldChargeMax;
        }

        private void Update()
        {
            if (isBuffed)
            {
                if (!hasStartedBuff)
                {
                    transform.localScale = new Vector2((startScaleX * 1.2f) + ((startScaleX * buffAmount * 1.5f) / 100), (startScaleY * 1.2f) + ((startScaleY * buffAmount * 1.5f) / 100));
                    transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3.0f);
                    hasStartedBuff = true;
                }
            }
            else
            {
                if (hasStartedBuff)
                {
                    transform.localScale = new Vector2(startScaleX, startScaleY);
                    hasStartedBuff = false;
                }
            }


            //If was punched up, perform punch up action
            if (m_wasPunchedUp)
            {
                GetPunchedUp();
            }
            else
            {
                //If was hit small, perform hit small action
                if (m_wasHitSmall)
                {
                    GetHitSmall();
                }

                //If was hit big, perform hit big action
                if (m_wasHitBig)
                {
                    GetHitBig();
                }
            }

            //Prevents player from taking damage too quickly between being attacked. Adds delay once hit, and they can't get hit until the timer is up.
            DamageTakenDelayTimer();
        
            //Control for shield;
            ShieldControl();
            //Control for dodge.
            DodgeControl();

            //Air control, allows player to fall quicker or slower with movement stick
            AirControlUpDown();
            //Control for jumping
            JumpControl();

            //Control for light attack, starts the attack
            LightAttackControl();
            //Control for heavy attack, starts the attack.
            HeavyAttackControl();

            //If is light attacking, perform light attack action.
            if (isLightAttacking)
            {
                IsLightAttacking();
            }
            //If is heavy attacking perform heavy attack action
            else if(isHeavyAttacking)
            {
                IsHeavyAttacking();
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
            if (m_canTakeDamage && !m_isShielding && !m_isDodging)
            {
                m_Character.m_Grounded = false;

                m_canTakeDamage = false;
                currentHealthPercent += damage;

                if (UnityEngine.Random.Range(0.0f, 200.0f) < currentHealthPercent)
                {
                    m_wasHitBig = true;

                    if(shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, ((1.2f * damage) + ((damage * currentHealthPercent) / 100)));
                        m_wasPunchedUp = true;
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, ((1.4f * damage) + ((damage * currentHealthPercent) / 100)));
                    }
                }
                else
                {
                    //Debug.Log("small hit");
                    m_wasHitSmall = true;

                    if (shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, ((1.2f * damage) + ((damage * currentHealthPercent) / 100)));
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, ((1.4f * damage) + ((damage * currentHealthPercent) / 100)));
                    }
                }
            }
        }

        void StopShielding()
        {
            m_Character.ResetGravityScale();
            m_isShielding = false;
            shieldSprite.SetActive(false);
            m_canShield = false;
        }

        void GetPunchedUp()
        {
            m_punchUpTimer += Time.deltaTime;
            m_canMove = false;
            canAttack = false;

            m_Character.UnfreezeRotation();            
            m_Character.m_Rigidbody2D.AddTorque(CrossPlatformInputManager.GetAxis(horizontalAxisButton) * 1);

            if (m_punchUpTimer > (m_minPunchUpTime + ((m_minPunchUpTime * currentHealthPercent) / 100)))
            {
                m_canShield = true;
                if (m_isShielding || (m_punchUpTimer > (m_maxPunchUpTime + ((m_maxPunchUpTime * currentHealthPercent) / 100))))
                {
                    canAttack = true;
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

        void GetHitSmall()
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

        void GetHitBig()
        {
            m_Character.UnfreezeRotation();
            m_Character.m_Rigidbody2D.AddTorque(CrossPlatformInputManager.GetAxis(horizontalAxisButton) * 1);

            m_canMove = false;
            m_hitBigTimer += Time.deltaTime;
            canAttack = false;

            if (m_hitBigTimer > (m_minBigHitTime + ((m_minBigHitTime * currentHealthPercent) / 100)))
            {
                m_canShield = true;

                if (m_hitBigTimer > (m_hitBigDelay + ((m_hitBigDelay * currentHealthPercent) / 100)) || m_isShielding)
                {
                    canAttack = true;
                    m_wasHitBig = false;
                    m_hitBigTimer = 0.0f;
                    m_canMove = true;
                    m_canShield = true;
                    m_Character.ResetRotation();
                }
            }
            else
            {
                m_canShield = false;
            }
        }

        void DamageTakenDelayTimer()
        {
            if (!m_canTakeDamage)
            {
                m_damageDelayTimer += Time.deltaTime;
                if (m_damageDelayTimer > m_damageDelay)
                {
                    m_damageDelayTimer = 0.0f;
                    m_canTakeDamage = true;
                }
            }
        }

        void AirControlUpDown()
        {
            if (!m_Character.m_Grounded && !m_isShielding)
            {
                if (CrossPlatformInputManager.GetAxisRaw(verticalAxisButton) > 0)
                {
                    m_Character.AddDownForce();
                }
                else if (CrossPlatformInputManager.GetAxisRaw(verticalAxisButton) < 0 && m_Character.CheckIfFalling())
                {
                    m_Character.AddUpForce();
                }
            }
        }

        void ShieldControl()
        {
            if (CrossPlatformInputManager.GetButton(fire2Button) && m_canShield)
            {
                if (!m_isDodging)
                {
                    m_Character.HalfGravityScale();
                    m_Character.StopMovementHorizontal();
                }

                if (!m_isShielding)
                {
                    m_isAxisInUse = true;
                    m_Character.StopMovement();
                }

                m_isShielding = true;

                shieldSprite.SetActive(true);
            }
            else if (m_isShielding)
            {
                StopShielding();
            }

            if (!m_canShield)
            {
                m_betweenShieldTimer += Time.deltaTime;
                if (m_betweenShieldTimer > m_timeBetweenShield)
                {
                    m_canShield = true;
                    m_betweenShieldTimer = 0.0f;
                }
            }

            if (m_isShielding)
            {
                m_shieldChargeCurrent -= (m_shieldDrainRate * Time.deltaTime);
                if (m_shieldChargeCurrent < 0)
                {
                    StopShielding();
                    m_shieldChargeCurrent = 0;
                }
            }
            else if (!m_isShielding && (m_shieldChargeCurrent < m_shieldChargeMax))
            {
                m_shieldChargeCurrent += (m_shieldRechargeRate * Time.deltaTime);
                if (m_shieldChargeCurrent > m_shieldChargeMax)
                {
                    m_shieldChargeCurrent = m_shieldChargeMax;
                }
            }

            if (m_isShielding && m_canDodge)
            {
                if (CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) != 0)
                {
                    if (!m_isAxisInUse)
                    {
                        if (CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) < 0)
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
                if (CrossPlatformInputManager.GetAxisRaw(horizontalAxisButton) == 0)
                {
                    m_isAxisInUse = false;
                }

            }
        }

        void DodgeControl()
        {
            if (!m_canDodge)
            {
                m_betweenDodgeTimer += Time.deltaTime;
                if (m_betweenDodgeTimer > m_timeBetweenDodge)
                {
                    m_canDodge = true;
                    m_betweenDodgeTimer = 0.0f;
                }
            }

            if (m_isDodging)
            {
                playerCollider.enabled = false;
                m_Character.StopGravityScale();
                m_dodgeStopTimer += Time.deltaTime;
                if (m_dodgeStopTimer > m_dodgeStopDuration)
                {
                    m_isDodging = false;
                    m_Character.ResetGravityScale();
                    m_dodgeStopTimer = 0.0f;

                    m_canDodge = false;
                    m_betweenDodgeTimer = 0.0f;
                    playerCollider.enabled = true;
                }

                shieldSprite.SetActive(false);
            }
        }

        void JumpControl()
        {
            if (CrossPlatformInputManager.GetButton(jumpButton) && m_Character.m_Grounded)
            {
                m_isChargingJump = true;
            }
            else if (!m_Character.m_Grounded)
            {
                m_isChargingJump = false;
            }

            if ((!m_Jump && (m_Character.m_timeSinceLastJump > m_Character.m_timeBetweenJump) && (m_Character.m_numberOfJumps < m_Character.m_jumpLimit) && !m_isShielding && m_canMove))
            {
                // Read the jump input in Update so button presses aren't missed.
                if (m_isChargingJump)
                {
                    m_jumpPower += m_JumpPowerGain * Time.deltaTime;
                    if (m_jumpPower > m_jumpPowerMax)
                    {
                        m_jumpPower = m_jumpPowerMax;
                        m_Jump = true;
                        m_isChargingJump = false;
                    }

                    if (CrossPlatformInputManager.GetButtonUp(jumpButton))
                    {
                        if (m_canMove)
                        {
                            m_Jump = true;
                        }
                        m_isChargingJump = false;
                    }
                }
                else
                {
                    if (CrossPlatformInputManager.GetButtonDown(jumpButton))
                    {
                        if (m_canMove)
                        {
                            m_Jump = true;
                        }
                    }
                }
            }
        }

        void LightAttackControl()
        {
            if (!canLightAttack)
            {
                lightAttackResetTimer += Time.deltaTime;
                if (lightAttackResetTimer > lightAttackResetDuration)
                {
                    canLightAttack = true;
                    lightAttackResetTimer = 0.0f;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown(fireButton) && canAttack)
            {
                if (!isLightAttacking && !isHeavyAttacking && canLightAttack)
                {
                    isLightAttacking = true;
                    m_Character.StopMovementVertical();

                    if (m_Character.m_FacingRight)
                    {
                        m_Character.m_Rigidbody2D.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                    }
                    else
                    {
                        m_Character.m_Rigidbody2D.AddForce(-Vector2.right * 10, ForceMode2D.Impulse);
                    }

                }
            }
        }

        void HeavyAttackControl()
        {
            if (!canHeavyAttack)
            {
                heavyAttackResetTimer += Time.deltaTime;
                if (heavyAttackResetTimer > heavyAttackResetDuration)
                {
                    canHeavyAttack = true;
                    heavyAttackResetTimer = 0.0f;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown(fire3Button) && canAttack)
            {
                if (!isLightAttacking && !isHeavyAttacking && canHeavyAttack)
                {
                    isHeavyAttacking = true;
                    m_Character.StopMovementVertical();
                    m_Character.m_Rigidbody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                }
            }
        }

        void IsLightAttacking()
        {
            m_canMove = false;
            attackBoxLight.SetActive(true);
            attackTimerLight += Time.deltaTime;
            if (attackTimerLight > attackDurationLight)
            {
                isLightAttacking = false;
                attackTimerLight = 0.0f;
                attackBoxLight.SetActive(false);
                canLightAttack = false;
                m_canMove = true;
            }
        }

        void IsHeavyAttacking()
        {
            m_canMove = false;
            attackBoxHeavy.SetActive(true);
            attackTimerHeavy += Time.deltaTime;
            if (attackTimerHeavy > attackDurationHeavy)
            {
                isHeavyAttacking = false;
                attackTimerHeavy = 0.0f;
                attackBoxHeavy.SetActive(false);
                canHeavyAttack = false;
                m_canMove = true;
            }
        }

        public void KillPlayer()
        {
            this.gameObject.SetActive(false);
        }

        public void RespawnPlayer()
        {
            m_shieldChargeCurrent = m_shieldChargeMax;
            currentHealthPercent = 0.0f;
        }
    }
}
