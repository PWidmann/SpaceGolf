using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3D Collision code snippets
// https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection

public class PhysicsManager : MonoBehaviour
{
    public Sphere ball;
    public GameObject playerStartPosition;

    [Header("Course Prefabs")]
    public GameObject[] trackParts;

    // Bounding boxes
    GameObject parentObject;
    private List<Box> boxList = new List<Box>();
    
    //Collision
    Vector3 normalVector;
    bool collisionHappened;

    void Start()
    {
        // Make bounding box list
        foreach (GameObject trackObject in trackParts)
        {
            if(trackObject)
                parentObject = trackObject;

            foreach (Transform child in parentObject.transform)
            {
                boxList.Add(child.GetComponent<Box>());
            }
        }

        ball.transform.position = playerStartPosition.transform.position;
    }

    private void Update()
    {
        // Reset player to start
        if (Input.GetKeyDown(KeyCode.R) && ball.movement == Vector3.zero)
            ball.transform.position = playerStartPosition.transform.position;
    }

    private void FixedUpdate()
    {
        CheckForCollisions();

        // Ball rolling physics
        HandlingBallRollGravity();

        // Gravity
        if (!ball.isColliding)
            ball.GetComponent<Sphere>().AddGravity();

        // Move ball
        ball.GetComponent<Sphere>().Move();
    }

    void CheckForCollisions()
    {
        collisionHappened = false;

        foreach (Box box in boxList)
        {
            if (isSphereIntersectingAABB(ball.transform.position, box.bounds))
            {
                ball.isColliding = true;

                if (ball.canChangeMovement == true)
                    ball.ChangeMovementOnCollision(calcNormalVector(ball, box.bounds), ball.bounciness * box.bounciness);

                // Only one movement change per collision
                collisionHappened = true; 
                ball.canChangeMovement = false;
                break;
            }
        }

        if (!collisionHappened)
        {
            // If no more colliding, make movement change ready again
            ball.isColliding = false;
            ball.canChangeMovement = true;
        }
    }

    private bool isSphereIntersectingAABB(Vector3 spherePosition, Bounds box)
    {
        // get box closest point to sphere center by clamping
        var x = Math.Max(box.min.x, Math.Min(spherePosition.x, box.max.x));
        var y = Math.Max(box.min.y, Math.Min(spherePosition.y, box.max.y));
        var z = Math.Max(box.min.z, Math.Min(spherePosition.z, box.max.z));

        // this is the same as isPointInsideSphere
        var distance = Math.Sqrt((x - spherePosition.x) * (x - spherePosition.x) +
                                 (y - spherePosition.y) * (y - spherePosition.y) +
                                 (z - spherePosition.z) * (z - spherePosition.z));

        return distance <= ball.radius;
    }

    private Vector3 calcNormalVector(Sphere sphere, Bounds box)
    {
        // Ball is already colliding with box when this code runs
        // Calculate normals from positions


        // Ball colliding in x direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - ball.radius) && 
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + ball.radius) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + ball.radius) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - ball.radius) && 
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.min.x + ball.radius))
            normalVector = -Vector3.right;

        // Ball colliding in minus x direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - ball.radius) && 
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + ball.radius) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + ball.radius) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - ball.radius) && 
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.max.x - ball.radius))
            normalVector = Vector3.right;

        // Ball colliding in z direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.min.z + ball.radius) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + ball.radius) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - ball.radius))
            normalVector = Vector3.forward;

        // Ball colliding in minus z direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.max.z - ball.radius) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + ball.radius) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - ball.radius))
            normalVector = -Vector3.forward;

        // Ball collides in Minus Y direction (Top -> Down)
        if ((sphere.transform.position.y + (ball.movement.z * Time.fixedDeltaTime) > box.max.y - ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - ball.radius) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + ball.radius) &&
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + ball.radius) &&
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - ball.radius))
            normalVector = Vector3.up;

        return normalVector;
    }

    private void HandlingBallRollGravity()
    {
        bool isOverGround = false;

        if (ball.ballIsRolling)
        {
            foreach (Box box in boxList)
            {
                // Check if ball is over a box
                if (isSphereIntersectingAABB(ball.transform.position + new Vector3(0, -0.3f, 0), box.bounds))
                {
                    isOverGround = true;
                    break;
                }
            }
        }


        if (isOverGround == false && ball.movement != Vector3.zero)
        {
            ball.gravityActive = true;
            ball.ballIsRolling = false;
        }


        // Handling ball roll slowing
        if (!ball.gravityActive)
            ball.movement *= 0.999f; // Good value for Fixed Timestep 0.005
    }
}
