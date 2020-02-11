using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public static float sensivityX;
    public static float sensivityY;
    public static float scrollSensivity;
    public static float distance;
    public Transform target;
    public GameObject cameraObject;

    private Transform _cameraTransform;

    private float _currentX;
    private float _currentY;


    private void Start()
    {
        _cameraTransform = cameraObject.transform;
    }

    private void LateUpdate()
    {
        var position = target.position;
        var direction = new Vector3(0, 0, -distance);
        var rotation = Quaternion.Euler(_currentY, _currentX, 0);
        _cameraTransform.position = position + rotation * direction;
        _cameraTransform.LookAt(position);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _currentX += Input.GetAxis("Mouse X") * sensivityX;
            _currentY += -Input.GetAxis("Mouse Y") * sensivityY;
        }

        distance -= Input.mouseScrollDelta.y * scrollSensivity;
        
    }
}