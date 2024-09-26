using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrder : MonoBehaviour
{
    private GameObject player_object;
    private SpriteRenderer sprite_renderer;

    public float transparence_distance;
    public float detection_distance_above = 7.0f;
    public float detection_distance_below = 2.0f;
    public int max_sort_order = 13;
    public int min_sort_order = 10;
    public float detection_distance = 3f;

    private int original_order_layer;
    private bool hasReturnedToOriginalLayer = false;

    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
        sprite_renderer = GetComponent<SpriteRenderer>();
        original_order_layer = sprite_renderer.sortingOrder;

        // Chamada inicial
        UpdateSortingOrder();
    }

    void Update()
    {
        // Se o jogador se mover, atualize a ordem
        if (Vector2.Distance(player_object.transform.position, transform.position) < detection_distance)
        {
            UpdateSortingOrder();
        }
    }

    private void UpdateSortingOrder()
    {
        float distance_to_player = Vector2.Distance(player_object.transform.position, transform.position);

        if (distance_to_player <= detection_distance)
        {
            if (player_object.transform.position.y > transform.position.y)
            {
                sprite_renderer.sortingOrder = max_sort_order;
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b,
                    distance_to_player < transparence_distance ? 0.5f : 1f);
            }
            else
            {
                sprite_renderer.sortingOrder = min_sort_order;
                sprite_renderer.color = new Color(sprite_renderer.color.r, sprite_renderer.color.g, sprite_renderer.color.b, 1f);
            }
            hasReturnedToOriginalLayer = false;
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

    // Se você ainda precisa dessas funções, considere chamá-las em intervalos ou eventos
    private void VerifyPositionForEnemies(List<GameObject> enemy_objects)
    {
        foreach (GameObject enemy in enemy_objects)
        {
            if (enemy != null)
            {
                UpdateEnemySorting(enemy);
            }
        }
    }

    private void UpdateEnemySorting(GameObject enemy)
    {
        SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();
        float distance_to_enemy = Vector2.Distance(enemy.transform.position, transform.position);

        if (enemy.transform.position.y > transform.position.y && distance_to_enemy <= detection_distance_above)
        {
            enemySpriteRenderer.sortingOrder = 3;
        }
        else if (enemy.transform.position.y < transform.position.y && distance_to_enemy <= detection_distance_below)
        {
            enemySpriteRenderer.sortingOrder = 13;
        }
    }

    // Similar para NPCs
    private void VerifyPositionForNPCs(List<GameObject> npc_objects)
    {
        foreach (GameObject npc in npc_objects)
        {
            if (npc != null)
            {
                UpdateNPCSorting(npc);
            }
        }
    }

    private void UpdateNPCSorting(GameObject npc)
    {
        SpriteRenderer npcSpriteRenderer = npc.GetComponent<SpriteRenderer>();
        float distance_to_npc = Vector2.Distance(npc.transform.position, transform.position);

        if (npc.transform.position.y > transform.position.y && distance_to_npc <= detection_distance_above)
        {
            npcSpriteRenderer.sortingOrder = 3;
        }
        else if (npc.transform.position.y < transform.position.y && distance_to_npc <= detection_distance_below)
        {
            npcSpriteRenderer.sortingOrder = 11;
        }
    }
}