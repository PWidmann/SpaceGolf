using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour
{
    public Vector3 position;
    public float bounciness = 1f;

    [HideInInspector]
    public float radius;
    [HideInInspector]
    public float height;
    
    new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();

        position = renderer.bounds.center;
        radius = renderer.bounds.extents.x;
        height = renderer.bounds.size.y;
    }
}
