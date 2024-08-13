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

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        weap_rot_controller = GetComponentInChildren<WeaponRotateController>();
        sword_behavior = GetComponentInChildren<SwordBehavior>();
    }

    void Update()
    {
        AttackOne();
        AttackCooldown();
    }

    void AttackOne()
    {
        if(can_attack == true && Input.GetMouseButtonDown(0))
        {
            can_attack = false;
            cooldown = 0;

            EnabledSwordCollider();

            if (weap_rot_controller.weaponFacingRight == true)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackR");
            } 
            else
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackL");
            }
        }
    }

    void AttackCooldown()
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

    private void EnabledSwordCollider()
    {
        sword_behavior.sword_collider.enabled = true;
        StartCoroutine(DisableSwordCollider(0.1f));
    }
    private IEnumerator DisableSwordCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        sword_behavior.sword_collider.enabled = false;
    }
}
