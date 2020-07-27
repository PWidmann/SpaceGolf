using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger: VolumeTriggerBase
{

    void Update()
    {
        position = transform.position;
        boxBounds = new Bounds(position, new Vector3(sizeX, sizeY, sizeZ));
        
        CalcPositons();
        DrawBox();

        if (isPointInsideAABB(playerGameobject.transform.position, boxBounds))
        {
            GameInterface.Instance.FinishPanel.SetActive(true);
        }
    }
}
