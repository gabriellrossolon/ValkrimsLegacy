using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

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

    [Header("Steps")]
    public AudioSource step_grass;

    [Header("Dash")]
    public AudioSource dash_soundSource;
    public AudioClip dash_soundClip;

    [Header("AttackImpact")]
    public AudioSource impact_soundSource;
    public AudioClip impact_soundClip; //Quem define o audioCLip do Slash é o ScriptableObject da arma.

    [Header("AttackSlash")]
    public AudioSource attack_soundSource;
    public AudioClip weaponAttack_audioClip_one; //Quem define o audioCLip do Slash é o ScriptableObject da arma.
    public AudioClip weaponAttack_audioClip_two;
    public AudioClip weaponAttack_audioClip_three;

    [Header("Potion")]
    public AudioSource potion_soundSource;
    public AudioClip potion_drinkClip;

    [Header("Drop Collect")]
    public AudioSource drop_soundSource;
    public AudioClip goldCollect_soundClip;



}
