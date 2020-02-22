using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaledSpace : MonoBehaviour
{
    [SerializeField] private Transform cameraReal;
    [SerializeField] private Transform cameraScaled;

    [SerializeField] private Vector3 traveledPosition;

    [SerializeField] private float scale = 3000;

    [SerializeField] private float spawnRadius = 1;
    [SerializeField] private int scaledLayer = 13;

    [SerializeField] private List<GameObject> spawned = new List<GameObject>();

    void Update()
    {
        cameraScaled.localPosition = (cameraReal.position + traveledPosition) / scale;
        cameraScaled.rotation = cameraReal.rotation;

        SpawnCheck();
    }

    public void FloatingOriginUpdate(Vector3 delta)
    {
        traveledPosition += delta;
        transform.position += delta;
    }

    public void SpawnCheck()
    {
        Collider[] colls = Physics.OverlapSphere(cameraScaled.position, spawnRadius);
        
        foreach(Collider c in colls)
        {
            if(spawned.Contains(c.gameObject) == false)
            {
                spawned.Add(c.gameObject);
                c.transform.localPosition =  (c.transform.localPosition - cameraScaled.localPosition) * scale;
                c.transform.parent = null;
                c.transform.localScale *= scale;
                c.gameObject.layer = 0;
            }
        }
    }
    
}
