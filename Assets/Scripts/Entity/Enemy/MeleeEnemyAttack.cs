using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    private EntityStats entity_stats;
    
    private MeleeEnemyMove enemy_move;

    private float cooldown;
    private bool can_attack = true;

    [Header("Attack Impact VFX")]
    public GameObject impact_fx;

    [Header("Attack Impact Sound")]
    public AudioClip enemyImpact_soundClip;

    [Header("Camera Shake Values")]
    public float shakeIntensity;
    public float shakeTime;

    void Start()
    {
        entity_stats = this.gameObject.GetComponent<EntityStats>();
        enemy_move = this.gameObject.GetComponent<MeleeEnemyMove>();
    }


    void Update()
    {
        Cooldown();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && can_attack == true)
        {
            StartCoroutine(AttackAnimation(collision));
            StartCoroutine(DoDamage(collision, 0.3f));
            can_attack = false;
            cooldown = 0;

            StartCoroutine(TimeToBreath());
        }
    }

    private IEnumerator DoDamage(Collision2D collision, float delay)
    {
        EntityStats collision_stats = collision.gameObject.GetComponent<EntityStats>();
        yield return new WaitForSeconds(delay);
        collision_stats.RemoveHp(entity_stats.attack_damage);
        CameraShake.Instance.ShakeCamera(shakeIntensity, shakeTime);
        Knockback(collision);
    }

    private IEnumerator AttackAnimation(Collision2D collision)
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("isAttack");      
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.impact_soundSource.PlayOneShot(enemyImpact_soundClip);
        EnemyImpactPointAnim(collision);
    }

    private void Knockback(Collision2D collision)
    {
        if (collision.contactCount > 0 && collision.gameObject.CompareTag("Player"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 contactNormal = contact.normal;

            Vector2 knockbackDirection = -contactNormal;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * entity_stats.knockback_force, ForceMode2D.Impulse);
        }
    }

    private IEnumerator TimeToBreath()
    {
        enemy_move.can_move = false;
        yield return new WaitForSeconds(0.5f);
        enemy_move.can_move = true;
    }

    private void EnemyImpactPointAnim(Collision2D collision)
    {
        ContactPoint2D contact = collision.contacts[0];
        GameObject impact_fx_prefab = Instantiate(impact_fx, contact.point, Quaternion.identity);
        impact_fx_prefab.transform.SetParent(collision.transform);
    }
    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void Cooldown()
    {
        if(cooldown >= entity_stats.attack_speed && can_attack == false)
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
