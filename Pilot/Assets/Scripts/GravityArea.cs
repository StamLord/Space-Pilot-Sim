using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityArea : MonoBehaviour
{
    [SerializeField] private float intensity = 10f;
    private List<GravityObject> objects = new List<GravityObject>();

    void Update()
    {
        foreach(GravityObject o in objects)
        {
            o.SetGravity(-transform.up, intensity);
            o.transform.up = transform.up;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GravityObject go = other.GetComponent<GravityObject>();
        if(go)
            objects.Add(go);
    }

    void OnTriggerExit(Collider other)
    {
        GravityObject go = other.GetComponent<GravityObject>();
        if(go)
            objects.Remove(go);
    }
}
