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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colidiu");
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EntityStats>().RemoveHp(player_status.attack_damage);
            collision.GetComponent<Animator>().SetTrigger("isHurt");
        }
    }
}
