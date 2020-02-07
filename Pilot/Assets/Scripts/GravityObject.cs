using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected float intensity;

    public void SetGravity(Vector3 direction, float intensity)
    {
        this.direction = direction.normalized;
        this.intensity = intensity;
    }
}
