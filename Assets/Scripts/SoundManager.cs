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
    public AudioSource impact_soundSource;//Quem define o audioCLip do Slash é o ScriptableObject da arma.
    //Quem define o Impact audioClip dos Projectiles é o proprio Objeto Projectile.
    public AudioClip impact_soundClip;

    [Header("AttackSlash")]
    public AudioSource attack_soundSource;
    public AudioClip weaponAttack_audioClip; //Quem define o audioCLip do Slash é o ScriptableObject da arma.

    [Header("Potion")]
    public AudioSource potion_soundSource;
    public AudioClip potion_drinkClip;




}
