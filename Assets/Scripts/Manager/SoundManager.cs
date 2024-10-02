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
    [Header("Ambient Sound")]
    public AudioSource ambient_soundSource;

    public AudioClip florest_ambient_AudioClip;
    public AudioClip dungeon_ambient_audioClip;
    public AudioClip cave_ambient_audioClip;

    [Header("Music Sound")]
    public AudioSource music_soundSource;

    public AudioClip peace_music_audioClip;
    public AudioClip dungeon_music_audioClip;

    [Header("Steps")]
    public AudioSource step_soundSource;
    public AudioSource step_run_soundSource;

    public AudioClip step_grass_audioClip;
    public AudioClip step_grass_run_audioClip;

    public AudioClip step_stone_audioClip;
    public AudioClip step_run_stone_audioClip;

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

    public AudioClip weaponCharge_audioClip;

    [Header("Potion")]
    public AudioSource potion_soundSource;
    public AudioClip potion_drinkClip;

    [Header("Drop Collect")]
    public AudioSource drop_soundSource;
    public AudioClip goldCollect_soundClip;

    [Header("Level XP Sound")]
    public AudioSource exp_soundSource;
    public AudioClip levelUp_soundClip;

    [Header("Shop Sound")]
    public AudioSource shop_soundSource;
    public AudioClip buy_soundClip;
    public AudioClip denied_buy_soundClip;

    [Header("Misc Button Sound")]
    public AudioSource button_soundSource;
    public AudioClip button_hover_soundClip;
    public AudioClip button_click_soundClip;

    [Header("Misc Sounds")]
    public AudioSource misc_soundSource;
    public AudioClip gameover_soundClip;

    [Header("Ambient Location")]
    public string player_ambient;

    public void SoundChecker()
    {
        if(player_ambient == "Florest")
        {
            ambient_soundSource.clip = florest_ambient_AudioClip;
            
            music_soundSource.clip = peace_music_audioClip;

            step_soundSource.clip = step_grass_audioClip;
            step_run_soundSource.clip = step_grass_run_audioClip;
        } 
        else if (player_ambient == "Dungeon1")
        {
            ambient_soundSource.clip = dungeon_ambient_audioClip;

            music_soundSource.clip = null;

            step_soundSource.clip = step_stone_audioClip;
            step_run_soundSource.clip = step_run_stone_audioClip;
        }
        else if(player_ambient == "Cave1")
        {
            ambient_soundSource.clip = cave_ambient_audioClip;

            music_soundSource.clip = null;

            step_soundSource.clip = step_stone_audioClip;
            step_run_soundSource.clip = step_run_stone_audioClip;
        }

        ambient_soundSource.loop = true;
        ambient_soundSource.Play();

        music_soundSource.loop = true;
        music_soundSource.Play();

        step_soundSource.loop = true;
        step_soundSource.Play();

        step_run_soundSource.loop = true;
        step_run_soundSource.Play();
    }

}
