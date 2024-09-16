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

    [Header("Status Bar")]
    public Slider player_hp_bar;
    public Slider player_stamina_bar;
    public Slider player_mana_bar;
    public Slider player_xp_bar;

    private GameObject player_object;
    private EntityStats player_stats;

    [Header("HUDs")]
    public GameObject pausedgame_hud;
    private bool isPaused;

    public GameObject gameover_hud;

    [Header("Popups")]
    public GameObject mana_warn;
    public GameObject stamina_warn;
    public GameObject cooldown_warn;
    public GameObject damage_popup;
    public GameObject collects_popup;

    [Header("Texts")]
    public Text level_text;
    public Text gold_text;

    [Header("Player Info Panel")]
    public GameObject player_info_panel;
    public Text max_hp_text;
    public Text attack_damage_text;
    public Text max_stamina_text;
    public Text max_mana_text;
    public Text speed_text;


    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
        player_stats = GameObject.FindWithTag("Player").gameObject.GetComponent<EntityStats>();

        level_text.text = player_stats.level.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        ShowPlayerHud();

        if(player_info_panel != null)
        {
            PlayerInfoPanel();

            if (Input.GetKeyDown(KeyCode.P))
            {
                bool isActive = player_info_panel.activeSelf;
                player_info_panel.SetActive(!isActive);
            }
        }
    }

    private void ShowPlayerHud()
    {
        PlayerHpHudBar();
        PlayerStaminaHudBar();
        PlayerManaHudBar();
        PlayerXpHudBar();
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

    private void PlayerXpHudBar()
    {
        player_xp_bar.maxValue = player_stats.xp_to_up;
        player_xp_bar.value = player_stats.xp;
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

    private void PlayerInfoPanel()
    {
        max_hp_text.text = player_stats.max_hp.ToString();
        attack_damage_text.text = player_stats.attack_damage.ToString();
        max_stamina_text.text = player_stats.max_stamina.ToString();
        max_mana_text.text = player_stats.max_mana.ToString();
        float speed_units = player_stats.base_speed / 1000;
        speed_text.text = speed_units.ToString();
    }

    public void QuiteGame()
    {
        //Ajustar pro Application Quit na build final.

        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
