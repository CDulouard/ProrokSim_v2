using System.Collections;
using System.Collections.Generic;
using ConsoleApplication1;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public string ipServ;
    public int portServ;
    public UdpSocket server; 
    
    private Dictionary<string, Servo> _dServo = new Dictionary<string, Servo>();
    private Dictionary<string, DCMotor> _dDCMotors = new Dictionary<string, DCMotor>();

    public List<DCMotor> lDCMotors;
    public List<Servo> lServo;
    


    private Dictionary<string, float> cServo = new Dictionary<string, float>()
    {
        {"booby", 50f}, {"lololol", 45f}, {"machin", 32f}
    };
    private Dictionary<string, float> cDCMotors = new Dictionary<string, float>()
    {
        {"WLF", 300}, {"WRF", 300}, {"blop", 233}
    };

    // Start is called before the first frame update
    void Start()
    {
        Init();
        //InitServoFromDic(Dictionary<string, float> dServo, List<Servo> lServo)
        server = new UdpSocket();
        server.Start(ipServ, portServ, "test");
        
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.StartMotor();
        }

        foreach (var servo in lServo)
        {
            servo.StartMotor();
        }
        
        //Recupérer les valeurs de nom et pos des moteurs pour créer les dictionnaires
        
    }
    
    // Update is called once per frame
    void Update()
    {    
        RefreshServoPos(cServo);
        RefreshDCMotorSpeed(cDCMotors);
        
        RefreshServoMotors();
        RefreshDCMotors();


    }

    //Initialize the motors 
    private void Init()
    {
        foreach (var dcMotor in lDCMotors)
        {
            _dDCMotors[dcMotor.name] = dcMotor;
        }

        foreach (var servo in lServo)
        {
            _dServo[servo.name] = servo;
        }
        
    }
    

    //Fonction prenant en argument un dictionnaire contenant les noms et target pos des servo moteurs, et retournant la liste de servo moteurs avec les valeurs modifiées
    public void RefreshServoPos(Dictionary<string, float> commandServo)
    {
        foreach (var command in commandServo)
        {
            if (IfExistsServo(command.Key))
            {
                _dServo[command.Key].SetTargetPos(command.Value);

            }
        }
    }

    public void RefreshDCMotorSpeed(Dictionary<string, float> commandDCMotor)
    {
        foreach (var command in commandDCMotor)
        {
            if (IfExistsDCMotor(command.Key))
            {
                _dDCMotors[command.Key].SetTargetSpeed(command.Value);
            }
        }
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

    //call refreshMotor for every Servo
    private void RefreshServoMotors()
    {
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.RefreshMotor();
        }
    }

    //call refreshMotor for every DCMotors
    private void RefreshDCMotors()
    {
        foreach (var servo in lServo)
        {
            servo.RefreshMotor();
        }
    }
    
    //Make sure the key exists
    public bool IfExistsServo( string key)
    {
        foreach (var dicKey in _dServo.Keys)
        {
            if (dicKey==key)
            {
                return true;
            }
        }

        return false;
    }

    public bool IfExistsDCMotor(string key)
    {
        foreach (var dicKey in _dDCMotors.Keys)
        {
            if (dicKey==key)
            {
                return true;
            }
        }

        return false;
    }
}