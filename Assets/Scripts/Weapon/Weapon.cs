using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Weapon Info")]
    public string weapon_name;
    public string weapon_atk_speed_type;
    public string weapon_range_type;
    public float weapon_gold_value;

    [Header("Only if Melee")]
    public bool great_sword;
    public bool dagger_sword;
    public bool normal_sword;

    [Header("Basic Status")]
    public float weapon_damage;
    public float weapon_attack_speed;

    [Header("Weapon Icon")]
    public Sprite weapon_icon;

    [Header("Type of Weapon")]
    public bool sword;
    public bool staff;
    public bool bow;

    [Header("Start Attack Sound")]
    public AudioClip attack_soundClip_one;
    public AudioClip attack_soundClip_two;
    public AudioClip attack_soundClip_three;

    [Header("Impact Animation")]
    public GameObject weapon_impact_anim;

    [Header("Only if Ranged")]
    public GameObject ranged_projectile;
    public float projectile_speed;

    [Header("Impact Sound")]
    public AudioClip impact_soundClip;

}
