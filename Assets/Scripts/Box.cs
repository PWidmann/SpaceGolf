using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // AABB
    public Bounds bounds;
    public float bounciness = 1f;
    new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        // Get box boundings from renderer
        bounds = renderer.bounds;
    }
}
