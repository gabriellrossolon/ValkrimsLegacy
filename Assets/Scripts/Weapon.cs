using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Basic Status")]
    public float weapon_damage;
    public float weapon_attack_speed;

    [Header("Weapon Icon")]
    public Sprite weapon_icon;

    [Header("Type of Weapon")]
    public bool sword;
    public bool staff;
    public bool bow;


    [Header("Impact Animation")]
    public GameObject weapon_impact_anim;

    [Header("Only if Ranged")]
    public GameObject ranged_projectile;
    public float projectile_speed;

}
