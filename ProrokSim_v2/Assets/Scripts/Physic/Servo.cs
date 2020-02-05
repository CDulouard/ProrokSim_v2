using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Servo
{
    public string name;

    public HingeJoint hJoint;

    //Pilotage moteur
    public float targetPos;
    public float torque;

    //Limites
    public Vector3 offsetHoming;
    public float minAngle;
    public float maxAngle;


    //Correction
    public float kp;
    public float ki;
    public float kd;

    //Private variable for correction
    private float _sumError;
    private float _lastError;

    private float _prevPos;
    private float _angularSpeed;
    private List<float> _lSpeed;
    private int _cmptSpeed;
    private float _speedMean;
    private const int Lenlspeed = 10;
    
    //Emplacement du fichier de donnée
    //private const string fileName = @"D:\unity\ProrokRoboticsInc\CLOVIS_MINI_REMOTE-master\Test Joint\Assets\Datas\dataFile.txt";

    private double _t0;
    //This function is called in start method of controller. Activate motor in the hinge joint. Set speed to zero.
    public void StartMotor()
    {
        //Creation du fichier de Donnees
        /*using (StreamWriter _f = File.CreateText(fileName))
        {
            
        }*/
        //DEBUG
        _t0 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        
        
        
        InitLim();

        _sumError = 0f;
        _lastError = 0f;

        _prevPos = hJoint.angle;
        hJoint.useMotor = true;
        _lSpeed = new List<float>();
        for (var i = 0; i < Lenlspeed; i++)
        {
            _lSpeed.Add(0f);
        }

        _cmptSpeed = 0;
    }

    private void InitLim()
    {
        hJoint.gameObject.SetActive(false);
        hJoint.transform.rotation = Quaternion.Euler(offsetHoming);
        hJoint.gameObject.SetActive(true);
        // ReSharper disable once Unity.InefficientPropertyAccess
        hJoint.transform.rotation = Quaternion.Euler(0, 0, 0);

        //Set limits
        var hJointLim = hJoint.limits;
        hJointLim.min = minAngle;
        hJointLim.max = maxAngle;
        hJoint.limits = hJointLim;
    }

    //This function create a new variable to store the motor of the hinge joint, set speed and torque value and restore it in the original motor
    //This function will be called multiple times in update method of controller
    public void RefreshMotor()
    {
        var tmpMotor = hJoint.motor;
        tmpMotor.targetVelocity = ComputeTargetSpeed();
        
        /*
        using (StreamWriter _f = File.AppendText(fileName))
        {
            _f.WriteLine(((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - _t0) + ";" + (hJoint.angle).ToString().Replace(",","."));
        }*/
        //Debug.Log(hJoint.angle);
        //Debug.Log((DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - _t0);

        //put same data into file

        tmpMotor.force = torque;
        hJoint.motor = tmpMotor;
        ComputeSpeed();
    }

    public float GetAngle()
    {
        return hJoint.angle;
    }

    private float ComputeTargetSpeed()
    {
        var error = targetPos - hJoint.angle;
        _sumError += error;
        
        //hJoint.motor.

        var targetSpeed = kp * error + ki * _sumError * Time.deltaTime + kd * (error - _lastError) / Time.deltaTime;

        _lastError = error;

        return targetSpeed;
    }

    //function we call in Controller
    public float GetSpeed()
    {
        return _angularSpeed;
    }

    //This function computes the value of angular speed
    private void ComputeSpeed()
    {
        var angle = hJoint.angle;
        //get the difference of angle between two frames, divided by time between two frame to get the angular speed
        var tmpSpeed = Mathf.DeltaAngle(_prevPos, angle) / Time.deltaTime;

        //Compute mean of angular speed on length LENLSPEED (can be changed above) 
        if (!tmpSpeed.Equals(0f))
        {
            _lSpeed[_cmptSpeed] = tmpSpeed;
            var sum = 0.0f;
            for (var i = 0; i < Lenlspeed; i++)
            {
                sum += _lSpeed[i];
            }

            //iterate comptspeed which is the index of tmpspeed in list lSpeed
            _cmptSpeed = (_cmptSpeed + 1) % Lenlspeed;
            _angularSpeed = sum / Lenlspeed;
        }

        _prevPos = angle;
    }
}
