using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Merchant Controll")]
    public bool merchant_npc;

    void Start()
    {
        button_image = GetComponentInChildren<Image>();
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
                    button_one.GetComponent<Button>().onClick.AddListener(OpenShop);
                    button_one.SetActive(true);

                    GameObject button_two = DialogueManager.Instance.dialogue_option_two;
                    button_two.GetComponentInChildren<Text>().text = "Exit";
                    button_two.GetComponent<Button>().onClick.AddListener(EndDialogue);
                    button_two.SetActive(true);
                }
                else
                {
                    DialogueManager.Instance.dialogue_box.SetActive(false);
                    dialogue_actived = false;

                    DialogueManager.Instance.hotbar.SetActive(true);
                    DialogueManager.Instance.buttons.SetActive(true);

                    InventoryManager.Instance.RefreshInventory();


                    if (quester_npc)
                    {
                        QuestManager.Instance.ActiveQuest(quest_to_active);
                        QuestManager.Instance.ShowQuestPanel();
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

    private void OpenShop()
    {
        Debug.Log("Oppened Shop");
    }
   
}
