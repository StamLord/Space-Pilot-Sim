using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] protected new Rigidbody rigidbody;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float intensity;

    private List<GravityArea> areasInContact = new List<GravityArea>();
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }
    
    public void AddContact(GravityArea area)
    {
        if(areasInContact.Contains(area) == false)
            areasInContact.Add(area);
    }

    public void RemoveContact(GravityArea area)
    {
        if(areasInContact.Contains(area))
            areasInContact.Remove(area);
    }

    public int GetAreaContacts()
    {
        return areasInContact.Count;
    }

    public void SetInterior(Transform interior)
    {
        gameObject.layer = (interior)? 10 : 9;
        transform.parent = interior;
    }

    public void SetGravity(Vector3 direction, float intensity)
    {
        this.direction = direction.normalized;
        this.intensity = intensity;
    }

    public void SetVelocity(Vector3 velocity)
    {
        rigidbody.velocity = velocity;
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(direction * intensity * rigidbody.mass);
    }

}
