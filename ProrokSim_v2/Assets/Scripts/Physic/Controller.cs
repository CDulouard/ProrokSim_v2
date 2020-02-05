using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public List<DCMotor> lDCMotors;
    public List<Servo> lServo;

    //public Dictionary<DCMotor, int> dDCMotors;

    //public Dictionary<Servo, int> dServo;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.StartMotor();
        }

        foreach (var servo in lServo)
        {
            servo.StartMotor();
            Debug.Log(servo.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.RefreshMotor();
            //Debug.Log(dcMotor.GetSpeed());
        }

        foreach (var servo in lServo)
        {
            servo.RefreshMotor();
            Debug.Log(servo.name);
            //Debug.Log(servo.GetAngle());
        }
        
    }
}
