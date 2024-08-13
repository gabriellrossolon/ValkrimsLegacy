using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private EntityStats player_status;
    [HideInInspector] public PolygonCollider2D sword_collider;

    void Start()
    {
        player_status = GameObject.FindWithTag("Player").GetComponent<EntityStats>();
        sword_collider = this.GetComponent<PolygonCollider2D>();
    }


    void Update()
    {
        
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            StartCoroutine(DoDamage(collision, 0.2f));
            EnemyHurtAnimation(collision);
        }
    }

    private IEnumerator DoDamage(Collider2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);
        collision.GetComponent<EntityStats>().RemoveHp(player_status.attack_damage);
        StartCoroutine(RedDamageIndicator(collision, 0.1f));
    }

    private void EnemyHurtAnimation(Collider2D collision)
    {
        collision.gameObject.GetComponent<Animator>().SetTrigger("isHurt");
    }

    private IEnumerator RedDamageIndicator(Collider2D collision, float delay)
    {
        collision.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(delay);
        if (collision != null)
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

}
