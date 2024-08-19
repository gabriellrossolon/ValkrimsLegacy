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
    public GameObject damage_popup;

    private GameObject player_object;

    public GameObject pausedgame_hud;
    private bool isPaused;

    public GameObject gameover_hud;

    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        PlayerHpHud();
    }

    void PlayerHpHud()
    {
        player_hp_bar.maxValue = player_object.gameObject.GetComponent<EntityStats>().max_hp;
        player_hp_bar.value = player_object.gameObject.GetComponent<EntityStats>().hp;
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
