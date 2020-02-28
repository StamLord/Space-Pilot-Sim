using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererPool : MonoBehaviour
{
    [SerializeField] private LineRenderer prefab;
    [SerializeField] private List<LineRenderer> pooled = new List<LineRenderer>();
    [SerializeField] private int spawnAmount = 10;

    public static LineRendererPool instance;
    void Awake()
    {
        if(instance == null)   
            instance = this;
        else
            Debug.LogWarning("More than 1 instance of LineRendererPool exists!");
    }

    void Start()
    {
        FillPool(spawnAmount);
    }
    
    void FillPool(int amount)
    {
        for(int i = 0; i< amount; i++)
        {
            LineRenderer lr = Instantiate(prefab, transform);
            lr.gameObject.SetActive(false);
            pooled.Add(lr);
        }
    }

    public LineRenderer GetLineFromPool()
    {
        foreach(LineRenderer lr in pooled)
        {
            if(lr.gameObject.activeSelf == false)
            {
                lr.gameObject.SetActive(true);
                return lr;
            }
        }
        
        // Add one and return it
        LineRenderer newLr = Instantiate(prefab, transform);
        pooled.Add(newLr);
        return newLr;
    }

    public void PoolLine(LineRenderer lr)
    {
        if(pooled.Contains(lr))
        {
            lr.gameObject.SetActive(false);
        }
    }
}
