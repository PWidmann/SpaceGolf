using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] GameObject ballObject;
    [SerializeField] GameObject targetPlane;
    [SerializeField] GameObject target;

    [SerializeField] GameObject powerBar;
    [SerializeField] Image powerBarImage;

    Sphere ball;
    Vector3 pushVector;
    float launchPower = 0f;

    void Start()
    {
        ball = ballObject.GetComponent<Sphere>();
    }

    void Update()
    {
        DrawTargetDirection();

        if (Input.GetMouseButton(0) && Sphere.Instance.movement == Vector3.zero)
        {
            // Show and fill power bar
            powerBar.SetActive(true);
            if (launchPower < 1f)
                launchPower += Time.deltaTime / 2;
        }

        if (Input.GetMouseButtonUp(0) && ball.GetComponent<Sphere>().movement == Vector3.zero)
        {
            // Launch Ball
            LaunchBall(launchPower);
            powerBar.SetActive(false);
        }

        powerBarImage.fillAmount = launchPower;
    }

    void DrawTargetDirection()
    {
        // Show and rotate target arrow
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
        // 35 Power max
        power *= 35;

        pushVector = (target.transform.position - ball.transform.position).normalized * power;
        pushVector.y = 0f;
        Sphere.Instance.pushVector = pushVector;

        launchPower = 0f;
    }
}
