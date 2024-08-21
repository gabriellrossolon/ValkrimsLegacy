using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    private EntityStats player_status;
    [HideInInspector] public PolygonCollider2D sword_collider;
    [HideInInspector] public GameObject impact_fx;
    public GameObject circle_impact_fx;
    public GameObject strong_attack_charging_fx;

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

            //Adiciona o Objeto Enemy que colidiu a uma lista de Objetos que levaram dano
            damagedEnemies.Add(collision);
            StartCoroutine(RemoveDamagedEnemy(collision));
        }
    }

    //Remove o Objeto da lista de Objetos que levaram dano depois do delay
    private IEnumerator RemoveDamagedEnemy(Collider2D collision)
    {
        yield return new WaitForSeconds(0.3f);
        damagedEnemies.Remove(collision);
    }

    private IEnumerator DoDamage(Collider2D collision, float delay)
    {
        yield return new WaitForSeconds(delay);
        collision.GetComponent<EntityStats>().RemoveHp(player_status.attack_damage);
        ImpactPointAnim(collision);
        Knockback(collision);
    }

    private void ImpactPointAnim(Collider2D collision)
    {
        Vector2 impactPoint = collision.ClosestPoint((Vector2)transform.position);
        GameObject impact_fx_instance = Instantiate(impact_fx, impactPoint, impact_fx.transform.rotation);
        impact_fx_instance.transform.SetParent(collision.transform);
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    //Ajustar futuramente
    private void Knockback(Collider2D collider)
    {
        // Converte a posição do collider e a posição do jogador para Vector2
        Vector2 playerPosition = transform.position; // Transform.position é Vector3, precisa ser convertido
        Vector2 colliderPosition = collider.transform.position; // Collider.transform.position é Vector3, precisa ser convertido

        // Calcula a direção do knockback (oposta à direção do jogador ao ponto de impacto)
        Vector2 knockbackDirection = (colliderPosition - playerPosition).normalized;

        // Obtém o Rigidbody2D do objeto que possui o Collider2D
        Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Aplica a força de knockback
            rb.AddForce(knockbackDirection * player_status.knockback_force, ForceMode2D.Impulse);
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//
}

