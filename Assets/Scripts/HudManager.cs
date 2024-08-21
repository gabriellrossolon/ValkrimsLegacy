using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; private set; }

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

    public Slider player_hp_bar;
    public Slider player_stamina_bar;
    public Slider player_mana_bar;

    public GameObject damage_popup;

    private GameObject player_object;
    private EntityStats player_stats;

    public GameObject pausedgame_hud;
    private bool isPaused;

    public GameObject gameover_hud;

    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
        player_stats = GameObject.FindWithTag("Player").gameObject.GetComponent<EntityStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        PlayerHpHudBar();
        PlayerStaminaHudBar();
        PlayerManaHudBar();
    }

    private void PlayerHpHudBar()
    {
        player_hp_bar.maxValue = player_stats.max_hp;
        player_hp_bar.value = player_stats.hp;
    }

    private void PlayerStaminaHudBar()
    {
        player_stamina_bar.maxValue = player_stats.max_stamina;
        player_stamina_bar.value = player_stats.stamina;
    }

    private void PlayerManaHudBar()
    {
        player_mana_bar.maxValue = player_stats.max_mana;
        player_mana_bar.value = player_stats.mana;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("SampleScene");
        Debug.Log("Reloading Scene");
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
            pausedgame_hud.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pausedgame_hud.SetActive(false);
        }
    }

    public void QuiteGame()
    {
        //Ajustar pro Application Quit na build final.

        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
