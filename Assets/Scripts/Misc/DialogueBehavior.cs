using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueBehavior : MonoBehaviour
{
    private Image button_image;

    public bool dialogue_actived = false;
    private int currentDialogueIndex = 0;

    public bool is_player_in_range = false;

    public Sprite npc_char_img;
    public string npc_name;

    private Coroutine dialogueCoroutine; // Armazena a referência da coroutine
    private bool isDialogueComplete = false; // Controla se o diálogo está completo

    [TextArea(3, 10)]
    public List<string> dialogues = new List<string>();

    [Header("Quest Controll")]
    public bool quester_npc;
    public string quest_to_active;
    public bool quest_gived;

    [Header("Merchant Controll")]
    public bool merchant_npc;
    public List<Weapon> weapons_sold;
    private List<Weapon> selectedWeapons = new List<Weapon>();

    void Start()
    {
        button_image = GetComponentInChildren<Image>();

        SortItensForSell();
    }

    private void Update()
    {
        StartDialogue();
        NextDialogue();

        if (this.gameObject.GetComponent<NPCMovement>() != null)
        {
            if (dialogue_actived)
            {
                this.gameObject.GetComponent<NPCMovement>().speed = 0f;
            }
            else if (!dialogue_actived)
            {
                this.gameObject.GetComponent<NPCMovement>().speed = 2f;
            }
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<PlayerMeleeAttack>().enabled)
            {
                collision.GetComponent<PlayerMeleeAttack>().enabled = false;
            }
            else if (collision.GetComponent<PlayerRangedAttack>().enabled)
            {
                collision.GetComponent<PlayerRangedAttack>().enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            button_image.enabled = false;
            dialogue_actived = false;
            is_player_in_range = false;

            DialogueManager.Instance.dialogue_box.SetActive(false);
            dialogue_actived = false;

            DialogueManager.Instance.hotbar.SetActive(true);
            DialogueManager.Instance.buttons.SetActive(true);

            DialogueManager.Instance.dialogue_option_one.SetActive(false);
            DialogueManager.Instance.dialogue_option_two.SetActive(false);
            DialogueManager.Instance.dialogue_option_three.SetActive(false);

            DialogueManager.Instance.CloseShop();

            if (InventoryManager.Instance.weapon_in_hands == InventoryManager.Instance.weapon_sword)
            {
                collision.GetComponent<PlayerMeleeAttack>().enabled = true;
            } 
            else if (InventoryManager.Instance.weapon_in_hands == InventoryManager.Instance.weapon_staff || InventoryManager.Instance.weapon_bow)
            {
                collision.GetComponent<PlayerRangedAttack>().enabled = true;
            }

            InventoryManager.Instance.RefreshInventory();
        }
    }

    private void StartDialogue()
    {
        if (is_player_in_range && Input.GetKeyDown(KeyCode.E) && !dialogue_actived)
        {
            dialogue_actived = true;
            currentDialogueIndex = 0;

            DialogueManager.Instance.hotbar.SetActive(false);
            DialogueManager.Instance.buttons.SetActive(false);

            if (dialogues.Count > 0)
            {
                DialogueManager.Instance.dialogue_box.SetActive(true);
                DialogueManager.Instance.char_image.sprite = npc_char_img;
                DialogueManager.Instance.char_name.text = npc_name;

                // Inicia a nova coroutine e armazena a referência
                if (dialogueCoroutine != null)
                {
                    StopCoroutine(dialogueCoroutine);
                }
                dialogueCoroutine = StartCoroutine(ShowDialogue());
            }
        }
    }

    private IEnumerator ShowDialogue()
    {
        DialogueManager.Instance.dialogue_text.text = "";
        isDialogueComplete = false; // Define que o diálogo não está completo ainda
        foreach (char letter in dialogues[currentDialogueIndex])
        {
            DialogueManager.Instance.dialogue_text.text += letter;
            yield return new WaitForSeconds(0.03f);
        }
        isDialogueComplete = true; // Define que o diálogo está completo
    }

    private void NextDialogue()
    {
        if (dialogue_actived && Input.GetMouseButtonDown(0))
        {
            if (isDialogueComplete)
            {
                if (currentDialogueIndex < dialogues.Count - 1)
                {
                    currentDialogueIndex++;

                    // Inicia a nova coroutine e armazena a referência
                    if (dialogueCoroutine != null)
                    {
                        StopCoroutine(dialogueCoroutine);
                    }
                    dialogueCoroutine = StartCoroutine(ShowDialogue());
                }
                else if (merchant_npc == true)
                {
                    currentDialogueIndex = 0;

                    GameObject button_one = DialogueManager.Instance.dialogue_option_one;
                    button_one.GetComponentInChildren<Text>().text = "Shop";

                    button_one.GetComponent<Button>().onClick.RemoveAllListeners();
                    button_one.GetComponent<Button>().onClick.AddListener(OpenShop);
                    button_one.SetActive(true);


                    GameObject button_two = DialogueManager.Instance.dialogue_option_two;
                    button_two.GetComponentInChildren<Text>().text = "Sell";

                    button_two.GetComponent<Button>().onClick.RemoveAllListeners();
                    button_two.GetComponent<Button>().onClick.AddListener(OpenSellShop);
                    button_two.SetActive(true);


                    GameObject button_three = DialogueManager.Instance.dialogue_option_three;
                    button_three.GetComponentInChildren<Text>().text = "Exit";

                    button_three.GetComponent<Button>().onClick.RemoveAllListeners();
                    button_three.GetComponent<Button>().onClick.AddListener(EndDialogue);
                    button_three.SetActive(true);
                }
                else
                {
                    DialogueManager.Instance.dialogue_box.SetActive(false);
                    dialogue_actived = false;

                    DialogueManager.Instance.hotbar.SetActive(true);
                    DialogueManager.Instance.buttons.SetActive(true);

                    InventoryManager.Instance.RefreshInventory();


                    if (quester_npc && !quest_gived)
                    {
                        quest_gived = true;
                        QuestManager.Instance.ActiveQuest(quest_to_active);
                        //QuestManager.Instance.ShowQuestPanel();
                    }
                    
                }
            }
            else
            {
                // Completa a exibição do texto atual imediatamente
                StopCoroutine(dialogueCoroutine);
                DialogueManager.Instance.dialogue_text.text = "";
                DialogueManager.Instance.dialogue_text.text = dialogues[currentDialogueIndex];
                isDialogueComplete = true; // Marca o diálogo como completo
            }
        }
    }

    private void EndDialogue()
    {
        DialogueManager.Instance.dialogue_box.SetActive(false);
        dialogue_actived = false;

        DialogueManager.Instance.hotbar.SetActive(true);
        DialogueManager.Instance.buttons.SetActive(true);

        InventoryManager.Instance.RefreshInventory();
    }

    ////////////PARTE DE SHOP///////////

    public void OpenShop()
    {
        DialogueManager.Instance.shop_panel.SetActive(true);
        DialogueManager.Instance.shop_panel_txt.text = "Shop";
        CreateShopItem();
    }

    private void SortItensForSell() //Sorteia os Itens que o NPC ira Vender quando o jogo Starta
    {
        int maxItems = Mathf.Min(3, weapons_sold.Count);
        List<int> chosenIndices = new List<int>();

        for (int i = 0; i < maxItems; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, weapons_sold.Count);
            }
            while (chosenIndices.Contains(randomIndex));

            chosenIndices.Add(randomIndex);
            selectedWeapons.Add(weapons_sold[randomIndex]); // Armazena os itens sorteados
        }
    }

    public void CreateShopItem() //Mostra os Itens pra Venda sempre que abrir a loja do NPC (Itens fixos, mas sorteados aleatoriamente no game Start
    {
        foreach (var weapon in selectedWeapons)
        {
            GameObject shop_item = Instantiate(DialogueManager.Instance.shop_item, DialogueManager.Instance.shop_item_holder.transform);
            shop_item.GetComponent<ShopItemBehavior>().w_ = weapon;
        }
    }

    public void OpenSellShop()
    {
        DialogueManager.Instance.shop_panel.SetActive(true);
        DialogueManager.Instance.shop_panel_txt.text = "Sell";

        GameObject[] destroy_slots = GameObject.FindGameObjectsWithTag("InventorySlot");
        foreach (GameObject slot in destroy_slots) { Destroy(slot); }

        InventoryManager.Instance.RefreshInventory();

        int hotkey = 1;
        foreach (Weapon w in InventoryManager.Instance.inventory)
        {  
            if (w != null)
            {
                GameObject slot_instance = Instantiate(InventoryManager.Instance.inv_slot, DialogueManager.Instance.shop_item_sell_holder.transform);

                Image icon_image = slot_instance.transform.Find("Icon").GetComponent<Image>();
                icon_image.sprite = w.weapon_icon;
                icon_image.enabled = true;

                slot_instance.transform.Find("Icon").GetComponent<Button>().onClick.RemoveAllListeners();
                slot_instance.transform.Find("Icon").GetComponent<Button>().onClick.AddListener(SellItem);

                slot_instance.transform.Find("Options").GetComponent<Button>().onClick.RemoveAllListeners();

                slot_instance.GetComponent<InvSlotBehavior>().invSlotRefer = hotkey - 1;

                slot_instance.GetComponentInChildren<Text>().text = null;
            }
            hotkey++;
        }
    }

    public void SellItem()
    {
        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        Transform parentTransform = clickedButton.transform.parent;
        InvSlotBehavior invslot = parentTransform.GetComponent<InvSlotBehavior>();

        if (InventoryManager.Instance.inventory.Count > 0 && invslot.invSlotRefer >= 0 && invslot.invSlotRefer < InventoryManager.Instance.inventory.Count)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<EntityStats>().AddGold(InventoryManager.Instance.inventory[invslot.invSlotRefer].weapon_gold_value * 0.5f);
            InventoryManager.Instance.inventory[invslot.invSlotRefer] = null;
            Destroy(parentTransform.gameObject);
        }
        InventoryManager.Instance.VerifyWeaponActive(InventoryManager.Instance.inventory[invslot.invSlotRefer]);
    }
}
