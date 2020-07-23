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

    // Update is called once per frame
    void Update()
    {
        DrawTargetDirection();

        // Launch Ball
        if (Input.GetMouseButton(0) && Sphere.Instance.movement == Vector3.zero)
        {
            powerBar.SetActive(true);

            

            if (launchPower < 1f)
                launchPower += Time.deltaTime / 2;
        }

        if (Input.GetMouseButtonUp(0) && ball.GetComponent<Sphere>().movement == Vector3.zero)
        {
            LaunchBall(launchPower);
            powerBar.SetActive(false);

            ball.canChangeMovement = false; // Set false so the collision system can check even when ball moves on the ground and therefore 'colliding'
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
        // Default 35
        power *= 35;

        pushVector = (target.transform.position - ball.transform.position).normalized * power;
        pushVector.y = 0f;
        Sphere.Instance.pushVector = pushVector;

        launchPower = 0f;
    }
}
