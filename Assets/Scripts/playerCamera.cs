using UnityEngine;
using Cinemachine;

/*
    CameraControl Class is used to rotate the
    free look camera used via mouse hold.
*/
public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public KeyCode rotateButton = KeyCode.Mouse1;

    /*
        Checks if Mouse is pressed to rotate the camera.
    */
    private void Update()
    {
        // Rotate Camera
        bool rotateButtonDown = Input.GetKey(rotateButton);
        if (rotateButtonDown)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        }
        else
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
    }
}
