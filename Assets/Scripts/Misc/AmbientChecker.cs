using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChecker : MonoBehaviour
{
    public string ambient_type;
    private GameObject player_object;

    void Start()
    {
        player_object = GameObject.FindGameObjectWithTag("Player");
    }

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(this.ambient_type == "Florest")
            {
                SoundManager.Instance.player_ambient = "Florest";
                player_object.GetComponent<EntityStats>().player_ambient = "Florest";
            }   
            else if (this.ambient_type == "Dungeon1")
            {
                SoundManager.Instance.player_ambient = "Dungeon1";
                player_object.GetComponent<EntityStats>().player_ambient = "Dungeon1";
            }
            else if (this.ambient_type == "Cave1")
            {
                SoundManager.Instance.player_ambient = "Cave1";
                player_object.GetComponent<EntityStats>().player_ambient = "Cave1";
            }

            SoundManager.Instance.SoundChecker();

            QuestManager.Instance.ReloadQuest();
        }
    }
}
