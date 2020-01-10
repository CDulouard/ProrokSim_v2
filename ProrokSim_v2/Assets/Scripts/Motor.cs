using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Motor : MonoBehaviour
{
    public HingeJoint theJoint;
    public float targetPos;
    public float kp;
    public float ki;
    public float kd;
    private float _sumError;
    private float _lastError;
    private const int FrameRate = 60; // Pour que Time.deltaTime soit a peu pres egal a 1 pour pas pourrir le calcul

    void Start()
    {
        _sumError = 0f;
        _lastError = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var error = targetPos - theJoint.angle;
        _sumError += error;
        
        _lastError = error;

        var targetSpeed = Time.deltaTime * FrameRate * (kp * error + ki * _sumError + kd * (error - _lastError));

        var tmpMotor = theJoint.motor;
        tmpMotor.targetVelocity = targetSpeed;
        theJoint.motor = tmpMotor;
        Debug.Log(theJoint.angle);
    }
}