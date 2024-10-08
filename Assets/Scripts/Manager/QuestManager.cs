using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
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

    public GameObject quest_panel;
    private bool opened_quest_panel = false;

    [Header("Quests")]
    public bool mushroom_investigation;
    public bool the_strange_cave;
    public bool the_crystal_creatures;

    public GameObject mushroom_pointer;
    public GameObject strangecave_pointer;
    public GameObject crystalcreatures_pointer;

    [HideInInspector]
    public Text quest_name;
    public Text quest_objective;
    public Text quest_count;

    public string quest_tracking;

    public int enemy_killed;

    private GameObject player_object;


    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        //ReloadQuest();
        if (quest_tracking == "mushroom investigation" && mushroom_investigation == true) { quest_count.text = $"{enemy_killed}/3"; }

        if (mushroom_investigation)
        {
            mushroom_pointer.SetActive(true);
        }
        if (the_strange_cave)
        {
            strangecave_pointer.SetActive(true);
        }
        if (the_crystal_creatures)
        {
            crystalcreatures_pointer.SetActive(true);
        }


    }

    public void ActiveQuest(string quest_to_active)
    {
        //quest_actived = true;

        if (quest_to_active == "mushroom investigation")
        {
            if(mushroom_investigation == false)
            {
                mushroom_investigation = true;
            }
        }
        else if(quest_to_active == "the strange cave")
        {
            if(the_strange_cave == false)
            {
                the_strange_cave = true;
            }
        }
        else if(quest_to_active == "the crystal creatures")
        {
            if (the_crystal_creatures == false)
            {
                the_crystal_creatures = true;
            }
        }
        AddQuestInQuestsPanel(quest_to_active);
    }

    private void AddQuestInQuestsPanel(string quest_to_active)
    {
        if(mushroom_investigation == true && quest_to_active == "mushroom investigation")
        {
            GameObject quest_banner_instance = Instantiate(
            HudManager.Instance.quests_banner,
            HudManager.Instance.quest_panel_one.transform.position,
            Quaternion.identity,
            HudManager.Instance.quest_panel_one.transform
            );

            Button quest_button = quest_banner_instance.GetComponentInChildren<Button>();
            quest_button.onClick.AddListener(SelectQuestToTrack);

            quest_banner_instance.GetComponentInChildren<Text>().text = "Mushroom Investigation";
        } 
        else if (the_strange_cave = true && quest_to_active == "the strange cave")
        {
            GameObject quest_banner_instance = Instantiate(
            HudManager.Instance.quests_banner,
            HudManager.Instance.quest_panel_one.transform.position,
            Quaternion.identity,
            HudManager.Instance.quest_panel_one.transform
            );

            Button quest_button = quest_banner_instance.GetComponentInChildren<Button>();
            quest_button.onClick.AddListener(SelectQuestToTrack);

            quest_banner_instance.GetComponentInChildren<Text>().text = "The Strange Cave";
        }
        else if (the_crystal_creatures = true && quest_to_active == "the crystal creatures")
        {
            GameObject quest_banner_instance = Instantiate(
            HudManager.Instance.quests_banner,
            HudManager.Instance.quest_panel_one.transform.position,
            Quaternion.identity,
            HudManager.Instance.quest_panel_one.transform
            );

            Button quest_button = quest_banner_instance.GetComponentInChildren<Button>();
            quest_button.onClick.AddListener(SelectQuestToTrack);

            quest_banner_instance.GetComponentInChildren<Text>().text = "The Crystal Creatures";
        }
    }

    public void SelectQuestToTrack()
    {
        // Pega o objeto que está atualmente selecionado pelo EventSystem (o botão que foi clicado)
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        Text buttonText = selectedButton.GetComponentInChildren<Text>();

        if (buttonText.text == "Mushroom Investigation" && mushroom_investigation == true)
        {
            quest_name.text = "Mushroom Investigation";
            quest_objective.text = "Kill 03 Mushroom Monsters";
            quest_count.text = $"{enemy_killed}/3";
            quest_tracking = "mushroom investigation";
        }
        else if (buttonText.text == "The Strange Cave" && the_strange_cave == true)
        {
            quest_name.text = "The Strange Cave";
            quest_objective.text = "Follow at North Path of the city and find the mentioned Strange Cave";
            quest_count.text = "";
            quest_tracking = "the strange cave";
        }
        else if (buttonText.text == "The Crystal Creatures" && the_strange_cave == true)
        {
            quest_name.text = "The Crystal Creatures";
            quest_objective.text = "Follow at West of the city and find any of the mentioned Crystal Creatures";
            quest_count.text = "";
            quest_tracking = "the crystal creatures";
        }
    }

    public void VerifyQuestCondition()
    {
        if (mushroom_investigation == true)
        {
            if (enemy_killed >= 3)
            {
                EndQuest("mushroom investigation");
                GiveReward(15f, 5f);
                mushroom_investigation = false;
                enemy_killed = 0;
            }
        }
        
        if (the_strange_cave == true)
        {
            if (player_object.GetComponent<EntityStats>().player_ambient == "Dungeon1")
            {
                EndQuest("the strange cave");
                GiveReward(5f, 0f);
                the_strange_cave = false;
            }
        }

        if (the_crystal_creatures == true)
        {
            if (player_object.GetComponent<EntityStats>().player_ambient == "Dungeon1")
            {
                EndQuest("the crystal creatures");
                GiveReward(8f, 12f);
                the_crystal_creatures = false;
            }
        }
    }

    private void EndQuest(string quest_to_complete)
    {
        Text[] questTexts = HudManager.Instance.quests_panel.GetComponentsInChildren<Text>();

        if (quest_to_complete == "mushroom investigation")
        {
            mushroom_pointer.SetActive(false);
            enemy_killed = 0;

            foreach (Text questText in questTexts)
            {
                if (questText.text == "Mushroom Investigation")
                {
                    Destroy(questText.transform.parent.parent.gameObject);
                    break;
                }
            }
        } 
        else if (quest_to_complete == "the strange cave")
        {
            strangecave_pointer.SetActive(false);

            foreach (Text questText in questTexts)
            {
                if (questText.text == "The Strange Cave")
                {
                    Destroy(questText.transform.parent.parent.gameObject);
                    break;
                }
            }
        }
        else if (quest_to_complete == "the crystal creatures")
        {
            crystalcreatures_pointer.SetActive(false);

            foreach (Text questText in questTexts)
            {
                if (questText.text == "The Crystal Creatures")
                {
                    Destroy(questText.transform.parent.parent.gameObject);
                    break;
                }
            }
        }

        quest_name.text = "";
        quest_objective.text = "";
        quest_count.text = $"";
        
    }

    private void GiveReward(float xp_to_give, float gold_to_give)
    {
        player_object.GetComponent<EntityStats>().AddExp(xp_to_give);
        player_object.GetComponent<EntityStats>().AddGold(gold_to_give);
    }

    public void ShowQuestPanel()
    {
        if (!opened_quest_panel)
        {
            opened_quest_panel = true;
            quest_panel.GetComponent<Animator>().SetBool("panelOpen", opened_quest_panel);
        } else
        {
            opened_quest_panel = false;
            quest_panel.GetComponent<Animator>().SetBool("panelOpen", opened_quest_panel);
        }
        
    }
}
