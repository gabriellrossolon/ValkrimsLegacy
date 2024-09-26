using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CullingSystem : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private ShadowCaster2D shadowCaster;
    private Animator animator;
    private SortingOrder sortingOrder;
    private Light2D light2D;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        shadowCaster = GetComponent<ShadowCaster2D>();
        animator = GetComponentInChildren<Animator>();
        sortingOrder = GetComponentInChildren<SortingOrder>();
        light2D = GetComponentInChildren<Light2D>();
    }

    void Update()
    {
        CullyingVerify();
    }

    private void CullyingVerify()
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(transform.position);
        float margin = 1f; // Ajuste este valor para aumentar ou diminuir a margem

        bool isVisible =
            screenPoint.x >= -margin && screenPoint.x <= 1 + margin &&
            screenPoint.y >= -margin && screenPoint.y <= 1 + margin;

        // Ativar ou desativar o SpriteRenderer
        spriteRenderer.enabled = isVisible;

        // Ativar ou desativar o ShadowCaster2D
        if (shadowCaster != null)
        {
            shadowCaster.enabled = isVisible;
        }

        // Ativar ou desativar o Animator
        if (animator != null)
        {
            animator.enabled = isVisible;
        }

        // Ativar ou desativar o SortingOrder
        if (sortingOrder != null)
        {
            sortingOrder.enabled = isVisible;
        }

        // Lógica para Light2D
        if (light2D != null)
        {
            if (DayManager.Instance.is_day)
            {
                light2D.enabled = false; // Desativa durante o dia
            }
            else
            {
                light2D.enabled = isVisible; // Ativa ou desativa com base na visibilidade
            }
        }
    }
}
