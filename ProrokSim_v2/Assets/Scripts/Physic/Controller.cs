using System.Collections.Generic;
using Server;
using UnityEngine;

namespace Physic
{
    public class Controller : MonoBehaviour
    {
        public string ipServ;
        public int portServ;
        public UdpSocket server; 
    
        private Dictionary<string, Servo> _dServo = new Dictionary<string, Servo>();
        private Dictionary<string, DCMotor> _dDCMotors = new Dictionary<string, DCMotor>();
    
        private Dictionary<string, float> dataServos = new Dictionary<string, float>();
        private Dictionary<string, float> dataDCMotors = new Dictionary<string, float>();

        public List<DCMotor> lDCMotors;
        public List<Servo> lServo;

        private Dictionary<string, float> cServo = new Dictionary<string, float>();

        private Dictionary<string, float> cDCMotors = new Dictionary<string, float>();

        // Start is called before the first frame update
        void Start()
        {
            Init();
            //InitServoFromDic(Dictionary<string, float> dServo, List<Servo> lServo)
            server = new UdpSocket();
            server.SetController(this);
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

            RefreshServoPos();
            RefreshDCMotorSpeed();
        
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
        public void RefreshServoPos()
        {
            foreach (var servo in cServo)
            {
                if (IfExistsServo(servo.Key))
                {
                    _dServo[servo.Key].SetTargetPos(servo.Value);
                }
            }
        }

        public void RefreshDCMotorSpeed()
        {
            foreach (var servo in cDCMotors)
            {
                if (IfExistsDCMotor(servo.Key))
                {
                    _dDCMotors[servo.Key].SetTargetSpeed(servo.Value);
                }
            }
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

        public Dictionary<string, float> GetServosData()

        {
            Dictionary<string, float> dataServo = new Dictionary<string, float>();
            foreach (var servo in _dServo)
            {
                dataServo[servo.Key] = servo.Value.targetPos;
            }
            return dataServo;
        }

        public Dictionary<string, float> GetDCMotorData()
        {
            Dictionary<string, float> dataDCMotors = new Dictionary<string, float>();
            foreach (var motor in _dDCMotors)
            {
                dataDCMotors[motor.Key] = motor.Value.speed;
            }
            return dataDCMotors;
        }

        public void SetServoPos(Dictionary<string, float> ServoTargetPos)
        {
            foreach (var servo in ServoTargetPos)
            {
                _dServo[servo.Key].targetPos = servo.Value;
            }
        }

        public void SetDCMotorsSpeed(Dictionary<string, float> DCTargetSpeed)
        {
            foreach (var motor in DCTargetSpeed)
            {
                _dDCMotors[motor.Key].speed = motor.Value;
            }
        }
    }
}