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
        if (using_staff) { StaffBasicAttack(15f); }
        if (using_bow) { BowBasicAttack(8f); }

        BasicAttackCooldown();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//--ATAQUES DE BOTÃO ESQUERDO--//=====//=====//=====//=====//=====//=====//
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void StaffBasicAttack(float mana_cost)
    {
        if (can_attack == true && Input.GetMouseButtonDown(0))
        {
            if (entity_stats.mana >= mana_cost)
            {
                ExecuteBasicAttack();
                entity_stats.RemoveMana(mana_cost);
            } 
            else if (entity_stats.mana < mana_cost)
            {
                GameObject mana_warn_instance = Instantiate(HudManager.Instance.mana_warn, transform.position, Quaternion.identity);
                Destroy(mana_warn_instance, 1f);
            }
        }
    }

    private void BowBasicAttack(float stamina_cost)
    {
        if (can_attack == true && Input.GetMouseButtonDown(0))
        {
            if (entity_stats.stamina >= stamina_cost)
            {
                ExecuteBasicAttack();
                entity_stats.RemoveStamina(stamina_cost);
            }
            else if (entity_stats.stamina < stamina_cost)
            {
                GameObject stamina_warn_instance = Instantiate(HudManager.Instance.stamina_warn, transform.position, Quaternion.identity);
                Destroy(stamina_warn_instance, 1f);
            }
        }
    }

    private void ExecuteBasicAttack()
    {
        projectile_instance = Instantiate(projectile_selected, transform.position, Quaternion.identity);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 projectile_direction = (mousePosition - transform.position).normalized;

        Vector2 spawn_position = (Vector2)transform.position + projectile_direction * 1.2f;
        projectile_instance.transform.position = spawn_position;

        float angle = Mathf.Atan2(projectile_direction.y, projectile_direction.x) * Mathf.Rad2Deg;
        projectile_instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        projectile_direction.Normalize();

        projectile_instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * projectile_instance.GetComponent<ProjectileBehavior>().projectile_speed, ForceMode2D.Impulse);

        SoundManager.Instance.attack_soundSource.PlayOneShot(SoundManager.Instance.weaponAttack_audioClip);

        can_attack = false;
        cooldown = 0;
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
