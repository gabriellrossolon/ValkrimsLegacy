using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    //Objetos
    private GameObject player_object;
    private GameObject[] enemy_objects;
    private SpriteRenderer sprite_renderer;

    public float transparence_distance;
    public float detection_distance = 3f;

    private int original_order_layer;
    private bool hasReturnedToOriginalLayer = false;

    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
        enemy_objects = GameObject.FindGameObjectsWithTag("Enemy");
        sprite_renderer = GetComponent<SpriteRenderer>();
        original_order_layer = sprite_renderer.sortingOrder;
    }

    void Update()
    {
        VerifyPositionForPlayer();
        VerifyPositionForEnemy();
    }

    private void VerifyPositionForPlayer()
    {
        float distance_to_player = Vector2.Distance(player_object.transform.position, transform.position);

        if (distance_to_player <= detection_distance)
        {
            if (!this.gameObject.CompareTag("Enemy"))
            {
                if (player_object.transform.position.y > transform.position.y)
                {
                    sprite_renderer.sortingOrder = 13;
                }
                else
                {
                    sprite_renderer.sortingOrder = 10;
                }

                if (distance_to_player < transparence_distance)
                {
                    if (player_object.transform.position.y > transform.position.y)
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

                hasReturnedToOriginalLayer = false;
            }
            else if (this.gameObject.CompareTag("Enemy"))
            {
                if (player_object.transform.position.y > transform.position.y)
                {
                    sprite_renderer.sortingOrder = 12;
                }
                else
                {
                    sprite_renderer.sortingOrder = 10;
                }
            }
        }
        else
        {
            if (!hasReturnedToOriginalLayer)
            {
                sprite_renderer.sortingOrder = original_order_layer;
                hasReturnedToOriginalLayer = true;
            }

            sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1f);
        }
    }

    private void VerifyPositionForEnemy()
    {
        if (!this.gameObject.CompareTag("Enemy"))
        {
            // Itera sobre todos os inimigos
            foreach (GameObject enemy in enemy_objects)
            {
                if (enemy != null)
                {
                    SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
                    if (enemySpriteRenderer != null)
                    {
                        float distance_to_enemy = Vector2.Distance(enemy.transform.position, transform.position);

                        if (distance_to_enemy <= detection_distance)
                        {
                            // Ajusta a camada do inimigo com base na posição do objeto atual
                            if (enemy.transform.position.y > transform.position.y)
                            {
                                enemySpriteRenderer.sortingOrder = 8;
                            }
                            else
                            {
                                enemySpriteRenderer.sortingOrder = 13;
                            }
                        }
                        //else
                        //{
                        //    // Volta ao valor original se não estiver perto
                        //    enemySpriteRenderer.sortingOrder = original_order_layer;
                        //}
                    }
                }
            }
        }
    }
}