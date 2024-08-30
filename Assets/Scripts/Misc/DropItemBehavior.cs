using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItemBehavior : MonoBehaviour
{
    [HideInInspector] public float gold_to_give;

    private void Start()
    {
        Destroy(this.gameObject, 300f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.gameObject.CompareTag("GoldDrop"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<EntityStats>().AddGold(gold_to_give);
                Destroy(this.gameObject);
            }
        }
    }
}
