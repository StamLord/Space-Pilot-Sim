using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HyperDriveManager : MonoBehaviour
{
    [SerializeField] private float chargeTime = 5;
    [SerializeField] private ParticleSystem[] particles;
    [SerializeField] private Camera[] cameras;
    [SerializeField] private AnimationCurve fovCurve;
    private float timer;
    private float parTimer;
    private float tranTimer;

    #region  Singleton
    
    public static HyperDriveManager instance;
    void Awake()
    {
        if(instance)
        {
            Debug.LogWarning("More than 1 instance of HypderDriveManager exists!");
            Destroy(this.gameObject);
        } 
        else
        {
            instance = this;
        }
        
    }
    #endregion

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void EnterHyperSpace(GameObject ship)
    {
        DontDestroyOnLoad(ship);
        DontDestroyOnLoad(ship.GetComponent<ShipInterior>().GetInterior.gameObject);
        DontDestroyOnLoad(Camera.main);

        StartCoroutine("CameraEffect");
        StartCoroutine("ParticlesEffect");
    }
    
    public void EnterHyperSpace(GameObject ship, Material transition)
    {
        EnterHyperSpace(ship);
        StartCoroutine("TransitionEffect", transition);
    }

    IEnumerator CameraEffect()
    {
        while(chargeTime > timer)
        {
            for(int i=0;i<cameras.Length;i++)
            {
                cameras[i].fieldOfView = fovCurve.Evaluate(timer/chargeTime) / (i + 1);
            }
            // foreach(Camera cam in cameras)
            //     cam.fieldOfView = fovCurve.Evaluate(timer/chargeTime);

            yield return null;
            timer += Time.deltaTime;
        }

        timer = 0;
        SceneManager.LoadScene("HyperSpace", LoadSceneMode.Single);
    }

    IEnumerator ParticlesEffect()
    {
        foreach(ParticleSystem par in particles)
            par.Play();

        while(chargeTime > parTimer)
        {
            foreach(ParticleSystem par in particles)
                par.startSpeed = 100 /** parTimer / chargeTime + 20*/;

            yield return null;
            parTimer += Time.deltaTime;
        }

        parTimer = 0;
        
    }

    IEnumerator TransitionEffect(Material transition)
    {
        while(chargeTime * 2 > tranTimer)
        {
            Color final = Color.Lerp(Color.black, Color.white, Mathf.PingPong(tranTimer / chargeTime, 1));
            transition.SetColor("_EmissionColor", final);
            
            yield return null;
            tranTimer += Time.deltaTime;
        }

        tranTimer = 0;
        transition.SetColor("_EmissionColor", Color.black);
    }
}
