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
            GameInterface.Instance.FinishText.text = "You have finished the course in " + GameManager.Instance.RoundSwings + " swings";
            GameManager.Instance.GameFinished = true;
            if (!triggered)
            {
                SoundManager.instance.PlaySound(2);
                triggered = true;
            }
        }
    }
}
