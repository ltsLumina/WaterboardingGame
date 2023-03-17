using System.Collections;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;  

public class CameraControllerExtra : MonoBehaviour
{ // Start is called before the first frame update
    private CinemachineFreeLook cinemachineCamera;
    private bool isHoldingCenteringButton;

    private float startingRecenteringTime;

    void Start()
    {
        cinemachineCamera = FindObjectOfType<CinemachineFreeLook>();
        startingRecenteringTime = cinemachineCamera.m_RecenterToTargetHeading.m_RecenteringTime;
    }
   
    private void OnRecenterCamera(InputValue value) {
        if (value.isPressed) {
            cinemachineCamera.m_RecenterToTargetHeading.m_enabled = true;
            cinemachineCamera.m_RecenterToTargetHeading.m_RecenteringTime = 0.2f;
        } else {
            cinemachineCamera.m_RecenterToTargetHeading.m_enabled = false;
            cinemachineCamera.m_RecenterToTargetHeading.m_RecenteringTime = startingRecenteringTime;
        }
    }

}
