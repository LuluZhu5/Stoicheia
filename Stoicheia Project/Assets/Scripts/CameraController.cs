using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    static CinemachineFramingTransposer transposer;

    private void Awake()
    {
        transposer = GetComponent<CinemachineFramingTransposer>();
    }

    public static void MoveUp()
    {
        transposer.m_ScreenX = 0.7f;
    }

    public static void MoveDown()
    {
        transposer.m_ScreenX = 0.3f;
    }
}
