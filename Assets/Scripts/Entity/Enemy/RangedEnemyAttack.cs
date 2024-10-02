using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    public AudioClip enemyImpact_soundClip;
    public AudioClip enemyShot_soundClip;

    private EntityStats entity_stats;
    private GameObject player_object;

    private float cooldown;
    private bool can_attack;

    public GameObject projectile_prefab;
    public float projectile_speed;
    public GameObject impact_fx;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();
        player_object = GameObject.FindWithTag("Player");

        can_attack = true;

    }

    void Update()
    {
        BasicAttack();
        BasicAttackCooldown();
    }

    private void BasicAttack()
    {
        float distance_to_player = Vector3.Distance(transform.position, player_object.transform.position);
        if (can_attack == true && distance_to_player <= this.gameObject.GetComponent<RangedEnemyMove>().detection_range)
        {
            ExecuteBasicAttack();
        }
    }

    private void ExecuteBasicAttack()
    {
        AttackAnimation();

        SoundManager.Instance.impact_soundSource.PlayOneShot(enemyShot_soundClip);

        GameObject projectile_instance = Instantiate(projectile_prefab, transform.position, Quaternion.identity);
        projectile_instance.gameObject.GetComponent<ProjectileBehavior>().instantiator = this.gameObject;

        Vector3 playerPosition = player_object.transform.position;
        Vector2 projectile_direction = (playerPosition - transform.position).normalized;

        float angle = Mathf.Atan2(projectile_direction.y, projectile_direction.x) * Mathf.Rad2Deg;
        projectile_instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        projectile_instance.gameObject.GetComponent<ProjectileBehavior>().impact_fx = impact_fx;

        projectile_instance.GetComponent<Rigidbody2D>().AddForce(projectile_direction * projectile_speed, ForceMode2D.Impulse);

        can_attack = false;
        cooldown = 0;
    }

    private void AttackAnimation()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("isAttack");
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
