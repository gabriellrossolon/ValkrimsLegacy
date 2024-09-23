using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance { get; private set; }

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

    public Light2D global_light;
    public bool is_day;

    void Update()
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
}
