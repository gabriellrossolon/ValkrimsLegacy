using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    private EntityStats entity_stats;
    private WeaponRotateController weap_rot_controller;
    private SwordBehavior sword_behavior;
    private PlayerMovement player_movement;

    public GameObject player_weapon;
    private Animator weapon_animator;

    public bool can_attack = true;
    private float cooldown;

    private Coroutine strongAttackCoroutine;
    private bool can_strong_attack = true;
    private float strongattack_cooldown;
    public GameObject strong_attack_anim;

    private int attack_step = 1;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        weap_rot_controller = GetComponentInChildren<WeaponRotateController>();
        sword_behavior = GetComponentInChildren<SwordBehavior>();
        player_movement = GetComponent<PlayerMovement>();
        weapon_animator = player_weapon.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        AllAtacks();
        AllCooldowns();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void AllAtacks()
    {
        BasicAttack(3f);

        if (SkillManager.Instance.can_use_powerCutter) { PowerCutter(10f); }

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

    private void BasicAttack(float stamina_to_remove)
    {
        if (can_attack == true && Input.GetMouseButtonDown(0) && player_movement.is_dashing == false)
        {
            player_weapon.gameObject.GetComponent<Animator>().enabled = true;
            can_attack = false;
            cooldown = 0;
            entity_stats.RemoveStamina(stamina_to_remove);

            EnabledSwordCollider(0.1f);
            if (attack_step == 1)
            {
                if (weap_rot_controller.weaponFacingRight == true)
                {
                    weapon_animator.SetTrigger("isAttackR");
                    can_attack = true;
                    attack_step++;
                }
                else
                {
                    weapon_animator.SetTrigger("isAttackL");
                    can_attack = true;
                    attack_step++;
                }
            } else if (attack_step == 2)
            {
                if (weap_rot_controller.weaponFacingRight == true)
                {
                    weapon_animator.SetTrigger("isAttackR2");
                    can_attack = true;
                    cooldown = 0;
                    attack_step++;
                }
                else
                {
                    weapon_animator.SetTrigger("isAttackL2");
                    can_attack = true;
                    cooldown = 0;
                    attack_step++;
                }
            }
            else if (attack_step == 3)
            {
                if (weap_rot_controller.weaponFacingRight == true)
                {
                    weapon_animator.SetTrigger("isAttackR3");
                    can_attack = false;
                    cooldown = 0;
                    attack_step = 1;
                }
                else
                {
                    weapon_animator.SetTrigger("isAttackL3");
                    can_attack = false;
                    cooldown = 0;
                    attack_step = 1;
                }
            }
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE BOTÃO DIREITO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void PowerCutter(float stamina_cost)
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (can_strong_attack == true)
            {
                if (entity_stats.stamina >= stamina_cost)
                {
                    can_attack = false;
                    cooldown = -2f;
                    // Se a corrotina não está em execução, inicia-a
                    if (strongAttackCoroutine == null)
                    {
                        player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", true);
                        strongAttackCoroutine = StartCoroutine(ExecuteStrongAttack(2f));
                        strong_attack_anim.GetComponent<ParticleSystem>().Play();

                        entity_stats.RemoveStamina(stamina_cost);
                    }
                }
                else
                {
                    GameObject stamina_warn_instance = Instantiate(HudManager.Instance.stamina_warn, transform.position, Quaternion.identity);
                    Destroy(stamina_warn_instance, 1f);
                }
            }
            else
            {
                GameObject cooldown_warn_instance = Instantiate(HudManager.Instance.cooldown_warn, transform.position, Quaternion.identity);
                Destroy(cooldown_warn_instance, 1f);
            }
            
        }

        //else
        //{
        //    // Cancela a corrotina se o botão for solto
        //    if (strongAttackCoroutine != null)
        //    {
        //        StopCoroutine(strongAttackCoroutine);
        //        strongAttackCoroutine = null;
        //        player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", false);
        //        strong_attack_anim.GetComponent<ParticleSystem>().Stop();
        //    }
        //}
    }

    private IEnumerator ExecuteStrongAttack(float delay)
    {
        yield return new WaitForSeconds(delay);
        float original_attack_damage = entity_stats.attack_damage;
        float original_knockback_force = entity_stats.knockback_force;

        StartCoroutine(ChangeAttackValues(original_attack_damage, original_knockback_force));

        EnabledSwordCollider(0.1f);
        strong_attack_anim.GetComponent<ParticleSystem>().Stop();

        player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", false);
        if (weap_rot_controller.weaponFacingRight == true)
        {
            player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isStrongAttackR");
        }
        else
        {
            player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isStrongAttackL");
        }
        // Reseta a referência da corrotina após o término
        strongAttackCoroutine = null;

        can_strong_attack = false;
        strongattack_cooldown = 0;
    }
    private IEnumerator ChangeAttackValues(float original_attack_damage, float original_knockback_force)
    {
        yield return new WaitForSeconds(0.1f);
        entity_stats.attack_damage *= 3;
        entity_stats.knockback_force = 15;

        yield return new WaitForSeconds(0.1f);
        entity_stats.attack_damage = original_attack_damage;
        entity_stats.knockback_force = original_knockback_force;
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE DASH + BOTÃO ESQUERDO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    private void BladeDance(float stamina_cost)
    {
        if (Input.GetMouseButtonDown(0) && player_movement.is_dashing == true)
        {
            if (entity_stats.stamina >= stamina_cost)
            {
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
