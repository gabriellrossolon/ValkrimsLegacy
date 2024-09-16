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

    [Header("Quests")]
    public bool mushroom_investigation;
    public bool the_strange_cave;

    public Text quest_name;
    public Text quest_objective;
    public Text quest_count;

    public int enemy_killed;

    private GameObject player_object;


    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
    }

    public void ActiveQuest(string quest_to_active)
    {
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
        if (mushroom_investigation)
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
        else if (the_strange_cave)
        {
            quest_name.text = "The Strange Cave";
            quest_objective.text = "Follow the Path North of the city and find the mentioned Strange Cave";
            quest_count.text = "";

            if(player_object.GetComponent<EntityStats>().player_ambient == "Dungeon1")
            {
                Debug.Log("Check");
                EndQuest("the strange cave");
                GiveReward(5f, 0f);
            }
        }
    }

    private void EndQuest(string quest_to_complete)
    {
        quest_name.text = "";
        quest_objective.text = "";
        quest_count.text = $"";

        Debug.Log("Quest Completa");

        if(quest_to_complete == "mushroom investigation")
        {
            mushroom_investigation = false;
        } 
        else if (quest_to_complete == "the strange cave")
        {
            the_strange_cave = false;
        }

    }

    private void GiveReward(float xp_to_give, float gold_to_give)
    {
        Debug.Log("Premio enviado");
        EntityStats player_entity_stats = player_object.GetComponent<EntityStats>();

        player_entity_stats.AddExp(xp_to_give);
        player_entity_stats.AddGold(gold_to_give);

    }
    
}
