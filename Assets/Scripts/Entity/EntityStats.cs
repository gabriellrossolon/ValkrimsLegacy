using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    public float base_speed;
    public float actual_speed;
    public float max_speed;
    [HideInInspector] public float original_speed;

    void Start()
    {
        actual_speed = base_speed;

        original_speed = base_speed;
    }


    void Update()
    {
        
    }
}
