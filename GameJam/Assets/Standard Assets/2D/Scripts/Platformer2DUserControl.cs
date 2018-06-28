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
        public bool m_canShield = true;
        public float m_shieldChargeCurrent = 0.0f;
        float m_shieldChargeMax = 100.0f;
        float m_shieldRechargeRate = 45.0f;
        float m_shieldDrainRate = 125.0f;
        float m_shieldMinChargeForUse = 25.0f;
        public GameObject shieldSprite;

        bool m_isAxisInUse = false;

        bool m_canTakeDamage = true;
        float m_damageDelay = 0.2f;
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
        public GameObject attackBoxLightBuffed;

        bool canHeavyAttack = true;
        bool isHeavyAttacking = false;
        float attackDurationHeavy = 0.3f;
        float attackTimerHeavy = 0.0f;
        public GameObject attackBoxHeavy;
        public GameObject attackBoxHeavyBuffed;

        public bool specialIsProjectile = false;
        public bool firesOneProjectile = true;
        float projectileSpawnDuration = 0.3f;
        float projectSpawnTimer = 0.0f;
        bool hasFiredProjectile = false;
        bool canSpecialAttack = true;
        bool isSpecialAttacking = false;
        public float attackDurationSpecial = 0.5f;
        float attackTimerSpecial = 0.0f;
        public GameObject attackBoxSpecial;
        public float specialChargeDuration = 0.4f;
        public bool specialsNeedsToHitGround = false;
        public bool canOnlyUseSpecialInAir = false;
        public bool canOnlyUseSpecialOnGround = false;
        float specialTimeOnGroundDuration = 0.6f;
        float specialTimeOnGroundTimer = 0.0f;

        float lightAttackResetDuration = 0.35f;
        float lightAttackResetTimer = 0.0f;
        float heavyAttackResetDuration = 0.8f;
        float heavyAttackResetTimer = 0.0f;
        float specialAttackResetDuration = 1.5f;
        float specialAttackResetTimer = 0.0f;

        public string horizontalAxisButton;
        public string verticalAxisButton;
        public string jumpButton;
        public string fireButton;
        public string fire2Button;
        public string fire3Button;
        public string triggersButton;

        public bool isBuffed;
        public float buffAmount;
        float startBuffAmount;
        float buffDrainRate = 4.0f;
        bool hasStartedBuff;
        float startScaleX;
        float startScaleY;
        [HideInInspector]
        public float speedScale = 1.0f;

        private void Awake()
        {
            UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);

            m_Character = GetComponent<PlatformerCharacter2D>();
            m_jumpPower = m_jumpPowerMin;
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
                    speedScale = speedScale + ((speedScale * buffAmount) / (100 + 25));
                    transform.localScale = new Vector3((startScaleX * 1.2f) + ((startScaleX * buffAmount) / 100), (startScaleY * 1.2f) + ((startScaleY * buffAmount) / 100));

                    if(!m_Character.m_FacingRight)
                    {
                        Vector3 tempScale = transform.localScale;
                        tempScale.x = -tempScale.x;
                        transform.localScale = tempScale;
                    }

                    transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 3.0f);
                    hasStartedBuff = true;
                }

                buffAmount -= buffDrainRate * Time.deltaTime;
                if(buffAmount < 0)
                {
                    isBuffed = false;
                    buffAmount = 0;
                }
            }
            else
            {
                speedScale = 1.0f;
                if (hasStartedBuff)
                {
                    transform.localScale = new Vector2(startScaleX, startScaleY);

                    if (!m_Character.m_FacingRight)
                    {
                        Vector3 tempScale = transform.localScale;
                        tempScale.x = -tempScale.x;
                        transform.localScale = tempScale;
                    }

                    hasStartedBuff = false;
                    m_Character.ResetGravityScale();
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
            SpecialAttackControl();

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
            else if(isSpecialAttacking)
            {
                IsSpecialAttacking();
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

                //Using chance based on remaining health, determine whether or not this attack was big, knocking them down
                if (UnityEngine.Random.Range(0.0f, (200.0f * (speedScale * 2))) < currentHealthPercent)
                {
                    m_wasHitBig = true;

                    if(shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, (((1.5f * damage) + ((damage * currentHealthPercent) / 100)) / speedScale));
                        m_wasPunchedUp = true;
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, (((1.5f * damage) + ((damage * currentHealthPercent) / 100)) / speedScale));
                    }
                }
                else
                {
                    m_wasHitSmall = true;

                    if (shouldPunchUp)
                    {
                        m_Character.StopMovement();
                        m_Character.AddPunchUpForce(attackPos, (((1.5f * damage) + ((damage * currentHealthPercent) / 100)) / speedScale));
                    }
                    else
                    {
                        m_Character.PushAwayFromPoint(attackPos, (((1.5f * damage) + ((damage * currentHealthPercent) / 100)) / speedScale));
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
            if ((CrossPlatformInputManager.GetAxis(triggersButton) > 0) && m_canShield)
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
                if (lightAttackResetTimer > (lightAttackResetDuration * speedScale))
                {
                    canLightAttack = true;
                    lightAttackResetTimer = 0.0f;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown(fireButton) && canAttack)
            {
                if (!isLightAttacking && !isHeavyAttacking && !isSpecialAttacking && canLightAttack)
                {
                    isLightAttacking = true;
                    m_Character.StopMovementVertical();

                    if (m_Character.m_FacingRight)
                    {
                         m_Character.m_Rigidbody2D.AddForce(Vector2.right * (10 * speedScale), ForceMode2D.Impulse);
                    }
                    else
                    {
                        m_Character.m_Rigidbody2D.AddForce(-Vector2.right * (10 * speedScale), ForceMode2D.Impulse);
                    }

                }
            }
        }

        void HeavyAttackControl()
        {
            if (!canHeavyAttack)
            {
                heavyAttackResetTimer += Time.deltaTime;
                if (heavyAttackResetTimer > (heavyAttackResetDuration * speedScale))
                {
                    canHeavyAttack = true;
                    heavyAttackResetTimer = 0.0f;
                }
            }

            if (CrossPlatformInputManager.GetButtonDown(fire3Button) && canAttack)
            {
                if (!isLightAttacking && !isHeavyAttacking && !isSpecialAttacking && canHeavyAttack)
                {
                    isHeavyAttacking = true;
                    m_Character.StopMovementVertical();
                    m_Character.m_Rigidbody2D.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
                }
            }
        }

        void SpecialAttackControl()
        {
            if (!canSpecialAttack)
            {               
                specialAttackResetTimer += Time.deltaTime;
                if (specialAttackResetTimer > (specialAttackResetDuration * speedScale))
                {
                    canSpecialAttack = true;
                    specialAttackResetTimer = 0.0f;
                }
            }

            if (CrossPlatformInputManager.GetButton(fire2Button) && canAttack)
            {
                if (canOnlyUseSpecialInAir && !m_Character.m_Grounded || !canOnlyUseSpecialInAir)
                {
                    if (canOnlyUseSpecialOnGround && m_Character.m_Grounded || !canOnlyUseSpecialOnGround)
                    {
                        if (!isLightAttacking && !isHeavyAttacking && !isSpecialAttacking && canSpecialAttack)
                        {
                            isSpecialAttacking = true;
                            m_Character.StopMovement();
                        }
                    }
                }
            }
        }

        void IsLightAttacking()
        {
            if(isBuffed)
            {
                attackBoxLightBuffed.SetActive(true);
                attackBoxLight.SetActive(false);
            }
            else
            {
                attackBoxLight.SetActive(true);
                attackBoxLightBuffed.SetActive(false);
            }

            m_canShield = false;
            m_canMove = false;
            attackTimerLight += Time.deltaTime;
            if (attackTimerLight > (attackDurationLight * speedScale))
            {
                isLightAttacking = false;
                attackTimerLight = 0.0f;
                attackBoxLight.SetActive(false);
                attackBoxLightBuffed.SetActive(false);
                canLightAttack = false;
                m_canMove = true;
                m_canShield = true;
            }
        }

        void IsHeavyAttacking()
        {
            if (isBuffed)
            {
                attackBoxHeavyBuffed.SetActive(true);
                attackBoxHeavy.SetActive(false);
            }
            else
            {
                attackBoxHeavy.SetActive(true);
                attackBoxHeavyBuffed.SetActive(false);
            }

            m_canShield = false;
            m_canMove = false;
            attackTimerHeavy += Time.deltaTime;
            if (attackTimerHeavy > (attackDurationHeavy * speedScale))
            {
                isHeavyAttacking = false;
                attackBoxHeavy.SetActive(false);
                attackBoxHeavyBuffed.SetActive(false);
                canHeavyAttack = false;
                m_canMove = true;
                m_canShield = true;
                attackTimerHeavy = 0.0f;
            }
        }

        void IsSpecialAttacking()
        {
            m_canShield = false;
            m_canMove = false;
            attackTimerSpecial += Time.deltaTime;

            if(specialsNeedsToHitGround && attackTimerSpecial < specialChargeDuration)
            {
                m_Character.StopMovement();
            }
            else if (!specialsNeedsToHitGround)
            {
                m_Character.StopMovement();
            }

            //Wait for charge before attacking
            if (attackTimerSpecial > specialChargeDuration)
            {
                if (specialIsProjectile)
                {
                    if (!hasFiredProjectile)
                    {
                        GameObject projectileClone;
                        projectileClone = Instantiate(attackBoxSpecial, attackBoxSpecial.transform.position, attackBoxSpecial.transform.rotation) as GameObject;
                        projectileClone.SetActive(true);
                        projectileClone.transform.localScale = new Vector3(projectileClone.transform.localScale.x + Mathf.Abs(this.gameObject.transform.localScale.x), projectileClone.transform.localScale.y + this.gameObject.transform.localScale.y, 1.0f);
                        if (m_Character.m_FacingRight)
                        {
                            projectileClone.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 500);
                        }
                        else
                        {
                            projectileClone.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * 500);
                        }
                        hasFiredProjectile = true;
                    }
                }
                else if(specialsNeedsToHitGround)
                {
                    m_Character.m_Rigidbody2D.AddForce(new Vector2(0, -100));
                    if(m_Character.m_Grounded)
                    {
                        specialTimeOnGroundTimer += Time.deltaTime;
                        attackBoxSpecial.SetActive(true);
                        m_Character.StopMovement();
                    }
                }
                else
                {
                    if(canOnlyUseSpecialOnGround)
                    {
                        m_Character.m_Rigidbody2D.AddForce(new Vector2(0, 800));
                    }

                    attackBoxSpecial.SetActive(true);
                }
            }

            if ((!specialsNeedsToHitGround && (attackTimerSpecial > (attackDurationSpecial * speedScale))) || (specialsNeedsToHitGround && (specialTimeOnGroundTimer > specialTimeOnGroundDuration)))
            {
                isSpecialAttacking = false;
                canSpecialAttack = false;
                m_canMove = true;
                attackTimerSpecial = 0.0f;
                hasFiredProjectile = false;
                attackBoxSpecial.SetActive(false);
                specialTimeOnGroundTimer = 0.0f;
                m_canShield = true;
            }
        }

        public void KillPlayer()
        {
            isBuffed = false;
            buffAmount = 0.0f;
            this.gameObject.SetActive(false);
        }

        public void RespawnPlayer()
        {
            m_shieldChargeCurrent = m_shieldChargeMax;
            currentHealthPercent = 0.0f;
        }
    }
}
