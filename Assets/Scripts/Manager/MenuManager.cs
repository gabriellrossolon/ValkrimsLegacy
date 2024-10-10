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
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public GameObject options_hud;

    public float smoothTime = 0.5f;
    float currentProgress = 0f;
    float velocity = 0f;

    void Start()
    {
        volumeSlider.value = 1.0f;
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20);
    }

    public void Play()
    {
        loadingScene.SetActive(true);
        buttons.SetActive(false);
        version.SetActive(false);
        StartCoroutine(LoadAsync("ClassMenu"));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            currentProgress = Mathf.SmoothDamp(currentProgress, operation.progress, ref velocity, smoothTime);
            progressBar.value = currentProgress;
            yield return null;
        }

        while (currentProgress < 0.99f)
        {
            currentProgress = Mathf.SmoothDamp(currentProgress, 1f, ref velocity, smoothTime);
            progressBar.value = currentProgress;
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    public void OpenCloseOptionsHud()
    {
        options_hud.SetActive(!options_hud.activeSelf);
    }

    public void QuiteGame()
    {
        Application.Quit();
    }
}
