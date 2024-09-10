using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientChecker : MonoBehaviour
{
    public string ambient_type;

   private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(this.ambient_type == "Florest")
            {
                SoundManager.Instance.player_ambient = "Florest";
            }   
            else if (this.ambient_type == "Dungeon1")
            {
                SoundManager.Instance.player_ambient = "Dungeon1";
            }

            SoundManager.Instance.SoundChecker();
        }
    }
}
