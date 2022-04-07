using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CarMovement : MonoBehaviour
{
    public Transform car;
    public float turningSpeed;
    public float forwardSpeed;

    public long timeBeforeInputReset;
    public bool mac;

    private long lastTime;

    private bool right;
    private bool left;
    private bool forward;
    private bool backwards;

    private void Start()
    {
        lastTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            forward = true;
        } 
        else if (Input.GetKey("w"))
        {
            forward = true;
        }
        else
        {
            forward = false;
        }
        
        if (Input.GetKeyDown("s"))
        {
            backwards = true;
        } 
        else if (Input.GetKey("s"))
        {
            backwards = true;
        }
        else
        {
            backwards = false;
        }
        
        if (Input.GetKeyDown("a"))
        {
            left = true;
        } 
        else if (Input.GetKey("a"))
        {
            left = true;
        }
        else
        {
            left = false;
        }
        
        if (Input.GetKeyDown("d"))
        {
            right = true;
        } 
        else if (Input.GetKey("d"))
        {
            right = true;
        }
        else
        {
            right = false;
        }

    }

    private void FixedUpdate()
    {
        Vector3 carRotation = car.eulerAngles;
        Vector3 carPosition = car.position;

        if (right)
        {
            // turn right
            car.eulerAngles = new Vector3(carRotation.x, carRotation.y + turningSpeed, carRotation.z);
            right = false;
        }

        if (left)
        {
            // turn left
            car.eulerAngles = new Vector3(carRotation.x, carRotation.y - turningSpeed, carRotation.z);
            left = false;
        }

        if (forward)
        {
            // move forward
            car.position = carPosition + (car.forward * (0.01f * forwardSpeed));
        }

        if (backwards)
        {
            // move backwards
            car.position = carPosition - (car.forward * (0.01f * forwardSpeed));
        }
        
        // reset all inputs to "fix" unity key stuck problem on mac
        if (mac)
        {
            if (System.DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastTime > timeBeforeInputReset)
            {
                Input.ResetInputAxes();
                lastTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            }
        }
    }

}
