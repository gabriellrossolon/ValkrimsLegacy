using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    //Objetos
    private GameObject player_object;
    private SpriteRenderer sprite_renderer;

    public float transparence_distance;


    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        VerifyPosition();
    }

    void VerifyPosition()
    {
        float distance = Vector2.Distance(player_object.transform.position, transform.position);

        if (distance < transparence_distance)
        {
            if (player_object.transform.position.y > (transform.position.y - 1))
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 0.5f);

            }
            else
            {
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1f);
            }
        }
        else
        {
            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1f);
        }


        if (player_object.transform.position.y > transform.position.y)
        {
            sprite_renderer.sortingOrder = 11;
        }
        else
        {
            sprite_renderer.sortingOrder = 9;
        }
    }
}
