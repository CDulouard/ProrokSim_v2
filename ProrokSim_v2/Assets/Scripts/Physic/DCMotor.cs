using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable once InconsistentNaming
[System.Serializable]
public class DCMotor
{
    public string name;
    public HingeJoint hJoint;
    public float speed;
    public float torque;
    private float _prevPos;
    private float _angularSpeed;
    private List<float> _lSpeed;
    private int _cmptSpeed;
    private float _speedMean;
    private const int Lenlspeed = 10;

    //This function is called in start method of controller. Activate motor in the hinge joint. Set speed to zero.
    public void StartMotor()
    {
        _prevPos = hJoint.angle;
        hJoint.useMotor = true;
        _lSpeed = new List<float>();
        for (var i = 0; i < Lenlspeed; i++)
        {
            _lSpeed.Add(0f);
        }

        _cmptSpeed = 0;
    }

    //This function create a new variable to store the motor of the hinge joint, set speed and torque value and restore it in the original motor
    //This function will be called multiple times in update method of controller
    public void RefreshMotor()
    {
        var tmpMotor = hJoint.motor;
        tmpMotor.targetVelocity = speed;
        tmpMotor.force = torque;
        hJoint.motor = tmpMotor;
        ComputeSpeed();
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

    public void SetTargetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}