using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBehavior : MonoBehaviour
{
    private Image button_image;

    private bool dialogue_actived = false;
    private bool is_player_in_range = false;

    void Start()
    {
        button_image = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (is_player_in_range && Input.GetKeyDown(KeyCode.E) && !dialogue_actived)
        {
            Debug.Log("Startou Dialogo");
            dialogue_actived = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button_image.enabled = true;
            is_player_in_range = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button_image.enabled = false;
            dialogue_actived = false;
            is_player_in_range = false;
        }
    }
}
