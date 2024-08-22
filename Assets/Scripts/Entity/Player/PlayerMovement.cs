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
    public ParticleSystem run_smoke;
    public ParticleSystem dash_smoke;

    private Vector2 movement_input;
    private bool is_moving;

    public float dash_speed;
    public float dash_cooldown;
    private float timer;
    private bool can_dash = true;

    private float dash_duration;
    [HideInInspector] public bool is_dashing;


    void Start()
    {
        rigid_body = GetComponent<Rigidbody2D>();
        entity_stats = GetComponent<EntityStats>();
        sprite_renderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        StartDash(10f);

        DashCooldown();
    }

    void FixedUpdate()
    {
        PlayerMove();
        Animate();
        PlayerRun();

    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void PlayerMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //rigid_body.AddForce(new Vector2(horizontal * entity_stats.actual_speed * Time.deltaTime, vertical * entity_stats.actual_speed * Time.deltaTime));

        movement_input = new Vector2(horizontal, vertical);
        movement_input.Normalize();
        rigid_body.AddForce(movement_input * entity_stats.actual_speed * Time.deltaTime);


        if (horizontal == 0 && vertical == 0)
        {
            animator.SetBool("isWalk", false);
            is_moving = false;
            return;
        }

        if(horizontal != 0 || vertical != 0)
        {
            animator.SetBool("isWalk", true);
            is_moving = true;

        }
        //else
        //{
        //    animator.SetBool("isWalk", false);
        //    is_moving = false;
        //}
    }

    private void Animate()
    {
        animator.SetFloat("MovementX", movement_input.x);
        animator.SetFloat("MovementY", movement_input.y);
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void PlayerRun()
    {
        bool is_running = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && (is_moving);

        if (is_running)
        {
            entity_stats.actual_speed = entity_stats.max_speed;
            animator.SetBool("isRun", true);

            if (!run_smoke.isPlaying)
            {
                run_smoke.Play();
            }

            if (!IsInvoking("DrainStamina"))
            {
                InvokeRepeating("DrainStamina", 0f, 0.1f); // Drena a stamina a cada 1 segundo
            }
        }
        else
        {
            entity_stats.actual_speed = entity_stats.base_speed;
            animator.SetBool("isRun", false);

            if (run_smoke.isPlaying)
            {
                run_smoke.Stop();
            }

            if (IsInvoking("DrainStamina"))
            {
                CancelInvoke("DrainStamina");
            }
        }
    }

    private void DrainStamina()
    {
        entity_stats.DrainStamina();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    private void StartDash(float stamina_cost)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (entity_stats.stamina >= stamina_cost)
            {
                if (is_moving && can_dash && entity_stats.stamina >= 15f)
                {
                    rigid_body.AddForce(movement_input * dash_speed);
                    can_dash = false;
                    is_dashing = true;
                    timer = 0;
                    dash_smoke.Play();
                    animator.SetTrigger("isDash");
                    StartCoroutine(EndDash(0.2f));

                    entity_stats.RemoveStamina(stamina_cost);
                }
            }
            else
            {
                GameObject stamina_warn_instance = Instantiate(HudManager.Instance.stamina_warn, transform.position, Quaternion.identity);
                Destroy(stamina_warn_instance, 1f);
            }

        }

        if (!is_dashing)
        {
            dash_smoke.Stop();
        }
    }

    private IEnumerator EndDash(float delay)
    {
        yield return new WaitForSeconds(delay);
        is_dashing = false;
    }

    private void DashCooldown()
    {
        if (timer >= dash_cooldown && can_dash == false)
        {
            can_dash = true;
            timer = 0;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

}
