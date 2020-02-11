using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMU : MonoBehaviour
{
    

    public GameObject Sensor;
    
    private static Dictionary<string, float> _sensorValues;

    private static Dictionary<string, float> comparisonDictionary = new Dictionary<string, float>();
    
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
        comparisonDictionary.Add("prevX", 0);
        comparisonDictionary.Add("prevY", 0);
        comparisonDictionary.Add("prevZ", 0);
        comparisonDictionary.Add("postX", _sensorValues["X"]);
        comparisonDictionary.Add("postY", _sensorValues["Y"]);
        comparisonDictionary.Add("postZ", _sensorValues["Z"]);

        comparisonDictionary.Add("prevSpeed", 0);
        comparisonDictionary.Add("postSpeed", 0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //update sensor values 
        _sensorValues = RefreshSensorValues();
        
        //buffer position values of previews frame (could make it an array of 10 values to have smoother values)
        comparisonDictionary["prevX"] = comparisonDictionary["postX"];
        comparisonDictionary["prevY"] = comparisonDictionary["postY"];
        comparisonDictionary["prevZ"] = comparisonDictionary["postZ"];
        comparisonDictionary["postX"] = _sensorValues["X"];
        comparisonDictionary["postY"] = _sensorValues["Y"];
        comparisonDictionary["postZ"] = _sensorValues["Z"];

        //compute speed with position of previews frame and actual frame
        comparisonDictionary["prevSpeed"] = comparisonDictionary["postSpeed"];
        var Speed = Mathf.Sqrt(Mathf.Pow(comparisonDictionary["postX"] - comparisonDictionary["prevX"], 2) +
                               Mathf.Pow(comparisonDictionary["postY"] - comparisonDictionary["prevY"], 2) +
                               Mathf.Pow(comparisonDictionary["postZ"] - comparisonDictionary["prevZ"], 2)) /
                    Time.deltaTime;
        comparisonDictionary["postSpeed"] = Speed;
        
        //compute acceleration with previews speed and actual speed
        var Acceleration = (comparisonDictionary["postSpeed"] - comparisonDictionary["prevSpeed"]) / Time.deltaTime;
        
        //update sensorvalues for speed and acceleration
        _sensorValues["speed"] = Speed;
        _sensorValues["acceleration"] = Acceleration;

        Debug.Log(_sensorValues["speed"]);
        Debug.Log(_sensorValues["acceleration"]);
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
