using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ClassMenuBehavior : MonoBehaviour
{
    public Slider progressBar;
    public GameObject loadingScene;
    public float smoothTime = 0.5f; // Tempo para suavização
    private float currentProgress = 0f; // Progresso atual da barra
    private float velocity = 0f; // Velocidade de suavização

    public void SelectClass()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Transform parentTransform = clickedButton.transform.parent;

        foreach (Transform sibling in parentTransform)
        {
            Text buttonText = sibling.GetComponent<Text>();

            if (buttonText != null && sibling.gameObject != clickedButton)
            {
                if (buttonText.text == "Warrior")
                {
                    ClassManager.Instance.choosedClass = "Warrior";
                }
                else if (buttonText.text == "Mage")
                {
                    ClassManager.Instance.choosedClass = "Mage";
                }
                else if (buttonText.text == "Thief")
                {
                    ClassManager.Instance.choosedClass = "Thief";
                }
            }
        }

        loadingScene.SetActive(true);
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
