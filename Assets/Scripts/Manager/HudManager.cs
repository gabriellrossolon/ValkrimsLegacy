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
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private GameObject player_object;
    private EntityStats player_stats;

    [Header("Status Bar")]
    public GameObject player_bars_go;
    public Slider player_hp_bar;
    public Slider player_stamina_bar;
    public Slider player_mana_bar;
    public Slider player_xp_bar;

    [Header("Misc HUDs")]
    public GameObject pausedgame_hud;
    private bool isPaused;

    public GameObject gameover_hud;

    public Text clock;

    public GameObject options_hud;

    [Header("Popups")]
    public GameObject mana_warn;
    public GameObject stamina_warn;
    public GameObject cooldown_warn;
    public GameObject damage_popup;
    public GameObject collects_popup;
    public GameObject levelUp_popup;

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
    public Text up_points_text;

    [Header("Map")]
    public GameObject map;
    [HideInInspector] public bool map_opened = false;

    [Header("Skills CDR")]
    public Slider dash_slider;
    public Slider dashAttack_slider;
    public Slider strongAttack_slider;

    [Header("Hud Key Button")]
    public GameObject buttons_go;

    [Header("Quests Panel")]
    public GameObject quests_panel;
    private bool quests_panel_opened;
    public GameObject quests_banner;
    public GameObject quest_panel_one;
    public GameObject quest_panel_two;

    private GameObject[] attribute_buttons;
    public List<Image> button_images;


    void Start()
    {      
        player_object = GameObject.FindGameObjectWithTag("Player");
        player_stats = GameObject.FindWithTag("Player").gameObject.GetComponent<EntityStats>();

        level_text.text = player_stats.level.ToString();

        attribute_buttons = GameObject.FindGameObjectsWithTag("UpButton");
        foreach (GameObject button in attribute_buttons)
        {
            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                button_images.Add(buttonImage); // Armazena o Image na lista
                buttonImage.enabled = false;  // Desabilita o componente Image
            }
        }
        player_info_panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!DialogueManager.Instance.shop_panel.activeSelf && !DialogueManager.Instance.dialogue_box.activeSelf 
                && !quests_panel.activeSelf && !map_opened && !player_info_panel.activeSelf && !gameover_hud.activeSelf)
            {
                PauseGame();
            }

            if (quests_panel.activeSelf)
            {
                OpenCloseQuestsJournal();
            }

            if (map_opened)
            {
                OpenMap();
            }

            if (player_info_panel.activeSelf)
            {
                OpenClosePlayerInfoPanel();
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            OpenCloseQuestsJournal();
        }

        ShowPlayerHud();

        if(player_info_panel != null)
        {
            PlayerInfoPanel();

            if (Input.GetKeyDown(KeyCode.P))
            {
                OpenClosePlayerInfoPanel();
                ActiveAttributeUpButtons();
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OpenMap();
        }

        if(player_stats.level_up_points > 0)
        {
            buttons_go.gameObject.GetComponent<Animator>().SetBool("isWarning", true);
        } 
        else
        {
            buttons_go.gameObject.GetComponent<Animator>().SetBool("isWarning", false);
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

    public void OpenClosePlayerInfoPanel()
    {
        bool isActive = player_info_panel.activeSelf;
        player_info_panel.SetActive(!isActive);
        ControlPlayerCanAttack(!isActive);
    }

    private void PlayerInfoPanel()
    {
        max_hp_text.text = player_stats.max_hp.ToString();
        attack_damage_text.text = player_stats.base_attack_damage.ToString() + " + " + player_stats.attack_damage.ToString();
        max_stamina_text.text = player_stats.max_stamina.ToString();
        max_mana_text.text = player_stats.max_mana.ToString();
        float speed_units = player_stats.base_speed / 1000;
        speed_text.text = speed_units.ToString();
    }

    public void ActiveAttributeUpButtons()
    {
        up_points_text.text = player_stats.level_up_points.ToString();
        if (player_stats.level_up_points > 0)
        {
            foreach (Image image in button_images)
            {
                image.enabled = true;
            }
        }
        else
        {
            foreach (Image image in button_images)
            {
                image.enabled = false;
            }
        }
    }

    public void OpenCloseQuestsJournal()
    {
        bool isActive = quests_panel.activeSelf;
        quests_panel.SetActive(!isActive);
        ControlPlayerCanAttack(!isActive);
    }

    private void OpenMap()
    {
        if (!map_opened)
        {
            map.GetComponent<Animator>().SetTrigger("openMap");
            map_opened = true;
        }
        else if (map_opened)
        {
            map.GetComponent<Animator>().SetTrigger("closeMap");
            map_opened = false;
        }

        ControlPlayerCanAttack(map_opened);
    }

    public void OpenCloseOptionsHud()
    {
        bool isActive = options_hud.activeSelf;
        options_hud.SetActive(!isActive);
        ControlPlayerCanAttack(!isActive);
    }

    private void ControlPlayerCanAttack(bool value)
    {
        bool window_opened = value;

        if (window_opened == true)
        {
            if (player_object.GetComponent<PlayerMeleeAttack>().enabled)
            {
                player_object.GetComponent<PlayerMeleeAttack>().enabled = false;
            }
            else if (player_object.GetComponent<PlayerRangedAttack>().enabled)
            {
                player_object.GetComponent<PlayerRangedAttack>().enabled = false;
            }
        }
        else
        {
            if (InventoryManager.Instance.weapon_in_hands == InventoryManager.Instance.weapon_sword)
            {
                player_object.GetComponent<PlayerMeleeAttack>().enabled = true;
            }
            else if (InventoryManager.Instance.weapon_in_hands == InventoryManager.Instance.weapon_staff || InventoryManager.Instance.weapon_bow)
            {
                player_object.GetComponent<PlayerRangedAttack>().enabled = true;
            }

            InventoryManager.Instance.RefreshInventory();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        ControlPlayerCanAttack(isPaused);
        if (isPaused)
        {
            SoundManager.Instance.ambient_soundSource.Pause();
            //SoundManager.Instance.music_soundSource.Pause();
            Time.timeScale = 0;
            pausedgame_hud.SetActive(true);
        }
        else
        {
            SoundManager.Instance.ambient_soundSource.Play();
            //SoundManager.Instance.music_soundSource.Play();
            Time.timeScale = 1;
            pausedgame_hud.SetActive(false);
        }
    }

    public void PlayAgain()
    {
        gameover_hud.SetActive(false);
        player_stats.hp = player_stats.max_hp;
        player_stats.stamina = player_stats.max_stamina;
        player_stats.mana = player_stats.max_mana;
        player_object.transform.position = new Vector3(-95.89f, 76.02f, 0);
        SoundManager.Instance.music_soundSource.Play();
        SoundManager.Instance.step_soundSource.Play();

        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void QuiteGame()
    {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
}
