using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public Vector3 position;
    public float radius;
    public float bounciness = 0.7f;
    public float gravity = -9.81f;


   

    public Vector3 movement;

    public float movementMagnitude;

    public bool isColliding = false;

    Renderer renderer;

    void Start()
    {
        position = transform.position;
        renderer = GetComponent<Renderer>();
        radius = renderer.bounds.extents.x;
    }

    
    void Update()
    {
        // Update sphere world position
        position = transform.position;

        movementMagnitude = movement.magnitude;
    }

    public void AddMovement(Vector3 additionalMovement)
    {
            movement += additionalMovement;
    }

    public void AddGravity()
    {
        if (!isColliding && movement.magnitude >= 0.01f)
            movement.y += gravity * Time.fixedDeltaTime;
    }

    public void Move()
    {
        if (movement.magnitude >= 0.1f)
            transform.Translate(movement * Time.fixedDeltaTime);
        else
            movement = Vector3.zero;
    }

    public void ChangeMovementOnCollision(Vector3 normal, float bounciness)
    {
        if (!isColliding)
        {
            movement = Quaternion.AngleAxis(180, normal) * movement;
            movement *= -bounciness; // das - dreht den Vektor weg von der Kollision
        }
    }
}
