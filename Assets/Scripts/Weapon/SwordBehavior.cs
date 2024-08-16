using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private EntityStats player_status;
    [HideInInspector] public PolygonCollider2D sword_collider;
    public GameObject impact_fx;
    public GameObject circle_impact_fx;

    private HashSet<Collider2D> damagedEnemies = new HashSet<Collider2D>();

    void Start()
    {
        player_status = GameObject.FindWithTag("Player").GetComponent<EntityStats>();
        sword_collider = this.GetComponent<PolygonCollider2D>();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !damagedEnemies.Contains(collision))
        {
            StartCoroutine(DoDamage(collision, 0.1f));
            EnemyHurtAnimation(collision);
            damagedEnemies.Add(collision);
            StartCoroutine(RemoveDamagedEnemy(collision));
        }
    }

    private IEnumerator RemoveDamagedEnemy(Collider2D collision)
    {
        yield return new WaitForSeconds(0.3f);
        damagedEnemies.Remove(collision);
    }

    private IEnumerator DoDamage(Collider2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);
        collision.GetComponent<EntityStats>().RemoveHp(player_status.attack_damage);

        Vector2 impactPoint = collision.ClosestPoint((Vector2)transform.position);
        GameObject impact_fx_instance = Instantiate(impact_fx, impactPoint, impact_fx.transform.rotation);
        impact_fx_instance.transform.SetParent(collision.transform);

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
