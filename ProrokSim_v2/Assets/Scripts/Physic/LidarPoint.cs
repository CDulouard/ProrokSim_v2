using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LidarPoint
{
    /*    Stores the 3 values of each measure point of the lidar.    */
    public LidarPoint(float hAngle, float vAngle, float distance)
    {
        HorizontalAngle = hAngle;
        VerticalAngle = vAngle;
        Distance = distance;
    }

    public float Distance { get; set; }

    public float VerticalAngle { get; set; }

    public float HorizontalAngle { get; set; }
}
