using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBall : MonoBehaviour
{
    public static PlayBall Instance;

    public float bounciness = 0.7f;
    public float gravity = -9.81f;

    [HideInInspector]
    public float radius;
    [HideInInspector]
    public Vector3 movement;
    [HideInInspector]
    public Vector3 pushVector;

    // For physics
    [HideInInspector]
    public bool isColliding = false;
    [HideInInspector]
    public bool canChangeMovement = false;
    public bool gravityActive = true;
    public bool ballIsRolling = false;
    
    new Renderer renderer;
    Vector3 checkPointPosition;

    void Start()
    {
        if (Instance == null)
            Instance = this;

        renderer = GetComponent<Renderer>();
        radius = renderer.bounds.extents.x;
    }
    
    void FixedUpdate()
    {
        // Reset to checkpoint when ball falls off the course
        if (transform.position.y <= -80f)
        {
            transform.position = checkPointPosition;
            movement = Vector3.zero;
            GameInterface.Instance.ScreenFlash();
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

        // Clamp movement for handling jittering
        if (movement.magnitude >= 0.1f)
        {
            transform.Translate(movement * Time.fixedDeltaTime);
        }
        else
        {
            movement = Vector3.zero;
            checkPointPosition = transform.position;
            gravityActive = false;
            ballIsRolling = true;
        }

        // Rise ball if under collision box
        if (isColliding && movement == Vector3.zero)
        {
            transform.position += new Vector3(0, 0.2f, 0) * Time.fixedDeltaTime;
        }
    }

    public void ChangeMovementOnCollision(Vector3 normal, float bounciness)
    {
        movement = Quaternion.AngleAxis(180, normal) * movement;
        movement *= -bounciness; // Turns movement vector away from collision
    }
}
