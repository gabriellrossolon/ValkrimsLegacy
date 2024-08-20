using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

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

    public List<Weapon> inventory;

    public GameObject inv_background;
    public GameObject inv_slot;
    private int selected_inv_slot = 0;

    private EntityStats player_stats;

    public GameObject weapon_in_hands;

    void Start()
    {
        player_stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        SelectWeapon(1);
    }


    void Update()
    {
        InventorySelection();
    }

    private void RefreshInventory()
    {
        GameObject[] destroy_slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        foreach (GameObject slot in destroy_slots) { Destroy(slot); }

        int hotkey = 1; 

        foreach(Weapon w in inventory)
        {
            GameObject slot_instance = Instantiate(inv_slot, inv_background.transform);

            if(w == null)
            {
                Image icon_image = slot_instance.transform.Find("Icon").GetComponent<Image>();
                icon_image.enabled = false;
            }
            else
            {
                Image icon_image = slot_instance.transform.Find("Icon").GetComponent<Image>();
                icon_image.sprite = w.weapon_icon;
                icon_image.enabled = true;

                slot_instance.GetComponent<Image>().color = Color.white;

                if (selected_inv_slot == hotkey)
                {
                    slot_instance.GetComponent<Image>().color = Color.red;
                }
            }

            slot_instance.GetComponentInChildren<Text>().text = hotkey.ToString();
            hotkey++;
        }
    }

    private void SelectWeapon(int hotkey)
    {
        Weapon selected_weapon = inventory[hotkey - 1];
        player_stats.attack_damage = selected_weapon.weapon_damage;
        player_stats.attack_speed = selected_weapon.weapon_attack_speed;

        selected_inv_slot = hotkey;
        weapon_in_hands.GetComponent<SpriteRenderer>().sprite = inventory[selected_inv_slot - 1].weapon_icon;
        RefreshInventory();
    }

    private void InventorySelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectWeapon(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectWeapon(5);
        }
    }
}
