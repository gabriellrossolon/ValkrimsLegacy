using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    EntityStats entity_stats;

    private bool can_attack = true;
    private float cooldown;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();
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
