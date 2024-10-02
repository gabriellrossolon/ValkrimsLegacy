using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBehavior : MonoBehaviour
{
    public Transform player;
    private GameObject[] map_pointers;

    public int openedCamSize = 75;
    public int closedCamSize = 30;

    void Start()
    {
        map_pointers = GameObject.FindGameObjectsWithTag("MapPointer");
        MapPointerControll();
    }

    void Update()
    {
        if (HudManager.Instance.map_opened == true)
        {
            GetComponent<Camera>().orthographicSize = openedCamSize; // Aumenta o "zoom" para abrir o mapa
        }
        else
        {
            GetComponent<Camera>().orthographicSize = closedCamSize; // Reduz o "zoom" quando o mapa está fechado
        }
    }

    void MapPointerControll()
    {
        if (map_pointers != null)
        {
            foreach (GameObject pointer in map_pointers)
            {
                if (pointer != null)
                {
                    SpriteRenderer spriteRenderer = pointer.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        if (pointer.GetComponentInParent<EntityStats>() != null && pointer.GetComponentInParent<EntityStats>().hp <= 0)
                        {
                            spriteRenderer.enabled = false;
                        }
                        else
                        {
                            spriteRenderer.enabled = true;
                        }
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -1f);
    }
}
