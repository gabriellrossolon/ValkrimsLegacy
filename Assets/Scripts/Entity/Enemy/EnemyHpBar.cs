using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    private Slider hp_slider;
    private EntityStats entity_stats;

    void Start()
    {
        hp_slider = GetComponentInChildren<Slider>();
        entity_stats = GetComponentInParent<EntityStats>();
    }


    void Update()
    {
        hp_slider.maxValue = entity_stats.max_hp;
        hp_slider.value = entity_stats.hp;
    }
}

