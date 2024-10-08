using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    public Slider progressBar;
    public GameObject loadingScene;
    public GameObject buttons;
    public GameObject version;

    public float smoothTime = 0.5f; // Tempo para suavização

    private float currentProgress = 0f; // Progresso atual da barra
    private float velocity = 0f; // Velocidade de suavização

    public AudioMixer audioMixer; // Arraste o AudioMixer aqui via Inspector
    public Slider volumeSlider;   // Arraste o Slider da UI aqui via Inspector

    public GameObject options_hud;

    void Start()
    {
        // Define o valor inicial do slider (exemplo: volume 1 = 100%)
        volumeSlider.value = 1.0f;

        // Adiciona o listener para chamar o método sempre que o slider mudar
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.0001f)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
    }

    public void Play()
    {
        loadingScene.SetActive(true);
        buttons.SetActive(false);
        version.SetActive(false);

        StartCoroutine(LoadAsync("SampleScene"));
    }

    public void OpenCloseOptionsHud()
    {
        bool isActive = options_hud.activeSelf;
        options_hud.SetActive(!isActive);
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
