using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject targetPlane;
    [SerializeField] GameObject target;

    [SerializeField] GameObject powerBar;
    [SerializeField] Image powerBarImage;

    float launchPower = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DrawTargetDirection();

        // Launch Ball
        if (Input.GetMouseButton(0) && Sphere.Instance.movement == Vector3.zero)
        {
            powerBar.SetActive(true);

            if(launchPower < 1f)
                launchPower += Time.deltaTime / 2;
        }

        if (Input.GetMouseButtonUp(0))
        {
            LaunchBall(launchPower);
            powerBar.SetActive(false);
        }

        powerBarImage.fillAmount = launchPower;
    }

    void DrawTargetDirection()
    {
        if (Sphere.Instance.movement == Vector3.zero)
        {
            targetPlane.SetActive(true);
            
            targetPlane.transform.rotation = Quaternion.Euler(new Vector3(0, CameraController.Instance.yaw + 180, 0));
        }
        else
        {
            targetPlane.SetActive(false);
        }
    }

    void LaunchBall(float power)
    {
        power *= 40;

        ball.transform.position += new Vector3(0, 0.06f, 0);
        Sphere.Instance.pushVector = (target.transform.position - ball.transform.position).normalized * power;

        launchPower = 0f;
    }
}
