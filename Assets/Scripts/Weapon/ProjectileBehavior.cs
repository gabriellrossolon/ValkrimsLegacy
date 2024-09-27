using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private EntityStats player_stats;

    public GameObject impact_fx;

    [HideInInspector] public float projectile_speed;

    private AudioClip impact_soundClip;

    [HideInInspector] public bool enemy_projectile;

    [HideInInspector] public GameObject instantiator;

    void Start()
    {
        player_stats = GameObject.FindWithTag("Player").GetComponent<EntityStats>();
        Destroy(this.gameObject, 4f);
        
        if(instantiator.CompareTag("Player"))
        {
            enemy_projectile = false;
            impact_soundClip = SoundManager.Instance.impact_soundClip;
        }

        if (instantiator.CompareTag("Enemy"))
        {
            enemy_projectile = true;
            impact_soundClip = instantiator.GetComponent<RangedEnemyAttack>().enemyImpact_soundClip;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!enemy_projectile)
            {
                collision.GetComponent<EntityStats>().RemoveHp(player_stats.attack_damage + player_stats.base_attack_damage);
                ImpactPointAnim(collision);
                SoundManager.Instance.impact_soundSource.PlayOneShot(impact_soundClip);
                Destroy(this.gameObject, 0.1f);
            }
            
        }
        if (collision.CompareTag("Player"))
        {
            if (enemy_projectile)
            {
                collision.GetComponent<EntityStats>().RemoveHp(instantiator.GetComponent<EntityStats>().attack_damage);
                ImpactPointAnim(collision);
                SoundManager.Instance.impact_soundSource.PlayOneShot(impact_soundClip);
                Destroy(this.gameObject, 0.1f);
            }
        }

    }

    private void ImpactPointAnim(Collider2D collision)
    {
        Vector2 impactPoint = collision.ClosestPoint((Vector2)transform.position);
        GameObject impact_fx_instance = Instantiate(impact_fx, impactPoint, impact_fx.transform.rotation);
        impact_fx_instance.transform.SetParent(collision.transform);
    }
}
