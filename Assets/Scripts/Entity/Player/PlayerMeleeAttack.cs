using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private EntityStats entity_stats;
    private WeaponRotateController weap_rot_controller;
    private SwordBehavior sword_behavior;
    private PlayerMovement player_movement;

    [Header("Weapons Ref")]
    public GameObject player_weapon;
    private Animator weapon_animator;

    private bool can_attack = true;
    private float cooldown;

    [Header("Strong Attack Refs")]
    public GameObject strong_attack_charg_fx;
    private Coroutine strongAttackCoroutine;
    private bool can_strong_attack = true;
    private float strongattack_cooldown;

    private float pressStartTime = 0f;
    private bool isPressing = false;
    private bool isStrongAttackQueued = false;

    private bool hasShownStaminaWarning = false;
    private bool hasShownCooldownWarning = false;

    private int attack_step;
    private Coroutine basicAttackCoroutine;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        weap_rot_controller = GetComponentInChildren<WeaponRotateController>();
        sword_behavior = GetComponentInChildren<SwordBehavior>();
        player_movement = GetComponent<PlayerMovement>();
        weapon_animator = player_weapon.gameObject.GetComponent<Animator>();

        attack_step = 1;
    }

    void Update()
    {
        AllAttacks();
        AllCooldowns();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    private void AllAttacks()
    {
        BasicAttack(3f);

        if (SkillManager.Instance.can_use_powerCutter) { PowerCutter(15f); }

        if (SkillManager.Instance.can_use_bladeDance) { BladeDance(10f); } 
    }

    private void AllCooldowns()
    {
        AttackCooldown();
        StrongAttackCooldown();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE BOTÃO ESQUERDO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void BasicAttack(float stamina_cost)
    {
        if (can_attack == true && Input.GetMouseButtonDown(0) && player_movement.is_dashing == false && (entity_stats.stamina >= stamina_cost) && !isPressing)
        {
            entity_stats.RemoveStamina(stamina_cost);
            switch (attack_step)
            {
                case 1:
                    attack_step++;
                    can_attack = false;
                    cooldown = entity_stats.attack_speed;
                    if (weap_rot_controller.weaponFacingRight == true)
                    {
                        weapon_animator.SetTrigger("isAttackR");
                    }
                    else
                    {
                        weapon_animator.SetTrigger("isAttackL");
                    }
                    
                    SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip_one);
                    Invoke("InvokeCollider", 0.1f);
                    StartResetCoroutine();
                    break;

                case 2:
                    attack_step++;
                    can_attack = false;
                    cooldown = entity_stats.attack_speed;
                    if (weap_rot_controller.weaponFacingRight == true)
                    {
                        weapon_animator.SetTrigger("isAttackR2");
                    }
                    else
                    {
                        weapon_animator.SetTrigger("isAttackL2");
                    }
                   
                    StopCoroutine(ResetSequence());
                    StartCoroutine(ResetSequence());
                    SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip_two);
                    Invoke("InvokeCollider", 0.1f);
                    StartResetCoroutine();
                    break;

                case 3:
                    attack_step = 1;
                    cooldown = 0;
                    can_attack = false;
                    if (weap_rot_controller.weaponFacingRight == true)
                    {
                        weapon_animator.SetTrigger("isAttackR3");
                    }
                    else
                    {
                        weapon_animator.SetTrigger("isAttackL3");
                    }
                    
                    StopCoroutine(ResetSequence());
                    SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip_one);
                    Invoke("InvokeCollider", 0.1f);
                    StopCoroutineIfNeeded();
                    break;

                default:
                    break;
            }
        }
    }
    private void InvokeCollider()
    {
        EnabledSwordCollider(0.1f);
    }

    private void StartResetCoroutine()
    {
        StopCoroutineIfNeeded();
        basicAttackCoroutine = StartCoroutine(ResetSequence());
    }

    private void StopCoroutineIfNeeded()
    {
        if (basicAttackCoroutine != null)
        {
            StopCoroutine(basicAttackCoroutine);
            basicAttackCoroutine = null;
        }
    }

    private IEnumerator ResetSequence()
    {
        yield return new WaitForSeconds(2f);
        attack_step = 1;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE BOTÃO DIREITO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void PowerCutter(float stamina_cost)
    {
        if (Input.GetMouseButton(1))
        {
            if (can_strong_attack && entity_stats.stamina >= stamina_cost)
            {
                if (!isPressing)
                {
                    pressStartTime = Time.time;
                    isPressing = true;
                    strong_attack_charg_fx.GetComponent<ParticleSystem>().Play();
                }
                if (!isStrongAttackQueued)
                {
                    player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", true);
                    hasShownStaminaWarning = false;
                    hasShownCooldownWarning = false;
                }        
            }
            else if (!can_strong_attack)
            {
                ShowWarning(ref hasShownCooldownWarning, HudManager.Instance.cooldown_warn);
            }
            else if (entity_stats.stamina < stamina_cost)
            {
                ShowWarning(ref hasShownStaminaWarning, HudManager.Instance.stamina_warn);
            }
        }
        else
        {
            if (isPressing)
            {
                strong_attack_charg_fx.GetComponent<ParticleSystem>().Stop();

                float pressDuration = Time.time - pressStartTime;

                if (pressDuration >= 2f)
                {
                    isStrongAttackQueued = true;
                }
                else
                {
                    CancelStrongAttack();
                }
                isPressing = false;
            }
        }
        if (isStrongAttackQueued && !Input.GetMouseButton(1))
        {
            if (entity_stats.stamina >= stamina_cost)
            {
                entity_stats.RemoveStamina(stamina_cost);
                ExecuteStrongAttack();
            }
            else
            {
                ShowWarning(ref hasShownStaminaWarning, HudManager.Instance.stamina_warn);
            }
            isStrongAttackQueued = false;
        }
    }

    private void ExecuteStrongAttack()
    {
        SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip_two);
        float original_attack_damage = entity_stats.attack_damage;
        float original_knockback_force = entity_stats.knockback_force;

        ChangeStrongAttackValues(original_attack_damage, original_knockback_force);

        EnabledSwordCollider(0.1f);
        player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", false);

        if (weap_rot_controller.weaponFacingRight)
        {
            player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isStrongAttackR");
        }
        else
        {
            player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isStrongAttackL");
        }
        can_strong_attack = false;
        strongattack_cooldown = 0;
    }

    private void ChangeStrongAttackValues(float original_attack_damage, float original_knockback_force)
    {
        entity_stats.attack_damage *= SkillManager.Instance.powerCutter_damageMultiplier;
        entity_stats.knockback_force = 15;

        StartCoroutine(RestoreStrongAttackValues(original_attack_damage, original_knockback_force));
    }

    private IEnumerator RestoreStrongAttackValues(float original_attack_damage, float original_knockback_force)
    {
        yield return new WaitForSeconds(0.5f);
        entity_stats.attack_damage = original_attack_damage;
        entity_stats.knockback_force = original_knockback_force;
    }

    private void ShowWarning(ref bool hasShownWarning, GameObject warningPrefab)
    {
        if (!hasShownWarning)
        {
            GameObject warning_instance = Instantiate(warningPrefab, transform.position, Quaternion.identity);
            Destroy(warning_instance, 1f);
            hasShownWarning = true;
        }
    }

    private void CancelStrongAttack()
    {
        player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", false);
        strong_attack_charg_fx.GetComponent<ParticleSystem>().Stop();
        hasShownStaminaWarning = false;
        hasShownCooldownWarning = false;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE DASH + BOTÃO ESQUERDO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    private void BladeDance(float stamina_cost)
    {
        if (Input.GetMouseButtonDown(0) && player_movement.is_dashing == true && !isPressing)
        {
            if (entity_stats.stamina >= stamina_cost)
            {
                SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip_three);

                float original_attack_damage = entity_stats.attack_damage;
                float original_knockback_force = entity_stats.knockback_force;

                StartCoroutine(ChangeDashAttackValues(original_attack_damage, original_knockback_force));

                EnabledSwordCollider(0.3f);

                if (weap_rot_controller.weaponFacingRight == true)
                {
                    player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackDashR");
                    GameObject dashimpact_fx_instance = Instantiate(sword_behavior.circle_impact_fx, sword_behavior.transform.position, Quaternion.Euler(0, 0, 0));
                    dashimpact_fx_instance.transform.SetParent(transform);

                }
                else if (weap_rot_controller.weaponFacingRight == false)
                {
                    player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackDashL");
                    GameObject dashimpact_fx_instance = Instantiate(sword_behavior.circle_impact_fx, sword_behavior.transform.position, Quaternion.Euler(0, -180, 0));
                    dashimpact_fx_instance.transform.SetParent(transform);
                }

                entity_stats.RemoveStamina(stamina_cost);
            }
            else
            {
                GameObject stamina_warn_instance = Instantiate(HudManager.Instance.stamina_warn, transform.position, Quaternion.identity);
                Destroy(stamina_warn_instance, 1f);
            }
            
        }
    }

    private IEnumerator ChangeDashAttackValues(float original_attack_damage, float original_knockback_force)
    {
        yield return new WaitForSeconds(0.1f);
        entity_stats.attack_damage *= SkillManager.Instance.bladeDance_damageMultiplier;
        entity_stats.knockback_force = 10;

        yield return new WaitForSeconds(0.1f);
        entity_stats.attack_damage = original_attack_damage;
        entity_stats.knockback_force = original_knockback_force;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void AttackCooldown()
    {
        if(cooldown >= entity_stats.attack_speed && can_attack == false)
        {
            can_attack = true;
            cooldown = 0;
        } else
        {
            cooldown += Time.deltaTime;
        }
    }

    private void StrongAttackCooldown()
    {
        if (strongattack_cooldown >= entity_stats.strong_attack_cooldown && can_strong_attack == false)
        {
            can_strong_attack = true;
            strongattack_cooldown = 0;
        }
        else
        {
            strongattack_cooldown += Time.deltaTime;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void EnabledSwordCollider(float collision_actived_time)
    {
        sword_behavior.sword_collider.enabled = true;
        StartCoroutine(DisableSwordCollider(collision_actived_time));
    }
    private IEnumerator DisableSwordCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        sword_behavior.sword_collider.enabled = false;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
}
