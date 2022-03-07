using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
    public enum GravityType {TransformDown, TransformCenter}
    [SerializeField] private GravityType gravityType;
    [SerializeField] private Rigidbody rigidParent;
    [SerializeField] private Transform interior;
    [SerializeField] private float intensity = 10f;
    [SerializeField] private List<GravityObject> objects = new List<GravityObject>();

    public Transform Interior {get{return interior;}}

    void FixedUpdate()
    {
        foreach(GravityObject o in objects)
        {
            Debug.DrawLine(transform.position, transform.position + transform.up);

            switch(gravityType)
            {
                case GravityType.TransformDown:
                    o.SetGravity(-transform.up, intensity);
                    break;
                case GravityType.TransformCenter:
                    o.SetGravity((transform.position - o.transform.position).normalized, intensity);
                    break;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GravityObject go = other.GetComponent<GravityObject>();
        if(go)
        {
            go.AddContact(this);
            objects.Add(go);
        }
    }

    void OnTriggerExit(Collider other)
    {
        GravityObject go = other.GetComponent<GravityObject>();
        if(go)
        {
            go.RemoveContact(this);
            objects.Remove(go);
            
            if(go.GetAreaContacts() == 0)
            {
                go.SetGravity(-transform.up, 0f);
                go.SetVelocity(rigidParent.velocity);
            }
        }
    }

    void OnDrawGizmos()
    {
        float arrowLength = .5f;
        float arrowHead = .2f;
        float arrowHeadStart = .3f;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position -transform.up * arrowLength);
        Gizmos.DrawLine(transform.position -transform.up * arrowLength - new Vector3(0,arrowHeadStart,0), transform.position + (-transform.up * arrowLength) + (transform.right * arrowHead));
        Gizmos.DrawLine(transform.position -transform.up * arrowLength - new Vector3(0,arrowHeadStart,0), transform.position + (-transform.up * arrowLength) - (transform.right * arrowHead));
        Gizmos.DrawLine(transform.position -transform.up * arrowLength - new Vector3(0,arrowHeadStart,0), transform.position + (-transform.up * arrowLength) + (transform.forward * arrowHead));
        Gizmos.DrawLine(transform.position -transform.up * arrowLength - new Vector3(0,arrowHeadStart,0), transform.position + (-transform.up * arrowLength) - (transform.forward * arrowHead));
    }
}
