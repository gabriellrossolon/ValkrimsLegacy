using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CullingSystem : MonoBehaviour
{
 private Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        float margin = 1f; // Ajuste este valor para aumentar ou diminuir a margem

        bool isVisible =
            screenPoint.x >= -margin && screenPoint.x <= 1 + margin &&
            screenPoint.y >= -margin && screenPoint.y <= 1 + margin;

        gameObject.GetComponent<SpriteRenderer>().enabled = isVisible;
        if(gameObject.GetComponent<ShadowCaster2D>() != null)
        {
            gameObject.GetComponent<ShadowCaster2D>().enabled = isVisible;
        }
        if (gameObject.GetComponentInChildren<Animator>() != null)
        {
            gameObject.GetComponentInChildren<Animator>().enabled = isVisible;
        }

        if (gameObject.GetComponentInChildren<Light2D>() != null)
        {
            if (DayManager.Instance.is_day == true && isVisible == false)
            {
                gameObject.GetComponentInChildren<Light2D>().enabled = false;
            } 
            else if (DayManager.Instance.is_day == true && isVisible == true)
            {
                gameObject.GetComponentInChildren<Light2D>().enabled = false;
            } 
            else if (DayManager.Instance.is_day == false && isVisible == true)
            {
                gameObject.GetComponentInChildren<Light2D>().enabled = true;
            } 
            else if (DayManager.Instance.is_day == false && isVisible == false)
            {
                gameObject.GetComponentInChildren<Light2D>().enabled = false;
            }
        }
    }
}
