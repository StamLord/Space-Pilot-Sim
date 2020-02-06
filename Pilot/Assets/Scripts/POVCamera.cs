using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class POVCamera : MonoBehaviour
{
    public PilotInterface pi;
    public CinemachineVirtualCamera freeCam;
    public CinemachineVirtualCamera lockedCam;
    
    private CinemachinePOV freePov;
    private CinemachinePOV lockedPov;

    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnMousePiloting += LockMode;
            pi.OnStartPiloting += StartPiloting;
            pi.OnStopPiloting += StopPiloting;
        }
            
        if(freeCam != null)
            freePov = freeCam.GetCinemachineComponent<CinemachinePOV>();

        if(lockedCam != null)
            lockedPov = lockedCam.GetCinemachineComponent<CinemachinePOV>();
    }

    void LockMode(bool state)
    {
        if(state) // Locked State
        {
            if(lockedCam)
                lockedCam.m_Priority = 100;
            

            // Wipe old cam values
            if(lockedPov)
            {
                lockedPov.m_VerticalAxis.Value = 0;
                lockedPov.m_HorizontalAxis.Value = 0;
            }
            
        } 
        else // Free Look State
        {
            if(lockedCam)
                lockedCam.m_Priority = 0;

            // Wipe old cam values
            if(freePov)
            {
                freePov.m_VerticalAxis.Value = 0;
                freePov.m_HorizontalAxis.Value = 0;
            }
        }
    }

    void StartPiloting()
    {
        freeCam.m_Priority = 10;
    }

    void StopPiloting()
    {
        freeCam.m_Priority = 0;
        lockedCam.m_Priority = 0;
    }
}
