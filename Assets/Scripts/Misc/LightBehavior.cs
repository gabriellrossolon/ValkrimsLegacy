using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightBehavior : MonoBehaviour
{
    private Light2D object_light;

    void Start()
    {
        object_light = gameObject.GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        if(TimeManager.Instance.is_day == true)
        {
            object_light.enabled = false;
        }
        else
        {
            object_light.enabled = true;
        }
    }
}
