using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://developer.mozilla.org/en-US/docs/Games/Techniques/3D_collision_detection

public class PhysicsManager : MonoBehaviour
{
    public Sphere ball;

    public List<Box> boxList = new List<Box>();

    public GameObject[] foundBoxes;

    void Start()
    {
        foundBoxes = GameObject.FindGameObjectsWithTag("Box");

        foreach (GameObject box in foundBoxes)
        {
            boxList.Add(box.GetComponent<Box>());
        }
    }

    private void FixedUpdate()
    {
        CheckForCollisions();

        // Add gravity
        if(!isSphereIntersectingAABB(ball, boxList[0].bounds))
            ball.GetComponent<Sphere>().AddGravity();

        // Move ball
        ball.GetComponent<Sphere>().Move();
    }

    void CheckForCollisions()
    {

        if (isSphereIntersectingAABB(ball, boxList[0].bounds))
        {
            ball.ChangeMovementOnCollision(-Vector3.up, ball.bounciness * boxList[0].bounciness);
            ball.isColliding = true;
        }
        else
        {
            ball.isColliding = false;
        }
    }

    public bool isPointIntersectingAABB(Vector3 point, Bounds box)
    { 
        return (point.x >= box.min.x && point.x <= box.max.x) &&
         (point.y >= box.min.y && point.y <= box.max.y) &&
         (point.z >= box.min.z && point.z <= box.max.z);
    }

    public bool isSphereIntersectingAABB(Sphere sphere, Bounds box)
    {
        // get box closest point to sphere center by clamping
        var x = Math.Max(box.min.x, Math.Min(sphere.position.x, box.max.x));
        var y = Math.Max(box.min.y, Math.Min(sphere.position.y, box.max.y));
        var z = Math.Max(box.min.z, Math.Min(sphere.position.z, box.max.z));

        // this is the same as isPointInsideSphere
        var distance = Math.Sqrt((x - sphere.position.x) * (x - sphere.position.x) +
                                 (y - sphere.position.y) * (y - sphere.position.y) +
                                 (z - sphere.position.z) * (z - sphere.position.z));

        return distance <= sphere.radius;
    }
}
