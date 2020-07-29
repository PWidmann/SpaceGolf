using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3D Collision code snippets
// https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection

public class PhysicsManager : MonoBehaviour
{
    public PlayBall ball;
    public GameObject playerStartPosition;

    [Header("Course Prefabs")]
    public GameObject[] trackParts;
    public GameObject[] cylinders;

    // Bounding Volumes
    GameObject parentObject;
    private List<Box> boxList = new List<Box>();
    private List<Cylinder> cylinderList = new List<Cylinder>();

    //Collision
    Vector3 normalVector;
    bool collisionHappened;

    bool afterFirstFrame = false;

    void Start()
    {
        trackParts = GameObject.FindGameObjectsWithTag("TrackPart");
        cylinders = GameObject.FindGameObjectsWithTag("Cylinder");

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

        // Create cylinder bounds list
        foreach (GameObject cylinder in cylinders)
        {
            cylinderList.Add(cylinder.GetComponent<Cylinder>());
        }

        ball.transform.position = playerStartPosition.transform.position;
        
    }

    private void Update()
    {
        // Reset player to start
        if (Input.GetKeyDown(KeyCode.R) && ball.movement == Vector3.zero)
        {
            ball.transform.position = playerStartPosition.transform.position;
            GameInterface.Instance.FinishPanel.SetActive(false);
            GameManager.Instance.GameFinished = false;
            GameManager.Instance.RoundSwings = 0;
            GameInterface.Instance.ScreenFlash();
            CameraController.Instance.yaw = 0;
        }
    }

    private void FixedUpdate()
    {
        CheckForCollisions();

        // Ball rolling physics
        HandlingBallRollGravity();

        // Gravity
        if (!ball.isColliding)
            ball.GetComponent<PlayBall>().AddGravity();

        // Move ball
        ball.GetComponent<PlayBall>().Move();
    }

    void CheckForCollisions()
    {
        collisionHappened = false;

        // Box Collisions
        foreach (Box box in boxList)
        {
            if (isSphereIntersectingAABB(ball.transform.position, box.bounds))
            {
                ball.isColliding = true;

                if (ball.canChangeMovement == true)
                    ball.ChangeMovementOnCollision(calcNormalVectorBox(ball, box.bounds), ball.bounciness * box.bounciness);

                collisionHappened = true; 
                ball.canChangeMovement = false; // Only one movement change per collision
                break;
            }
        }

        // Cylinder Collisions
        foreach (Cylinder cylinder in cylinderList)
        {
            if (isSphereIntersectingCylinder(ball.transform.position, cylinder))
            {
                ball.isColliding = true;



                if (ball.canChangeMovement == true)
                    ball.ChangeMovementOnCollision(calcNormalVectorCylinder(ball, cylinder), ball.bounciness * cylinder.bounciness);

                // Clamp speed at 35 (max BallLauncher power)
                if (ball.movement.magnitude > 35f)
                    ball.movement = ball.movement.normalized * 35f;

                collisionHappened = true;
                ball.canChangeMovement = false; // Only one movement change per collision
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

    private bool isSphereIntersectingCylinder(Vector3 ball, Cylinder cylinder)
    {
        Vector3 vectorToCenter = ball - cylinder.position;
        Vector2 v2distance = new Vector2(vectorToCenter.x, vectorToCenter.z);
        float distance = v2distance.magnitude;

        if ((distance < PlayBall.Instance.radius + cylinder.radius) &&
            (ball.y < cylinder.position.y + cylinder.height / 2 + PlayBall.Instance.radius) &&
            (ball.y > cylinder.position.y - cylinder.height / 2 - PlayBall.Instance.radius))
        {
            return true;

        }
        else
        {
            return false;
        }

    }

    private Vector3 calcNormalVectorBox(PlayBall sphere, Bounds box)
    {
        // Ball is already colliding with the box when this code runs
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

    private Vector3 calcNormalVectorCylinder(PlayBall ball, Cylinder cylinder)
    {
        // Ball is already colliding with the Cylinder when this code runs

        // direction vector from the center of the cylinder to the center of the ball
        Vector3 heading = ball.transform.position - cylinder.position;
        heading.y = 0;
        normalVector = heading.normalized; // Gets pushed away with speed 35

        //If ball is on top
        if (ball.transform.position.y > cylinder.position.y + cylinder.height / 2)
            normalVector = Vector3.up;

        //If ball is under cylinder
        if (ball.transform.position.y < cylinder.position.y - cylinder.height / 2)
            return Vector3.down;

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
                if (isSphereIntersectingAABB(ball.transform.position + new Vector3(0, -0.1f, 0), box.bounds))
                {
                    isOverGround = true;
                    break;
                }
            }

            foreach (Cylinder cylinder in cylinderList)
            {
                // Check if ball is over a box
                if (isSphereIntersectingCylinder(ball.transform.position + new Vector3(0, -0.1f, 0), cylinder))
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
