using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropItemBehavior : MonoBehaviour
{
    [Header("If Gold")]
    public float gold_to_give;

    [Header("Drop Icon")]
    public Sprite item_icon;

    private string drop_name;

    private AudioClip collect_sound;

    private void Start()
    {
        Destroy(this.gameObject, 300f);
        collect_sound = SoundManager.Instance.goldCollect_soundClip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("GoldDrop"))
        {
            drop_name = "Gold Coin(s)";
            if (collision.gameObject.CompareTag("Player"))
            {
                //SoundManager.Instance.drop_soundSource.PlayOneShot(collect_sound);
                collision.gameObject.GetComponent<EntityStats>().AddGold(gold_to_give);
                Destroy(this.gameObject);

                GameObject collect_popup = Instantiate(HudManager.Instance.collects_popup);
                collect_popup.GetComponentInChildren<Text>().text = $"{gold_to_give} {drop_name}";
                collect_popup.GetComponentInChildren<Image>().sprite = this.item_icon;
                Destroy(collect_popup, 1f);
            }
        }
    }
}
