using UnityEngine;

public class Targeting : Component
{
    public PilotInterface pi;
    [SerializeField] private Transform _target;
    public Transform target { get{ return _target; }}
    [SerializeField] private bool isLocked;

    void Awake()
    {
        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnLockTarget += Lock;
        }
    }

    void Update()
    {
        if(isLocked == false)
        {
            FindTarget();
        }
    }

    void FindTarget()
    {
        if(functional == false) return;
        // Send Raycast forward
        // If Targetable component / Tag set as target
    }

    void Lock()
    {
        isLocked = !isLocked;
    }

}