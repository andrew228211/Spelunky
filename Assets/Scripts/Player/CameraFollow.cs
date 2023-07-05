using Cinemachine;
using UnityEngine;
using System.Collections;
public class CameraFollow : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCam;
    private CinemachineTransposer transposer;
    private void Awake()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        transposer = virtualCam.GetCinemachineComponent<CinemachineTransposer>();
    }

    public void SetPosition(float offset)
    {     
        Vector3 cameraOffset = transposer.m_FollowOffset;
        cameraOffset.y = offset;
        transposer.m_FollowOffset = cameraOffset;     
    }
}
