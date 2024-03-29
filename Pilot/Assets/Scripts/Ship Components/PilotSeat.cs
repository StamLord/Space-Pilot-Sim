﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PilotSeat : MonoBehaviour
{
    [SerializeField] private CharacterControls _seated;
    [SerializeField] private PilotInterface pi;

    [SerializeField] private Transform exitPoint;

    void Awake()
    {
        if(pi)
            pi.OnExit += Exit;
    }

    public void Enter(CharacterControls pilot)
    {
        _seated = pilot;
        _seated.enabled = false;
        pi.enabled = true;
        pi.StartPiloting();

        // Play Animation
        
        // Parent
        _seated.ActivateRigidbody(false);
        _seated.transform.SetParent(transform);
        _seated.transform.localPosition = Vector3.zero;
    }

    public void Exit()
    {
        if(_seated)
        {
            _seated.enabled = true;
            pi.StopPiloting();
            pi.enabled = false;

            // Play Animation

            // Unparent
            _seated.ActivateRigidbody(true);
            _seated.transform.SetParent(transform.parent); // Ship interior
            _seated.transform.position = exitPoint.position;
            
            _seated = null;
        }
        
    }
}
