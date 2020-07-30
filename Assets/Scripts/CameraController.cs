using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform target;
    public float mouseSensitivity = 5;

    //Camera Zoom 
    [Header("Camera Zoom")]
    public float maxDistanceFromTarget = 4;
    public float minDistanceFromTarget = 0.5f;

    private float currentCameraZoom;
    private float targetCameraZoom;
    private float cameraZoomRate = 2f;
    private float cameraSmoothing = 0.05f;

    //Camera Rotation
    [HideInInspector]
    public float yaw;

    float pitch;
    private float rotationSmoothTime = 0.1f;
    Vector2 pitchMinMax = new Vector2(10, 89);
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    Vector3 targetRotation;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        currentCameraZoom = 4;
        targetCameraZoom = 4;

        // Starting camera rotation
        yaw = 0;
        pitch = 30;
        targetRotation = new Vector3(pitch, yaw);

        // Camera smoothing
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
    }


    void LateUpdate()
    {
        if (target && GameManager.GameHasStarted && !GameManager.GameFinished)
        {
            if (!GameManager.InEscapeMenu)
            {
                // Camera rotation
                yaw += Input.GetAxis("Mouse X") * GameManager.MouseSensitivity;
                pitch += Input.GetAxis("Mouse Y") * GameManager.MouseSensitivity * -1;
                pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
                targetRotation = new Vector3(pitch, yaw);
            }

            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            // Camera Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                // Zoom in
                if (targetCameraZoom > minDistanceFromTarget)
                    targetCameraZoom -= cameraZoomRate;
            }
            else if (scroll < 0f)
            {
                // Zoom out
                if (targetCameraZoom < maxDistanceFromTarget)
                    targetCameraZoom += cameraZoomRate;
            }

            transform.position = target.position - transform.forward * currentCameraZoom;
            currentCameraZoom = Mathf.Lerp(currentCameraZoom, targetCameraZoom, cameraSmoothing);
        }
    }
}
