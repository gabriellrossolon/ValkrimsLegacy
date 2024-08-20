using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    EntityStats entity_stats;
    public GameObject player_weapon;

    private bool can_attack = true;
    private float cooldown;

    private WeaponRotateController weap_rot_controller;
    private SwordBehavior sword_behavior;
    private PlayerMovement player_movement;

    private Coroutine strongAttackCoroutine;
    private bool can_strong_attack = true;
    private float strongattack_cooldown;
    public GameObject strong_attack_anim;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        weap_rot_controller = GetComponentInChildren<WeaponRotateController>();
        sword_behavior = GetComponentInChildren<SwordBehavior>();
        player_movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        AllAtacks();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    private void AllAtacks()
    {
        AttackCooldown();
        StrongAttackCooldown();

        AttackOne();
        AttackTwo();
        DashAttack();
    }

    private void AttackOne()
    {
        if (can_attack == true && Input.GetMouseButtonDown(0) && player_movement.is_dashing == false)
        {
            can_attack = false;
            cooldown = 0;

            EnabledSwordCollider(0.1f);

            if (weap_rot_controller.weaponFacingRight == true)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackR");
            } 
            else
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackL");
            }

            entity_stats.AddStamina(5f);
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void AttackTwo()
    {
        if (Input.GetMouseButton(1) && can_strong_attack == true && entity_stats.stamina >= 15f)
        {
            can_attack = false;
            // Se a corrotina não está em execução, inicia-a
            if (strongAttackCoroutine == null)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", true);
                strongAttackCoroutine = StartCoroutine(ExecuteStrongAttack(2f));
                strong_attack_anim.GetComponent<ParticleSystem>().Play();

                entity_stats.RemoveStamina(5f);
            }
        }
        else
        {
            // Cancela a corrotina se o botão for solto
            if (strongAttackCoroutine != null)
            {
                StopCoroutine(strongAttackCoroutine);
                strongAttackCoroutine = null;
                player_weapon.gameObject.GetComponent<Animator>().SetBool("isStrongAttackCharg", false);
                strong_attack_anim.GetComponent<ParticleSystem>().Stop();
            }
        }
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

        entity_stats.RemoveStamina(10f);
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

    private void DashAttack()
    {
        if (Input.GetMouseButtonDown(0) && player_movement.is_dashing == true && entity_stats.stamina >= 10f)
        {
            EnabledSwordCollider(0.3f);

            if (weap_rot_controller.weaponFacingRight == true)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackDashR");
                GameObject dashimpact_fx_instance = Instantiate(sword_behavior.circle_impact_fx, transform.position, Quaternion.Euler(0, 0, 0));
                dashimpact_fx_instance.transform.SetParent(transform);

            }
            else if(weap_rot_controller.weaponFacingRight == false)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackDashL");
                GameObject dashimpact_fx_instance = Instantiate(sword_behavior.circle_impact_fx, transform.position, Quaternion.Euler(0, -180, 0));
                dashimpact_fx_instance.transform.SetParent(transform);
            }

            entity_stats.RemoveStamina(10f);
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
