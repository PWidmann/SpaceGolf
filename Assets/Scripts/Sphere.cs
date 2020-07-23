using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public static Sphere Instance;

    public Vector3 movement;
    public float radius;
    public float bounciness = 0.7f;
    public float gravity = -9.81f;

    Vector3 checkPointPosition;

    public Vector3 pushVector;

    public bool isColliding = false;

    public bool canChangeMovement = false;

    Renderer renderer;

    public bool gravityActive = true;
    public bool ballIsRolling = false;

    

    void Start()
    {
        if (Instance == null)
            Instance = this;

        renderer = GetComponent<Renderer>();
        radius = renderer.bounds.extents.x;
    }

    
    void FixedUpdate()
    {
        // Reset to checkpoint when ball falls off
        if (transform.position.y <= -30f)
        {
            transform.position = checkPointPosition;
            movement = Vector3.zero;
        }
            
    }
    public void AddGravity()
    {
        if(gravityActive)
            movement.y += gravity * Time.fixedDeltaTime;
    }

    public void Move()
    {
        if (pushVector != Vector3.zero)
        {
            movement += pushVector;
            pushVector = Vector3.zero;
        }

        if (movement.magnitude >= 0.1f)
        {
            transform.Translate(movement * Time.fixedDeltaTime);
        }
        else
        {
            movement = Vector3.zero;
            gravityActive = false;
            checkPointPosition = transform.position;
            ballIsRolling = true;
        }


        // Rise ball if under collision box
        if (isColliding && movement == Vector3.zero)
        {
            transform.position += new Vector3(0, 0.2f, 0) * Time.deltaTime;
        }
        
    }

    public void ChangeMovementOnCollision(Vector3 normal, float bounciness)
    {
        movement = Quaternion.AngleAxis(180, normal) * movement;
        movement *= -bounciness; // das - dreht den Vektor weg von der Kollision
    }
}
