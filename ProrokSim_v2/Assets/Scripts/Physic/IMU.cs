using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMU : MonoBehaviour
{
    

    public GameObject Sensor;
    
    private static Dictionary<string, float> _sensorValues; //static variable 

    private static Dictionary<string, float> _comparisonDictionary = new Dictionary<string, float>();
    
    // Start is called before the first frame update
    void Start()
    {
        //getfirst values at the beginning of the simulation
        _sensorValues = RefreshSensorValues();
        _sensorValues.Add("speed", 0);
        _sensorValues.Add("acceleration", 0);
        _sensorValues.Add("rotationSpeed", 0);
        _sensorValues.Add("rotationAcceleration", 0);
        
        //Initialize the dictionary of buffer values to compute speed and acceleration
        _comparisonDictionary.Add("prevX", 0);
        _comparisonDictionary.Add("prevY", 0);
        _comparisonDictionary.Add("prevZ", 0);
        _comparisonDictionary.Add("postX", _sensorValues["X"]);
        _comparisonDictionary.Add("postY", _sensorValues["Y"]);
        _comparisonDictionary.Add("postZ", _sensorValues["Z"]);

        _comparisonDictionary.Add("prevSpeed", 0);
        _comparisonDictionary.Add("postSpeed", 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //update sensor values 
        _sensorValues = RefreshSensorValues();
        
        //buffer position values of previews frame (could make it an array of 10 values to have smoother values)
        _comparisonDictionary["prevX"] = _comparisonDictionary["postX"];
        _comparisonDictionary["prevY"] = _comparisonDictionary["postY"];
        _comparisonDictionary["prevZ"] = _comparisonDictionary["postZ"];
        _comparisonDictionary["postX"] = _sensorValues["X"];
        _comparisonDictionary["postY"] = _sensorValues["Y"];
        _comparisonDictionary["postZ"] = _sensorValues["Z"];

        //compute speed with position of previews frame and actual frame
        _comparisonDictionary["prevSpeed"] = _comparisonDictionary["postSpeed"];
        var Speed = Mathf.Sqrt(Mathf.Pow(_comparisonDictionary["postX"] - _comparisonDictionary["prevX"], 2) +
                               Mathf.Pow(_comparisonDictionary["postY"] - _comparisonDictionary["prevY"], 2) +
                               Mathf.Pow(_comparisonDictionary["postZ"] - _comparisonDictionary["prevZ"], 2)) /
                    Time.deltaTime;
        _comparisonDictionary["postSpeed"] = Speed;
        
        //compute acceleration with previews speed and actual speed
        var Acceleration = (_comparisonDictionary["postSpeed"] - _comparisonDictionary["prevSpeed"]) / Time.deltaTime;
        
        //update sensorvalues for speed and acceleration
        _sensorValues["speed"] = Speed;
        _sensorValues["acceleration"] = Acceleration;


    }

    public static Dictionary<string, float> GetValues()
    {
        return _sensorValues;
    }
    
    private Dictionary<string, float> RefreshSensorValues()
    {
        /*    Stores the new attitude values in _sensorValues    */
        var newValues = new Dictionary<string, float>();
        var sensorTransform = Sensor.transform;
        var sensorTransformPosition = sensorTransform.position;
        sensorTransform.rotation.ToAngleAxis(out var angle, out var axis);
        newValues.Add("roll", angle * axis.z);
        newValues.Add("pitch", angle * axis.x);
        newValues.Add("yaw", angle * axis.y);
        newValues.Add("X", sensorTransformPosition.x);
        newValues.Add("Y", sensorTransformPosition.y);
        newValues.Add("Z", sensorTransformPosition.z);
        return newValues;
    }
}
