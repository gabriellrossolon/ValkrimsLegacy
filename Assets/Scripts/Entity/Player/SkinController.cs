using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : MonoBehaviour
{
    private GameObject player_object;
    private Animator anim;

    [Header("Head")]
    public bool using_hair_1;
    public bool using_hat_1;

    [Header("Body")]
    public bool using_armor_one;
    public bool using_armor_two;

    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
        anim = player_object.GetComponent<Animator>();
    }

    void Update()
    {
        CheckArmorUsed();
    }

    private void CheckArmorUsed()
    {
        anim.SetBool("usingArmor1", using_armor_one);
        anim.SetBool("usingArmor2", using_armor_two);
        anim.SetBool("usingHair1", using_hair_1);
        anim.SetBool("usingHat1", using_hat_1);
    }
}
