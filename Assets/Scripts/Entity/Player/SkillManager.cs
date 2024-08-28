using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }
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

    //=====//=====//=====//=====//=====//=====//=====//=====//>>SKILLS MELEE<<//=====//=====//=====//=====//=====//=====//=====//=====//
    //Basic Attacks

    [Header("Power Cutter")]
    public bool can_use_powerCutter;
    public float powerCutter_damageMultiplier;

    [Header("Dash")]
    public bool can_use_dashSkill;

    [Header("Dash Attacks")]
    public bool can_use_bladeDance;
    public float bladeDance_damageMultiplier;


    //=====//=====//=====//=====//=====//=====//=====//=====//>>SKILLS RANGED<<//=====//=====//=====//=====//=====//=====//=====//=====//
}
