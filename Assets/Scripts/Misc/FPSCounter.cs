using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText; // Arraste um objeto Text aqui no Inspector se quiser mostrar na tela

    private float deltaTime = 0.0f;

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f; // Suaviza o valor do deltaTime
        float fps = 1.0f / deltaTime; // Calcula o FPS

        // Exibe no console (opcional)
        //Debug.Log($"FPS: {fps:0.}");

        // Atualiza a UI, se houver um Text definido
        if (fpsText != null)
        {
            fpsText.text = $"FPS: {fps:0.}";
        }
    }
}
