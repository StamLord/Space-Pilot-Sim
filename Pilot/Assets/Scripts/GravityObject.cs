﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] protected new Rigidbody rigidbody;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float intensity;
    [SerializeField] protected bool useMass = true;

    public Vector3 Direction {get{return direction;}}
    public float Intensity {get{return intensity;}}

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
        
        SetInterior(area.Interior);
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

    private void UpdateInterior()
    {
        if(GetAreaContacts() < 1)
            SetInterior(null);
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
        UpdateInterior();
        ApplyGravity();
    }

    void ApplyGravity()
    {
        Vector3 gravity = direction * intensity;
        if(useMass)
            gravity *= rigidbody.mass;
    
        rigidbody.AddForce(gravity);
    }

}
