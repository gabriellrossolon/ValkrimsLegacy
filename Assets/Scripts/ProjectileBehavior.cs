using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private EntityStats player_stats;

    [HideInInspector] public float projectile_speed;

    void Start()
    {
        player_stats = GameObject.FindWithTag("Player").GetComponent<EntityStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Deu dano");
            collision.GetComponent<EntityStats>().RemoveHp(player_stats.attack_damage);
        }
        Destroy(this.gameObject, 0.1f);
    }
}
