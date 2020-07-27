using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeTriggerBase : MonoBehaviour
{
    [Header("Player")]
    public GameObject playerGameobject;

    [Header("Box Bound")]
    public float sizeX;
    public float sizeY;
    public float sizeZ;

    public Color lineColor;

    public Bounds boxBounds;
    
    [HideInInspector]
    public Vector3 position;
    [HideInInspector]
    public Vector3 v3FrontTopLeft;
    [HideInInspector]
    public Vector3 v3FrontTopRight;
    [HideInInspector]
    public Vector3 v3FrontBottomLeft;
    [HideInInspector]
    public Vector3 v3FrontBottomRight;
    [HideInInspector]
    public Vector3 v3BackTopLeft;
    [HideInInspector]
    public Vector3 v3BackTopRight;
    [HideInInspector]
    public Vector3 v3BackBottomLeft;
    [HideInInspector]
    public Vector3 v3BackBottomRight;

    private void Update()
    {
        Debug.Log("VolumeTriggerUpdate");
    }
    public void CalcPositons()
    {
        Vector3 v3Center = boxBounds.center;
        Vector3 v3Extents = boxBounds.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner
    }

    public void DrawBox()
    {
        Debug.DrawLine(v3FrontTopLeft, v3FrontTopRight, lineColor);
        Debug.DrawLine(v3FrontTopRight, v3FrontBottomRight, lineColor);
        Debug.DrawLine(v3FrontBottomRight, v3FrontBottomLeft, lineColor);
        Debug.DrawLine(v3FrontBottomLeft, v3FrontTopLeft, lineColor);

        Debug.DrawLine(v3BackTopLeft, v3BackTopRight, lineColor);
        Debug.DrawLine(v3BackTopRight, v3BackBottomRight, lineColor);
        Debug.DrawLine(v3BackBottomRight, v3BackBottomLeft, lineColor);
        Debug.DrawLine(v3BackBottomLeft, v3BackTopLeft, lineColor);

        Debug.DrawLine(v3FrontTopLeft, v3BackTopLeft, lineColor);
        Debug.DrawLine(v3FrontTopRight, v3BackTopRight, lineColor);
        Debug.DrawLine(v3FrontBottomRight, v3BackBottomRight, lineColor);
        Debug.DrawLine(v3FrontBottomLeft, v3BackBottomLeft, lineColor);
    }

    public bool isPointInsideAABB(Vector3 pos, Bounds boxBounds)
    {
        return (pos.x >= boxBounds.min.x && pos.x <= boxBounds.max.x) &&
               (pos.y >= boxBounds.min.y && pos.y <= boxBounds.max.y) &&
               (pos.z >= boxBounds.min.z && pos.z <= boxBounds.max.z);
    }
}
