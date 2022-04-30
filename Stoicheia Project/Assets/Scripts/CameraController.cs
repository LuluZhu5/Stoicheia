using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static void Reset(CinemachineVirtualCamera cam)
    {
        CinemachineCameraOffset offset = cam.GetComponent<CinemachineCameraOffset>();
        Vector3 targetPos = new Vector3(0f, 0f, 0f);
        offset.m_Offset = Vector3.Lerp(offset.m_Offset, targetPos, 0.1f);
    }
    public static void LookUp(CinemachineVirtualCamera cam)
    {
        CinemachineCameraOffset offset = cam.GetComponent<CinemachineCameraOffset>();
        Vector3 targetPos = new Vector3(0f, 7.0f, 0f);
        offset.m_Offset = Vector3.Lerp(offset.m_Offset, targetPos, 0.1f);
    }

    public static void LookDown(CinemachineVirtualCamera cam)
    {
        CinemachineCameraOffset offset = cam.GetComponent<CinemachineCameraOffset>();
        Vector3 targetPos = new Vector3(0f, -7.0f, 0f);
        offset.m_Offset = Vector3.Lerp(offset.m_Offset, targetPos, 0.1f);
    }
}
