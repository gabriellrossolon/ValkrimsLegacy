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

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        weap_rot_controller = GetComponentInChildren<WeaponRotateController>();
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
            Debug.Log("Atacou");
            can_attack = false;
            cooldown = 0;

            if (weap_rot_controller.weaponFacingRight == true)
            {
                player_weapon.gameObject.GetComponent<Animator>().SetTrigger("isAttackR");
            } else
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
}
