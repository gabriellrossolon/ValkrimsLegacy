using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public static StatusManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    private EntityStats player_stats;

    void Start()
    {
        player_stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>(); 
    }

    public void ImproveHp()
    {
        player_stats.max_hp += 2;
        player_stats.hp = player_stats.max_hp;
    }
    public void ImproveAttack()
    {
        player_stats.base_attack_damage += 1;
    }
    public void ImproveStamina()
    {
        player_stats.max_stamina += 10;
        player_stats.stamina = player_stats.max_stamina;
    }
    public void ImproveMana()
    {
        player_stats.max_mana += 10;
        player_stats.mana = player_stats.max_mana;
    }
    public void ImproveSpeed()
    {
        player_stats.base_speed += 100;
        player_stats.actual_speed = player_stats.base_speed;
        player_stats.max_speed = player_stats.base_speed * 1.5f;
    }
}
