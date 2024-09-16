using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Quests")]
    public bool mushroom_investigation;
    public bool the_strange_cave;
    public bool quest_actived = false;

    [HideInInspector]
    public Text quest_name;
    public Text quest_objective;
    public Text quest_count;

    public int enemy_killed;

    private GameObject player_object;


    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        ReloadQuest();
        if (quest_actived)
        {
            quest_panel.SetActive(true);
        } 
        else if (!quest_actived)
        {
            quest_panel.SetActive(false);
        }
    }

    public void ActiveQuest(string quest_to_active)
    {
        quest_actived = true;

        if (quest_to_active == "mushroom investigation")
        {
            mushroom_investigation = true;
        }
        else if(quest_to_active == "the strange cave")
        {
            the_strange_cave = true;
        }
        ReloadQuest();
    }

    public void ReloadQuest()
    {
        if (mushroom_investigation == true)
        {
            quest_name.text = "Mushroom Investigation";
            quest_objective.text = "Kill 03 Mushroom Monsters";
            quest_count.text = $"{enemy_killed}/3";

            if(enemy_killed >= 3)
            {
                EndQuest("mushroom investigation");
                GiveReward(15f, 5f);
            }
        }
        else if (the_strange_cave == true)
        {
            quest_name.text = "The Strange Cave";
            quest_objective.text = "Follow at North Path of the city and find the mentioned Strange Cave";
            quest_count.text = "";

            if(player_object.GetComponent<EntityStats>().player_ambient == "Dungeon1")
            {
                EndQuest("the strange cave");
                GiveReward(5f, 0f);
            }
        }
        else
        {
            quest_name.text = "";
            quest_objective.text = "";
            quest_count.text = $"";
            quest_actived = false;

        }
    }

    private void EndQuest(string quest_to_complete)
    {
        if(quest_to_complete == "mushroom investigation")
        {
            mushroom_investigation = false;
            enemy_killed = 0;
        } 
        else if (quest_to_complete == "the strange cave")
        {
            the_strange_cave = false;
        }
    }

    private void GiveReward(float xp_to_give, float gold_to_give)
    {
        player_object.GetComponent<EntityStats>().AddExp(xp_to_give);
        player_object.GetComponent<EntityStats>().AddGold(gold_to_give);
    }
}
