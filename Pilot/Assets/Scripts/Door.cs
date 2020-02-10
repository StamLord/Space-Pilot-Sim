using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 closedPosition;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private float speed = 0.01f;
    [SerializeField] private bool isOpen;
    [SerializeField] [Range(0,1)] private float percentage = 0;
    [SerializeField] private Switch[] switches;
    [SerializeField] private Button[] buttons;

    void Awake()
    {
        foreach(Switch s in switches)  
            s.OnStateChange += SetOpen;
        foreach(Button b in buttons)  
            b.OnPress += Activate;
    }

    void Update()
    {
        UpdatePercentage();
    }

    void Activate()
    {
        isOpen = !isOpen;
    }

    void SetOpen(bool state)
    {
        isOpen = state;
    }

    void UpdatePercentage()
    {
        float newPercent = percentage;
        if(isOpen && percentage < 1)
            newPercent = Mathf.Clamp(percentage + speed * Time.deltaTime, 0, 1);
        else if(isOpen == false && percentage > 0)
            newPercent = Mathf.Clamp(percentage - speed * Time.deltaTime, 0, 1);

        if(newPercent != percentage)
        {
            percentage = newPercent;
            UpdatePosition();
            UpdateRotation();
        }
    }

    void UpdatePosition()
    {
        transform.localPosition = Vector3.Lerp(closedPosition, openPosition, percentage);
    }

    void UpdateRotation()
    {
        transform.localRotation = Quaternion.Euler(Vector3.Lerp(closedRotation, openRotation, percentage));
    }
}
