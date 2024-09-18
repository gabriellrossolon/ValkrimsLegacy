using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2.0f;
    public float waypointThreshold = 0.2f;

    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;
    private Rigidbody2D rb;
    private Animator anim;

    private Vector2 previousPosition; // Posição anterior
    private bool movingForward = true; // Direção do movimento

    void Start()
    {
        previousPosition = transform.position;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
        else
        {
            Debug.LogWarning("Nenhum waypoint definido.");
        }
    }

    void Update()
    {
        VerifyAnim();

        if (waypoints.Length == 0) return;

        MoveTowardsWaypoint();

        if (Vector2.Distance(transform.position, targetWaypoint.position) < waypointThreshold)
        {
            UpdateWaypointIndex();
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void MoveTowardsWaypoint()
    {
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void UpdateWaypointIndex()
    {
        if (movingForward)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = waypoints.Length - 1; // Fique no último waypoint
                movingForward = false; // Mude para a direção reversa
            }
        }
        else
        {
            currentWaypointIndex--;
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 0; // Fique no primeiro waypoint
                movingForward = true; // Mude para a direção normal
            }
        }
    }

    void VerifyAnim()
    {
        float threshold = 0.01f;
        bool isMoving = rb.velocity.magnitude >= threshold;

        Vector2 velocity = rb.velocity;
        Vector2 direction = velocity.normalized;

        anim.SetBool("isMovingUp", false);
        anim.SetBool("isMovingDown", false);
        anim.SetBool("isMovingRight", false);
        anim.SetBool("isMovingLeft", false);

        if (isMoving)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    anim.SetBool("isMovingRight", true);
                }
                else
                {
                    anim.SetBool("isMovingLeft", true);
                }
            }
            else
            {
                if (direction.y > 0)
                {
                    anim.SetBool("isMovingUp", true);
                }
                else
                {
                    anim.SetBool("isMovingDown", true);
                }
            }
        }

        anim.SetBool("isWalk", isMoving);
    }
}


