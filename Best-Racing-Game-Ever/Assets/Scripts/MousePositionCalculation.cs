using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePositionCalculation : MonoBehaviour
{
    public CameraController cameraController;
    public long buttonPressOffsetTime;
    public long playerInputDelay;
    public long playerUpdateDelay;

    private bool buttonState = false;
    private long lastButtonTime;
    private long lastPlayerTime;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        lastButtonTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private void Update()
    {
        long delta = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastButtonTime;
        
        // this works kinda
        if (Input.GetKey(KeyCode.Escape) && delta > buttonPressOffsetTime)
        {
            // if you press esc you get out
            if (buttonState)
            {
                Cursor.lockState = CursorLockMode.None;
                buttonState = false;
            }
            
        }
        if (Input.GetMouseButton(0))
        {
            // if you press mouse 1 get in game
            if (!buttonState)
            {
                Cursor.lockState = CursorLockMode.Locked;
                buttonState = true;
            }
        }
        // this works kind end
        
        // get mouse movement
        if (buttonState)
        {
            // left right camera movement
            float xAxis = Input.GetAxis("Mouse X");
            if (xAxis != 0f)
            {
                // mouse x axis turns the camera y axis ;)
                cameraController.UpdateYDelta(xAxis);
                cameraController.SetPlayerInput(true);
                lastPlayerTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            
            // up down camera movement
            float yAxis = Input.GetAxis("Mouse Y");
            if (yAxis != 0)
            {
                // mouse y axis turns the camera x axis ;)
                cameraController.UpdateXDelta(yAxis);
                cameraController.SetPlayerInput(true);
                lastPlayerTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
            
            // check for camera resets
            if (System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastPlayerTime > playerInputDelay)
            {
                // the camera will follow the car automatically
                cameraController.SetPlayerInput(false);
            }
            if (System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastPlayerTime > playerUpdateDelay)
            {
                // set camera rotation to 0
                cameraController.UpdateYDelta(0f);
                cameraController.UpdateXDelta(0f);
            }

        }

        if (delta > buttonPressOffsetTime)
        {
            lastButtonTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastButtonTime;
        }
        
    }
}
