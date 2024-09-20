using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ShopItemBehavior : MonoBehaviour
{
    private EntityStats player_stats;

    public Weapon w_;

    public Text item_name;
    public Text item_value;
    public Image item_icon;
    public Text item_infos;
    public Button item_buy_btn;

    void Start()
    {
        player_stats = GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>();
        Setup(w_);

        item_buy_btn.onClick.RemoveAllListeners();
        item_buy_btn.onClick.AddListener(BuyItem);
    }

    private void Setup(Weapon w)
    {
        item_name.text = w.weapon_name;
        item_value.text = w.weapon_gold_value.ToString();
        item_icon.sprite = w.weapon_icon;
        item_infos.text = "Damage: " + w.weapon_damage.ToString() + "\nSpeed: " + w.weapon_atk_speed_type + "\nRange: " + w.weapon_range_type;

        if (player_stats.gold_carry < w.weapon_gold_value)
        {
            item_buy_btn.interactable = false;
        }
        else
        {
            item_buy_btn.interactable = true;
        }
    }

    private void BuyItem()
    {
        if (InventoryManager.Instance.inventory.All(weapon => weapon != null))
        {
            Debug.Log("Full Inventory");
            SoundManager.Instance.shop_soundSource.PlayOneShot(SoundManager.Instance.denied_buy_soundClip);
        }
        else
        {
            for (int i = 0; i < InventoryManager.Instance.inventory.Count; i++)
            {
                if (InventoryManager.Instance.inventory[i] == null)
                {
                    InventoryManager.Instance.inventory[i] = w_; // Adiciona no primeiro espaço vazio
                    player_stats.AddGold(w_.weapon_gold_value * -1f);
                    SoundManager.Instance.shop_soundSource.PlayOneShot(SoundManager.Instance.buy_soundClip);
                    Destroy(this.gameObject, 0.1f);
                    return; // Sai da função após adicionar o item
                }
            }
        }
    }
}
