using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChange : MonoBehaviour
{ 
    private int m_CurrentCameraIndex = 0;

    public CinemachineVirtualCamera[] m_Cameras;

	void Update()
    {
        // Q 키를 누르면 왼쪽 카메라를 활성화
        if (Input.GetKeyDown(KeyCode.Q))
            SwitchCamera((m_CurrentCameraIndex - 1 + m_Cameras.Length) % m_Cameras.Length);

        // E 키를 누르면 오른쪽 카메라를 활성화
        if (Input.GetKeyDown(KeyCode.E))
            SwitchCamera((m_CurrentCameraIndex + 1) % m_Cameras.Length);
    }

    void SwitchCamera(int newIndex)
    {
        m_Cameras[m_CurrentCameraIndex].gameObject.SetActive(false);
        m_Cameras[newIndex].gameObject.SetActive(true);

        m_CurrentCameraIndex = newIndex;
    }
}
