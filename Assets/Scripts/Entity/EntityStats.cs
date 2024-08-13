using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float base_speed;
    public float actual_speed;
    public float max_speed;
    [HideInInspector] public float original_speed;

    public float attack_damage;

    public float max_hp;
    public float hp;

    public float attack_speed;

    void Start()
    {
        actual_speed = base_speed;
        original_speed = base_speed;
        hp = max_hp;
    }


    void Update()
    {
        
    }

    void Death()
    {
        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void RemoveHp(float hp_to_remove)
    {
        hp -= hp_to_remove;
        Death();
    }
}
