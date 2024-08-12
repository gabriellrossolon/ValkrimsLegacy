using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Acesso a outros componentes do Player
    private Rigidbody2D rigid_body;
    private EntityStats entity_stats;
    private SpriteRenderer sprite_renderer;
    private Animator animator;

    private Vector2 MovementInput;

    
    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        entity_stats = GetComponent<EntityStats>();
        sprite_renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }


    void FixedUpdate()
    {
        PlayerMove();
        Animate();
    }

    private void PlayerMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //rigid_body.AddForce(new Vector2(horizontal * entity_stats.actual_speed * Time.deltaTime, vertical * entity_stats.actual_speed * Time.deltaTime));
        if (horizontal == 0 && vertical == 0)
        {
            rigid_body.velocity = new Vector2(0, 0);
            animator.SetBool("isWalk", false);
            return;
        }

        MovementInput = new Vector2(horizontal, vertical);
        rigid_body.AddForce(MovementInput * entity_stats.actual_speed * Time.deltaTime);

        if(horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isWalk", true);
            
        }
        else
        {
            animator.SetBool("isWalk", false);
        }

        //Corta velocidade quando na diagonal.
        if ((horizontal != 0 && vertical != 0))
        {
            entity_stats.actual_speed = entity_stats.original_speed * 0.66f;
        }
        else
        {
            entity_stats.actual_speed = entity_stats.original_speed;
        }
    }

    private void Animate()
    {
        animator.SetFloat("MovementX", MovementInput.x);
        animator.SetFloat("MovementY", MovementInput.y);
    }

}
