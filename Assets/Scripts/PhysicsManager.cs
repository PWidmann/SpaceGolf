using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection

public class PhysicsManager : MonoBehaviour
{
    public bool useRaycastNormals = true;
    public Sphere ball;

    public List<Box> boxList = new List<Box>();

    public GameObject[] foundBoxes;

    bool collisionHappened;

    float collisionThreshold;

    Vector3 normalVector;

    Box lastCollisionBox = new Box();

    void Start()
    {

        foundBoxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject box in foundBoxes)
        {
            boxList.Add(box.GetComponent<Box>());
        }
    }

    private void Update()
    {

        CheckForCollisions();

        // Add gravity
        if (!ball.isColliding )
            ball.GetComponent<Sphere>().AddGravity();

        // Move ball
        ball.GetComponent<Sphere>().Move();
    }

    void CheckForCollisions()
    {
        collisionHappened = false;

        

        foreach (Box box in boxList)
        {
            if (isSphereIntersectingAABB(ball, box.bounds))
            {
                if (ball.changedMovementOnCollision == false)
                    ball.ChangeMovementOnCollision(calcNormalVector(ball, box.bounds), ball.bounciness * box.bounciness);

                collisionHappened = true;
                ball.isColliding = true;
            }
        }

        if (!collisionHappened)
        {
            ball.changedMovementOnCollision = false;
            ball.isColliding = false;
        }
    }

    public bool isSphereIntersectingAABB(Sphere sphere, Bounds box)
    {
        // get box closest point to sphere center by clamping
        var x = Math.Max(box.min.x, Math.Min(sphere.transform.position.x, box.max.x));
        var y = Math.Max(box.min.y, Math.Min(sphere.transform.position.y, box.max.y));
        var z = Math.Max(box.min.z, Math.Min(sphere.transform.position.z, box.max.z));

        // this is the same as isPointInsideSphere
        var distance = Math.Sqrt((x - sphere.transform.position.x) * (x - sphere.transform.position.x) +
                                 (y - sphere.transform.position.y) * (y - sphere.transform.position.y) +
                                 (z - sphere.transform.position.z) * (z - sphere.transform.position.z));

        return distance <= sphere.radius;
    }

    public Vector3 calcNormalVector(Sphere sphere, Bounds box)
    {
        // Ball is already colliding with box when this code runs
        // Calculate normals from positions


        // To deal with fast ball traveling
        collisionThreshold = ball.radius * 2;


        // Ball collides in Y direction
        if (sphere.transform.position.y >= box.max.y - collisionThreshold)
        {
            normalVector = Vector3.up;
        }
            

        // Ball colliding in z direction
        if ((sphere.transform.position.z < box.min.z + collisionThreshold) && (sphere.transform.position.y < box.max.y) && (sphere.transform.position.y > box.min.y) && (sphere.transform.position.x < box.max.x) && (sphere.transform.position.x > box.min.x))
            normalVector = Vector3.forward;

        // Ball colliding in minus z direction
        if ((sphere.transform.position.z > box.max.z - collisionThreshold) && (sphere.transform.position.y < box.max.y) && (sphere.transform.position.y > box.min.y) && (sphere.transform.position.x < box.max.x) && (sphere.transform.position.x > box.min.x))
            normalVector = -Vector3.forward;

        // Ball colliding in x direction
        if ((sphere.transform.position.z > box.min.z) && (sphere.transform.position.z < box.max.z) && (sphere.transform.position.y < box.max.y) && (sphere.transform.position.y > box.min.y) && (sphere.transform.position.x < box.min.x + collisionThreshold))
            normalVector = Vector3.right;

        // Ball colliding in minus x direction
        if ((sphere.transform.position.z > box.min.z) && (sphere.transform.position.z < box.max.z) && (sphere.transform.position.y < box.max.y) && (sphere.transform.position.y > box.min.y) && (sphere.transform.position.x > box.max.x - collisionThreshold))
            normalVector = -Vector3.right;

        return normalVector;



    }
}
