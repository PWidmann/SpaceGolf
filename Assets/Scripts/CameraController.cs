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
    private float rotationSmoothTime = 0.1f;
    Vector2 pitchMinMax = new Vector2(10, 89);
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;
    Vector3 targetRotation;
    public float yaw;
    float pitch;
    Vector2 saveMousePosition;

    private bool cameraRotationActive = false;

    private void Start()
    {
        if (Instance == null)
            Instance = this;

        currentCameraZoom = 2;
        targetCameraZoom = 2;

        // Starting camera rotation
        yaw = 50;
        pitch = 20;
        targetRotation = new Vector3(pitch, yaw);

        // Camera smoothing depends on timescale
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        Cursor.lockState = CursorLockMode.Locked;

    }


    void LateUpdate()
    {
        if (target)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch += Input.GetAxis("Mouse Y") * mouseSensitivity * -1;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
            targetRotation = new Vector3(pitch, yaw);

            currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            // Camera Zoom
            var d = Input.GetAxis("Mouse ScrollWheel");
            if (d > 0f)
            {
                if (targetCameraZoom > minDistanceFromTarget)
                    targetCameraZoom -= cameraZoomRate;
            }
            else if (d < 0f)
            {
                if (targetCameraZoom < maxDistanceFromTarget)
                    targetCameraZoom += cameraZoomRate;
            }

            transform.position = target.position - transform.forward * currentCameraZoom;
            currentCameraZoom = Mathf.Lerp(currentCameraZoom, targetCameraZoom, cameraSmoothing);
        }
    }
}
