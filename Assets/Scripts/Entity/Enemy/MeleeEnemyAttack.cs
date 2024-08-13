using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyAttack : MonoBehaviour
{
    EntityStats entity_stats;

    void Start()
    {
        entity_stats = this.gameObject.GetComponent<EntityStats>();
    }


    void Update()
    {
        
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DoDamage(collision, 0.2f));
            AttackAnimation();       
        }
    }

    private IEnumerator DoDamage(Collision2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);
        collision.gameObject.GetComponent<EntityStats>().RemoveHp(entity_stats.attack_damage);
        StartCoroutine(RedDamageIndicator(collision, 0.1f));
        Knockback(collision);
    }

    private void AttackAnimation()
    {
        this.gameObject.GetComponent<Animator>().SetTrigger("isAttack");
    }

    private IEnumerator RedDamageIndicator(Collision2D collision, float delay)
    {
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(delay);
        collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void Knockback(Collision2D collision)
    {
        if (collision.contactCount > 0)
        {
            // Obtém o primeiro ponto de contato da colisão
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 contactNormal = contact.normal;

            // Calcula a direção do knockback (oposta à normal do contato)
            Vector2 knockbackDirection = -contactNormal;

            // Aplica a força de knockback
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * entity_stats.knockback_force, ForceMode2D.Impulse);
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
}
