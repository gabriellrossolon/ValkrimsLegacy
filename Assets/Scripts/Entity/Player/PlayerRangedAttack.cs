using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRangedAttack : MonoBehaviour
{
    private EntityStats entity_stats;

    private float cooldown;
    private bool can_attack;

    public GameObject projectile_selected;

    [HideInInspector] public GameObject projectile_instance;

    public bool using_staff;
    public bool using_bow;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();

        can_attack = true;
    }


    void Update()
    {
        BasicAttack();
        BasicAttackCooldown();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE BOTÃO ESQUERDO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void BasicAttack()
    {
        if (can_attack == true && Input.GetMouseButtonDown(0))
        {
            projectile_instance = Instantiate(projectile_selected, transform.position, Quaternion.identity);

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            Vector2 projectile_direction = (mousePosition - transform.position).normalized;

            Vector2 spawn_position = (Vector2)transform.position + projectile_direction * 1.2f;
            projectile_instance.transform.position = spawn_position;

            float angle = Mathf.Atan2(projectile_direction.y, projectile_direction.x) * Mathf.Rad2Deg;

            projectile_direction.Normalize();
            projectile_instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * projectile_instance.GetComponent<ProjectileBehavior>().projectile_speed, ForceMode2D.Impulse);
            projectile_instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            can_attack = false;
            cooldown = 0;

            if (using_staff)
            {
                entity_stats.RemoveMana(15f);
            } 
            else if (using_bow)
            {
                entity_stats.RemoveStamina(8f);
            }
            
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void BasicAttackCooldown()
    {
        if (cooldown >= entity_stats.attack_speed && can_attack == false)
        {
            can_attack = true;
            cooldown = 0;
        }
        else
        {
            cooldown += Time.deltaTime;
        }
    }
}
