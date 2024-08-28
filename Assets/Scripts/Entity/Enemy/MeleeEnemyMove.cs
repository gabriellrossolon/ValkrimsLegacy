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
    [SerializeField] private float detection_range = 8f;


    private Vector3 targetPosition;
    private float randomMoveRange = 3f;
    private float changeDirectionInterval = 2f;
    private float timeSinceLastChange;
    private bool isMovingTowardsPlayer;
    private float waitTime = 2f;
    private float waitTimer;
    private bool isWaiting;

    private Vector3 originPosition;
    [SerializeField] private float patrolRadius = 5f;
    void Start()
    {
        entity_stats = GetComponent<EntityStats>();
        rb = GetComponent<Rigidbody2D>();
        player_object = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        SetRandomTargetPosition();
        originPosition = transform.position;
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
        bool isMoving = false;

        if (!can_move)
        {
            isWalkAnim(isMoving);
            return;
        }

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                SetRandomTargetPosition();
            }
            isWalkAnim(isMoving);
            return;
        }

        float distance_to_player = Vector3.Distance(transform.position, player_object.transform.position);

        if (distance_to_player <= detection_range)
        {
            isMovingTowardsPlayer = true;
            Vector3 playerPosition = player_object.transform.position;

            MoveTowards(playerPosition);
            isMoving = true;
        }
        else
        {
            isMovingTowardsPlayer = false;
            timeSinceLastChange += Time.deltaTime;

            if (timeSinceLastChange >= changeDirectionInterval)
            {
                SetRandomTargetPosition();
                timeSinceLastChange = 0f;
            }

            MoveTowards(targetPosition);
            isMoving = true;
        }

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isWaiting = true;
        }

        FlipSprite(isMovingTowardsPlayer ? player_object.transform.position : targetPosition);
        isWalkAnim(isMoving);
    }

    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, entity_stats.actual_speed * Time.deltaTime);
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = transform.position + new Vector3(Random.Range(-randomMoveRange, randomMoveRange), Random.Range(-randomMoveRange, randomMoveRange), 0);

        // Limita a posição alvo dentro do raio de patrulha
        if (Vector3.Distance(targetPosition, originPosition) > patrolRadius)
        {
            targetPosition = originPosition + (targetPosition - originPosition).normalized * patrolRadius;
        }
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
