using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public List<DCMotor> lDCMotors;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var dcMotor in lDCMotors)
        {
             dcMotor.StartMotor();   
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var dcMotor in lDCMotors)
        {
            dcMotor.RefreshMotor();
            Debug.Log(dcMotor.GetSpeed());
        }    
        
        
    }
}
