using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Configuração de Luz")]
    public Light2D global_light;

    [Header("Configuração de Tempo")]
    public float realTimeToGameTimeRatio = 60f;

    [SerializeField] private float currentHour = 6f;
    [SerializeField] private float currentMinutes = 0f;
    public bool is_day = true;

    private float minutesCounter = 0f;
    public event Action OnEveryTenMinutes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateClockDisplay(); // Inicializa o relógio na HUD já com a hora atual
        StartCoroutine(TimeCycle());
    }

    IEnumerator TimeCycle()
    {
        while (true)
        {
            currentMinutes += (60f / realTimeToGameTimeRatio);

            if (currentMinutes >= 60f)
            {
                currentMinutes = 0f;
                currentHour += 1f;

                if (currentHour >= 24f)
                {
                    currentHour = 0f;
                }
            }

            UpdateDayNightCycle();

            minutesCounter += 1f / realTimeToGameTimeRatio * 60f;

            if (minutesCounter >= 10f)
            {
                minutesCounter = 0f;
                UpdateClockDisplay();
                OnEveryTenMinutes?.Invoke();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void UpdateDayNightCycle()
    {
        if (currentHour >= 6f && currentHour <= 17f)
        {
            is_day = true;
        }
        else
        {
            is_day = false;
        }

        UpdateLighting();
    }

    void UpdateLighting()
    {
        if (is_day)
        {
            global_light.color = Color.white;
        }
        else
        {
            global_light.color = new Color(0.082f, 0.176f, 0.263f);
        }
    }

    void UpdateClockDisplay()
    {
        string formattedHour = currentHour.ToString("00");
        string formattedMinutes = currentMinutes.ToString("00");
        HudManager.Instance.clock.text = formattedHour + ":" + formattedMinutes;
    }

    public float GetCurrentHour()
    {
        return currentHour;
    }

    public float GetCurrentMinutes()
    {
        return currentMinutes;
    }
}