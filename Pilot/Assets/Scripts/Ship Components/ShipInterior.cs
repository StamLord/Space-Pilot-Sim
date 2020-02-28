using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInterior : MonoBehaviour
{
    [SerializeField] private Transform interior;
    public Transform GetInterior { get{return interior;}}
    Vector3 lastPostion;
    Vector3 lastRotation;
    float margin = .01f;

    void Awake()
    {
        if(interior == null)
            interior = new GameObject().transform;
    }

    void Update()
    {
        interior.position = transform.position;
        interior.rotation = transform.rotation;
}
}
