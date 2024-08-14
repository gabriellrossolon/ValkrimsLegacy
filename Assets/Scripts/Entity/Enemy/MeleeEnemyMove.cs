using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyMove : MonoBehaviour
{
    private EntityStats entity_stats;
    private Rigidbody2D rb;
    private GameObject player_object;
    private Animator anim;

    private Vector3 previousPosition;
    [HideInInspector] public bool can_move = true;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();
        player_object = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        MoveToPlayer();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void MoveToPlayer()
    {
        if (can_move == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player_object.transform.position, entity_stats.actual_speed * Time.deltaTime);
            //Vector2 direction = (player_object.transform.position - transform.position).normalized;
            //rb.velocity = (direction * entity_stats.actual_speed);
        }

        if (transform.position.x > player_object.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (transform.position.x < player_object.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        //isMoving retorna true ou false
        bool isMoving = transform.position != previousPosition;
        //bool isMoving = entity_stats.actual_speed > 0;

        isWalkAnim(isMoving);
    }

    private void isWalkAnim(bool isMoving)
    {
        anim.SetBool("isWalk", isMoving);
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

}
