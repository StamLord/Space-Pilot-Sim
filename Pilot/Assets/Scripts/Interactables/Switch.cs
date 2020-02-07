using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IInteractable
{
    [SerializeField] private bool state;

    public delegate void StateChangeDelegate(bool state);
    public event StateChangeDelegate OnStateChange;

    public void Interact()
    {
        state = !state;
        if(OnStateChange != null) OnStateChange(state);
    }
}