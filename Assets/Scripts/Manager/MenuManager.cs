using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Slider progressBar;
    public GameObject loadingScene;
    public GameObject buttons;
    public GameObject version;

    public float smoothTime = 0.5f; // Tempo para suavização

    private float currentProgress = 0f; // Progresso atual da barra
    private float velocity = 0f; // Velocidade de suavização

    public void Play()
    {
        loadingScene.SetActive(true);
        buttons.SetActive(false);
        version.SetActive(false);

        StartCoroutine(LoadAsync("SampleScene"));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            currentProgress = Mathf.SmoothDamp(currentProgress, operation.progress, ref velocity, smoothTime);
            if (progressBar != null)
            {
                progressBar.value = currentProgress;
            }
            yield return null;
        }
        if (progressBar != null)
        {
            progressBar.value = 1f;
        }
    }
}
