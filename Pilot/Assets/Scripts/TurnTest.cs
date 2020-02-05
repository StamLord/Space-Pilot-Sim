using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTest : MonoBehaviour
{
    public Rigidbody target;
    public float minAngleMargin = 0.5f;
    public float xAngle;
    public float yAngle;
    public float zAngle;
    public bool turn;
    public float turnForce = 10f;
    
    void OnTriggerEnter(Collider other)
    {
        PilotInterface pi = other.attachedRigidbody.transform.GetComponent<PilotInterface>();
        Debug.Log(pi);
        if(pi)
        {
            pi.HiJack(0f);
            if(target == null)
                target = other.attachedRigidbody;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(target == other.attachedRigidbody)
            target = null;
    }

    void FixedUpdate()
    {
        if(target == null)
            return;

        xAngle = Vector3.SignedAngle(target.transform.up, Vector3.up, Vector3.right);
        yAngle = Vector3.SignedAngle(target.transform.right, Vector3.right, Vector3.up);
        zAngle = Vector3.SignedAngle(target.transform.up, Vector3.up, Vector3.forward);

        if(turn)
        {
            Vector3 torque = Vector3.zero;
            if(Mathf.Abs(xAngle) >= minAngleMargin) torque += turnForce * target.transform.right * xAngle;
            if(Mathf.Abs(yAngle) >= minAngleMargin) torque += turnForce * target.transform.up * yAngle;
            if(Mathf.Abs(zAngle) >= minAngleMargin)  torque += turnForce * target.transform.forward * zAngle;

            target.AddTorque(torque, ForceMode.Impulse);
        }

    }
}
