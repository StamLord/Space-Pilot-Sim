using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, IInteractable
{
    public delegate void PressDelegate();
    public event PressDelegate OnPress;

    public void Interact()
    {
        if(OnPress != null) OnPress();
    }
}
