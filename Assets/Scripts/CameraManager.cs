using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private CinemachineBlendListCamera bLC;
    private ThirdPersonCams thirdPersonCam;
    
    public GameObject vCam1Obj;
    public GameObject vCam2Obj;
    private CinemachineVirtualCameraBase vCam1;
    private CinemachineVirtualCameraBase vCam2;

    public bool isTeacher;

    void Start()
    {
        bLC = GetComponent<CinemachineBlendListCamera>();
        bLC.m_Loop = false;
        thirdPersonCam = FindObjectOfType<ThirdPersonCams>();
        
        vCam1 = vCam1Obj.GetComponent<CinemachineVirtualCameraBase>();
        vCam2 = vCam2Obj.GetComponent<CinemachineVirtualCameraBase>();
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        LookTeacher();
        yield return new WaitForSeconds(3);
        LookPlayer();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.O))
    //     {
    //         LookTeacher();
    //     }
    //
    //     if (Input.GetKeyDown(KeyCode.P))
    //     {
    //         LookPlayer();;
    //     }
    // }

    public void LookTeacher()
    {
        thirdPersonCam.CursorOn(true);
        isTeacher = true;
        Debug.Log("LookTeacher");
        vCam1Obj.transform.SetParent(transform);
        vCam2Obj.transform.SetParent(transform);


        bLC.m_Instructions[0].m_VirtualCamera = vCam1;
        bLC.m_Instructions[1].m_VirtualCamera = vCam2;
        bLC.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        bLC.m_Instructions[1].m_Blend.m_Time = 0.5f;

        bLC.m_Instructions[0].m_Hold = 0f;
    }

    public void LookPlayer()
    {
        thirdPersonCam.CursorOn(false);
        Debug.Log("LookPlayer");
        isTeacher = false;
        vCam1Obj.transform.SetParent(transform);
        vCam2Obj.transform.SetParent(transform);

        bLC.m_Instructions[0].m_VirtualCamera = vCam2;
        bLC.m_Instructions[1].m_VirtualCamera = vCam1;
        bLC.m_Instructions[1].m_Blend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        bLC.m_Instructions[1].m_Blend.m_Time = 2.0f;

        bLC.m_Instructions[0].m_Hold = 1.0f;
    }
}
