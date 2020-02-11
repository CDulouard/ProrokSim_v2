using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Dictionary<string, float> dServo =   new Dictionary<string, float>()
    {
        {"booby", 5}, {"lololol", 45} 
    };
    public Dictionary<string, float> dDCMotors = new Dictionary<string, float>()
    {
        {"WRF", 200}, {"WLF", 200}, {"WRB", 200}, {"WLB", 200}
    };
    
    public List<DCMotor> lDCMotors;
    public List<Servo> lServo;

    //public Dictionary<DCMotor, int> dDCMotors;

    //public Dictionary<Servo, int> dServo;
    // Start is called before the first frame update
    void Start()
    {
        
        //InitServoFromDic(Dictionary<string, float> dServo, List<Servo> lServo)
        
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
        lServo =  SetServoFromDic(dServo, lServo);
        lDCMotors = SetDCMotorsFromDic(dDCMotors, lDCMotors);
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.RefreshMotor();
            //Debug.Log(dcMotor.GetSpeed());
        }

        foreach (var servo in lServo)
        {
            servo.RefreshMotor();
            //Debug.Log(servo.name);
            //Debug.Log(servo.GetAngle());
        }
    }
    
    public List<Servo> InitServoFromDic(Dictionary<string, float> dServo, List<Servo> lServo)
    {
        //set Target speed or target pos
        //set number of items
        List<Servo> lServoNew = new List<Servo>();

        foreach (var motorName in dServo.Keys)
        {
            Servo tempServo = new Servo();
            tempServo.name = motorName;
            tempServo.targetPos = dServo[motorName];
            lServoNew.Add(tempServo);
        }
        return lServoNew;
    }
    
    //Fonction prenant en argument un dictionnaire contenant les noms et target pos des servo moteurs, et retournant la liste de servo moteurs avec les valeurs modifiées
    public List<Servo> SetServoFromDic(Dictionary<string, float> dServo, List<Servo> lServo)
    {
        foreach (var motorName in dServo.Keys)
        {
            //find Motor in MServo where motorName==Servo.name
            //lServo[Servo] = dServo[Servo.name]
            foreach (var Motor in lServo)
            {
                if (Motor.name == motorName)
                {
                    Motor.targetPos = dServo[motorName];
                }
            }
        }
        return lServo;
    }
    
    //Fonction prenant en argument un dictionnaire contenant les noms et target speed des dcmotors moteurs, et retournant la liste de moteurs avec les valeurs modifiées
    public List<DCMotor> SetDCMotorsFromDic(Dictionary<string, float> dDCMotors, List<DCMotor> lDCMotors)
    {
        foreach (var motorName in dDCMotors.Keys)
        {
            //find Motor in MServo where motorName==Servo.name
            //lServo[Servo] = dServo[Servo.name]
            foreach (var Motor in lDCMotors)
            {
                if (Motor.name == motorName)
                {
                    Motor.speed = dDCMotors[motorName];
                }
            }
        }
        return lDCMotors;
    }
}