using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon")]
public class Weapon : ScriptableObject
{
    public float weapon_damage;
    public float weapon_attack_speed;
    public Sprite weapon_icon;
    public bool sword;
    public bool staff;
    public bool bow;

}
