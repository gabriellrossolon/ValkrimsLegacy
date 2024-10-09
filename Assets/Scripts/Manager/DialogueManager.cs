using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private GameObject[] npc_objects;


    public GameObject dialogue_box;
    public Image char_image;
    public Text char_name;
    public Text dialogue_text;
    public GameObject dialogue_option_one;
    public GameObject dialogue_option_two;
    public GameObject dialogue_option_three;

    public GameObject hotbar;
    public GameObject buttons;

    public Text shop_panel_txt;
    public GameObject shop_panel;
    public GameObject shop_item_holder;
    public GameObject shop_item_sell_holder;
    public GameObject shop_item;

    private void Start()
    {
        npc_objects = GameObject.FindGameObjectsWithTag("NPC");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shop_panel.activeSelf)
            {
                CloseShop();
            }
            else if (dialogue_box.activeSelf)
            {
                foreach (GameObject npc in npc_objects)
                {
                    npc.GetComponent<CircleCollider2D>().enabled = false;
                    npc.GetComponent<CircleCollider2D>().enabled = true;
                }
            }
        }
    }

    public void CloseShop()
    {
        GameObject[] shopItems = GameObject.FindGameObjectsWithTag("ShopItem");
        foreach (GameObject shopItem in shopItems)
        {
            Destroy(shopItem);
        }

        GameObject[] shopSellItems = GameObject.FindGameObjectsWithTag("InventorySlot");
        foreach (GameObject shopSellItem in shopSellItems)
        {
            Destroy(shopSellItem);
        }
        InventoryManager.Instance.RefreshInventory();

        shop_panel.SetActive(false);
    }
}
