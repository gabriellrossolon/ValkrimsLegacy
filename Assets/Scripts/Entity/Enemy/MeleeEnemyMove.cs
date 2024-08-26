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
    private float detection_range = 8f;


    private Vector3 targetPosition;
    private float randomMoveRange = 3f;
    private float changeDirectionInterval = 2f;
    private float timeSinceLastChange;
    private bool isMovingTowardsPlayer;
    private float waitTime = 2f;
    private float waitTimer;
    private bool isWaiting;

    void Start()
    {
        entity_stats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();
        player_object = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        SetRandomTargetPosition();
    }

    private void Update()
    {
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        Move();
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

    //Legacy Movement
    private void MoveToPlayer()
    {
        float distance_to_player = Vector3.Distance(transform.position, player_object.transform.position);

        if (can_move == true && distance_to_player <= detection_range)
        {
            transform.position = Vector3.MoveTowards(transform.position, player_object.transform.position, entity_stats.actual_speed * Time.deltaTime);
        }

        if (transform.position.x > player_object.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (transform.position.x < player_object.transform.position.x)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }

        bool isMoving = transform.position != previousPosition;
        isWalkAnim(isMoving);
    }

    private void Move()
    {
        bool isMoving = false; // Inicializa isMoving como false

        if (!can_move) // Verifica se o inimigo pode se mover
        {
            isWalkAnim(isMoving); // Atualiza animação de espera
            return; // Sai do método se não puder se mover
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                SetRandomTargetPosition(); // Define nova posição aleatória após espera
            }
            isWalkAnim(isMoving); // Atualiza animação de espera
            return;
        }

        float distance_to_player = Vector3.Distance(transform.position, player_object.transform.position);

        if (distance_to_player <= detection_range)
        {
            isMovingTowardsPlayer = true;
            MoveTowards(player_object.transform.position);
            isMoving = true; // Marca como movendo-se em direção ao jogador
        }
        else
        {
            isMovingTowardsPlayer = false;
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= changeDirectionInterval)
            {
                SetRandomTargetPosition(); // Atualiza a posição aleatória
                timeSinceLastChange = 0f;
            }

            MoveTowards(targetPosition);
            isMoving = true; // Marca como movendo-se para a posição aleatória
        }

        // Verifica se chegou perto da posição alvo
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isWaiting = true; // Inicia a pausa
        }

        FlipSprite(isMovingTowardsPlayer ? player_object.transform.position : targetPosition);
        isWalkAnim(isMoving); // Atualiza animação de movimento
    }

    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, entity_stats.actual_speed * Time.deltaTime);
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = transform.position + new Vector3(Random.Range(-randomMoveRange, randomMoveRange), Random.Range(-randomMoveRange, randomMoveRange), 0);
    }

    private void FlipSprite(Vector3 target)
    {
        GetComponent<SpriteRenderer>().flipX = transform.position.x < target.x;
    }

    private void isWalkAnim(bool isMoving)
    {
        anim.SetBool("isWalk", isMoving);
    }

    //=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//=====//

}
