using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyMove : MonoBehaviour
{
    private EntityStats entity_stats;
    private Rigidbody2D rb;
    private GameObject player_object;
    private Animator anim;

    private Vector3 previousPosition;
    [HideInInspector] public bool can_move = true;
    public float detection_range = 8f;

    private Vector3 targetPosition;
    private float randomMoveRange = 3f;
    private float changeDirectionInterval = 2f;
    private float timeSinceLastChange;
    private bool isMovingAwayFromPlayer;
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

    private void FixedUpdate()
    {
        Move();
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
            isMovingAwayFromPlayer = true;
            Vector3 directionAwayFromPlayer = (transform.position - player_object.transform.position).normalized;
            Vector3 awayPosition = transform.position + directionAwayFromPlayer * detection_range; // Move away from player

            MoveTowards(awayPosition);
            isMoving = true;
        }
        else
        {
            isMovingAwayFromPlayer = false;
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

        FlipSprite(isMovingAwayFromPlayer ? player_object.transform.position : targetPosition);
        isWalkAnim(isMoving);
    }

    private void MoveTowards(Vector3 destination)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, entity_stats.actual_speed * Time.deltaTime);
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = transform.position + new Vector3(Random.Range(-randomMoveRange, randomMoveRange), Random.Range(-randomMoveRange, randomMoveRange), 0);

        // Limit target position within patrol radius
        if (Vector3.Distance(targetPosition, originPosition) > patrolRadius)
        {
            targetPosition = originPosition + (targetPosition - originPosition).normalized * patrolRadius;
        }
    }

    private void FlipSprite(Vector3 target)
    {
        if (entity_stats.identifier_name == "Slime")
        {
            GetComponent<SpriteRenderer>().flipX = transform.position.x > target.x;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = transform.position.x < target.x;
        }

    }

    private void isWalkAnim(bool isMoving)
    {
        anim.SetBool("isWalk", isMoving);
    }
}
