using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    [Header("Inventory Itens")]
    public List<Weapon> inventory;
    public Potion potion;

    [Header("Potion Hud")]
    public GameObject potion_slot;
    public int potion_quantity;
    public Text potion_quantity_text;

    [Header("Inventory Hud")]
    public GameObject inv_background;
    public GameObject inv_slot;
    private int selected_inv_slot = 0;

    private EntityStats player_stats;
    private GameObject player_object;
    private PlayerRangedAttack player_ranged_attack;
    private PlayerMeleeAttack player_melee_attack;

    [Header("Weapons Object")]
    public GameObject weapon_sword;
    public GameObject weapon_staff;
    public GameObject weapon_bow;

    public GameObject weapon_in_hands;
    public GameObject weapon_parent;


    private bool options_active = false;

    void Start()
    {
        player_stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        player_object = GameObject.FindGameObjectWithTag("Player");

        player_ranged_attack = player_object.GetComponent<PlayerRangedAttack>();
        player_melee_attack = player_object.GetComponent<PlayerMeleeAttack>();

        SelectWeapon(1);
    }


    void Update()
    {
        InventorySelection();
    }

    public void RefreshInventory()
    {
        //Slot da Poção
        Image potion_icon_image = potion_slot.transform.Find("Icon").GetComponent<Image>();
        if (potion != null)
        {
            potion_icon_image.enabled = true;
            potion_icon_image.sprite = potion.potion_icon;
            potion_quantity_text.text = potion_quantity.ToString();
        }
        else
        {
            potion_icon_image.enabled = false;
        }

        player_melee_attack.attack_step = 1;


        //Slots das Armas
        GameObject[] destroy_slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        foreach (GameObject slot in destroy_slots) { Destroy(slot); }

        int hotkey = 1;

        foreach (Weapon w in inventory)
        {
            GameObject slot_instance = Instantiate(inv_slot, inv_background.transform);

            if (w == null)
            {
                Image icon_image = slot_instance.transform.Find("Icon").GetComponent<Image>();
                icon_image.enabled = false;

                slot_instance.GetComponent<Image>().color = Color.white;
                if (selected_inv_slot == hotkey)
                {
                    slot_instance.GetComponent<Image>().color = Color.red;
                }
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

                slot_instance.transform.Find("Icon").GetComponent<Button>().onClick.RemoveAllListeners();
                slot_instance.transform.Find("Icon").GetComponent<Button>().onClick.AddListener(ItemOptions);

                slot_instance.transform.Find("Options").GetComponent<Button>().onClick.RemoveAllListeners();
                slot_instance.transform.Find("Options").GetComponent<Button>().onClick.AddListener(DiscardItem);

            }

            slot_instance.GetComponentInChildren<Text>().text = hotkey.ToString();
            hotkey++;
        }
    }

    private void SelectWeapon(int hotkey)
    {
        Weapon selected_weapon = inventory[hotkey - 1];

        if (selected_weapon != null)
        {
            VerifyWeaponActive(selected_weapon);

            player_stats.attack_damage = selected_weapon.weapon_damage;
            player_stats.attack_speed = selected_weapon.weapon_attack_speed;

            SoundManager.Instance.weaponAttack_audioClip_one = selected_weapon.attack_soundClip_one;
            SoundManager.Instance.weaponAttack_audioClip_two = selected_weapon.attack_soundClip_two;
            SoundManager.Instance.weaponAttack_audioClip_three = selected_weapon.attack_soundClip_three;
            SoundManager.Instance.impact_soundClip = selected_weapon.impact_soundClip;

            selected_inv_slot = hotkey;
            weapon_in_hands.GetComponent<SpriteRenderer>().sprite = inventory[selected_inv_slot - 1].weapon_icon;
        }
        else
        {
            VerifyWeaponActive(selected_weapon);
            selected_inv_slot = hotkey;
        }

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            UsePotion();
        }
    }

    //Gerenciamento de Mudança das armas Sword/Staff/Bow

    private void VerifyWeaponActive(Weapon selected_weapon)
    {
        WeaponRotateController rotate_controller = weapon_parent.GetComponent<WeaponRotateController>();

        if (selected_weapon != null)
        {
            if (selected_weapon.sword == true)
            {
                weapon_in_hands = weapon_sword;
                weapon_sword.SetActive(true);
                weapon_staff.SetActive(false);
                weapon_bow.SetActive(false);
                rotate_controller.weapon = rotate_controller.sword_transform;
                player_melee_attack.enabled = true;
                player_ranged_attack.enabled = false;
                player_ranged_attack.using_staff = false; player_ranged_attack.using_bow = false;
                player_object.GetComponentInChildren<SwordBehavior>().impact_fx = selected_weapon.weapon_impact_anim;

                weapon_sword.GetComponent<SwordBehavior>().great_sword = selected_weapon.great_sword;
                weapon_sword.GetComponent<SwordBehavior>().normal_sword = selected_weapon.normal_sword;
                weapon_sword.GetComponent<SwordBehavior>().dagger_sword = selected_weapon.dagger_sword;
            }
            else if (selected_weapon.staff == true)
            {
                weapon_in_hands = weapon_staff;
                weapon_sword.SetActive(false);
                weapon_staff.SetActive(true);
                weapon_bow.SetActive(false);
                rotate_controller.weapon = rotate_controller.staff_transform;
                player_melee_attack.enabled = false;
                player_ranged_attack.enabled = true;
                if (selected_weapon.ranged_projectile != null)
                {
                    player_object.GetComponent<PlayerRangedAttack>().projectile_selected = selected_weapon.ranged_projectile;
                    selected_weapon.ranged_projectile.gameObject.GetComponent<ProjectileBehavior>().projectile_speed = selected_weapon.projectile_speed;
                }
                player_ranged_attack.using_staff = true; player_ranged_attack.using_bow = false;
                selected_weapon.ranged_projectile.gameObject.GetComponent<ProjectileBehavior>().impact_fx = selected_weapon.weapon_impact_anim;
            }
            else if (selected_weapon.bow == true)
            {
                weapon_in_hands = weapon_bow;
                weapon_sword.SetActive(false);
                weapon_staff.SetActive(false);
                weapon_bow.SetActive(true);
                rotate_controller.weapon = rotate_controller.bow_transform;
                player_melee_attack.enabled = false;
                player_ranged_attack.enabled = true;
                if (selected_weapon.ranged_projectile != null)
                {
                    player_object.GetComponent<PlayerRangedAttack>().projectile_selected = selected_weapon.ranged_projectile;
                    selected_weapon.ranged_projectile.gameObject.GetComponent<ProjectileBehavior>().projectile_speed = selected_weapon.projectile_speed;
                }
                player_ranged_attack.using_staff = false; player_ranged_attack.using_bow = true;
                selected_weapon.ranged_projectile.gameObject.GetComponent<ProjectileBehavior>().impact_fx = selected_weapon.weapon_impact_anim;
            }
        }
        else
        {
            weapon_in_hands = null;
            weapon_sword.SetActive(false);
            weapon_staff.SetActive(false);
            weapon_bow.SetActive(false);
            rotate_controller.weapon = rotate_controller.sword_transform;
            player_melee_attack.enabled = false;
            player_ranged_attack.enabled = false;
            player_ranged_attack.using_staff = false; player_ranged_attack.using_bow = false;
        }
    }

    private void UsePotion()
    {
        if (potion != null && potion_quantity > 0)
        {
            SoundManager.Instance.potion_soundSource.PlayOneShot(SoundManager.Instance.potion_drinkClip);
            player_stats.AddHp(potion.hp_added);
            player_stats.AddMana(potion.mana_added);
            player_stats.AddStamina(potion.stamina_added);

            potion_quantity -= 1;

            RefreshInventory();
        }
        else
        {
            Debug.Log("Sem poções");
        }
    }

    public void ItemOptions()
    {
        options_active = !options_active;

        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;

        Transform parentTransform = clickedButton.transform.parent;

        Transform optionsTransform = parentTransform.Find("Options");
        optionsTransform.GetComponent<Image>().enabled = options_active;
        optionsTransform.GetComponent<Button>().enabled = options_active;
        optionsTransform.GetComponentInChildren<Text>().enabled = options_active;

        optionsTransform.gameObject.SetActive(options_active);
    }

    public void DiscardItem()
    {
        if (inventory.Count > 0) // Verifica se a lista não está vazia
        {
            //inventory[0] = null;
        }
        else
        {
            Debug.Log("O inventário está vazio, não há itens para descartar.");
        }

        RefreshInventory();
    }
}
