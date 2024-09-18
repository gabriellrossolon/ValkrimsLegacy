using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    //Objetos
    private GameObject player_object;
    private GameObject[] enemy_objects;
    private GameObject[] npc_objects;
    private SpriteRenderer sprite_renderer;

    public float transparence_distance;

    [Header("Between NPC || Enemy X Enviroment")]
    public float detection_distance_above = 7.0f; // Distância de detecção quando o NPC está acima
    public float detection_distance_below = 2.0f; // Distância de detecção quando o NPC está abaixo

    [Header("Between Player X Enviroment")]
    public int max_sort_order = 13;
    public int min_sort_order = 10;
    public float detection_distance = 3f;

    private int original_order_layer;
    private bool hasReturnedToOriginalLayer = false;

    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
        enemy_objects = GameObject.FindGameObjectsWithTag("Enemy");
        npc_objects = GameObject.FindGameObjectsWithTag("NPC");
        sprite_renderer = GetComponent<SpriteRenderer>();
        original_order_layer = sprite_renderer.sortingOrder;
    }

    void Update()
    {
        VerifyPositionForPlayer();
        VerifyPositionForEnemy();
        VerifyPositionForNpc();
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
                    sprite_renderer.sortingOrder = max_sort_order;
                }
                else
                {
                    sprite_renderer.sortingOrder = min_sort_order;
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
                    if (enemy != null)
                    {
                        float distance_to_enemy = Vector2.Distance(enemy.transform.position, transform.position);

                        if (enemy.transform.position.y > transform.position.y)
                        {
                            // Enemy está acima do objeto
                            if (distance_to_enemy <= detection_distance_above)
                            {
                                // Ajusta a camada do inimigo com base na posição do objeto atual
                                enemySpriteRenderer.sortingOrder = 3;
                            }
                        }
                        else if ((enemy.transform.position.y < transform.position.y))
                        {
                            // Enemy está abaixo do objeto
                            if (distance_to_enemy <= detection_distance_below)
                            {
                                // Ajusta a camada do inimigo com base na posição do objeto atual
                                enemySpriteRenderer.sortingOrder = 13;
                            }
                        }
                    }
                }
            }
        }
    }

    private void VerifyPositionForNpc()
    {
        if (!this.gameObject.CompareTag("NPC"))
        {
            // Itera sobre todos os inimigos
            foreach (GameObject npc in npc_objects)
            {
                if (npc != null)
                {
                    SpriteRenderer npcSpriteRenderer = npc.GetComponent<SpriteRenderer>();
                    if (npc != null)
                    {
                        float distance_to_npc = Vector2.Distance(npc.transform.position, transform.position);

                        if (npc.transform.position.y > transform.position.y)
                        {
                            // NPC está acima do objeto
                            if (distance_to_npc <= detection_distance_above)
                            {
                                // Ajusta a camada do inimigo com base na posição do objeto atual
                                npcSpriteRenderer.sortingOrder = 3;
                            }
                        }
                        else if((npc.transform.position.y < transform.position.y))
                        {
                            // NPC está abaixo do objeto
                            if (distance_to_npc <= detection_distance_below)
                            {
                                // Ajusta a camada do inimigo com base na posição do objeto atual
                                npcSpriteRenderer.sortingOrder = 13;
                            }
                        }
                    }
                }
            }
        }
    }
}