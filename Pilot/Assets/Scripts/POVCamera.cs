using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class POVCamera : MonoBehaviour
{
    public PilotInterface pi;
    public CinemachineVirtualCamera cam;
    private CinemachinePOV pov;

    public float[] freeVertical = {-65, 20};
    public float[] freeHorizontal = {-60, 60};

    public float freeVSpeed = 300;
    public float freeHSpeed = 300;

    public float[] lockedVertical = {-5, 0};
    public float[] lockedHorizontal = {-10, 10};

    public float lockedVSpeed = 100;
    public float lockedHSpeed = 100;

    void Awake()
    {
        if(pi != null)
            pi.OnMousePiloting += LockMode;
            
        if(cam != null)
            pov = cam.GetCinemachineComponent<CinemachinePOV>();
    }

    void LockMode(bool state)
    {
        if(state) // Locked State
        {
            pov.m_VerticalAxis.m_MinValue = lockedVertical[0];
            pov.m_VerticalAxis.m_MaxValue = lockedVertical[1];

            pov.m_HorizontalAxis.m_MinValue = lockedHorizontal[0];
            pov.m_HorizontalAxis.m_MaxValue = lockedHorizontal[1];

            pov.m_VerticalAxis.m_MaxSpeed = lockedVSpeed;
            pov.m_HorizontalAxis.m_MaxSpeed = lockedHSpeed;
        } 
        else // Free Look State
        {
            pov.m_VerticalAxis.m_MinValue = freeVertical[0];
            pov.m_VerticalAxis.m_MaxValue = freeVertical[1];

            pov.m_HorizontalAxis.m_MinValue = freeHorizontal[0];
            pov.m_HorizontalAxis.m_MaxValue = freeHorizontal[1];

            pov.m_VerticalAxis.m_MaxSpeed = freeVSpeed;
            pov.m_HorizontalAxis.m_MaxSpeed = freeHSpeed;
        }
    }
}
