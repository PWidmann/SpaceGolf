using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// 3D Collision code snippets
// https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection

public class PhysicsManager : MonoBehaviour
{
    public Sphere ball;
    public GameObject playerStart;

    [Header("Course Prefabs")]
    public GameObject[] trackParts;
    GameObject parentObject;

    public List<Box> boxList = new List<Box>();
    


    //Collision
    Vector3 normalVector;
    bool collisionHappened;
    float collisionThreshold;

    void Start()
    {      
        foreach (GameObject trackObject in trackParts)
        {
            if(trackObject)
                parentObject = trackObject;

            foreach (Transform child in parentObject.transform)
            {
                boxList.Add(child.GetComponent<Box>());
            }
            
        }

        ball.transform.position = playerStart.transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && ball.movement == Vector3.zero)
            ball.transform.position = playerStart.transform.position;
    }

    private void FixedUpdate()
    {
        CheckForCollisions();

        // Gravity
        if (!ball.isColliding)
            ball.GetComponent<Sphere>().AddGravity();

        HandlingBallRollGravity();

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

                if (ball.canChangeMovement == true)
                    ball.ChangeMovementOnCollision(calcNormalVector(ball, box.bounds), ball.bounciness * box.bounciness);

                collisionHappened = true;
                ball.isColliding = true;
                ball.canChangeMovement = false;
                break;
            }
        }

        if (!collisionHappened)
        {
            ball.canChangeMovement = true;
            ball.isColliding = false;
        }
    }

    public bool isSphereIntersectingAABB(Vector3 spherePosition, Bounds box)
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

    public Vector3 calcNormalVector(Sphere sphere, Bounds box)
    {
        // Ball is already colliding with box when this code runs
        
        // For dealing with frame skipping
        collisionThreshold = ball.radius * 1;

        // Calculate normals from positions

        // TO DO Abfrage was passiert wenn mit Y kollidiert UND mit Wänden

        

        // Ball colliding in x direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - collisionThreshold) && 
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + collisionThreshold) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + collisionThreshold) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - collisionThreshold) && 
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.min.x + collisionThreshold))
            normalVector = -Vector3.right;

        // Ball colliding in minus x direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - collisionThreshold) && 
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + collisionThreshold) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + collisionThreshold) && 
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - collisionThreshold) && 
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.max.x - collisionThreshold))
            normalVector = Vector3.right;

        // Ball colliding in z direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.min.z + collisionThreshold) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + collisionThreshold) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - collisionThreshold))
            normalVector = Vector3.forward;

        // Ball colliding in minus z direction
        if ((sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.max.z - collisionThreshold) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) < box.max.y + collisionThreshold) &&
            (sphere.transform.position.y + (ball.movement.y * Time.fixedDeltaTime) > box.min.y - collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - collisionThreshold))
            normalVector = -Vector3.forward;

        // Ball collides in Minus Y direction (Top -> Down)
        if ((sphere.transform.position.y + (ball.movement.z * Time.fixedDeltaTime) > box.max.y - collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) > box.min.x - collisionThreshold) &&
            (sphere.transform.position.x + (ball.movement.x * Time.fixedDeltaTime) < box.max.x + collisionThreshold) &&
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) < box.max.z + collisionThreshold) &&
            (sphere.transform.position.z + (ball.movement.z * Time.fixedDeltaTime) > box.min.z - collisionThreshold))
            normalVector = Vector3.up;

        return normalVector;
    }

    public void HandlingBallRollGravity()
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
