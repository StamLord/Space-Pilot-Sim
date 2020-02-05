using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCollector : Component
{
    public PilotInterface pi;
    public ShipStorage storage;
    
    public bool isActive;

    public Transform pullOrigin;
    public float scaleFilter = .5f;
    public float pullRadius = 5f;
    public float pullForce = .1f;

    public float gatherRadius = 2f;

    void Awake()
    {
        if(storage == null)
            storage = GetComponent<ShipStorage>();

        if(pi == null)
            pi = GetComponent<PilotInterface>();

        if(pi != null)
        {
            pi.OnOreMagnet += SetActive;
        }
    }

    void SetActive()
    {
        isActive = !isActive;
    }

    void FixedUpdate()
    {
        if(isActive == false || functional == false)
            return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);
        
        
        foreach(Collider c in colliders)
        {   
            Metal m = c.GetComponent<Metal>();
            float scale = c.transform.localScale.x;
            
            if(m && scale <= scaleFilter)
            {
                Vector3 direction = (pullOrigin.position - c.transform.position).normalized;
                float distance = Vector3.Distance(pullOrigin.position, c.transform.position);
                if(distance <= gatherRadius)
                {
                    if(storage.AddMetal(m.metalContent)) Destroy(c.gameObject);
                }
                else
                    c.attachedRigidbody.AddForce(direction * pullForce * distance, ForceMode.Force);
            }
        }

    }

    void OnDrawGizmos()
    {
        if(isActive)
        {            
            Gizmos.DrawWireSphere(pullOrigin.position, pullRadius);
            Gizmos.DrawWireSphere(pullOrigin.position, gatherRadius);
        }
    }
}
