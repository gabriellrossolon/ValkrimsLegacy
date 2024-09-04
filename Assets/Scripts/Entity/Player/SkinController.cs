using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    private Animator anim;
    public bool using_armor_one;
    public bool using_armor_two;
    public int armor_equipped;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        CheckArmorUsed();
    }

    private void CheckArmorUsed()
    {
        anim.SetBool("usingArmor1", using_armor_one);
        anim.SetBool("usingArmor2", using_armor_two);
    }
}
