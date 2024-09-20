using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion")]
public class Potion : ScriptableObject
{
    public Sprite potion_icon;

    public float hp_added;
    public float mana_added;
    public float stamina_added;
}
