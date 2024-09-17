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
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void MoveTowardsWaypoint()
    {
        Vector2 direction = (targetWaypoint.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void VerifyAnim()
    {
        // Defina um valor pequeno para considerar como "zero"
        float threshold = 0.01f;

        // Verifique se a magnitude da velocidade é maior que o limiar
        bool isMoving = rb.velocity.magnitude >= threshold;

        // Determine a direção predominante com base na velocidade
        Vector2 velocity = rb.velocity;
        Vector2 direction = velocity.normalized;

        // Desative todos os parâmetros primeiro
        anim.SetBool("isMovingUp", false);
        anim.SetBool("isMovingDown", false);
        anim.SetBool("isMovingRight", false);
        anim.SetBool("isMovingLeft", false);

        if (isMoving)
        {
            // Defina a direção predominante
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Movimento horizontal predomina
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
                // Movimento vertical predomina
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

        // Atualize o parâmetro isWalk com base no movimento
        anim.SetBool("isWalk", isMoving);
    }
}


