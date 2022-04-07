using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform camera;
    public Transform car;
    
    public float cameraCarDistance;

    private bool usePlayerInput;
    private float yDelta;

    // Update is called once per frame
    // LateUpdate -> all physics updates have already been done for this frame
    void LateUpdate()
    {
        // if the player rotates the camera dont! interfere
        if (usePlayerInput)
        {
            //Debug.Log("MouseInput");
            // we theoretically only need the camera rotation and position here
            Vector3 cameraRotation = camera.eulerAngles;
            Vector3 cameraPosition = camera.position;
            
            // we might also need this...
            Vector3 carPosition = car.position;
            
            // position
            float newYRotation = (cameraRotation.y - yDelta);
            float angleRadians = newYRotation * (Mathf.PI / 180);
            float xOffset = -Mathf.Sin(angleRadians) * cameraCarDistance;
            float zOffset = -Mathf.Cos(angleRadians) * cameraCarDistance;

            Vector3 newCameraPosition = new Vector3(carPosition.x + xOffset, cameraPosition.y, carPosition.z + zOffset);
            camera.position = newCameraPosition;
            
            // rotation
            camera.eulerAngles = new Vector3(cameraRotation.x, newYRotation, cameraRotation.z);
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
            float xOffset = -Mathf.Sin(angleRadians) * cameraCarDistance;
            float zOffset = -Mathf.Cos(angleRadians) * cameraCarDistance;
            Vector3 newCameraPosition = new Vector3(carPosition.x + xOffset, cameraPosition.y, carPosition.z + zOffset);
            //Debug.Log("yRot: " + angleRadians + " sin: " + xOffset + " cos: " + zOffset);
        
            camera.position = newCameraPosition;
        
            // rotation THIS WORKS i never work with Quaternions ever again...
            camera.eulerAngles = new Vector3(cameraRotation.x, carRotation.y, cameraRotation.z);
        }

    }

    public void SetPlayerInput(bool use)
    {
        usePlayerInput = use;
    }
    
    public void UpdateYDelta(float delta)
    {
        yDelta = delta;
    }
    
}
