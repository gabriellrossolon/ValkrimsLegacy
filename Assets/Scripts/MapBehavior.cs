using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehavior : MonoBehaviour
{
    public Transform player;
    private GameObject[] map_pointers;

    void Start()
    {
        map_pointers = GameObject.FindGameObjectsWithTag("MapPointer");
        foreach (GameObject pointer in map_pointers)
        {
            pointer.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -1f);
    }
}
