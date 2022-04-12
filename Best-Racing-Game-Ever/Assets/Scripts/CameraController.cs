using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform camera;
    public Transform car;

    public Vector2 initialCameraCarDistance;
    
    private float distance;
    private float initX;
    private float initY;

    private float initialDifference;
    
    private float oldCameraYPosition;
    private float oldCameraXAngle;
    
    private bool usePlayerInput;
    private float xDelta;
    private float yDelta;

    public float minXRotation = 0f;
    public float maxXRotation = 90f;

    private void Start()
    {
        Vector3 cameraPosition = camera.position;
        Vector3 carPosition = car.position;
        distance = Mathf.Sqrt(initialCameraCarDistance.x * initialCameraCarDistance.x +
                              initialCameraCarDistance.y * initialCameraCarDistance.y);
        initX = initialCameraCarDistance.x;
        initY = initialCameraCarDistance.y;
        initialDifference = 1 - (cameraPosition.y - carPosition.y);
        oldCameraYPosition = cameraPosition.y;
        oldCameraXAngle = camera.eulerAngles.x;
    }

    // Update is called once per frame
    // LateUpdate -> all physics updates have already been done for this frame
    void LateUpdate()
    {
        // if the player rotates the camera dont! interfere
        if (usePlayerInput)
        {
            // rework
            // rotation and position pre-allocation for less "cpu work"
            Vector3 cameraRotation = camera.eulerAngles;
            Vector3 cameraPosition = camera.position;
            Vector3 carPosition = car.position;
            
            // calculate the new angles (clamp x angle between 0 and 90)
            float newYRotation = (cameraRotation.y - yDelta);
            float angleYRadians = newYRotation * (Mathf.PI / 180);
            
            float newXRotation = (cameraRotation.x - xDelta);
            if (newXRotation >= maxXRotation)
            {
                newXRotation = maxXRotation;
            } else if (newXRotation <= minXRotation)
            {
                newXRotation = minXRotation;
            }
            float angleXRadians = newXRotation * (Mathf.PI / 180);

            // calculate the percentage for y Dist / xz Dist
            // THIS IS HOW IT SHOULD BE
            float yPercentage = Mathf.Sin(angleXRadians);
            float xzPercentage = Mathf.Cos(angleXRadians);
            
            //float xzPercentage = Mathf.Sin(angleXRadians);
            //float yPercentage = Mathf.Cos(angleXRadians);
            
            // position calculations for camera left right movement
            float xOffset = -Mathf.Sin(angleYRadians) * (distance * xzPercentage);
            float zOffset = -Mathf.Cos(angleYRadians) * (distance * xzPercentage);

            // position calculations for camera up down movement
            // THIS IS HOW IT SHOULD BE
            float yOffset = yPercentage * distance;

            Vector3 newCameraPosition = new Vector3(carPosition.x + xOffset, carPosition.y + yOffset, carPosition.z + zOffset);
            camera.position = newCameraPosition;

            Vector3 newCameraAngles = new Vector3(newXRotation, newYRotation, cameraRotation.z);
            camera.eulerAngles = newCameraAngles;
            
        }
        else
        {
            //Debug.Log("FollowMode");
            // car position and rotation in Radians (we need it a view times)
            Vector3 carRotation = car.eulerAngles;
            Vector3 carPosition = car.position;
        
            // camera position and rotation
            Vector3 cameraRotation = camera.eulerAngles;
            Vector3 cameraPosition = camera.position;

            // positioning
            // this is fucked up but i actually get angles between 0 and 360 (Rotation in Quaternions is wierd)
            // code so the camera follows the cars rotation
            float angleRadians = carRotation.y * (Mathf.PI / 180);
            float xOffset = Mathf.Sin(angleRadians) * initX;
            float zOffset = Mathf.Cos(angleRadians) * initX;
            Vector3 newCameraPosition = new Vector3(carPosition.x + xOffset, carPosition.y + oldCameraYPosition, carPosition.z + zOffset);
            //Debug.Log("yRot: " + angleRadians + " sin: " + xOffset + " cos: " + zOffset);
        
            camera.position = newCameraPosition;
        
            // rotation THIS WORKS i never work with Quaternions ever again...
            camera.eulerAngles = new Vector3(oldCameraXAngle, carRotation.y, cameraRotation.z);
        }

    }

    private float GetAproxY(float angleX)
    {
        float value;
        if (angleX <= 15.0f)
        {
            value = (angleX / 15) - initialDifference;
            if (value < 0)
            {
                value = 0;
                return value;
            }
        }

        return (1 - initialDifference) + ((distance - initY) * ((angleX - 15) / 75));
    }

    public void SetPlayerInput(bool use)
    {
        usePlayerInput = use;
    }

    public void UpdateXDelta(float delta)
    {
        xDelta = delta;
    }
    
    public void UpdateYDelta(float delta)
    {
        yDelta = delta;
    }
    
}
