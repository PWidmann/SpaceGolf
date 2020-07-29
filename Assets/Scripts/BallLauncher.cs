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

    GameObject powerBar;
    Image powerBarImage;

    
    Vector3 pushVector;
    float launchPower = 0f;

    PlayBall ball;

    void Start()
    {
        ball = ballObject.GetComponent<PlayBall>();

        powerBar = GameInterface.Instance.PowerBar;
        powerBarImage = GameInterface.Instance.PowerBarImage;
    }

    void Update()
    {
        if (GameManager.Instance.GameHasStarted && !GameManager.Instance.GameFinished)
        {
            // Direction arrow
            DrawTargetDirection();

            if (Input.GetMouseButton(0) && PlayBall.Instance.movement == Vector3.zero && GameManager.Instance.GameHasStarted)
            {
                // Show and fill power bar
                powerBar.SetActive(true);
                if (launchPower < 1f)
                    launchPower += Time.deltaTime / 2;
            }

            if (Input.GetMouseButtonUp(0) && ball.GetComponent<PlayBall>().movement == Vector3.zero && GameManager.Instance.GameHasStarted && powerBar.activeSelf == true)
            {
                // Launch Ball
                LaunchBall(launchPower);
                powerBar.SetActive(false);
            }

            powerBarImage.fillAmount = launchPower;
        }
    }

    void DrawTargetDirection()
    {
        // Show and rotate direction arrow
        if (PlayBall.Instance.movement == Vector3.zero)
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
        // Power max = 35
        power *= 35;

        pushVector = (target.transform.position - ball.transform.position).normalized * power;
        pushVector.y = 0f;
        PlayBall.Instance.pushVector = pushVector;
        launchPower = 0f;

        GameManager.Instance.RoundSwings++;
        SoundManager.instance.PlaySound(1);
    }
}
