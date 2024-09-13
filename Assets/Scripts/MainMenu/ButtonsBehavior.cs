using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonsBehavior : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource song_player;
    public AudioClip song_to_play;

    // Método chamado quando o ponteiro entra na área do botão
    public void OnPointerEnter(PointerEventData eventData)
    {
        song_player.PlayOneShot(song_to_play);
    }

 
}
